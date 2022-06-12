using System;
using UnityEngine;
using Photon.Pun;

public class DoorState : IState
{
	public DoorState(GhostAI ghostAI, GhostInteraction ghostInteraction, GhostInfo ghostInfo, PhotonObjectInteract obj)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.ghostInfo = ghostInfo;
		this.door = obj.GetComponent<Door>();
	}

	public void Enter()
	{
		if (EvidenceController.instance.IsFingerPrintEvidence())
		{
			this.door.SpawnHandPrintEvidence();
		}
		if (!this.door.GetComponent<PhotonView>().IsMine)
		{
			this.door.GetComponent<PhotonView>().RequestOwnership();
		}
		Rigidbody component = this.door.GetComponent<Rigidbody>();
		component.mass = 1f;
		component.isKinematic = false;
		component.AddTorque(new Vector3(0f, UnityEngine.Random.Range(-1f, 1f), 0f), ForceMode.Impulse);
		this.ghostAI.StartCoroutine(this.ghostAI.ResetRigidbody(component, this.door));
		this.ghostInteraction.CreateDoorNoise(this.door.transform.position);
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
	}

	public void Execute()
	{
	}

	public void Exit()
	{
	}

	private GhostAI ghostAI;

	private GhostInteraction ghostInteraction;

	private GhostInfo ghostInfo;

	private Door door;
}


using System;
using UnityEngine;
using Photon.Pun;

public class DoorLockState : IState
{
	public DoorLockState(GhostAI ghostAI, GhostInteraction ghostInteraction, PhotonObjectInteract obj)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.door = obj.GetComponent<Door>();
	}

	public void Enter()
	{
		if (this.door.GetComponent<Door>().type == Key.KeyType.none || this.door.type == Key.KeyType.Car)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (this.door.GetComponent<PhotonObjectInteract>().isGrabbed)
		{
			return;
		}
		if (!this.door.GetComponent<PhotonView>().IsMine)
		{
			this.door.GetComponent<PhotonView>().RequestOwnership();
		}
		if (!this.door.closed)
		{
			Rigidbody component = this.door.GetComponent<Rigidbody>();
			component.mass = 1f;
			component.isKinematic = false;
			component.useGravity = true;
			component.AddTorque(new Vector3(0f, (component.GetComponent<HingeJoint>().limits.min == 0f) ? -1.25f : 1.25f, 0f), ForceMode.VelocityChange);
			this.ghostAI.StartCoroutine(this.ghostAI.ResetRigidbody(component, this.door));
			this.ghostInteraction.CreateDoorNoise(this.door.transform.position);
		}
		this.door.LockDoor();
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

	private Door door;
}


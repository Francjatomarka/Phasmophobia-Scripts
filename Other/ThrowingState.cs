using System;
using UnityEngine;
using Photon.Pun;

public class ThrowingState : IState
{
	public ThrowingState(GhostAI ghostAI, GhostInteraction ghostInteraction, PhotonObjectInteract prop)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.prop = prop;
	}

	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		if (this.prop == null)
		{
			return;
		}
		if (LevelController.instance.currentGhostRoom == LevelController.instance.outsideRoom)
		{
			return;
		}
		if (LevelController.instance.fuseBox.isOn)
		{
			for (int i = 0; i < LevelController.instance.currentGhostRoom.lightSwitches.Count; i++)
			{
				if (LevelController.instance.currentGhostRoom.lightSwitches[i].isOn)
				{
					return;
				}
			}
		}
		if (!this.prop.GetComponent<PhotonView>().IsMine)
		{
			this.prop.GetComponent<PhotonView>().RequestOwnership();
		}
		if (this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Poltergeist)
		{
			this.prop.GetComponent<Rigidbody>().AddForce(new Vector3(UnityEngine.Random.Range(-4f, 4f), UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-2.5f, 2.5f)), ForceMode.Impulse);
		}
		else if (this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Poltergeist)
		{
			this.prop.GetComponent<Rigidbody>().AddForce(new Vector3(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-2.5f, 2.5f), UnityEngine.Random.Range(-3f, 3f)), ForceMode.Impulse);
		}
		else
		{
			this.prop.GetComponent<Rigidbody>().AddForce(new Vector3(UnityEngine.Random.Range(-2.5f, 2.5f), UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-2.5f, 2.5f)), ForceMode.Impulse);
		}
		this.ghostInteraction.CreateThrowingEMF(this.prop.transform.position);
	}

	public void Execute()
	{
	}

	public void Exit()
	{
	}

	private GhostAI ghostAI;

	private GhostInteraction ghostInteraction;

	private PhotonObjectInteract prop;
}


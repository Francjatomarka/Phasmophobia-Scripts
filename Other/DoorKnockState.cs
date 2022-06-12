using System;
using UnityEngine;
using Photon.Pun;

public class DoorKnockState : IState
{
	public DoorKnockState(GhostAI ghostAI, GhostInteraction ghostInteraction)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
	}

	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		if (UnityEngine.Random.Range(0, 5) > 3)
		{
			return;
		}
		if (Vector3.Distance(this.ghostAI.transform.position, SoundController.instance.doorAudioSource.transform.position) > 3f)
		{
			return;
		}
		SoundController.instance.view.RPC("PlayDoorKnockingSound", RpcTarget.All, Array.Empty<object>());
		this.ghostInteraction.CreateInteractionEMF(SoundController.instance.doorAudioSource.transform.position);
	}

	public void Execute()
	{
	}

	public void Exit()
	{
	}

	private GhostAI ghostAI;

	private GhostInteraction ghostInteraction;
}


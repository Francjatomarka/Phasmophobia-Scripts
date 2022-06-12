using System;
using UnityEngine;
using Photon.Pun;

public class LightFlickerState : IState
{
	public LightFlickerState(GhostAI ghostAI, GhostInteraction ghostInteraction)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
	}

	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		if (LevelController.instance.currentGhostRoom.lightSwitches.Count == 0)
		{
			return;
		}
		int index = UnityEngine.Random.Range(0, LevelController.instance.currentGhostRoom.lightSwitches.Count);
		if (LevelController.instance.currentGhostRoom.lightSwitches[index].isOn)
		{
			LevelController.instance.currentGhostRoom.lightSwitches[index].view.RPC("FlickerNetworked", RpcTarget.All, Array.Empty<object>());
		}
		this.ghostInteraction.CreateInteractionEMF(LevelController.instance.currentGhostRoom.lightSwitches[index].transform.position);
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


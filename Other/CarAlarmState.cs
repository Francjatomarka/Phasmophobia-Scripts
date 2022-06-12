using System;
using UnityEngine;
using Photon.Pun;

public class CarAlarmState : IState
{
	public CarAlarmState(GhostAI ghostAI, GhostInteraction ghostInteraction)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
	}

	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		if (LevelController.instance.car != null && UnityEngine.Random.Range(0, 2) == 1)
		{
			if (Vector3.Distance(LevelController.instance.currentGhost.transform.position, LevelController.instance.car.transform.position) > 2f)
			{
				return;
			}
			LevelController.instance.car.view.RPC("TurnAlarmOn", RpcTarget.All, Array.Empty<object>());
			this.ghostInteraction.CreateInteractionEMF(LevelController.instance.car.raycastSpot.position);
		}
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


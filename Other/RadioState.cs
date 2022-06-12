using System;
using UnityEngine;
using Photon.Pun;

public class RadioState : IState
{
	public RadioState(GhostAI ghostAI, GhostInteraction ghostInteraction)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
	}

	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		if (UnityEngine.Random.Range(0, 5) > 1)
		{
			return;
		}
		if (LevelController.instance.radiosInLevel.Length != 0)
		{
			for (int i = 0; i < LevelController.instance.radiosInLevel.Length; i++)
			{
				if (Vector3.Distance(this.ghostAI.transform.position, LevelController.instance.radiosInLevel[i].transform.position) < 2f)
				{
					LevelController.instance.radiosInLevel[i].view.RPC("TurnOn", RpcTarget.All, Array.Empty<object>());
					this.ghostInteraction.CreateInteractionEMF(LevelController.instance.radiosInLevel[i].transform.position);
				}
			}
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


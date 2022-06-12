using System;
using UnityEngine;

public class FuseBoxState : IState
{
	public FuseBoxState(GhostAI ghostAI, GhostInteraction ghostInteraction)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
	}

	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		if (GameController.instance.isTutorial)
		{
			return;
		}
		if (LevelController.instance.fuseBox.isOn)
		{
			if (this.ghostAI.ghostInfo.ghostTraits.ghostType != GhostTraits.Type.Jinn)
			{
				this.ghostInteraction.CreateInteractionEMF(LevelController.instance.fuseBox.transform.position);
				LevelController.instance.fuseBox.TurnOff();
				return;
			}
		}
		else if (UnityEngine.Random.Range(0, 5) == 1)
		{
			this.ghostInteraction.CreateInteractionEMF(LevelController.instance.fuseBox.transform.position);
			LevelController.instance.fuseBox.Use();
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


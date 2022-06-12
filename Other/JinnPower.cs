using System;
using UnityEngine;

public class JinnPower : IState
{
	public JinnPower(GhostAI ghostAI, GhostInteraction ghostInteraction)
	{
		this.ghostInteraction = ghostInteraction;
		this.ghostAI = ghostAI;
	}

	public void Enter()
	{
		if (!LevelController.instance.fuseBox.isOn)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		}
	}

	public void Execute()
	{
		this.timer -= Time.deltaTime;
		if (this.timer < 0f)
		{
			this.timer = 100f;
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			this.ghostAI.JinnPowerDistanceCheck();
			this.ghostInteraction.CreateInteractionEMF(this.ghostAI.raycastPoint.position);
		}
	}

	public void Exit()
	{
	}

	private GhostInteraction ghostInteraction;

	private GhostAI ghostAI;

	private float timer = 5f;
}


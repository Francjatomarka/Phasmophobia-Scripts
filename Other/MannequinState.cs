using System;
using UnityEngine;

public class MannequinState : IState
{
	public MannequinState(GhostAI ghostAI, GhostInteraction ghostInteraction, PhotonObjectInteract obj)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.mannequin = obj.GetComponent<Mannequin>();
	}

	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		if (UnityEngine.Random.Range(0, 3) < 2)
		{
			this.mannequin.Teleport();
		}
		else
		{
			this.mannequin.Rotate();
		}
		this.ghostInteraction.CreateInteractionEMF(this.mannequin.transform.position);
	}

	public void Execute()
	{
	}

	public void Exit()
	{
	}

	private GhostAI ghostAI;

	private GhostInteraction ghostInteraction;

	private Mannequin mannequin;
}


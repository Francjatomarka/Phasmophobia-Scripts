using System;

public class PaintingState : IState
{
	public PaintingState(GhostAI ghostAI, GhostInteraction ghostInteraction, PhotonObjectInteract obj)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.painting = obj.GetComponent<Painting>();
	}

	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		this.painting.KnockOver();
		this.ghostInteraction.CreateInteractionEMF(this.painting.transform.position);
	}

	public void Execute()
	{
	}

	public void Exit()
	{
	}

	private GhostAI ghostAI;

	private GhostInteraction ghostInteraction;

	private Painting painting;
}


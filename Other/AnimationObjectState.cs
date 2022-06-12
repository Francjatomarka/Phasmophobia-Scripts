using System;

public class AnimationObjectState : IState
{
	public AnimationObjectState(GhostAI ghostAI, GhostInteraction ghostInteraction, PhotonObjectInteract obj)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.animation = obj.GetComponent<AnimationObject>();
	}

	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		this.animation.Use();
		this.ghostInteraction.CreateInteractionEMF(this.animation.transform.position);
	}

	public void Execute()
	{
	}

	public void Exit()
	{
	}

	private GhostAI ghostAI;

	private GhostInteraction ghostInteraction;

	private AnimationObject animation;
}


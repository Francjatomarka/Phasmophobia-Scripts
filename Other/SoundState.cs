using System;

public class SoundState : IState
{
	public SoundState(GhostAI ghostAI, GhostInteraction ghostInteraction, PhotonObjectInteract obj)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.sound = obj.GetComponent<Sound>();
	}

	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		this.sound.Use();
		this.ghostInteraction.CreateInteractionEMF(this.sound.transform.position);
	}

	public void Execute()
	{
	}

	public void Exit()
	{
	}

	private GhostAI ghostAI;

	private GhostInteraction ghostInteraction;

	private Sound sound;
}


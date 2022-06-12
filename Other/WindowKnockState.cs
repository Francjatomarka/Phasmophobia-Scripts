using System;

public class WindowKnockState : IState
{
	public WindowKnockState(GhostAI ghostAI, GhostInteraction ghostInteraction, PhotonObjectInteract obj)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.window = obj.GetComponent<Window>();
	}

	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		this.window.PlayKnockingSound();
		if (EvidenceController.instance.IsFingerPrintEvidence())
		{
			this.window.SpawnHandPrintEvidence();
		}
		this.ghostInteraction.CreateInteractionEMF(this.window.transform.position);
	}

	public void Execute()
	{
	}

	public void Exit()
	{
	}

	private GhostAI ghostAI;

	private GhostInteraction ghostInteraction;

	private Window window;
}


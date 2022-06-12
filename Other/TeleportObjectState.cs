using System;

public class TeleportObjectState : IState
{
	public TeleportObjectState(GhostAI ghostAI, GhostInteraction ghostInteraction, PhotonObjectInteract obj)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.teleportObj = obj.GetComponent<TeleportableObject>();
	}

	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		this.teleportObj.Use();
		this.ghostInteraction.CreateInteractionEMF(this.teleportObj.transform.position);
	}

	public void Execute()
	{
	}

	public void Exit()
	{
	}

	private GhostAI ghostAI;

	private GhostInteraction ghostInteraction;

	private TeleportableObject teleportObj;
}


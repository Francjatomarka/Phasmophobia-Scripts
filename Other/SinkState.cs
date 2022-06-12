using System;

public class SinkState : IState
{
	public SinkState(GhostAI ghostAI, GhostInteraction ghostInteraction, PhotonObjectInteract obj)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.sink = obj.GetComponent<Sink>();
	}

	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		this.sink.Use();
		this.sink.SpawnDirtyWater();
		this.ghostInteraction.CreateInteractionEMF(this.sink.transform.position);
	}

	public void Execute()
	{
	}

	public void Exit()
	{
	}

	private GhostAI ghostAI;

	private GhostInteraction ghostInteraction;

	private Sink sink;
}


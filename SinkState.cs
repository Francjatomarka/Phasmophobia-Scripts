using System;

// Token: 0x020000C2 RID: 194
public class SinkState : IState
{
	// Token: 0x06000585 RID: 1413 RVA: 0x00020814 File Offset: 0x0001EA14
	public SinkState(GhostAI ghostAI, GhostInteraction ghostInteraction, PhotonObjectInteract obj)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.sink = obj.GetComponent<Sink>();
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x00020838 File Offset: 0x0001EA38
	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		this.sink.Use();
		this.sink.SpawnDirtyWater();
		this.ghostInteraction.CreateInteractionEMF(this.sink.transform.position);
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Execute()
	{
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Exit()
	{
	}

	// Token: 0x04000535 RID: 1333
	private GhostAI ghostAI;

	// Token: 0x04000536 RID: 1334
	private GhostInteraction ghostInteraction;

	// Token: 0x04000537 RID: 1335
	private Sink sink;
}

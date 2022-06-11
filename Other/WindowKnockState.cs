using System;

// Token: 0x020000C7 RID: 199
public class WindowKnockState : IState
{
	// Token: 0x0600059A RID: 1434 RVA: 0x00020CBE File Offset: 0x0001EEBE
	public WindowKnockState(GhostAI ghostAI, GhostInteraction ghostInteraction, PhotonObjectInteract obj)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.window = obj.GetComponent<Window>();
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x00020CE0 File Offset: 0x0001EEE0
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

	// Token: 0x0600059C RID: 1436 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Execute()
	{
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Exit()
	{
	}

	// Token: 0x04000544 RID: 1348
	private GhostAI ghostAI;

	// Token: 0x04000545 RID: 1349
	private GhostInteraction ghostInteraction;

	// Token: 0x04000546 RID: 1350
	private Window window;
}

using System;

// Token: 0x020000C0 RID: 192
public class PaintingState : IState
{
	// Token: 0x0600057D RID: 1405 RVA: 0x000206E0 File Offset: 0x0001E8E0
	public PaintingState(GhostAI ghostAI, GhostInteraction ghostInteraction, PhotonObjectInteract obj)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.painting = obj.GetComponent<Painting>();
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x00020702 File Offset: 0x0001E902
	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		this.painting.KnockOver();
		this.ghostInteraction.CreateInteractionEMF(this.painting.transform.position);
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Execute()
	{
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Exit()
	{
	}

	// Token: 0x04000530 RID: 1328
	private GhostAI ghostAI;

	// Token: 0x04000531 RID: 1329
	private GhostInteraction ghostInteraction;

	// Token: 0x04000532 RID: 1330
	private Painting painting;
}

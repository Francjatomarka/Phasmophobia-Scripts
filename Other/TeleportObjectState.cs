using System;

// Token: 0x020000C4 RID: 196
public class TeleportObjectState : IState
{
	// Token: 0x0600058D RID: 1421 RVA: 0x000208DC File Offset: 0x0001EADC
	public TeleportObjectState(GhostAI ghostAI, GhostInteraction ghostInteraction, PhotonObjectInteract obj)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.teleportObj = obj.GetComponent<TeleportableObject>();
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x000208FE File Offset: 0x0001EAFE
	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		this.teleportObj.Use();
		this.ghostInteraction.CreateInteractionEMF(this.teleportObj.transform.position);
	}

	// Token: 0x0600058F RID: 1423 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Execute()
	{
	}

	// Token: 0x06000590 RID: 1424 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Exit()
	{
	}

	// Token: 0x0400053B RID: 1339
	private GhostAI ghostAI;

	// Token: 0x0400053C RID: 1340
	private GhostInteraction ghostInteraction;

	// Token: 0x0400053D RID: 1341
	private TeleportableObject teleportObj;
}

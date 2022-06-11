using System;

// Token: 0x020000AD RID: 173
public class AnimationObjectState : IState
{
	// Token: 0x06000529 RID: 1321 RVA: 0x0001C810 File Offset: 0x0001AA10
	public AnimationObjectState(GhostAI ghostAI, GhostInteraction ghostInteraction, PhotonObjectInteract obj)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.animation = obj.GetComponent<AnimationObject>();
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x0001C832 File Offset: 0x0001AA32
	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		this.animation.Use();
		this.ghostInteraction.CreateInteractionEMF(this.animation.transform.position);
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Execute()
	{
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Exit()
	{
	}

	// Token: 0x040004E5 RID: 1253
	private GhostAI ghostAI;

	// Token: 0x040004E6 RID: 1254
	private GhostInteraction ghostInteraction;

	// Token: 0x040004E7 RID: 1255
	private AnimationObject animation;
}

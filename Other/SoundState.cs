using System;

// Token: 0x020000C3 RID: 195
public class SoundState : IState
{
	// Token: 0x06000589 RID: 1417 RVA: 0x00020884 File Offset: 0x0001EA84
	public SoundState(GhostAI ghostAI, GhostInteraction ghostInteraction, PhotonObjectInteract obj)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.sound = obj.GetComponent<Sound>();
	}

	// Token: 0x0600058A RID: 1418 RVA: 0x000208A6 File Offset: 0x0001EAA6
	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		this.sound.Use();
		this.ghostInteraction.CreateInteractionEMF(this.sound.transform.position);
	}

	// Token: 0x0600058B RID: 1419 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Execute()
	{
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Exit()
	{
	}

	// Token: 0x04000538 RID: 1336
	private GhostAI ghostAI;

	// Token: 0x04000539 RID: 1337
	private GhostInteraction ghostInteraction;

	// Token: 0x0400053A RID: 1338
	private Sound sound;
}

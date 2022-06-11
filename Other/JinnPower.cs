using System;
using UnityEngine;

// Token: 0x020000A2 RID: 162
public class JinnPower : IState
{
	// Token: 0x060004C3 RID: 1219 RVA: 0x0001A357 File Offset: 0x00018557
	public JinnPower(GhostAI ghostAI, GhostInteraction ghostInteraction)
	{
		this.ghostInteraction = ghostInteraction;
		this.ghostAI = ghostAI;
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x0001A378 File Offset: 0x00018578
	public void Enter()
	{
		if (!LevelController.instance.fuseBox.isOn)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		}
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x0001A39C File Offset: 0x0001859C
	public void Execute()
	{
		this.timer -= Time.deltaTime;
		if (this.timer < 0f)
		{
			this.timer = 100f;
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			this.ghostAI.JinnPowerDistanceCheck();
			this.ghostInteraction.CreateInteractionEMF(this.ghostAI.raycastPoint.position);
		}
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Exit()
	{
	}

	// Token: 0x0400049C RID: 1180
	private GhostInteraction ghostInteraction;

	// Token: 0x0400049D RID: 1181
	private GhostAI ghostAI;

	// Token: 0x0400049E RID: 1182
	private float timer = 5f;
}

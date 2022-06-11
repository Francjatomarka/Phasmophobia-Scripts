using System;
using UnityEngine;

// Token: 0x020000BF RID: 191
public class MannequinState : IState
{
	// Token: 0x06000579 RID: 1401 RVA: 0x00020664 File Offset: 0x0001E864
	public MannequinState(GhostAI ghostAI, GhostInteraction ghostInteraction, PhotonObjectInteract obj)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.mannequin = obj.GetComponent<Mannequin>();
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x00020688 File Offset: 0x0001E888
	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		if (UnityEngine.Random.Range(0, 3) < 2)
		{
			this.mannequin.Teleport();
		}
		else
		{
			this.mannequin.Rotate();
		}
		this.ghostInteraction.CreateInteractionEMF(this.mannequin.transform.position);
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Execute()
	{
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Exit()
	{
	}

	// Token: 0x0400052D RID: 1325
	private GhostAI ghostAI;

	// Token: 0x0400052E RID: 1326
	private GhostInteraction ghostInteraction;

	// Token: 0x0400052F RID: 1327
	private Mannequin mannequin;
}

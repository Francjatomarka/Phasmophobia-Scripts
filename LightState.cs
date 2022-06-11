using System;
using UnityEngine;

// Token: 0x020000BE RID: 190
public class LightState : IState
{
	// Token: 0x06000575 RID: 1397 RVA: 0x000205C5 File Offset: 0x0001E7C5
	public LightState(GhostAI ghostAI, GhostInteraction ghostInteraction, PhotonObjectInteract obj)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.lightSwitch = obj.GetComponent<LightSwitch>();
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x000205E8 File Offset: 0x0001E7E8
	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		if (this.lightSwitch.isOn)
		{
			this.lightSwitch.UseLight();
		}
		else if (UnityEngine.Random.Range(0, 2) == 1)
		{
			this.lightSwitch.UseLight();
		}
		if (EvidenceController.instance.IsFingerPrintEvidence())
		{
			this.lightSwitch.SpawnHandPrintEvidence();
		}
		this.ghostInteraction.CreateInteractionEMF(this.lightSwitch.transform.position);
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Execute()
	{
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Exit()
	{
	}

	// Token: 0x0400052A RID: 1322
	private GhostAI ghostAI;

	// Token: 0x0400052B RID: 1323
	private GhostInteraction ghostInteraction;

	// Token: 0x0400052C RID: 1324
	private LightSwitch lightSwitch;
}

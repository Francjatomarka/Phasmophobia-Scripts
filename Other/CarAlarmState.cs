using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x020000B0 RID: 176
public class CarAlarmState : IState
{
	// Token: 0x06000535 RID: 1333 RVA: 0x0001CCC0 File Offset: 0x0001AEC0
	public CarAlarmState(GhostAI ghostAI, GhostInteraction ghostInteraction)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x0001CCD8 File Offset: 0x0001AED8
	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		if (LevelController.instance.car != null && UnityEngine.Random.Range(0, 2) == 1)
		{
			if (Vector3.Distance(LevelController.instance.currentGhost.transform.position, LevelController.instance.car.transform.position) > 2f)
			{
				return;
			}
			LevelController.instance.car.view.RPC("TurnAlarmOn", RpcTarget.All, Array.Empty<object>());
			this.ghostInteraction.CreateInteractionEMF(LevelController.instance.car.raycastSpot.position);
		}
	}

	// Token: 0x06000537 RID: 1335 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Execute()
	{
	}

	// Token: 0x06000538 RID: 1336 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Exit()
	{
	}

	// Token: 0x040004F0 RID: 1264
	private GhostAI ghostAI;

	// Token: 0x040004F1 RID: 1265
	private GhostInteraction ghostInteraction;
}

using System;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

// Token: 0x020000BC RID: 188
public class KillPlayerState : IState
{
	// Token: 0x0600056D RID: 1389 RVA: 0x0002036F File Offset: 0x0001E56F
	public KillPlayerState(GhostAI ghostAI, NavMeshAgent agent, PhotonView view)
	{
		this.ghostAI = ghostAI;
		this.agent = agent;
		this.view = view;
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x00020398 File Offset: 0x0001E598
	public void Enter()
	{
		this.ghostAI.ghostAudio.PlayOrStopAppearSource(true);
		if (this.ghostAI.playerToKill.VRIKObj != null)
		{
			DeadZoneController.instance.playerDeathSpot = this.ghostAI.playerToKill.headObject.transform.position;
		}
		else
		{
			DeadZoneController.instance.playerDeathSpot = this.ghostAI.playerToKill.transform.position;
		}
		this.agent.isStopped = true;
		this.ghostAI.ghostAudio.PlaySound(1, false, false);
		this.ghostAI.UnAppear(true);
		this.ghostAI.anim.SetBool("isIdle", true);
		this.ghostAI.anim.SetTrigger("Attack");
		this.ghostAI.playerToKill.StartKillingPlayer();
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x00020478 File Offset: 0x0001E678
	public void Execute()
	{
		this.killTimer -= Time.deltaTime;
		if (this.killTimer < 0f)
		{
			this.ghostAI.Appear(true);
			this.agent.isStopped = false;
			DeadZoneController.instance.SpawnDeathRoom();
			this.killTimer = 100f;
		}
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x000204D0 File Offset: 0x0001E6D0
	public void Exit()
	{
		this.ghostAI.ghostAudio.PlayOrStopAppearSource(false);
		this.ghostAI.playerToKill = null;
	}

	// Token: 0x04000524 RID: 1316
	private GhostAI ghostAI;

	// Token: 0x04000525 RID: 1317
	private NavMeshAgent agent;

	// Token: 0x04000526 RID: 1318
	private PhotonView view;

	// Token: 0x04000527 RID: 1319
	private float killTimer = 4.55f;
}

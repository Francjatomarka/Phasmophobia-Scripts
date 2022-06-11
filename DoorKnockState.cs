using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x020000B1 RID: 177
public class DoorKnockState : IState
{
	// Token: 0x06000539 RID: 1337 RVA: 0x0001CD82 File Offset: 0x0001AF82
	public DoorKnockState(GhostAI ghostAI, GhostInteraction ghostInteraction)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
	}

	// Token: 0x0600053A RID: 1338 RVA: 0x0001CD98 File Offset: 0x0001AF98
	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		if (UnityEngine.Random.Range(0, 5) > 3)
		{
			return;
		}
		if (Vector3.Distance(this.ghostAI.transform.position, SoundController.instance.doorAudioSource.transform.position) > 3f)
		{
			return;
		}
		SoundController.instance.view.RPC("PlayDoorKnockingSound", RpcTarget.All, Array.Empty<object>());
		this.ghostInteraction.CreateInteractionEMF(SoundController.instance.doorAudioSource.transform.position);
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Execute()
	{
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Exit()
	{
	}

	// Token: 0x040004F2 RID: 1266
	private GhostAI ghostAI;

	// Token: 0x040004F3 RID: 1267
	private GhostInteraction ghostInteraction;
}

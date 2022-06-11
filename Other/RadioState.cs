using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x020000C1 RID: 193
public class RadioState : IState
{
	// Token: 0x06000581 RID: 1409 RVA: 0x00020738 File Offset: 0x0001E938
	public RadioState(GhostAI ghostAI, GhostInteraction ghostInteraction)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
	}

	// Token: 0x06000582 RID: 1410 RVA: 0x00020750 File Offset: 0x0001E950
	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		if (UnityEngine.Random.Range(0, 5) > 1)
		{
			return;
		}
		if (LevelController.instance.radiosInLevel.Length != 0)
		{
			for (int i = 0; i < LevelController.instance.radiosInLevel.Length; i++)
			{
				if (Vector3.Distance(this.ghostAI.transform.position, LevelController.instance.radiosInLevel[i].transform.position) < 2f)
				{
					LevelController.instance.radiosInLevel[i].view.RPC("TurnOn", RpcTarget.All, Array.Empty<object>());
					this.ghostInteraction.CreateInteractionEMF(LevelController.instance.radiosInLevel[i].transform.position);
				}
			}
		}
	}

	// Token: 0x06000583 RID: 1411 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Execute()
	{
	}

	// Token: 0x06000584 RID: 1412 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Exit()
	{
	}

	// Token: 0x04000533 RID: 1331
	private GhostAI ghostAI;

	// Token: 0x04000534 RID: 1332
	private GhostInteraction ghostInteraction;
}

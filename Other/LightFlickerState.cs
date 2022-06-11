using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x020000BD RID: 189
public class LightFlickerState : IState
{
	// Token: 0x06000571 RID: 1393 RVA: 0x000204EF File Offset: 0x0001E6EF
	public LightFlickerState(GhostAI ghostAI, GhostInteraction ghostInteraction)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x00020508 File Offset: 0x0001E708
	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		if (LevelController.instance.currentGhostRoom.lightSwitches.Count == 0)
		{
			return;
		}
		int index = UnityEngine.Random.Range(0, LevelController.instance.currentGhostRoom.lightSwitches.Count);
		if (LevelController.instance.currentGhostRoom.lightSwitches[index].isOn)
		{
			LevelController.instance.currentGhostRoom.lightSwitches[index].view.RPC("FlickerNetworked", RpcTarget.All, Array.Empty<object>());
		}
		this.ghostInteraction.CreateInteractionEMF(LevelController.instance.currentGhostRoom.lightSwitches[index].transform.position);
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Execute()
	{
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Exit()
	{
	}

	// Token: 0x04000528 RID: 1320
	private GhostAI ghostAI;

	// Token: 0x04000529 RID: 1321
	private GhostInteraction ghostInteraction;
}

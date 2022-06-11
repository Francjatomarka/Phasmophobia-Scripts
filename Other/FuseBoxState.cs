using System;
using UnityEngine;

// Token: 0x020000B9 RID: 185
public class FuseBoxState : IState
{
	// Token: 0x0600055E RID: 1374 RVA: 0x0001EDBB File Offset: 0x0001CFBB
	public FuseBoxState(GhostAI ghostAI, GhostInteraction ghostInteraction)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
	}

	// Token: 0x0600055F RID: 1375 RVA: 0x0001EDD4 File Offset: 0x0001CFD4
	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		if (GameController.instance.isTutorial)
		{
			return;
		}
		if (LevelController.instance.fuseBox.isOn)
		{
			if (this.ghostAI.ghostInfo.ghostTraits.ghostType != GhostTraits.Type.Jinn)
			{
				this.ghostInteraction.CreateInteractionEMF(LevelController.instance.fuseBox.transform.position);
				LevelController.instance.fuseBox.TurnOff();
				return;
			}
		}
		else if (UnityEngine.Random.Range(0, 5) == 1)
		{
			this.ghostInteraction.CreateInteractionEMF(LevelController.instance.fuseBox.transform.position);
			LevelController.instance.fuseBox.Use();
		}
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Execute()
	{
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Exit()
	{
	}

	// Token: 0x04000513 RID: 1299
	private GhostAI ghostAI;

	// Token: 0x04000514 RID: 1300
	private GhostInteraction ghostInteraction;
}

using System;
using Photon.Pun;

// Token: 0x020000AF RID: 175
public class CCTVState : IState
{
	// Token: 0x06000531 RID: 1329 RVA: 0x0001CBEE File Offset: 0x0001ADEE
	public CCTVState(GhostAI ghostAI, GhostInteraction ghostInteraction, PhotonObjectInteract obj)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.cam = obj.GetComponent<CCTV>();
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x0001CC10 File Offset: 0x0001AE10
	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		if (LevelController.instance.fuseBox.isOn)
		{
			for (int i = 0; i < LevelController.instance.currentGhostRoom.lightSwitches.Count; i++)
			{
				if (LevelController.instance.currentGhostRoom.lightSwitches[i].isOn)
				{
					return;
				}
			}
		}
		if (!this.cam.GetComponent<PhotonView>().IsMine)
		{
			this.cam.GetComponent<PhotonView>().RequestOwnership();
		}
		this.ghostInteraction.CreateInteractionEMF(this.cam.transform.position);
		this.cam.TurnOff();
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Execute()
	{
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Exit()
	{
	}

	// Token: 0x040004ED RID: 1261
	private GhostAI ghostAI;

	// Token: 0x040004EE RID: 1262
	private GhostInteraction ghostInteraction;

	// Token: 0x040004EF RID: 1263
	private CCTV cam;
}

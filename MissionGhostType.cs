using System;
using Photon.Pun;

// Token: 0x02000150 RID: 336
public class MissionGhostType : Mission
{
	// Token: 0x0600098B RID: 2443 RVA: 0x0003AC76 File Offset: 0x00038E76
	private void Awake()
	{
		MissionGhostType.instance = this;
		this.view = base.GetComponent<PhotonView>();
		this.SetMissionType();
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x0003AC90 File Offset: 0x00038E90
	[PunRPC]
	private void CompletedGhostMissionSync()
	{
		base.Completed();
	}

	// Token: 0x0600098D RID: 2445 RVA: 0x0003AC98 File Offset: 0x00038E98
	public void CheckMissionComplete()
	{
		if (LevelController.instance && LevelController.instance.journalController.GetGhostType() == LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType)
		{
			this.view.RPC("CompletedGhostMissionSync", RpcTarget.AllBuffered, Array.Empty<object>());
		}
	}

	// Token: 0x0600098E RID: 2446 RVA: 0x0003ACF1 File Offset: 0x00038EF1
	private void SetMissionType()
	{
		this.type = Mission.MissionType.main;
		this.completed = false;
		this.missionName = LocalisationSystem.GetLocalisedValue("Mission_GhostType");
	}

	// Token: 0x040009C9 RID: 2505
	public static MissionGhostType instance;
}

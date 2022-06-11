using System;
using Photon.Pun;

// Token: 0x0200015C RID: 348
public class MissionVictimName : Mission
{
	// Token: 0x060009CB RID: 2507 RVA: 0x0003B489 File Offset: 0x00039689
	private void Awake()
	{
		MissionVictimName.instance = this;
		this.SetMissionType();
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x060009CC RID: 2508 RVA: 0x0003B4A3 File Offset: 0x000396A3
	public void CompleteMission()
	{
		this.view.RPC("CompletedVictimMissionSync", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	// Token: 0x060009CD RID: 2509 RVA: 0x0003AC90 File Offset: 0x00038E90
	[PunRPC]
	private void CompletedVictimMissionSync()
	{
		base.Completed();
	}

	// Token: 0x060009CE RID: 2510 RVA: 0x0003B4BB File Offset: 0x000396BB
	private void SetMissionType()
	{
		this.type = Mission.MissionType.side;
		this.completed = false;
		this.missionName = LocalisationSystem.GetLocalisedValue("Mission_VictimName");
	}

	// Token: 0x040009E5 RID: 2533
	public static MissionVictimName instance;
}

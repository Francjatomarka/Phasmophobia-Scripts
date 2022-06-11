using System;
using Photon.Pun;

// Token: 0x02000157 RID: 343
public class MissionDirtyWater : Mission
{
	// Token: 0x060009B2 RID: 2482 RVA: 0x0003B2D7 File Offset: 0x000394D7
	private void Awake()
	{
		MissionDirtyWater.instance = this;
		this.SetMissionType();
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x060009B3 RID: 2483 RVA: 0x0003B2F1 File Offset: 0x000394F1
	public void CompleteMission()
	{
		this.view.RPC("CompletedDirtyWaterMissionSync", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	// Token: 0x060009B4 RID: 2484 RVA: 0x0003AC90 File Offset: 0x00038E90
	[PunRPC]
	private void CompletedDirtyWaterMissionSync()
	{
		base.Completed();
	}

	// Token: 0x060009B5 RID: 2485 RVA: 0x0003B309 File Offset: 0x00039509
	private void SetMissionType()
	{
		this.type = Mission.MissionType.side;
		this.completed = false;
		this.missionName = LocalisationSystem.GetLocalisedValue("Mission_DirtyWater");
	}

	// Token: 0x040009E0 RID: 2528
	public static MissionDirtyWater instance;
}

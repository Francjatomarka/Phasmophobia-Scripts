using System;
using Photon.Pun;

// Token: 0x02000153 RID: 339
public class MissionBurnSage : Mission
{
	// Token: 0x0600099E RID: 2462 RVA: 0x0003B18F File Offset: 0x0003938F
	private void Awake()
	{
		MissionBurnSage.instance = this;
		this.SetMissionType();
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x0600099F RID: 2463 RVA: 0x0003B1A9 File Offset: 0x000393A9
	public void CompleteMission()
	{
		this.view.RPC("CompletedBurnSageMissionSync", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	// Token: 0x060009A0 RID: 2464 RVA: 0x0003AC90 File Offset: 0x00038E90
	[PunRPC]
	private void CompletedBurnSageMissionSync()
	{
		base.Completed();
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x0003B1C1 File Offset: 0x000393C1
	private void SetMissionType()
	{
		this.type = Mission.MissionType.side;
		this.completed = false;
		this.missionName = LocalisationSystem.GetLocalisedValue("Mission_BurnSage");
	}

	// Token: 0x040009DC RID: 2524
	public static MissionBurnSage instance;
}

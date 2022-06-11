using System;
using Photon.Pun;

// Token: 0x02000155 RID: 341
public class MissionCrucifix : Mission
{
	// Token: 0x060009A8 RID: 2472 RVA: 0x0003B233 File Offset: 0x00039433
	private void Awake()
	{
		MissionCrucifix.instance = this;
		this.SetMissionType();
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x060009A9 RID: 2473 RVA: 0x0003B24D File Offset: 0x0003944D
	public void CompleteMission()
	{
		this.view.RPC("CompletedCrucifixMissionSync", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x0003AC90 File Offset: 0x00038E90
	[PunRPC]
	private void CompletedCrucifixMissionSync()
	{
		base.Completed();
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x0003B265 File Offset: 0x00039465
	private void SetMissionType()
	{
		this.type = Mission.MissionType.side;
		this.completed = false;
		this.missionName = LocalisationSystem.GetLocalisedValue("Mission_CrucifixName");
	}

	// Token: 0x040009DE RID: 2526
	public static MissionCrucifix instance;
}

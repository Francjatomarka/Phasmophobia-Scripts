using System;
using Photon.Pun;

// Token: 0x02000159 RID: 345
public class MissionGhostEvent : Mission
{
	// Token: 0x060009BC RID: 2492 RVA: 0x0003B37B File Offset: 0x0003957B
	private void Awake()
	{
		MissionGhostEvent.instance = this;
		this.SetMissionType();
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x060009BD RID: 2493 RVA: 0x0003B395 File Offset: 0x00039595
	public void CompleteMission()
	{
		this.view.RPC("CompletedGhostEventMissionSync", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	// Token: 0x060009BE RID: 2494 RVA: 0x0003AC90 File Offset: 0x00038E90
	[PunRPC]
	private void CompletedGhostEventMissionSync()
	{
		base.Completed();
	}

	// Token: 0x060009BF RID: 2495 RVA: 0x0003B3AD File Offset: 0x000395AD
	private void SetMissionType()
	{
		this.type = Mission.MissionType.side;
		this.completed = false;
		this.missionName = LocalisationSystem.GetLocalisedValue("Mission_GhostEventName");
	}

	// Token: 0x040009E2 RID: 2530
	public static MissionGhostEvent instance;
}

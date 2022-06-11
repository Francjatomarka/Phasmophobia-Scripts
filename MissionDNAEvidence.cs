using System;
using Photon.Pun;

// Token: 0x02000156 RID: 342
public class MissionDNAEvidence : Mission
{
	// Token: 0x060009AD RID: 2477 RVA: 0x0003B285 File Offset: 0x00039485
	private void Awake()
	{
		MissionDNAEvidence.instance = this;
		this.SetMissionType();
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x0003B29F File Offset: 0x0003949F
	public void CompleteMission()
	{
		this.view.RPC("CompletedDNAMissionSync", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x0003AC90 File Offset: 0x00038E90
	[PunRPC]
	private void CompletedDNAMissionSync()
	{
		base.Completed();
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x0003B2B7 File Offset: 0x000394B7
	private void SetMissionType()
	{
		this.type = Mission.MissionType.side;
		this.completed = false;
		this.missionName = LocalisationSystem.GetLocalisedValue("Mission_DNAEvidence");
	}

	// Token: 0x040009DF RID: 2527
	public static MissionDNAEvidence instance;
}

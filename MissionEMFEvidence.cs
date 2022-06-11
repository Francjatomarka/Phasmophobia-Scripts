using System;
using Photon.Pun;

// Token: 0x02000158 RID: 344
public class MissionEMFEvidence : Mission
{
	// Token: 0x060009B7 RID: 2487 RVA: 0x0003B329 File Offset: 0x00039529
	private void Awake()
	{
		MissionEMFEvidence.instance = this;
		this.SetMissionType();
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x060009B8 RID: 2488 RVA: 0x0003B343 File Offset: 0x00039543
	public void CompleteMission()
	{
		this.view.RPC("CompletedEMFMissionSync", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	// Token: 0x060009B9 RID: 2489 RVA: 0x0003AC90 File Offset: 0x00038E90
	[PunRPC]
	private void CompletedEMFMissionSync()
	{
		base.Completed();
	}

	// Token: 0x060009BA RID: 2490 RVA: 0x0003B35B File Offset: 0x0003955B
	private void SetMissionType()
	{
		this.type = Mission.MissionType.side;
		this.completed = false;
		this.missionName = LocalisationSystem.GetLocalisedValue("Mission_EMFEvidence");
	}

	// Token: 0x040009E1 RID: 2529
	public static MissionEMFEvidence instance;
}

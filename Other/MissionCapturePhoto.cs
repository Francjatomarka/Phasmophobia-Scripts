using System;
using Photon.Pun;

// Token: 0x02000154 RID: 340
public class MissionCapturePhoto : Mission
{
	// Token: 0x060009A3 RID: 2467 RVA: 0x0003B1E1 File Offset: 0x000393E1
	private void Awake()
	{
		MissionCapturePhoto.instance = this;
		this.SetMissionType();
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x060009A4 RID: 2468 RVA: 0x0003B1FB File Offset: 0x000393FB
	public void CompleteMission()
	{
		this.view.RPC("CompletedCapturePhotoMissionSync", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x0003AC90 File Offset: 0x00038E90
	[PunRPC]
	private void CompletedCapturePhotoMissionSync()
	{
		base.Completed();
	}

	// Token: 0x060009A6 RID: 2470 RVA: 0x0003B213 File Offset: 0x00039413
	private void SetMissionType()
	{
		this.type = Mission.MissionType.side;
		this.completed = false;
		this.missionName = LocalisationSystem.GetLocalisedValue("Mission_CapturePhoto");
	}

	// Token: 0x040009DD RID: 2525
	public static MissionCapturePhoto instance;
}

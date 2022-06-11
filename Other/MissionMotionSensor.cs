using System;
using Photon.Pun;

// Token: 0x0200015A RID: 346
public class MissionMotionSensor : Mission
{
	// Token: 0x060009C1 RID: 2497 RVA: 0x0003B3CD File Offset: 0x000395CD
	private void Awake()
	{
		MissionMotionSensor.instance = this;
		this.SetMissionType();
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x0003B3E7 File Offset: 0x000395E7
	public void CompleteMission()
	{
		this.view.RPC("CompletedMotionMissionSync", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x0003AC90 File Offset: 0x00038E90
	[PunRPC]
	private void CompletedMotionMissionSync()
	{
		base.Completed();
	}

	// Token: 0x060009C4 RID: 2500 RVA: 0x0003B3FF File Offset: 0x000395FF
	private void SetMissionType()
	{
		this.type = Mission.MissionType.side;
		this.completed = false;
		this.missionName = LocalisationSystem.GetLocalisedValue("Mission_MotionSensor");
	}

	// Token: 0x040009E3 RID: 2531
	public static MissionMotionSensor instance;
}

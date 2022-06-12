using System;
using Photon.Pun;

public class MissionDirtyWater : Mission
{
	private void Awake()
	{
		MissionDirtyWater.instance = this;
		this.SetMissionType();
		this.view = base.GetComponent<PhotonView>();
	}

	public void CompleteMission()
	{
		this.view.RPC("CompletedDirtyWaterMissionSync", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	[PunRPC]
	private void CompletedDirtyWaterMissionSync()
	{
		base.Completed();
	}

	private void SetMissionType()
	{
		this.type = Mission.MissionType.side;
		this.completed = false;
		this.missionName = LocalisationSystem.GetLocalisedValue("Mission_DirtyWater");
	}

	public static MissionDirtyWater instance;
}


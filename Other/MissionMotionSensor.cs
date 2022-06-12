using System;
using Photon.Pun;

public class MissionMotionSensor : Mission
{
	private void Awake()
	{
		MissionMotionSensor.instance = this;
		this.SetMissionType();
		this.view = base.GetComponent<PhotonView>();
	}

	public void CompleteMission()
	{
		this.view.RPC("CompletedMotionMissionSync", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	[PunRPC]
	private void CompletedMotionMissionSync()
	{
		base.Completed();
	}

	private void SetMissionType()
	{
		this.type = Mission.MissionType.side;
		this.completed = false;
		this.missionName = LocalisationSystem.GetLocalisedValue("Mission_MotionSensor");
	}

	public static MissionMotionSensor instance;
}


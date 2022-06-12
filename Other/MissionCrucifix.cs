using System;
using Photon.Pun;

public class MissionCrucifix : Mission
{
	private void Awake()
	{
		MissionCrucifix.instance = this;
		this.SetMissionType();
		this.view = base.GetComponent<PhotonView>();
	}

	public void CompleteMission()
	{
		this.view.RPC("CompletedCrucifixMissionSync", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	[PunRPC]
	private void CompletedCrucifixMissionSync()
	{
		base.Completed();
	}

	private void SetMissionType()
	{
		this.type = Mission.MissionType.side;
		this.completed = false;
		this.missionName = LocalisationSystem.GetLocalisedValue("Mission_CrucifixName");
	}

	public static MissionCrucifix instance;
}


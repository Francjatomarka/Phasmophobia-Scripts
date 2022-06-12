using System;
using Photon.Pun;

public class MissionVictimName : Mission
{
	private void Awake()
	{
		MissionVictimName.instance = this;
		this.SetMissionType();
		this.view = base.GetComponent<PhotonView>();
	}

	public void CompleteMission()
	{
		this.view.RPC("CompletedVictimMissionSync", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	[PunRPC]
	private void CompletedVictimMissionSync()
	{
		base.Completed();
	}

	private void SetMissionType()
	{
		this.type = Mission.MissionType.side;
		this.completed = false;
		this.missionName = LocalisationSystem.GetLocalisedValue("Mission_VictimName");
	}

	public static MissionVictimName instance;
}


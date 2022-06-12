using System;
using Photon.Pun;

public class MissionGhostEvent : Mission
{
	private void Awake()
	{
		MissionGhostEvent.instance = this;
		this.SetMissionType();
		this.view = base.GetComponent<PhotonView>();
	}

	public void CompleteMission()
	{
		this.view.RPC("CompletedGhostEventMissionSync", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	[PunRPC]
	private void CompletedGhostEventMissionSync()
	{
		base.Completed();
	}

	private void SetMissionType()
	{
		this.type = Mission.MissionType.side;
		this.completed = false;
		this.missionName = LocalisationSystem.GetLocalisedValue("Mission_GhostEventName");
	}

	public static MissionGhostEvent instance;
}


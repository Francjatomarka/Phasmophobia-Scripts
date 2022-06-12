using System;
using UnityEngine;
using Photon.Pun;

public class MissionTemperature : Mission
{
	private void Awake()
	{
		MissionTemperature.instance = this;
		this.SetMissionType();
		this.view = base.GetComponent<PhotonView>();
	}

	public void CompleteMission()
	{
		this.view.RPC("CompletedTemperatureMissionSync", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	[PunRPC]
	private void CompletedTemperatureMissionSync()
	{
		base.Completed();
	}

	private void SetMissionType()
	{
		this.type = Mission.MissionType.side;
		this.completed = false;
		this.missionName = ((PlayerPrefs.GetInt("degreesValue") == 0) ? LocalisationSystem.GetLocalisedValue("Mission_Temperature") : LocalisationSystem.GetLocalisedValue("Mission_TemperatureFarenheit"));
	}

	public static MissionTemperature instance;
}


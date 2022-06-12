using System;
using Photon.Pun;

public class MissionEMFEvidence : Mission
{
	private void Awake()
	{
		MissionEMFEvidence.instance = this;
		this.SetMissionType();
		this.view = base.GetComponent<PhotonView>();
	}

	public void CompleteMission()
	{
		this.view.RPC("CompletedEMFMissionSync", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	[PunRPC]
	private void CompletedEMFMissionSync()
	{
		base.Completed();
	}

	private void SetMissionType()
	{
		this.type = Mission.MissionType.side;
		this.completed = false;
		this.missionName = LocalisationSystem.GetLocalisedValue("Mission_EMFEvidence");
	}

	public static MissionEMFEvidence instance;
}


using System;
using Photon.Pun;

public class MissionDNAEvidence : Mission
{
	private void Awake()
	{
		MissionDNAEvidence.instance = this;
		this.SetMissionType();
		this.view = base.GetComponent<PhotonView>();
	}

	public void CompleteMission()
	{
		this.view.RPC("CompletedDNAMissionSync", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	[PunRPC]
	private void CompletedDNAMissionSync()
	{
		base.Completed();
	}

	private void SetMissionType()
	{
		this.type = Mission.MissionType.side;
		this.completed = false;
		this.missionName = LocalisationSystem.GetLocalisedValue("Mission_DNAEvidence");
	}

	public static MissionDNAEvidence instance;
}


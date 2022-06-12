using System;
using Photon.Pun;

public class MissionCapturePhoto : Mission
{
	private void Awake()
	{
		MissionCapturePhoto.instance = this;
		this.SetMissionType();
		this.view = base.GetComponent<PhotonView>();
	}

	public void CompleteMission()
	{
		this.view.RPC("CompletedCapturePhotoMissionSync", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	[PunRPC]
	private void CompletedCapturePhotoMissionSync()
	{
		base.Completed();
	}

	private void SetMissionType()
	{
		this.type = Mission.MissionType.side;
		this.completed = false;
		this.missionName = LocalisationSystem.GetLocalisedValue("Mission_CapturePhoto");
	}

	public static MissionCapturePhoto instance;
}


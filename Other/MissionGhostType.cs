using System;
using Photon.Pun;

public class MissionGhostType : Mission
{
	private void Awake()
	{
		MissionGhostType.instance = this;
		this.view = base.GetComponent<PhotonView>();
		this.SetMissionType();
	}

	[PunRPC]
	private void CompletedGhostMissionSync()
	{
		base.Completed();
	}

	public void CheckMissionComplete()
	{
		if (LevelController.instance && LevelController.instance.journalController.GetGhostType() == LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType)
		{
			this.view.RPC("CompletedGhostMissionSync", RpcTarget.AllBuffered, Array.Empty<object>());
		}
	}

	private void SetMissionType()
	{
		this.type = Mission.MissionType.main;
		this.completed = false;
		this.missionName = LocalisationSystem.GetLocalisedValue("Mission_GhostType");
	}

	public static MissionGhostType instance;
}


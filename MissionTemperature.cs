using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x020001A0 RID: 416
public class MissionTemperature : Mission
{
	// Token: 0x06000B56 RID: 2902 RVA: 0x00045D72 File Offset: 0x00043F72
	private void Awake()
	{
		MissionTemperature.instance = this;
		this.SetMissionType();
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x06000B57 RID: 2903 RVA: 0x00045D8C File Offset: 0x00043F8C
	public void CompleteMission()
	{
		this.view.RPC("CompletedTemperatureMissionSync", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	// Token: 0x06000B58 RID: 2904 RVA: 0x000454FC File Offset: 0x000436FC
	[PunRPC]
	private void CompletedTemperatureMissionSync()
	{
		base.Completed();
	}

	// Token: 0x06000B59 RID: 2905 RVA: 0x00045DA4 File Offset: 0x00043FA4
	private void SetMissionType()
	{
		this.type = Mission.MissionType.side;
		this.completed = false;
		this.missionName = ((PlayerPrefs.GetInt("degreesValue") == 0) ? LocalisationSystem.GetLocalisedValue("Mission_Temperature") : LocalisationSystem.GetLocalisedValue("Mission_TemperatureFarenheit"));
	}

	// Token: 0x04000B90 RID: 2960
	public static MissionTemperature instance;
}

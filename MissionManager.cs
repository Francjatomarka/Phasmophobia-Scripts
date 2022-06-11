using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000152 RID: 338
[RequireComponent(typeof(PhotonView))]
public class MissionManager : MonoBehaviourPunCallbacks
{
	// Token: 0x06000993 RID: 2451 RVA: 0x0003ADA0 File Offset: 0x00038FA0
	private void Awake()
	{
		MissionManager.instance = this;
		this.view = base.GetComponent<PhotonView>();
		this.CreateMissionsList();
	}

	// Token: 0x06000994 RID: 2452 RVA: 0x0003ADBA File Offset: 0x00038FBA
	private void Start()
	{
		if (PhotonNetwork.IsMasterClient && GameController.instance != null)
		{
			base.StartCoroutine(this.GiveQuests());
			GameController.instance.OnGhostSpawned.AddListener(new UnityAction(this.SetMissionDescription));
		}
	}

	// Token: 0x06000995 RID: 2453 RVA: 0x0003ADBA File Offset: 0x00038FBA
	private void OnJoinedRoom()
	{
		if (PhotonNetwork.IsMasterClient && GameController.instance != null)
		{
			base.StartCoroutine(this.GiveQuests());
			GameController.instance.OnGhostSpawned.AddListener(new UnityAction(this.SetMissionDescription));
		}
	}

	// Token: 0x06000996 RID: 2454 RVA: 0x0003ADF7 File Offset: 0x00038FF7
	private IEnumerator GiveQuests()
	{
		this.view.RPC("SetMission", RpcTarget.AllBuffered, new object[]
		{
			1,
			UnityEngine.Random.Range(0, this.mainMissions.Count),
			1
		});
		yield return new WaitForSeconds(0.5f);
		this.view.RPC("SetMission", RpcTarget.AllBuffered, new object[]
		{
			2,
			UnityEngine.Random.Range(0, this.sideMissions.Count),
			2
		});
		yield return new WaitForSeconds(0.5f);
		this.view.RPC("SetMission", RpcTarget.AllBuffered, new object[]
		{
			2,
			UnityEngine.Random.Range(0, this.sideMissions.Count),
			3
		});
		yield return new WaitForSeconds(0.5f);
		this.view.RPC("SetMission", RpcTarget.AllBuffered, new object[]
		{
			2,
			UnityEngine.Random.Range(0, this.sideMissions.Count),
			4
		});
		yield break;
	}

	// Token: 0x06000997 RID: 2455 RVA: 0x0003AE08 File Offset: 0x00039008
	[PunRPC]
	private void SetMission(int typeID, int missionID, int textUIID)
	{
		this.CreateMissionsList();
		Text text;
		if (textUIID == 1)
		{
			text = this.mainMissionText;
		}
		else if (textUIID == 2)
		{
			text = this.sideMissionText;
		}
		else if (textUIID == 3)
		{
			text = this.side2MissionText;
		}
		else
		{
			text = this.hiddenMissionText;
		}
		if (typeID == 1)
		{
			this.AddMission(this.mainMissions[missionID], Mission.MissionType.main, text);
			return;
		}
		if (typeID == 2)
		{
			this.AddMission(this.sideMissions[missionID], Mission.MissionType.side, text);
		}
	}

	// Token: 0x06000998 RID: 2456 RVA: 0x0003AE7C File Offset: 0x0003907C
	private void CreateMissionsList()
	{
		if (this.hasCreatedMissions)
		{
			return;
		}
		this.hasCreatedMissions = true;
		this.sideMissions.Add(typeof(MissionCapturePhoto));
		this.sideMissions.Add(typeof(MissionEMFEvidence));
		this.sideMissions.Add(typeof(MissionBurnSage));
		this.sideMissions.Add(typeof(MissionMotionSensor));
		this.sideMissions.Add(typeof(MissionTemperature));
		this.sideMissions.Add(typeof(MissionDirtyWater));
		this.sideMissions.Add(typeof(MissionCrucifix));
		this.sideMissions.Add(typeof(MissionGhostEvent));
		this.mainMissions.Add(typeof(MissionGhostType));
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x0003AF56 File Offset: 0x00039156
	private void AddMission(Type mission, Mission.MissionType type, Text text)
	{
		this.AddMissionComponent(mission, type, text);
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x0003AF64 File Offset: 0x00039164
	private void AddMissionComponent(Type mission, Mission.MissionType type, Text text)
	{
		if (type == Mission.MissionType.main)
		{
			for (int i = 0; i < this.mainMissions.Count; i++)
			{
				if (this.mainMissions[i] == mission)
				{
					Mission mission2 = base.gameObject.AddComponent(this.mainMissions[i]) as Mission;
					this.currentMissions.Add(mission2);
					mission2.myText = text;
					mission2.sideMissionID = 0;
					mission2.SetUIText();
					this.mainMissions.RemoveAt(i);
				}
			}
			return;
		}
		if (type == Mission.MissionType.side)
		{
			this.missionIndex++;
			for (int j = 0; j < this.sideMissions.Count; j++)
			{
				if (this.sideMissions[j] == mission)
				{
					Mission mission3 = base.gameObject.AddComponent(this.sideMissions[j]) as Mission;
					this.currentMissions.Add(mission3);
					mission3.myText = text;
					mission3.sideMissionID = this.missionIndex;
					mission3.SetUIText();
					this.sideMissions.RemoveAt(j);
				}
			}
		}
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x0003B078 File Offset: 0x00039278
	private void SetMissionDescription()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			string text = LocalisationSystem.GetLocalisedValue("WhiteBoard_FirstSentence") + " " + LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostName + ".";
			text = text + " " + (LevelController.instance.currentGhost.ghostInfo.ghostTraits.isShy ? LocalisationSystem.GetLocalisedValue("WhiteBoard_GhostResponse2") : LocalisationSystem.GetLocalisedValue("WhiteBoard_GhostResponse1"));
			string localisedValue = LocalisationSystem.GetLocalisedValue("WhiteBoard_SecondSentence");
			string localisedValue2 = LocalisationSystem.GetLocalisedValue("WhiteBoard_ThirdSentence");
			string text2 = string.Concat(new string[]
			{
				text,
				" ",
				localisedValue,
				" ",
				localisedValue2
			});
			this.view.RPC("SetMissionDescriptionSynced", RpcTarget.AllBuffered, new object[]
			{
				text2
			});
		}
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x0003B158 File Offset: 0x00039358
	[PunRPC]
	private void SetMissionDescriptionSynced(string description)
	{
		this.missionDescription.text = description;
	}

	// Token: 0x040009D0 RID: 2512
	public static MissionManager instance;

	// Token: 0x040009D1 RID: 2513
	private List<Type> mainMissions = new List<Type>();

	// Token: 0x040009D2 RID: 2514
	private List<Type> sideMissions = new List<Type>();

	// Token: 0x040009D3 RID: 2515
	[SerializeField]
	private Text mainMissionText;

	// Token: 0x040009D4 RID: 2516
	[SerializeField]
	private Text sideMissionText;

	// Token: 0x040009D5 RID: 2517
	[SerializeField]
	private Text side2MissionText;

	// Token: 0x040009D6 RID: 2518
	[SerializeField]
	private Text hiddenMissionText;

	// Token: 0x040009D7 RID: 2519
	[SerializeField]
	private Text missionDescription;

	// Token: 0x040009D8 RID: 2520
	public List<Mission> currentMissions = new List<Mission>();

	// Token: 0x040009D9 RID: 2521
	private PhotonView view;

	// Token: 0x040009DA RID: 2522
	private bool hasCreatedMissions;

	// Token: 0x040009DB RID: 2523
	private int missionIndex;
}

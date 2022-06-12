using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class MissionManager : MonoBehaviourPunCallbacks
{
	private void Awake()
	{
		MissionManager.instance = this;
		this.view = base.GetComponent<PhotonView>();
		this.CreateMissionsList();
	}

	private void Start()
	{
		if (PhotonNetwork.IsMasterClient && GameController.instance != null)
		{
			base.StartCoroutine(this.GiveQuests());
			GameController.instance.OnGhostSpawned.AddListener(new UnityAction(this.SetMissionDescription));
		}
	}

	private void OnJoinedRoom()
	{
		if (PhotonNetwork.IsMasterClient && GameController.instance != null)
		{
			base.StartCoroutine(this.GiveQuests());
			GameController.instance.OnGhostSpawned.AddListener(new UnityAction(this.SetMissionDescription));
		}
	}

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

	private void AddMission(Type mission, Mission.MissionType type, Text text)
	{
		this.AddMissionComponent(mission, type, text);
	}

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

	[PunRPC]
	private void SetMissionDescriptionSynced(string description)
	{
		this.missionDescription.text = description;
	}

	public static MissionManager instance;

	private List<Type> mainMissions = new List<Type>();

	private List<Type> sideMissions = new List<Type>();

	[SerializeField]
	private Text mainMissionText;

	[SerializeField]
	private Text sideMissionText;

	[SerializeField]
	private Text side2MissionText;

	[SerializeField]
	private Text hiddenMissionText;

	[SerializeField]
	private Text missionDescription;

	public List<Mission> currentMissions = new List<Mission>();

	private PhotonView view;

	private bool hasCreatedMissions;

	private int missionIndex;
}


using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using Photon.Pun;

public class ExitLevel : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
	}

	public void StartAttemptExitLevel()
	{
		base.StopAllCoroutines();
		if (this.isExiting)
		{
			base.StartCoroutine(this.AttemptExitLevel());
		}
	}

	public bool ThereAreAlivePlayersOutsideTheTruck()
	{
		bool result = false;
		for (int i = 0; i < GameController.instance.playersData.Count; i++)
		{
			if (GameController.instance.playersData[i] != null && GameController.instance.playersData[i].player != null && !GameController.instance.playersData[i].player.isDead && !this.trigger.playersInTruck.Contains(GameController.instance.playersData[i].player))
			{
				result = true;
			}
		}
		return result;
	}

	[PunRPC]
	private void PlayTruckStartUpSound()
	{
		this.source.clip = this.startExitSound;
		this.source.Play();
	}

	[PunRPC]
	private void PlayTruckStopSound()
	{
		this.source.clip = this.stopExitSound;
		this.source.Play();
	}

	private IEnumerator AttemptExitLevel()
	{
		this.view.RPC("PlayTruckStartUpSound", RpcTarget.All, Array.Empty<object>());
		yield return new WaitForSeconds(9f);
		if (!this.isExiting)
		{
			this.view.RPC("PlayTruckStopSound", RpcTarget.All, Array.Empty<object>());
			yield return null;
		}
		if (this.trigger.playersInTruck.Count == 0)
		{
			this.isExiting = false;
			this.view.RPC("PlayTruckStopSound", RpcTarget.All, Array.Empty<object>());
			yield return null;
		}
		if (this.ThereAreAlivePlayersOutsideTheTruck())
		{
			this.isExiting = false;
			this.view.RPC("PlayTruckStopSound", RpcTarget.All, Array.Empty<object>());
			yield return null;
		}
		if (this.isExiting)
		{
			this.view.RPC("Exit", RpcTarget.AllBufferedViaServer, Array.Empty<object>());
		}
		yield break;
	}

	[PunRPC]
	private void Exit()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			this.view.RPC("SyncPhotoValue", RpcTarget.AllBuffered, new object[]
			{
				EvidenceController.instance.totalEvidenceFoundInPhotos
			});
		}
		if (GameController.instance.isTutorial)
		{
			PlayerPrefs.SetInt("MissionStatus", 3);
			PlayerPrefs.SetInt("setupPhase", 0);
			PlayerPrefs.SetInt("completedTraining", 1);
			PlayerPrefs.SetInt("StayInServerRoom", 0);
			if (MissionGhostType.instance)
			{
				MissionGhostType.instance.CheckMissionComplete();
			}
		}
		else if (!GameController.instance.myPlayer.player.isDead)
		{
			PlayerPrefs.SetInt("MissionStatus", 1);
			PlayerPrefs.SetInt("StayInServerRoom", 1);
			this.CheckMissions();
			this.CheckExp();
			this.CheckChallenges(false);
		}
		else
		{
			PlayerPrefs.SetInt("PlayerDied", 1);
			PlayerPrefs.SetInt("MissionStatus", 1);
			PlayerPrefs.SetInt("StayInServerRoom", 1);
			this.CheckMissions();
			this.CheckChallenges(true);
			InventoryManager.RemoveItemsFromInventory();
		}
		base.StartCoroutine(this.LoadLevelAfterDelay());
	}

	private IEnumerator LoadLevelAfterDelay()
	{
		yield return new WaitForSeconds(2f);
		GameController.instance.OnExitLevel.Invoke();
		AsyncOperation async = SceneManager.LoadSceneAsync("Menu_New");
		while (!async.isDone)
		{
			if (async.progress == 0.9f)
			{
				GameController.instance.myPlayer.player.pcCanvas.LoadingGame();
			}
			yield return null;
		}
		GameController.instance.myPlayer.player.gameObject.SetActive(false);
		async = null;
		yield break;
	}

	private void CheckMissions()
	{
		if (MissionGhostType.instance)
		{
			MissionGhostType.instance.CheckMissionComplete();
		}
		PlayerPrefs.SetInt("MissionType", (int)LevelController.instance.type);
		foreach (Mission mission in MissionManager.instance.currentMissions)
		{
			if (mission.type == Mission.MissionType.main)
			{
				PlayerPrefs.SetInt("MainMission", mission.completed ? 1 : 0);
			}
			else if (mission.type == Mission.MissionType.side)
			{
				if (mission.sideMissionID == 1)
				{
					PlayerPrefs.SetInt("SideMission1", mission.completed ? 1 : 0);
				}
				else if (mission.sideMissionID == 2)
				{
					PlayerPrefs.SetInt("SideMission2", mission.completed ? 1 : 0);
				}
				else if (mission.sideMissionID == 3)
				{
					PlayerPrefs.SetInt("SideMission3", mission.completed ? 1 : 0);
				}
			}
		}
		PlayerPrefs.SetInt("DNAMission", EvidenceController.instance.foundGhostDNA ? 1 : 0);
		int num = 0;
		num += PlayerPrefs.GetInt("MainMission");
		num += PlayerPrefs.GetInt("SideMission1");
		num += PlayerPrefs.GetInt("SideMission2");
		num += PlayerPrefs.GetInt("SideMission3");
		if (num > 0)
		{
			DailyChallengesController.Instance.ChangeChallengeProgression(ChallengeType.CompleteObjectives, num);
		}
	}

	private void CheckExp()
	{
		int num = 0;
		int @int = PlayerPrefs.GetInt("myTotalExp");
		num += PlayerPrefs.GetInt("MainMission") * 20 * (int)(LevelController.instance.type + 1);
		num += PlayerPrefs.GetInt("SideMission1") * 15;
		num += PlayerPrefs.GetInt("SideMission2") * 15;
		num += PlayerPrefs.GetInt("SideMission3") * 15;
		num += PlayerPrefs.GetInt("DNAMission") * 10 * (int)(LevelController.instance.type + 1);
		for (int i = 0; i < GameController.instance.playersData.Count; i++)
		{
			if (GameController.instance.playersData[i].player.isDead)
			{
				num -= 10;
			}
		}
		if (num < 0)
		{
			num = 0;
		}
		if (GameController.instance.levelDifficulty == Contract.LevelDifficulty.Intermediate)
		{
			num = (int)((double)num * 1.5);
		}
		else if (GameController.instance.levelDifficulty == Contract.LevelDifficulty.Professional)
		{
			num *= 2;
		}
		PlayerPrefs.SetInt("totalExp", num);
		PlayerPrefs.SetInt("myTotalExp", @int + num);
	}

	private void CheckChallenges(bool isDead)
	{
		DailyChallengesController.Instance.ChangeChallengeProgression(ChallengeType.playContracts, 1);
		if (PhotonNetwork.PlayerList.Length > 1)
		{
			DailyChallengesController.Instance.ChangeChallengeProgression(ChallengeType.playTogether, 1);
		}
		if (!isDead && !this.itemSpawner.hasSpawnedOtherItems)
		{
			DailyChallengesController.Instance.ChangeChallengeProgression(ChallengeType.completeWithDefaultItems, 1);
		}
		if (LevelController.instance.type == LevelController.levelType.medium)
		{
			DailyChallengesController.Instance.ChangeChallengeProgression(ChallengeType.PlayAMediumMap, 1);
		}
		foreach (Mission mission in MissionManager.instance.currentMissions)
		{
			if (mission.type == Mission.MissionType.main && mission.completed)
			{
				DailyChallengesController.Instance.ChangeChallengeProgression(ChallengeType.DiscoverGhostType, 1);
			}
		}
		if (EvidenceController.instance.foundGhostDNA)
		{
			DailyChallengesController.Instance.ChangeChallengeProgression(ChallengeType.FindDNAEvidence, 1);
		}
	}

	[PunRPC]
	private void SyncPhotoValue(int value)
	{
		PlayerPrefs.SetInt("PhotosMission", value);
		if (value >= 50)
		{
			DailyChallengesController.Instance.ChangeChallengeProgression(ChallengeType.PhotoReward, 1);
		}
	}

	private PhotonView view;

	[HideInInspector]
	public bool isExiting;

	[SerializeField]
	private ExitLevelTrigger trigger;

	[SerializeField]
	private AudioSource source;

	[SerializeField]
	private AudioClip startExitSound;

	[SerializeField]
	private AudioClip stopExitSound;

	[SerializeField]
	private ItemSpawner itemSpawner;
}


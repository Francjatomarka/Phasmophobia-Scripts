using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using Photon.Pun;

// Token: 0x0200017B RID: 379
public class ExitLevel : MonoBehaviour
{
	// Token: 0x06000AC9 RID: 2761 RVA: 0x000439D2 File Offset: 0x00041BD2
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x06000ACA RID: 2762 RVA: 0x000439E0 File Offset: 0x00041BE0
	public void StartAttemptExitLevel()
	{
		base.StopAllCoroutines();
		if (this.isExiting)
		{
			base.StartCoroutine(this.AttemptExitLevel());
		}
	}

	// Token: 0x06000ACB RID: 2763 RVA: 0x00043A00 File Offset: 0x00041C00
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

	// Token: 0x06000ACC RID: 2764 RVA: 0x00043AA1 File Offset: 0x00041CA1
	[PunRPC]
	private void PlayTruckStartUpSound()
	{
		this.source.clip = this.startExitSound;
		this.source.Play();
	}

	// Token: 0x06000ACD RID: 2765 RVA: 0x00043ABF File Offset: 0x00041CBF
	[PunRPC]
	private void PlayTruckStopSound()
	{
		this.source.clip = this.stopExitSound;
		this.source.Play();
	}

	// Token: 0x06000ACE RID: 2766 RVA: 0x00043ADD File Offset: 0x00041CDD
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

	// Token: 0x06000ACF RID: 2767 RVA: 0x00043AEC File Offset: 0x00041CEC
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

	// Token: 0x06000AD0 RID: 2768 RVA: 0x00043BFD File Offset: 0x00041DFD
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

	// Token: 0x06000AD1 RID: 2769 RVA: 0x00043C08 File Offset: 0x00041E08
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

	// Token: 0x06000AD2 RID: 2770 RVA: 0x00043D74 File Offset: 0x00041F74
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

	// Token: 0x06000AD3 RID: 2771 RVA: 0x00043E80 File Offset: 0x00042080
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

	// Token: 0x06000AD4 RID: 2772 RVA: 0x00043F5C File Offset: 0x0004215C
	[PunRPC]
	private void SyncPhotoValue(int value)
	{
		PlayerPrefs.SetInt("PhotosMission", value);
		if (value >= 50)
		{
			DailyChallengesController.Instance.ChangeChallengeProgression(ChallengeType.PhotoReward, 1);
		}
	}

	// Token: 0x04000B2D RID: 2861
	private PhotonView view;

	// Token: 0x04000B2E RID: 2862
	[HideInInspector]
	public bool isExiting;

	// Token: 0x04000B2F RID: 2863
	[SerializeField]
	private ExitLevelTrigger trigger;

	// Token: 0x04000B30 RID: 2864
	[SerializeField]
	private AudioSource source;

	// Token: 0x04000B31 RID: 2865
	[SerializeField]
	private AudioClip startExitSound;

	// Token: 0x04000B32 RID: 2866
	[SerializeField]
	private AudioClip stopExitSound;

	// Token: 0x04000B33 RID: 2867
	[SerializeField]
	private ItemSpawner itemSpawner;
}

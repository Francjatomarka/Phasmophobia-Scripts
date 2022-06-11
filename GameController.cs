using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

// Token: 0x020000CF RID: 207
[RequireComponent(typeof(PhotonView))]
public class GameController : MonoBehaviourPunCallbacks
{
	// Token: 0x060005D6 RID: 1494 RVA: 0x00021E18 File Offset: 0x00020018
	private void Awake()
	{
		GameController.instance = this;
		this.view = base.GetComponent<PhotonView>();
		this.OnPlayerSpawned.AddListener(new UnityAction(this.CheckIfAllPlayersAreConnected));
		this.OnPlayerSpawned.AddListener(new UnityAction(this.CheckConnected));
		if (PlayerPrefs.GetInt("isTutorial") == 0)
		{
			this.levelDifficulty = (Contract.LevelDifficulty)PlayerPrefs.GetInt("LevelDifficulty");
		}
		else
		{
			this.levelDifficulty = Contract.LevelDifficulty.Amateur;
		}
		PlayerPrefs.SetInt("PlayerDied", 0);
	}

	// Token: 0x060005D7 RID: 1495 RVA: 0x00021E98 File Offset: 0x00020098
	public float GetAveragePlayerInsanity()
	{
		float num = 0f;
		for (int i = 0; i < this.playersData.Count; i++)
		{
			num += this.playersData[i].player.insanity;
		}
		this.currentAverageSanity = num / (float)this.playersData.Count;
		return this.currentAverageSanity;
	}

	// Token: 0x060005D8 RID: 1496 RVA: 0x00021EF4 File Offset: 0x000200F4
	public void PlayerDied()
	{
		if (PhotonNetwork.IsMasterClient && this.AllPlayersAreDead())
		{
			base.StartCoroutine(this.ExitLevelAfterDelay());
		}
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x00021F14 File Offset: 0x00020114
	private bool AllPlayersAreDead()
	{
		foreach (PlayerData playerData in this.playersData)
		{
			if (playerData != null && !playerData.player.isDead)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x00021F78 File Offset: 0x00020178
	public void CheckIfAllPlayersAreConnected()
	{
		if (this.allPlayersAreConnected)
		{
			return;
		}
		if (this.playersData.Count == PhotonNetwork.PlayerList.Length)
		{
			this.allPlayersAreConnected = true;
			this.OnAllPlayersConnected.Invoke();
		}
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x00021FA9 File Offset: 0x000201A9
	private void CheckConnected()
	{
		base.StartCoroutine(this.CheckConnectedTimer());
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x00021FB8 File Offset: 0x000201B8
	private IEnumerator CheckConnectedTimer()
	{
		yield return new WaitForSeconds(15f);
		this.CheckIfAllPlayersAreConnected();
		yield break;
	}

    // Token: 0x060005DD RID: 1501 RVA: 0x00021FC8 File Offset: 0x000201C8
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
		if (PhotonNetwork.IsMasterClient)
		{
			this.CheckIfAllPlayersAreConnected();
		}
		for (int i = 0; i < this.playersData.Count; i++)
		{
			if (this.playersData[i].actorID == int.Parse(otherPlayer.UserId))
			{
				this.playersData[i].player.ForceDropAllProps();
				this.playersData.RemoveAt(i);
			}
		}
		if (this.AllPlayersAreDead())
		{
			base.StartCoroutine(this.ExitLevelAfterDelay());
		}
		this.OnPlayerDisconnected.Invoke();
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x00022053 File Offset: 0x00020253
	private IEnumerator ExitLevelAfterDelay()
	{
		yield return new WaitForSeconds((PhotonNetwork.PlayerList.Length == 1) ? 4f : 2f);
		this.view.RPC("Exit", RpcTarget.AllBufferedViaServer, Array.Empty<object>());
		yield break;
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x00022064 File Offset: 0x00020264
	[PunRPC]
	private void Exit()
	{
		this.isLeavingLevel = true;
		PlayerPrefs.SetInt("StayInServerRoom", 1);
		PlayerPrefs.SetInt("MissionStatus", 2);
		PhotonNetwork.LoadLevel("Menu_New");
	}

	// Token: 0x060005E0 RID: 1504 RVA: 0x000220BC File Offset: 0x000202BC
	private void OnApplicationQuit()
	{
		if (Application.isEditor && this.isTutorial)
		{
			PlayerPrefs.SetInt("isTutorial", 0);
			return;
		}
		if (this.isTutorial)
		{
			PlayerPrefs.SetInt("MissionStatus", 3);
			PlayerPrefs.SetInt("setupPhase", 0);
			PlayerPrefs.SetInt("completedTraining", 1);
			PlayerPrefs.SetInt("StayInServerRoom", 0);
			return;
		}
		PlayerPrefs.SetInt("StayInServerRoom", 0);
		PlayerPrefs.SetInt("MissionStatus", 2);
	}

	// Token: 0x04000579 RID: 1401
	public static GameController instance;

	// Token: 0x0400057A RID: 1402
	public PlayerData myPlayer;

	// Token: 0x0400057B RID: 1403
	[HideInInspector]
	public UnityEvent OnAllPlayersConnected = new UnityEvent();

	// Token: 0x0400057C RID: 1404
	[HideInInspector]
	public UnityEvent OnPlayerSpawned = new UnityEvent();

	// Token: 0x0400057D RID: 1405
	[HideInInspector]
	public UnityEvent OnLocalPlayerSpawned = new UnityEvent();

	// Token: 0x0400057E RID: 1406
	[HideInInspector]
	public UnityEvent OnPlayerDisconnected = new UnityEvent();

	// Token: 0x0400057F RID: 1407
	[HideInInspector]
	public UnityEvent OnGhostSpawned = new UnityEvent();

	// Token: 0x04000580 RID: 1408
	[HideInInspector]
	public UnityEvent OnExitLevel = new UnityEvent();

	// Token: 0x04000581 RID: 1409
	public List<PlayerData> playersData = new List<PlayerData>();

	// Token: 0x04000582 RID: 1410
	[HideInInspector]
	public float currentAverageSanity;

	// Token: 0x04000584 RID: 1412
	private PhotonView view;

	// Token: 0x04000585 RID: 1413
	[HideInInspector]
	public bool isLeavingLevel;

	// Token: 0x04000586 RID: 1414
	[HideInInspector]
	public bool isTutorial;

	// Token: 0x04000587 RID: 1415
	[HideInInspector]
	public Contract.LevelDifficulty levelDifficulty;

	// Token: 0x04000588 RID: 1416
	[HideInInspector]
	public bool allPlayersAreConnected;

	[HideInInspector]
	public bool isLoadingBackToMenu;
}

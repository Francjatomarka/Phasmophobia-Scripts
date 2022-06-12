using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

[RequireComponent(typeof(PhotonView))]
public class GameController : MonoBehaviourPunCallbacks
{
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

	public void PlayerDied()
	{
		if (PhotonNetwork.IsMasterClient && this.AllPlayersAreDead())
		{
			base.StartCoroutine(this.ExitLevelAfterDelay());
		}
	}

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

	private void CheckConnected()
	{
		base.StartCoroutine(this.CheckConnectedTimer());
	}

	private IEnumerator CheckConnectedTimer()
	{
		yield return new WaitForSeconds(15f);
		this.CheckIfAllPlayersAreConnected();
		yield break;
	}

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

	private IEnumerator ExitLevelAfterDelay()
	{
		yield return new WaitForSeconds((PhotonNetwork.PlayerList.Length == 1) ? 4f : 2f);
		this.view.RPC("Exit", RpcTarget.AllBufferedViaServer, Array.Empty<object>());
		yield break;
	}

	[PunRPC]
	private void Exit()
	{
		this.isLeavingLevel = true;
		PlayerPrefs.SetInt("StayInServerRoom", 1);
		PlayerPrefs.SetInt("MissionStatus", 2);
		PhotonNetwork.LoadLevel("Menu_New");
	}

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

	public static GameController instance;

	public PlayerData myPlayer;

	[HideInInspector]
	public UnityEvent OnAllPlayersConnected = new UnityEvent();

	[HideInInspector]
	public UnityEvent OnPlayerSpawned = new UnityEvent();

	[HideInInspector]
	public UnityEvent OnLocalPlayerSpawned = new UnityEvent();

	[HideInInspector]
	public UnityEvent OnPlayerDisconnected = new UnityEvent();

	[HideInInspector]
	public UnityEvent OnGhostSpawned = new UnityEvent();

	[HideInInspector]
	public UnityEvent OnExitLevel = new UnityEvent();

	public List<PlayerData> playersData = new List<PlayerData>();

	[HideInInspector]
	public float currentAverageSanity;

	private PhotonView view;

	[HideInInspector]
	public bool isLeavingLevel;

	[HideInInspector]
	public bool isTutorial;

	[HideInInspector]
	public Contract.LevelDifficulty levelDifficulty;

	[HideInInspector]
	public bool allPlayersAreConnected;

	[HideInInspector]
	public bool isLoadingBackToMenu;
}


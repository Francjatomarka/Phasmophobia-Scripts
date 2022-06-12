using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class MultiplayerController : MonoBehaviourPunCallbacks
{
	private void Awake()
	{
		MultiplayerController.instance = this;
		if (!XRDevice.isPresent)
		{
			for (int i = 0; i < this.spawns.Count; i++)
			{
				this.spawns[i].Translate(Vector3.up);
			}
		}
		if (!PhotonNetwork.InRoom && Application.isEditor && !PhotonNetwork.OfflineMode)
		{
			PhotonNetwork.ConnectUsingSettings();
			return;
		}
		this.SpawnPlayer();
	}

	void Start()
    {
		PhotonNetwork.ConnectUsingSettings();
	}

    public override void OnConnectedToMaster()
    {
		PhotonNetwork.JoinLobby();
	}

    public override void OnJoinedLobby()
    {
		RoomOptions roomOptions = new RoomOptions
		{
			IsVisible = false,
			IsOpen = false,
			MaxPlayers = 4
		};
		PhotonNetwork.JoinOrCreateRoom(UnityEngine.Random.Range(1000, 100000).ToString(), roomOptions, null);
	}

    public override void OnCreatedRoom()
    {
		SpawnPlayer();
    }

    private void SpawnPlayer()
	{
		PhotonNetwork.Instantiate("PCPlayer", this.spawns[UnityEngine.Random.Range(0, this.spawns.Count)].position, Quaternion.identity, 0);
		this.sceneCamera.gameObject.SetActive(false);
	}

	private void OnDisconnectedFromPhoton()
	{
		SceneManager.LoadScene("Menu_New", LoadSceneMode.Single);
	}

	private void OnConnectionFail(DisconnectCause cause)
	{
		PlayerPrefs.SetString("ErrorMessage", LocalisationSystem.GetLocalisedValue("Error_Disconnected") + cause);
		PlayerPrefs.SetInt("MissionStatus", 1);
		PlayerPrefs.SetInt("StayInServerRoom", 0);
		PlayerPrefs.SetInt("setupPhase", 1);
	}

	public static MultiplayerController instance;

	public Camera sceneCamera;

	public List<Transform> spawns = new List<Transform>();
}


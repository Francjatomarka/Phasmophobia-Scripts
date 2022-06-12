using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.XR;

public class TrainingManager : MonoBehaviourPunCallbacks
{
	private void Start()
	{
		if (PhotonNetwork.IsConnected)
		{
			CreateTrainningRoom();
			return;
		}
        else
        {
			PhotonNetwork.ConnectUsingSettings();
        }
	}

    public override void OnConnectedToMaster()
    {
		CreateTrainningRoom();
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
		PhotonNetwork.OfflineMode = true;
		base.OnDisconnected(cause);
    }

	public void CreateTrainningRoom()
	{
		RoomOptions roomOptions = new RoomOptions
		{
			IsOpen = false,
			IsVisible = false,
			MaxPlayers = 1,
			PlayerTtl = 10000,
			EmptyRoomTtl = 10000
		};
		PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.NickName + "Training#" + UnityEngine.Random.Range(0f, 10000f).ToString(), roomOptions, null);
	}

	public override void OnCreatedRoom()
	{
		Hashtable propertiesToSet = new Hashtable
		{
			{
				"GameStarted",
				true
			}
		};
		PhotonNetwork.CurrentRoom.SetCustomProperties(propertiesToSet, null, null);
	}

	public override void OnJoinedRoom()
	{
		PlayerPrefs.SetInt("isTutorial", 1);
		PlayerPrefs.SetInt("isInGame", 1);
		this.loadingScreen.SetActive(true);
		this.mainScreen.SetActive(false);
		PhotonNetwork.IsMessageQueueRunning = false;
		this.loadingAsyncManager.LoadScene("SplashScreen_Headphones");
	}

	[SerializeField]
	private GameObject loadingScreen;

	[SerializeField]
	private GameObject mainScreen;

	[SerializeField]
	private LoadingAsyncManager loadingAsyncManager;
}


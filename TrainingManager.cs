using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x0200014D RID: 333
public class TrainingManager : MonoBehaviourPunCallbacks
{
	// Token: 0x06000961 RID: 2401 RVA: 0x00039F8F File Offset: 0x0003818F
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

    // Token: 0x06000962 RID: 2402 RVA: 0x000395F4 File Offset: 0x000377F4
    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
		PhotonNetwork.OfflineMode = true;
		base.OnDisconnected(cause);
    }

	// Token: 0x06000963 RID: 2403 RVA: 0x00039FA4 File Offset: 0x000381A4
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

	// Token: 0x06000964 RID: 2404 RVA: 0x0003A050 File Offset: 0x00038250
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

	// Token: 0x06000965 RID: 2405 RVA: 0x0003A084 File Offset: 0x00038284
	public override void OnJoinedRoom()
	{
		PlayerPrefs.SetInt("isTutorial", 1);
		PlayerPrefs.SetInt("isInGame", 1);
		this.loadingScreen.SetActive(true);
		this.mainScreen.SetActive(false);
		PhotonNetwork.IsMessageQueueRunning = false;
		this.loadingAsyncManager.LoadScene("SplashScreen_Headphones");
	}

	// Token: 0x040009A1 RID: 2465
	[SerializeField]
	private GameObject loadingScreen;

	// Token: 0x040009A2 RID: 2466
	[SerializeField]
	private GameObject mainScreen;

	// Token: 0x040009A4 RID: 2468
	[SerializeField]
	private LoadingAsyncManager loadingAsyncManager;
}

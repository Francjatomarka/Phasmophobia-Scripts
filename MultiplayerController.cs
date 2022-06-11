using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

// Token: 0x020000DA RID: 218
public class MultiplayerController : MonoBehaviourPunCallbacks
{
	// Token: 0x0600062B RID: 1579 RVA: 0x00024930 File Offset: 0x00022B30
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

    // Token: 0x0600062F RID: 1583 RVA: 0x000249FC File Offset: 0x00022BFC
    private void SpawnPlayer()
	{
		PhotonNetwork.Instantiate("PCPlayer", this.spawns[UnityEngine.Random.Range(0, this.spawns.Count)].position, Quaternion.identity, 0);
		this.sceneCamera.gameObject.SetActive(false);
	}

	// Token: 0x06000630 RID: 1584 RVA: 0x00024A5A File Offset: 0x00022C5A
	private void OnDisconnectedFromPhoton()
	{
		SceneManager.LoadScene("Menu_New", LoadSceneMode.Single);
	}

	// Token: 0x06000631 RID: 1585 RVA: 0x00024A94 File Offset: 0x00022C94
	private void OnConnectionFail(DisconnectCause cause)
	{
		PlayerPrefs.SetString("ErrorMessage", LocalisationSystem.GetLocalisedValue("Error_Disconnected") + cause);
		PlayerPrefs.SetInt("MissionStatus", 1);
		PlayerPrefs.SetInt("StayInServerRoom", 0);
		PlayerPrefs.SetInt("setupPhase", 1);
	}

	// Token: 0x040005FA RID: 1530
	public static MultiplayerController instance;

	// Token: 0x040005FB RID: 1531
	public Camera sceneCamera;

	// Token: 0x040005FC RID: 1532
	public List<Transform> spawns = new List<Transform>();
}

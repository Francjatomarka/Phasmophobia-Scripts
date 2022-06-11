using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

// Token: 0x02000182 RID: 386
public class LobbyManager : MonoBehaviourPunCallbacks
{
	// Token: 0x06000A57 RID: 2647 RVA: 0x00040590 File Offset: 0x0003E790
	private void Start()
	{
		this.regionDropdown.value = PlayerPrefs.GetInt("currentRegionID");
		if (this.regionDropdown.value == 0)
		{
			this.hasSetInitialRegion = true;
		}
		this.RefreshList();
		MenuAudio.instance.LobbyScreen(PhotonNetwork.PlayerList.Length);
	}

	// Token: 0x06000A58 RID: 2648 RVA: 0x000405DD File Offset: 0x0003E7DD
	private void Update()
	{
		/*if (this.rooms.Length == 0)
		{
			this.refreshTimer -= Time.deltaTime;
			if (this.refreshTimer < 0f)
			{
				this.RefreshList();
				this.refreshTimer = 5f;
			}
		}*/
	}

	// Token: 0x06000A59 RID: 2649 RVA: 0x00040618 File Offset: 0x0003E818
	public void RefreshList()
	{
		if (PhotonNetwork.OfflineMode)
		{
			this.populationText.text = LocalisationSystem.GetLocalisedValue("Lobby_Population") + ": " + LocalisationSystem.GetLocalisedValue("Menu_Offline");
			this.roomsText.text = LocalisationSystem.GetLocalisedValue("Lobby_Rooms") + ": " + LocalisationSystem.GetLocalisedValue("Menu_Offline");
		}
		else
		{
			this.populationText.text = LocalisationSystem.GetLocalisedValue("Lobby_Population") + ": " + PhotonNetwork.CountOfPlayers.ToString();
			this.roomsText.text = LocalisationSystem.GetLocalisedValue("Lobby_Rooms") + ": " + PhotonNetwork.CountOfRooms.ToString();
		}
		this.pageText.text = this.pageIndex / 4 + " / " + (1 / 4 + 1);
		this.leftButton.SetActive(false);
		this.rightButton.SetActive(false);
		if (1 > 4)
		{
			this.rightButton.SetActive(true);
		}
		this.pageIndex = 4;
		this.SetRooms();
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x00040754 File Offset: 0x0003E954
	public void PreviousPageButton()
	{
		this.pageIndex -= 4;
		//this.pageText.text = this.pageIndex / 4 + " / " + (this.rooms.Length / 4 + 1);
		if (this.pageIndex == 4)
		{
			this.leftButton.SetActive(false);
		}
		/*if (this.rooms.Length > this.pageIndex)
		{
			this.rightButton.SetActive(true);
		}*/
		this.SetRooms();
	}

	// Token: 0x06000A5B RID: 2651 RVA: 0x000407DC File Offset: 0x0003E9DC
	public void NextPageButton()
	{
		this.pageIndex += 4;
		/*if (this.pageIndex / 4 > this.rooms.Length / 4)
		{
			this.pageText.text = this.pageIndex / 4 + " / " + this.pageIndex / 4;
		}
		else
		{
			this.pageText.text = this.pageIndex / 4 + " / " + (this.rooms.Length / 4 + 1);
		}*/
		if (this.pageIndex > 4)
		{
			this.leftButton.SetActive(true);
		}
		/*if (this.rooms.Length < this.pageIndex)
		{
			this.rightButton.SetActive(false);
		}*/
		this.SetRooms();
	}

	// Token: 0x06000A5C RID: 2652 RVA: 0x000408A8 File Offset: 0x0003EAA8
	public void SetRooms()
	{
		/*this.serverItem1.gameObject.SetActive(false);
		this.serverItem2.gameObject.SetActive(false);
		this.serverItem3.gameObject.SetActive(false);
		this.serverItem4.gameObject.SetActive(false);
		if (this.rooms.Length == 0)
		{
			return;
		}
		if (this.rooms.Length >= this.pageIndex - 3)
		{
			this.serverItem1.gameObject.SetActive(true);
			this.serverItem1.SetUI(this.rooms[this.pageIndex - 4].Name, this.rooms[this.pageIndex - 4].PlayerCount.ToString(), this.rooms[this.pageIndex - 4]);
		}
		if (this.rooms.Length >= this.pageIndex - 2)
		{
			this.serverItem2.gameObject.SetActive(true);
			this.serverItem2.SetUI(this.rooms[this.pageIndex - 3].Name, this.rooms[this.pageIndex - 3].PlayerCount.ToString(), this.rooms[this.pageIndex - 3]);
		}
		if (this.rooms.Length >= this.pageIndex - 1)
		{
			this.serverItem3.gameObject.SetActive(true);
			this.serverItem3.SetUI(this.rooms[this.pageIndex - 2].Name, this.rooms[this.pageIndex - 2].PlayerCount.ToString(), this.rooms[this.pageIndex - 2]);
		}
		if (this.rooms.Length >= this.pageIndex)
		{
			this.serverItem4.gameObject.SetActive(true);
			this.serverItem4.SetUI(this.rooms[this.pageIndex - 1].Name, this.rooms[this.pageIndex - 1].PlayerCount.ToString(), this.rooms[this.pageIndex - 1]);
		}*/
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x00040AB8 File Offset: 0x0003ECB8
	public void JoinServer(RoomInfo info)
	{
		if (!info.IsOpen || info.PlayerCount == 4 || !info.IsVisible)
		{
			this.RefreshList();
			return;
		}
		if (XRDevice.isPresent)
		{
			this.SaveVRPlayerPositions();
		}
		else
		{
			this.SavePCPlayerPositions();
		}
		PhotonNetwork.JoinRoom(info.Name);
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x00040B06 File Offset: 0x0003ED06
	public void JoinServerByName(string serverName)
	{
		if (XRDevice.isPresent)
		{
			this.SaveVRPlayerPositions();
		}
		else
		{
			this.SavePCPlayerPositions();
		}
		PhotonNetwork.JoinRoom(serverName);
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		passwordManager.ErrorJoiningRoom();
	}

    // Token: 0x06000A60 RID: 2656 RVA: 0x00040B44 File Offset: 0x0003ED44
    public void CreateServer(bool isPrivate)
	{
		if (XRDevice.isPresent)
		{
			this.SaveVRPlayerPositions();
		}
		else
		{
			this.SavePCPlayerPositions();
		}
		PlayerPrefs.SetInt("isPublicServer", isPrivate ? 0 : 1);
		PhotonNetwork.NickName = "DanielTesting";
		RoomOptions roomOptions = new RoomOptions
		{
			IsOpen = true,
			IsVisible = !isPrivate,
			MaxPlayers = 4,
			PlayerTtl = 2000
		};
		PhotonNetwork.CreateRoom(UnityEngine.Random.Range(0, 999999).ToString("000000"), roomOptions, null);
	}

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
    }

    // Token: 0x06000A61 RID: 2657 RVA: 0x00040C00 File Offset: 0x0003EE00
    public override void OnJoinedLobby()
	{
		this.RefreshList();
	}

	// Token: 0x06000A62 RID: 2658 RVA: 0x00040C08 File Offset: 0x0003EE08
	public override void OnJoinedRoom()
	{
		if (this.mainManager.localPlayer)
		{
			Destroy(this.mainManager.localPlayer.gameObject);
		}
		base.StartCoroutine(this.OnJoinedRoomDelay());
	}

    // Token: 0x06000A63 RID: 2659 RVA: 0x00040C3E File Offset: 0x0003EE3E
    private IEnumerator OnJoinedRoomDelay()
	{
		this.ServerManager.SetActive(true);
		for (int i = 0; i < this.lobbyObjects.Length; i++)
		{
			this.lobbyObjects[i].SetActive(false);
		}
		yield return new WaitForEndOfFrame();
		base.gameObject.SetActive(false);
		if (XRDevice.isPresent)
		{
			this.mainManager.localPlayer = PhotonNetwork.Instantiate("VRPlayer", this.mainManager.spawns[UnityEngine.Random.Range(0, this.mainManager.spawns.Count)].position, Quaternion.identity, 0).GetComponent<Player>();
		}
		else
		{
			this.mainManager.localPlayer = PhotonNetwork.Instantiate("PCPlayer", new Vector3(this.pcPlayerXPos, this.pcPlayerYPos, this.pcPlayerZPos), this.pcPlayerRotation, 0).GetComponent<Player>();
		}
		yield break;
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x00040C50 File Offset: 0x0003EE50
	private void SaveVRPlayerPositions()
	{
		this.steamVRXPos = this.mainManager.localPlayer.steamVRObj.position.x;
		this.steamVRYPos = this.mainManager.localPlayer.steamVRObj.position.y;
		this.steamVRZPos = this.mainManager.localPlayer.steamVRObj.position.z;
		this.steamVRRotation = this.mainManager.localPlayer.steamVRObj.rotation;
		this.vrikXPos = this.mainManager.localPlayer.VRIKObj.position.x;
		this.vrikYPos = this.mainManager.localPlayer.VRIKObj.position.y;
		this.vrikZPos = this.mainManager.localPlayer.VRIKObj.position.z;
		this.vrikRotation = this.mainManager.localPlayer.VRIKObj.rotation;
	}

	// Token: 0x06000A65 RID: 2661 RVA: 0x00040D54 File Offset: 0x0003EF54
	private void SavePCPlayerPositions()
	{
		if (this.mainManager.localPlayer == null && FindObjectOfType<Player>() != null)
		{
			this.mainManager.localPlayer = FindObjectOfType<Player>();
		}
		if (this.mainManager.localPlayer)
		{
			this.pcPlayerXPos = this.mainManager.localPlayer.transform.position.x;
			this.pcPlayerYPos = this.mainManager.localPlayer.transform.position.y;
			this.pcPlayerZPos = this.mainManager.localPlayer.transform.position.z;
			this.pcPlayerRotation = this.mainManager.localPlayer.transform.rotation;
		}
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x00040E20 File Offset: 0x0003F020
	public void ChangeRegion()
	{
		if (PlayerPrefs.HasKey("isCrackedVersion") && PlayerPrefs.GetInt("isCrackedVersion") == 1)
		{
			return;
		}
		if (!this.hasSetInitialRegion)
		{
			this.hasSetInitialRegion = true;
			return;
		}
		if (PhotonNetwork.OfflineMode)
		{
			return;
		}
		PlayerPrefs.SetInt("currentRegionID", this.regionDropdown.value);
		this.isChangingRegion = true;
		PhotonNetwork.Disconnect();
	}

    // Token: 0x06000A67 RID: 2663 RVA: 0x00040ED0 File Offset: 0x0003F0D0
    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
	{
		if (!this.isChangingRegion)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		else
		{
			PhotonNetwork.ConnectUsingSettings();
		}
		this.isChangingRegion = false;
	}

	// Token: 0x06000A68 RID: 2664 RVA: 0x0002D981 File Offset: 0x0002BB81
	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby();
	}

	[SerializeField]
	private PasswordManager passwordManager;

	// Token: 0x04000A83 RID: 2691
	[SerializeField]
	private Text populationText;

	// Token: 0x04000A84 RID: 2692
	[SerializeField]
	private Text roomsText;

	// Token: 0x04000A85 RID: 2693
	[SerializeField]
	private ServerListItem serverItem1;

	// Token: 0x04000A86 RID: 2694
	[SerializeField]
	private ServerListItem serverItem2;

	// Token: 0x04000A87 RID: 2695
	[SerializeField]
	private ServerListItem serverItem3;

	// Token: 0x04000A88 RID: 2696
	[SerializeField]
	private ServerListItem serverItem4;

	// Token: 0x04000A89 RID: 2697
	public GameObject ServerManager;

	// Token: 0x04000A8A RID: 2698
	[SerializeField]
	private MainManager mainManager;

	// Token: 0x04000A8B RID: 2699
	[SerializeField]
	private StoreSDKManager storeSDKManager;

	// Token: 0x04000A8C RID: 2700
	[SerializeField]
	private Dropdown regionDropdown;

	// Token: 0x04000A8D RID: 2701
	private bool hasSetInitialRegion;

	// Token: 0x04000A8E RID: 2702
	private bool isChangingRegion;

	// Token: 0x04000A8F RID: 2703
	[SerializeField]
	private PCManager pcManager;

	// Token: 0x04000A90 RID: 2704
	private float steamVRXPos;

	// Token: 0x04000A91 RID: 2705
	private float steamVRYPos;

	// Token: 0x04000A92 RID: 2706
	private float steamVRZPos;

	// Token: 0x04000A93 RID: 2707
	private Quaternion steamVRRotation;

	// Token: 0x04000A94 RID: 2708
	private float vrikXPos;

	// Token: 0x04000A95 RID: 2709
	private float vrikYPos;

	// Token: 0x04000A96 RID: 2710
	private float vrikZPos;

	// Token: 0x04000A97 RID: 2711
	private Quaternion vrikRotation;

	// Token: 0x04000A98 RID: 2712
	private float pcPlayerXPos;

	// Token: 0x04000A99 RID: 2713
	private float pcPlayerYPos;

	// Token: 0x04000A9A RID: 2714
	private float pcPlayerZPos;

	// Token: 0x04000A9B RID: 2715
	private Quaternion pcPlayerRotation;

	// Token: 0x04000A9D RID: 2717
	//public RoomInfo[] rooms;

	// Token: 0x04000A9E RID: 2718
	[SerializeField]
	private GameObject leftButton;

	// Token: 0x04000A9F RID: 2719
	[SerializeField]
	private GameObject rightButton;

	// Token: 0x04000AA0 RID: 2720
	[SerializeField]
	private Text pageText;

	// Token: 0x04000AA1 RID: 2721
	private int pageIndex = 4;

	// Token: 0x04000AA2 RID: 2722
	private float refreshTimer = 5f;

	// Token: 0x04000AA3 RID: 2723
	[SerializeField]
	private GameObject[] lobbyObjects;
}

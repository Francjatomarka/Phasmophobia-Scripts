using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class LobbyManager : MonoBehaviourPunCallbacks
{
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

    public override void OnJoinedLobby()
	{
		this.RefreshList();
	}

	public override void OnJoinedRoom()
	{
		if (this.mainManager.localPlayer)
		{
			Destroy(this.mainManager.localPlayer.gameObject);
		}
		base.StartCoroutine(this.OnJoinedRoomDelay());
	}

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

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby();
	}

	[SerializeField]
	private PasswordManager passwordManager;

	[SerializeField]
	private Text populationText;

	[SerializeField]
	private Text roomsText;

	[SerializeField]
	private ServerListItem serverItem1;

	[SerializeField]
	private ServerListItem serverItem2;

	[SerializeField]
	private ServerListItem serverItem3;

	[SerializeField]
	private ServerListItem serverItem4;

	public GameObject ServerManager;

	[SerializeField]
	private MainManager mainManager;

	[SerializeField]
	private StoreSDKManager storeSDKManager;

	[SerializeField]
	private Dropdown regionDropdown;

	private bool hasSetInitialRegion;

	private bool isChangingRegion;

	[SerializeField]
	private PCManager pcManager;

	private float steamVRXPos;

	private float steamVRYPos;

	private float steamVRZPos;

	private Quaternion steamVRRotation;

	private float vrikXPos;

	private float vrikYPos;

	private float vrikZPos;

	private Quaternion vrikRotation;

	private float pcPlayerXPos;

	private float pcPlayerYPos;

	private float pcPlayerZPos;

	private Quaternion pcPlayerRotation;

	//public RoomInfo[] rooms;

	[SerializeField]
	private GameObject leftButton;

	[SerializeField]
	private GameObject rightButton;

	[SerializeField]
	private Text pageText;

	private int pageIndex = 4;

	private float refreshTimer = 5f;

	[SerializeField]
	private GameObject[] lobbyObjects;
}


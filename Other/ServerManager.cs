using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

// Token: 0x02000149 RID: 329
public class ServerManager : MonoBehaviourPunCallbacks
{
	// Token: 0x06000935 RID: 2357 RVA: 0x00038BB7 File Offset: 0x00036DB7
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.inventoryManager = base.GetComponent<InventoryManager>();
		this.myCharacterIndex = PlayerPrefs.GetInt("CharacterIndex");
		this.inviteCodeText.text = "??????";
		this.UpdateUI();
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x00038BE8 File Offset: 0x00036DE8
	private void Start()
	{
		this.jobFinderButton.interactable = PhotonNetwork.IsMasterClient;
		this.UpdateUI();
		base.Invoke("AssignAllPlayerInfoDelay", 3f);
		this.publicPrivateText.text = LocalisationSystem.GetLocalisedValue("Server_Public");
		PlayerPrefs.SetString("ServerName", PhotonNetwork.CurrentRoom.Name);
		if (PhotonNetwork.IsMasterClient)
		{
			this.isPublicServer = (PlayerPrefs.GetInt("isPublicServer") == 1);
			if (!this.isPublicServer)
			{
				PhotonNetwork.CurrentRoom.IsOpen = true;
				PhotonNetwork.CurrentRoom.IsVisible = false;
			}
		}
		if (!PhotonNetwork.CurrentRoom.IsVisible)
		{
			this.publicPrivateText.text = LocalisationSystem.GetLocalisedValue("Server_Private");
		}
	}

	// Token: 0x06000937 RID: 2359 RVA: 0x00038CC8 File Offset: 0x00036EC8
	private void AssignAllPlayerInfoDelay()
	{
		for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
		{
			AssignNewPlayerSpot(PhotonNetwork.PlayerList[i]);
		}
		this.UpdateUI();
	}

	// Token: 0x06000938 RID: 2360 RVA: 0x00038CFC File Offset: 0x00036EFC
	public void EnableOrDisablePlayerModels(bool active)
	{
		if (XRDevice.isPresent)
		{
			return;
		}
		if (active)
		{
			for (int i = 0; i < this.players.Count; i++)
			{
				if (this.players[i].myPlayer != null)
				{
					this.players[i].myPlayer.characterModels[this.players[i].myPlayer.modelID].SetActive(true);
				}
			}
			return;
		}
		for (int j = 0; j < this.players.Count; j++)
		{
			if (this.players[j].myPlayer != null)
			{
				for (int k = 0; k < this.players[j].myPlayer.characterModels.Length; k++)
				{
					this.players[j].myPlayer.characterModels[k].SetActive(false);
				}
			}
		}
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x00038DE7 File Offset: 0x00036FE7
	private IEnumerator AssignPlayerInfoDelay(Photon.Realtime.Player player)
	{
		yield return new WaitForSeconds(1f);
		this.AssignNewPlayerSpot(player);
		this.UpdateUI();
		yield break;
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x00038DFD File Offset: 0x00036FFD
	private void OnEnable()
	{
		this.UpdateUI();
		this.EnableMasks(true);
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x00038E0C File Offset: 0x0003700C
	public void EnableMasks(bool active)
	{
		this.mainMask.sizeDelta = new Vector2((float)(active ? 1200 : 0), (float)(active ? 1200 : 0));
		this.serverMask.sizeDelta = new Vector2((float)(active ? 1200 : 0), (float)(active ? 1200 : 0));
	}

	// Token: 0x0600093C RID: 2364 RVA: 0x00038E69 File Offset: 0x00037069
	public void KickPlayer(int id)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			this.view.RPC("LeaveServer", this.players[id].player, new object[]
			{
				true
			});
		}
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x00038EA4 File Offset: 0x000370A4
	public void UpdateUI()
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			this.players[i].playerReady = false;
		}
		for (int j = 0; j < this.serverItems.Count; j++)
		{
			this.serverItems[j].gameObject.SetActive(false);
		}
		for (int k = 0; k < this.kickPlayerButtons.Length; k++)
		{
			this.kickPlayerButtons[k].interactable = false;
		}
		for (int l = 0; l < this.players.Count; l++)
		{
			this.serverItems[l].gameObject.SetActive(true);
			this.serverItems[l].playerName.text = this.players[l].player.NickName;
			if (this.players[l].level != 0)
			{
				this.serverItems[l].playerLevel.text = LocalisationSystem.GetLocalisedValue("Experience_Level") + ": " + this.players[l].level;
			}
			this.serverItems[l].playerReadyText.text = LocalisationSystem.GetLocalisedValue("Server_Unready");
			this.serverItems[l].playerIcon.sprite = this.characterIcons[this.players[l].playerCharacterIndex];
			if (this.players[l].player != PhotonNetwork.LocalPlayer && PhotonNetwork.IsMasterClient)
			{
				this.kickPlayerButtons[l].interactable = true;
			}
		}
		this.startGameButton.interactable = false;
		this.readyText.color = new Color32(50, 50, 50, byte.MaxValue);
		if (PhotonNetwork.IsMasterClient)
		{
			this.selectJobText.color = new Color32(50, 50, 50, byte.MaxValue);
		}
		else
		{
			this.selectJobText.color = this.disabledColour;
		}
		if (this.levelSelectionManager.selectedLevelName != string.Empty)
		{
			this.readyButton.interactable = true;
			this.readyText.color = new Color32(50, 50, 50, byte.MaxValue);
			this.levelSelectionText.text = LocalisationSystem.GetLocalisedValue("Map_Contract") + ": " + this.levelSelectionManager.contractLevelName;
			if (this.levelSelectionManager.contractLevelDifficulty == Contract.LevelDifficulty.Amateur)
			{
				this.difficultyText.text = LocalisationSystem.GetLocalisedValue("Contract_Amateur");
				return;
			}
			if (this.levelSelectionManager.contractLevelDifficulty == Contract.LevelDifficulty.Intermediate)
			{
				this.difficultyText.text = LocalisationSystem.GetLocalisedValue("Contract_Intermediate");
				return;
			}
			this.difficultyText.text = LocalisationSystem.GetLocalisedValue("Contract_Professional");
		}
	}

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
		if (PhotonNetwork.IsMasterClient && this.levelSelectionManager.selectedContract != null)
		{
			this.levelSelectionManager.SyncContract();
		}
		this.AssignNewPlayerSpot(newPlayer);
		this.jobFinderButton.interactable = PhotonNetwork.IsMasterClient;
		this.UpdateUI();
		base.StartCoroutine(this.AssignPlayerInfoDelay(newPlayer));
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x000391D8 File Offset: 0x000373D8
	public override void OnDisable()
	{
		this.players.Clear();
		for (int i = 0; i < this.serverItems.Count; i++)
		{
			this.serverItems[i].gameObject.SetActive(false);
		}
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		base.OnJoinRoomFailed(returnCode, message);
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x0003921D File Offset: 0x0003741D
	public override void OnPlayerLeftRoom(Photon.Realtime.Player player)
	{
		this.RemovePlayerSpot(player);
		this.jobFinderButton.interactable = PhotonNetwork.IsMasterClient;
		this.UpdateUI();
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x0003923C File Offset: 0x0003743C
	private void AssignNewPlayerSpot(Photon.Realtime.Player photonPlayer)
	{
		if (PhotonNetwork.LocalPlayer == photonPlayer && MainManager.instance != null && MainManager.instance.localPlayer != null)
		{
			this.view.RPC("SetPlayerInformation", RpcTarget.AllBufferedViaServer, new object[]
			{
				photonPlayer,
				this.myCharacterIndex,
				Mathf.FloorToInt((float)(PlayerPrefs.GetInt("myTotalExp") / 100)),
				MainManager.instance.localPlayer.view.ViewID
			});
		}
		this.UpdateUI();
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x000392D8 File Offset: 0x000374D8
	[PunRPC]
	public void SetPlayerInformation(Photon.Realtime.Player photonPlayer, int index, int level, int playerViewID)
	{
		bool flag = false;
		/*for (int i = 0; i < this.players.Count; i++)
		{
			if (this.players[i].player == photonPlayer)
			{
				flag = true;
			}
		}*/
		if (!flag)
		{
			PlayerServerSpot playerServerSpot = new PlayerServerSpot
			{
				playerReady = false,
				player = photonPlayer
			};
			if (PhotonView.Find(playerViewID) != null && PhotonView.Find(playerViewID).GetComponent<Player>() != null)
			{
				playerServerSpot.myPlayer = PhotonView.Find(playerViewID).GetComponent<Player>();
			}
			this.players.Add(playerServerSpot);
		}
		else
		{
			for (int j = 0; j < this.players.Count; j++)
			{
				if (this.players[j].player == photonPlayer)
				{
					PlayerServerSpot playerServerSpot2 = new PlayerServerSpot
					{
						player = photonPlayer
					};
					if (playerServerSpot2.myPlayer == null)
					{
						playerServerSpot2.myPlayer = PhotonView.Find(playerViewID).GetComponent<Player>();
					}
					this.players[j] = playerServerSpot2;
				}
			}
		}
		for (int k = 0; k < this.players.Count; k++)
		{
			if (this.players[k].player == photonPlayer)
			{
				this.players[k].playerCharacterIndex = index;
				this.serverItems[k].playerIcon.sprite = this.characterIcons[index];
				this.serverItems[k].playerName.text = photonPlayer.NickName;
				this.serverItems[k].playerLevel.text = "Level: " + level.ToString();
				this.players[k].level = level;
				break;
			}
		}
		this.UpdateUI();
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x00039454 File Offset: 0x00037654
	private void RemovePlayerSpot(Photon.Realtime.Player player)
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			if (this.players[i].player == player)
			{
				this.serverItems[i].playerName.text = "";
				this.serverItems[i].playerReadyText.text = LocalisationSystem.GetLocalisedValue("Server_Unready");
				this.serverItems[i].playerIcon.sprite = this.characterIcons[0];
				this.serverItems[i].gameObject.SetActive(false);
				this.players.RemoveAt(i);
			}
		}
		this.UpdateUI();
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x00039513 File Offset: 0x00037713
	public void Ready()
	{
		this.view.RPC("NetworkedReady", RpcTarget.AllBufferedViaServer, new object[]
		{
			PhotonNetwork.LocalPlayer
		});
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x00039534 File Offset: 0x00037734
	public void StartGame()
	{
		if (this.levelSelectionManager.selectedLevelName != string.Empty)
		{
			this.LoadScene(this.levelSelectionManager.selectedLevelName);
		}
		
	}

    // Token: 0x06000946 RID: 2374 RVA: 0x000395F4 File Offset: 0x000377F4

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
		PhotonNetwork.OfflineMode = true;
		base.OnDisconnected(cause);
    }

    // Token: 0x06000947 RID: 2375 RVA: 0x000395FC File Offset: 0x000377FC
    public override void OnConnectedToMaster()
	{
		if (PhotonNetwork.OfflineMode)
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
	}

	public void HideCodeButton()
	{
		if (inviteCodeText.text.Contains("?"))
		{
			inviteCodeText.text = PhotonNetwork.CurrentRoom.Name;
			PlayerPrefs.SetInt("inviteCodeHidden", 0);
		}
		else
		{
			inviteCodeText.text = "??????";
			PlayerPrefs.SetInt("inviteCodeHidden", 1);
		}
	}

	// Token: 0x06000948 RID: 2376 RVA: 0x000396B0 File Offset: 0x000378B0
	public override void OnCreatedRoom()
	{
		if (PhotonNetwork.OfflineMode)
		{
			this.view.RPC("LoadScene", RpcTarget.AllBufferedViaServer, new object[]
			{
				this.levelSelectionManager.selectedLevelName
			});
		}
	}

	// Token: 0x06000949 RID: 2377 RVA: 0x000396E0 File Offset: 0x000378E0
	public void ChangeCharacterButton()
	{
		this.myCharacterIndex++;
		if (this.myCharacterIndex >= this.characterIcons.Length)
		{
			this.myCharacterIndex = 0;
		}
		this.view.RPC("UpdateCharacter", RpcTarget.AllBufferedViaServer, new object[]
		{
			PhotonNetwork.LocalPlayer,
			this.myCharacterIndex
		});
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x00039740 File Offset: 0x00037940
	public void OpenStore(bool active)
	{
		this.storeObject.SetActive(active);
		this.serverMask.sizeDelta = new Vector2((float)(active ? 0 : 1200), (float)(active ? 0 : 1200));
		if (!active)
		{
			for (int i = 0; i < this.inventoryManager.items.Count; i++)
			{
				this.inventoryManager.items[i].UpdateTotalText();
			}
		}
	}

	// Token: 0x0600094B RID: 2379 RVA: 0x000397B8 File Offset: 0x000379B8
	public void SelectJob(bool active)
	{
		this.contractSelectionObject.SetActive(active);
		this.mainMask.sizeDelta = new Vector2((float)(active ? 0 : 1200), (float)(active ? 0 : 1200));
	}

	// Token: 0x0600094C RID: 2380 RVA: 0x00039813 File Offset: 0x00037A13
	public void ChangePublicPrivateType()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			this.view.RPC("ChangePublicPrivateTypeNetworked", RpcTarget.AllBuffered, new object[]
			{
				!this.isPublicServer
			});
		}
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x00039844 File Offset: 0x00037A44
	[PunRPC]
	private void ChangePublicPrivateTypeNetworked(bool _isPublicServer)
	{
		this.isPublicServer = _isPublicServer;
		if (this.isPublicServer)
		{
			if (PhotonNetwork.IsMasterClient)
			{
				PhotonNetwork.CurrentRoom.IsOpen = true;
				PhotonNetwork.CurrentRoom.IsVisible = true;
			}
			this.publicPrivateText.text = LocalisationSystem.GetLocalisedValue("Server_Public");
		}
		else
		{
			if (PhotonNetwork.IsMasterClient)
			{
				PhotonNetwork.CurrentRoom.IsOpen = false;
				PhotonNetwork.CurrentRoom.IsVisible = false;
			}
			this.publicPrivateText.text = LocalisationSystem.GetLocalisedValue("Server_Private");
		}
		PlayerPrefs.SetInt("isPublicServer", this.isPublicServer ? 1 : 0);
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x000398DC File Offset: 0x00037ADC
	[PunRPC]
	private void UpdateCharacter(Photon.Realtime.Player player, int characterIndex)
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			if (this.players[i].player == player)
			{
				this.players[i].playerCharacterIndex = characterIndex;
				if (this.players[i].playerCharacterIndex >= this.characterIcons.Length)
				{
					this.players[i].playerCharacterIndex = 0;
				}
				this.serverItems[i].playerIcon.sprite = this.characterIcons[characterIndex];
			}
		}
		this.UpdateUI();
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x0003997C File Offset: 0x00037B7C
	[PunRPC]
	private void NetworkedReady(Photon.Realtime.Player player)
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			if (this.players[i].player == player)
			{
				this.players[i].playerReady = !this.players[i].playerReady;
				this.serverItems[i].playerReadyText.text = (this.players[i].playerReady ? LocalisationSystem.GetLocalisedValue("Server_Ready") : LocalisationSystem.GetLocalisedValue("Server_Unready"));
			}
		}
		if (PhotonNetwork.IsMasterClient)
		{
			this.startGameButton.interactable = true;
			this.startGameText.color = new Color32(50, 50, 50, byte.MaxValue);
			for (int j = 0; j < this.players.Count; j++)
			{
				if (!this.players[j].playerReady)
				{
					this.startGameButton.interactable = false;
					this.startGameText.color = this.disabledColour;
				}
			}
		}
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x00039A84 File Offset: 0x00037C84
	[PunRPC]
	private void LoadScene(string levelToLoad)
	{
		PhotonNetwork.IsMessageQueueRunning = false;
		this.inventoryManager.SaveInventory();
		PlayerPrefs.SetInt("CharacterIndex", this.myCharacterIndex);
		PlayerPrefs.SetInt("isInGame", 1);
		if (PhotonNetwork.IsMessageQueueRunning)
		{
			PhotonNetwork.CurrentRoom.IsOpen = false;
			PhotonNetwork.CurrentRoom.IsVisible = false;
			PhotonNetwork.CurrentRoom.MaxPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
		}
		this.loadingScreen.SetActive(true);
		base.gameObject.SetActive(false);
		this.storeObject.SetActive(false);
		this.loadingAsyncManager.LoadScene(levelToLoad);
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x00039B40 File Offset: 0x00037D40
	[PunRPC]
	public void LeaveServer(bool isKicked)
	{
		if (isKicked)
		{
			PlayerPrefs.SetString("ErrorMessage", "You were kicked from the server.");
		}
		this.inventoryManager.LeftRoom();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	// Token: 0x04000969 RID: 2409
	[SerializeField]
	private Sprite[] characterIcons = new Sprite[0];

	// Token: 0x0400096A RID: 2410
	[SerializeField]
	private GameObject loadingScreen;

	// Token: 0x0400096B RID: 2411
	[SerializeField]
	private Button startGameButton;

	// Token: 0x0400096C RID: 2412
	[SerializeField]
	private Button readyButton;

	// Token: 0x0400096D RID: 2413
	[SerializeField]
	private Text readyText;

	// Token: 0x0400096E RID: 2414
	[SerializeField]
	private Text selectJobText;

	// Token: 0x0400096F RID: 2415
	[SerializeField]
	private Text startGameText;

	// Token: 0x04000970 RID: 2416
	[SerializeField]
	private Color enabledColour;

	// Token: 0x04000971 RID: 2417
	[SerializeField]
	private Color disabledColour;

	// Token: 0x04000972 RID: 2418
	[HideInInspector]
	public PhotonView view;

	// Token: 0x04000973 RID: 2419
	[HideInInspector]
	public InventoryManager inventoryManager;

	// Token: 0x04000974 RID: 2420
	[SerializeField]
	private LevelSelectionManager levelSelectionManager;

	// Token: 0x04000975 RID: 2421
	[SerializeField]
	private LoadingAsyncManager loadingAsyncManager;

	// Token: 0x04000977 RID: 2423
	[SerializeField]
	private Button jobFinderButton;

	// Token: 0x04000978 RID: 2424
	[SerializeField]
	private GameObject contractSelectionObject;

	// Token: 0x04000979 RID: 2425
	[SerializeField]
	private Text difficultyText;

	// Token: 0x0400097A RID: 2426
	[SerializeField]
	private Text levelSelectionText;

	[SerializeField]
	private Text inviteCodeText;

	[SerializeField]
	private Text inviteText;

	// Token: 0x0400097B RID: 2427
	public List<PlayerServerSpot> players = new List<PlayerServerSpot>();

	// Token: 0x0400097C RID: 2428
	public List<ServerItem> serverItems = new List<ServerItem>();

	// Token: 0x0400097D RID: 2429
	private int myCharacterIndex;

	// Token: 0x0400097E RID: 2430
	[SerializeField]
	private RectTransform serverMask;

	// Token: 0x0400097F RID: 2431
	[SerializeField]
	private RectTransform mainMask;

	// Token: 0x04000980 RID: 2432
	[SerializeField]
	private GameObject storeObject;

	// Token: 0x04000983 RID: 2435
	[SerializeField]
	private Text publicPrivateText;

	// Token: 0x04000984 RID: 2436
	[SerializeField]
	private Button publicPrivateButton;

	// Token: 0x04000985 RID: 2437
	private bool isPublicServer = true;

	// Token: 0x04000986 RID: 2438
	[SerializeField]
	private Button[] kickPlayerButtons;
}

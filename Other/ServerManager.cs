using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class ServerManager : MonoBehaviourPunCallbacks
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.inventoryManager = base.GetComponent<InventoryManager>();
		this.myCharacterIndex = PlayerPrefs.GetInt("CharacterIndex");
		this.inviteCodeText.text = "??????";
		this.UpdateUI();
	}

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

	private void AssignAllPlayerInfoDelay()
	{
		for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
		{
			AssignNewPlayerSpot(PhotonNetwork.PlayerList[i]);
		}
		this.UpdateUI();
	}

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

	private IEnumerator AssignPlayerInfoDelay(Photon.Realtime.Player player)
	{
		yield return new WaitForSeconds(1f);
		this.AssignNewPlayerSpot(player);
		this.UpdateUI();
		yield break;
	}

	private void OnEnable()
	{
		this.UpdateUI();
		this.EnableMasks(true);
	}

	public void EnableMasks(bool active)
	{
		this.mainMask.sizeDelta = new Vector2((float)(active ? 1200 : 0), (float)(active ? 1200 : 0));
		this.serverMask.sizeDelta = new Vector2((float)(active ? 1200 : 0), (float)(active ? 1200 : 0));
	}

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

	public override void OnPlayerLeftRoom(Photon.Realtime.Player player)
	{
		this.RemovePlayerSpot(player);
		this.jobFinderButton.interactable = PhotonNetwork.IsMasterClient;
		this.UpdateUI();
	}

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

	public void Ready()
	{
		this.view.RPC("NetworkedReady", RpcTarget.AllBufferedViaServer, new object[]
		{
			PhotonNetwork.LocalPlayer
		});
	}

	public void StartGame()
	{
		if (this.levelSelectionManager.selectedLevelName != string.Empty)
		{
			this.LoadScene(this.levelSelectionManager.selectedLevelName);
		}
		
	}


    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
		PhotonNetwork.OfflineMode = true;
		base.OnDisconnected(cause);
    }

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

	public void SelectJob(bool active)
	{
		this.contractSelectionObject.SetActive(active);
		this.mainMask.sizeDelta = new Vector2((float)(active ? 0 : 1200), (float)(active ? 0 : 1200));
	}

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

	[SerializeField]
	private Sprite[] characterIcons = new Sprite[0];

	[SerializeField]
	private GameObject loadingScreen;

	[SerializeField]
	private Button startGameButton;

	[SerializeField]
	private Button readyButton;

	[SerializeField]
	private Text readyText;

	[SerializeField]
	private Text selectJobText;

	[SerializeField]
	private Text startGameText;

	[SerializeField]
	private Color enabledColour;

	[SerializeField]
	private Color disabledColour;

	[HideInInspector]
	public PhotonView view;

	[HideInInspector]
	public InventoryManager inventoryManager;

	[SerializeField]
	private LevelSelectionManager levelSelectionManager;

	[SerializeField]
	private LoadingAsyncManager loadingAsyncManager;

	[SerializeField]
	private Button jobFinderButton;

	[SerializeField]
	private GameObject contractSelectionObject;

	[SerializeField]
	private Text difficultyText;

	[SerializeField]
	private Text levelSelectionText;

	[SerializeField]
	private Text inviteCodeText;

	[SerializeField]
	private Text inviteText;

	public List<PlayerServerSpot> players = new List<PlayerServerSpot>();

	public List<ServerItem> serverItems = new List<ServerItem>();

	private int myCharacterIndex;

	[SerializeField]
	private RectTransform serverMask;

	[SerializeField]
	private RectTransform mainMask;

	[SerializeField]
	private GameObject storeObject;

	[SerializeField]
	private Text publicPrivateText;

	[SerializeField]
	private Button publicPrivateButton;

	private bool isPublicServer = true;

	[SerializeField]
	private Button[] kickPlayerButtons;
}


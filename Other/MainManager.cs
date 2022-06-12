using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class MainManager : MonoBehaviourPunCallbacks
{
	private void Awake()
	{
		MainManager.instance = this;
	}

	public IEnumerator Start()
	{
		yield return new WaitForSeconds(0.5f);
		if (!XRDevice.isPresent)
		{
			for (int i = 0; i < this.spawns.Count; i++)
			{
				this.spawns[i].Translate(Vector3.up);
			}
		}
		if (!PhotonNetwork.IsConnected || PhotonNetwork.OfflineMode)
		{
			PhotonNetwork.ConnectUsingSettings();
			if (XRDevice.isPresent)
			{
				this.localPlayer = UnityEngine.Object.Instantiate<GameObject>(this.vrPlayerModel, this.spawns[UnityEngine.Random.Range(0, this.spawns.Count)].position, Quaternion.identity).GetComponent<Player>();
			}
			else
			{
				this.localPlayer = UnityEngine.Object.Instantiate<GameObject>(this.pcPlayerModel, this.spawns[UnityEngine.Random.Range(0, this.spawns.Count)].position, Quaternion.identity).GetComponent<Player>();
				this.pcManager.SetValues();
			}
			PlayerPrefs.SetInt("StayInServerRoom", 0);
		}
		else if (PlayerPrefs.GetInt("StayInServerRoom") == 1)
		{
			PhotonNetwork.LeaveRoom(true);
		}
		else
		{
			PlayerPrefs.SetInt("StayInServerRoom", 0);
			PhotonNetwork.LeaveRoom(true);
		}
		/*if (PlayerPrefs.GetInt("completedTraining") == 0 && !Application.isEditor)
		{
			this.serverLobbyButton.interactable = false;
			this.serverLobbyText.color = new Color32(50, 50, 50, 119);
		}*/
		PlayerPrefs.SetInt("isTutorial", 0);
		this.trainingGhostTypeText.text = LocalisationSystem.GetLocalisedValue("Reward_Ghost") + " " + PlayerPrefs.GetString("GhostType");
		this.SetScreenResolution();
		if (PlayerPrefs.GetInt("myTotalExp") < 100)
		{
			PlayerPrefs.SetInt("myTotalExp", 100);
		}
		yield break;
	}

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}


	public void OnFailedToConnectToPhoton()
	{
		PhotonNetwork.OfflineMode = true;
		Debug.Log("Photon is now in Offline Mode: Failed to get a connection");
	}

	public override void OnConnectedToMaster()
	{
		if (!PhotonNetwork.OfflineMode)
		{
			PhotonNetwork.JoinLobby();
			return;
		}
		this.OnJoinedLobby();
	}

	public void ConnectToMaster()
    {
		PhotonNetwork.ConnectUsingSettings();
    }

	public override void OnLeftRoom()
	{
		if (PlayerPrefs.GetInt("StayInServerRoom") == 0)
		{
			if (XRDevice.isPresent)
			{
				this.localPlayer = UnityEngine.Object.Instantiate<GameObject>(this.vrPlayerModel, this.spawns[UnityEngine.Random.Range(0, this.spawns.Count)].position, Quaternion.identity).GetComponent<Player>();
			}
			else
			{
				this.localPlayer = UnityEngine.Object.Instantiate<GameObject>(this.pcPlayerModel, this.spawns[UnityEngine.Random.Range(0, this.spawns.Count)].position, Quaternion.identity).GetComponent<Player>();
				this.pcManager.SetValues();
			}
		}
		if (PlayerPrefs.GetInt("MissionStatus") == 3)
		{
			this.LoadRewardScreens();
		}
	}

	public override void OnJoinedRoom()
	{
		if (XRDevice.isPresent)
		{
			this.localPlayer = PhotonNetwork.Instantiate("VRPlayer", this.spawns[UnityEngine.Random.Range(0, this.spawns.Count)].position, Quaternion.identity, 0).GetComponent<Player>();
		}
		else
		{
			this.localPlayer = PhotonNetwork.Instantiate("PCPlayer", this.spawns[UnityEngine.Random.Range(0, this.spawns.Count)].position, Quaternion.identity, 0).GetComponent<Player>();
			this.pcManager.SetValues();
		}
		this.LoadRewardScreens();
	}

	public override void OnJoinedLobby()
	{
		PhotonNetwork.LocalPlayer.NickName = "DaniTesting";
		if (PlayerPrefs.GetInt("StayInServerRoom") == 1)
		{
			RoomOptions roomOptions = new RoomOptions
			{
				IsOpen = true,
				IsVisible = true,
				MaxPlayers = 4,
				PlayerTtl = 10000
			};
			PhotonNetwork.JoinOrCreateRoom(PlayerPrefs.GetString("ServerName"), roomOptions, null);
			return;
		}
		this.LoadRewardScreens();
	}

	private void OnPhotonCreateRoomFailed()
	{
		RoomOptions roomOptions = new RoomOptions
		{
			IsOpen = true,
			IsVisible = true,
			MaxPlayers = 4,
			PlayerTtl = 10000
		};
		PhotonNetwork.JoinOrCreateRoom(PlayerPrefs.GetString("ServerName"), roomOptions, null);
	}

	private void OnPhotonJoinRoomFailed()
	{
		RoomOptions roomOptions = new RoomOptions
		{
			IsOpen = true,
			IsVisible = true,
			MaxPlayers = 4,
			PlayerTtl = 10000
		};
		PhotonNetwork.JoinOrCreateRoom(PlayerPrefs.GetString("ServerName"), roomOptions, null);
	}

	public void AcceptPhotoWarning()
	{
		PlayerPrefs.SetInt("PhotoSensitivityWarning", 3);
		this.PhotoWarningScreen.SetActive(false);
		base.gameObject.SetActive(true);
	}

	private void LoadRewardScreens()
	{
		if (this.ranOnce)
		{
			return;
		}
		this.ranOnce = true;
		if (PlayerPrefs.GetString("ErrorMessage") != string.Empty)
		{
			Debug.LogError("Disconnect Error: " + PlayerPrefs.GetString("ErrorMessage"));
			this.ResetSettings(true);
			this.ErrorScreen.SetActive(true);
			base.gameObject.SetActive(false);
		}
		else if (PlayerPrefs.GetInt("PhotoSensitivityWarning") != 3)
		{
			this.ResetSettings(true);
			this.PhotoWarningScreen.SetActive(true);
			base.gameObject.SetActive(false);
		}
		else if (PlayerPrefs.GetInt("MissionStatus") == 1)
		{
			this.ResetSettings(true);
			this.RewardScreen.SetActive(true);
			base.gameObject.SetActive(false);
		}
		else if (PlayerPrefs.GetInt("MissionStatus") == 3)
		{
			this.ResetSettings(true);
			this.TrainingScreen.SetActive(true);
			base.gameObject.SetActive(false);
		}
		else if (PlayerPrefs.GetInt("MissionStatus") == 2 || PlayerPrefs.GetInt("isInGame") == 1)
		{
			this.ResetSettings(false);
			this.FailureScreen.SetActive(true);
			base.gameObject.SetActive(false);
		}
		if (PhotonNetwork.InRoom)
		{
			this.serverScreen.SetActive(true);
			this.serverManager.EnableMasks(false);
		}
	}

	private void ResetSettings(bool resetSetup)
	{
		PlayerPrefs.SetInt("isInGame", 0);
		PlayerPrefs.SetInt("isTutorial", 0);
		PlayerPrefs.SetInt("MissionStatus", 0);
		if (resetSetup)
		{
			PlayerPrefs.SetInt("setupPhase", 0);
		}
	}

	private void SetScreenResolution()
	{
		if (!XRDevice.isPresent)
		{
			if (PlayerPrefs.GetInt("resolutionValue") > Screen.resolutions.Length - 1)
			{
				PlayerPrefs.SetInt("resolutionValue", Screen.resolutions.Length - 1);
			}
			else if (PlayerPrefs.GetInt("resolutionValue") < 0)
			{
				PlayerPrefs.SetInt("resolutionValue", Screen.resolutions.Length - 1);
			}
			if (PlayerPrefs.GetInt("resolutionValue") == 0)
			{
				PlayerPrefs.SetInt("resolutionValue", Screen.resolutions.Length - 1);
				Screen.SetResolution(Screen.resolutions[Screen.resolutions.Length - 1].width, Screen.resolutions[Screen.resolutions.Length - 1].height, true);
				return;
			}
			Screen.SetResolution(Screen.resolutions[PlayerPrefs.GetInt("resolutionValue")].width, Screen.resolutions[PlayerPrefs.GetInt("resolutionValue")].height, PlayerPrefs.GetInt("fullscreenType") == 1);
		}
	}

	private void WipeMoneyAndEquipment()
	{
		PlayerPrefs.SetInt("PlayerMoney", 0);
		PlayerPrefs.SetInt("EMFReaderInventory", 0);
		PlayerPrefs.SetInt("FlashlightInventory", 0);
		PlayerPrefs.SetInt("CameraInventory", 0);
		PlayerPrefs.SetInt("LighterInventory", 0);
		PlayerPrefs.SetInt("CandleInventory", 0);
		PlayerPrefs.SetInt("UVFlashlightInventory", 0);
		PlayerPrefs.SetInt("CrucifixInventory", 0);
		PlayerPrefs.SetInt("DSLRCameraInventory", 0);
		PlayerPrefs.SetInt("EVPRecorderInventory", 0);
		PlayerPrefs.SetInt("SaltInventory", 0);
		PlayerPrefs.SetInt("SageInventory", 0);
		PlayerPrefs.SetInt("TripodInventory", 0);
		PlayerPrefs.SetInt("StrongFlashlightInventory", 0);
		PlayerPrefs.SetInt("MotionSensorInventory", 0);
		PlayerPrefs.SetInt("SoundSensorInventory", 0);
		PlayerPrefs.SetInt("SanityPillsInventory", 0);
		PlayerPrefs.SetInt("ThermometerInventory", 0);
		PlayerPrefs.SetInt("GhostWritingBookInventory", 0);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public static MainManager instance;

	public Camera sceneCamera;

	public List<Transform> spawns = new List<Transform>();

	[SerializeField]
	private GameObject vrPlayerModel;

	[SerializeField]
	private GameObject pcPlayerModel;

	private bool ranOnce;

	[SerializeField]
	private GameObject serverObject;

	[SerializeField]
	private PCManager pcManager;

	[SerializeField]
	private MyAudioManager audioManager;

	[SerializeField]
	private StoreSDKManager storeSDKManager;

	public ServerManager serverManager;

	[SerializeField]
	private GameObject RewardScreen;

	[SerializeField]
	private GameObject FailureScreen;

	[SerializeField]
	private GameObject TrainingScreen;

	[SerializeField]
	private GameObject serverScreen;

	[SerializeField]
	private GameObject ErrorScreen;

	[SerializeField]
	private GameObject PhotoWarningScreen;

	[SerializeField]
	private Text trainingGhostTypeText;

	[SerializeField]
	private Button serverLobbyButton;

	[SerializeField]
	private Text serverLobbyText;

	[HideInInspector]
	public Player localPlayer;
}


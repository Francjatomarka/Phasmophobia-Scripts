using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

// Token: 0x0200013F RID: 319
public class MainManager : MonoBehaviourPunCallbacks
{
	// Token: 0x060008E6 RID: 2278 RVA: 0x00036CCB File Offset: 0x00034ECB
	private void Awake()
	{
		MainManager.instance = this;
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x00036CE3 File Offset: 0x00034EE3
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

    // Token: 0x060008E8 RID: 2280 RVA: 0x00036CF4 File Offset: 0x00034EF4
    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}


	// Token: 0x060008E9 RID: 2281 RVA: 0x00036D47 File Offset: 0x00034F47
	public void OnFailedToConnectToPhoton()
	{
		PhotonNetwork.OfflineMode = true;
		Debug.Log("Photon is now in Offline Mode: Failed to get a connection");
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x00036D99 File Offset: 0x00034F99
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

	// Token: 0x060008EC RID: 2284 RVA: 0x00036DB0 File Offset: 0x00034FB0
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

	// Token: 0x060008ED RID: 2285 RVA: 0x00036E6C File Offset: 0x0003506C
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

	// Token: 0x060008EE RID: 2286 RVA: 0x00036F0C File Offset: 0x0003510C
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

	// Token: 0x060008EF RID: 2287 RVA: 0x00036FCC File Offset: 0x000351CC
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

	// Token: 0x060008F0 RID: 2288 RVA: 0x00037018 File Offset: 0x00035218
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

	// Token: 0x060008F1 RID: 2289 RVA: 0x00037061 File Offset: 0x00035261
	public void AcceptPhotoWarning()
	{
		PlayerPrefs.SetInt("PhotoSensitivityWarning", 3);
		this.PhotoWarningScreen.SetActive(false);
		base.gameObject.SetActive(true);
	}

	// Token: 0x060008F2 RID: 2290 RVA: 0x00037088 File Offset: 0x00035288
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

	// Token: 0x060008F3 RID: 2291 RVA: 0x000371DD File Offset: 0x000353DD
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

	// Token: 0x060008F4 RID: 2292 RVA: 0x00037210 File Offset: 0x00035410
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

	// Token: 0x060008F5 RID: 2293 RVA: 0x0003730C File Offset: 0x0003550C
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

	// Token: 0x060008F6 RID: 2294 RVA: 0x000373EA File Offset: 0x000355EA
	public void QuitGame()
	{
		Application.Quit();
	}

	// Token: 0x04000904 RID: 2308
	public static MainManager instance;

	// Token: 0x04000905 RID: 2309
	public Camera sceneCamera;

	// Token: 0x04000906 RID: 2310
	public List<Transform> spawns = new List<Transform>();

	// Token: 0x04000907 RID: 2311
	[SerializeField]
	private GameObject vrPlayerModel;

	// Token: 0x04000908 RID: 2312
	[SerializeField]
	private GameObject pcPlayerModel;

	// Token: 0x04000909 RID: 2313
	private bool ranOnce;

	// Token: 0x0400090A RID: 2314
	[SerializeField]
	private GameObject serverObject;

	// Token: 0x0400090C RID: 2316
	[SerializeField]
	private PCManager pcManager;

	// Token: 0x0400090D RID: 2317
	[SerializeField]
	private MyAudioManager audioManager;

	// Token: 0x0400090E RID: 2318
	[SerializeField]
	private StoreSDKManager storeSDKManager;

	// Token: 0x04000910 RID: 2320
	public ServerManager serverManager;

	// Token: 0x04000911 RID: 2321
	[SerializeField]
	private GameObject RewardScreen;

	// Token: 0x04000912 RID: 2322
	[SerializeField]
	private GameObject FailureScreen;

	// Token: 0x04000913 RID: 2323
	[SerializeField]
	private GameObject TrainingScreen;

	// Token: 0x04000914 RID: 2324
	[SerializeField]
	private GameObject serverScreen;

	// Token: 0x04000915 RID: 2325
	[SerializeField]
	private GameObject ErrorScreen;

	// Token: 0x04000916 RID: 2326
	[SerializeField]
	private GameObject PhotoWarningScreen;

	// Token: 0x04000917 RID: 2327
	[SerializeField]
	private Text trainingGhostTypeText;

	// Token: 0x04000918 RID: 2328
	[SerializeField]
	private Button serverLobbyButton;

	// Token: 0x04000919 RID: 2329
	[SerializeField]
	private Text serverLobbyText;

	// Token: 0x0400091A RID: 2330
	[HideInInspector]
	public Player localPlayer;
}

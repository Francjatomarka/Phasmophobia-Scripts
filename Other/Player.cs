using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.XR;
using UnityStandardAssets.Characters.FirstPerson;
using Photon.Pun;
using Photon.Voice;

public class Player : MonoBehaviour
{
	private void Awake()
	{
		this.firstPersonController = GetComponent<FirstPersonController>();
		this.view = base.GetComponent<PhotonView>();
		this.keys.Clear();
	}

	private void Start()
	{
		if (this.hasRun)
		{
			return;
		}
		if (LevelController.instance)
		{
			this.currentRoom = LevelController.instance.outsideRoom;
		}
		if (this.view.IsMine || !PhotonNetwork.InRoom)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			if (!XRDevice.isPresent && PlayerPrefs.GetInt("fovValue") != 0)
			{
				this.cam.fieldOfView = (float)PlayerPrefs.GetInt("fovValue");
				this.pcPropGrab.ChangeItemSpotWithFOV((float)PlayerPrefs.GetInt("fovValue"));
				this.itemSway.SetPosition();
			}
			if (this.firstPersonController)
			{
				bool flag = PlayerPrefs.GetInt("invertedXLookValue") == 1;
				bool flag2 = PlayerPrefs.GetInt("invertedYLookValue") == 1;
				this.firstPersonController.m_MouseLook.XSensitivity = PlayerPrefs.GetFloat("sensitivityValue") / 10;
				this.firstPersonController.m_MouseLook.YSensitivity = PlayerPrefs.GetFloat("sensitivityValue") / 10;
			}
			if (PhotonNetwork.InRoom && MainManager.instance == null)
			{
				SetPlayerData(1001);
			}
			if (MainManager.instance == null)
			{
				if (PhotonNetwork.InRoom && this.view.IsMine)
				{
					Canvas[] array = UnityEngine.Object.FindObjectsOfType<Canvas>();
					for (int i = 0; i < array.Length; i++)
					{
						array[i].worldCamera = this.cam;
					}
				}
				else
				{
					Canvas[] array = UnityEngine.Object.FindObjectsOfType<Canvas>();
					for (int i = 0; i < array.Length; i++)
					{
						array[i].worldCamera = this.cam;
					}
				}
			}
			if (PhotonNetwork.InRoom && !XRDevice.isPresent)
			{
				this.pcMenu.ForceIntoMenu();
			}
			this.ApplyBrightnessSetting();
			this.ApplyAntiAliasing();
			this.ApplyAudioSetting();
			this.ApplyScreenSpaceReflectionSetting();
			this.ApplyAmbientOcclusionSetting();
			if (MainManager.instance)
			{
				MainManager.instance.localPlayer = this;
			}
			if (GameController.instance != null)
			{
				if (GameController.instance.isTutorial)
				{
					this.difficultyRate = 0.5f;
				}
				else if (GameController.instance.levelDifficulty == Contract.LevelDifficulty.Intermediate)
				{
					this.difficultyRate = 1.5f;
				}
				else if (GameController.instance.levelDifficulty == Contract.LevelDifficulty.Professional)
				{
					this.difficultyRate = 2f;
				}
				if (LevelController.instance.type == LevelController.levelType.medium)
				{
					this.normalSanityRate = 0.08f;
					this.setupSanityRate = 0.05f;
				}
				else if (LevelController.instance.type == LevelController.levelType.large)
				{
					this.normalSanityRate = 0.05f;
					this.setupSanityRate = 0.03f;
				}
				if (PhotonNetwork.PlayerList.Length == 1 && !GameController.instance.isTutorial)
				{
					this.normalSanityRate /= 2f;
					this.setupSanityRate /= 2f;
				}
			}
			PhotonNetwork.IsMessageQueueRunning = true;
		}
		this.hasRun = true;
		if (MainManager.instance)
		{
			this.currentPlayerSnapshot = SoundController.instance.firstFloorSnapshot;
			return;
		}
		this.currentPlayerSnapshot = this.truckSnapshot;
	}

	private void Update()
	{
		/*if (!this.view.IsMine)
		{
			return;
		}*/
		if (this.charController != null && this.charAnim != null)
		{
			this.charAnim.SetFloat("speed", this.charController.velocity.magnitude);
		}
		if (!this.isDead && GameController.instance != null)
		{
			if (SetupPhaseController.instance == null && GhostController.instance == null)
			{
				return;
			}
			if (SetupPhaseController.instance.mainDoorHasUnlocked)
			{
				if (!this.playerIsInLight && this.currentRoom != LevelController.instance.outsideRoom)
				{
					this.insanity += Time.deltaTime * ((SetupPhaseController.instance.isSetupPhase ? this.setupSanityRate : this.normalSanityRate) * this.difficultyRate);
				}
				this.insanity = Mathf.Clamp(this.insanity, 0f, (float)(SetupPhaseController.instance.isSetupPhase ? 50 : 100));
				if (!this.sanityChallengeHasBeenSet && !this.isDead && this.insanity >= 99f)
				{
					DailyChallengesController.Instance.ChangeChallengeProgression(ChallengeType.GetZeroSanity, 1);
					this.sanityChallengeHasBeenSet = true;
				}
				if (this.sanityCheckTimer < 0f)
				{
					this.CheckCurrentLight();
					this.sanityCheckTimer = 2f;
				}
				else
				{
					this.sanityCheckTimer -= Time.deltaTime;
				}
				if (this.sanityUpdateTimer < 0f)
				{
					GhostController.instance.UpdatePlayerSanity();
					this.sanityUpdateTimer = 5f;
					return;
				}
				this.sanityUpdateTimer -= Time.deltaTime;
			}
		}
	}

	[PunRPC]
	private void SetPlayerData(int photonPlayerID)
	{
		if (MainManager.instance)
		{
			return;
		}
		PlayerData playerData = new PlayerData
		{
			player = this,
			actorID = photonPlayerID,
			playerName = "Dani"
		};
		GameController.instance.playersData.Add(playerData);
		GameController.instance.myPlayer = playerData;
		GameController.instance.OnLocalPlayerSpawned.Invoke();
		GameController.instance.OnPlayerSpawned.Invoke();
	}

	public void ActivateOrDeactivateRecordingCam(bool isActive)
	{
		if (this.view.IsMine || !PhotonNetwork.InRoom)
		{
			this.smoothVRCamera.SetActive(isActive);
		}
	}

	public void ChangeSanity(int value)
	{
		this.view.RPC("ChangeSanitySync", this.view.Owner, new object[]
		{
			value
		});
	}

	[PunRPC]
	private void ChangeSanitySync(int value)
	{
		this.insanity += (float)value;
	}

	public void KillPlayer()
	{
		this.view.RPC("Dead", RpcTarget.All, Array.Empty<object>());
	}

	public void StartKillingPlayer()
	{
		this.view.RPC("StartKillingPlayerNetworked", this.view.Owner, Array.Empty<object>());
	}

	public void SpawnDeadBody(Vector3 spawnPos)
	{
		PhotonNetwork.InstantiateSceneObject("DeadPlayerRagdoll", spawnPos, LevelController.instance.currentGhost.transform.rotation, 0, null).GetComponent<DeadPlayer>().Spawn(this.modelID, int.Parse(this.view.Owner.UserId));
	}

	[PunRPC]
	private void StartKillingPlayerNetworked()
	{
		this.isDead = true;
		if (this.pcFlashlight)
		{
			this.pcFlashlight.EnableOrDisableLight(false, true);
		}
		this.deathAudioSource.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		this.deathAudioSource.Play();
		this.ghostDeathHands.SetActive(true);
		this.chokingAudioSource.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		this.chokingAudioSource.Play();
		this.playerHeadCamera.DisableCamera();
		if (!XRDevice.isPresent)
		{
			this.pcPropGrab.DropAllInventoryProps();
			return;
		}
		this.DropAllVRObjects();
	}

	public void StopAllMovement()
	{
		if (!XRDevice.isPresent)
		{
			this.firstPersonController.m_WalkSpeed = 0f;
			this.firstPersonController.m_RunSpeed = 0f;
		}
	}

	[PunRPC]
	private void Dead()
	{
		this.isDead = true;
		base.gameObject.layer = LayerMask.NameToLayer("DeadPlayer");
		if (!Application.isEditor)
		{
			GameController.instance.PlayerDied();
		}
		this.insanity = 0f;
		if (this.view.IsMine)
		{
			PlayerPrefs.SetInt("PlayerDied", 1);
			this.postProcessingVolume.profile = this.deadProfile;
			RenderSettings.fog = true;
			DeadZoneController.instance.EnableOrDisableDeadZone(true);
			this.deathSnapshot.TransitionTo(0.1f);
			this.firstPersonController.m_WalkSpeed = 1.2f;
			this.firstPersonController.m_RunSpeed = 1.6f;
			for (int i = 0; i < LevelController.instance.doors.Count; i++)
			{
				if (LevelController.instance.doors[i] != null)
				{
					if (LevelController.instance.doors[i].GetComponent<Door>() != null && LevelController.instance.doors[i].GetComponent<Door>().rend != null)
					{
						LevelController.instance.doors[i].GetComponent<Door>().rend.gameObject.SetActive(false);
					}
					LevelController.instance.doors[i].SetActive(false);
				}
			}
			for (int j = 0; j < LevelController.instance.rooms.Length; j++)
			{
				for (int k = 0; k < LevelController.instance.rooms[j].lightSwitches.Count; k++)
				{
					for (int l = 0; l < LevelController.instance.rooms[j].lightSwitches[k].probes.Count; l++)
					{
						LevelController.instance.rooms[j].lightSwitches[k].probes[l].RenderProbe();
					}
				}
			}
		}
		base.transform.SetParent(DeadZoneController.instance.zoneObjects.transform);
	}

	private void CheckCurrentLight()
	{
		Texture2D texture2D = new Texture2D(32, 32, TextureFormat.RGB24, false);
		Rect source = new Rect(0f, 0f, 32f, 32f);
		RenderTexture.active = this.shadowRenderTexture;
		texture2D.ReadPixels(source, 0, 0);
		texture2D.Apply();
		RenderTexture.active = null;
		int num = 0;
		for (int i = 0; i < 32; i++)
		{
			for (int j = 0; j < 32; j++)
			{
				if (texture2D.GetPixel(j, i).grayscale <= 0.008f)
				{
					num++;
				}
			}
		}
		this.playerIsInLight = (num <= 900);
	}

	private void ApplyBrightnessSetting()
	{
		ColorGrading colorGrading = null;
		this.postProcessingVolume.profile.TryGetSettings<ColorGrading>(out colorGrading);
		colorGrading.postExposure.value = PlayerPrefs.GetFloat("brightnessValue");
	}

	public void ApplyAntiAliasing()
	{
		if (!XRDevice.isPresent)
		{
			if (PlayerPrefs.GetInt("taaValue") == 0)
			{
				this.postProcessingLayer.antialiasingMode = PostProcessLayer.Antialiasing.None;
				return;
			}
			this.postProcessingLayer.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
		}
	}

	public void ApplyAudioSetting()
	{
		this.masterAudio.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")) * 20f + 15f);
	}

	public void ApplyScreenSpaceReflectionSetting()
	{
		if (!XRDevice.isPresent)
		{
			ScreenSpaceReflections screenSpaceReflections = null;
			this.postProcessingVolume.profile.TryGetSettings<ScreenSpaceReflections>(out screenSpaceReflections);
		}
	}

	public void ApplyAmbientOcclusionSetting()
	{
		if (!XRDevice.isPresent)
		{
			AmbientOcclusion ambientOcclusion = null;
			this.postProcessingVolume.profile.TryGetSettings<AmbientOcclusion>(out ambientOcclusion);
			ambientOcclusion.enabled.value = (PlayerPrefs.GetInt("ambientOcclusion") == 1);
		}
	}

	public void ForceDropAllProps()
	{
		foreach (PhotonObjectInteract photonObjectInteract in base.GetComponentsInChildren<PhotonObjectInteract>(true))
		{
			if (photonObjectInteract.isProp && !photonObjectInteract.isFixedItem)
			{
				photonObjectInteract.gameObject.SetActive(true);
				photonObjectInteract.transform.SetParent(null);
				photonObjectInteract.isGrabbed = false;
				photonObjectInteract.GetComponent<Collider>().enabled = true;
				photonObjectInteract.GetComponent<Rigidbody>().useGravity = true;
				photonObjectInteract.GetComponent<Rigidbody>().isKinematic = false;
				if (photonObjectInteract.myLeftHandModel)
				{
					photonObjectInteract.myLeftHandModel.SetActive(false);
				}
				if (photonObjectInteract.myRightHandModel)
				{
					photonObjectInteract.myRightHandModel.SetActive(false);
				}
			}
		}
	}

	public void DropAllVRObjects()
	{
		
	}

	private void OnDisable()
	{
		this.DropAllVRObjects();
		if (MapController.instance)
		{
			MapController.instance.RemovePlayer(this);
		}
	}

	[HideInInspector]
	public PhotonView view;

	[HideInInspector]
	public bool beingHunted;

	public bool isDead;

	private bool cameraBoard = false;

	[HideInInspector]
	public int modelID;

	public Camera cameraBoardObj;

	private bool sanityChallengeHasBeenSet;

	[Header("Post Processing")]
	public PostProcessVolume postProcessingVolume;

	public PostProcessLayer postProcessingLayer;

	[SerializeField]
	private PostProcessProfile mainProfile;

	[SerializeField]
	private PostProcessProfile deadProfile;

	[Header("Main")]
	public GameObject headObject;

	[SerializeField]
	private Breath breath;

	public List<global::Key.KeyType> keys = new List<global::Key.KeyType>();

	public Camera cam;

	[HideInInspector]
	public LevelRoom currentRoom;

	[HideInInspector]
	public Transform mapIcon;

	public PhotonObjectInteract currentHeldObject;

	public GameObject[] characterModels;

	public GameObject ghostDeathHands;

	public LayerMask ghostRaycastMask;

	public LayerMask mainLayerMask;

	public PlayerHeadCamera playerHeadCamera;

	public Transform aiTargetPoint;

	[Header("Audio")]
	[SerializeField]
	private AudioMixerSnapshot interiorSnapshot;

	[SerializeField]
	private AudioMixerSnapshot deathSnapshot;

	[SerializeField]
	private AudioMixerSnapshot truckSnapshot;

	[HideInInspector]
	public AudioMixerSnapshot currentPlayerSnapshot;

	public VoiceVolume voiceVolume;


	public FootstepController footstepController;

	public AudioSource evidenceAudioSource;

	public AudioSource keysAudioSource;

	[SerializeField]
	private AudioSource deathAudioSource;

	public AudioSource chokingAudioSource;

	public AudioSource heartBeatAudioSource;

	public VoiceOcclusion voiceOcclusion;

	[SerializeField]
	private AudioMixer masterAudio;

	[Header("Sanity")]
	[HideInInspector]
	public float insanity;

	private float sanityUpdateTimer = 15f;

	private float sanityCheckTimer = 2f;

	[SerializeField]
	private RenderTexture shadowRenderTexture;

	[SerializeField]
	private bool playerIsInLight;

	private float difficultyRate = 1f;

	private float normalSanityRate = 0.12f;

	private float setupSanityRate = 0.09f;

	[Header("PC")]
	public CharacterController charController;

	public AudioListener listener;

	public FirstPersonController firstPersonController;

	public PCPropGrab pcPropGrab;

	public DragRigidbodyUse dragRigidBodyUse;

	public PCCanvas pcCanvas;

	public PCCrouch pcCrouch;

	public PCMenu pcMenu;

	public PCControls pcControls;

	public PCFlashlight pcFlashlight;

	[HideInInspector]
	public Animator charAnim;

	public PlayerInput playerInput;

	public PCItemSway itemSway;

	public Transform steamVRObj;

	public VRMovementSettings movementSettings;

	[SerializeField]
	private GameObject smoothVRCamera;

	public Transform VRIKObj;

	private bool hasRun;
}


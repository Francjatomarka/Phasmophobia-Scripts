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

// Token: 0x0200016F RID: 367
public class Player : MonoBehaviour
{
	// Token: 0x06000A71 RID: 2673 RVA: 0x000405E1 File Offset: 0x0003E7E1
	private void Awake()
	{
		this.firstPersonController = GetComponent<FirstPersonController>();
		this.view = base.GetComponent<PhotonView>();
		this.keys.Clear();
	}

	// Token: 0x06000A72 RID: 2674 RVA: 0x000405FC File Offset: 0x0003E7FC
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

	// Token: 0x06000A73 RID: 2675 RVA: 0x00040954 File Offset: 0x0003EB54
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

	// Token: 0x06000A74 RID: 2676 RVA: 0x00040B08 File Offset: 0x0003ED08
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

	// Token: 0x06000A75 RID: 2677 RVA: 0x00040BA3 File Offset: 0x0003EDA3
	public void ActivateOrDeactivateRecordingCam(bool isActive)
	{
		if (this.view.IsMine || !PhotonNetwork.InRoom)
		{
			this.smoothVRCamera.SetActive(isActive);
		}
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x00040BC5 File Offset: 0x0003EDC5
	public void ChangeSanity(int value)
	{
		this.view.RPC("ChangeSanitySync", this.view.Owner, new object[]
		{
			value
		});
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x00040BF1 File Offset: 0x0003EDF1
	[PunRPC]
	private void ChangeSanitySync(int value)
	{
		this.insanity += (float)value;
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x00040C02 File Offset: 0x0003EE02
	public void KillPlayer()
	{
		this.view.RPC("Dead", RpcTarget.All, Array.Empty<object>());
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x00040C1A File Offset: 0x0003EE1A
	public void StartKillingPlayer()
	{
		this.view.RPC("StartKillingPlayerNetworked", this.view.Owner, Array.Empty<object>());
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x00040C3C File Offset: 0x0003EE3C
	public void SpawnDeadBody(Vector3 spawnPos)
	{
		PhotonNetwork.InstantiateSceneObject("DeadPlayerRagdoll", spawnPos, LevelController.instance.currentGhost.transform.rotation, 0, null).GetComponent<DeadPlayer>().Spawn(this.modelID, int.Parse(this.view.Owner.UserId));
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x00040C7C File Offset: 0x0003EE7C
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

	// Token: 0x06000A7C RID: 2684 RVA: 0x00040D3A File Offset: 0x0003EF3A
	public void StopAllMovement()
	{
		if (!XRDevice.isPresent)
		{
			this.firstPersonController.m_WalkSpeed = 0f;
			this.firstPersonController.m_RunSpeed = 0f;
		}
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x00040D64 File Offset: 0x0003EF64
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

	// Token: 0x06000A7E RID: 2686 RVA: 0x00040FE4 File Offset: 0x0003F1E4
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

	// Token: 0x06000A7F RID: 2687 RVA: 0x00041088 File Offset: 0x0003F288
	private void ApplyBrightnessSetting()
	{
		ColorGrading colorGrading = null;
		this.postProcessingVolume.profile.TryGetSettings<ColorGrading>(out colorGrading);
		colorGrading.postExposure.value = PlayerPrefs.GetFloat("brightnessValue");
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x000410BF File Offset: 0x0003F2BF
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

	// Token: 0x06000A81 RID: 2689 RVA: 0x000410ED File Offset: 0x0003F2ED
	public void ApplyAudioSetting()
	{
		this.masterAudio.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")) * 20f + 15f);
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x0004111C File Offset: 0x0003F31C
	public void ApplyScreenSpaceReflectionSetting()
	{
		if (!XRDevice.isPresent)
		{
			ScreenSpaceReflections screenSpaceReflections = null;
			this.postProcessingVolume.profile.TryGetSettings<ScreenSpaceReflections>(out screenSpaceReflections);
		}
	}

	// Token: 0x06000A83 RID: 2691 RVA: 0x00041160 File Offset: 0x0003F360
	public void ApplyAmbientOcclusionSetting()
	{
		if (!XRDevice.isPresent)
		{
			AmbientOcclusion ambientOcclusion = null;
			this.postProcessingVolume.profile.TryGetSettings<AmbientOcclusion>(out ambientOcclusion);
			ambientOcclusion.enabled.value = (PlayerPrefs.GetInt("ambientOcclusion") == 1);
		}
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x000411A4 File Offset: 0x0003F3A4
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

	// Token: 0x06000A85 RID: 2693 RVA: 0x00041258 File Offset: 0x0003F458
	public void DropAllVRObjects()
	{
		
	}

	// Token: 0x06000A86 RID: 2694 RVA: 0x00041417 File Offset: 0x0003F617
	private void OnDisable()
	{
		this.DropAllVRObjects();
		if (MapController.instance)
		{
			MapController.instance.RemovePlayer(this);
		}
	}

	// Token: 0x04000AA2 RID: 2722
	[HideInInspector]
	public PhotonView view;

	// Token: 0x04000AA3 RID: 2723
	[HideInInspector]
	public bool beingHunted;

	// Token: 0x04000AA4 RID: 2724
	public bool isDead;

	private bool cameraBoard = false;

	// Token: 0x04000AA5 RID: 2725
	[HideInInspector]
	public int modelID;

	public Camera cameraBoardObj;

	// Token: 0x04000AA6 RID: 2726
	private bool sanityChallengeHasBeenSet;

	// Token: 0x04000AA7 RID: 2727
	[Header("Post Processing")]
	public PostProcessVolume postProcessingVolume;

	// Token: 0x04000AA8 RID: 2728
	public PostProcessLayer postProcessingLayer;

	// Token: 0x04000AA9 RID: 2729
	[SerializeField]
	private PostProcessProfile mainProfile;

	// Token: 0x04000AAA RID: 2730
	[SerializeField]
	private PostProcessProfile deadProfile;

	// Token: 0x04000AAB RID: 2731
	[Header("Main")]
	public GameObject headObject;

	// Token: 0x04000AAC RID: 2732
	[SerializeField]
	private Breath breath;

	// Token: 0x04000AAD RID: 2733
	public List<global::Key.KeyType> keys = new List<global::Key.KeyType>();

	// Token: 0x04000AAE RID: 2734
	public Camera cam;

	// Token: 0x04000AAF RID: 2735
	[HideInInspector]
	public LevelRoom currentRoom;

	// Token: 0x04000AB0 RID: 2736
	[HideInInspector]
	public Transform mapIcon;

	// Token: 0x04000AB1 RID: 2737
	public PhotonObjectInteract currentHeldObject;

	// Token: 0x04000AB2 RID: 2738
	public GameObject[] characterModels;

	// Token: 0x04000AB3 RID: 2739
	public GameObject ghostDeathHands;

	// Token: 0x04000AB4 RID: 2740
	public LayerMask ghostRaycastMask;

	// Token: 0x04000AB5 RID: 2741
	public LayerMask mainLayerMask;

	// Token: 0x04000AB6 RID: 2742
	public PlayerHeadCamera playerHeadCamera;

	// Token: 0x04000AB7 RID: 2743
	public Transform aiTargetPoint;

	// Token: 0x04000AB8 RID: 2744
	[Header("Audio")]
	[SerializeField]
	private AudioMixerSnapshot interiorSnapshot;

	// Token: 0x04000AB9 RID: 2745
	[SerializeField]
	private AudioMixerSnapshot deathSnapshot;

	// Token: 0x04000ABA RID: 2746
	[SerializeField]
	private AudioMixerSnapshot truckSnapshot;

	// Token: 0x04000ABB RID: 2747
	[HideInInspector]
	public AudioMixerSnapshot currentPlayerSnapshot;

	// Token: 0x04000ABC RID: 2748
	public VoiceVolume voiceVolume;


	// Token: 0x04000ABE RID: 2750
	public FootstepController footstepController;

	// Token: 0x04000ABF RID: 2751
	public AudioSource evidenceAudioSource;

	// Token: 0x04000AC0 RID: 2752
	public AudioSource keysAudioSource;

	// Token: 0x04000AC1 RID: 2753
	[SerializeField]
	private AudioSource deathAudioSource;

	// Token: 0x04000AC2 RID: 2754
	public AudioSource chokingAudioSource;

	// Token: 0x04000AC3 RID: 2755
	public AudioSource heartBeatAudioSource;

	// Token: 0x04000AC4 RID: 2756
	public VoiceOcclusion voiceOcclusion;

	// Token: 0x04000AC5 RID: 2757
	[SerializeField]
	private AudioMixer masterAudio;

	// Token: 0x04000AC6 RID: 2758
	[Header("Sanity")]
	[HideInInspector]
	public float insanity;

	// Token: 0x04000AC7 RID: 2759
	private float sanityUpdateTimer = 15f;

	// Token: 0x04000AC8 RID: 2760
	private float sanityCheckTimer = 2f;

	// Token: 0x04000AC9 RID: 2761
	[SerializeField]
	private RenderTexture shadowRenderTexture;

	// Token: 0x04000ACA RID: 2762
	[SerializeField]
	private bool playerIsInLight;

	// Token: 0x04000ACB RID: 2763
	private float difficultyRate = 1f;

	// Token: 0x04000ACC RID: 2764
	private float normalSanityRate = 0.12f;

	// Token: 0x04000ACD RID: 2765
	private float setupSanityRate = 0.09f;

	// Token: 0x04000ACE RID: 2766
	[Header("PC")]
	public CharacterController charController;

	// Token: 0x04000ACF RID: 2767
	public AudioListener listener;

	// Token: 0x04000AD0 RID: 2768
	public FirstPersonController firstPersonController;

	// Token: 0x04000AD1 RID: 2769
	public PCPropGrab pcPropGrab;

	// Token: 0x04000AD2 RID: 2770
	public DragRigidbodyUse dragRigidBodyUse;

	// Token: 0x04000AD3 RID: 2771
	public PCCanvas pcCanvas;

	// Token: 0x04000AD4 RID: 2772
	public PCCrouch pcCrouch;

	// Token: 0x04000AD5 RID: 2773
	public PCMenu pcMenu;

	// Token: 0x04000AD6 RID: 2774
	public PCControls pcControls;

	// Token: 0x04000AD7 RID: 2775
	public PCFlashlight pcFlashlight;

	// Token: 0x04000AD8 RID: 2776
	[HideInInspector]
	public Animator charAnim;

	// Token: 0x04000AD9 RID: 2777
	public PlayerInput playerInput;

	// Token: 0x04000ADA RID: 2778
	public PCItemSway itemSway;

	// Token: 0x04000ADF RID: 2783
	public Transform steamVRObj;

	// Token: 0x04000AE0 RID: 2784
	public VRMovementSettings movementSettings;

	// Token: 0x04000AE3 RID: 2787
	[SerializeField]
	private GameObject smoothVRCamera;

	// Token: 0x04000AE5 RID: 2789
	public Transform VRIKObj;

	// Token: 0x04000AE7 RID: 2791
	private bool hasRun;
}

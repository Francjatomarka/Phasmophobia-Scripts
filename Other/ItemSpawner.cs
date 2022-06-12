using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class ItemSpawner : MonoBehaviourPunCallbacks
{
	private void Awake()
	{
		for (int i = 0; i < this.headMountedCameras.Length; i++)
		{
			if(headMountedCameras[i] != null)
            {
				this.headMountedCameras[i].gameObject.SetActive(false);
			}
		}
	}

	private void Start()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			if (GameController.instance.playersData.Count != PhotonNetwork.PlayerList.Length)
			{
				GameController.instance.OnAllPlayersConnected.AddListener(new UnityAction(this.SpawnItems));
			}
			else
			{
				this.SpawnItems();
			}
		}
		this.CheckSanityPillsOwners();
	}

    public override void OnJoinedRoom()
    {
		if (PhotonNetwork.IsMasterClient)
		{
			if (GameController.instance.playersData.Count != PhotonNetwork.PlayerList.Length)
			{
				GameController.instance.OnAllPlayersConnected.AddListener(new UnityAction(this.SpawnItems));
				return;
			}
			this.SpawnItems();
		}
	}

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
		if (PhotonNetwork.IsMasterClient && GameController.instance.playersData.Count == PhotonNetwork.PlayerList.Length)
		{
			this.SpawnItems();
		}
	}

	[PunRPC]
	private void SyncSpawn()
	{
		this.hasSpawned = true;
	}

	private void SpawnItems()
	{
		if (this.hasSpawned)
		{
			return;
		}
		this.view.RPC("SyncSpawn", RpcTarget.AllBuffered, Array.Empty<object>());
		if (Application.isEditor && defaultItemSpawners.Length != 0 || PlayerPrefs.GetInt("isTutorial") == 1 && defaultItemSpawners.Length != 0)
		{
			for (int i = 0; i < this.defaultItemSpawners.Length; i++)
			{
				this.defaultItemSpawners[i].SetActive(true);
			}
		}
		if (PlayerPrefs.GetInt("isTutorial") == 1 && PlayerPrefs.GetInt("completedTraining") == 1)
		{
			for (int j = 0; j < PlayerPrefs.GetInt("EMFReaderInventory"); j++)
			{
				if (this.emfSpawners.Length > j)
				{
					this.emfSpawners[j].SetActive(true);
				}
			}
			for (int k = 0; k < PlayerPrefs.GetInt("FlashlightInventory"); k++)
			{
				if (this.flashlightSpawners.Length > k)
				{
					this.flashlightSpawners[k].SetActive(true);
				}
			}
			for (int l = 0; l < PlayerPrefs.GetInt("CameraInventory"); l++)
			{
				if (this.cameraSpawners.Length > l)
				{
					this.cameraSpawners[l].SetActive(true);
				}
			}
			for (int m = 0; m < PlayerPrefs.GetInt("LighterInventory"); m++)
			{
				if (this.lighterSpawners.Length > m)
				{
					this.lighterSpawners[m].SetActive(true);
				}
			}
			for (int n = 0; n < PlayerPrefs.GetInt("CandleInventory"); n++)
			{
				if (this.candleSpawners.Length > n)
				{
					this.candleSpawners[n].SetActive(true);
				}
			}
			for (int num = 0; num < PlayerPrefs.GetInt("UVFlashlightInventory"); num++)
			{
				if (this.uvFlashlightSpawners.Length > num)
				{
					this.uvFlashlightSpawners[num].SetActive(true);
				}
			}
			for (int num2 = 0; num2 < PlayerPrefs.GetInt("CrucifixInventory"); num2++)
			{
				if (this.crucifixSpawners.Length > num2)
				{
					this.crucifixSpawners[num2].SetActive(true);
				}
			}
			for (int num3 = 0; num3 < PlayerPrefs.GetInt("DSLRCameraInventory"); num3++)
			{
				if (this.dslrCameraSpawners.Length > num3)
				{
					this.dslrCameraSpawners[num3].SetActive(true);
				}
			}
			for (int num4 = 0; num4 < PlayerPrefs.GetInt("EVPRecorderInventory"); num4++)
			{
				if (this.evpRecorderSpawners.Length > num4)
				{
					this.evpRecorderSpawners[num4].SetActive(true);
				}
			}
			for (int num5 = 0; num5 < PlayerPrefs.GetInt("SaltInventory"); num5++)
			{
				if (this.saltSpawners.Length > num5)
				{
					this.saltSpawners[num5].SetActive(true);
				}
			}
			for (int num6 = 0; num6 < PlayerPrefs.GetInt("SageInventory"); num6++)
			{
				if (this.sageSpawners.Length > num6)
				{
					this.sageSpawners[num6].SetActive(true);
				}
			}
			for (int num7 = 0; num7 < PlayerPrefs.GetInt("TripodInventory"); num7++)
			{
				if (this.tripodSpawners.Length > num7)
				{
					this.tripodSpawners[num7].SetActive(true);
				}
			}
			for (int num8 = 0; num8 < PlayerPrefs.GetInt("MotionSensorInventory"); num8++)
			{
				if (this.motionSensorSpawners.Length > num8)
				{
					this.motionSensorSpawners[num8].SetActive(true);
				}
			}
			for (int num9 = 0; num9 < PlayerPrefs.GetInt("SoundSensorInventory"); num9++)
			{
				if (this.soundSensorSpawners.Length > num9)
				{
					this.soundSensorSpawners[num9].SetActive(true);
				}
			}
			for (int num10 = 0; num10 < PlayerPrefs.GetInt("SanityPillsInventory"); num10++)
			{
				if (this.painKillersSpawners.Length > num10)
				{
					this.painKillersSpawners[num10].SetActive(true);
				}
			}
			for (int num11 = 0; num11 < PlayerPrefs.GetInt("ThermometerInventory"); num11++)
			{
				if (this.thermometerSpawners.Length > num11)
				{
					this.thermometerSpawners[num11].SetActive(true);
				}
			}
			for (int num12 = 0; num12 < PlayerPrefs.GetInt("StrongFlashlightInventory"); num12++)
			{
				if (this.strongFlashlightSpawners.Length > num12)
				{
					this.strongFlashlightSpawners[num12].SetActive(true);
				}
			}
			for (int num13 = 0; num13 < PlayerPrefs.GetInt("GhostWritingBookInventory"); num13++)
			{
				if (this.ghostWritingBookSpawners.Length > num13)
				{
					this.ghostWritingBookSpawners[num13].SetActive(true);
				}
			}
			for (int num14 = 0; num14 < PlayerPrefs.GetInt("IRLightSensorInventory"); num14++)
			{
				if (this.irLightSensorSpawners.Length > num14)
				{
					this.irLightSensorSpawners[num14].SetActive(true);
				}
			}
			for (int num15 = 0; num15 < PlayerPrefs.GetInt("ParabolicMicrophoneInventory"); num15++)
			{
				if (this.parabolicMicrophoneSpawners.Length > num15)
				{
					this.parabolicMicrophoneSpawners[num15].SetActive(true);
				}
			}
			for (int num16 = 0; num16 < PlayerPrefs.GetInt("GlowstickInventory"); num16++)
			{
				if (this.glowstickSpawners.Length > num16)
				{
					this.glowstickSpawners[num16].SetActive(true);
				}
			}
			for (int num17 = 0; num17 < PlayerPrefs.GetInt("HeadMountedCameraInventory"); num17++)
			{
				if (this.headMountedCameras.Length > num17)
				{
					this.headMountedCameras[num17].gameObject.SetActive(true);
				}
			}
			return;
		}
		if (PhotonNetwork.InRoom)
		{
			this.view.RPC("SpawnHeadMountedCameras", RpcTarget.AllBuffered, new object[]
			{
				PlayerPrefs.GetInt("totalHeadMountedCameraAmount")
			});
		}
		else
		{
			this.SpawnHeadMountedCameras(PlayerPrefs.GetInt("totalHeadMountedCameraAmount"));
		}
		for (int num18 = 0; num18 < PlayerPrefs.GetInt("totalEMFReaderAmount"); num18++)
		{
			if (this.emfSpawners[num18] != null)
			{
				this.emfSpawners[num18].SetActive(true);
			}
		}
		for (int num19 = 0; num19 < PlayerPrefs.GetInt("totalFlashlightAmount"); num19++)
		{
			if (this.flashlightSpawners[num19] != null)
			{
				this.flashlightSpawners[num19].SetActive(true);
			}
		}
		for (int num20 = 0; num20 < PlayerPrefs.GetInt("totalCameraAmount"); num20++)
		{
			if (this.cameraSpawners[num20] != null)
			{
				this.cameraSpawners[num20].SetActive(true);
			}
		}
		for (int num21 = 0; num21 < PlayerPrefs.GetInt("totalLighterAmount"); num21++)
		{
			if (this.lighterSpawners[num21] != null)
			{
				this.lighterSpawners[num21].SetActive(true);
			}
		}
		for (int num22 = 0; num22 < PlayerPrefs.GetInt("totalCandleAmount"); num22++)
		{
			if (this.candleSpawners[num22] != null)
			{
				this.candleSpawners[num22].SetActive(true);
			}
		}
		for (int num23 = 0; num23 < PlayerPrefs.GetInt("totalUVFlashlightAmount"); num23++)
		{
			if (this.uvFlashlightSpawners[num23] != null)
			{
				this.uvFlashlightSpawners[num23].SetActive(true);
			}
		}
		for (int num24 = 0; num24 < PlayerPrefs.GetInt("totalCrucifixAmount"); num24++)
		{
			if (this.crucifixSpawners[num24] != null)
			{
				this.crucifixSpawners[num24].SetActive(true);
			}
		}
		for (int num25 = 0; num25 < PlayerPrefs.GetInt("totalDSLRCameraAmount"); num25++)
		{
			if (this.dslrCameraSpawners[num25] != null)
			{
				this.dslrCameraSpawners[num25].SetActive(true);
			}
		}
		for (int num26 = 0; num26 < PlayerPrefs.GetInt("totalEVPRecorderAmount"); num26++)
		{
			if (this.evpRecorderSpawners[num26] != null)
			{
				this.evpRecorderSpawners[num26].SetActive(true);
			}
		}
		for (int num27 = 0; num27 < PlayerPrefs.GetInt("totalSaltAmount"); num27++)
		{
			if (this.saltSpawners[num27] != null)
			{
				this.saltSpawners[num27].SetActive(true);
			}
		}
		for (int num28 = 0; num28 < PlayerPrefs.GetInt("totalSageAmount"); num28++)
		{
			if (this.sageSpawners[num28] != null)
			{
				this.sageSpawners[num28].SetActive(true);
			}
		}
		for (int num29 = 0; num29 < PlayerPrefs.GetInt("totalTripodAmount"); num29++)
		{
			if (this.tripodSpawners[num29] != null)
			{
				this.tripodSpawners[num29].SetActive(true);
			}
		}
		for (int num30 = 0; num30 < PlayerPrefs.GetInt("totalMotionSensorAmount"); num30++)
		{
			if (this.motionSensorSpawners[num30] != null)
			{
				this.motionSensorSpawners[num30].SetActive(true);
			}
		}
		for (int num31 = 0; num31 < PlayerPrefs.GetInt("totalSoundSensorAmount"); num31++)
		{
			if (this.soundSensorSpawners[num31] != null)
			{
				this.soundSensorSpawners[num31].SetActive(true);
			}
		}
		for (int num32 = 0; num32 < PlayerPrefs.GetInt("totalSanityPillsAmount"); num32++)
		{
			if (this.painKillersSpawners[num32] != null)
			{
				this.painKillersSpawners[num32].SetActive(true);
			}
		}
		for (int num33 = 0; num33 < PlayerPrefs.GetInt("totalThermometerAmount"); num33++)
		{
			if (this.thermometerSpawners[num33] != null)
			{
				this.thermometerSpawners[num33].SetActive(true);
			}
		}
		for (int num34 = 0; num34 < PlayerPrefs.GetInt("totalStrongFlashlightAmount"); num34++)
		{
			if (this.strongFlashlightSpawners[num34] != null)
			{
				this.strongFlashlightSpawners[num34].SetActive(true);
			}
		}
		for (int num35 = 0; num35 < PlayerPrefs.GetInt("totalGhostWritingBookAmount"); num35++)
		{
			if (this.ghostWritingBookSpawners[num35] != null)
			{
				this.ghostWritingBookSpawners[num35].SetActive(true);
			}
		}
		for (int num36 = 0; num36 < PlayerPrefs.GetInt("totalIRLightSensorAmount"); num36++)
		{
			if (this.irLightSensorSpawners[num36] != null)
			{
				this.irLightSensorSpawners[num36].SetActive(true);
			}
		}
		for (int num37 = 0; num37 < PlayerPrefs.GetInt("totalParabolicMicrophoneAmount"); num37++)
		{
			if (this.parabolicMicrophoneSpawners[num37] != null)
			{
				this.parabolicMicrophoneSpawners[num37].SetActive(true);
			}
		}
		for (int num38 = 0; num38 < PlayerPrefs.GetInt("totalGlowstickAmount"); num38++)
		{
			if (this.glowstickSpawners[num38] != null)
			{
				this.glowstickSpawners[num38].SetActive(true);
			}
		}
		this.CheckIfAnyItemsHaveSpawned();
	}

	[PunRPC]
	private void SpawnHeadMountedCameras(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			if (this.headMountedCameras[i] != null)
			{
				this.headMountedCameras[i].gameObject.SetActive(true);
			}
		}
	}

	private IEnumerator SpawnHeadMountedCamerasDelay(int amount)
	{
		yield return new WaitForSeconds(2f);
		for (int i = 0; i < amount; i++)
		{
			if (this.headMountedCameras[i] != null)
			{
				this.headMountedCameras[i].gameObject.SetActive(true);
			}
		}
		yield break;
	}

	private void CheckIfAnyItemsHaveSpawned()
	{
		this.hasSpawnedOtherItems = false;
		if (PlayerPrefs.GetInt("totalEMFReaderAmount") > 0 || PlayerPrefs.GetInt("totalCameraAmount") > 0 || PlayerPrefs.GetInt("totalLighterAmount") > 0 || PlayerPrefs.GetInt("totalCandleAmount") > 0 || PlayerPrefs.GetInt("totalUVFlashlightAmount") > 0 || PlayerPrefs.GetInt("totalCrucifixAmount") > 0 || PlayerPrefs.GetInt("totalDSLRCameraAmount") > 0 || PlayerPrefs.GetInt("totalEVPRecorderAmount") > 0 || PlayerPrefs.GetInt("totalSaltAmount") > 0 || PlayerPrefs.GetInt("totalSageAmount") > 0 || PlayerPrefs.GetInt("totalTripodAmount") > 0 || PlayerPrefs.GetInt("totalMotionSensorAmount") > 0 || PlayerPrefs.GetInt("totalSoundSensorAmount") > 0 || PlayerPrefs.GetInt("totalSanityPillsAmount") > 0 || PlayerPrefs.GetInt("totalThermometerAmount") > 0 || PlayerPrefs.GetInt("totalStrongFlashlightAmount") > 0 || PlayerPrefs.GetInt("totalGhostWritingBookAmount") > 0 || PlayerPrefs.GetInt("totalIRLightSensorAmount") > 0 || PlayerPrefs.GetInt("ParabolicMicrophoneAmount") > 0 || PlayerPrefs.GetInt("GlowstickAmount") > 0 || PlayerPrefs.GetInt("HeadMountedCameraAmount") > 0)
		{
			this.hasSpawnedOtherItems = true;
		}
	}

	private void CheckSanityPillsOwners()
	{
		if (PlayerPrefs.GetInt("currentSanityPillsAmount") > 0)
		{
			this.view.RPC("AddSanityPillOwner", PhotonNetwork.MasterClient, new object[]
			{
				PhotonNetwork.LocalPlayer,
				PlayerPrefs.GetInt("currentSanityPillsAmount")
			});
		}
	}

	[PunRPC]
	private void AddSanityPillOwner(Photon.Realtime.Player player, int amount)
	{
		this.playersWhoOwnSanityPills.Add(player);
		this.playerSanityPillAmounts.Add(amount);
	}

	[PunRPC]
	private void RemoveSanityPillFromOwner()
	{
		if (GameController.instance.isTutorial)
		{
			return;
		}
		PlayerPrefs.SetInt("SanityPillsInventory", PlayerPrefs.GetInt("SanityPillsInventory") - 1);
	}

	public void RemovePainKillers()
	{
		if (GameController.instance.isTutorial)
		{
			return;
		}
		int num = UnityEngine.Random.Range(0, this.playersWhoOwnSanityPills.Count);
		List<int> list = this.playerSanityPillAmounts;
		int i = num;
		list[i]--;
		bool flag = false;
		Photon.Realtime.Player[] playerList = PhotonNetwork.PlayerList;
		for (i = 0; i < playerList.Length; i++)
		{
			if (playerList[i] == this.playersWhoOwnSanityPills[num])
			{
				flag = true;
			}
		}
		if (flag)
		{
			this.view.RPC("RemoveSanityPillFromOwner", this.playersWhoOwnSanityPills[num], Array.Empty<object>());
			if (this.playerSanityPillAmounts[num] == 0)
			{
				this.playerSanityPillAmounts.RemoveAt(num);
				this.playersWhoOwnSanityPills.RemoveAt(num);
			}
		}
	}

	[SerializeField]
	private GameObject[] emfSpawners;

	[SerializeField]
	private GameObject[] flashlightSpawners;

	[SerializeField]
	private GameObject[] cameraSpawners;

	[SerializeField]
	private GameObject[] lighterSpawners;

	[SerializeField]
	private GameObject[] candleSpawners;

	[SerializeField]
	private GameObject[] uvFlashlightSpawners;

	[SerializeField]
	private GameObject[] crucifixSpawners;

	[SerializeField]
	private GameObject[] dslrCameraSpawners;

	[SerializeField]
	private GameObject[] evpRecorderSpawners;

	[SerializeField]
	private GameObject[] saltSpawners;

	[SerializeField]
	private GameObject[] sageSpawners;

	[SerializeField]
	private GameObject[] tripodSpawners;

	[SerializeField]
	private GameObject[] motionSensorSpawners;

	[SerializeField]
	private GameObject[] soundSensorSpawners;

	[SerializeField]
	private GameObject[] painKillersSpawners;

	[SerializeField]
	private GameObject[] thermometerSpawners;

	[SerializeField]
	private GameObject[] strongFlashlightSpawners;

	[SerializeField]
	private GameObject[] ghostWritingBookSpawners;

	[SerializeField]
	private GameObject[] irLightSensorSpawners;

	[SerializeField]
	private GameObject[] parabolicMicrophoneSpawners;

	[SerializeField]
	private GameObject[] glowstickSpawners;

	[SerializeField]
	private CCTV[] headMountedCameras;

	[SerializeField]
	private GameObject[] defaultItemSpawners;

	private List<Photon.Realtime.Player> playersWhoOwnSanityPills = new List<Photon.Realtime.Player>();

	private List<int> playerSanityPillAmounts = new List<int>();

	[SerializeField]
	private PhotonView view;

	private bool hasSpawned;

	[HideInInspector]
	public bool hasSpawnedOtherItems;
}


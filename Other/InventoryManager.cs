using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviourPunCallbacks
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		for (int i = 0; i < this.maskTransforms.Length; i++)
		{
			this.maskTransforms[i].sizeDelta = new Vector2(0f, 0f);
		}
		this.maskTransforms[0].sizeDelta = new Vector2(1200f, 1200f);
	}

	public void AddButton(InventoryItem item)
	{
		if (item.canChangeAmount && item.totalAmount < item.maxAmount)
		{
			int i = 0;
			while (i < item.players.Count)
			{
				if (item.players[i].isLocalPlayer)
				{
					if (PlayerPrefs.GetInt(item.itemName + "Inventory") > item.players[i].currentAmount)
					{
						item.canChangeAmount = false;
						item.ChangeTotalAmount(int.Parse(PhotonNetwork.LocalPlayer.UserId), 1);
						return;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}
	}

	public void UpdateText()
	{
		this.inventoryText.text = "";
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].totalAmount > 0)
			{
				Text text = this.inventoryText;
				text.text = string.Concat(new object[]
				{
					text.text,
					(i == 0) ? "" : "\n",
					LocalisationSystem.GetLocalisedValue(this.items[i].localisedItemName),
					" x ",
					this.items[i].totalAmount
				});
			}
		}
	}

	public void MinusButton(InventoryItem item)
	{
		if (item.canChangeAmount)
		{
			for (int i = 0; i < item.players.Count; i++)
			{
				if (item.players[i].isLocalPlayer && item.players[i].currentAmount >= 1)
				{
					item.canChangeAmount = false;
					item.ChangeTotalAmount(int.Parse(PhotonNetwork.LocalPlayer.UserId), -1);
					return;
				}
			}
		}
	}

	public void LeftRoom()
	{
	}

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
		if (PhotonNetwork.IsMasterClient)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				this.items[i].LeftRoom(int.Parse(otherPlayer.UserId));
			}
		}
	}

	public void SaveInventory()
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			this.items[i].SaveItem();
		}
	}

	public void ChangePageButton(int pageID)
	{
		for (int i = 0; i < this.maskTransforms.Length; i++)
		{
			this.maskTransforms[i].sizeDelta = new Vector2(0f, 0f);
		}
		this.maskTransforms[pageID].sizeDelta = new Vector2(1200f, 1200f);
	}

	public static void RemoveItemsFromInventory()
	{
		PlayerPrefs.SetInt("EMFReaderInventory", PlayerPrefs.GetInt("EMFReaderInventory") - PlayerPrefs.GetInt("currentEMFReaderAmount"));
		PlayerPrefs.SetInt("FlashlightInventory", PlayerPrefs.GetInt("FlashlightInventory") - PlayerPrefs.GetInt("currentFlashlightAmount"));
		PlayerPrefs.SetInt("CameraInventory", PlayerPrefs.GetInt("CameraInventory") - PlayerPrefs.GetInt("currentCameraAmount"));
		PlayerPrefs.SetInt("LighterInventory", PlayerPrefs.GetInt("LighterInventory") - PlayerPrefs.GetInt("currentLighterAmount"));
		PlayerPrefs.SetInt("CandleInventory", PlayerPrefs.GetInt("CandleInventory") - PlayerPrefs.GetInt("currentCandleAmount"));
		PlayerPrefs.SetInt("UVFlashlightInventory", PlayerPrefs.GetInt("UVFlashlightInventory") - PlayerPrefs.GetInt("currentUVFlashlightAmount"));
		PlayerPrefs.SetInt("CrucifixInventory", PlayerPrefs.GetInt("CrucifixInventory") - PlayerPrefs.GetInt("currentCrucifixAmount"));
		PlayerPrefs.SetInt("DSLRCameraInventory", PlayerPrefs.GetInt("DSLRCameraInventory") - PlayerPrefs.GetInt("currentDSLRCameraAmount"));
		PlayerPrefs.SetInt("EVPRecorderInventory", PlayerPrefs.GetInt("EVPRecorderInventory") - PlayerPrefs.GetInt("currentEVPRecorderAmount"));
		PlayerPrefs.SetInt("SaltInventory", PlayerPrefs.GetInt("SaltInventory") - PlayerPrefs.GetInt("currentSaltAmount"));
		PlayerPrefs.SetInt("SageInventory", PlayerPrefs.GetInt("SageInventory") - PlayerPrefs.GetInt("currentSageAmount"));
		PlayerPrefs.SetInt("TripodInventory", PlayerPrefs.GetInt("TripodInventory") - PlayerPrefs.GetInt("currentTripodAmount"));
		PlayerPrefs.SetInt("StrongFlashlightInventory", PlayerPrefs.GetInt("StrongFlashlightInventory") - PlayerPrefs.GetInt("currentStrongFlashlightAmount"));
		PlayerPrefs.SetInt("MotionSensorInventory", PlayerPrefs.GetInt("MotionSensorInventory") - PlayerPrefs.GetInt("currentMotionSensorAmount"));
		PlayerPrefs.SetInt("SoundSensorInventory", PlayerPrefs.GetInt("SoundSensorInventory") - PlayerPrefs.GetInt("currentSoundSensorAmount"));
		PlayerPrefs.SetInt("SanityPillsInventory", PlayerPrefs.GetInt("SanityPillsInventory") - PlayerPrefs.GetInt("currentSanityPillsAmount"));
		PlayerPrefs.SetInt("ThermometerInventory", PlayerPrefs.GetInt("ThermometerInventory") - PlayerPrefs.GetInt("currentThermometerAmount"));
		PlayerPrefs.SetInt("GhostWritingBookInventory", PlayerPrefs.GetInt("GhostWritingBookInventory") - PlayerPrefs.GetInt("currentGhostWritingBookAmount"));
		PlayerPrefs.SetInt("IRLightSensorInventory", PlayerPrefs.GetInt("IRLightSensorInventory") - PlayerPrefs.GetInt("currentIRLightSensorAmount"));
		PlayerPrefs.SetInt("ParabolicMicrophoneInventory", PlayerPrefs.GetInt("ParabolicMicrophoneInventory") - PlayerPrefs.GetInt("currentParabolicMicrophoneAmount"));
		PlayerPrefs.SetInt("GlowstickInventory", PlayerPrefs.GetInt("GlowstickInventory") - PlayerPrefs.GetInt("currentGlowstickAmount"));
		PlayerPrefs.SetInt("HeadMountedCameraInventory", PlayerPrefs.GetInt("HeadMountedCameraInventory") - PlayerPrefs.GetInt("currentHeadMountedCameraAmount"));
		InventoryManager.ResetTemporaryInventory();
	}

	public static void ResetTemporaryInventory()
	{
		PlayerPrefs.SetInt("currentEMFReaderAmount", 0);
		PlayerPrefs.SetInt("currentFlashlightAmount", 0);
		PlayerPrefs.SetInt("currentCameraAmount", 0);
		PlayerPrefs.SetInt("currentLighterAmount", 0);
		PlayerPrefs.SetInt("currentCandleAmount", 0);
		PlayerPrefs.SetInt("currentUVFlashlightAmount", 0);
		PlayerPrefs.SetInt("currentCrucifixAmount", 0);
		PlayerPrefs.SetInt("currentDSLRCameraAmount", 0);
		PlayerPrefs.SetInt("currentEVPRecorderAmount", 0);
		PlayerPrefs.SetInt("currentSaltAmount", 0);
		PlayerPrefs.SetInt("currentSageAmount", 0);
		PlayerPrefs.SetInt("currentTripodAmount", 0);
		PlayerPrefs.SetInt("currentStrongFlashlightAmount", 0);
		PlayerPrefs.SetInt("currentMotionSensorAmount", 0);
		PlayerPrefs.SetInt("currentSoundSensorAmount", 0);
		PlayerPrefs.SetInt("currentSanityPillsAmount", 0);
		PlayerPrefs.SetInt("currentThermometerAmount", 0);
		PlayerPrefs.SetInt("currentGhostWritingBookAmount", 0);
		PlayerPrefs.SetInt("currentIRLightSensorAmount", 0);
		PlayerPrefs.SetInt("currentParabolicMicrophoneAmount", 0);
		PlayerPrefs.SetInt("currentGlowstickAmount", 0);
		PlayerPrefs.SetInt("currentHeadMountedCameraAmount", 0);
		PlayerPrefs.SetInt("totalEMFReaderAmount", 0);
		PlayerPrefs.SetInt("totalFlashlightAmount", 0);
		PlayerPrefs.SetInt("totalCameraAmount", 0);
		PlayerPrefs.SetInt("totalLighterAmount", 0);
		PlayerPrefs.SetInt("totalCandleAmount", 0);
		PlayerPrefs.SetInt("totalUVFlashlightAmount", 0);
		PlayerPrefs.SetInt("totalCrucifixAmount", 0);
		PlayerPrefs.SetInt("totalDSLRCameraAmount", 0);
		PlayerPrefs.SetInt("totalEVPRecorderAmount", 0);
		PlayerPrefs.SetInt("totalSaltAmount", 0);
		PlayerPrefs.SetInt("totalSageAmount", 0);
		PlayerPrefs.SetInt("totalTripodAmount", 0);
		PlayerPrefs.SetInt("totalStrongFlashlightAmount", 0);
		PlayerPrefs.SetInt("totalMotionSensorAmount", 0);
		PlayerPrefs.SetInt("totalSoundSensorAmount", 0);
		PlayerPrefs.SetInt("totalSanityPillsAmount", 0);
		PlayerPrefs.SetInt("totalThermometerAmount", 0);
		PlayerPrefs.SetInt("totalGhostWritingBookAmount", 0);
		PlayerPrefs.SetInt("totalIRLightSensorAmount", 0);
		PlayerPrefs.SetInt("totalParabolicMicrophoneAmount", 0);
		PlayerPrefs.SetInt("totalGlowstickAmount", 0);
		PlayerPrefs.SetInt("totalGHeadMountedCameraAmount", 0);
	}

	private PhotonView view;

	public List<InventoryItem> items = new List<InventoryItem>();

	[SerializeField]
	private RectTransform[] maskTransforms;

	[SerializeField]
	private Text inventoryText;
}


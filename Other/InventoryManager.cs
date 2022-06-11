using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200013A RID: 314
public class InventoryManager : MonoBehaviourPunCallbacks
{
	// Token: 0x060008B9 RID: 2233 RVA: 0x000353A4 File Offset: 0x000335A4
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		for (int i = 0; i < this.maskTransforms.Length; i++)
		{
			this.maskTransforms[i].sizeDelta = new Vector2(0f, 0f);
		}
		this.maskTransforms[0].sizeDelta = new Vector2(1200f, 1200f);
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x00035408 File Offset: 0x00033608
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

	// Token: 0x060008BB RID: 2235 RVA: 0x00035498 File Offset: 0x00033698
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

	// Token: 0x060008BC RID: 2236 RVA: 0x00035554 File Offset: 0x00033754
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

	// Token: 0x060008BD RID: 2237 RVA: 0x00003D4C File Offset: 0x00001F4C
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

	// Token: 0x060008BF RID: 2239 RVA: 0x00035604 File Offset: 0x00033804
	public void SaveInventory()
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			this.items[i].SaveItem();
		}
	}

	// Token: 0x060008C0 RID: 2240 RVA: 0x00035638 File Offset: 0x00033838
	public void ChangePageButton(int pageID)
	{
		for (int i = 0; i < this.maskTransforms.Length; i++)
		{
			this.maskTransforms[i].sizeDelta = new Vector2(0f, 0f);
		}
		this.maskTransforms[pageID].sizeDelta = new Vector2(1200f, 1200f);
	}

	// Token: 0x060008C1 RID: 2241 RVA: 0x000356B0 File Offset: 0x000338B0
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

	// Token: 0x060008C2 RID: 2242 RVA: 0x0003596C File Offset: 0x00033B6C
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

	// Token: 0x040008C6 RID: 2246
	private PhotonView view;

	// Token: 0x040008C7 RID: 2247
	public List<InventoryItem> items = new List<InventoryItem>();

	// Token: 0x040008C8 RID: 2248
	[SerializeField]
	private RectTransform[] maskTransforms;

	// Token: 0x040008C9 RID: 2249
	[SerializeField]
	private Text inventoryText;
}

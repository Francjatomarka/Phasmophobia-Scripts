using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// Token: 0x02000136 RID: 310
public class FailureManager : MonoBehaviour
{
	// Token: 0x0600088D RID: 2189 RVA: 0x000343D2 File Offset: 0x000325D2
	private void Awake()
	{
		if (PlayerPrefs.GetInt("PlayerDied") == 1)
		{
			InventoryManager.RemoveItemsFromInventory();
		}
		PlayerPrefs.SetInt("PlayerDied", 0);
	}

	// Token: 0x0600088E RID: 2190 RVA: 0x00034400 File Offset: 0x00032600
	private void Start()
	{
		if (PlayerPrefs.GetInt("setupPhase") == 1 && PlayerPrefs.GetInt("LevelDifficulty") == 0)
		{
			PlayerPrefs.SetInt("PlayerMoney", PlayerPrefs.GetInt("PlayerMoney") + 10);
			this.rewardMessage.text = LocalisationSystem.GetLocalisedValue("Failure_ContractPay") + ": $10";
		}
		else
		{
			this.rewardMessage.text = LocalisationSystem.GetLocalisedValue("Failure_ContractPay") + ": $0";
		}
		PlayerPrefs.SetInt("setupPhase", 0);
		this.storeManager.UpdatePlayerMoneyText();
		PlayerPrefs.SetInt("MissionStatus", 0);
	}

	// Token: 0x0600088F RID: 2191 RVA: 0x0003449E File Offset: 0x0003269E
	public void ResumeButton()
	{
		if (PhotonNetwork.InRoom)
		{
			MainManager.instance.serverManager.EnableMasks(true);
			base.gameObject.SetActive(false);
			return;
		}
		this.mainObject.SetActive(true);
		base.gameObject.SetActive(false);
	}

	// Token: 0x040008A1 RID: 2209
	[SerializeField]
	private Text rewardMessage;

	// Token: 0x040008A2 RID: 2210
	[SerializeField]
	private Text failureMessage;

	// Token: 0x040008A3 RID: 2211
	[SerializeField]
	private StoreManager storeManager;

	// Token: 0x040008A4 RID: 2212
	[SerializeField]
	private GameObject mainObject;
}

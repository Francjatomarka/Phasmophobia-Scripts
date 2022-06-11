using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// Token: 0x0200014C RID: 332
public class StoreManager : MonoBehaviour
{
	// Token: 0x06000959 RID: 2393 RVA: 0x00039CF5 File Offset: 0x00037EF5
	private void Awake()
	{
		this.UpdatePlayerMoneyText();
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x00039CFD File Offset: 0x00037EFD
	private void Start()
	{
		this.myTotalExp = PlayerPrefs.GetInt("myTotalExp");
		if (Application.isEditor || this.storeSDKManager.storeBranchType == StoreSDKManager.StoreBranchType.youtube)
		{
			this.freeMoneyButton.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600095B RID: 2395 RVA: 0x00039D38 File Offset: 0x00037F38
	public void ItemButtonPress(StoreItem item)
	{
		this.currentItem = item;
		for (int i = 0; i < this.itemCards.Length; i++)
		{
			this.itemCards[i].SetActive(false);
		}
		this.currentItem.description.SetActive(true);
		this.currentItem.amountOwnedText.text = LocalisationSystem.GetLocalisedValue("Store_Owned") + ": " + PlayerPrefs.GetInt(this.currentItem.itemName + "Inventory");
		if (Mathf.FloorToInt((float)(this.myTotalExp / 100)) < item.requiredLevel && this.storeSDKManager.storeBranchType != StoreSDKManager.StoreBranchType.youtube)
		{
			this.currentItem.buyButton.interactable = false;
			this.currentItem.buyButtonText.color = new Color32(50, 50, 50, 119);
			this.currentItem.amountOwnedText.text = LocalisationSystem.GetLocalisedValue("Experience_Required") + item.requiredLevel;
		}
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x00039E44 File Offset: 0x00038044
	public void BuyButton()
	{
		if (PlayerPrefs.GetInt("PlayerMoney") >= this.currentItem.cost)
		{
			PlayerPrefs.SetInt(this.currentItem.itemName + "Inventory", PlayerPrefs.GetInt(this.currentItem.itemName + "Inventory") + 1);
			PlayerPrefs.SetInt("PlayerMoney", PlayerPrefs.GetInt("PlayerMoney") - this.currentItem.cost);
			this.UpdatePlayerMoneyText();
			this.currentItem.amountOwnedText.text = "Owned: " + PlayerPrefs.GetInt(this.currentItem.itemName + "Inventory");
			DailyChallengesController.Instance.ChangeChallengeProgression(ChallengeType.BuyAnItem, 1);
		}
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x00039F0C File Offset: 0x0003810C
	public void BackButton()
	{
		if (PhotonNetwork.InRoom)
		{
			this.serverManager.OpenStore(false);
			return;
		}
		this.mainObject.SetActive(true);
		this.storeObject.SetActive(false);
	}

	// Token: 0x0600095E RID: 2398 RVA: 0x00039F3C File Offset: 0x0003813C
	public void UpdatePlayerMoneyText()
	{
		this.playerMoneyText.text = "$" + PlayerPrefs.GetInt("PlayerMoney").ToString();
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x00039F70 File Offset: 0x00038170
	public void GivePlayerMoneyButton()
	{
		PlayerPrefs.SetInt("PlayerMoney", PlayerPrefs.GetInt("PlayerMoney") + 100);
		this.UpdatePlayerMoneyText();
	}

	// Token: 0x04000998 RID: 2456
	[SerializeField]
	private Text playerMoneyText;

	// Token: 0x04000999 RID: 2457
	[SerializeField]
	private GameObject[] itemCards;

	// Token: 0x0400099A RID: 2458
	private StoreItem currentItem;

	// Token: 0x0400099B RID: 2459
	[SerializeField]
	private Button freeMoneyButton;

	// Token: 0x0400099C RID: 2460
	[SerializeField]
	private GameObject mainObject;

	// Token: 0x0400099D RID: 2461
	[SerializeField]
	private GameObject storeObject;

	// Token: 0x0400099E RID: 2462
	[SerializeField]
	private ServerManager serverManager;

	// Token: 0x0400099F RID: 2463
	[SerializeField]
	private StoreSDKManager storeSDKManager;

	// Token: 0x040009A0 RID: 2464
	private int myTotalExp;
}

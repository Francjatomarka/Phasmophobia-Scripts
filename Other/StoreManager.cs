using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class StoreManager : MonoBehaviour
{
	private void Awake()
	{
		this.UpdatePlayerMoneyText();
	}

	private void Start()
	{
		this.myTotalExp = PlayerPrefs.GetInt("myTotalExp");
		if (Application.isEditor || this.storeSDKManager.storeBranchType == StoreSDKManager.StoreBranchType.youtube)
		{
			this.freeMoneyButton.gameObject.SetActive(true);
		}
	}

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

	public void UpdatePlayerMoneyText()
	{
		this.playerMoneyText.text = "$" + PlayerPrefs.GetInt("PlayerMoney").ToString();
	}

	public void GivePlayerMoneyButton()
	{
		PlayerPrefs.SetInt("PlayerMoney", PlayerPrefs.GetInt("PlayerMoney") + 100);
		this.UpdatePlayerMoneyText();
	}

	[SerializeField]
	private Text playerMoneyText;

	[SerializeField]
	private GameObject[] itemCards;

	private StoreItem currentItem;

	[SerializeField]
	private Button freeMoneyButton;

	[SerializeField]
	private GameObject mainObject;

	[SerializeField]
	private GameObject storeObject;

	[SerializeField]
	private ServerManager serverManager;

	[SerializeField]
	private StoreSDKManager storeSDKManager;

	private int myTotalExp;
}


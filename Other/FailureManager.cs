using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class FailureManager : MonoBehaviour
{
	private void Awake()
	{
		if (PlayerPrefs.GetInt("PlayerDied") == 1)
		{
			InventoryManager.RemoveItemsFromInventory();
		}
		PlayerPrefs.SetInt("PlayerDied", 0);
	}

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

	[SerializeField]
	private Text rewardMessage;

	[SerializeField]
	private Text failureMessage;

	[SerializeField]
	private StoreManager storeManager;

	[SerializeField]
	private GameObject mainObject;
}


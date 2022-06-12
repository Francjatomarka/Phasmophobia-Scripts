using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LevelSelectionManager : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
	}

	private void Start()
	{
		if (this.storeSDKManager.storeBranchType == StoreSDKManager.StoreBranchType.youtube)
		{
			this.AssignYoutuberContracts();
		}
		else
		{
			this.AssignContracts();
		}
		if (this.selectedLevelName == string.Empty)
		{
			this.readyButton.interactable = false;
			this.readyText.color = new Color32(50, 50, 50, 119);
		}
		this.localisedExperience_Required = LocalisationSystem.GetLocalisedValue("Experience_Required");
	}

	private void AssignContracts()
	{
		for (int i = 0; i < this.contracts.Count; i++)
		{
			Contract value = this.contracts[i];
			int index = UnityEngine.Random.Range(i, this.contracts.Count);
			this.contracts[i] = this.contracts[index];
			this.contracts[index] = value;
			this.contracts[i].gameObject.SetActive(false);
		}
		int num = UnityEngine.Random.Range(2, 5);
		for (int j = 0; j < num; j++)
		{
			this.contracts[j].gameObject.SetActive(true);
			this.contracts[j].GenerateContract();
		}
		this.ForceGenerateSmallContract(num);
	}

	private void AssignYoutuberContracts()
	{
		this.contracts[0].gameObject.SetActive(true);
		this.contracts[0].GenerateSmallLevel(true, 0);
		this.contracts[0].levelNameText.text = this.contracts[0].levelName;
		this.contracts[1].gameObject.SetActive(true);
		this.contracts[1].GenerateSmallLevel(true, 1);
		this.contracts[1].levelNameText.text = this.contracts[1].levelName;
		this.contracts[2].gameObject.SetActive(true);
		this.contracts[2].GenerateSmallLevel(true, 2);
		this.contracts[2].levelNameText.text = this.contracts[2].levelName;
		this.contracts[3].gameObject.SetActive(true);
		this.contracts[3].GenerateSmallLevel(true, 3);
		this.contracts[3].levelNameText.text = this.contracts[3].levelName;
		this.contracts[4].gameObject.SetActive(true);
		this.contracts[4].GenerateSmallLevel(true, 4);
		this.contracts[4].levelNameText.text = this.contracts[4].levelName;
		this.contracts[5].gameObject.SetActive(true);
		this.contracts[5].GenerateMediumLevel();
		this.contracts[5].levelNameText.text = this.contracts[5].levelName;
		this.contracts[6].gameObject.SetActive(true);
		this.contracts[6].GenerateLargeLevel();
		this.contracts[6].levelNameText.text = this.contracts[6].levelName;
	}

	public void SelectContractButton(Contract contract)
	{
		this.selectedContract = contract;
		this.ShowContract();
	}

	public void SyncContract()
	{
		this.view.RPC("NetworkedLevelSelect", RpcTarget.AllBufferedViaServer, new object[]
		{
			this.selectedContract.levelName,
			this.selectedContract.levelType.ToString(),
			(int)this.selectedContract.levelDiffulty
		});
	}

	public void SelectButton()
	{
		this.view.RPC("NetworkedLevelSelect", RpcTarget.AllBufferedViaServer, new object[]
		{
			this.selectedContract.levelName,
			this.selectedContract.levelType.ToString(),
			(int)this.selectedContract.levelDiffulty
		});
		this.descriptionObject.SetActive(false);
		this.mapObject.SetActive(true);
		this.serverManager.SelectJob(false);
	}

	[PunRPC]
	public void NetworkedLevelSelect(string name, string levelName, int difficulty)
	{
		this.selectedLevelName = levelName;
		this.contractLevelName = name;
		this.contractLevelDifficulty = (Contract.LevelDifficulty)difficulty;
		this.serverManager.UpdateUI();
		PlayerPrefs.SetInt("LevelDifficulty", difficulty);
	}

	private void ShowContract()
	{
		this.titleText.text = this.selectedContract.levelName;
		this.descriptionText.text = string.Concat(new string[]
		{
			this.selectedContract.basicDescription,
			" ",
			this.selectedContract.secondDescription,
			" ",
			this.selectedContract.thirdDescription
		});
		this.firstBulletPointText.text = this.selectedContract.firstBulletPoint;
		this.secondBulletPointText.text = this.selectedContract.secondBulletPoint;
		this.thirdBulletPointText.text = this.selectedContract.thirdBulletPoint;
		if (this.selectedContract.levelDiffulty == Contract.LevelDifficulty.Amateur)
		{
			this.difficultyLevelText.text = LocalisationSystem.GetLocalisedValue("Contract_Amateur");
		}
		else if (this.selectedContract.levelDiffulty == Contract.LevelDifficulty.Intermediate)
		{
			this.difficultyLevelText.text = LocalisationSystem.GetLocalisedValue("Contract_Intermediate");
		}
		else
		{
			this.difficultyLevelText.text = LocalisationSystem.GetLocalisedValue("Contract_Professional");
		}
		this.readyButton.interactable = true;
		this.selectButton.interactable = true;
		this.selectButtonText.color = new Color32(50, 50, 50, byte.MaxValue);
		this.descriptionObject.SetActive(true);
		this.mapObject.SetActive(false);
	}

	private void ForceGenerateSmallContract(int amount)
	{
		for (int i = 0; i < this.currentContracts.Count; i++)
		{
			if (this.currentContracts[i].gameObject.activeInHierarchy && this.currentContracts[i].levelSize == Contract.LevelSize.small)
			{
				return;
			}
		}
		Contract contract = this.contracts[amount + 1];
		contract.gameObject.SetActive(true);
		contract.GenerateSmallLevel(false, 0);
		contract.levelNameText.text = contract.levelName;
	}

	[SerializeField]
	private ServerManager serverManager;

	[SerializeField]
	private StoreSDKManager storeSDKManager;

	[HideInInspector]
	public List<Contract> currentContracts = new List<Contract>();

	[SerializeField]
	private List<Contract> contracts = new List<Contract>();

	[HideInInspector]
	public string selectedLevelName;

	[HideInInspector]
	public string contractLevelName;

	[HideInInspector]
	public Contract.LevelDifficulty contractLevelDifficulty;

	[SerializeField]
	private Button readyButton;

	[SerializeField]
	private Text readyText;

	[SerializeField]
	private Text titleText;

	[SerializeField]
	private Text descriptionText;

	[SerializeField]
	private Text firstBulletPointText;

	[SerializeField]
	private Text secondBulletPointText;

	[SerializeField]
	private Text thirdBulletPointText;

	[SerializeField]
	private Text difficultyLevelText;

	[SerializeField]
	private Button selectButton;

	[SerializeField]
	private Text selectButtonText;

	private string localisedExperience_Required;

	private PhotonView view;

	[HideInInspector]
	public Contract selectedContract;

	[SerializeField]
	private GameObject mapObject;

	[SerializeField]
	private GameObject descriptionObject;
}


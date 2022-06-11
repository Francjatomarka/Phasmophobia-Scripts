using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// Token: 0x0200013B RID: 315
public class LevelSelectionManager : MonoBehaviour
{
	// Token: 0x060008C4 RID: 2244 RVA: 0x00035B70 File Offset: 0x00033D70
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x00035B80 File Offset: 0x00033D80
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

	// Token: 0x060008C6 RID: 2246 RVA: 0x00035BF4 File Offset: 0x00033DF4
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

	// Token: 0x060008C7 RID: 2247 RVA: 0x00035CBC File Offset: 0x00033EBC
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

	// Token: 0x060008C8 RID: 2248 RVA: 0x00035EFC File Offset: 0x000340FC
	public void SelectContractButton(Contract contract)
	{
		this.selectedContract = contract;
		this.ShowContract();
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x00035F0C File Offset: 0x0003410C
	public void SyncContract()
	{
		this.view.RPC("NetworkedLevelSelect", RpcTarget.AllBufferedViaServer, new object[]
		{
			this.selectedContract.levelName,
			this.selectedContract.levelType.ToString(),
			(int)this.selectedContract.levelDiffulty
		});
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x00035F6C File Offset: 0x0003416C
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

	// Token: 0x060008CB RID: 2251 RVA: 0x00035FEE File Offset: 0x000341EE
	[PunRPC]
	public void NetworkedLevelSelect(string name, string levelName, int difficulty)
	{
		this.selectedLevelName = levelName;
		this.contractLevelName = name;
		this.contractLevelDifficulty = (Contract.LevelDifficulty)difficulty;
		this.serverManager.UpdateUI();
		PlayerPrefs.SetInt("LevelDifficulty", difficulty);
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x0003601C File Offset: 0x0003421C
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

	// Token: 0x060008CD RID: 2253 RVA: 0x00036180 File Offset: 0x00034380
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

	// Token: 0x040008CC RID: 2252
	[SerializeField]
	private ServerManager serverManager;

	// Token: 0x040008CD RID: 2253
	[SerializeField]
	private StoreSDKManager storeSDKManager;

	// Token: 0x040008CE RID: 2254
	[HideInInspector]
	public List<Contract> currentContracts = new List<Contract>();

	// Token: 0x040008CF RID: 2255
	[SerializeField]
	private List<Contract> contracts = new List<Contract>();

	// Token: 0x040008D0 RID: 2256
	[HideInInspector]
	public string selectedLevelName;

	// Token: 0x040008D1 RID: 2257
	[HideInInspector]
	public string contractLevelName;

	// Token: 0x040008D2 RID: 2258
	[HideInInspector]
	public Contract.LevelDifficulty contractLevelDifficulty;

	// Token: 0x040008D3 RID: 2259
	[SerializeField]
	private Button readyButton;

	// Token: 0x040008D4 RID: 2260
	[SerializeField]
	private Text readyText;

	// Token: 0x040008D5 RID: 2261
	[SerializeField]
	private Text titleText;

	// Token: 0x040008D6 RID: 2262
	[SerializeField]
	private Text descriptionText;

	// Token: 0x040008D7 RID: 2263
	[SerializeField]
	private Text firstBulletPointText;

	// Token: 0x040008D8 RID: 2264
	[SerializeField]
	private Text secondBulletPointText;

	// Token: 0x040008D9 RID: 2265
	[SerializeField]
	private Text thirdBulletPointText;

	// Token: 0x040008DA RID: 2266
	[SerializeField]
	private Text difficultyLevelText;

	// Token: 0x040008DB RID: 2267
	[SerializeField]
	private Button selectButton;

	// Token: 0x040008DC RID: 2268
	[SerializeField]
	private Text selectButtonText;

	// Token: 0x040008DD RID: 2269
	private string localisedExperience_Required;

	// Token: 0x040008DE RID: 2270
	private PhotonView view;

	// Token: 0x040008DF RID: 2271
	[HideInInspector]
	public Contract selectedContract;

	// Token: 0x040008E0 RID: 2272
	[SerializeField]
	private GameObject mapObject;

	// Token: 0x040008E1 RID: 2273
	[SerializeField]
	private GameObject descriptionObject;
}

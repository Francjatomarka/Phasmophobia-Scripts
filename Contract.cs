using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000133 RID: 307
public class Contract : MonoBehaviour
{
	// Token: 0x06000879 RID: 2169 RVA: 0x00033AEC File Offset: 0x00031CEC
	private void Awake()
	{
		this.CreateDescriptions();
	}

	// Token: 0x0600087A RID: 2170 RVA: 0x00033AF4 File Offset: 0x00031CF4
	public void SelectContractButton()
	{
		this.levelSelectionManager.SelectContractButton(this);
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x00033B04 File Offset: 0x00031D04
	private void CreateDescriptions()
	{
		this.secondDescription = LocalisationSystem.GetLocalisedValue("Map_SecondDescription" + UnityEngine.Random.Range(1, 6));
		this.thirdDescription = LocalisationSystem.GetLocalisedValue("Map_ThirdDescription" + UnityEngine.Random.Range(1, 6));
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x00033B54 File Offset: 0x00031D54
	public void GenerateContract()
	{
		int num = Mathf.FloorToInt((float)(PlayerPrefs.GetInt("myTotalExp") / 100));
		if (num < 3)
		{
			this.GenerateSmallLevel(false, 0);
		}
		else if (num < 5)
		{
			if (UnityEngine.Random.Range(0, 2) == 1)
			{
				this.GenerateSmallLevel(false, 0);
			}
			else
			{
				this.GenerateMediumLevel();
			}
		}
		else
		{
			int num2 = UnityEngine.Random.Range(0, 5);
			if (num2 == 0 || num2 == 1)
			{
				this.GenerateSmallLevel(false, 0);
			}
			else if (num2 == 2 || num2 == 3)
			{
				this.GenerateMediumLevel();
			}
			else
			{
				this.GenerateLargeLevel();
			}
		}
		if (num < 10)
		{
			this.levelDiffulty = Contract.LevelDifficulty.Amateur;
		}
		else if (num >= 10 && num < 15)
		{
			if (UnityEngine.Random.Range(0, 4) == 1)
			{
				this.levelDiffulty = Contract.LevelDifficulty.Intermediate;
			}
			else
			{
				this.levelDiffulty = Contract.LevelDifficulty.Amateur;
			}
		}
		else if (num >= 25)
		{
			int num3 = UnityEngine.Random.Range(0, 10);
			if (num3 < 3)
			{
				this.levelDiffulty = Contract.LevelDifficulty.Amateur;
			}
			else if (num3 < 7)
			{
				this.levelDiffulty = Contract.LevelDifficulty.Intermediate;
			}
			else
			{
				this.levelDiffulty = Contract.LevelDifficulty.Professional;
			}
		}
		else
		{
			int num4 = UnityEngine.Random.Range(0, 10);
			if (num4 < 5)
			{
				this.levelDiffulty = Contract.LevelDifficulty.Amateur;
			}
			else if (num4 < 8)
			{
				this.levelDiffulty = Contract.LevelDifficulty.Intermediate;
			}
			else
			{
				this.levelDiffulty = Contract.LevelDifficulty.Professional;
			}
		}
		this.levelNameText.text = this.levelName;
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x00033C78 File Offset: 0x00031E78
	public void GenerateSmallLevel(bool forceLevel, int _levelID)
	{
		int num;
		if (forceLevel)
		{
			num = _levelID;
		}
		else
		{
			num = UnityEngine.Random.Range(0, 5);
		}
		switch (num)
		{
		case 0:
			this.levelName = "Tanglewood Street House";
			this.levelType = Contract.LevelType.Tanglewood_Street_House;
			this.levelSize = Contract.LevelSize.small;
			this.basicDescription = LocalisationSystem.GetLocalisedValue("Map_TanglewoodDescription");
			this.firstBulletPoint = LocalisationSystem.GetLocalisedValue("Map_TeamSize") + "1";
			this.secondBulletPoint = LocalisationSystem.GetLocalisedValue("Map_RecommendedItem") + "None";
			this.thirdBulletPoint = LocalisationSystem.GetLocalisedValue("Map_LocationSizeSmall");
			break;
		case 1:
			this.levelName = "Edgefield Street House";
			this.levelType = Contract.LevelType.Edgefield_Street_House;
			this.levelSize = Contract.LevelSize.small;
			this.basicDescription = LocalisationSystem.GetLocalisedValue("Map_EdgefieldDescription");
			this.firstBulletPoint = LocalisationSystem.GetLocalisedValue("Map_TeamSize") + "2";
			this.secondBulletPoint = LocalisationSystem.GetLocalisedValue("Map_RecommendedItem") + LocalisationSystem.GetLocalisedValue("Equipment_Thermometer");
			this.thirdBulletPoint = LocalisationSystem.GetLocalisedValue("Map_LocationSizeSmall");
			break;
		case 2:
			this.levelName = "Ridgeview Road House";
			this.levelType = Contract.LevelType.Ridgeview_Road_House;
			this.levelSize = Contract.LevelSize.small;
			this.basicDescription = LocalisationSystem.GetLocalisedValue("Map_RidgeviewDescription");
			this.firstBulletPoint = LocalisationSystem.GetLocalisedValue("Map_TeamSize") + "2";
			this.secondBulletPoint = LocalisationSystem.GetLocalisedValue("Map_RecommendedItem") + LocalisationSystem.GetLocalisedValue("Equipment_Thermometer");
			this.thirdBulletPoint = LocalisationSystem.GetLocalisedValue("Map_LocationSizeSmall");
			break;
		case 3:
			this.levelName = "Bleasdale Farmhouse";
			this.levelType = Contract.LevelType.Bleasdale_Farmhouse;
			this.levelSize = Contract.LevelSize.small;
			this.basicDescription = LocalisationSystem.GetLocalisedValue("Map_BleasdaleDescription");
			this.firstBulletPoint = LocalisationSystem.GetLocalisedValue("Map_TeamSize") + "2";
			this.secondBulletPoint = LocalisationSystem.GetLocalisedValue("Map_RecommendedItem") + LocalisationSystem.GetLocalisedValue("Equipment_Thermometer");
			this.thirdBulletPoint = LocalisationSystem.GetLocalisedValue("Map_LocationSizeSmall");
			break;
		case 4:
			this.levelName = "Grafton Farmhouse";
			this.levelType = Contract.LevelType.Grafton_Farmhouse;
			this.levelSize = Contract.LevelSize.small;
			this.basicDescription = LocalisationSystem.GetLocalisedValue("Map_GraftonDescription");
			this.firstBulletPoint = LocalisationSystem.GetLocalisedValue("Map_TeamSize") + "2";
			this.secondBulletPoint = LocalisationSystem.GetLocalisedValue("Map_RecommendedItem") + LocalisationSystem.GetLocalisedValue("Equipment_Thermometer");
			this.thirdBulletPoint = LocalisationSystem.GetLocalisedValue("Map_LocationSizeSmall");
			break;
		default:
			Debug.LogError("Cannot generate a small map with id: " + num);
			break;
		}
		for (int i = 0; i < this.levelSelectionManager.currentContracts.Count; i++)
		{
			if (this.levelSelectionManager.currentContracts[i].levelType == this.levelType)
			{
				this.GenerateContract();
				return;
			}
		}
		this.levelSelectionManager.currentContracts.Add(this);
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x00033F68 File Offset: 0x00032168
	public void GenerateMediumLevel()
	{
		this.levelName = "Brownstone High School";
		this.levelType = Contract.LevelType.Brownstone_High_School;
		this.levelSize = Contract.LevelSize.medium;
		this.basicDescription = LocalisationSystem.GetLocalisedValue("Map_SchoolDescription");
		this.firstBulletPoint = LocalisationSystem.GetLocalisedValue("Map_TeamSize") + "2";
		this.secondBulletPoint = LocalisationSystem.GetLocalisedValue("Map_RecommendedItem") + LocalisationSystem.GetLocalisedValue("Equipment_SanityPills");
		this.thirdBulletPoint = LocalisationSystem.GetLocalisedValue("Map_LocationSizeMedium");
		for (int i = 0; i < this.levelSelectionManager.currentContracts.Count; i++)
		{
			if (this.levelSelectionManager.currentContracts[i].levelType == this.levelType)
			{
				this.GenerateContract();
				return;
			}
		}
		this.levelSelectionManager.currentContracts.Add(this);
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x00034038 File Offset: 0x00032238
	public void GenerateLargeLevel()
	{
		this.levelName = "Asylum";
		this.levelType = Contract.LevelType.Asylum;
		this.levelSize = Contract.LevelSize.large;
		this.basicDescription = LocalisationSystem.GetLocalisedValue("Map_AsylumDescription");
		this.firstBulletPoint = LocalisationSystem.GetLocalisedValue("Map_TeamSize") + "3";
		this.secondBulletPoint = LocalisationSystem.GetLocalisedValue("Map_RecommendedItem") + LocalisationSystem.GetLocalisedValue("Equipment_SanityPills");
		this.thirdBulletPoint = LocalisationSystem.GetLocalisedValue("Map_LocationSizeLarge");
		for (int i = 0; i < this.levelSelectionManager.currentContracts.Count; i++)
		{
			if (this.levelSelectionManager.currentContracts[i].levelType == this.levelType)
			{
				this.GenerateContract();
				return;
			}
		}
		this.levelSelectionManager.currentContracts.Add(this);
	}

	// Token: 0x0400088C RID: 2188
	[HideInInspector]
	public string levelName;

	// Token: 0x0400088D RID: 2189
	[HideInInspector]
	public Contract.LevelType levelType;

	// Token: 0x0400088E RID: 2190
	[HideInInspector]
	public Contract.LevelSize levelSize;

	// Token: 0x0400088F RID: 2191
	[HideInInspector]
	public Contract.LevelDifficulty levelDiffulty;

	// Token: 0x04000890 RID: 2192
	[HideInInspector]
	public string basicDescription;

	// Token: 0x04000891 RID: 2193
	[HideInInspector]
	public string secondDescription;

	// Token: 0x04000892 RID: 2194
	[HideInInspector]
	public string thirdDescription;

	// Token: 0x04000893 RID: 2195
	[HideInInspector]
	public string firstBulletPoint;

	// Token: 0x04000894 RID: 2196
	[HideInInspector]
	public string secondBulletPoint;

	// Token: 0x04000895 RID: 2197
	[HideInInspector]
	public string thirdBulletPoint;

	// Token: 0x04000896 RID: 2198
	[SerializeField]
	private LevelSelectionManager levelSelectionManager;

	// Token: 0x04000897 RID: 2199
	public Text levelNameText;

	// Token: 0x020004E0 RID: 1248
	public enum LevelDifficulty
	{
		// Token: 0x0400230B RID: 8971
		Amateur,
		// Token: 0x0400230C RID: 8972
		Intermediate,
		// Token: 0x0400230D RID: 8973
		Professional
	}

	// Token: 0x020004E1 RID: 1249
	public enum LevelSize
	{
		// Token: 0x0400230F RID: 8975
		none,
		// Token: 0x04002310 RID: 8976
		small,
		// Token: 0x04002311 RID: 8977
		medium,
		// Token: 0x04002312 RID: 8978
		large
	}

	// Token: 0x020004E2 RID: 1250
	public enum LevelType
	{
		// Token: 0x04002314 RID: 8980
		none,
		// Token: 0x04002315 RID: 8981
		Tanglewood_Street_House,
		// Token: 0x04002316 RID: 8982
		Asylum,
		// Token: 0x04002317 RID: 8983
		Edgefield_Street_House,
		// Token: 0x04002318 RID: 8984
		Ridgeview_Road_House,
		// Token: 0x04002319 RID: 8985
		Brownstone_High_School,
		// Token: 0x0400231A RID: 8986
		Bleasdale_Farmhouse,
		// Token: 0x0400231B RID: 8987
		Grafton_Farmhouse
	}
}

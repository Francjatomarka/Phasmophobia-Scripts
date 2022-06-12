using System;
using UnityEngine;
using UnityEngine.UI;

public class Contract : MonoBehaviour
{
	private void Awake()
	{
		this.CreateDescriptions();
	}

	public void SelectContractButton()
	{
		this.levelSelectionManager.SelectContractButton(this);
	}

	private void CreateDescriptions()
	{
		this.secondDescription = LocalisationSystem.GetLocalisedValue("Map_SecondDescription" + UnityEngine.Random.Range(1, 6));
		this.thirdDescription = LocalisationSystem.GetLocalisedValue("Map_ThirdDescription" + UnityEngine.Random.Range(1, 6));
	}

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

	[HideInInspector]
	public string levelName;

	[HideInInspector]
	public Contract.LevelType levelType;

	[HideInInspector]
	public Contract.LevelSize levelSize;

	[HideInInspector]
	public Contract.LevelDifficulty levelDiffulty;

	[HideInInspector]
	public string basicDescription;

	[HideInInspector]
	public string secondDescription;

	[HideInInspector]
	public string thirdDescription;

	[HideInInspector]
	public string firstBulletPoint;

	[HideInInspector]
	public string secondBulletPoint;

	[HideInInspector]
	public string thirdBulletPoint;

	[SerializeField]
	private LevelSelectionManager levelSelectionManager;

	public Text levelNameText;

	public enum LevelDifficulty
	{
		Amateur,
		Intermediate,
		Professional
	}

	public enum LevelSize
	{
		none,
		small,
		medium,
		large
	}

	public enum LevelType
	{
		none,
		Tanglewood_Street_House,
		Asylum,
		Edgefield_Street_House,
		Ridgeview_Road_House,
		Brownstone_High_School,
		Bleasdale_Farmhouse,
		Grafton_Farmhouse
	}
}


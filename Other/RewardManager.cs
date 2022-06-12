using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RewardManager : MonoBehaviour
{
	private void Start()
	{
		this.levelDifficulty = (Contract.LevelDifficulty)PlayerPrefs.GetInt("LevelDifficulty");
		this.SetupExperienceReward();
		this.SetupMoneyReward();
	}

	private void SetupMoneyReward()
	{
		int num;
		if (PlayerPrefs.GetInt("MissionType") == 0)
		{
			num = 10;
		}
		else if (PlayerPrefs.GetInt("MissionType") == 1)
		{
			num = 30;
		}
		else
		{
			num = 45;
		}
		this.mainMissionRewardText.text = ((PlayerPrefs.GetInt("MainMission") == 1) ? ("$" + num) : "$0");
		this.sideMission1RewardText.text = ((PlayerPrefs.GetInt("SideMission1") == 1) ? "$10" : "$0");
		this.sideMission2RewardText.text = ((PlayerPrefs.GetInt("SideMission2") == 1) ? "$10" : "$0");
		this.hiddenMissionRewardText.text = ((PlayerPrefs.GetInt("SideMission3") == 1) ? "$10" : "$0");
		this.totalPhotosRewardText.text = "$" + this.GetPhotosRewardAmount().ToString();
		this.totalDNARewardText.text = ((PlayerPrefs.GetInt("DNAMission") == 1) ? "$10" : "$0");
		this.ghostTypeText.text = LocalisationSystem.GetLocalisedValue("Reward_Ghost") + " " + PlayerPrefs.GetString("GhostType");
		int num2 = 0;
		num2 += ((PlayerPrefs.GetInt("MainMission") == 1) ? num : 0);
		num2 += ((PlayerPrefs.GetInt("SideMission1") == 1) ? 10 : 0);
		num2 += ((PlayerPrefs.GetInt("SideMission2") == 1) ? 10 : 0);
		num2 += ((PlayerPrefs.GetInt("SideMission3") == 1) ? 10 : 0);
		num2 += this.GetPhotosRewardAmount();
		num2 += ((PlayerPrefs.GetInt("DNAMission") == 1) ? 10 : 0);
		if (this.levelDifficulty == Contract.LevelDifficulty.Intermediate)
		{
			num2 *= 2;
		}
		else if (this.levelDifficulty == Contract.LevelDifficulty.Professional)
		{
			num2 *= 3;
		}
		if (PlayerPrefs.GetInt("PlayerDied") == 1)
		{
			this.insuranceAmountText.text = "-$" + (num2 / 2).ToString();
			num2 /= 2;
		}
		else if (PhotonNetwork.PlayerList.Length == 1)
		{
			if (PlayerPrefs.GetInt("MainMission") == 1)
			{
				this.insuranceAmountText.text = "$10";
				num2 += 10;
			}
		}
		else
		{
			this.insuranceAmountText.text = "$0";
		}
		PlayerPrefs.SetInt("PlayerMoney", PlayerPrefs.GetInt("PlayersMoney") + num2);
		this.totalMissionRewardText.text = "$" + num2.ToString();
		InventoryManager.ResetTemporaryInventory();
		PlayerPrefs.SetInt("PhotosMission", 0);
		PlayerPrefs.SetInt("MainMission", 0);
		PlayerPrefs.SetInt("MissionType", 0);
		PlayerPrefs.SetInt("SideMission1", 0);
		PlayerPrefs.SetInt("SideMission2", 0);
		PlayerPrefs.SetInt("SideMission3", 0);
		PlayerPrefs.SetInt("DNAMission", 0);
		PlayerPrefs.SetInt("PhotosMission", 0);
		this.storeManager.UpdatePlayerMoneyText();
		this.playerStatsManager.UpdateMoney();
	}

	private void SetupExperienceReward()
	{
		if (PlayerPrefs.GetInt("PlayerDied") == 1)
		{
			this.mainExperience.SetActive(false);
			this.deadExperience.SetActive(true);
			return;
		}
		int @int = PlayerPrefs.GetInt("myTotalExp");
		int int2 = PlayerPrefs.GetInt("totalExp");
		int num = Mathf.FloorToInt((float)(@int / 100));
		int num2 = num + 1;
		this.currentLevelText.text = num.ToString();
		this.nextLevelText.text = num2.ToString();
		this.experienceGainedText.text = LocalisationSystem.GetLocalisedValue("Experience_Gained") + int2.ToString();
		if (Mathf.FloorToInt((float)((@int - int2) / 100)) < num && int2 > 0)
		{
			this.levelUpText.enabled = true;
			this.levelUpText.text = LocalisationSystem.GetLocalisedValue("Experience_Congrats") + num.ToString();
			this.CheckUnlocks(num);
		}
		else
		{
			this.levelUpText.enabled = false;
		}
		this.expSlider.value = (float)(100 - (num2 * 100 - @int));
		this.expSliderValueText.text = 100 - (num2 * 100 - @int) + "/100 XP".ToString();
		this.playerStatsManager.UpdateLevel();
		this.playerStatsManager.UpdateExperience();
	}

	private int GetPhotosRewardAmount()
	{
		if (PlayerPrefs.GetInt("PhotosMission") == 0)
		{
			return 0;
		}
		if (PlayerPrefs.GetInt("PhotosMission") < 50)
		{
			return 10;
		}
		if (PlayerPrefs.GetInt("PhotosMission") < 100)
		{
			return 15;
		}
		if (PlayerPrefs.GetInt("PhotosMission") < 200)
		{
			return 20;
		}
		if (PlayerPrefs.GetInt("PhotosMission") < 300)
		{
			return 25;
		}
		if (PlayerPrefs.GetInt("PhotosMission") < 400)
		{
			return 30;
		}
		if (PlayerPrefs.GetInt("PhotosMission") < 500)
		{
			return 35;
		}
		return 40;
	}

	private void CheckUnlocks(int level)
	{
		if (level == 3)
		{
			this.unlock1Text.text = LocalisationSystem.GetLocalisedValue("Experience_Unlocked") + LocalisationSystem.GetLocalisedValue("Experience_Medium");
			this.unlock2Text.text = LocalisationSystem.GetLocalisedValue("Experience_Unlocked") + LocalisationSystem.GetLocalisedValue("Equipment_StrongFlashlight");
			return;
		}
		if (level == 4)
		{
			this.unlock1Text.text = LocalisationSystem.GetLocalisedValue("Experience_Unlocked") + LocalisationSystem.GetLocalisedValue("Equipment_Thermometer");
			return;
		}
		if (level == 5)
		{
			this.unlock1Text.text = LocalisationSystem.GetLocalisedValue("Experience_Unlocked") + LocalisationSystem.GetLocalisedValue("Experience_Large");
			this.unlock2Text.text = LocalisationSystem.GetLocalisedValue("Experience_Unlocked") + LocalisationSystem.GetLocalisedValue("Equipment_SanityPills");
			return;
		}
		if (level == 6)
		{
			this.unlock1Text.text = LocalisationSystem.GetLocalisedValue("Experience_Unlocked") + LocalisationSystem.GetLocalisedValue("Equipment_MotionSensor");
			this.unlock2Text.text = LocalisationSystem.GetLocalisedValue("Experience_Unlocked") + LocalisationSystem.GetLocalisedValue("Equipment_IRLightSensor");
			return;
		}
		if (level == 7)
		{
			this.unlock1Text.text = LocalisationSystem.GetLocalisedValue("Experience_Unlocked") + LocalisationSystem.GetLocalisedValue("Equipment_SoundSensor");
			this.unlock2Text.text = LocalisationSystem.GetLocalisedValue("Experience_Unlocked") + LocalisationSystem.GetLocalisedValue("Equipment_ParabolicMicrophone");
			return;
		}
		if (level == 8)
		{
			this.unlock1Text.text = LocalisationSystem.GetLocalisedValue("Experience_Unlocked") + LocalisationSystem.GetLocalisedValue("Equipment_HeadMountedCamera");
			return;
		}
		if (level == 10)
		{
			this.unlock1Text.text = LocalisationSystem.GetLocalisedValue("Experience_Unlocked") + LocalisationSystem.GetLocalisedValue("Experience_Intermediate");
			return;
		}
		if (level == 15)
		{
			this.unlock1Text.text = LocalisationSystem.GetLocalisedValue("Experience_Unlocked") + LocalisationSystem.GetLocalisedValue("Experience_Professional");
		}
	}

	public void ResumeButton()
	{
		if (PhotonNetwork.InRoom)
		{
			MainManager.instance.serverManager.EnableMasks(true);
			base.gameObject.SetActive(false);
			this.serverSelector.SetSelection();
			return;
		}
		this.mainObject.SetActive(true);
		base.gameObject.SetActive(false);
	}

	[Header("Main")]
	[SerializeField]
	private GameObject mainObject;

	[SerializeField]
	private PlayerStatsManager playerStatsManager;

	[SerializeField]
	private GamepadUISelector serverSelector;

	[Header("Money Reward")]
	[SerializeField]
	private Text mainMissionRewardText;

	[SerializeField]
	private Text sideMission1RewardText;

	[SerializeField]
	private Text sideMission2RewardText;

	[SerializeField]
	private Text hiddenMissionRewardText;

	[SerializeField]
	private Text totalPhotosRewardText;

	[SerializeField]
	private Text totalDNARewardText;

	[SerializeField]
	private Text totalMissionRewardText;

	[SerializeField]
	private Text ghostTypeText;

	[SerializeField]
	private StoreManager storeManager;

	[SerializeField]
	private Text insuranceAmountText;

	[Header("Experience Reward")]
	[SerializeField]
	private Text levelUpText;

	[SerializeField]
	private Text experienceGainedText;

	[SerializeField]
	private Slider expSlider;

	[SerializeField]
	private Text expSliderValueText;

	[SerializeField]
	private Text currentLevelText;

	[SerializeField]
	private Text nextLevelText;

	[SerializeField]
	private Text unlock1Text;

	[SerializeField]
	private Text unlock2Text;

	[SerializeField]
	private Text unlock3Text;

	[SerializeField]
	private GameObject mainExperience;

	[SerializeField]
	private GameObject deadExperience;

	private Contract.LevelDifficulty levelDifficulty;
}


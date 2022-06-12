using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsManager : MonoBehaviour
{
	private void OnEnable()
	{
		this.UpdateLevel();
		this.UpdateExperience();
		this.UpdateMoney();
	}

	public void UpdateLevel()
	{
		this.levelText.text = LocalisationSystem.GetLocalisedValue("Experience_Level") + ": " + Mathf.FloorToInt((float)(PlayerPrefs.GetInt("myTotalExp") / 100));
	}

	public void UpdateExperience()
	{
		int num = 100 - (100 - ((Mathf.FloorToInt((float)(PlayerPrefs.GetInt("myTotalExp") / 100)) + 1) * 100 - PlayerPrefs.GetInt("myTotalExp")));
		this.experienceText.text = string.Concat(new object[]
		{
			LocalisationSystem.GetLocalisedValue("Menu_Experience"),
			": ",
			num,
			"XP"
		});
	}

	public void UpdateMoney()
	{
		this.moneyText.text = LocalisationSystem.GetLocalisedValue("Menu_Money") + ": $" + PlayerPrefs.GetInt("PlayerMoney").ToString();
	}

	[SerializeField]
	private Text levelText;

	[SerializeField]
	private Text experienceText;

	[SerializeField]
	private Text moneyText;
}


using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000144 RID: 324
public class PlayerStatsManager : MonoBehaviour
{
	// Token: 0x06000923 RID: 2339 RVA: 0x000382F9 File Offset: 0x000364F9
	private void OnEnable()
	{
		this.UpdateLevel();
		this.UpdateExperience();
		this.UpdateMoney();
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x0003830D File Offset: 0x0003650D
	public void UpdateLevel()
	{
		this.levelText.text = LocalisationSystem.GetLocalisedValue("Experience_Level") + ": " + Mathf.FloorToInt((float)(PlayerPrefs.GetInt("myTotalExp") / 100));
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x00038348 File Offset: 0x00036548
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

	// Token: 0x06000926 RID: 2342 RVA: 0x000383BC File Offset: 0x000365BC
	public void UpdateMoney()
	{
		this.moneyText.text = LocalisationSystem.GetLocalisedValue("Menu_Money") + ": $" + PlayerPrefs.GetInt("PlayerMoney").ToString();
	}

	// Token: 0x04000940 RID: 2368
	[SerializeField]
	private Text levelText;

	// Token: 0x04000941 RID: 2369
	[SerializeField]
	private Text experienceText;

	// Token: 0x04000942 RID: 2370
	[SerializeField]
	private Text moneyText;
}

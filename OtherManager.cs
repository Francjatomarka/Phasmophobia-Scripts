using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000141 RID: 321
public class OtherManager : MonoBehaviour
{
	// Token: 0x06000901 RID: 2305 RVA: 0x0003779A File Offset: 0x0003599A
	private void Start()
	{
		this.LoadValues();
	}

	// Token: 0x06000902 RID: 2306 RVA: 0x000377A4 File Offset: 0x000359A4
	private void LoadValues()
	{
		this.languageValue = PlayerPrefs.GetInt("languageValue");
		this.degreesValue = PlayerPrefs.GetInt("degreesValue");
		LocalisationSystem.ChangeLanguage(this.languageValue, this.languageValue);
		for (int i = 0; i < this.localiserText.Length; i++)
		{
			this.localiserText[i].LoadText();
		}
		this.UpdateUIValues();
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x00037804 File Offset: 0x00035A04
	public void SetValues()
	{
		PlayerPrefs.SetInt("languageValue", this.languageValue);
		PlayerPrefs.SetInt("degreesValue", this.degreesValue);
		LocalisationSystem.ChangeLanguage(this.languageValue, this.languageValue);
		for (int i = 0; i < this.localiserText.Length; i++)
		{
			this.localiserText[i].LoadText();
		}
		this.UpdateUIValues();
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x00037862 File Offset: 0x00035A62
	private void UpdateUIValues()
	{
		this.languageValueText.text = this.GetLanguageText();
		this.languageValueText2.text = this.GetLanguageText();
		this.degreesValueText.text = this.GetDegreesText();
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x00037897 File Offset: 0x00035A97
	public void ApplyButton()
	{
		this.SetValues();
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x0003789F File Offset: 0x00035A9F
	public void LanguageChangeValue(int value)
	{
		this.languageValue += value;
		if (this.languageValue < 0)
		{
			this.languageValue = 0;
		}
		else if (this.languageValue > 24)
		{
			this.languageValue = 24;
		}
		this.SetValues();
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x000378D9 File Offset: 0x00035AD9
	public void DegreesChangeValue(int value)
	{
		this.degreesValue += value;
		if (this.degreesValue < 0)
		{
			this.degreesValue = 0;
		}
		else if (this.degreesValue > 1)
		{
			this.degreesValue = 1;
		}
		this.SetValues();
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x00037914 File Offset: 0x00035B14
	private string GetLanguageText()
	{
		switch (this.languageValue)
		{
		case 0:
			return "English";
		case 1:
			return "Brazilian Portuguese";
		case 2:
			return "Spanish";
		case 3:
			return "Portuguese";
		case 4:
			return "German";
		case 5:
			return "French";
		case 6:
			return "Italian";
		case 7:
			return "Czech";
		case 8:
			return "Polish";
		case 9:
			return "Russian";
		case 10:
			return "Japanese";
		case 11:
			return "Korean";
		case 12:
			return "Turkish";
		case 13:
			return "Simplified Chinese";
		case 14:
			return "Traditional Chinese";
		case 15:
			return "Dutch";
		case 16:
			return "Greek";
		case 17:
			return "Norwegian";
		case 18:
			return "Romanian";
		case 19:
			return "Swedish";
		case 20:
			return "Ukrainian";
		case 21:
			return "Bulgarian";
		case 22:
			return "Danish";
		case 23:
			return "Finnish";
		case 24:
			return "Hungarian";
		default:
			return "";
		}
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x00037A32 File Offset: 0x00035C32
	private string GetDegreesText()
	{
		if (this.degreesValue == 0)
		{
			return LocalisationSystem.GetLocalisedValue("Other_Celsius");
		}
		return LocalisationSystem.GetLocalisedValue("Other_Farenheit");
	}

	// Token: 0x04000922 RID: 2338
	private int languageValue;

	// Token: 0x04000923 RID: 2339
	private int degreesValue;

	// Token: 0x04000924 RID: 2340
	[SerializeField]
	private Text languageValueText;

	// Token: 0x04000925 RID: 2341
	[SerializeField]
	private Text languageValueText2;

	// Token: 0x04000926 RID: 2342
	[SerializeField]
	private Text degreesValueText;

	// Token: 0x04000927 RID: 2343
	[SerializeField]
	private TextLocaliserUI[] localiserText;
}

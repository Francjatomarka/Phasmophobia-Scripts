using System;
using UnityEngine;
using UnityEngine.UI;

public class OtherManager : MonoBehaviour
{
	private void Start()
	{
		this.LoadValues();
	}

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

	private void UpdateUIValues()
	{
		this.languageValueText.text = this.GetLanguageText();
		this.languageValueText2.text = this.GetLanguageText();
		this.degreesValueText.text = this.GetDegreesText();
	}

	public void ApplyButton()
	{
		this.SetValues();
	}

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

	private string GetDegreesText()
	{
		if (this.degreesValue == 0)
		{
			return LocalisationSystem.GetLocalisedValue("Other_Celsius");
		}
		return LocalisationSystem.GetLocalisedValue("Other_Farenheit");
	}

	private int languageValue;

	private int degreesValue;

	[SerializeField]
	private Text languageValueText;

	[SerializeField]
	private Text languageValueText2;

	[SerializeField]
	private Text degreesValueText;

	[SerializeField]
	private TextLocaliserUI[] localiserText;
}


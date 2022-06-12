using System;
using RedScarf.EasyCSV;
using UnityEngine;

public class LocalisationSystem : MonoBehaviour
{
	public static void Init()
	{
		LocalisationSystem.csvFile = Resources.Load<TextAsset>("localisation");
		CsvHelper.Init(',');
		LocalisationSystem.table = CsvHelper.Create(LocalisationSystem.csvFile.name, LocalisationSystem.csvFile.text, true, true);
		LocalisationSystem.isInit = true;
	}

	public static string GetLocalisedValue(string key)
	{
		if (!LocalisationSystem.isInit)
		{
			LocalisationSystem.Init();
		}
		string result = "";
		switch (LocalisationSystem.language)
		{
			case LocalisationSystem.Language.English:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 1);
				break;
			case LocalisationSystem.Language.BrazilianPortuguese:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 2);
				break;
			case LocalisationSystem.Language.Spanish:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 3);
				break;
			case LocalisationSystem.Language.Portuguese:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 4);
				break;
			case LocalisationSystem.Language.German:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 5);
				break;
			case LocalisationSystem.Language.French:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 6);
				break;
			case LocalisationSystem.Language.Italian:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 7);
				break;
			case LocalisationSystem.Language.Czech:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 8);
				break;
			case LocalisationSystem.Language.Polish:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 9);
				break;
			case LocalisationSystem.Language.Russian:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 10);
				break;
			case LocalisationSystem.Language.Japanese:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 11);
				break;
			case LocalisationSystem.Language.Korean:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 12);
				break;
			case LocalisationSystem.Language.Turkish:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 13);
				break;
			case LocalisationSystem.Language.SimplifiedChinese:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 14);
				break;
			case LocalisationSystem.Language.TraditionalChinese:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 15);
				break;
			case LocalisationSystem.Language.Dutch:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 16);
				break;
			case LocalisationSystem.Language.Greek:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 17);
				break;
			case LocalisationSystem.Language.Norwegian:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 18);
				break;
			case LocalisationSystem.Language.Romanian:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 19);
				break;
			case LocalisationSystem.Language.Swedish:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 20);
				break;
			case LocalisationSystem.Language.Ukrainian:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 21);
				break;
			case LocalisationSystem.Language.Bulgarian:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 22);
				break;
			case LocalisationSystem.Language.Danish:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 23);
				break;
			case LocalisationSystem.Language.Finnish:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 24);
				break;
			case LocalisationSystem.Language.Hungarian:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 25);
				break;
		}
		return result;
		/*switch (LocalisationSystem.language)
		{
			case LocalisationSystem.Language.English:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 3);
				break;
		}*/
		return result;
	}

	public static string GetLocalisedVoiceValue(string key)
	{
		if (!LocalisationSystem.isInit)
		{
			LocalisationSystem.Init();
		}
		string result = "";
		switch (LocalisationSystem.voiceLanguage)
		{
			case LocalisationSystem.VoiceLanguage.English:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 1);
				break;
			case LocalisationSystem.VoiceLanguage.BrazilianPortuguese:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 2);
				break;
			case LocalisationSystem.VoiceLanguage.Spanish:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 3);
				break;
			case LocalisationSystem.VoiceLanguage.Portuguese:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 4);
				break;
			case LocalisationSystem.VoiceLanguage.German:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 5);
				break;
			case LocalisationSystem.VoiceLanguage.French:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 6);
				break;
			case LocalisationSystem.VoiceLanguage.Italian:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 7);
				break;
			case LocalisationSystem.VoiceLanguage.Czech:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 8);
				break;
			case LocalisationSystem.VoiceLanguage.Polish:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 9);
				break;
			case LocalisationSystem.VoiceLanguage.Russian:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 10);
				break;
			case LocalisationSystem.VoiceLanguage.Japanese:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 11);
				break;
			case LocalisationSystem.VoiceLanguage.Korean:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 12);
				break;
			case LocalisationSystem.VoiceLanguage.Turkish:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 13);
				break;
			case LocalisationSystem.VoiceLanguage.SimplifiedChinese:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 14);
				break;
			case LocalisationSystem.VoiceLanguage.TraditionalChinese:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 15);
				break;
			case LocalisationSystem.VoiceLanguage.Dutch:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 16);
				break;
			case LocalisationSystem.VoiceLanguage.Greek:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 17);
				break;
			case LocalisationSystem.VoiceLanguage.Norwegian:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 18);
				break;
			case LocalisationSystem.VoiceLanguage.Romanian:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 19);
				break;
			case LocalisationSystem.VoiceLanguage.Swedish:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 20);
				break;
			case LocalisationSystem.VoiceLanguage.Ukrainian:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 21);
				break;
			case LocalisationSystem.VoiceLanguage.Bulgarian:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 22);
				break;
			case LocalisationSystem.VoiceLanguage.Danish:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 23);
				break;
			case LocalisationSystem.VoiceLanguage.Finnish:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 24);
				break;
			case LocalisationSystem.VoiceLanguage.Hungarian:
				result = LocalisationSystem.table.Read(LocalisationSystem.table.FindValue(key).row, 25);
				break;
		}
		return result;
	}

	public static void ChangeLanguage(int id, int id2)
	{
		LocalisationSystem.language = (LocalisationSystem.Language)id;
		LocalisationSystem.voiceLanguage = (LocalisationSystem.VoiceLanguage)id2;
	}

	public static LocalisationSystem.Language language = (LocalisationSystem.Language)PlayerPrefs.GetInt("languageValue");

	public static LocalisationSystem.VoiceLanguage voiceLanguage = (LocalisationSystem.VoiceLanguage)PlayerPrefs.GetInt("voiceLanguageValue");

	public static bool isInit;

	private static TextAsset csvFile;

	private static CsvTable table;

	public enum Language
	{
		English,
		BrazilianPortuguese,
		Spanish,
		Portuguese,
		German,
		French,
		Italian,
		Czech,
		Polish,
		Russian,
		Japanese,
		Korean,
		Turkish,
		SimplifiedChinese,
		TraditionalChinese,
		Dutch,
		Greek,
		Norwegian,
		Romanian,
		Swedish,
		Ukrainian,
		Bulgarian,
		Danish,
		Finnish,
		Hungarian
	}

	public enum VoiceLanguage
	{
		English,
		BrazilianPortuguese,
		Spanish,
		Portuguese,
		German,
		French,
		Italian,
		Czech,
		Polish,
		Russian,
		Japanese,
		Korean,
		Turkish,
		SimplifiedChinese,
		TraditionalChinese,
		Dutch,
		Greek,
		Norwegian,
		Romanian,
		Swedish,
		Ukrainian,
		Bulgarian,
		Danish,
		Finnish,
		Hungarian
	}
}


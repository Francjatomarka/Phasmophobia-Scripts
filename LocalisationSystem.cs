using System;
using RedScarf.EasyCSV;
using UnityEngine;

// Token: 0x02000171 RID: 369
public class LocalisationSystem : MonoBehaviour
{
	// Token: 0x060009DB RID: 2523 RVA: 0x0003CB59 File Offset: 0x0003AD59
	public static void Init()
	{
		LocalisationSystem.csvFile = Resources.Load<TextAsset>("localisation");
		CsvHelper.Init(',');
		LocalisationSystem.table = CsvHelper.Create(LocalisationSystem.csvFile.name, LocalisationSystem.csvFile.text, true, true);
		LocalisationSystem.isInit = true;
	}

	// Token: 0x060009DC RID: 2524 RVA: 0x0003CB98 File Offset: 0x0003AD98
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

	// Token: 0x060009DD RID: 2525 RVA: 0x0003CF68 File Offset: 0x0003B168
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

	// Token: 0x060009DE RID: 2526 RVA: 0x0003D336 File Offset: 0x0003B536
	public static void ChangeLanguage(int id, int id2)
	{
		LocalisationSystem.language = (LocalisationSystem.Language)id;
		LocalisationSystem.voiceLanguage = (LocalisationSystem.VoiceLanguage)id2;
	}

	// Token: 0x04000A08 RID: 2568
	public static LocalisationSystem.Language language = (LocalisationSystem.Language)PlayerPrefs.GetInt("languageValue");

	// Token: 0x04000A09 RID: 2569
	public static LocalisationSystem.VoiceLanguage voiceLanguage = (LocalisationSystem.VoiceLanguage)PlayerPrefs.GetInt("voiceLanguageValue");

	// Token: 0x04000A0A RID: 2570
	public static bool isInit;

	// Token: 0x04000A0B RID: 2571
	private static TextAsset csvFile;

	// Token: 0x04000A0C RID: 2572
	private static CsvTable table;

	// Token: 0x0200053D RID: 1341
	public enum Language
	{
		// Token: 0x04002517 RID: 9495
		English,
		// Token: 0x04002518 RID: 9496
		BrazilianPortuguese,
		// Token: 0x04002519 RID: 9497
		Spanish,
		// Token: 0x0400251A RID: 9498
		Portuguese,
		// Token: 0x0400251B RID: 9499
		German,
		// Token: 0x0400251C RID: 9500
		French,
		// Token: 0x0400251D RID: 9501
		Italian,
		// Token: 0x0400251E RID: 9502
		Czech,
		// Token: 0x0400251F RID: 9503
		Polish,
		// Token: 0x04002520 RID: 9504
		Russian,
		// Token: 0x04002521 RID: 9505
		Japanese,
		// Token: 0x04002522 RID: 9506
		Korean,
		// Token: 0x04002523 RID: 9507
		Turkish,
		// Token: 0x04002524 RID: 9508
		SimplifiedChinese,
		// Token: 0x04002525 RID: 9509
		TraditionalChinese,
		// Token: 0x04002526 RID: 9510
		Dutch,
		// Token: 0x04002527 RID: 9511
		Greek,
		// Token: 0x04002528 RID: 9512
		Norwegian,
		// Token: 0x04002529 RID: 9513
		Romanian,
		// Token: 0x0400252A RID: 9514
		Swedish,
		// Token: 0x0400252B RID: 9515
		Ukrainian,
		// Token: 0x0400252C RID: 9516
		Bulgarian,
		// Token: 0x0400252D RID: 9517
		Danish,
		// Token: 0x0400252E RID: 9518
		Finnish,
		// Token: 0x0400252F RID: 9519
		Hungarian
	}

	// Token: 0x0200053E RID: 1342
	public enum VoiceLanguage
	{
		// Token: 0x04002531 RID: 9521
		English,
		// Token: 0x04002532 RID: 9522
		BrazilianPortuguese,
		// Token: 0x04002533 RID: 9523
		Spanish,
		// Token: 0x04002534 RID: 9524
		Portuguese,
		// Token: 0x04002535 RID: 9525
		German,
		// Token: 0x04002536 RID: 9526
		French,
		// Token: 0x04002537 RID: 9527
		Italian,
		// Token: 0x04002538 RID: 9528
		Czech,
		// Token: 0x04002539 RID: 9529
		Polish,
		// Token: 0x0400253A RID: 9530
		Russian,
		// Token: 0x0400253B RID: 9531
		Japanese,
		// Token: 0x0400253C RID: 9532
		Korean,
		// Token: 0x0400253D RID: 9533
		Turkish,
		// Token: 0x0400253E RID: 9534
		SimplifiedChinese,
		// Token: 0x0400253F RID: 9535
		TraditionalChinese,
		// Token: 0x04002540 RID: 9536
		Dutch,
		// Token: 0x04002541 RID: 9537
		Greek,
		// Token: 0x04002542 RID: 9538
		Norwegian,
		// Token: 0x04002543 RID: 9539
		Romanian,
		// Token: 0x04002544 RID: 9540
		Swedish,
		// Token: 0x04002545 RID: 9541
		Ukrainian,
		// Token: 0x04002546 RID: 9542
		Bulgarian,
		// Token: 0x04002547 RID: 9543
		Danish,
		// Token: 0x04002548 RID: 9544
		Finnish,
		// Token: 0x04002549 RID: 9545
		Hungarian
	}
}

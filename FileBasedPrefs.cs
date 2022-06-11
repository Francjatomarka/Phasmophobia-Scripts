using System;
using System.IO;
using UnityEngine;

// Token: 0x02000002 RID: 2
public static class FileBasedPrefs
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public static void SetString(string key, string value = "")
	{
		FileBasedPrefs.AddDataToSaveFile(key, value);
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00002059 File Offset: 0x00000259
	public static string GetString(string key, string defaultValue = "")
	{
		return (string)FileBasedPrefs.GetDataFromSaveFile(key, defaultValue);
	}

	// Token: 0x06000003 RID: 3 RVA: 0x00002067 File Offset: 0x00000267
	public static void SetInt(string key, int value = 0)
	{
		FileBasedPrefs.AddDataToSaveFile(key, value);
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002075 File Offset: 0x00000275
	public static int GetInt(string key, int defaultValue = 0)
	{
		return (int)FileBasedPrefs.GetDataFromSaveFile(key, defaultValue);
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00002088 File Offset: 0x00000288
	public static void SetFloat(string key, float value = 0f)
	{
		FileBasedPrefs.AddDataToSaveFile(key, value);
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002096 File Offset: 0x00000296
	public static float GetFloat(string key, float defaultValue = 0f)
	{
		return (float)FileBasedPrefs.GetDataFromSaveFile(key, defaultValue);
	}

	// Token: 0x06000007 RID: 7 RVA: 0x000020A9 File Offset: 0x000002A9
	public static void SetBool(string key, bool value = false)
	{
		FileBasedPrefs.AddDataToSaveFile(key, value);
	}

	// Token: 0x06000008 RID: 8 RVA: 0x000020B7 File Offset: 0x000002B7
	public static bool GetBool(string key, bool defaultValue = false)
	{
		return (bool)FileBasedPrefs.GetDataFromSaveFile(key, defaultValue);
	}

	// Token: 0x06000009 RID: 9 RVA: 0x000020CA File Offset: 0x000002CA
	public static bool HasKey(string key)
	{
		return FileBasedPrefs.GetSaveFile().HasKey(key);
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000020D7 File Offset: 0x000002D7
	public static bool HasKeyForString(string key)
	{
		return FileBasedPrefs.GetSaveFile().HasKeyFromObject(key, string.Empty);
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000020E9 File Offset: 0x000002E9
	public static bool HasKeyForInt(string key)
	{
		return FileBasedPrefs.GetSaveFile().HasKeyFromObject(key, 0);
	}

	// Token: 0x0600000C RID: 12 RVA: 0x000020FC File Offset: 0x000002FC
	public static bool HasKeyForFloat(string key)
	{
		return FileBasedPrefs.GetSaveFile().HasKeyFromObject(key, 0f);
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00002113 File Offset: 0x00000313
	public static bool HasKeyForBool(string key)
	{
		return FileBasedPrefs.GetSaveFile().HasKeyFromObject(key, false);
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002126 File Offset: 0x00000326
	public static void DeleteKey(string key)
	{
		FileBasedPrefs.GetSaveFile().DeleteKey(key);
		FileBasedPrefs.SaveSaveFile(false);
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00002139 File Offset: 0x00000339
	public static void DeleteString(string key)
	{
		FileBasedPrefs.GetSaveFile().DeleteString(key);
		FileBasedPrefs.SaveSaveFile(false);
	}

	// Token: 0x06000010 RID: 16 RVA: 0x0000214C File Offset: 0x0000034C
	public static void DeleteInt(string key)
	{
		FileBasedPrefs.GetSaveFile().DeleteInt(key);
		FileBasedPrefs.SaveSaveFile(false);
	}

	// Token: 0x06000011 RID: 17 RVA: 0x0000215F File Offset: 0x0000035F
	public static void DeleteFloat(string key)
	{
		FileBasedPrefs.GetSaveFile().DeleteFloat(key);
		FileBasedPrefs.SaveSaveFile(false);
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00002172 File Offset: 0x00000372
	public static void DeleteBool(string key)
	{
		FileBasedPrefs.GetSaveFile().DeleteBool(key);
		FileBasedPrefs.SaveSaveFile(false);
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002185 File Offset: 0x00000385
	public static void DeleteAll()
	{
		FileBasedPrefs.WriteToSaveFile(JsonUtility.ToJson(new FileBasedPrefsSaveFileModel()));
		FileBasedPrefs._latestData = new FileBasedPrefsSaveFileModel();
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000021A0 File Offset: 0x000003A0
	public static void OverwriteLocalSaveFile(string data)
	{
		FileBasedPrefs.WriteToSaveFile(data);
		FileBasedPrefs._latestData = null;
	}

	// Token: 0x06000015 RID: 21 RVA: 0x000021B0 File Offset: 0x000003B0
	private static FileBasedPrefsSaveFileModel GetSaveFile()
	{
		FileBasedPrefs.CheckSaveFileExists();
		if (FileBasedPrefs._latestData == null)
		{
			string text = File.ReadAllText(FileBasedPrefs.GetSaveFilePath());
			text = FileBasedPrefs.DataScrambler(text);
			Debug.Log("Trying: " + text);
			try
			{
				FileBasedPrefs._latestData = JsonUtility.FromJson<FileBasedPrefsSaveFileModel>(text);
			}
			catch (ArgumentException ex)
			{
				Debug.LogException(new Exception("SAVE FILE IN WRONG FORMAT, CREATING NEW SAVE FILE : " + ex.Message));
				FileBasedPrefs.DeleteAll();
			}
		}
		return FileBasedPrefs._latestData;
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002220 File Offset: 0x00000420
	public static string GetSaveFilePath()
	{
		return Path.Combine(Application.persistentDataPath, "saveData.txt");
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002231 File Offset: 0x00000431
	public static string GetSaveFileAsJson()
	{
		FileBasedPrefs.CheckSaveFileExists();
		return File.ReadAllText(FileBasedPrefs.GetSaveFilePath());
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002242 File Offset: 0x00000442
	private static object GetDataFromSaveFile(string key, object defaultValue)
	{
		return FileBasedPrefs.GetSaveFile().GetValueFromKey(key, defaultValue);
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002250 File Offset: 0x00000450
	private static void AddDataToSaveFile(string key, object value)
	{
		FileBasedPrefs.GetSaveFile().UpdateOrAddData(key, value);
		FileBasedPrefs.SaveSaveFile(false);
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002264 File Offset: 0x00000464
	public static void ManualySave()
	{
		FileBasedPrefs.SaveSaveFile(true);
	}

	// Token: 0x0600001B RID: 27 RVA: 0x0000226C File Offset: 0x0000046C
	private static void SaveSaveFile(bool manualSave = false)
	{
		FileBasedPrefs.WriteToSaveFile(JsonUtility.ToJson(FileBasedPrefs.GetSaveFile()));
	}

	// Token: 0x0600001C RID: 28 RVA: 0x0000227D File Offset: 0x0000047D
	private static void WriteToSaveFile(string data)
	{
		StreamWriter streamWriter = new StreamWriter(FileBasedPrefs.GetSaveFilePath());
		data = FileBasedPrefs.DataScrambler(data);
		streamWriter.Write(data);
		streamWriter.Close();
	}

	// Token: 0x0600001D RID: 29 RVA: 0x0000229D File Offset: 0x0000049D
	private static void CheckSaveFileExists()
	{
		if (!FileBasedPrefs.DoesSaveFileExist())
		{
			FileBasedPrefs.CreateNewSaveFile();
		}
	}

	// Token: 0x0600001E RID: 30 RVA: 0x000022AB File Offset: 0x000004AB
	private static bool DoesSaveFileExist()
	{
		return File.Exists(FileBasedPrefs.GetSaveFilePath());
	}

	// Token: 0x0600001F RID: 31 RVA: 0x000022B7 File Offset: 0x000004B7
	private static void CreateNewSaveFile()
	{
		FileBasedPrefs.WriteToSaveFile(JsonUtility.ToJson(new FileBasedPrefsSaveFileModel()));
	}

	// Token: 0x06000020 RID: 32 RVA: 0x000022C8 File Offset: 0x000004C8
	private static string DataScrambler(string data)
	{
		string text = "";
		for (int i = 0; i < data.Length; i++)
		{
			text += (data[i] ^ "CHANGE ME TO YOUR OWN RANDOM STRING"[i % "CHANGE ME TO YOUR OWN RANDOM STRING".Length]).ToString();
		}
		return text;
	}

	// Token: 0x04000001 RID: 1
	private const string SaveFileName = "saveData.txt";

	// Token: 0x04000002 RID: 2
	private const bool ScrambleSaveData = true;

	// Token: 0x04000003 RID: 3
	private const string EncryptionCodeword = "CHANGE ME TO YOUR OWN RANDOM STRING";

	// Token: 0x04000004 RID: 4
	private const bool AutoSaveData = true;

	// Token: 0x04000005 RID: 5
	private static FileBasedPrefsSaveFileModel _latestData;

	// Token: 0x04000006 RID: 6
	private const string String_Empty = "";
}

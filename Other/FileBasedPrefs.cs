using System;
using System.IO;
using UnityEngine;

public static class FileBasedPrefs
{
	public static void SetString(string key, string value = "")
	{
		FileBasedPrefs.AddDataToSaveFile(key, value);
	}

	public static string GetString(string key, string defaultValue = "")
	{
		return (string)FileBasedPrefs.GetDataFromSaveFile(key, defaultValue);
	}

	public static void SetInt(string key, int value = 0)
	{
		FileBasedPrefs.AddDataToSaveFile(key, value);
	}

	public static int GetInt(string key, int defaultValue = 0)
	{
		return (int)FileBasedPrefs.GetDataFromSaveFile(key, defaultValue);
	}

	public static void SetFloat(string key, float value = 0f)
	{
		FileBasedPrefs.AddDataToSaveFile(key, value);
	}

	public static float GetFloat(string key, float defaultValue = 0f)
	{
		return (float)FileBasedPrefs.GetDataFromSaveFile(key, defaultValue);
	}

	public static void SetBool(string key, bool value = false)
	{
		FileBasedPrefs.AddDataToSaveFile(key, value);
	}

	public static bool GetBool(string key, bool defaultValue = false)
	{
		return (bool)FileBasedPrefs.GetDataFromSaveFile(key, defaultValue);
	}

	public static bool HasKey(string key)
	{
		return FileBasedPrefs.GetSaveFile().HasKey(key);
	}

	public static bool HasKeyForString(string key)
	{
		return FileBasedPrefs.GetSaveFile().HasKeyFromObject(key, string.Empty);
	}

	public static bool HasKeyForInt(string key)
	{
		return FileBasedPrefs.GetSaveFile().HasKeyFromObject(key, 0);
	}

	public static bool HasKeyForFloat(string key)
	{
		return FileBasedPrefs.GetSaveFile().HasKeyFromObject(key, 0f);
	}

	public static bool HasKeyForBool(string key)
	{
		return FileBasedPrefs.GetSaveFile().HasKeyFromObject(key, false);
	}

	public static void DeleteKey(string key)
	{
		FileBasedPrefs.GetSaveFile().DeleteKey(key);
		FileBasedPrefs.SaveSaveFile(false);
	}

	public static void DeleteString(string key)
	{
		FileBasedPrefs.GetSaveFile().DeleteString(key);
		FileBasedPrefs.SaveSaveFile(false);
	}

	public static void DeleteInt(string key)
	{
		FileBasedPrefs.GetSaveFile().DeleteInt(key);
		FileBasedPrefs.SaveSaveFile(false);
	}

	public static void DeleteFloat(string key)
	{
		FileBasedPrefs.GetSaveFile().DeleteFloat(key);
		FileBasedPrefs.SaveSaveFile(false);
	}

	public static void DeleteBool(string key)
	{
		FileBasedPrefs.GetSaveFile().DeleteBool(key);
		FileBasedPrefs.SaveSaveFile(false);
	}

	public static void DeleteAll()
	{
		FileBasedPrefs.WriteToSaveFile(JsonUtility.ToJson(new FileBasedPrefsSaveFileModel()));
		FileBasedPrefs._latestData = new FileBasedPrefsSaveFileModel();
	}

	public static void OverwriteLocalSaveFile(string data)
	{
		FileBasedPrefs.WriteToSaveFile(data);
		FileBasedPrefs._latestData = null;
	}

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

	public static string GetSaveFilePath()
	{
		return Path.Combine(Application.persistentDataPath, "saveData.txt");
	}

	public static string GetSaveFileAsJson()
	{
		FileBasedPrefs.CheckSaveFileExists();
		return File.ReadAllText(FileBasedPrefs.GetSaveFilePath());
	}

	private static object GetDataFromSaveFile(string key, object defaultValue)
	{
		return FileBasedPrefs.GetSaveFile().GetValueFromKey(key, defaultValue);
	}

	private static void AddDataToSaveFile(string key, object value)
	{
		FileBasedPrefs.GetSaveFile().UpdateOrAddData(key, value);
		FileBasedPrefs.SaveSaveFile(false);
	}

	public static void ManualySave()
	{
		FileBasedPrefs.SaveSaveFile(true);
	}

	private static void SaveSaveFile(bool manualSave = false)
	{
		FileBasedPrefs.WriteToSaveFile(JsonUtility.ToJson(FileBasedPrefs.GetSaveFile()));
	}

	private static void WriteToSaveFile(string data)
	{
		StreamWriter streamWriter = new StreamWriter(FileBasedPrefs.GetSaveFilePath());
		data = FileBasedPrefs.DataScrambler(data);
		streamWriter.Write(data);
		streamWriter.Close();
	}

	private static void CheckSaveFileExists()
	{
		if (!FileBasedPrefs.DoesSaveFileExist())
		{
			FileBasedPrefs.CreateNewSaveFile();
		}
	}

	private static bool DoesSaveFileExist()
	{
		return File.Exists(FileBasedPrefs.GetSaveFilePath());
	}

	private static void CreateNewSaveFile()
	{
		FileBasedPrefs.WriteToSaveFile(JsonUtility.ToJson(new FileBasedPrefsSaveFileModel()));
	}

	private static string DataScrambler(string data)
	{
		string text = "";
		for (int i = 0; i < data.Length; i++)
		{
			text += (data[i] ^ "CHANGE ME TO YOUR OWN RANDOM STRING"[i % "CHANGE ME TO YOUR OWN RANDOM STRING".Length]).ToString();
		}
		return text;
	}

	private const string SaveFileName = "saveData.txt";

	private const bool ScrambleSaveData = true;

	private const string EncryptionCodeword = "CHANGE ME TO YOUR OWN RANDOM STRING";

	private const bool AutoSaveData = true;

	private static FileBasedPrefsSaveFileModel _latestData;

	private const string String_Empty = "";
}


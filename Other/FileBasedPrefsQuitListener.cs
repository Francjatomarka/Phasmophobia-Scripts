using System;
using UnityEngine;

public class FileBasedPrefsQuitListener : MonoBehaviour
{
	private void Awake()
	{
		if (FileBasedPrefsQuitListener.Instance != null)
		{
			Destroy(base.gameObject);
			return;
		}
		FileBasedPrefsQuitListener.Instance = this;
		DontDestroyOnLoad(base.gameObject);
	}

	private void OnApplicationQuit()
	{
		FileBasedPrefs.ManualySave();
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		FileBasedPrefs.ManualySave();
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		FileBasedPrefs.ManualySave();
	}

	public static FileBasedPrefsQuitListener Instance;
}


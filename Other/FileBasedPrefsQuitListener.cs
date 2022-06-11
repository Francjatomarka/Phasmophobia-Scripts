using System;
using UnityEngine;

// Token: 0x02000003 RID: 3
public class FileBasedPrefsQuitListener : MonoBehaviour
{
	// Token: 0x06000021 RID: 33 RVA: 0x0000231B File Offset: 0x0000051B
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

	// Token: 0x06000022 RID: 34 RVA: 0x00002347 File Offset: 0x00000547
	private void OnApplicationQuit()
	{
		FileBasedPrefs.ManualySave();
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00002347 File Offset: 0x00000547
	private void OnApplicationFocus(bool hasFocus)
	{
		FileBasedPrefs.ManualySave();
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00002347 File Offset: 0x00000547
	private void OnApplicationPause(bool pauseStatus)
	{
		FileBasedPrefs.ManualySave();
	}

	// Token: 0x04000007 RID: 7
	public static FileBasedPrefsQuitListener Instance;
}

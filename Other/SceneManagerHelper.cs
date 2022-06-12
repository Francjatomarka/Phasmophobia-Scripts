using System;
using UnityEngine.SceneManagement;

public class SceneManagerHelper
{
	// (get) Token: 0x06000275 RID: 629 RVA: 0x000113E4 File Offset: 0x0000F5E4
	public static string ActiveSceneName
	{
		get
		{
			return SceneManager.GetActiveScene().name;
		}
	}

	// (get) Token: 0x06000276 RID: 630 RVA: 0x00011400 File Offset: 0x0000F600
	public static int ActiveSceneBuildIndex
	{
		get
		{
			return SceneManager.GetActiveScene().buildIndex;
		}
	}
}


using System;
using UnityEngine.SceneManagement;

// Token: 0x02000065 RID: 101
public class SceneManagerHelper
{
	// Token: 0x1700005D RID: 93
	// (get) Token: 0x06000275 RID: 629 RVA: 0x000113E4 File Offset: 0x0000F5E4
	public static string ActiveSceneName
	{
		get
		{
			return SceneManager.GetActiveScene().name;
		}
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x06000276 RID: 630 RVA: 0x00011400 File Offset: 0x0000F600
	public static int ActiveSceneBuildIndex
	{
		get
		{
			return SceneManager.GetActiveScene().buildIndex;
		}
	}
}

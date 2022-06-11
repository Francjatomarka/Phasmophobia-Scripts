using System;
using UnityEngine;

// Token: 0x020000E0 RID: 224
public class SupportLogger : MonoBehaviour
{
	// Token: 0x06000628 RID: 1576 RVA: 0x00022719 File Offset: 0x00020919
	public void Start()
	{
		if (GameObject.Find("PunSupportLogger") == null)
		{
			GameObject gameObject = new GameObject("PunSupportLogger");
			DontDestroyOnLoad(gameObject);
			gameObject.AddComponent<SupportLogging>().LogTrafficStats = this.LogTrafficStats;
		}
	}

	// Token: 0x04000618 RID: 1560
	public bool LogTrafficStats = true;
}

using System;
using UnityEngine;

// Token: 0x02000042 RID: 66
public static class GameObjectExtensions
{
	// Token: 0x06000158 RID: 344 RVA: 0x0000A1D1 File Offset: 0x000083D1
	public static bool GetActive(this GameObject target)
	{
		return target.activeInHierarchy;
	}
}

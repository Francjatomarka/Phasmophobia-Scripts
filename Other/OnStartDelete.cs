using System;
using UnityEngine;

// Token: 0x0200008F RID: 143
public class OnStartDelete : MonoBehaviour
{
	// Token: 0x0600045A RID: 1114 RVA: 0x00018C70 File Offset: 0x00016E70
	private void Start()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}

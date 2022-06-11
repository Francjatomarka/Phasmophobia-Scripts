using System;
using UnityEngine;

// Token: 0x0200002C RID: 44
public class Unparent : MonoBehaviour
{
	// Token: 0x06000115 RID: 277 RVA: 0x00008DDE File Offset: 0x00006FDE
	private void Start()
	{
		base.gameObject.transform.parent = null;
	}
}

using System;
using UnityEngine;

// Token: 0x02000167 RID: 359
public class PCLook : MonoBehaviour
{
	// Token: 0x06000A25 RID: 2597 RVA: 0x0003DF46 File Offset: 0x0003C146
	private void LateUpdate()
	{
		this.spineBoneTransform.rotation = this.cam.transform.rotation;
	}

	// Token: 0x04000A5D RID: 2653
	[SerializeField]
	private Transform spineBoneTransform;

	// Token: 0x04000A5E RID: 2654
	[SerializeField]
	private Camera cam;
}

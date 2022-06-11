using System;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class HxSimpleRotate : MonoBehaviour
{
	// Token: 0x06000032 RID: 50 RVA: 0x00002AD2 File Offset: 0x00000CD2
	private void Update()
	{
		base.transform.Rotate(this.RotateSpeed * Time.deltaTime, Space.Self);
	}

	// Token: 0x0400000C RID: 12
	public Vector3 RotateSpeed;
}

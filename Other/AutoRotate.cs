using System;
using UnityEngine;

// Token: 0x0200001D RID: 29
public class AutoRotate : MonoBehaviour
{
	// Token: 0x060000D1 RID: 209 RVA: 0x00006621 File Offset: 0x00004821
	private void Start()
	{
		this.tr = base.GetComponent<Transform>();
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x0000662F File Offset: 0x0000482F
	private void Update()
	{
		this.tr.Rotate(this.rotation * Time.deltaTime);
	}

	// Token: 0x040000B4 RID: 180
	private Transform tr;

	// Token: 0x040000B5 RID: 181
	public Vector3 rotation;
}

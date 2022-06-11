using System;
using UnityEngine;

// Token: 0x02000007 RID: 7
[RequireComponent(typeof(HxVolumetricLight))]
public class HxDummyLight : MonoBehaviour
{
	// Token: 0x0600003D RID: 61 RVA: 0x00002E1A File Offset: 0x0000101A
	public void Update()
	{
	}

	// Token: 0x0400001E RID: 30
	public LightType type = LightType.Point;

	// Token: 0x0400001F RID: 31
	public float range = 10f;

	// Token: 0x04000020 RID: 32
	[Range(0f, 179f)]
	public float spotAngle = 40f;

	// Token: 0x04000021 RID: 33
	public Color color = Color.white;

	// Token: 0x04000022 RID: 34
	[Range(0f, 8f)]
	public float intensity = 1f;

	// Token: 0x04000023 RID: 35
	public Texture cookie;
}

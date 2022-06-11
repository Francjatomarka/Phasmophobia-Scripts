using System;
using UnityEngine;

// Token: 0x0200001E RID: 30
public class ColorPingPong : MonoBehaviour
{
	// Token: 0x060000D4 RID: 212 RVA: 0x0000664C File Offset: 0x0000484C
	private void Update()
	{
		this.mat.SetColor("_TintColor", Color.Lerp(this.colorA, this.colorB, Mathf.PingPong(Time.time / 30f, 1f)));
	}

	// Token: 0x040000B6 RID: 182
	public Material mat;

	// Token: 0x040000B7 RID: 183
	public Color colorA;

	// Token: 0x040000B8 RID: 184
	public Color colorB;
}

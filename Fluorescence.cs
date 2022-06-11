using System;
using UnityEngine;

// Token: 0x02000032 RID: 50
[RequireComponent(typeof(Renderer))]
public class Fluorescence : MonoBehaviour
{
	// Token: 0x0600012D RID: 301 RVA: 0x00009649 File Offset: 0x00007849
	private void Start()
	{
		this.mat = base.GetComponent<Renderer>().material;
	}

	// Token: 0x0600012E RID: 302 RVA: 0x0000965C File Offset: 0x0000785C
	private void Update()
	{
		this.mat.SetInt("_ColorCount", this.FluorescentColors.Length);
		this.mat.SetColorArray("_ColorArray", this.FluorescentColors);
		this.mat.SetColorArray("_ColorReplaceArray", this.ReplacementColors);
		this.mat.SetFloatArray("_PrecisionArray", this.CheckPrecisions);
	}

	// Token: 0x0400017C RID: 380
	public Color[] FluorescentColors;

	// Token: 0x0400017D RID: 381
	public Color[] ReplacementColors;

	// Token: 0x0400017E RID: 382
	public float[] CheckPrecisions;

	// Token: 0x0400017F RID: 383
	private Material mat;
}

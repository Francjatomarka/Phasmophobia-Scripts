using System;
using UnityEngine;

// Token: 0x02000015 RID: 21
public class HE_H_distortedLight : MonoBehaviour
{
	// Token: 0x060000A1 RID: 161 RVA: 0x00004F73 File Offset: 0x00003173
	private void Start()
	{
		this.myLight = base.gameObject.GetComponent<Light>();
		this.baseColor = this.myLight.color;
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x00004F98 File Offset: 0x00003198
	private void Update()
	{
		this.blinkIterator += 1f * Time.deltaTime;
		if (this.blinkIterator >= this.blinkFrequency)
		{
			this.blinkIterator = UnityEngine.Random.Range(0f, this.blinkFrequency) * 0.5f;
			if (this.myLight.color != this.distortColor)
			{
				this.myLight.color = this.distortColor;
				return;
			}
			this.myLight.color = this.baseColor;
		}
	}

	// Token: 0x04000071 RID: 113
	public Color distortColor = Color.white;

	// Token: 0x04000072 RID: 114
	private Color baseColor = Color.white;

	// Token: 0x04000073 RID: 115
	public float blinkFrequency = 1f;

	// Token: 0x04000074 RID: 116
	private float blinkIterator;

	// Token: 0x04000075 RID: 117
	private Light myLight;
}

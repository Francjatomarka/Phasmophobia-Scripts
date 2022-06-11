using System;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class WallDripper : MonoBehaviour
{
	// Token: 0x06000019 RID: 25 RVA: 0x000025B7 File Offset: 0x000007B7
	private void Start()
	{
		this.rend = base.GetComponent<Renderer>();
		this.dripSpeed = UnityEngine.Random.Range(this.dripSpeed - 0.01f, this.dripSpeed + 0.01f);
	}

	// Token: 0x0600001A RID: 26 RVA: 0x000025E8 File Offset: 0x000007E8
	private void Update()
	{
		if (this.speedArc < 90f)
		{
			this.speedArc += this.dripSpeed;
		}
		if (!this.makeBloodDry)
		{
			this.rend.material.SetFloat("_YOffset", Mathf.Lerp(this.yOffsetStartValue, this.yOffsetEndValue, Mathf.Sin(this.speedArc * 0.017453292f)));
		}
		if (this.speedArc >= 90f)
		{
			this.makeBloodDry = true;
		}
		if (this.makeBloodDry)
		{
			if (this.bloodDryAmount < 1f)
			{
				this.bloodDryAmount += this.autoDrySpeed * Time.deltaTime;
				this.rend.material.SetFloat("_BloodDrying", this.bloodDryAmount);
			}
			if (this.bloodDryAmount > 1f)
			{
				this.bloodDryAmount = 1f;
			}
			if (this.bloodDryAmount < 0f)
			{
				this.bloodDryAmount = 0f;
			}
			if (this.bloodDryAmount > 0.2f)
			{
				this.rend.material.SetFloat("_Fade", this.fadeAmount);
				if (this.fadeAmount < 1f)
				{
					this.fadeAmount += 0.1f * Time.deltaTime;
					return;
				}
				Destroy(base.transform.parent.gameObject);
			}
		}
	}

	// Token: 0x0400001D RID: 29
	public Renderer rend;

	// Token: 0x0400001E RID: 30
	public float bloodDryAmount;

	// Token: 0x0400001F RID: 31
	public float autoDrySpeed = 0.1f;

	// Token: 0x04000020 RID: 32
	public float dripSpeed = 1f;

	// Token: 0x04000021 RID: 33
	public float yOffsetStartValue = -1f;

	// Token: 0x04000022 RID: 34
	public float yOffsetEndValue = -0.05f;

	// Token: 0x04000023 RID: 35
	private bool makeBloodDry;

	// Token: 0x04000024 RID: 36
	private float speedArc;

	// Token: 0x04000025 RID: 37
	private float fadeAmount;
}

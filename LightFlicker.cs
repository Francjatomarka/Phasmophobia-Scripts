using System;
using UnityEngine;

// Token: 0x02000033 RID: 51
[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
	// Token: 0x0600012E RID: 302 RVA: 0x00009577 File Offset: 0x00007777
	private void Awake()
	{
		this.myLight = base.GetComponent<Light>();
	}

	// Token: 0x0600012F RID: 303 RVA: 0x00009585 File Offset: 0x00007785
	private void Start()
	{
		if (this.RandomInterval)
		{
			this.Interval = UnityEngine.Random.Range(0f, this.MaxInterval);
			return;
		}
		this.Interval = this.MaxInterval;
	}

	// Token: 0x06000130 RID: 304 RVA: 0x000095B4 File Offset: 0x000077B4
	private void Update()
	{
		if (this.timer < this.Interval)
		{
			this.timer += Time.deltaTime;
			return;
		}
		if (this.StayOnAfter > 0)
		{
			if (this.counter >= this.StayOnAfter)
			{
				this.myLight.enabled = true;
			}
			else
			{
				this.counter++;
				this.myLight.enabled = !this.myLight.enabled;
			}
		}
		else
		{
			this.myLight.enabled = !this.myLight.enabled;
		}
		this.timer = 0f;
		if (this.RandomInterval)
		{
			this.Interval = UnityEngine.Random.Range(0f, this.MaxInterval);
		}
	}

	// Token: 0x0400017D RID: 381
	public float MaxInterval;

	// Token: 0x0400017E RID: 382
	private float Interval;

	// Token: 0x0400017F RID: 383
	public bool RandomInterval;

	// Token: 0x04000180 RID: 384
	private float timer;

	// Token: 0x04000181 RID: 385
	private Light myLight;

	// Token: 0x04000182 RID: 386
	public int StayOnAfter;

	// Token: 0x04000183 RID: 387
	private int counter;
}

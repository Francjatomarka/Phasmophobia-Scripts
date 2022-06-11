using System;
using UnityEngine;

// Token: 0x02000025 RID: 37
public class AnalogClockScript : MonoBehaviour
{
	// Token: 0x060000FC RID: 252 RVA: 0x00008380 File Offset: 0x00006580
	public override string ToString()
	{
		string text = this.currentHour.ToString().PadLeft(2, '0');
		if (!this.use24HourFormat && this.currentHour > 12)
		{
			text = (this.currentHour - 12).ToString().PadLeft(2, '0');
		}
		string text2 = this.currentMinutes.ToString().PadLeft(2, '0');
		string text3 = this.currentSeconds.ToString().PadLeft(2, '0');
		return string.Format("{0}:{1}:{2}{3}", new object[]
		{
			text.Substring(text.Length - 2, 2),
			text2.Substring(text2.Length - 2, 2),
			text3.Substring(text3.Length - 2, 2),
			this.use24HourFormat ? "" : ((this.currentHour > 12) ? " PM" : " AM")
		});
	}

	// Token: 0x060000FD RID: 253 RVA: 0x00008464 File Offset: 0x00006664
	private void Start()
	{
		if (!this.secondHand)
		{
			throw new UnityException("The Second Hand GameObject cannot be null");
		}
		if (!this.minuteHand)
		{
			throw new UnityException("The Minute Hand GameObject cannot be null");
		}
		if (!this.hourHand)
		{
			throw new UnityException("The Hour Hand GameObject cannot be null");
		}
		DateTime now = DateTime.Now;
		if (this.currentHour == 0 && this.currentMinutes == 0 && this.currentSeconds == 0)
		{
			this.currentHour = now.Hour;
			this.currentMinutes = now.Minute;
			this.currentSeconds = now.Second;
		}
	}

	// Token: 0x060000FE RID: 254 RVA: 0x00008500 File Offset: 0x00006700
	private void Update()
	{
		this.currentTime = this.ToString();
		this.updateTimeElapsed += Time.deltaTime * this.currentTimeRate;
		if (this.updateTimeElapsed < 1f)
		{
			return;
		}
		this.updateTimeElapsed = 0f;
		this.currentSeconds++;
		this.secondHand.transform.localEulerAngles = new Vector3(0f, 0f, 6f * (float)this.currentSeconds);
		if ((float)this.currentSeconds >= 60f)
		{
			this.currentSeconds = 0;
			this.currentMinutes++;
		}
		this.minuteHand.transform.localEulerAngles = new Vector3(0f, 0f, 6f * (float)this.currentMinutes);
		if ((float)this.currentMinutes >= 60f)
		{
			this.currentMinutes = 0;
			this.currentHour++;
		}
		this.hourHand.transform.localEulerAngles = new Vector3(0f, 0f, 30f * (float)this.currentHour);
		if ((float)this.currentHour >= 24f)
		{
			this.currentHour = 0;
		}
	}

	// Token: 0x04000117 RID: 279
	private const float MAX_DEGREE_ANGLE = 360f;

	// Token: 0x04000118 RID: 280
	private const float SECONDS_PER_MINUTE = 60f;

	// Token: 0x04000119 RID: 281
	private const float MINUTES_PER_HOUR = 60f;

	// Token: 0x0400011A RID: 282
	private const float HOURS_PER_WHOLEDAY = 24f;

	// Token: 0x0400011B RID: 283
	private const float HOURS_PER_HALFDAY = 12f;

	// Token: 0x0400011C RID: 284
	private const float NORMAL_TIMERATE = 1f;

	// Token: 0x0400011D RID: 285
	private const float VECTOR_NOCHANGE = 0f;

	// Token: 0x0400011E RID: 286
	public GameObject secondHand;

	// Token: 0x0400011F RID: 287
	public GameObject minuteHand;

	// Token: 0x04000120 RID: 288
	public GameObject hourHand;

	// Token: 0x04000121 RID: 289
	[Range(0f, 23f)]
	public int currentHour;

	// Token: 0x04000122 RID: 290
	[Range(0f, 59f)]
	public int currentMinutes;

	// Token: 0x04000123 RID: 291
	[Range(0f, 59f)]
	public int currentSeconds;

	// Token: 0x04000124 RID: 292
	public string currentTime = "";

	// Token: 0x04000125 RID: 293
	[Range(0.1f, 4f)]
	public float currentTimeRate = 1f;

	// Token: 0x04000126 RID: 294
	public bool use24HourFormat;

	// Token: 0x04000127 RID: 295
	private float updateTimeElapsed;
}

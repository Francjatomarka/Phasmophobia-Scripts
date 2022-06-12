using System;
using UnityEngine;

public class AnalogClockScript : MonoBehaviour
{
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

	private const float MAX_DEGREE_ANGLE = 360f;

	private const float SECONDS_PER_MINUTE = 60f;

	private const float MINUTES_PER_HOUR = 60f;

	private const float HOURS_PER_WHOLEDAY = 24f;

	private const float HOURS_PER_HALFDAY = 12f;

	private const float NORMAL_TIMERATE = 1f;

	private const float VECTOR_NOCHANGE = 0f;

	public GameObject secondHand;

	public GameObject minuteHand;

	public GameObject hourHand;

	[Range(0f, 23f)]
	public int currentHour;

	[Range(0f, 59f)]
	public int currentMinutes;

	[Range(0f, 59f)]
	public int currentSeconds;

	public string currentTime = "";

	[Range(0.1f, 4f)]
	public float currentTimeRate = 1f;

	public bool use24HourFormat;

	private float updateTimeElapsed;
}


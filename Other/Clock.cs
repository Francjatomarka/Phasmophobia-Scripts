using System;
using UnityEngine;

public class Clock : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (this.analog)
		{
			TimeSpan timeOfDay = DateTime.Now.TimeOfDay;
			this.hours.localRotation = Quaternion.Euler(0f, 0f, (float)timeOfDay.TotalHours * 30f);
			this.minutes.localRotation = Quaternion.Euler(0f, 0f, (float)timeOfDay.TotalMinutes * 6f);
			this.seconds.localRotation = Quaternion.Euler(0f, 0f, (float)timeOfDay.TotalSeconds * 6f);
			return;
		}
		DateTime now = DateTime.Now;
		this.hours.localRotation = Quaternion.Euler(0f, 0f, (float)now.Hour * 30f);
		this.minutes.localRotation = Quaternion.Euler(0f, 0f, (float)now.Minute * 6f);
		this.seconds.localRotation = Quaternion.Euler(0f, 0f, (float)now.Second * 6f);
	}

	private const float hoursToDegrees = 30f;

	private const float minutesToDegrees = 6f;

	private const float secondsToDegrees = 6f;

	public Transform hours;

	public Transform minutes;

	public Transform seconds;

	public bool analog = true;
}


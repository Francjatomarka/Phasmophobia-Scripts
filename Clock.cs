using System;
using UnityEngine;

// Token: 0x02000022 RID: 34
public class Clock : MonoBehaviour
{
	// Token: 0x060000E0 RID: 224 RVA: 0x00003D4C File Offset: 0x00001F4C
	private void Start()
	{
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x000070A4 File Offset: 0x000052A4
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

	// Token: 0x040000D2 RID: 210
	private const float hoursToDegrees = 30f;

	// Token: 0x040000D3 RID: 211
	private const float minutesToDegrees = 6f;

	// Token: 0x040000D4 RID: 212
	private const float secondsToDegrees = 6f;

	// Token: 0x040000D5 RID: 213
	public Transform hours;

	// Token: 0x040000D6 RID: 214
	public Transform minutes;

	// Token: 0x040000D7 RID: 215
	public Transform seconds;

	// Token: 0x040000D8 RID: 216
	public bool analog = true;
}

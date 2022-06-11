using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001C6 RID: 454
public class SoundSensorData : MonoBehaviour
{
	// Token: 0x06000C86 RID: 3206 RVA: 0x00050139 File Offset: 0x0004E339
	private void Awake()
	{
		SoundSensorData.instance = this;
	}

	// Token: 0x06000C87 RID: 3207 RVA: 0x00050144 File Offset: 0x0004E344
	public void UpdateSensorValue(int id, float volume)
	{
		switch (id)
		{
		case 0:
			this.sensor1Bar.offsetMax = new Vector2(this.sensor1Bar.offsetMax.x, volume * 500f);
			return;
		case 1:
			this.sensor2Bar.offsetMax = new Vector2(this.sensor2Bar.offsetMax.x, volume * 500f);
			return;
		case 2:
			this.sensor3Bar.offsetMax = new Vector2(this.sensor3Bar.offsetMax.x, volume * 500f);
			return;
		case 3:
			this.sensor4Bar.offsetMax = new Vector2(this.sensor4Bar.offsetMax.x, volume * 500f);
			return;
		default:
			return;
		}
	}

	// Token: 0x06000C88 RID: 3208 RVA: 0x00050208 File Offset: 0x0004E408
	public void SetText(SoundSensor sensor)
	{
		if (!this.Sensors.Contains(sensor))
		{
			this.Sensors.Add(sensor);
			sensor.id = this.Sensors.Count - 1;
		}
		switch (sensor.id)
		{
		case 0:
			this.sensor_1_Text.text = this.Sensors[0].roomName;
			return;
		case 1:
			this.sensor_2_Text.text = this.Sensors[1].roomName;
			return;
		case 2:
			this.sensor_3_Text.text = this.Sensors[2].roomName;
			return;
		case 3:
			this.sensor_4_Text.text = this.Sensors[3].roomName;
			return;
		default:
			return;
		}
	}

	// Token: 0x06000C89 RID: 3209 RVA: 0x000502D4 File Offset: 0x0004E4D4
	public void RemoveText(SoundSensor sensor)
	{
		switch (sensor.id)
		{
		case 0:
			this.sensor_1_Text.text = "";
			return;
		case 1:
			this.sensor_2_Text.text = "";
			return;
		case 2:
			this.sensor_3_Text.text = "";
			return;
		case 3:
			this.sensor_4_Text.text = "";
			return;
		default:
			return;
		}
	}

	// Token: 0x04000D31 RID: 3377
	public static SoundSensorData instance;

	// Token: 0x04000D32 RID: 3378
	private List<SoundSensor> Sensors = new List<SoundSensor>();

	// Token: 0x04000D33 RID: 3379
	[SerializeField]
	private RectTransform sensor1Bar;

	// Token: 0x04000D34 RID: 3380
	[SerializeField]
	private RectTransform sensor2Bar;

	// Token: 0x04000D35 RID: 3381
	[SerializeField]
	private RectTransform sensor3Bar;

	// Token: 0x04000D36 RID: 3382
	[SerializeField]
	private RectTransform sensor4Bar;

	// Token: 0x04000D37 RID: 3383
	[SerializeField]
	private Text sensor_1_Text;

	// Token: 0x04000D38 RID: 3384
	[SerializeField]
	private Text sensor_2_Text;

	// Token: 0x04000D39 RID: 3385
	[SerializeField]
	private Text sensor_3_Text;

	// Token: 0x04000D3A RID: 3386
	[SerializeField]
	private Text sensor_4_Text;
}

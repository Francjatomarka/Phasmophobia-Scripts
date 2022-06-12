using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSensorData : MonoBehaviour
{
	private void Awake()
	{
		SoundSensorData.instance = this;
	}

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

	public static SoundSensorData instance;

	private List<SoundSensor> Sensors = new List<SoundSensor>();

	[SerializeField]
	private RectTransform sensor1Bar;

	[SerializeField]
	private RectTransform sensor2Bar;

	[SerializeField]
	private RectTransform sensor3Bar;

	[SerializeField]
	private RectTransform sensor4Bar;

	[SerializeField]
	private Text sensor_1_Text;

	[SerializeField]
	private Text sensor_2_Text;

	[SerializeField]
	private Text sensor_3_Text;

	[SerializeField]
	private Text sensor_4_Text;
}


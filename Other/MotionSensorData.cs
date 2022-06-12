using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotionSensorData : MonoBehaviour
{
	private void Awake()
	{
		MotionSensorData.instance = this;
		this.startColor = ((this.image1 == null) ? Color.yellow : this.image1.color);
	}

	private void Update()
	{
		if (this.detected1)
		{
			this.timer1 -= Time.deltaTime;
			if (this.timer1 < 0f)
			{
				this.timer1 = 0f;
				this.detected1 = false;
				if (this.image1)
				{
					this.image1.color = this.startColor;
				}
			}
		}
		if (this.detected2)
		{
			this.timer2 -= Time.deltaTime;
			if (this.timer2 < 0f)
			{
				this.timer2 = 0f;
				this.detected2 = false;
				if (this.image2)
				{
					this.image2.color = this.startColor;
				}
			}
		}
		if (this.detected3)
		{
			this.timer3 -= Time.deltaTime;
			if (this.timer3 < 0f)
			{
				this.timer3 = 0f;
				this.detected3 = false;
				if (this.image3)
				{
					this.image3.color = this.startColor;
				}
			}
		}
		if (this.detected4)
		{
			this.timer4 -= Time.deltaTime;
			if (this.timer4 < 0f)
			{
				this.timer4 = 0f;
				this.detected4 = false;
				if (this.image4)
				{
					this.image4.color = this.startColor;
				}
			}
		}
	}

	public void Detected(MotionSensor sensor)
	{
		this.source.Play();
		if (sensor.id == 1)
		{
			this.detected1 = true;
			this.timer1 = 2f;
			if (this.image1)
			{
				this.image1.color = Color.green;
				return;
			}
		}
		else if (sensor.id == 2)
		{
			this.detected2 = true;
			this.timer2 = 2f;
			if (this.image2)
			{
				this.image2.color = Color.green;
				return;
			}
		}
		else if (sensor.id == 3)
		{
			this.detected3 = true;
			this.timer3 = 2f;
			if (this.image3)
			{
				this.image3.color = Color.green;
				return;
			}
		}
		else if (sensor.id == 4)
		{
			this.detected4 = true;
			this.timer4 = 2f;
			if (this.image4)
			{
				this.image4.color = Color.green;
			}
		}
	}

	public void SetText(MotionSensor sensor)
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

	public void RemoveText(MotionSensor sensor)
	{
		switch (sensor.id)
		{
		case 0:
			this.sensor_1_Text.text = "Sensor 1";
			return;
		case 1:
			this.sensor_2_Text.text = "Sensor 2";
			return;
		case 2:
			this.sensor_3_Text.text = "Sensor 3";
			return;
		case 3:
			this.sensor_4_Text.text = "Sensor 4";
			return;
		default:
			return;
		}
	}

	public static MotionSensorData instance;

	[HideInInspector]
	public List<MotionSensor> Sensors = new List<MotionSensor>();

	private float timer1 = 6f;

	private float timer2 = 6f;

	private float timer3 = 6f;

	private float timer4 = 6f;

	private bool detected1;

	private bool detected2;

	private bool detected3;

	private bool detected4;

	public Image image1;

	public Image image2;

	public Image image3;

	public Image image4;

	private Color startColor;

	[SerializeField]
	private Text sensor_1_Text;

	[SerializeField]
	private Text sensor_2_Text;

	[SerializeField]
	private Text sensor_3_Text;

	[SerializeField]
	private Text sensor_4_Text;

	[SerializeField]
	private AudioSource source;
}


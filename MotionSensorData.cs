using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000180 RID: 384
public class MotionSensorData : MonoBehaviour
{
	// Token: 0x06000AF3 RID: 2803 RVA: 0x00045464 File Offset: 0x00043664
	private void Awake()
	{
		MotionSensorData.instance = this;
		this.startColor = ((this.image1 == null) ? Color.yellow : this.image1.color);
	}

	// Token: 0x06000AF4 RID: 2804 RVA: 0x00045494 File Offset: 0x00043694
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

	// Token: 0x06000AF5 RID: 2805 RVA: 0x00045600 File Offset: 0x00043800
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

	// Token: 0x06000AF6 RID: 2806 RVA: 0x00045704 File Offset: 0x00043904
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

	// Token: 0x06000AF7 RID: 2807 RVA: 0x000457D0 File Offset: 0x000439D0
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

	// Token: 0x04000B67 RID: 2919
	public static MotionSensorData instance;

	// Token: 0x04000B68 RID: 2920
	[HideInInspector]
	public List<MotionSensor> Sensors = new List<MotionSensor>();

	// Token: 0x04000B69 RID: 2921
	private float timer1 = 6f;

	// Token: 0x04000B6A RID: 2922
	private float timer2 = 6f;

	// Token: 0x04000B6B RID: 2923
	private float timer3 = 6f;

	// Token: 0x04000B6C RID: 2924
	private float timer4 = 6f;

	// Token: 0x04000B6D RID: 2925
	private bool detected1;

	// Token: 0x04000B6E RID: 2926
	private bool detected2;

	// Token: 0x04000B6F RID: 2927
	private bool detected3;

	// Token: 0x04000B70 RID: 2928
	private bool detected4;

	// Token: 0x04000B71 RID: 2929
	public Image image1;

	// Token: 0x04000B72 RID: 2930
	public Image image2;

	// Token: 0x04000B73 RID: 2931
	public Image image3;

	// Token: 0x04000B74 RID: 2932
	public Image image4;

	// Token: 0x04000B75 RID: 2933
	private Color startColor;

	// Token: 0x04000B76 RID: 2934
	[SerializeField]
	private Text sensor_1_Text;

	// Token: 0x04000B77 RID: 2935
	[SerializeField]
	private Text sensor_2_Text;

	// Token: 0x04000B78 RID: 2936
	[SerializeField]
	private Text sensor_3_Text;

	// Token: 0x04000B79 RID: 2937
	[SerializeField]
	private Text sensor_4_Text;

	// Token: 0x04000B7A RID: 2938
	[SerializeField]
	private AudioSource source;
}

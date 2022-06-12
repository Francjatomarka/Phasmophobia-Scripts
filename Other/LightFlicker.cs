using System;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
	private void Awake()
	{
		this.myLight = base.GetComponent<Light>();
	}

	private void Start()
	{
		if (this.RandomInterval)
		{
			this.Interval = UnityEngine.Random.Range(0f, this.MaxInterval);
			return;
		}
		this.Interval = this.MaxInterval;
	}

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

	public float MaxInterval;

	private float Interval;

	public bool RandomInterval;

	private float timer;

	private Light myLight;

	public int StayOnAfter;

	private int counter;
}


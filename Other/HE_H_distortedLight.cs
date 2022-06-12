using System;
using UnityEngine;

public class HE_H_distortedLight : MonoBehaviour
{
	private void Start()
	{
		this.myLight = base.gameObject.GetComponent<Light>();
		this.baseColor = this.myLight.color;
	}

	private void Update()
	{
		this.blinkIterator += 1f * Time.deltaTime;
		if (this.blinkIterator >= this.blinkFrequency)
		{
			this.blinkIterator = UnityEngine.Random.Range(0f, this.blinkFrequency) * 0.5f;
			if (this.myLight.color != this.distortColor)
			{
				this.myLight.color = this.distortColor;
				return;
			}
			this.myLight.color = this.baseColor;
		}
	}

	public Color distortColor = Color.white;

	private Color baseColor = Color.white;

	public float blinkFrequency = 1f;

	private float blinkIterator;

	private Light myLight;
}


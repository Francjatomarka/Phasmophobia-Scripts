using System;
using UnityEngine;

public class WallDripper : MonoBehaviour
{
	private void Start()
	{
		this.rend = base.GetComponent<Renderer>();
		this.dripSpeed = UnityEngine.Random.Range(this.dripSpeed - 0.01f, this.dripSpeed + 0.01f);
	}

	private void Update()
	{
		if (this.speedArc < 90f)
		{
			this.speedArc += this.dripSpeed;
		}
		if (!this.makeBloodDry)
		{
			this.rend.material.SetFloat("_YOffset", Mathf.Lerp(this.yOffsetStartValue, this.yOffsetEndValue, Mathf.Sin(this.speedArc * 0.017453292f)));
		}
		if (this.speedArc >= 90f)
		{
			this.makeBloodDry = true;
		}
		if (this.makeBloodDry)
		{
			if (this.bloodDryAmount < 1f)
			{
				this.bloodDryAmount += this.autoDrySpeed * Time.deltaTime;
				this.rend.material.SetFloat("_BloodDrying", this.bloodDryAmount);
			}
			if (this.bloodDryAmount > 1f)
			{
				this.bloodDryAmount = 1f;
			}
			if (this.bloodDryAmount < 0f)
			{
				this.bloodDryAmount = 0f;
			}
			if (this.bloodDryAmount > 0.2f)
			{
				this.rend.material.SetFloat("_Fade", this.fadeAmount);
				if (this.fadeAmount < 1f)
				{
					this.fadeAmount += 0.1f * Time.deltaTime;
					return;
				}
				Destroy(base.transform.parent.gameObject);
			}
		}
	}

	public Renderer rend;

	public float bloodDryAmount;

	public float autoDrySpeed = 0.1f;

	public float dripSpeed = 1f;

	public float yOffsetStartValue = -1f;

	public float yOffsetEndValue = -0.05f;

	private bool makeBloodDry;

	private float speedArc;

	private float fadeAmount;
}


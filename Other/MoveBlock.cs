using System;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{
	protected virtual void Start()
	{
		this.startY = base.transform.position.y;
		this.moveUpAmount = Mathf.Abs(this.moveYAmount);
		if (this.moveYAmount < 0f)
		{
			this.startY -= this.moveYAmount;
			this.goingUp = false;
		}
		this.stoppedUntilTime = Time.time + this.waitTime;
	}

	protected virtual void Update()
	{
		if (Time.time > this.stoppedUntilTime)
		{
			if (this.goingUp)
			{
				if (base.transform.position.y < this.startY + this.moveUpAmount)
				{
					Vector3 position = base.transform.position;
					position.y += Time.deltaTime * this.moveSpeed;
					base.transform.position = position;
				}
				else
				{
					this.goingUp = false;
					this.stoppedUntilTime = Time.time + this.waitTime;
				}
			}
			else if (base.transform.position.y > this.startY)
			{
				Vector3 position2 = base.transform.position;
				position2.y -= Time.deltaTime * this.moveSpeed;
				base.transform.position = position2;
			}
			else
			{
				this.goingUp = true;
				this.stoppedUntilTime = Time.time + this.waitTime;
			}
		}
		base.transform.Rotate(new Vector3(0f, this.rotateSpeed * Time.deltaTime, 0f));
	}

	public float moveYAmount = 20f;

	public float moveSpeed = 1f;

	public float waitTime = 5f;

	public float rotateSpeed = 10f;

	private float startY;

	private bool goingUp = true;

	private float stoppedUntilTime;

	private float moveUpAmount;
}


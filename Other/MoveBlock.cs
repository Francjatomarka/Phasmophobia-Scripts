using System;
using UnityEngine;

// Token: 0x020001AF RID: 431
public class MoveBlock : MonoBehaviour
{
	// Token: 0x06000C87 RID: 3207 RVA: 0x0004F554 File Offset: 0x0004D754
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

	// Token: 0x06000C88 RID: 3208 RVA: 0x0004F5C4 File Offset: 0x0004D7C4
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

	// Token: 0x04000CBB RID: 3259
	public float moveYAmount = 20f;

	// Token: 0x04000CBC RID: 3260
	public float moveSpeed = 1f;

	// Token: 0x04000CBD RID: 3261
	public float waitTime = 5f;

	// Token: 0x04000CBE RID: 3262
	public float rotateSpeed = 10f;

	// Token: 0x04000CBF RID: 3263
	private float startY;

	// Token: 0x04000CC0 RID: 3264
	private bool goingUp = true;

	// Token: 0x04000CC1 RID: 3265
	private float stoppedUntilTime;

	// Token: 0x04000CC2 RID: 3266
	private float moveUpAmount;
}

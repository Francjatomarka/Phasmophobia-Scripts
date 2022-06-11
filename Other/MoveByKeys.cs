using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200008A RID: 138
[RequireComponent(typeof(PhotonView))]
public class MoveByKeys : MonoBehaviourPunCallbacks
{
	// Token: 0x06000447 RID: 1095 RVA: 0x00018346 File Offset: 0x00016546
	public void Start()
	{
		this.isSprite = (base.GetComponent<SpriteRenderer>() != null);
		this.body2d = base.GetComponent<Rigidbody2D>();
		this.body = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06000448 RID: 1096 RVA: 0x00018374 File Offset: 0x00016574
	public void FixedUpdate()
	{
		if (!base.photonView.IsMine)
		{
			return;
		}
		if (Input.GetAxisRaw("Horizontal") < -0.1f || Input.GetAxisRaw("Horizontal") > 0.1f)
		{
			base.transform.position += Vector3.right * (this.Speed * Time.deltaTime) * Input.GetAxisRaw("Horizontal");
		}
		if (this.jumpingTime <= 0f)
		{
			if ((this.body != null || this.body2d != null) && Input.GetKey(KeyCode.Space))
			{
				this.jumpingTime = this.JumpTimeout;
				Vector2 vector = Vector2.up * this.JumpForce;
				if (this.body2d != null)
				{
					this.body2d.AddForce(vector);
				}
				else if (this.body != null)
				{
					this.body.AddForce(vector);
				}
			}
		}
		else
		{
			this.jumpingTime -= Time.deltaTime;
		}
		if (!this.isSprite && (Input.GetAxisRaw("Vertical") < -0.1f || Input.GetAxisRaw("Vertical") > 0.1f))
		{
			base.transform.position += Vector3.forward * (this.Speed * Time.deltaTime) * Input.GetAxisRaw("Vertical");
		}
	}

	// Token: 0x0400045C RID: 1116
	public float Speed = 10f;

	// Token: 0x0400045D RID: 1117
	public float JumpForce = 200f;

	// Token: 0x0400045E RID: 1118
	public float JumpTimeout = 0.5f;

	// Token: 0x0400045F RID: 1119
	private bool isSprite;

	// Token: 0x04000460 RID: 1120
	private float jumpingTime;

	// Token: 0x04000461 RID: 1121
	private Rigidbody body;

	// Token: 0x04000462 RID: 1122
	private Rigidbody2D body2d;
}

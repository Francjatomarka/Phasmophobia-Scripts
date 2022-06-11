using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x02000039 RID: 57
public class JumpAndRunMovement : MonoBehaviour
{
	// Token: 0x06000143 RID: 323 RVA: 0x00009C09 File Offset: 0x00007E09
	private void Awake()
	{
		this.m_Animator = base.GetComponent<Animator>();
		this.m_Body = base.GetComponent<Rigidbody2D>();
		this.m_PhotonView = base.GetComponent<PhotonView>();
	}

	// Token: 0x06000144 RID: 324 RVA: 0x00009C2F File Offset: 0x00007E2F
	private void Update()
	{
		this.UpdateIsGrounded();
		this.UpdateIsRunning();
		this.UpdateFacingDirection();
	}

	// Token: 0x06000145 RID: 325 RVA: 0x00009C43 File Offset: 0x00007E43
	private void FixedUpdate()
	{
		if (!this.m_PhotonView.IsMine)
		{
			return;
		}
		this.UpdateMovement();
		this.UpdateJumping();
	}

	// Token: 0x06000146 RID: 326 RVA: 0x00009C60 File Offset: 0x00007E60
	private void UpdateFacingDirection()
	{
		if (this.m_Body.velocity.x > 0.2f)
		{
			base.transform.localScale = new Vector3(1f, 1f, 1f);
			return;
		}
		if (this.m_Body.velocity.x < -0.2f)
		{
			base.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
	}

	// Token: 0x06000147 RID: 327 RVA: 0x00009CDC File Offset: 0x00007EDC
	private void UpdateJumping()
	{
		if (Input.GetButton("Jump") && this.m_IsGrounded)
		{
			this.m_Animator.SetTrigger("IsJumping");
			this.m_Body.AddForce(Vector2.up * this.JumpForce);
			this.m_PhotonView.RPC("DoJump", RpcTarget.Others, Array.Empty<object>());
		}
	}

	// Token: 0x06000148 RID: 328 RVA: 0x00009D3E File Offset: 0x00007F3E
	[PunRPC]
	private void DoJump()
	{
		this.m_Animator.SetTrigger("IsJumping");
	}

	// Token: 0x06000149 RID: 329 RVA: 0x00009D50 File Offset: 0x00007F50
	private void UpdateMovement()
	{
		Vector2 velocity = this.m_Body.velocity;
		if (Input.GetAxisRaw("Horizontal") > 0.5f)
		{
			velocity.x = this.Speed;
		}
		else if (Input.GetAxisRaw("Horizontal") < -0.5f)
		{
			velocity.x = -this.Speed;
		}
		else
		{
			velocity.x = 0f;
		}
		this.m_Body.velocity = velocity;
	}

	// Token: 0x0600014A RID: 330 RVA: 0x00009DC2 File Offset: 0x00007FC2
	private void UpdateIsRunning()
	{
		this.m_Animator.SetBool("IsRunning", Mathf.Abs(this.m_Body.velocity.x) > 0.1f);
	}

	// Token: 0x0600014B RID: 331 RVA: 0x00009DF0 File Offset: 0x00007FF0
	private void UpdateIsGrounded()
	{
		this.m_IsGrounded = (Physics2D.Raycast(new Vector2(base.transform.position.x, base.transform.position.y), -Vector2.up, 0.1f).collider != null);
		this.m_Animator.SetBool("IsGrounded", this.m_IsGrounded);
	}

	// Token: 0x04000190 RID: 400
	public float Speed;

	// Token: 0x04000191 RID: 401
	public float JumpForce;

	// Token: 0x04000192 RID: 402
	private Animator m_Animator;

	// Token: 0x04000193 RID: 403
	private Rigidbody2D m_Body;

	// Token: 0x04000194 RID: 404
	private PhotonView m_PhotonView;

	// Token: 0x04000195 RID: 405
	private bool m_IsGrounded;
}

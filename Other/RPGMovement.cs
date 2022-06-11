using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x02000061 RID: 97
[RequireComponent(typeof(CharacterController))]
public class RPGMovement : MonoBehaviour
{
	// Token: 0x0600020C RID: 524 RVA: 0x0000E66F File Offset: 0x0000C86F
	private void Start()
	{
		this.m_CharacterController = base.GetComponent<CharacterController>();
		this.m_Animator = base.GetComponent<Animator>();
		this.m_PhotonView = base.GetComponent<PhotonView>();
		this.m_TransformView = base.GetComponent<PhotonTransformView>();
	}

	// Token: 0x0600020D RID: 525 RVA: 0x0000E6A4 File Offset: 0x0000C8A4
	private void Update()
	{
		if (this.m_PhotonView.IsMine)
		{
			this.ResetSpeedValues();
			this.UpdateRotateMovement();
			this.UpdateForwardMovement();
			this.UpdateBackwardMovement();
			this.UpdateStrafeMovement();
			this.MoveCharacterController();
			this.ApplyGravityToCharacterController();
			this.ApplySynchronizedValues();
		}
		this.UpdateAnimation();
	}

	// Token: 0x0600020E RID: 526 RVA: 0x0000E6F4 File Offset: 0x0000C8F4
	private void UpdateAnimation()
	{
		Vector3 vector = base.transform.position - this.m_LastPosition;
		float num = Vector3.Dot(vector.normalized, base.transform.forward);
		float num2 = Vector3.Dot(vector.normalized, base.transform.right);
		if (Mathf.Abs(num) < 0.2f)
		{
			num = 0f;
		}
		if (num > 0.6f)
		{
			num = 1f;
			num2 = 0f;
		}
		if (num >= 0f && Mathf.Abs(num2) > 0.7f)
		{
			num = 1f;
		}
		this.m_AnimatorSpeed = Mathf.MoveTowards(this.m_AnimatorSpeed, num, Time.deltaTime * 5f);
		this.m_Animator.SetFloat("Speed", this.m_AnimatorSpeed);
		this.m_Animator.SetFloat("Direction", num2);
		this.m_LastPosition = base.transform.position;
	}

	// Token: 0x0600020F RID: 527 RVA: 0x0000E7DF File Offset: 0x0000C9DF
	private void ResetSpeedValues()
	{
		this.m_CurrentMovement = Vector3.zero;
		this.m_CurrentTurnSpeed = 0f;
	}

	// Token: 0x06000210 RID: 528 RVA: 0x0000E7F7 File Offset: 0x0000C9F7
	private void ApplySynchronizedValues()
	{
		
	}

	// Token: 0x06000211 RID: 529 RVA: 0x0000E810 File Offset: 0x0000CA10
	private void ApplyGravityToCharacterController()
	{
		this.m_CharacterController.Move(base.transform.up * Time.deltaTime * -9.81f);
	}

	// Token: 0x06000212 RID: 530 RVA: 0x0000E83D File Offset: 0x0000CA3D
	private void MoveCharacterController()
	{
		this.m_CharacterController.Move(this.m_CurrentMovement * Time.deltaTime);
	}

	// Token: 0x06000213 RID: 531 RVA: 0x0000E85B File Offset: 0x0000CA5B
	private void UpdateForwardMovement()
	{
		if (Input.GetKey(KeyCode.W) || Input.GetAxisRaw("Vertical") > 0.1f)
		{
			this.m_CurrentMovement = base.transform.forward * this.ForwardSpeed;
		}
	}

	// Token: 0x06000214 RID: 532 RVA: 0x0000E893 File Offset: 0x0000CA93
	private void UpdateBackwardMovement()
	{
		if (Input.GetKey(KeyCode.S) || Input.GetAxisRaw("Vertical") < -0.1f)
		{
			this.m_CurrentMovement = -base.transform.forward * this.BackwardSpeed;
		}
	}

	// Token: 0x06000215 RID: 533 RVA: 0x0000E8D0 File Offset: 0x0000CAD0
	private void UpdateStrafeMovement()
	{
		if (Input.GetKey(KeyCode.Q))
		{
			this.m_CurrentMovement = -base.transform.right * this.StrafeSpeed;
		}
		if (Input.GetKey(KeyCode.E))
		{
			this.m_CurrentMovement = base.transform.right * this.StrafeSpeed;
		}
	}

	// Token: 0x06000216 RID: 534 RVA: 0x0000E92C File Offset: 0x0000CB2C
	private void UpdateRotateMovement()
	{
		if (Input.GetKey(KeyCode.A) || Input.GetAxisRaw("Horizontal") < -0.1f)
		{
			this.m_CurrentTurnSpeed = -this.RotateSpeed;
			base.transform.Rotate(0f, -this.RotateSpeed * Time.deltaTime, 0f);
		}
		if (Input.GetKey(KeyCode.D) || Input.GetAxisRaw("Horizontal") > 0.1f)
		{
			this.m_CurrentTurnSpeed = this.RotateSpeed;
			base.transform.Rotate(0f, this.RotateSpeed * Time.deltaTime, 0f);
		}
	}

	// Token: 0x04000253 RID: 595
	public float ForwardSpeed;

	// Token: 0x04000254 RID: 596
	public float BackwardSpeed;

	// Token: 0x04000255 RID: 597
	public float StrafeSpeed;

	// Token: 0x04000256 RID: 598
	public float RotateSpeed;

	// Token: 0x04000257 RID: 599
	private CharacterController m_CharacterController;

	// Token: 0x04000258 RID: 600
	private Vector3 m_LastPosition;

	// Token: 0x04000259 RID: 601
	private Animator m_Animator;

	// Token: 0x0400025A RID: 602
	private PhotonView m_PhotonView;

	// Token: 0x0400025B RID: 603
	private PhotonTransformView m_TransformView;

	// Token: 0x0400025C RID: 604
	private float m_AnimatorSpeed;

	// Token: 0x0400025D RID: 605
	private Vector3 m_CurrentMovement;

	// Token: 0x0400025E RID: 606
	private float m_CurrentTurnSpeed;
}

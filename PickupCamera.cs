using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200005A RID: 90
public class PickupCamera : MonoBehaviourPunCallbacks
{
	// Token: 0x060001E1 RID: 481 RVA: 0x0000D018 File Offset: 0x0000B218
	private void OnEnable()
	{
		if (base.photonView != null && !base.photonView.IsMine)
		{
			base.enabled = false;
			return;
		}
		if (!this.cameraTransform && Camera.main)
		{
			this.cameraTransform = Camera.main.transform;
		}
		if (!this.cameraTransform)
		{
			Debug.Log("Please assign a camera to the ThirdPersonCamera script.");
			base.enabled = false;
		}
		this.m_CameraTransformCamera = this.cameraTransform.GetComponent<Camera>();
		this._target = base.transform;
		if (this._target)
		{
			this.controller = this._target.GetComponent<PickupController>();
		}
		if (this.controller)
		{
			CharacterController characterController = (CharacterController)this._target.GetComponent<Collider>();
			this.centerOffset = characterController.bounds.center - this._target.position;
			this.headOffset = this.centerOffset;
			this.headOffset.y = characterController.bounds.max.y - this._target.position.y;
		}
		else
		{
			Debug.Log("Please assign a target to the camera that has a ThirdPersonController script attached.");
		}
		this.Cut(this._target, this.centerOffset);
	}

	// Token: 0x060001E2 RID: 482 RVA: 0x0000D165 File Offset: 0x0000B365
	private void DebugDrawStuff()
	{
		Debug.DrawLine(this._target.position, this._target.position + this.headOffset);
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x0000D18D File Offset: 0x0000B38D
	private float AngleDistance(float a, float b)
	{
		a = Mathf.Repeat(a, 360f);
		b = Mathf.Repeat(b, 360f);
		return Mathf.Abs(b - a);
	}

	// Token: 0x060001E4 RID: 484 RVA: 0x0000D1B4 File Offset: 0x0000B3B4
	private void Apply(Transform dummyTarget, Vector3 dummyCenter)
	{
		if (!this.controller)
		{
			return;
		}
		Vector3 vector = this._target.position + this.centerOffset;
		Vector3 headPos = this._target.position + this.headOffset;
		float y = this._target.eulerAngles.y;
		float num = this.cameraTransform.eulerAngles.y;
		float num2 = y;
		if (Input.GetButton("Fire2"))
		{
			this.snap = true;
		}
		if (this.snap)
		{
			if (this.AngleDistance(num, y) < 3f)
			{
				this.snap = false;
			}
			num = Mathf.SmoothDampAngle(num, num2, ref this.angleVelocity, this.snapSmoothLag, this.snapMaxSpeed);
		}
		else
		{
			if (this.controller.GetLockCameraTimer() < this.lockCameraTimeout)
			{
				num2 = num;
			}
			if (this.AngleDistance(num, num2) > 160f && this.controller.IsMovingBackwards())
			{
				num2 += 180f;
			}
			num = Mathf.SmoothDampAngle(num, num2, ref this.angleVelocity, this.angularSmoothLag, this.angularMaxSpeed);
		}
		if (this.controller.IsJumping())
		{
			float num3 = vector.y + this.height;
			if (num3 < this.targetHeight || num3 - this.targetHeight > 5f)
			{
				this.targetHeight = vector.y + this.height;
			}
		}
		else
		{
			this.targetHeight = vector.y + this.height;
		}
		float num4 = this.cameraTransform.position.y;
		num4 = Mathf.SmoothDamp(num4, this.targetHeight, ref this.heightVelocity, this.heightSmoothLag);
		Quaternion rotation = Quaternion.Euler(0f, num, 0f);
		this.cameraTransform.position = vector;
		this.cameraTransform.position += rotation * Vector3.back * this.distance;
		this.cameraTransform.position = new Vector3(this.cameraTransform.position.x, num4, this.cameraTransform.position.z);
		this.SetUpRotation(vector, headPos);
	}

	// Token: 0x060001E5 RID: 485 RVA: 0x0000D3D8 File Offset: 0x0000B5D8
	private void LateUpdate()
	{
		this.Apply(base.transform, Vector3.zero);
	}

	// Token: 0x060001E6 RID: 486 RVA: 0x0000D3EC File Offset: 0x0000B5EC
	private void Cut(Transform dummyTarget, Vector3 dummyCenter)
	{
		float num = this.heightSmoothLag;
		float num2 = this.snapMaxSpeed;
		float num3 = this.snapSmoothLag;
		this.snapMaxSpeed = 10000f;
		this.snapSmoothLag = 0.001f;
		this.heightSmoothLag = 0.001f;
		this.snap = true;
		this.Apply(base.transform, Vector3.zero);
		this.heightSmoothLag = num;
		this.snapMaxSpeed = num2;
		this.snapSmoothLag = num3;
	}

	// Token: 0x060001E7 RID: 487 RVA: 0x0000D45C File Offset: 0x0000B65C
	private void SetUpRotation(Vector3 centerPos, Vector3 headPos)
	{
		Vector3 position = this.cameraTransform.position;
		Vector3 vector = centerPos - position;
		Quaternion lhs = Quaternion.LookRotation(new Vector3(vector.x, 0f, vector.z));
		Vector3 forward = Vector3.forward * this.distance + Vector3.down * this.height;
		this.cameraTransform.rotation = lhs * Quaternion.LookRotation(forward);
		Ray ray = this.m_CameraTransformCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1f));
		Ray ray2 = this.m_CameraTransformCamera.ViewportPointToRay(new Vector3(0.5f, this.clampHeadPositionScreenSpace, 1f));
		Vector3 point = ray.GetPoint(this.distance);
		Vector3 point2 = ray2.GetPoint(this.distance);
		float num = Vector3.Angle(ray.direction, ray2.direction);
		float num2 = num / (point.y - point2.y) * (point.y - centerPos.y);
		if (num2 < num)
		{
			return;
		}
		num2 -= num;
		this.cameraTransform.rotation *= Quaternion.Euler(-num2, 0f, 0f);
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x0000D5AC File Offset: 0x0000B7AC
	private Vector3 GetCenterOffset()
	{
		return this.centerOffset;
	}

	// Token: 0x040001FF RID: 511
	public Transform cameraTransform;

	// Token: 0x04000200 RID: 512
	private Transform _target;

	// Token: 0x04000201 RID: 513
	public float distance = 7f;

	// Token: 0x04000202 RID: 514
	public float height = 3f;

	// Token: 0x04000203 RID: 515
	public float angularSmoothLag = 0.3f;

	// Token: 0x04000204 RID: 516
	public float angularMaxSpeed = 15f;

	// Token: 0x04000205 RID: 517
	public float heightSmoothLag = 0.3f;

	// Token: 0x04000206 RID: 518
	public float snapSmoothLag = 0.2f;

	// Token: 0x04000207 RID: 519
	public float snapMaxSpeed = 720f;

	// Token: 0x04000208 RID: 520
	public float clampHeadPositionScreenSpace = 0.75f;

	// Token: 0x04000209 RID: 521
	public float lockCameraTimeout = 0.2f;

	// Token: 0x0400020A RID: 522
	private Vector3 headOffset = Vector3.zero;

	// Token: 0x0400020B RID: 523
	private Vector3 centerOffset = Vector3.zero;

	// Token: 0x0400020C RID: 524
	private float heightVelocity;

	// Token: 0x0400020D RID: 525
	private float angleVelocity;

	// Token: 0x0400020E RID: 526
	private bool snap;

	// Token: 0x0400020F RID: 527
	private PickupController controller;

	// Token: 0x04000210 RID: 528
	private float targetHeight = 100000f;

	// Token: 0x04000211 RID: 529
	private Camera m_CameraTransformCamera;
}

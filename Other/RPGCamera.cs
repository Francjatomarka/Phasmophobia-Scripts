using System;
using UnityEngine;

// Token: 0x02000060 RID: 96
public class RPGCamera : MonoBehaviour
{
	// Token: 0x06000205 RID: 517 RVA: 0x0000E484 File Offset: 0x0000C684
	private void Start()
	{
		this.m_CameraTransform = base.transform.GetChild(0);
		this.m_LocalForwardVector = this.m_CameraTransform.forward;
		this.m_Distance = -this.m_CameraTransform.localPosition.z / this.m_CameraTransform.forward.z;
		this.m_Distance = Mathf.Clamp(this.m_Distance, this.MinimumDistance, this.MaximumDistance);
		this.m_LookAtPoint = this.m_CameraTransform.localPosition + this.m_LocalForwardVector * this.m_Distance;
	}

	// Token: 0x06000206 RID: 518 RVA: 0x0000E520 File Offset: 0x0000C720
	private void LateUpdate()
	{
		this.UpdateDistance();
		this.UpdateZoom();
		this.UpdatePosition();
		this.UpdateRotation();
	}

	// Token: 0x06000207 RID: 519 RVA: 0x0000E53A File Offset: 0x0000C73A
	private void UpdateDistance()
	{
		this.m_Distance = Mathf.Clamp(this.m_Distance - Input.GetAxis("Mouse ScrollWheel") * this.ScrollModifier, this.MinimumDistance, this.MaximumDistance);
	}

	// Token: 0x06000208 RID: 520 RVA: 0x0000E56B File Offset: 0x0000C76B
	private void UpdateZoom()
	{
		this.m_CameraTransform.localPosition = this.m_LookAtPoint - this.m_LocalForwardVector * this.m_Distance;
	}

	// Token: 0x06000209 RID: 521 RVA: 0x0000E594 File Offset: 0x0000C794
	private void UpdatePosition()
	{
		if (this.Target == null)
		{
			return;
		}
		base.transform.position = this.Target.transform.position;
	}

	// Token: 0x0600020A RID: 522 RVA: 0x0000E5C0 File Offset: 0x0000C7C0
	private void UpdateRotation()
	{
		if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetButton("Fire1") || Input.GetButton("Fire2"))
		{
			base.transform.Rotate(0f, Input.GetAxis("Mouse X") * this.TurnModifier, 0f);
		}
		if ((Input.GetMouseButton(1) || Input.GetButton("Fire2")) && this.Target != null)
		{
			this.Target.rotation = Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y, 0f);
		}
	}

	// Token: 0x0400024A RID: 586
	public Transform Target;

	// Token: 0x0400024B RID: 587
	public float MaximumDistance;

	// Token: 0x0400024C RID: 588
	public float MinimumDistance;

	// Token: 0x0400024D RID: 589
	public float ScrollModifier;

	// Token: 0x0400024E RID: 590
	public float TurnModifier;

	// Token: 0x0400024F RID: 591
	private Transform m_CameraTransform;

	// Token: 0x04000250 RID: 592
	private Vector3 m_LookAtPoint;

	// Token: 0x04000251 RID: 593
	private Vector3 m_LocalForwardVector;

	// Token: 0x04000252 RID: 594
	private float m_Distance;
}

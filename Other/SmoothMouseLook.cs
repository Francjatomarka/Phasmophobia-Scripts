using System;
using UnityEngine;

public class SmoothMouseLook : MonoBehaviour
{
	private void Start()
	{
		this.targetDirection = base.transform.localRotation.eulerAngles;
	}

	private void Update()
	{
		if (this.lockCursor)
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
		Quaternion quaternion = Quaternion.Euler(this.targetDirection);
		Vector2 vector = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
		vector = Vector2.Scale(vector, new Vector2(this.sensitivity.x * this.smoothing.x, this.sensitivity.y * this.smoothing.y));
		this.smoothMouse.x = Mathf.Lerp(this.smoothMouse.x, vector.x, 1f / this.smoothing.x);
		this.smoothMouse.y = Mathf.Lerp(this.smoothMouse.y, vector.y, 1f / this.smoothing.y);
		this.mouseAbsolute += this.smoothMouse;
		if (this.clampInDegrees.x < 360f)
		{
			this.mouseAbsolute.x = Mathf.Clamp(this.mouseAbsolute.x, -this.clampInDegrees.x * 0.5f, this.clampInDegrees.x * 0.5f);
		}
		if (this.clampInDegrees.y < 360f)
		{
			this.mouseAbsolute.y = Mathf.Clamp(this.mouseAbsolute.y, -this.clampInDegrees.y * 0.5f, this.clampInDegrees.y * 0.5f);
		}
		base.transform.localRotation = Quaternion.AngleAxis(-this.mouseAbsolute.y, quaternion * Vector3.right) * quaternion;
		Quaternion rhs = Quaternion.AngleAxis(this.mouseAbsolute.x, base.transform.InverseTransformDirection(Vector3.up));
		base.transform.localRotation *= rhs;
	}

	public Vector2 clampInDegrees = new Vector2(360f, 180f);

	public bool lockCursor;

	public Vector2 sensitivity = new Vector2(2f, 2f);

	public Vector2 smoothing = new Vector2(3f, 3f);

	public Vector2 targetDirection;

	private Vector2 mouseAbsolute;

	private Vector2 smoothMouse;
}


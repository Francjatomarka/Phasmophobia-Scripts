using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x020001A6 RID: 422
public class FlyCam : MonoBehaviour
{
	// Token: 0x06000B8A RID: 2954 RVA: 0x00047540 File Offset: 0x00045740
	private void Awake()
	{
		this.myLight = base.GetComponentInChildren<Light>();
		this.lightCookie = this.myLight.cookie;
	}

	// Token: 0x06000B8B RID: 2955 RVA: 0x0004755F File Offset: 0x0004575F
	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	// Token: 0x06000B8C RID: 2956 RVA: 0x00047570 File Offset: 0x00045770
	private void Update()
	{
		if (Keyboard.current.gKey.wasPressedThisFrame)
		{
			this.isDim = !this.isDim;
			if (this.isDim)
			{
				this.myLight.range = 10f;
				this.myLight.cookie = this.lightCookie;
			}
			else
			{
				this.myLight.range = 20f;
				this.myLight.cookie = null;
			}
		}
		if (Keyboard.current.fKey.wasPressedThisFrame)
		{
			this.myLight.enabled = !this.myLight.enabled;
		}
		if (Keyboard.current.dKey.isPressed)
		{
			this.xSpeed = 1f;
		}
		else if (Keyboard.current.aKey.isPressed)
		{
			this.xSpeed = -1f;
		}
		else
		{
			this.xSpeed = 0f;
		}
		if (Keyboard.current.wKey.isPressed)
		{
			this.ySpeed = 1f;
		}
		else if (Keyboard.current.sKey.isPressed)
		{
			this.ySpeed = -1f;
		}
		else
		{
			this.ySpeed = 0f;
		}
		this.rotationX += Mouse.current.delta.ReadValue().x * 1f * Time.deltaTime;
		this.rotationY += Mouse.current.delta.ReadValue().y * 1f * Time.deltaTime;
		this.rotationY = Mathf.Clamp(this.rotationY, -90f, 90f);
		base.transform.localRotation = Quaternion.AngleAxis(this.rotationX, Vector3.up);
		base.transform.localRotation *= Quaternion.AngleAxis(this.rotationY, Vector3.left);
		if (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed)
		{
			base.transform.position += base.transform.forward * (this.normalMoveSpeed * this.fastMoveFactor) * this.ySpeed * Time.deltaTime;
			base.transform.position += base.transform.right * (this.normalMoveSpeed * this.fastMoveFactor) * this.xSpeed * Time.deltaTime;
		}
		else if (Keyboard.current.leftCtrlKey.isPressed || Keyboard.current.rightCtrlKey.isPressed)
		{
			base.transform.position += base.transform.forward * (this.normalMoveSpeed * this.slowMoveFactor) * this.ySpeed * Time.deltaTime;
			base.transform.position += base.transform.right * (this.normalMoveSpeed * this.slowMoveFactor) * this.xSpeed * Time.deltaTime;
		}
		else
		{
			base.transform.position += base.transform.forward * this.normalMoveSpeed * this.ySpeed * Time.deltaTime;
			base.transform.position += base.transform.right * this.normalMoveSpeed * this.xSpeed * Time.deltaTime;
		}
		if (Keyboard.current.qKey.isPressed)
		{
			base.transform.position += base.transform.up * this.climbSpeed * Time.deltaTime;
		}
		if (Keyboard.current.eKey.isPressed)
		{
			base.transform.position -= base.transform.up * this.climbSpeed * Time.deltaTime;
		}
	}

	// Token: 0x04000BD9 RID: 3033
	public float climbSpeed = 4f;

	// Token: 0x04000BDA RID: 3034
	public float normalMoveSpeed = 10f;

	// Token: 0x04000BDB RID: 3035
	public float slowMoveFactor = 0.25f;

	// Token: 0x04000BDC RID: 3036
	public float fastMoveFactor = 3f;

	// Token: 0x04000BDD RID: 3037
	private float rotationX;

	// Token: 0x04000BDE RID: 3038
	private float rotationY;

	// Token: 0x04000BDF RID: 3039
	private float horizontalLook;

	// Token: 0x04000BE0 RID: 3040
	private float verticalLook;

	// Token: 0x04000BE1 RID: 3041
	private float xSpeed;

	// Token: 0x04000BE2 RID: 3042
	private float ySpeed;

	// Token: 0x04000BE3 RID: 3043
	private Light myLight;

	// Token: 0x04000BE4 RID: 3044
	private bool isDim = true;

	// Token: 0x04000BE5 RID: 3045
	private Texture lightCookie;
}

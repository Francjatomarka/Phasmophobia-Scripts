using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlyCam : MonoBehaviour
{
	private void Awake()
	{
		this.myLight = base.GetComponentInChildren<Light>();
		this.lightCookie = this.myLight.cookie;
	}

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

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

	public float climbSpeed = 4f;

	public float normalMoveSpeed = 10f;

	public float slowMoveFactor = 0.25f;

	public float fastMoveFactor = 3f;

	private float rotationX;

	private float rotationY;

	private float horizontalLook;

	private float verticalLook;

	private float xSpeed;

	private float ySpeed;

	private Light myLight;

	private bool isDim = true;

	private Texture lightCookie;
}


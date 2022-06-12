using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;

[RequireComponent(typeof(ScrollRect))]
public class ScrollableArea : MonoBehaviour, IMoveHandler, IEventSystemHandler
{
	private void Awake()
	{
		if (XRDevice.isPresent)
		{
			base.enabled = false;
		}
	}

	public void OnMove(AxisEventData e)
	{
		this.xSpeed += e.moveVector.y * (Mathf.Abs(this.xSpeed) + 0.1f);
		this.ySpeed += e.moveVector.y * (Mathf.Abs(this.ySpeed) + 0.1f);
	}

	public void SetScroll(Vector2 scrollAmount)
	{
		if (scrollAmount.x != 0f)
		{
			this.xSpeed += scrollAmount.x * (Mathf.Abs(this.xSpeed) + 0.1f);
			this.hPos = this.scrollRect.horizontalNormalizedPosition + this.xSpeed * 0.4f;
			this.xSpeed = 0f;
			if (this.scrollRect.movementType == ScrollRect.MovementType.Clamped)
			{
				this.hPos = Mathf.Clamp01(this.hPos);
			}
		}
		else
		{
			this.hPos = this.scrollRect.normalizedPosition.x;
		}
		if (scrollAmount.y != 0f)
		{
			this.ySpeed += scrollAmount.y * (Mathf.Abs(this.ySpeed) + 0.1f);
			this.vPos = this.scrollRect.verticalNormalizedPosition + this.ySpeed * 0.4f;
			this.ySpeed = 0f;
			if (this.scrollRect.movementType == ScrollRect.MovementType.Clamped)
			{
				this.vPos = Mathf.Clamp01(this.vPos);
			}
		}
		else
		{
			this.vPos = this.scrollRect.normalizedPosition.y;
		}
		this.scrollRect.normalizedPosition = new Vector2(this.hPos, this.vPos);
	}

	private void OnEnable()
	{
		if (!XRDevice.isPresent && MainManager.instance.localPlayer.playerInput.enabled)
		{
			MainManager.instance.localPlayer.playerInput.actions["Look"].performed += delegate(InputAction.CallbackContext ctx)
			{
				this.RightStickScroll(ctx);
			};
		}
	}

	private void OnDisable()
	{
		if (!XRDevice.isPresent && MainManager.instance.localPlayer.playerInput.enabled)
		{
			MainManager.instance.localPlayer.playerInput.actions["Look"].performed -= delegate(InputAction.CallbackContext ctx)
			{
				this.RightStickScroll(ctx);
			};
		}
	}

	public void RightStickScroll(InputAction.CallbackContext context)
	{
		Vector2 scroll = context.ReadValue<Vector2>();
		if (MainManager.instance.localPlayer.playerInput.currentControlScheme == "Gamepad")
		{
			this.SetScroll(scroll);
		}
	}

	[SerializeField]
	private ScrollRect scrollRect;

	private const float speedMultiplier = 0.4f;

	private float xSpeed;

	private float ySpeed;

	private float hPos;

	private float vPos;
}


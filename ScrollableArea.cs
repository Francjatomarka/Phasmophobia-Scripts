using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;

// Token: 0x020000F5 RID: 245
[RequireComponent(typeof(ScrollRect))]
public class ScrollableArea : MonoBehaviour, IMoveHandler, IEventSystemHandler
{
	// Token: 0x060006AE RID: 1710 RVA: 0x000276B8 File Offset: 0x000258B8
	private void Awake()
	{
		if (XRDevice.isPresent)
		{
			base.enabled = false;
		}
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x000276C8 File Offset: 0x000258C8
	public void OnMove(AxisEventData e)
	{
		this.xSpeed += e.moveVector.y * (Mathf.Abs(this.xSpeed) + 0.1f);
		this.ySpeed += e.moveVector.y * (Mathf.Abs(this.ySpeed) + 0.1f);
	}

	// Token: 0x060006B0 RID: 1712 RVA: 0x0002772C File Offset: 0x0002592C
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

	// Token: 0x060006B1 RID: 1713 RVA: 0x0002787C File Offset: 0x00025A7C
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

	// Token: 0x060006B2 RID: 1714 RVA: 0x000278D8 File Offset: 0x00025AD8
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

	// Token: 0x060006B3 RID: 1715 RVA: 0x00027934 File Offset: 0x00025B34
	public void RightStickScroll(InputAction.CallbackContext context)
	{
		Vector2 scroll = context.ReadValue<Vector2>();
		if (MainManager.instance.localPlayer.playerInput.currentControlScheme == "Gamepad")
		{
			this.SetScroll(scroll);
		}
	}

	// Token: 0x040006CD RID: 1741
	[SerializeField]
	private ScrollRect scrollRect;

	// Token: 0x040006CE RID: 1742
	private const float speedMultiplier = 0.4f;

	// Token: 0x040006CF RID: 1743
	private float xSpeed;

	// Token: 0x040006D0 RID: 1744
	private float ySpeed;

	// Token: 0x040006D1 RID: 1745
	private float hPos;

	// Token: 0x040006D2 RID: 1746
	private float vPos;
}

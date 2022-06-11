using System;
using UnityEngine;

// Token: 0x020000CA RID: 202
public class InputToEvent : MonoBehaviour
{
	// Token: 0x170000DD RID: 221
	// (get) Token: 0x060005BA RID: 1466 RVA: 0x00020956 File Offset: 0x0001EB56
	// (set) Token: 0x060005BB RID: 1467 RVA: 0x0002095D File Offset: 0x0001EB5D
	public static GameObject goPointedAt { get; private set; }

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x060005BC RID: 1468 RVA: 0x00020965 File Offset: 0x0001EB65
	public Vector2 DragVector
	{
		get
		{
			if (!this.Dragging)
			{
				return Vector2.zero;
			}
			return this.currentPos - this.pressedPosition;
		}
	}

	// Token: 0x060005BD RID: 1469 RVA: 0x00020986 File Offset: 0x0001EB86
	private void Start()
	{
		this.m_Camera = base.GetComponent<Camera>();
	}

	// Token: 0x060005BE RID: 1470 RVA: 0x00020994 File Offset: 0x0001EB94
	private void Update()
	{
		if (this.DetectPointedAtGameObject)
		{
			InputToEvent.goPointedAt = this.RaycastObject(Input.mousePosition);
		}
		if (Input.touchCount <= 0)
		{
			this.currentPos = Input.mousePosition;
			if (Input.GetMouseButtonDown(0))
			{
				this.Press(Input.mousePosition);
			}
			if (Input.GetMouseButtonUp(0))
			{
				this.Release(Input.mousePosition);
			}
			if (Input.GetMouseButtonDown(1))
			{
				this.pressedPosition = Input.mousePosition;
				this.lastGo = this.RaycastObject(this.pressedPosition);
				if (this.lastGo != null)
				{
					this.lastGo.SendMessage("OnPressRight", SendMessageOptions.DontRequireReceiver);
				}
			}
			return;
		}
		Touch touch = Input.GetTouch(0);
		this.currentPos = touch.position;
		if (touch.phase == TouchPhase.Began)
		{
			this.Press(touch.position);
			return;
		}
		if (touch.phase == TouchPhase.Ended)
		{
			this.Release(touch.position);
		}
	}

	// Token: 0x060005BF RID: 1471 RVA: 0x00020A92 File Offset: 0x0001EC92
	private void Press(Vector2 screenPos)
	{
		this.pressedPosition = screenPos;
		this.Dragging = true;
		this.lastGo = this.RaycastObject(screenPos);
		if (this.lastGo != null)
		{
			this.lastGo.SendMessage("OnPress", SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x060005C0 RID: 1472 RVA: 0x00020AD0 File Offset: 0x0001ECD0
	private void Release(Vector2 screenPos)
	{
		if (this.lastGo != null)
		{
			if (this.RaycastObject(screenPos) == this.lastGo)
			{
				this.lastGo.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
			}
			this.lastGo.SendMessage("OnRelease", SendMessageOptions.DontRequireReceiver);
			this.lastGo = null;
		}
		this.pressedPosition = Vector2.zero;
		this.Dragging = false;
	}

	// Token: 0x060005C1 RID: 1473 RVA: 0x00020B3C File Offset: 0x0001ED3C
	private GameObject RaycastObject(Vector2 screenPos)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(this.m_Camera.ScreenPointToRay(screenPos), out raycastHit, 200f))
		{
			InputToEvent.inputHitPos = raycastHit.point;
			return raycastHit.collider.gameObject;
		}
		return null;
	}

	// Token: 0x040005DC RID: 1500
	private GameObject lastGo;

	// Token: 0x040005DD RID: 1501
	public static Vector3 inputHitPos;

	// Token: 0x040005DE RID: 1502
	public bool DetectPointedAtGameObject;

	// Token: 0x040005E0 RID: 1504
	private Vector2 pressedPosition = Vector2.zero;

	// Token: 0x040005E1 RID: 1505
	private Vector2 currentPos = Vector2.zero;

	// Token: 0x040005E2 RID: 1506
	public bool Dragging;

	// Token: 0x040005E3 RID: 1507
	private Camera m_Camera;
}

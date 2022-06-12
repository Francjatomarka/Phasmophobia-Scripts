using System;
using UnityEngine;

public class InputToEvent : MonoBehaviour
{
	// (get) Token: 0x060005BA RID: 1466 RVA: 0x00020956 File Offset: 0x0001EB56
	// (set) Token: 0x060005BB RID: 1467 RVA: 0x0002095D File Offset: 0x0001EB5D
	public static GameObject goPointedAt { get; private set; }

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

	private void Start()
	{
		this.m_Camera = base.GetComponent<Camera>();
	}

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

	private GameObject lastGo;

	public static Vector3 inputHitPos;

	public bool DetectPointedAtGameObject;

	private Vector2 pressedPosition = Vector2.zero;

	private Vector2 currentPos = Vector2.zero;

	public bool Dragging;

	private Camera m_Camera;
}


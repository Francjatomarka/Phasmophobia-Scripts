using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020000F9 RID: 249
public class WorldUIPointer : StandaloneInputModule
{
	// Token: 0x060006BC RID: 1724 RVA: 0x00027B23 File Offset: 0x00025D23
	public void UpdateCursorPosition()
	{
		this.ActivateModule();
		this.Process();
	}

	// Token: 0x060006BD RID: 1725 RVA: 0x00027B34 File Offset: 0x00025D34
	protected override PointerInputModule.MouseState GetMousePointerEventData(int id = 0)
	{
		PointerEventData pointerEventData;
		bool pointerData = base.GetPointerData(-1, out pointerEventData, true);
		pointerEventData.Reset();
		if (pointerData)
		{
			pointerEventData.position = Input.mousePosition;
		}
		pointerEventData.position = Input.mousePosition;
		Vector2 vector = Input.mousePosition;
		pointerEventData.delta = vector - pointerEventData.position;
		pointerEventData.position = vector;
		pointerEventData.scrollDelta = Input.mouseScrollDelta;
		pointerEventData.button = PointerEventData.InputButton.Left;
		base.eventSystem.RaycastAll(pointerEventData, this.m_RaycastResultCache);
		RaycastResult pointerCurrentRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
		pointerEventData.pointerCurrentRaycast = pointerCurrentRaycast;
		this.m_RaycastResultCache.Clear();
		PointerEventData pointerEventData2;
		base.GetPointerData(-2, out pointerEventData2, true);
		base.CopyFromTo(pointerEventData, pointerEventData2);
		pointerEventData2.button = PointerEventData.InputButton.Right;
		PointerEventData pointerEventData3;
		base.GetPointerData(-3, out pointerEventData3, true);
		base.CopyFromTo(pointerEventData, pointerEventData3);
		pointerEventData3.button = PointerEventData.InputButton.Middle;
		this.m_MouseState.SetButtonState(PointerEventData.InputButton.Left, base.StateForMouseButton(0), pointerEventData);
		this.m_MouseState.SetButtonState(PointerEventData.InputButton.Right, base.StateForMouseButton(1), pointerEventData2);
		this.m_MouseState.SetButtonState(PointerEventData.InputButton.Middle, base.StateForMouseButton(2), pointerEventData3);
		return this.m_MouseState;
	}

	// Token: 0x040006D3 RID: 1747
	private readonly PointerInputModule.MouseState m_MouseState = new PointerInputModule.MouseState();
}

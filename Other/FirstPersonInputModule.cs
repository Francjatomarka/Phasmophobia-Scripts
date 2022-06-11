using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020000F2 RID: 242
public class FirstPersonInputModule : StandaloneInputModule
{
	// Token: 0x060006A1 RID: 1697 RVA: 0x00027398 File Offset: 0x00025598
	protected override PointerInputModule.MouseState GetMousePointerEventData(int id)
	{
		CursorLockMode lockState = Cursor.lockState;
		Cursor.lockState = CursorLockMode.None;
		PointerInputModule.MouseState mousePointerEventData = base.GetMousePointerEventData(id);
		Cursor.lockState = lockState;
		return mousePointerEventData;
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x000273BE File Offset: 0x000255BE
	protected override void ProcessMove(PointerEventData pointerEvent)
	{
		CursorLockMode lockState = Cursor.lockState;
		Cursor.lockState = CursorLockMode.None;
		base.ProcessMove(pointerEvent);
		Cursor.lockState = lockState;
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x000273D7 File Offset: 0x000255D7
	protected override void ProcessDrag(PointerEventData pointerEvent)
	{
		CursorLockMode lockState = Cursor.lockState;
		Cursor.lockState = CursorLockMode.None;
		base.ProcessDrag(pointerEvent);
		Cursor.lockState = lockState;
	}
}

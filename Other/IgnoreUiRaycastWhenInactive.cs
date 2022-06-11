using System;
using UnityEngine;

// Token: 0x0200004D RID: 77
public class IgnoreUiRaycastWhenInactive : MonoBehaviour, ICanvasRaycastFilter
{
	// Token: 0x060001AB RID: 427 RVA: 0x0000B927 File Offset: 0x00009B27
	public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
	{
		return base.gameObject.activeInHierarchy;
	}
}

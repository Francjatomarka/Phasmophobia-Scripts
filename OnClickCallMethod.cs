using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000052 RID: 82
[RequireComponent(typeof(Collider))]
public class OnClickCallMethod : MonoBehaviourPunCallbacks
{
	// Token: 0x060001C4 RID: 452 RVA: 0x0000C0A1 File Offset: 0x0000A2A1
	public void OnClick()
	{
		if (this.TargetGameObject == null || string.IsNullOrEmpty(this.TargetMethod))
		{
			Debug.LogWarning(this + " can't call, cause GO or Method are empty.");
			return;
		}
		this.TargetGameObject.SendMessage(this.TargetMethod);
	}

	// Token: 0x040001E8 RID: 488
	public GameObject TargetGameObject;

	// Token: 0x040001E9 RID: 489
	public string TargetMethod;
}

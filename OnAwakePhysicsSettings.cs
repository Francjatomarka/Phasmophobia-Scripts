using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200003D RID: 61
[RequireComponent(typeof(PhotonView))]
public class OnAwakePhysicsSettings : MonoBehaviourPunCallbacks
{
	// Token: 0x06000165 RID: 357 RVA: 0x0000A34C File Offset: 0x0000854C
	public void Awake()
	{
		if (!base.photonView.IsMine)
		{
			Rigidbody component = base.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.isKinematic = true;
				return;
			}
			Rigidbody2D component2 = base.GetComponent<Rigidbody2D>();
			if (component2 != null)
			{
				component2.isKinematic = true;
			}
		}
	}
}

using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x02000150 RID: 336
public class Crucifix : MonoBehaviour
{
	// Token: 0x060008F3 RID: 2291 RVA: 0x00035B8E File Offset: 0x00033D8E
	private void Start()
	{
		if (LevelController.instance)
		{
			LevelController.instance.crucifix.Add(this);
		}
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x00035BAC File Offset: 0x00033DAC
	public void Used()
	{
		this.view.RPC("NetworkedUsed", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x00035BC4 File Offset: 0x00033DC4
	[PunRPC]
	public void NetworkedUsed()
	{
		this.usesCount++;
		if (this.usesCount > 1)
		{
			LevelController.instance.crucifix.Remove(this);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04000910 RID: 2320
	[SerializeField]
	private PhotonView view;

	// Token: 0x04000911 RID: 2321
	private int usesCount;
}

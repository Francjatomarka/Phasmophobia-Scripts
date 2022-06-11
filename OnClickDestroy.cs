using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200008C RID: 140
[RequireComponent(typeof(PhotonView))]
public class OnClickDestroy : MonoBehaviourPunCallbacks
{
	// Token: 0x06000452 RID: 1106 RVA: 0x00018A60 File Offset: 0x00016C60
	public void OnClick()
	{
		if (!this.DestroyByRpc)
		{
			PhotonNetwork.Destroy(base.gameObject);
			return;
		}
		base.photonView.RPC("DestroyRpc", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x00018A8C File Offset: 0x00016C8C
	[PunRPC]
	public IEnumerator DestroyRpc()
	{
		UnityEngine.Object.Destroy(base.gameObject);
		yield return 0;
		PhotonNetwork.AllocateViewID(base.photonView.ViewID);
		yield break;
	}

	// Token: 0x0400046A RID: 1130
	public bool DestroyByRpc;
}

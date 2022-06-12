using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class OnClickDestroy : MonoBehaviourPunCallbacks
{
	public void OnClick()
	{
		if (!this.DestroyByRpc)
		{
			PhotonNetwork.Destroy(base.gameObject);
			return;
		}
		base.photonView.RPC("DestroyRpc", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	[PunRPC]
	public IEnumerator DestroyRpc()
	{
		UnityEngine.Object.Destroy(base.gameObject);
		yield return 0;
		PhotonNetwork.AllocateViewID(base.photonView.ViewID);
		yield break;
	}

	public bool DestroyByRpc;
}


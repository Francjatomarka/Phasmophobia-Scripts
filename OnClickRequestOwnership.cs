using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000046 RID: 70
[RequireComponent(typeof(PhotonView))]
public class OnClickRequestOwnership : MonoBehaviourPunCallbacks
{
	// Token: 0x0600017C RID: 380 RVA: 0x0000A990 File Offset: 0x00008B90
	public void OnClick()
	{
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			Vector3 vector = new Vector3(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
			base.photonView.RPC("ColorRpc", RpcTarget.AllBufferedViaServer, new object[]
			{
				vector
			});
			return;
		}
		if (base.photonView.Owner.UserId == PhotonNetwork.LocalPlayer.UserId)
		{
			Debug.Log("Not requesting ownership. Already mine.");
			return;
		}
		base.photonView.RequestOwnership();
	}

	// Token: 0x0600017D RID: 381 RVA: 0x0000AA38 File Offset: 0x00008C38
	[PunRPC]
	public void ColorRpc(Vector3 col)
	{
		Color color = new Color(col.x, col.y, col.z);
		base.gameObject.GetComponent<Renderer>().material.color = color;
	}
}

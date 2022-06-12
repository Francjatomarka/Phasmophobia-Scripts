using System;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class OnClickRequestOwnership : MonoBehaviourPunCallbacks
{
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

	[PunRPC]
	public void ColorRpc(Vector3 col)
	{
		Color color = new Color(col.x, col.y, col.z);
		base.gameObject.GetComponent<Renderer>().material.color = color;
	}
}


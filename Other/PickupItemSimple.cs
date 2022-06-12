using System;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PickupItemSimple : MonoBehaviourPunCallbacks
{
	public void OnTriggerEnter(Collider other)
	{
		PhotonView component = other.GetComponent<PhotonView>();
		if (this.PickupOnCollide && component != null && component.IsMine)
		{
			this.Pickup();
		}
	}

	public void Pickup()
	{
		if (this.SentPickup)
		{
			return;
		}
		this.SentPickup = true;
		base.photonView.RPC("PunPickupSimple", RpcTarget.AllViaServer, Array.Empty<object>());
	}

	[PunRPC]
	public void PunPickupSimple(PhotonMessageInfo msgInfo)
	{
		if (this.SentPickup && msgInfo.Sender.IsLocal)
		{
			base.gameObject.GetActive();
		}
		this.SentPickup = false;
		if (!base.gameObject.GetActive())
		{
			Debug.Log("Ignored PU RPC, cause item is inactive. " + base.gameObject);
			return;
		}
		double num = PhotonNetwork.Time - msgInfo.timestamp;
		float num2 = this.SecondsBeforeRespawn - (float)num;
		if (num2 > 0f)
		{
			base.gameObject.SetActive(false);
			base.Invoke("RespawnAfter", num2);
		}
	}

	public void RespawnAfter()
	{
		if (base.gameObject != null)
		{
			base.gameObject.SetActive(true);
		}
	}

	public float SecondsBeforeRespawn = 2f;

	public bool PickupOnCollide;

	public bool SentPickup;
}


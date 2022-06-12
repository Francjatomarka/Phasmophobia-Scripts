using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PickupItem : MonoBehaviourPunCallbacks, IPunObservable
{
	// (get) Token: 0x0600045C RID: 1116 RVA: 0x00018C7D File Offset: 0x00016E7D
	public int ViewID
	{
		get
		{
			return base.photonView.ViewID;
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		PhotonView component = other.GetComponent<PhotonView>();
		if (this.PickupOnTrigger && component != null && component.IsMine)
		{
			this.Pickup();
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting && this.SecondsBeforeRespawn <= 0f)
		{
			stream.SendNext(base.gameObject.transform.position);
			return;
		}
		Vector3 position = (Vector3)stream.ReceiveNext();
		base.gameObject.transform.position = position;
	}

	public void Pickup()
	{
		if (this.SentPickup)
		{
			return;
		}
		this.SentPickup = true;
		base.photonView.RPC("PunPickup", RpcTarget.AllViaServer, Array.Empty<object>());
	}

	public void Drop()
	{
		if (this.PickupIsMine)
		{
			base.photonView.RPC("PunRespawn", RpcTarget.AllViaServer, Array.Empty<object>());
		}
	}

	public void Drop(Vector3 newPosition)
	{
		if (this.PickupIsMine)
		{
			base.photonView.RPC("PunRespawn", RpcTarget.AllViaServer, new object[]
			{
				newPosition
			});
		}
	}

	[PunRPC]
	public void PunPickup(PhotonMessageInfo msgInfo)
	{
		if (msgInfo.Sender.IsLocal)
		{
			this.SentPickup = false;
		}
		if (!base.gameObject.GetActive())
		{
			Debug.Log(string.Concat(new object[]
			{
				"Ignored PU RPC, cause item is inactive. ",
				base.gameObject,
				" SecondsBeforeRespawn: ",
				this.SecondsBeforeRespawn,
				" TimeOfRespawn: ",
				this.TimeOfRespawn,
				" respawn in future: ",
				(this.TimeOfRespawn > PhotonNetwork.Time).ToString()
			}));
			return;
		}
		this.PickupIsMine = msgInfo.Sender.IsLocal;
		if (this.OnPickedUpCall != null)
		{
			this.OnPickedUpCall.SendMessage("OnPickedUp", this);
		}
		if (this.SecondsBeforeRespawn <= 0f)
		{
			this.PickedUp(0f);
			return;
		}
		double num = PhotonNetwork.Time - msgInfo.timestamp;
		double num2 = (double)this.SecondsBeforeRespawn - num;
		if (num2 > 0.0)
		{
			this.PickedUp((float)num2);
		}
	}

	internal void PickedUp(float timeUntilRespawn)
	{
		base.gameObject.SetActive(false);
		PickupItem.DisabledPickupItems.Add(this);
		this.TimeOfRespawn = 0.0;
		if (timeUntilRespawn > 0f)
		{
			this.TimeOfRespawn = PhotonNetwork.Time + (double)timeUntilRespawn;
			base.Invoke("PunRespawn", timeUntilRespawn);
		}
	}

	[PunRPC]
	internal void PunRespawn(Vector3 pos)
	{
		Debug.Log("PunRespawn with Position.");
		this.PunRespawn();
		base.gameObject.transform.position = pos;
	}

	[PunRPC]
	internal void PunRespawn()
	{
		PickupItem.DisabledPickupItems.Remove(this);
		this.TimeOfRespawn = 0.0;
		this.PickupIsMine = false;
		if (base.gameObject != null)
		{
			base.gameObject.SetActive(true);
		}
	}

	public float SecondsBeforeRespawn = 2f;

	public bool PickupOnTrigger;

	public bool PickupIsMine;

	public UnityEngine.MonoBehaviour OnPickedUpCall;

	public bool SentPickup;

	public double TimeOfRespawn;

	public static HashSet<PickupItem> DisabledPickupItems = new HashSet<PickupItem>();
}


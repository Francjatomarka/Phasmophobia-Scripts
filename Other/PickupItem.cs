using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000090 RID: 144
[RequireComponent(typeof(PhotonView))]
public class PickupItem : MonoBehaviourPunCallbacks, IPunObservable
{
	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x0600045C RID: 1116 RVA: 0x00018C7D File Offset: 0x00016E7D
	public int ViewID
	{
		get
		{
			return base.photonView.ViewID;
		}
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x00018C8C File Offset: 0x00016E8C
	public void OnTriggerEnter(Collider other)
	{
		PhotonView component = other.GetComponent<PhotonView>();
		if (this.PickupOnTrigger && component != null && component.IsMine)
		{
			this.Pickup();
		}
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x00018CC0 File Offset: 0x00016EC0
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

	// Token: 0x0600045F RID: 1119 RVA: 0x00018D1B File Offset: 0x00016F1B
	public void Pickup()
	{
		if (this.SentPickup)
		{
			return;
		}
		this.SentPickup = true;
		base.photonView.RPC("PunPickup", RpcTarget.AllViaServer, Array.Empty<object>());
	}

	// Token: 0x06000460 RID: 1120 RVA: 0x00018D43 File Offset: 0x00016F43
	public void Drop()
	{
		if (this.PickupIsMine)
		{
			base.photonView.RPC("PunRespawn", RpcTarget.AllViaServer, Array.Empty<object>());
		}
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x00018D63 File Offset: 0x00016F63
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

	// Token: 0x06000462 RID: 1122 RVA: 0x00018D90 File Offset: 0x00016F90
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

	// Token: 0x06000463 RID: 1123 RVA: 0x00018EA4 File Offset: 0x000170A4
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

	// Token: 0x06000464 RID: 1124 RVA: 0x00018EFA File Offset: 0x000170FA
	[PunRPC]
	internal void PunRespawn(Vector3 pos)
	{
		Debug.Log("PunRespawn with Position.");
		this.PunRespawn();
		base.gameObject.transform.position = pos;
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x00018F1D File Offset: 0x0001711D
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

	// Token: 0x04000472 RID: 1138
	public float SecondsBeforeRespawn = 2f;

	// Token: 0x04000473 RID: 1139
	public bool PickupOnTrigger;

	// Token: 0x04000474 RID: 1140
	public bool PickupIsMine;

	// Token: 0x04000475 RID: 1141
	public UnityEngine.MonoBehaviour OnPickedUpCall;

	// Token: 0x04000476 RID: 1142
	public bool SentPickup;

	// Token: 0x04000477 RID: 1143
	public double TimeOfRespawn;

	// Token: 0x04000478 RID: 1144
	public static HashSet<PickupItem> DisabledPickupItems = new HashSet<PickupItem>();
}

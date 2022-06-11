using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000091 RID: 145
[RequireComponent(typeof(PhotonView))]
public class PickupItemSimple : MonoBehaviourPunCallbacks
{
	// Token: 0x06000468 RID: 1128 RVA: 0x00018F7C File Offset: 0x0001717C
	public void OnTriggerEnter(Collider other)
	{
		PhotonView component = other.GetComponent<PhotonView>();
		if (this.PickupOnCollide && component != null && component.IsMine)
		{
			this.Pickup();
		}
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x00018FAF File Offset: 0x000171AF
	public void Pickup()
	{
		if (this.SentPickup)
		{
			return;
		}
		this.SentPickup = true;
		base.photonView.RPC("PunPickupSimple", RpcTarget.AllViaServer, Array.Empty<object>());
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x00018FD8 File Offset: 0x000171D8
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

	// Token: 0x0600046B RID: 1131 RVA: 0x00019068 File Offset: 0x00017268
	public void RespawnAfter()
	{
		if (base.gameObject != null)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x04000479 RID: 1145
	public float SecondsBeforeRespawn = 2f;

	// Token: 0x0400047A RID: 1146
	public bool PickupOnCollide;

	// Token: 0x0400047B RID: 1147
	public bool SentPickup;
}

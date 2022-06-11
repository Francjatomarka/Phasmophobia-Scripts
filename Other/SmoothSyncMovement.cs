using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200009D RID: 157
[RequireComponent(typeof(PhotonView))]
public class SmoothSyncMovement : MonoBehaviourPunCallbacks, IPunObservable
{
	// Token: 0x060004A5 RID: 1189 RVA: 0x00019CA8 File Offset: 0x00017EA8
	public void Awake()
	{
		bool flag = false;
		using (List<Component>.Enumerator enumerator = base.photonView.ObservedComponents.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current == this)
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			Debug.LogWarning(this + " is not observed by this object's photonView! OnPhotonSerializeView() in this class won't be used.");
		}
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x00019D1C File Offset: 0x00017F1C
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(base.transform.position);
			stream.SendNext(base.transform.rotation);
			return;
		}
		this.correctPlayerPos = (Vector3)stream.ReceiveNext();
		this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x00019D80 File Offset: 0x00017F80
	public void Update()
	{
		if (!base.photonView.IsMine)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, this.correctPlayerPos, Time.deltaTime * this.SmoothingDelay);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.correctPlayerRot, Time.deltaTime * this.SmoothingDelay);
		}
	}

	// Token: 0x0400048C RID: 1164
	public float SmoothingDelay = 5f;

	// Token: 0x0400048D RID: 1165
	private Vector3 correctPlayerPos = Vector3.zero;

	// Token: 0x0400048E RID: 1166
	private Quaternion correctPlayerRot = Quaternion.identity;
}

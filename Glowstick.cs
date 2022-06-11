using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Photon.Pun;

// Token: 0x02000112 RID: 274
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(AudioSource))]
public class Glowstick : MonoBehaviour
{
	// Token: 0x060007A0 RID: 1952 RVA: 0x0002DCB0 File Offset: 0x0002BEB0
	private void Start()
	{
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
		if (XRDevice.isPresent)
		{
			this.photonInteract.AddGrabbedEvent(new UnityAction(this.Grabbed));
			this.photonInteract.AddUnGrabbedEvent(new UnityAction(this.Dropped));
			return;
		}
		this.photonInteract.AddPCGrabbedEvent(new UnityAction(this.Grabbed));
		this.photonInteract.AddPCUnGrabbedEvent(new UnityAction(this.Dropped));
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x0002DD38 File Offset: 0x0002BF38
	private void Grabbed()
	{
		if (!this.used)
		{
			return;
		}
        if (PhotonNetwork.InRoom)
        {
			this.view.RPC("SyncGrab", RpcTarget.All, new object[]
			{
				true
			});
			return;
		} 
		else
        {
			SyncGrab(true);
		}
	}

	// Token: 0x060007A2 RID: 1954 RVA: 0x0002DD63 File Offset: 0x0002BF63
	public void Dropped()
	{
		if (!this.used)
		{
			return;
		}
		base.StartCoroutine(this.DropDelay());
	}

	// Token: 0x060007A3 RID: 1955 RVA: 0x0002DD7B File Offset: 0x0002BF7B
	private IEnumerator DropDelay()
	{
		yield return new WaitForSeconds(0.1f);
		if (!base.transform.root.CompareTag("Player"))
		{
			if (PhotonNetwork.InRoom)
			{
				this.view.RPC("SyncGrab", RpcTarget.All, new object[]
				{
					false
				});
				yield return null;
			}
			SyncGrab(false);
		}
		yield break;
	}

	// Token: 0x060007A4 RID: 1956 RVA: 0x0002DD8C File Offset: 0x0002BF8C
	private void Use()
	{
		if (this.used)
		{
			return;
		}
		if (PhotonNetwork.InRoom)
		{
			this.view.RPC("NetworkedUse", RpcTarget.All, Array.Empty<object>());
		} 
		else
        {
			NetworkedUse();
		}
		if (this.photonInteract.isGrabbed)
		{
			if (PhotonNetwork.InRoom)
			{
				this.view.RPC("SyncGrab", RpcTarget.All, new object[]
				{
					true
				});
				return;
			}
			SyncGrab(true);
		}
	}

	// Token: 0x060007A5 RID: 1957 RVA: 0x0002DDE5 File Offset: 0x0002BFE5
	[PunRPC]
	private void NetworkedUse()
	{
		this.used = true;
		this.myLight.enabled = true;
		this.rend.material.EnableKeyword("_EMISSION");
	}

	// Token: 0x060007A6 RID: 1958 RVA: 0x0002DE0F File Offset: 0x0002C00F
	[PunRPC]
	private void SyncGrab(bool isGrabbed)
	{
		this.myLight.range = (isGrabbed ? 0.5f : 1.5f);
	}

	// Token: 0x040007A9 RID: 1961
	[SerializeField]
	private Light myLight;

	// Token: 0x040007AA RID: 1962
	[SerializeField]
	private Renderer rend;

	// Token: 0x040007AB RID: 1963
	[SerializeField]
	private PhotonView view;

	// Token: 0x040007AC RID: 1964
	[SerializeField]
	private PhotonObjectInteract photonInteract;

	// Token: 0x040007AD RID: 1965
	private bool used;
}

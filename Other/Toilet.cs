using System;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

// Token: 0x02000109 RID: 265
public class Toilet : MonoBehaviour
{
	// Token: 0x06000742 RID: 1858 RVA: 0x0002B176 File Offset: 0x00029376
	private void Awake()
	{
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.source = base.GetComponent<AudioSource>();
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x06000743 RID: 1859 RVA: 0x0002B19C File Offset: 0x0002939C
	private void Start()
	{
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
	}

	// Token: 0x06000744 RID: 1860 RVA: 0x0002B1B5 File Offset: 0x000293B5
	private void Use()
	{
		this.view.RPC("NetworkedUse", RpcTarget.All, Array.Empty<object>());
	}

	// Token: 0x06000745 RID: 1861 RVA: 0x0002B1CD File Offset: 0x000293CD
	[PunRPC]
	private void NetworkedUse()
	{
		if (this.source.isPlaying)
		{
			return;
		}
		this.source.Play();
	}

	// Token: 0x04000758 RID: 1880
	private PhotonObjectInteract photonInteract;

	// Token: 0x04000759 RID: 1881
	private AudioSource source;

	// Token: 0x0400075A RID: 1882
	private PhotonView view;
}

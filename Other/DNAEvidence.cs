using System;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

// Token: 0x0200012D RID: 301
public class DNAEvidence : MonoBehaviour
{
	// Token: 0x06000805 RID: 2053 RVA: 0x00030299 File Offset: 0x0002E499
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.rigid = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x000302C0 File Offset: 0x0002E4C0
	private void Start()
	{
		this.photonInteract.AddPCGrabbedEvent(new UnityAction(this.Grabbed));
		this.photonInteract.AddGrabbedEvent(new UnityAction(this.Grabbed));
		if (!this.view.IsMine)
		{
			this.rigid.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
			this.rigid.isKinematic = true;
			this.rigid.useGravity = false;
		}
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x0003032C File Offset: 0x0002E52C
	private void Grabbed()
	{
		this.view.RPC("GrabbedNetworked", RpcTarget.All, Array.Empty<object>());
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x00030344 File Offset: 0x0002E544
	[PunRPC]
	private void GrabbedNetworked()
	{
		if(GameController.instance != null && EvidenceController.instance != null)
        {
			EvidenceController.instance.foundGhostDNA = true;
			GameController.instance.myPlayer.player.evidenceAudioSource.Play();
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x04000827 RID: 2087
	private PhotonView view;

	// Token: 0x04000828 RID: 2088
	private PhotonObjectInteract photonInteract;

	// Token: 0x04000829 RID: 2089
	private Rigidbody rigid;
}

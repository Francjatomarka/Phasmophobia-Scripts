using System;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class DNAEvidence : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.rigid = base.GetComponent<Rigidbody>();
	}

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

	private void Grabbed()
	{
		this.view.RPC("GrabbedNetworked", RpcTarget.All, Array.Empty<object>());
	}

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

	private PhotonView view;

	private PhotonObjectInteract photonInteract;

	private Rigidbody rigid;
}


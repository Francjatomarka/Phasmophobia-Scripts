using System;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class Toilet : MonoBehaviour
{
	private void Awake()
	{
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.source = base.GetComponent<AudioSource>();
		this.view = base.GetComponent<PhotonView>();
	}

	private void Start()
	{
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
	}

	private void Use()
	{
		this.view.RPC("NetworkedUse", RpcTarget.All, Array.Empty<object>());
	}

	[PunRPC]
	private void NetworkedUse()
	{
		if (this.source.isPlaying)
		{
			return;
		}
		this.source.Play();
	}

	private PhotonObjectInteract photonInteract;

	private AudioSource source;

	private PhotonView view;
}


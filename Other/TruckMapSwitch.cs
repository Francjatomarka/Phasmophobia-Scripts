using System;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class TruckMapSwitch : MonoBehaviour
{
	private void Awake()
	{
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.view = base.GetComponent<PhotonView>();
	}

	private void Start()
	{
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
	}

	private void Use()
	{
		if (GameController.instance.allPlayersAreConnected)
		{
			this.view.RPC("NetworkedUse", RpcTarget.All, Array.Empty<object>());
		}
	}

	[PunRPC]
	private void NetworkedUse()
	{
		if (this.source)
		{
			this.source.Play();
		}
		if (MapController.instance)
		{
			MapController.instance.ChangeFloor();
		}
	}

	private PhotonView view;

	private PhotonObjectInteract photonInteract;

	[SerializeField]
	private AudioSource source;
}


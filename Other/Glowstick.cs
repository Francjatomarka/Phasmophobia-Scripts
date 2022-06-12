using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(AudioSource))]
public class Glowstick : MonoBehaviour
{
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

	public void Dropped()
	{
		if (!this.used)
		{
			return;
		}
		base.StartCoroutine(this.DropDelay());
	}

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

	[PunRPC]
	private void NetworkedUse()
	{
		this.used = true;
		this.myLight.enabled = true;
		this.rend.material.EnableKeyword("_EMISSION");
	}

	[PunRPC]
	private void SyncGrab(bool isGrabbed)
	{
		this.myLight.range = (isGrabbed ? 0.5f : 1.5f);
	}

	[SerializeField]
	private Light myLight;

	[SerializeField]
	private Renderer rend;

	[SerializeField]
	private PhotonView view;

	[SerializeField]
	private PhotonObjectInteract photonInteract;

	private bool used;
}


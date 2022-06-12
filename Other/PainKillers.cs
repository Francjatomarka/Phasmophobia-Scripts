using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Photon.Pun;

public class PainKillers : MonoBehaviour
{
	private void Awake()
	{
		this.noise = base.GetComponentInChildren<Noise>();
		this.source = base.GetComponent<AudioSource>();
		this.view = base.GetComponent<PhotonView>();
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.noise.gameObject.SetActive(false);
	}

	private void Start()
	{
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
	}

	private void Use()
	{
		this.hasBeenUsed = true;
		if (!this.hasBeenUsed)
		{
			Player player = GameObject.Find("PCPlayer(Clone)").GetComponent<Player>();
			player.insanity -= 40f;
		}
        if (PhotonNetwork.InRoom)
        {
			this.view.RPC("NetworkedUse", RpcTarget.All, Array.Empty<object>());
			return;
		}
		NetworkedUse();
	}

	[PunRPC]
	private void NetworkedUse()
	{
		this.hasBeenUsed = true;
		base.StartCoroutine(this.PlayNoiseObject());
		base.StartCoroutine(this.UsePills());
		if (PhotonNetwork.IsMasterClient || !PhotonNetwork.InRoom)
		{
			if(LevelController.instance != null)
            {
				LevelController.instance.itemSpawner.RemovePainKillers();
			}
		}
	}

	private IEnumerator UsePills()
	{
		this.source.Play();
		yield return new WaitUntil(() => !this.source.isPlaying);
		if (this.view.IsMine)
		{
			if (XRDevice.isPresent)
			{
				this.photonInteract.ActivateHands();
			}
			else
			{
				PCPropGrab pcPropGrab = GameObject.Find("PCPlayerHead").GetComponent<PCPropGrab>();
				pcPropGrab.Drop(false);
			}
		}
		base.gameObject.SetActive(false);
		yield break;
	}

	private IEnumerator PlayNoiseObject()
	{
		this.noise.gameObject.SetActive(true);
		yield return 0;
		this.noise.gameObject.SetActive(false);
		yield break;
	}

	private AudioSource source;

	private PhotonView view;

	private PhotonObjectInteract photonInteract;

	private Noise noise;

	private bool hasBeenUsed;
}


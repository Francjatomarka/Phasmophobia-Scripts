using System;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class Radio : MonoBehaviour
{
	private void Awake()
	{
		this.source = base.GetComponent<AudioSource>();
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.view = base.GetComponent<PhotonView>();
		this.noise.gameObject.SetActive(false);
	}

	private void Start()
	{
		this.isOn = false;
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
		this.source.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
	}

	private void Use()
	{
		this.view.RPC("NetworkedUse", RpcTarget.All, Array.Empty<object>());
	}

	[PunRPC]
	private void NetworkedUse()
	{
		this.isOn = !this.isOn;
		this.noise.gameObject.SetActive(this.isOn);
		if (this.isOn)
		{
			this.source.Play();
			return;
		}
		this.source.Stop();
	}

	[PunRPC]
	private void TurnOn()
	{
		this.isOn = true;
		this.source.Play();
		this.noise.gameObject.SetActive(this.isOn);
	}

	[HideInInspector]
	public PhotonView view;

	private PhotonObjectInteract photonInteract;

	[SerializeField]
	private Noise noise;

	private bool isOn;

	private AudioSource source;
}


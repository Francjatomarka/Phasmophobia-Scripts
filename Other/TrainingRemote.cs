using System;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class TrainingRemote : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.source = base.GetComponent<AudioSource>();
	}

	private void Start()
	{
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
	}

	private void Use()
	{
		TrainingController.instance.NextSlide();
		this.source.Play();
	}

	private PhotonView view;

	private PhotonObjectInteract photonInteract;

	private AudioSource source;
}


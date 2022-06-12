using System;
using UnityEngine;
using Photon.Pun;

public class KeyWarning : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.isTrigger)
		{
			return;
		}
		if (PhotonNetwork.InRoom && other.transform.root.CompareTag("Player"))
		{
			this.view.RPC("PlayAudio", PhotonNetwork.MasterClient, Array.Empty<object>());
		}
	}

	[PunRPC]
	private void PlayAudio()
	{
		if (!TruckRadioController.instance.playedKeyAudio)
		{
			TruckRadioController.instance.PlayKeyWarningAudio();
		}
	}

	private PhotonView view;
}


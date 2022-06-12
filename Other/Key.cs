using System;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class Key : MonoBehaviour
{
	private void Awake()
	{
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.view = base.GetComponent<PhotonView>();
	}

	private void Start()
	{
		this.photonInteract.AddGrabbedEvent(new UnityAction(this.Grabbed));
		this.photonInteract.AddPCGrabbedEvent(new UnityAction(this.Grabbed));
		switch (this.type)
		{
		case Key.KeyType.main:
			this.KeyName = "Key_Main";
			return;
		case Key.KeyType.basement:
			this.KeyName = "Key_Basement";
			return;
		case Key.KeyType.garage:
			this.KeyName = "Key_Garage";
			return;
		case Key.KeyType.Car:
			this.KeyName = "Key_Car";
			return;
		default:
			return;
		}
	}

	private void Grabbed()
	{
        if (!PhotonNetwork.InRoom)
        {
			GrabbedKey();
		}
		else
        {
			this.view.RPC("GrabbedKey", RpcTarget.AllBuffered, new object[]{});
		}
		if(GameController.instance != null)
        {
			GameController.instance.myPlayer.player.keysAudioSource.Play();
		}
	}

	[PunRPC]
	private void GrabbedKey()
	{
		if (GameController.instance != null && LevelController.instance != null)
		{
			GameController.instance.myPlayer.player.keys.Add(this.type);
			LevelController.instance.journalController.AddKey(LocalisationSystem.GetLocalisedValue(this.KeyName));
		}
		base.gameObject.SetActive(false);
	}

	private PhotonView view;

	public Key.KeyType type;

	private Door door;

	private PhotonObjectInteract photonInteract;

	private Ray playerAim;

	[SerializeField]
	private LayerMask mask;

	private string KeyName = "Key";

	public enum KeyType
	{
		main,
		basement,
		garage,
		Car,
		none
	}
}


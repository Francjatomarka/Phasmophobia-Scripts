using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using Photon.Pun;

public class FallTeleportBox : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.isTrigger)
		{
			return;
		}
		if (other.transform.root.CompareTag("Ghost"))
		{
			return;
		}
		if (other.transform.root.CompareTag("Player"))
		{
			if (GameController.instance)
			{
				if (GameController.instance.myPlayer == null)
				{
					return;
				}
			}
			else if (MainManager.instance && MainManager.instance.localPlayer == null)
			{
				return;
			}
			if (other.GetComponent<PhotonObjectInteract>())
			{
				return;
			}
			if (other.GetComponent<ThermometerSpot>())
			{
				return;
			}
			if (other.transform.root.GetComponent<PhotonView>().IsMine || !PhotonNetwork.InRoom)
			{
				this.TeleportPlayer();
				return;
			}
		}
		else if (other.GetComponent<PhotonObjectInteract>())
		{
			if (other.GetComponent<PhotonView>())
			{
				if ((other.GetComponent<PhotonView>().IsMine || !PhotonNetwork.InRoom) && !other.GetComponent<PhotonObjectInteract>().isGrabbed && !other.GetComponent<ThermometerSpot>())
				{
					other.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
					other.transform.position = other.transform.GetComponent<PhotonObjectInteract>().spawnPoint;
					return;
				}
			}
			else
			{
				Debug.LogError(other.name + " does not have a photonview! Can't teleport the object back.");
			}
		}
	}

	private void Update()
	{
		if (Keyboard.current.pKey.wasPressedThisFrame && Application.isEditor)
		{
			this.TeleportPlayer();
		}
	}

	private void TeleportPlayer()
	{
		if (GameController.instance)
		{
			GameController.instance.myPlayer.player.transform.position = MultiplayerController.instance.spawns[0].position;
			return;
		}
		MainManager.instance.localPlayer.transform.position = MainManager.instance.spawns[0].position;
		return;
	}
}


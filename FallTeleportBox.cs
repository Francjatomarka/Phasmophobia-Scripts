using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using Photon.Pun;

// Token: 0x0200012C RID: 300
public class FallTeleportBox : MonoBehaviour
{
	// Token: 0x06000856 RID: 2134 RVA: 0x000329E4 File Offset: 0x00030BE4
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

	// Token: 0x06000857 RID: 2135 RVA: 0x00032B36 File Offset: 0x00030D36
	private void Update()
	{
		if (Keyboard.current.pKey.wasPressedThisFrame && Application.isEditor)
		{
			this.TeleportPlayer();
		}
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x00032B58 File Offset: 0x00030D58
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

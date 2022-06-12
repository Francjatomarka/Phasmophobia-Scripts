using System;
using UnityEngine;
using Photon.Pun;

public class SaltSpot : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
	}

	[PunRPC]
	private void SyncSalt()
	{
		this.normalSalt.SetActive(false);
		this.flatSalt.SetActive(true);
		if(LevelController.instance != null)
        {
			LevelController.instance.currentGhost.ghostInteraction.hasWalkedInSalt = true;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.isTrigger)
		{
			return;
		}
		if (this.used)
		{
			return;
		}
		if (!PhotonNetwork.IsMasterClient || PhotonNetwork.InRoom)
		{
			return;
		}
		if (other.CompareTag("Ghost"))
		{
			if (LevelController.instance != null && LevelController.instance.currentGhost.ghostInteraction.hasWalkedInSalt)
			{
				return;
			}
            if (PhotonNetwork.InRoom)
            {
				this.view.RPC("SyncSalt", RpcTarget.All, Array.Empty<object>());
				this.used = true;
				return;
			}
			this.SyncSalt();
			this.used = true;
		}
	}

	[SerializeField]
	private GameObject normalSalt;

	[SerializeField]
	private GameObject flatSalt;

	private PhotonView view;

	private bool used;
}


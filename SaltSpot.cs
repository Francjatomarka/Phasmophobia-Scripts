using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x02000120 RID: 288
public class SaltSpot : MonoBehaviour
{
	// Token: 0x06000818 RID: 2072 RVA: 0x00031394 File Offset: 0x0002F594
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x000313A2 File Offset: 0x0002F5A2
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

	// Token: 0x0600081A RID: 2074 RVA: 0x000313D4 File Offset: 0x0002F5D4
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

	// Token: 0x04000816 RID: 2070
	[SerializeField]
	private GameObject normalSalt;

	// Token: 0x04000817 RID: 2071
	[SerializeField]
	private GameObject flatSalt;

	// Token: 0x04000818 RID: 2072
	private PhotonView view;

	// Token: 0x04000819 RID: 2073
	private bool used;
}

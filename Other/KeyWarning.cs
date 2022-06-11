using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x020000CA RID: 202
public class KeyWarning : MonoBehaviour
{
	// Token: 0x060005A5 RID: 1445 RVA: 0x00020DAC File Offset: 0x0001EFAC
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x060005A6 RID: 1446 RVA: 0x00020DBC File Offset: 0x0001EFBC
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

	// Token: 0x060005A7 RID: 1447 RVA: 0x00020E0A File Offset: 0x0001F00A
	[PunRPC]
	private void PlayAudio()
	{
		if (!TruckRadioController.instance.playedKeyAudio)
		{
			TruckRadioController.instance.PlayKeyWarningAudio();
		}
	}

	// Token: 0x04000549 RID: 1353
	private PhotonView view;
}

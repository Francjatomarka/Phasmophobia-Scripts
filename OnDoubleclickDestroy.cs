using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200003F RID: 63
public class OnDoubleclickDestroy : MonoBehaviourPunCallbacks
{
	// Token: 0x0600016A RID: 362 RVA: 0x0000A3BC File Offset: 0x000085BC
	private void OnClick()
	{
		if (!base.photonView.IsMine)
		{
			return;
		}
		if (Time.time - this.timeOfLastClick < 0.2f)
		{
			PhotonNetwork.Destroy(base.gameObject);
			return;
		}
		this.timeOfLastClick = Time.time;
	}

	// Token: 0x040001AB RID: 427
	private float timeOfLastClick;

	// Token: 0x040001AC RID: 428
	private const float ClickDeltaForDoubleclick = 0.2f;
}

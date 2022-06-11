using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x02000129 RID: 297
public class WhiteSageTrigger : MonoBehaviour
{
	// Token: 0x06000850 RID: 2128 RVA: 0x00032718 File Offset: 0x00030918
	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Lighter>() != null)
		{
			if (other.GetComponent<Lighter>().isOn && other.GetComponent<PhotonView>().IsMine)
			{
				this.whiteSage.Use();
				return;
			}
		}
		else if (other.GetComponent<Candle>() != null && other.GetComponent<Candle>().isOn && other.GetComponent<PhotonView>().IsMine)
		{
			this.whiteSage.Use();
		}
	}

	// Token: 0x04000853 RID: 2131
	[SerializeField]
	private WhiteSage whiteSage;
}

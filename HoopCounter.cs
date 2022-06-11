using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// Token: 0x0200017C RID: 380
public class HoopCounter : MonoBehaviour
{
	// Token: 0x06000A2D RID: 2605 RVA: 0x0003EFBD File Offset: 0x0003D1BD
	private void Start()
	{
		this.counter = PlayerPrefs.GetInt("HoopCounter");
		this.counterText.text = this.counter.ToString();
	}

	// Token: 0x06000A2E RID: 2606 RVA: 0x0003EFE8 File Offset: 0x0003D1E8
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Ball"))
		{
			if (other.GetComponent<PhotonObjectInteract>().isGrabbed)
			{
				return;
			}
			if (!other.GetComponent<PhotonObjectInteract>().view.IsMine && PhotonNetwork.InRoom)
			{
				return;
			}
			this.counter++;
			this.counterText.text = this.counter.ToString();
			PlayerPrefs.SetInt("HoopCounter", this.counter);
		}
	}

	// Token: 0x04000A57 RID: 2647
	private int counter;

	// Token: 0x04000A58 RID: 2648
	[SerializeField]
	private Text counterText;
}

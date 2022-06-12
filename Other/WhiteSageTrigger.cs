using System;
using UnityEngine;
using Photon.Pun;

public class WhiteSageTrigger : MonoBehaviour
{
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

	[SerializeField]
	private WhiteSage whiteSage;
}


using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class HoopCounter : MonoBehaviour
{
	private void Start()
	{
		this.counter = PlayerPrefs.GetInt("HoopCounter");
		this.counterText.text = this.counter.ToString();
	}

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

	private int counter;

	[SerializeField]
	private Text counterText;
}


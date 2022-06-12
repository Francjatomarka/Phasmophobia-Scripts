using System;
using Photon.Pun;
using UnityEngine;

public class OnDoubleclickDestroy : MonoBehaviourPunCallbacks
{
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

	private float timeOfLastClick;

	private const float ClickDeltaForDoubleclick = 0.2f;
}


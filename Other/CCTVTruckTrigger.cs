using System;
using UnityEngine;
using Photon.Pun;

public class CCTVTruckTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.root.CompareTag("Player"))
		{
			if (other.isTrigger)
			{
				return;
			}
			if (other.GetComponent<PhotonObjectInteract>() && !other.GetComponent<WalkieTalkie>())
			{
				return;
			}
			if (other.GetComponent<ThermometerSpot>())
			{
				return;
			}
			if (other.transform.root.GetComponent<PhotonView>().IsMine)
			{
				CCTVController.instance.StartRendering();
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.transform.root.CompareTag("Player"))
		{
			if (other.isTrigger)
			{
				return;
			}
			if (other.GetComponent<PhotonObjectInteract>() && !other.GetComponent<WalkieTalkie>())
			{
				return;
			}
			if (other.GetComponent<ThermometerSpot>())
			{
				return;
			}
			if (other.transform.root.GetComponent<PhotonView>().IsMine)
			{
				CCTVController.instance.StopRendering();
			}
		}
	}
}


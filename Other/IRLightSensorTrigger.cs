using System;
using UnityEngine;
using Photon.Pun;

public class IRLightSensorTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (!this.irLightSensor.isPlaced)
		{
			return;
		}
		if (other.isTrigger)
		{
			return;
		}
		if (PhotonNetwork.IsMasterClient && other.CompareTag("Ghost"))
		{
			this.irLightSensor.Detection();
		}
		if (other.transform.root.CompareTag("Player"))
		{
			this.irLightSensor.Detection();
		}
	}

	[SerializeField]
	private IRLightSensor irLightSensor;
}


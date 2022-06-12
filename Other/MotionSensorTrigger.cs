using System;
using UnityEngine;
using Photon.Pun;

public class MotionSensorTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (!this.motionSensor.isPlaced)
		{
			return;
		}
		if ((PhotonNetwork.IsMasterClient && !other.isTrigger) || (!other.isTrigger))
		{
			if (other.CompareTag("Ghost"))
			{
				this.motionSensor.Detection(true);
				return;
			}
			if (other.transform.root.CompareTag("Player"))
			{
				this.motionSensor.Detection(false);
			}
		}
	}

	[SerializeField]
	private MotionSensor motionSensor;
}


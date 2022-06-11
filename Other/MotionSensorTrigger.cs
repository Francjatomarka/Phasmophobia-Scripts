using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x02000119 RID: 281
public class MotionSensorTrigger : MonoBehaviour
{
	// Token: 0x060007DD RID: 2013 RVA: 0x0002F2E8 File Offset: 0x0002D4E8
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

	// Token: 0x040007ED RID: 2029
	[SerializeField]
	private MotionSensor motionSensor;
}

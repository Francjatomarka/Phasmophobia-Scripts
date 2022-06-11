using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x02000158 RID: 344
public class IRLightSensorTrigger : MonoBehaviour
{
	// Token: 0x0600093E RID: 2366 RVA: 0x0003825C File Offset: 0x0003645C
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

	// Token: 0x0400095B RID: 2395
	[SerializeField]
	private IRLightSensor irLightSensor;
}

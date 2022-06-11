using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x02000179 RID: 377
public class CCTVTruckTrigger : MonoBehaviour
{
	// Token: 0x06000AC1 RID: 2753 RVA: 0x000428C0 File Offset: 0x00040AC0
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

	// Token: 0x06000AC2 RID: 2754 RVA: 0x00042938 File Offset: 0x00040B38
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

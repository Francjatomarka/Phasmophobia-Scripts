using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C1 RID: 449
[RequireComponent(typeof(Rigidbody))]
public class ExitLevelTrigger : MonoBehaviour
{
	// Token: 0x06000C63 RID: 3171 RVA: 0x0004EA60 File Offset: 0x0004CC60
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
			if (other.GetComponent<VRJournal>())
			{
				return;
			}
			if (other.GetComponent<Noise>())
			{
				return;
			}
			if (!this.playersInTruck.Contains(other.transform.root.GetComponent<Player>()))
			{
				this.playersInTruck.Add(other.transform.root.GetComponent<Player>());
			}
		}
	}

	// Token: 0x06000C64 RID: 3172 RVA: 0x0004EB10 File Offset: 0x0004CD10
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
			if (other.GetComponent<VRJournal>())
			{
				return;
			}
			if (other.GetComponent<Noise>())
			{
				return;
			}
			if (this.playersInTruck.Contains(other.transform.root.GetComponent<Player>()))
			{
				this.playersInTruck.Remove(other.transform.root.GetComponent<Player>());
			}
		}
	}

	// Token: 0x04000CED RID: 3309
	public List<Player> playersInTruck = new List<Player>();
}

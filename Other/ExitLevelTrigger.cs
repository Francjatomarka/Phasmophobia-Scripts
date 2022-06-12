using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ExitLevelTrigger : MonoBehaviour
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

	public List<Player> playersInTruck = new List<Player>();
}


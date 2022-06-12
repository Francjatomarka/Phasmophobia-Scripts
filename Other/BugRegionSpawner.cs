using System;
using UnityEngine;
using Photon.Pun;

public class BugRegionSpawner : MonoBehaviour
{
	private void Start()
	{
		for (int i = 0; i < this.amountToSpawn; i++)
		{
			BugsAI component = PhotonNetwork.Instantiate(this.bugToSpawn.name, base.transform.position, this.bugToSpawn.transform.rotation, 0, null).GetComponent<BugsAI>();
			component.col = this.regionCollider;
			component.transform.SetParent(base.transform);
		}
	}

	[SerializeField]
	private BoxCollider regionCollider;

	[SerializeField]
	private int amountToSpawn;

	[SerializeField]
	private GameObject bugToSpawn;
}


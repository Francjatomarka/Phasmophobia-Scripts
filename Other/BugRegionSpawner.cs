using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x02000029 RID: 41
public class BugRegionSpawner : MonoBehaviour
{
	// Token: 0x06000107 RID: 263 RVA: 0x000088AC File Offset: 0x00006AAC
	private void Start()
	{
		for (int i = 0; i < this.amountToSpawn; i++)
		{
			BugsAI component = PhotonNetwork.Instantiate(this.bugToSpawn.name, base.transform.position, this.bugToSpawn.transform.rotation, 0, null).GetComponent<BugsAI>();
			component.col = this.regionCollider;
			component.transform.SetParent(base.transform);
		}
	}

	// Token: 0x04000135 RID: 309
	[SerializeField]
	private BoxCollider regionCollider;

	// Token: 0x04000136 RID: 310
	[SerializeField]
	private int amountToSpawn;

	// Token: 0x04000137 RID: 311
	[SerializeField]
	private GameObject bugToSpawn;
}

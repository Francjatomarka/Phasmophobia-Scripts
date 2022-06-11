using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x0200008E RID: 142
public class OnJoinedInstantiate : MonoBehaviour
{
	// Token: 0x06000458 RID: 1112 RVA: 0x00018BAC File Offset: 0x00016DAC
	public void OnJoinedRoom()
	{
		if (this.PrefabsToInstantiate != null)
		{
			foreach (GameObject gameObject in this.PrefabsToInstantiate)
			{
				Debug.Log("Instantiating: " + gameObject.name);
				Vector3 a = Vector3.up;
				if (this.SpawnPosition != null)
				{
					a = this.SpawnPosition.position;
				}
				Vector3 a2 = UnityEngine.Random.insideUnitSphere;
				a2.y = 0f;
				a2 = a2.normalized;
				Vector3 position = a + this.PositionOffset * a2;
				PhotonNetwork.Instantiate(gameObject.name, position, Quaternion.identity, 0);
			}
		}
	}

	// Token: 0x0400046F RID: 1135
	public Transform SpawnPosition;

	// Token: 0x04000470 RID: 1136
	public float PositionOffset = 2f;

	// Token: 0x04000471 RID: 1137
	public GameObject[] PrefabsToInstantiate;
}

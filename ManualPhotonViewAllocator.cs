using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x02000089 RID: 137
[RequireComponent(typeof(PhotonView))]
public class ManualPhotonViewAllocator : MonoBehaviour
{
	// Token: 0x06000444 RID: 1092 RVA: 0x000182A4 File Offset: 0x000164A4
	public void AllocateManualPhotonView()
	{
		PhotonView photonView = base.gameObject.GetPhotonView();
		if (photonView == null)
		{
			Debug.LogError("Can't do manual instantiation without PhotonView component.");
			return;
		}
	}

	// Token: 0x06000445 RID: 1093 RVA: 0x000182F4 File Offset: 0x000164F4
	[PunRPC]
	public void InstantiateRpc(int viewID)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Prefab, InputToEvent.inputHitPos + new Vector3(0f, 5f, 0f), Quaternion.identity);
		gameObject.GetPhotonView().ViewID = viewID;
		gameObject.GetComponent<OnClickDestroy>().DestroyByRpc = true;
	}

	// Token: 0x0400045B RID: 1115
	public GameObject Prefab;
}

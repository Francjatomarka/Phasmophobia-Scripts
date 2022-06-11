using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x02000043 RID: 67
public class InstantiateCube : MonoBehaviour
{
	// Token: 0x06000175 RID: 373 RVA: 0x0000A858 File Offset: 0x00008A58
	private void OnClick()
	{
		int instantiateType = this.InstantiateType;
		if (instantiateType == 0)
		{
			PhotonNetwork.Instantiate(this.Prefab.name, base.transform.position + 3f * Vector3.up, Quaternion.identity, 0);
			return;
		}
		if (instantiateType != 1)
		{
			return;
		}
		PhotonNetwork.InstantiateSceneObject(this.Prefab.name, InputToEvent.inputHitPos + new Vector3(0f, 5f, 0f), Quaternion.identity, 0, null);
	}

	// Token: 0x040001B5 RID: 437
	public GameObject Prefab;

	// Token: 0x040001B6 RID: 438
	public int InstantiateType;

	// Token: 0x040001B7 RID: 439
	public bool showGui;
}

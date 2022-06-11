using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x02000047 RID: 71
public class OnClickRightDestroy : MonoBehaviour
{
	// Token: 0x0600017F RID: 383 RVA: 0x0000AA74 File Offset: 0x00008C74
	public void OnPressRight()
	{
		Debug.Log("RightClick Destroy");
		PhotonNetwork.Destroy(base.gameObject);
	}
}

using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x02000059 RID: 89
public class OnPickedUpScript : MonoBehaviour
{
	// Token: 0x060001DF RID: 479 RVA: 0x0000CFED File Offset: 0x0000B1ED
	public void OnPickedUp(PickupItem item)
	{
		if (item.PickupIsMine)
		{
			Debug.Log("I picked up something. That's a score!");
			return;
		}
		Debug.Log("Someone else picked up something. Lucky!");
	}
}

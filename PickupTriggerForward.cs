using System;
using UnityEngine;

// Token: 0x0200005E RID: 94
public class PickupTriggerForward : MonoBehaviour
{
	// Token: 0x06000200 RID: 512 RVA: 0x0000E404 File Offset: 0x0000C604
	public void OnTriggerEnter(Collider other)
	{
		PickupItem component = base.transform.parent.GetComponent<PickupItem>();
		if (component != null)
		{
			component.OnTriggerEnter(other);
		}
	}
}

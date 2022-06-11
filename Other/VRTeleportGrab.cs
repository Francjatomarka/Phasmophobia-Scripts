using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000186 RID: 390
public class VRTeleportGrab : MonoBehaviour
{
	// Token: 0x06000B16 RID: 2838 RVA: 0x0004631F File Offset: 0x0004451F
	private void Awake()
	{
		
	}

	// Token: 0x06000B17 RID: 2839 RVA: 0x00046339 File Offset: 0x00044539
	private void OnEnable()
	{
		
	}

	// Token: 0x06000B18 RID: 2840 RVA: 0x00046357 File Offset: 0x00044557
	private void OnDisable()
	{
		
	}

	// Token: 0x06000B19 RID: 2841 RVA: 0x00046375 File Offset: 0x00044575
	private void GrabPressed()
	{
		
	}

	// Token: 0x06000B1A RID: 2842 RVA: 0x000463B0 File Offset: 0x000445B0
	private void AttemptGrab()
	{
		for (int i = 0; i < this.raycastPoints.Length; i++)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(this.raycastPoints[i].position, this.raycastPoints[i].forward, out raycastHit, 1.5f, this.mask, QueryTriggerInteraction.Ignore) && raycastHit.collider.GetComponent<Prop>())
			{
				PhotonObjectInteract component = raycastHit.collider.GetComponent<PhotonObjectInteract>();
				if (!component.isGrabbed)
				{
					if (!component.view.IsMine)
					{
						component.view.RequestOwnership();
					}
					component.transform.position = base.transform.position;
				}
			}
		}
	}

	// Token: 0x04000BA0 RID: 2976
	[SerializeField]
	private Transform[] raycastPoints;

	// Token: 0x04000BA1 RID: 2977
	[SerializeField]
	private LayerMask mask;
}

using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000108 RID: 264
[RequireComponent(typeof(PhotonView))]
public class TeleportableObject : MonoBehaviourPunCallbacks
{
	// Token: 0x0600073D RID: 1853 RVA: 0x0002B004 File Offset: 0x00029204
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.body = base.GetComponent<Rigidbody>();
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x0002B02C File Offset: 0x0002922C
	public void Use()
	{
		if (this.photonInteract.isGrabbed)
		{
			return;
		}
		this.body.useGravity = true;
		this.body.isKinematic = false;
		if (this.view.IsMine)
		{
			this.TeleportObject();
			return;
		}
		this.view.RequestOwnership();
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x0002B07E File Offset: 0x0002927E
	public void OnOwnershipRequest()
	{
		this.TeleportObject();
	}

	// Token: 0x06000740 RID: 1856 RVA: 0x0002B088 File Offset: 0x00029288
	private void TeleportObject()
	{
		int num = UnityEngine.Random.Range(0, EvidenceController.instance.roomsToSpawnDNAEvidenceInside.Length);
		int index = UnityEngine.Random.Range(0, EvidenceController.instance.roomsToSpawnDNAEvidenceInside[num].colliders.Count);
		Bounds bounds = EvidenceController.instance.roomsToSpawnDNAEvidenceInside[num].colliders[index].bounds;
		Vector3 position = new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), UnityEngine.Random.Range(bounds.min.y, bounds.max.y), UnityEngine.Random.Range(bounds.min.z, bounds.max.z));
		if (this.specificLocation != null)
		{
			position = this.specificLocation.position;
		}
		this.body.velocity = Vector3.zero;
		base.transform.position = position;
	}

	// Token: 0x04000754 RID: 1876
	[HideInInspector]
	public PhotonView view;

	// Token: 0x04000755 RID: 1877
	private PhotonObjectInteract photonInteract;

	// Token: 0x04000756 RID: 1878
	private Rigidbody body;

	// Token: 0x04000757 RID: 1879
	[SerializeField]
	private Transform specificLocation;
}

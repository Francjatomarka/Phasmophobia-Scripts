using System;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class TeleportableObject : MonoBehaviourPunCallbacks
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.body = base.GetComponent<Rigidbody>();
	}

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

	public void OnOwnershipRequest()
	{
		this.TeleportObject();
	}

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

	[HideInInspector]
	public PhotonView view;

	private PhotonObjectInteract photonInteract;

	private Rigidbody body;

	[SerializeField]
	private Transform specificLocation;
}


using System;
using UnityEngine;
using UnityEngine.Events;

public class Tripod : MonoBehaviour
{
	private void Awake()
	{
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.body = base.GetComponent<Rigidbody>();
	}

	private void Start()
	{
		this.photonInteract.AddUnGrabbedEvent(new UnityAction(this.UnGrab));
		this.photonInteract.AddPCUnGrabbedEvent(new UnityAction(this.UnGrab));
		this.photonInteract.AddGrabbedEvent(new UnityAction(this.Grab));
		this.body.constraints = (RigidbodyConstraints)122;
	}

	public void UnGrab()
	{
		this.body.constraints = (RigidbodyConstraints)122;
		Quaternion rotation = base.transform.rotation;
		Vector3 eulerAngles = rotation.eulerAngles;
		eulerAngles = new Vector3(0f, eulerAngles.y, 0f);
		rotation.eulerAngles = eulerAngles;
		base.transform.rotation = rotation;
	}

	private void Grab()
	{
		this.body.constraints = RigidbodyConstraints.None;
	}

	private PhotonObjectInteract photonInteract;

	private Rigidbody body;

	public Transform snapZone;
}


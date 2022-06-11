using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000127 RID: 295
public class Tripod : MonoBehaviour
{
	// Token: 0x06000844 RID: 2116 RVA: 0x000323D2 File Offset: 0x000305D2
	private void Awake()
	{
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.body = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06000845 RID: 2117 RVA: 0x000323EC File Offset: 0x000305EC
	private void Start()
	{
		this.photonInteract.AddUnGrabbedEvent(new UnityAction(this.UnGrab));
		this.photonInteract.AddPCUnGrabbedEvent(new UnityAction(this.UnGrab));
		this.photonInteract.AddGrabbedEvent(new UnityAction(this.Grab));
		this.body.constraints = (RigidbodyConstraints)122;
	}

	// Token: 0x06000846 RID: 2118 RVA: 0x0003244C File Offset: 0x0003064C
	public void UnGrab()
	{
		this.body.constraints = (RigidbodyConstraints)122;
		Quaternion rotation = base.transform.rotation;
		Vector3 eulerAngles = rotation.eulerAngles;
		eulerAngles = new Vector3(0f, eulerAngles.y, 0f);
		rotation.eulerAngles = eulerAngles;
		base.transform.rotation = rotation;
	}

	// Token: 0x06000847 RID: 2119 RVA: 0x000324A5 File Offset: 0x000306A5
	private void Grab()
	{
		this.body.constraints = RigidbodyConstraints.None;
	}

	// Token: 0x0400084A RID: 2122
	private PhotonObjectInteract photonInteract;

	// Token: 0x0400084B RID: 2123
	private Rigidbody body;

	// Token: 0x0400084C RID: 2124
	public Transform snapZone;
}

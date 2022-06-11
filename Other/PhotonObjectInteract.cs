using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200015F RID: 351
[RequireComponent(typeof(PhotonView))]
public class PhotonObjectInteract : MonoBehaviour
{

	// Token: 0x060009E1 RID: 2529 RVA: 0x0003C0AC File Offset: 0x0003A2AC
	[PunRPC]
	private void NetworkedGrab(int viewID)
	{
		this.isGrabbed = true;
		if (!this.isProp)
		{
			return;
		}
		if (PhotonView.Find(viewID) == null)
		{
			return;
		}
		if (PhotonView.Find(viewID).IsMine)
		{
			if (!this.isFixedItem)
			{
				base.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
				base.GetComponent<Rigidbody>().isKinematic = false;
			}
		}
		else
		{
			base.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
			base.GetComponent<Rigidbody>().isKinematic = true;
		}
		if (PhotonView.Find(viewID) != null)
		{
			base.transform.SetParent(PhotonView.Find(viewID).transform);
		}
	}

	// Token: 0x060009E2 RID: 2530 RVA: 0x0003C144 File Offset: 0x0003A344
	[PunRPC]
	private void NetworkedUnGrab()
	{
		this.isGrabbed = false;
		if (!this.isProp)
		{
			if (PhotonNetwork.IsMasterClient && !this.view.IsMine && base.GetComponent<Door>())
			{
				this.view.RequestOwnership();
			}
			return;
		}
		if (this.view.IsMine && !this.isFixedItem)
		{
			base.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
			base.GetComponent<Rigidbody>().isKinematic = false;
		}
		if (!this.isFixedItem)
		{
			base.transform.SetParent(null);
		}
	}

	// Token: 0x060009E3 RID: 2531 RVA: 0x0003C1CE File Offset: 0x0003A3CE
	private IEnumerator OwnershipDelay()
	{
		yield return new WaitUntil(() => this.view.IsMine);
		this.transformView.m_SynchronizeRotation = true;
		this.transformView.m_SynchronizePosition = true;
		yield break;
	}

	// Token: 0x060009E4 RID: 2532 RVA: 0x0003C1E0 File Offset: 0x0003A3E0
	private void Awake()
	{
		this.holdButtonToGrab = true;
		this.stayGrabbedOnTeleport = true;
		if (this.view == null)
		{
			this.view = base.GetComponent<PhotonView>();
		}
		if (base.GetComponent<PhotonTransformView>())
		{
			this.transformView = base.GetComponent<PhotonTransformView>();
		}
		this.spawnPoint = base.transform.position;
		this.wasGravity = base.GetComponent<Rigidbody>().useGravity;
		this.wasKinematic = base.GetComponent<Rigidbody>().isKinematic;
		if (this.isDraw)
		{
			this.drawer = base.GetComponent<Drawer>();
		}
		if ((base.gameObject.CompareTag("Item") || base.gameObject.CompareTag("DSLR")) && !this.isFixedItem)
		{
			base.transform.SetParent(null);
		}
	}


	// Token: 0x060009E6 RID: 2534 RVA: 0x0003C2D0 File Offset: 0x0003A4D0
	private void Update()
	{
		
	}

	// Token: 0x060009E7 RID: 2535 RVA: 0x0003C4D8 File Offset: 0x0003A6D8
	void OnEnable()
	{
		if (this.isGrabbed)
		{
			return;
		}
		if (base.transform.root.CompareTag("Player") && this.isProp)
		{
			return;
		}
		if (this.drawer != null)
		{
			this.drawer.enabled = true;
		}
	}

	// Token: 0x060009E8 RID: 2536 RVA: 0x0003C530 File Offset: 0x0003A730
	public void ActivateHands()
	{
		if (this.RightHandModel != null)
		{
			this.RightHandModel.SetActive(true);
			return;
		}
		else if (this.leftHandModel != null)
		{
			this.leftHandModel.SetActive(true);
		}
	}

	// Token: 0x060009E9 RID: 2537 RVA: 0x0003C580 File Offset: 0x0003A780
	void OnDisable()
	{
		if (this.isGrabbed)
		{
			return;
		}
		if (base.transform.root.CompareTag("Player") && this.isProp)
		{
			return;
		}
		if (this.drawer != null)
		{
			this.drawer.enabled = false;
		}
	}

	// Token: 0x060009EA RID: 2538 RVA: 0x0003C5D6 File Offset: 0x0003A7D6
	public void AddUseEvent(UnityAction action)
	{
		this.OnUse.AddListener(action);
	}

	// Token: 0x060009EB RID: 2539 RVA: 0x0003C5E4 File Offset: 0x0003A7E4
	public void AddStopEvent(UnityAction action)
	{
		this.OnStopUse.AddListener(action);
	}

	// Token: 0x060009EC RID: 2540 RVA: 0x0003C5F2 File Offset: 0x0003A7F2
	public void AddGrabbedEvent(UnityAction action)
	{
		this.OnGrabbed.AddListener(action);
	}

	// Token: 0x060009ED RID: 2541 RVA: 0x0003C600 File Offset: 0x0003A800
	public void AddUnGrabbedEvent(UnityAction action)
	{
		this.OnUnGrabbed.AddListener(action);
	}

	// Token: 0x060009EE RID: 2542 RVA: 0x0003C60E File Offset: 0x0003A80E
	public void AddPCGrabbedEvent(UnityAction action)
	{
		this.OnPCGrabbed.AddListener(action);
	}

	// Token: 0x060009EF RID: 2543 RVA: 0x0003C61C File Offset: 0x0003A81C
	public void AddPCUnGrabbedEvent(UnityAction action)
	{
		this.OnPCUnGrabbed.AddListener(action);
	}

	// Token: 0x060009F0 RID: 2544 RVA: 0x0003C62A File Offset: 0x0003A82A
	public void AddPCStopUseEvent(UnityAction action)
	{
		this.OnPCStopUse.AddListener(action);
	}

	// Token: 0x060009F1 RID: 2545 RVA: 0x0003C638 File Offset: 0x0003A838
	public void AddPCSecondaryUseEvent(UnityAction action)
	{
		this.OnPCSecondaryUse.AddListener(action);
	}

	// Token: 0x060009F2 RID: 2546 RVA: 0x0003C646 File Offset: 0x0003A846
	private void OnDestroy()
	{
		this.OnUse.RemoveAllListeners();
		this.OnStopUse.RemoveAllListeners();
		this.OnGrabbed.RemoveAllListeners();
		this.OnUnGrabbed.RemoveAllListeners();
	}

	// Token: 0x04000A04 RID: 2564
	public PhotonView view;

	// Token: 0x04000A05 RID: 2565
	[HideInInspector]
	public PhotonTransformView transformView;

	// Token: 0x04000A06 RID: 2566
	public bool isDraw;

	// Token: 0x04000A07 RID: 2567
	public bool isProp;

	// Token: 0x04000A08 RID: 2568
	private Drawer drawer;

	// Token: 0x04000A09 RID: 2569
	[HideInInspector]
	public UnityEvent OnUse = new UnityEvent();

	// Token: 0x04000A0A RID: 2570
	private UnityEvent OnStopUse = new UnityEvent();

	// Token: 0x04000A0B RID: 2571
	private UnityEvent OnGrabbed = new UnityEvent();

	// Token: 0x04000A0C RID: 2572
	private UnityEvent OnUnGrabbed = new UnityEvent();

	// Token: 0x04000A0D RID: 2573
	[HideInInspector]
	public UnityEvent OnPCGrabbed = new UnityEvent();

	// Token: 0x04000A0E RID: 2574
	[HideInInspector]
	public UnityEvent OnPCUnGrabbed = new UnityEvent();

	// Token: 0x04000A0F RID: 2575
	[HideInInspector]
	public UnityEvent OnPCStopUse = new UnityEvent();

	// Token: 0x04000A10 RID: 2576
	[HideInInspector]
	public UnityEvent OnPCSecondaryUse = new UnityEvent();

	// Token: 0x04000A11 RID: 2577
	public bool isGrabbed;

	// Token: 0x04000A12 RID: 2578
	private GameObject leftHandModel;

	// Token: 0x04000A13 RID: 2579
	private GameObject RightHandModel;

	// Token: 0x04000A14 RID: 2580
	public GameObject myLeftHandModel;

	// Token: 0x04000A15 RID: 2581
	public GameObject myRightHandModel;

	// Token: 0x04000A16 RID: 2582
	[HideInInspector]
	public Vector3 spawnPoint;

	// Token: 0x04000A17 RID: 2583
	public Vector3 localPlayerRotation;

	// Token: 0x04000A18 RID: 2584
	public Vector3 localPlayerPosition;

	// Token: 0x04000A19 RID: 2585
	[HideInInspector]
	public bool wasGravity;

	// Token: 0x04000A1A RID: 2586
	[HideInInspector]
	public bool wasKinematic;

	// Token: 0x04000A1B RID: 2587
	public bool isFixedItem;

	// Token: 0x04000A1D RID: 2589
	private bool hasUsed;
    private bool holdButtonToGrab;
    private bool stayGrabbedOnTeleport;
}

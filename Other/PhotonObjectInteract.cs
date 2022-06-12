using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PhotonView))]
public class PhotonObjectInteract : MonoBehaviour
{

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

	private IEnumerator OwnershipDelay()
	{
		yield return new WaitUntil(() => this.view.IsMine);
		this.transformView.m_SynchronizeRotation = true;
		this.transformView.m_SynchronizePosition = true;
		yield break;
	}

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


	private void Update()
	{
		
	}

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

	public void AddUseEvent(UnityAction action)
	{
		this.OnUse.AddListener(action);
	}

	public void AddStopEvent(UnityAction action)
	{
		this.OnStopUse.AddListener(action);
	}

	public void AddGrabbedEvent(UnityAction action)
	{
		this.OnGrabbed.AddListener(action);
	}

	public void AddUnGrabbedEvent(UnityAction action)
	{
		this.OnUnGrabbed.AddListener(action);
	}

	public void AddPCGrabbedEvent(UnityAction action)
	{
		this.OnPCGrabbed.AddListener(action);
	}

	public void AddPCUnGrabbedEvent(UnityAction action)
	{
		this.OnPCUnGrabbed.AddListener(action);
	}

	public void AddPCStopUseEvent(UnityAction action)
	{
		this.OnPCStopUse.AddListener(action);
	}

	public void AddPCSecondaryUseEvent(UnityAction action)
	{
		this.OnPCSecondaryUse.AddListener(action);
	}

	private void OnDestroy()
	{
		this.OnUse.RemoveAllListeners();
		this.OnStopUse.RemoveAllListeners();
		this.OnGrabbed.RemoveAllListeners();
		this.OnUnGrabbed.RemoveAllListeners();
	}

	public PhotonView view;

	[HideInInspector]
	public PhotonTransformView transformView;

	public bool isDraw;

	public bool isProp;

	private Drawer drawer;

	[HideInInspector]
	public UnityEvent OnUse = new UnityEvent();

	private UnityEvent OnStopUse = new UnityEvent();

	private UnityEvent OnGrabbed = new UnityEvent();

	private UnityEvent OnUnGrabbed = new UnityEvent();

	[HideInInspector]
	public UnityEvent OnPCGrabbed = new UnityEvent();

	[HideInInspector]
	public UnityEvent OnPCUnGrabbed = new UnityEvent();

	[HideInInspector]
	public UnityEvent OnPCStopUse = new UnityEvent();

	[HideInInspector]
	public UnityEvent OnPCSecondaryUse = new UnityEvent();

	public bool isGrabbed;

	private GameObject leftHandModel;

	private GameObject RightHandModel;

	public GameObject myLeftHandModel;

	public GameObject myRightHandModel;

	[HideInInspector]
	public Vector3 spawnPoint;

	public Vector3 localPlayerRotation;

	public Vector3 localPlayerPosition;

	[HideInInspector]
	public bool wasGravity;

	[HideInInspector]
	public bool wasKinematic;

	public bool isFixedItem;

	private bool hasUsed;
    private bool holdButtonToGrab;
    private bool stayGrabbedOnTeleport;
}


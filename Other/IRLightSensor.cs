using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Photon.Pun;

public class IRLightSensor : MonoBehaviour
{
	private void Awake()
	{
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.rigidbdy = base.GetComponent<Rigidbody>();
		this.view = base.GetComponent<PhotonView>();
	}

	private void Start()
	{
		if (XRDevice.isPresent)
		{
			this.photonInteract.AddUseEvent(new UnityAction(this.MotionUse));
			this.photonInteract.AddGrabbedEvent(new UnityAction(this.OnGrabbed));
		}
		else
		{
			this.photonInteract.AddPCSecondaryUseEvent(new UnityAction(this.SecondaryUse));
			this.photonInteract.AddPCGrabbedEvent(new UnityAction(this.OnGrabbed));
		}
		this.rend.material.DisableKeyword("_EMISSION");
	}

	private void Update()
	{
		if (!this.isPlaced && this.view.IsMine)
		{
			if (this.photonInteract.isGrabbed)
			{
				if (!XRDevice.isPresent)
				{
					Camera playerCam = GameObject.Find("PCPlayerHead").GetComponent<Camera>();
					RaycastHit raycastHit;
					if (Physics.Raycast(playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out raycastHit, this.grabDistance, this.mask))
					{
						if (!this.helperObject.activeInHierarchy)
						{
							this.helperObject.SetActive(true);
						}
						this.helperObject.transform.position = raycastHit.point;
						Quaternion rotation = this.helperObject.transform.rotation;
						rotation.SetLookRotation(raycastHit.normal);
						rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, Mathf.Round(rotation.eulerAngles.y / 45f) * 45f, rotation.eulerAngles.z);
						this.helperObject.transform.rotation = rotation;
						return;
					}
					if (this.helperObject.activeInHierarchy)
					{
						this.helperObject.SetActive(false);
						return;
					}
				}
				else
				{
					RaycastHit raycastHit;
					if (Physics.Raycast(base.transform.position, base.transform.TransformDirection(Vector3.back), out raycastHit, 0.5f, this.mask))
					{
						if (!this.helperObject.activeInHierarchy)
						{
							this.helperObject.SetActive(true);
						}
						this.helperObject.transform.position = raycastHit.point;
						Quaternion rotation2 = this.helperObject.transform.rotation;
						rotation2.SetLookRotation(raycastHit.normal);
						rotation2.eulerAngles = new Vector3(rotation2.eulerAngles.x, Mathf.Round(rotation2.eulerAngles.y / 45f) * 45f, rotation2.eulerAngles.z);
						this.helperObject.transform.rotation = rotation2;
						return;
					}
					if (this.helperObject.activeInHierarchy)
					{
						this.helperObject.SetActive(false);
						return;
					}
				}
			}
			else if (this.helperObject.activeInHierarchy)
			{
				this.helperObject.SetActive(false);
			}
		}
	}

	private void SecondaryUse()
	{
		Camera playerCam = GameObject.Find("PCPlayerHead").GetComponent<Camera>();
		RaycastHit raycastHit;
		if (Physics.Raycast(playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out raycastHit, this.grabDistance, this.mask))
		{
			PCPropGrab player = GameObject.Find("PCPlayerHead").GetComponent<PCPropGrab>();
			player.Drop(false);
            if (PhotonNetwork.InRoom)
            {
				this.view.RPC("PlaceOrPickupSensor", RpcTarget.All, new object[]
				{
					true,
					raycastHit.point,
					raycastHit.normal,
					LevelController.instance.currentPlayerRoom.roomName
				});
				return;
			}
			PlaceOrPickupSensor(true, raycastHit.point, raycastHit.normal, "Menu_New");
		}
	}

	public void Detection()
	{
		if (!this.detected)
		{
            if (PhotonNetwork.InRoom)
            {
				this.view.RPC("DetectionNetworked", RpcTarget.All, Array.Empty<object>());
				return;
			}
			DetectionNetworked();
		}
	}

	[PunRPC]
	private void DetectionNetworked()
	{
		this.detected = true;
		this.rend.material.EnableKeyword("_EMISSION");
		this.myLight.enabled = true;
		base.StartCoroutine(this.StopDetection());
	}

	private IEnumerator StopDetection()
	{
		yield return new WaitForSeconds(2f);
		this.myLight.enabled = false;
		this.detected = false;
		this.rend.material.DisableKeyword("_EMISSION");
		yield break;
	}

	private void MotionUse()
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position, base.transform.TransformDirection(Vector3.back), out raycastHit, 0.5f, this.mask))
		{
            if (PhotonNetwork.InRoom)
            {
				this.view.RPC("PlaceOrPickupSensor", RpcTarget.All, new object[]
				{
					true,
					raycastHit.point,
					raycastHit.normal,
					LevelController.instance.currentPlayerRoom.roomName
				});
				return;
			}
			PlaceOrPickupSensor(true, raycastHit.point, raycastHit.normal, "Menu_New");
		}
	}

	private void OnGrabbed()
	{
		if (this.isPlaced)
		{
			if (PhotonNetwork.InRoom)
			{
				this.view.RPC("PlaceOrPickupSensor", RpcTarget.All, new object[]
				{
					false,
					Vector3.zero,
					Vector3.zero,
					LevelController.instance.currentPlayerRoom.roomName
				});
				return;
			}
			PlaceOrPickupSensor(false, Vector3.zero, Vector3.zero, "Menu_New");
		}
	}

	[PunRPC]
	private void PlaceOrPickupSensor(bool isBeingPlaced, Vector3 position, Vector3 normal, string _roomName)
	{
		if (this.isPlaced)
		{
			base.GetComponent<PhotonTransformView>().m_SynchronizePosition = false;
			base.GetComponent<PhotonTransformView>().m_SynchronizeRotation = false;
		}
		this.isPlaced = isBeingPlaced;
		this.roomName = _roomName;
		if (this.isPlaced)
		{
			base.StartCoroutine(this.Place(position, normal));
		}
	}

	private IEnumerator Place(Vector3 position, Vector3 normal)
	{
		base.transform.SetParent(null);
		yield return new WaitForSeconds(0.1f);
		this.rigidbdy.isKinematic = true;
		base.transform.position = position;
		Quaternion rotation = base.transform.rotation;
		rotation.SetLookRotation(normal);
		Vector3 eulerAngles = rotation.eulerAngles;
		eulerAngles.x = 0f;
		eulerAngles.y = Mathf.Round(eulerAngles.y / 45f) * 45f;
		eulerAngles.z = 0f;
		rotation.eulerAngles = eulerAngles;
		base.transform.rotation = rotation;
		this.helperObject.SetActive(false);
		yield break;
	}

	[HideInInspector]
	public bool isPlaced;

	private PhotonObjectInteract photonInteract;

	private Rigidbody rigidbdy;

	private PhotonView view;

	[SerializeField]
	private Renderer rend;

	[HideInInspector]
	public string roomName;

	[SerializeField]
	private LayerMask mask;

	[SerializeField]
	private GameObject helperObject;

	[SerializeField]
	private Light myLight;

	[HideInInspector]
	public int id;

	private bool detected;

	[Header("PC")]
	private float grabDistance = 1f;

	private Ray playerAim;
}


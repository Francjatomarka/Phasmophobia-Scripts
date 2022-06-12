using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR;
using Photon.Pun;

public class MotionSensor : MonoBehaviour
{
	private void Awake()
	{
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.rigidbdy = base.GetComponent<Rigidbody>();
		this.view = base.GetComponent<PhotonView>();
		this.rend = base.GetComponent<Renderer>();
		this.noise = base.GetComponentInChildren<Noise>();
		this.noise.gameObject.SetActive(false);
		this.iconStartLocalPosition = this.mapIcon.localPosition;
		this.iconStartLocalRotation = this.mapIcon.localRotation;
		this.iconStartLocalScale = this.mapIcon.localScale;
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
		this.rend.material.SetTexture("_EmissionMap", this.redTexture);
	}

	private void Update()
	{
		if (this.isPlaced)
		{
			if (this.detected)
			{
				this.timer -= Time.deltaTime;
				if (this.timer < 0f)
				{
					this.detected = false;
					this.rend.material.SetTexture("_EmissionMap", this.redTexture);
					return;
				}
			}
		}
		else if (this.view.IsMine)
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
			PCPropGrab pcPropGrab = GameObject.Find("PCPlayerHead").GetComponent<PCPropGrab>();
			if (PhotonNetwork.InRoom)
            {
				pcPropGrab.Drop(false);
				this.view.RPC("PlaceOrPickupSensor", RpcTarget.All, new object[]
				{
					true,
					raycastHit.point,
					raycastHit.normal,
					LevelController.instance.currentPlayerRoom.roomName
				});
				return;
			}
			pcPropGrab.Drop(false);
			PlaceOrPickupSensor(true, raycastHit.point, raycastHit.normal, "Menu_New");
		}
	}

	public void Detection(bool isGhost)
	{
        if (PhotonNetwork.InRoom)
        {
			this.view.RPC("DetectionNetworked", RpcTarget.All, new object[]
			{
				isGhost
			});
			return;
		}
		DetectionNetworked(isGhost);
	}

	[PunRPC]
	private void DetectionNetworked(bool isGhost)
	{
		this.detected = true;
		this.timer = 2f;
		this.rend.material.SetTexture("_EmissionMap", this.greenTexture);
		if (isGhost && MissionMotionSensor.instance != null && !MissionMotionSensor.instance.completed)
		{
			MissionMotionSensor.instance.CompleteMission();
		}
		if(MotionSensorData.instance != null)
        {
			MotionSensorData.instance.Detected(this);
		}
		base.StartCoroutine(this.PlayNoiseObject());
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
			return;
		}
		this.mapIcon.SetParent(base.transform);
		this.mapIcon.localPosition = this.iconStartLocalPosition;
		this.mapIcon.localRotation = this.iconStartLocalRotation;
		this.mapIcon.localScale = this.iconStartLocalScale;
		if (MotionSensorData.instance != null)
		{
			MotionSensorData.instance.RemoveText(this);
		}
		this.mapIcon.gameObject.SetActive(false);
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
		if (this.view.IsMine)
		{
			base.StartCoroutine(this.MapIconDelay());
		}
		if(MotionSensorData.instance != null)
        {
			MotionSensorData.instance.SetText(this);
		}
		this.helperObject.SetActive(false);
		yield break;
	}

	private IEnumerator MapIconDelay()
	{
		yield return new WaitForSeconds(3f);
		if (MapController.instance)
		{
            if (PhotonNetwork.InRoom)
            {
				this.view.RPC("AssignSensorOnMap", RpcTarget.All, new object[]
				{
					(int)LevelController.instance.currentPlayerRoom.floorType
				});
				yield return null;
			}
			AssignSensorOnMap((int)LevelController.instance.currentPlayerRoom.floorType);
		}
		yield break;
	}

	[PunRPC]
	private void AssignSensorOnMap(int floorID)
	{
		this.mapIcon.gameObject.SetActive(true);
		if(MapController.instance != null)
        {
			MapController.instance.AssignSensor(base.transform, this.mapIcon, floorID, this);
		}
	}

	private IEnumerator PlayNoiseObject()
	{
		this.noise.gameObject.SetActive(true);
		yield return 0;
		this.noise.gameObject.SetActive(false);
		yield break;
	}

	[HideInInspector]
	public bool isPlaced;

	private PhotonObjectInteract photonInteract;

	private Rigidbody rigidbdy;

	private PhotonView view;

	private Renderer rend;

	private Noise noise;

	[HideInInspector]
	public string roomName;

	[SerializeField]
	private Texture greenTexture;

	[SerializeField]
	private Texture redTexture;

	[SerializeField]
	private LayerMask mask;

	[SerializeField]
	private GameObject helperObject;

	[HideInInspector]
	public int id;

	private float timer = 2f;

	private bool detected;

	[SerializeField]
	private Transform mapIcon;

	public Image sensorIcon;

	private Quaternion iconStartLocalRotation;

	private Vector3 iconStartLocalScale;

	private Vector3 iconStartLocalPosition;

	[Header("PC")]
	private float grabDistance = 1f;

	private Ray playerAim;
}


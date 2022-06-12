using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Photon.Pun;

public class SoundSensor : MonoBehaviour
{
	private void Awake()
	{
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.rigidbdy = base.GetComponent<Rigidbody>();
		this.view = base.GetComponent<PhotonView>();
		this.rend = base.GetComponent<Renderer>();
		this.helperObject.transform.SetParent(null);
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
		this.rend.material.SetColor("_EmissionColor", Color.red);
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
		this.rend.material.SetColor("_EmissionColor", this.isPlaced ? Color.green : Color.red);
		if (this.isPlaced)
		{
			base.StartCoroutine(this.Place(position, normal));
			return;
		}
		this.mapIcon.SetParent(base.transform);
		this.mapIcon.localPosition = this.iconStartLocalPosition;
		this.mapIcon.localRotation = this.iconStartLocalRotation;
		this.mapIcon.localScale = this.iconStartLocalScale;
		if(SoundSensorData.instance != null)
        {
			SoundSensorData.instance.RemoveText(this);
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
		if (SoundSensorData.instance != null)
		{
			SoundSensorData.instance.SetText(this);
		}
		this.helperObject.SetActive(false);
		yield break;
	}

	private IEnumerator MapIconDelay()
	{
		yield return new WaitForSeconds(3f);
		if (MapController.instance && PhotonNetwork.InRoom)
		{
			this.view.RPC("AssignSoundSensorToMap", RpcTarget.All, new object[]
			{
				(int)LevelController.instance.currentPlayerRoom.floorType
			});
		}
		yield break;
	}

	[PunRPC]
	private void AssignSoundSensorToMap(int floorID)
	{
		this.mapIcon.gameObject.SetActive(true);
		MapController.instance.AssignSensor(base.transform, this.mapIcon, floorID, null);
	}

	private void Update()
	{
		if (this.isPlaced)
		{
			this.checkTimer -= Time.deltaTime;
			if (this.checkTimer < 0f)
			{
				if(SoundSensorData.instance != null)
                {
					SoundSensorData.instance.UpdateSensorValue(this.id, this.highestVolume);
				}
				this.highestVolume = 0f;
				base.StartCoroutine(this.ResetTrigger());
				this.checkTimer = 5f;
				return;
			}
		}
		else if (this.view.IsMine)
		{
			if (this.photonInteract.isGrabbed)
			{
				if (!XRDevice.isPresent)
				{
					Camera playerCamera = GameObject.Find("PCPlayerHead").GetComponent<Camera>();
					RaycastHit raycastHit;
					if (Physics.Raycast(playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out raycastHit, this.grabDistance, this.mask))
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
		Camera playerCamera = GameObject.Find("PCPlayerHead").GetComponent<Camera>();
		this.playerAim = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		RaycastHit raycastHit;
		if (Physics.Raycast(this.playerAim, out raycastHit, this.grabDistance, this.mask))
		{
			PCPropGrab pcPropGrab = GameObject.Find("PCPlayerHead").GetComponent<PCPropGrab>();
			pcPropGrab.Drop(false);
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

	private IEnumerator ResetTrigger()
	{
		this.col.enabled = false;
		yield return 0;
		this.col.enabled = true;
		yield break;
	}

	private void OnDisable()
	{
		if (this.helperObject)
		{
			this.helperObject.SetActive(false);
		}
	}

	[HideInInspector]
	public bool isPlaced;

	[HideInInspector]
	public int id;

	[HideInInspector]
	public string roomName;

	private PhotonObjectInteract photonInteract;

	private Rigidbody rigidbdy;

	private PhotonView view;

	private Renderer rend;

	private float checkTimer = 5f;

	public float highestVolume;

	[SerializeField]
	private BoxCollider col;

	[SerializeField]
	private GameObject helperObject;

	[SerializeField]
	private Transform mapIcon;

	private Quaternion iconStartLocalRotation;

	private Vector3 iconStartLocalScale;

	private Vector3 iconStartLocalPosition;

	[Header("PC")]
	private float grabDistance = 1f;

	private Ray playerAim;

	[SerializeField]
	private LayerMask mask;
}


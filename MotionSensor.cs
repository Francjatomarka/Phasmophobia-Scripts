using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR;
using Photon.Pun;

// Token: 0x02000118 RID: 280
public class MotionSensor : MonoBehaviour
{
	// Token: 0x060007CF RID: 1999 RVA: 0x0002EB7C File Offset: 0x0002CD7C
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

	// Token: 0x060007D0 RID: 2000 RVA: 0x0002EC0C File Offset: 0x0002CE0C
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

	// Token: 0x060007D1 RID: 2001 RVA: 0x0002EC9C File Offset: 0x0002CE9C
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

	// Token: 0x060007D2 RID: 2002 RVA: 0x0002EF40 File Offset: 0x0002D140
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

	// Token: 0x060007D3 RID: 2003 RVA: 0x0002EFFE File Offset: 0x0002D1FE
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

	// Token: 0x060007D4 RID: 2004 RVA: 0x0002F020 File Offset: 0x0002D220
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

	// Token: 0x060007D5 RID: 2005 RVA: 0x0002F098 File Offset: 0x0002D298
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

	// Token: 0x060007D6 RID: 2006 RVA: 0x0002F12C File Offset: 0x0002D32C
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

	// Token: 0x060007D7 RID: 2007 RVA: 0x0002F190 File Offset: 0x0002D390
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

	// Token: 0x060007D8 RID: 2008 RVA: 0x0002F261 File Offset: 0x0002D461
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

	// Token: 0x060007D9 RID: 2009 RVA: 0x0002F27E File Offset: 0x0002D47E
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

	// Token: 0x060007DA RID: 2010 RVA: 0x0002F28D File Offset: 0x0002D48D
	[PunRPC]
	private void AssignSensorOnMap(int floorID)
	{
		this.mapIcon.gameObject.SetActive(true);
		if(MapController.instance != null)
        {
			MapController.instance.AssignSensor(base.transform, this.mapIcon, floorID, this);
		}
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x0002F2B8 File Offset: 0x0002D4B8
	private IEnumerator PlayNoiseObject()
	{
		this.noise.gameObject.SetActive(true);
		yield return 0;
		this.noise.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x040007D8 RID: 2008
	[HideInInspector]
	public bool isPlaced;

	// Token: 0x040007D9 RID: 2009
	private PhotonObjectInteract photonInteract;

	// Token: 0x040007DA RID: 2010
	private Rigidbody rigidbdy;

	// Token: 0x040007DB RID: 2011
	private PhotonView view;

	// Token: 0x040007DC RID: 2012
	private Renderer rend;

	// Token: 0x040007DD RID: 2013
	private Noise noise;

	// Token: 0x040007DE RID: 2014
	[HideInInspector]
	public string roomName;

	// Token: 0x040007DF RID: 2015
	[SerializeField]
	private Texture greenTexture;

	// Token: 0x040007E0 RID: 2016
	[SerializeField]
	private Texture redTexture;

	// Token: 0x040007E1 RID: 2017
	[SerializeField]
	private LayerMask mask;

	// Token: 0x040007E2 RID: 2018
	[SerializeField]
	private GameObject helperObject;

	// Token: 0x040007E3 RID: 2019
	[HideInInspector]
	public int id;

	// Token: 0x040007E4 RID: 2020
	private float timer = 2f;

	// Token: 0x040007E5 RID: 2021
	private bool detected;

	// Token: 0x040007E6 RID: 2022
	[SerializeField]
	private Transform mapIcon;

	// Token: 0x040007E7 RID: 2023
	public Image sensorIcon;

	// Token: 0x040007E8 RID: 2024
	private Quaternion iconStartLocalRotation;

	// Token: 0x040007E9 RID: 2025
	private Vector3 iconStartLocalScale;

	// Token: 0x040007EA RID: 2026
	private Vector3 iconStartLocalPosition;

	// Token: 0x040007EB RID: 2027
	[Header("PC")]
	private float grabDistance = 1f;

	// Token: 0x040007EC RID: 2028
	private Ray playerAim;
}

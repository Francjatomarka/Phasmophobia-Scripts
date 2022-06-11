using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Photon.Pun;

// Token: 0x02000121 RID: 289
public class SoundSensor : MonoBehaviour
{
	// Token: 0x0600081C RID: 2076 RVA: 0x0003143C File Offset: 0x0002F63C
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

	// Token: 0x0600081D RID: 2077 RVA: 0x000314C0 File Offset: 0x0002F6C0
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

	// Token: 0x0600081E RID: 2078 RVA: 0x0003154C File Offset: 0x0002F74C
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

	// Token: 0x0600081F RID: 2079 RVA: 0x000315B0 File Offset: 0x0002F7B0
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

	// Token: 0x06000820 RID: 2080 RVA: 0x00031644 File Offset: 0x0002F844
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

	// Token: 0x06000821 RID: 2081 RVA: 0x0003173E File Offset: 0x0002F93E
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

	// Token: 0x06000822 RID: 2082 RVA: 0x0003175B File Offset: 0x0002F95B
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

	// Token: 0x06000823 RID: 2083 RVA: 0x0003176A File Offset: 0x0002F96A
	[PunRPC]
	private void AssignSoundSensorToMap(int floorID)
	{
		this.mapIcon.gameObject.SetActive(true);
		MapController.instance.AssignSensor(base.transform, this.mapIcon, floorID, null);
	}

	// Token: 0x06000824 RID: 2084 RVA: 0x00031798 File Offset: 0x0002F998
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

	// Token: 0x06000825 RID: 2085 RVA: 0x00031A48 File Offset: 0x0002FC48
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

	// Token: 0x06000826 RID: 2086 RVA: 0x00031B12 File Offset: 0x0002FD12
	private IEnumerator ResetTrigger()
	{
		this.col.enabled = false;
		yield return 0;
		this.col.enabled = true;
		yield break;
	}

	// Token: 0x06000827 RID: 2087 RVA: 0x00031B21 File Offset: 0x0002FD21
	private void OnDisable()
	{
		if (this.helperObject)
		{
			this.helperObject.SetActive(false);
		}
	}

	// Token: 0x0400081A RID: 2074
	[HideInInspector]
	public bool isPlaced;

	// Token: 0x0400081B RID: 2075
	[HideInInspector]
	public int id;

	// Token: 0x0400081C RID: 2076
	[HideInInspector]
	public string roomName;

	// Token: 0x0400081D RID: 2077
	private PhotonObjectInteract photonInteract;

	// Token: 0x0400081E RID: 2078
	private Rigidbody rigidbdy;

	// Token: 0x0400081F RID: 2079
	private PhotonView view;

	// Token: 0x04000820 RID: 2080
	private Renderer rend;

	// Token: 0x04000821 RID: 2081
	private float checkTimer = 5f;

	// Token: 0x04000822 RID: 2082
	public float highestVolume;

	// Token: 0x04000823 RID: 2083
	[SerializeField]
	private BoxCollider col;

	// Token: 0x04000824 RID: 2084
	[SerializeField]
	private GameObject helperObject;

	// Token: 0x04000825 RID: 2085
	[SerializeField]
	private Transform mapIcon;

	// Token: 0x04000826 RID: 2086
	private Quaternion iconStartLocalRotation;

	// Token: 0x04000827 RID: 2087
	private Vector3 iconStartLocalScale;

	// Token: 0x04000828 RID: 2088
	private Vector3 iconStartLocalPosition;

	// Token: 0x04000829 RID: 2089
	[Header("PC")]
	private float grabDistance = 1f;

	// Token: 0x0400082A RID: 2090
	private Ray playerAim;

	// Token: 0x0400082B RID: 2091
	[SerializeField]
	private LayerMask mask;
}

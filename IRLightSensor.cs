using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Photon.Pun;

// Token: 0x02000114 RID: 276
public class IRLightSensor : MonoBehaviour
{
	// Token: 0x060007B3 RID: 1971 RVA: 0x0002E13B File Offset: 0x0002C33B
	private void Awake()
	{
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.rigidbdy = base.GetComponent<Rigidbody>();
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x060007B4 RID: 1972 RVA: 0x0002E164 File Offset: 0x0002C364
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

	// Token: 0x060007B5 RID: 1973 RVA: 0x0002E1EC File Offset: 0x0002C3EC
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

	// Token: 0x060007B6 RID: 1974 RVA: 0x0002E444 File Offset: 0x0002C644
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

	// Token: 0x060007B7 RID: 1975 RVA: 0x0002E502 File Offset: 0x0002C702
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

	// Token: 0x060007B8 RID: 1976 RVA: 0x0002E522 File Offset: 0x0002C722
	[PunRPC]
	private void DetectionNetworked()
	{
		this.detected = true;
		this.rend.material.EnableKeyword("_EMISSION");
		this.myLight.enabled = true;
		base.StartCoroutine(this.StopDetection());
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x0002E559 File Offset: 0x0002C759
	private IEnumerator StopDetection()
	{
		yield return new WaitForSeconds(2f);
		this.myLight.enabled = false;
		this.detected = false;
		this.rend.material.DisableKeyword("_EMISSION");
		yield break;
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x0002E568 File Offset: 0x0002C768
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

	// Token: 0x060007BB RID: 1979 RVA: 0x0002E5FC File Offset: 0x0002C7FC
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

	// Token: 0x060007BC RID: 1980 RVA: 0x0002E660 File Offset: 0x0002C860
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

	// Token: 0x060007BD RID: 1981 RVA: 0x0002E6D0 File Offset: 0x0002C8D0
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

	// Token: 0x040007BA RID: 1978
	[HideInInspector]
	public bool isPlaced;

	// Token: 0x040007BB RID: 1979
	private PhotonObjectInteract photonInteract;

	// Token: 0x040007BC RID: 1980
	private Rigidbody rigidbdy;

	// Token: 0x040007BD RID: 1981
	private PhotonView view;

	// Token: 0x040007BE RID: 1982
	[SerializeField]
	private Renderer rend;

	// Token: 0x040007BF RID: 1983
	[HideInInspector]
	public string roomName;

	// Token: 0x040007C0 RID: 1984
	[SerializeField]
	private LayerMask mask;

	// Token: 0x040007C1 RID: 1985
	[SerializeField]
	private GameObject helperObject;

	// Token: 0x040007C2 RID: 1986
	[SerializeField]
	private Light myLight;

	// Token: 0x040007C3 RID: 1987
	[HideInInspector]
	public int id;

	// Token: 0x040007C4 RID: 1988
	private bool detected;

	// Token: 0x040007C5 RID: 1989
	[Header("PC")]
	private float grabDistance = 1f;

	// Token: 0x040007C6 RID: 1990
	private Ray playerAim;
}

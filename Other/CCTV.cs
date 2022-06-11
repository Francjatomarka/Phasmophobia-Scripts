using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;
using Photon.Pun;

// Token: 0x0200010B RID: 267
public class CCTV : MonoBehaviour
{
	// Token: 0x0600074E RID: 1870 RVA: 0x0002B308 File Offset: 0x00029508
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.myLight = this.cam.GetComponentInChildren<Light>();
		this.boxCollider = base.GetComponent<BoxCollider>();
		this.startColSize = this.boxCollider.size;
		if (!this.isFixedCamera)
		{
			this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		}
	}

	// Token: 0x0600074F RID: 1871 RVA: 0x0002B364 File Offset: 0x00029564
	private void Start()
	{
		if (MainManager.instance)
		{
			//base.gameObject.SetActive(false);
			//return;
		}
		this.IsThisCameraSwitchedOn = false;
		this.isThisCameraActiveOnACCTVScreen = false;
		this.cam.enabled = false;
		if (LevelController.instance != null)
        {
			this.nightVision.Power = LevelController.instance.nightVisionPower;
		}
		this.rt = new RenderTexture(600, 500, 16, RenderTextureFormat.ARGB32);
		this.rt.Create();
		this.cam.targetTexture = this.rt;
		if (this.isFixedCamera)
		{
			this.SetupCCTV();
			return;
		}
		this.SetupDSLR();
	}

	// Token: 0x06000750 RID: 1872 RVA: 0x0002B404 File Offset: 0x00029604
	private void SetupCCTV()
	{
		if (MapController.instance)
		{
			MapController.instance.AssignIcon(this.mapIcon.transform, this.floorType);
			return;
		}
		FindObjectOfType<MapController>().AssignIcon(this.mapIcon.transform, this.floorType);
	}

	// Token: 0x06000751 RID: 1873 RVA: 0x0002B454 File Offset: 0x00029654
	private void SetupDSLR()
	{
		if (!this.isHeadCamera)
		{
			this.photonInteract.AddUseEvent(new UnityAction(this.Use));
			this.photonInteract.AddPCSecondaryUseEvent(new UnityAction(this.SecondaryUse));
			this.photonInteract.AddPCSecondaryUseEvent(new UnityAction(this.SecondaryUse));
		}
		this.photonInteract.AddGrabbedEvent(new UnityAction(this.OnGrabbed));
		this.photonInteract.AddPCGrabbedEvent(new UnityAction(this.OnGrabbed));
		if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
		{
			this.TurnOff();
		}
		if (CCTVController.instance)
		{
			CCTVController.instance.allcctvCameras.Add(this);
		}
		else
		{
			//FindObjectOfType<CCTVController>().allcctvCameras.Add(this);
		}
		if (!XRDevice.isPresent && !this.isHeadCamera)
		{
			if (GameController.instance != null && GameController.instance.myPlayer == null)
			{
				GameController.instance.OnLocalPlayerSpawned.AddListener(new UnityAction(this.OnPlayerSpawned));
			}
			else
			{
				this.OnPlayerSpawned();
			}
		}
		if (this.helperObject)
		{
			this.helperObject.SetActive(false);
			this.helperObject.transform.SetParent(null);
		}
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x0002B568 File Offset: 0x00029768
	public void Use()
	{
		if (this.isHeadCamera)
		{
			return;
		}
		if (PhotonNetwork.InRoom)
		{
			this.NetworkedUse();
			return;
		}
		this.NetworkedUse();
	}

	// Token: 0x06000753 RID: 1875 RVA: 0x0002B597 File Offset: 0x00029797
	public void TurnOff()
	{
		if (this.IsThisCameraSwitchedOn)
		{
			if (PhotonNetwork.InRoom)
			{
				this.view.RPC("NetworkedUse", RpcTarget.All, Array.Empty<object>());
				return;
			}
			this.NetworkedUse();
		}
	}

	// Token: 0x06000754 RID: 1876 RVA: 0x0002B5C5 File Offset: 0x000297C5
	public void TurnOn()
	{
		if (!this.IsThisCameraSwitchedOn)
		{
			if (PhotonNetwork.InRoom)
			{
				this.view.RPC("NetworkedUse", RpcTarget.All, Array.Empty<object>());
				return;
			}
			this.NetworkedUse();
		}
	}

	// Token: 0x06000755 RID: 1877 RVA: 0x0002B5F4 File Offset: 0x000297F4
	public void SecondaryUse()
	{
		if (this.isFixedCamera)
		{
			return;
		}
		bool flag = false;
		Camera playerCam = GameObject.Find("PCPlayerHead").GetComponent<Camera>();
		PCPropGrab pcPropGrab = GameObject.Find("PCPlayerHead").GetComponent<PCPropGrab>();
		if (!XRDevice.isPresent && pcPropGrab.inventoryProps[pcPropGrab.inventoryIndex] == this.photonInteract)
		{
			this.playerAim = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
			RaycastHit raycastHit;
			if (Physics.Raycast(this.playerAim, out raycastHit, 1.6f, this.helperMask, QueryTriggerInteraction.Ignore) && raycastHit.collider.GetComponent<Tripod>())
			{
				Debug.Log("Poner en tripode");
				pcPropGrab.Drop(false);
                if (PhotonNetwork.InRoom)
                {
					this.view.RPC("PlaceCamera", RpcTarget.All, new object[]
					{
						raycastHit.collider.GetComponent<Tripod>().GetComponent<PhotonView>().ViewID
					});
					flag = true;
					return;
				}
				PlaceCamera(raycastHit.collider.GetComponent<Tripod>().GetComponent<PhotonView>().ViewID);
				flag = true;
            }
            else
            {
				Physics.Raycast(this.playerAim, out raycastHit, 1.6f, this.helperMask, QueryTriggerInteraction.Ignore);
				Debug.Log("Objeto: " + raycastHit.collider.gameObject.name);
				Debug.Log("No se puede poner el tripode");
            }
			RaycastHit raycastHit2;
			if (!flag && Physics.Raycast(this.playerAim, out raycastHit2, 1.6f, this.helperMask, QueryTriggerInteraction.Ignore))
			{
				if (raycastHit2.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
				{
					return;
				}

				if (PhotonNetwork.InRoom)
				{
					pcPropGrab.Drop(true);
					this.view.RPC("NonVRPlaceCamera", RpcTarget.All, new object[]
					{
						raycastHit2.point,
						this.helperObject.transform.rotation
					});
					flag = true;
					return;
				}
				pcPropGrab.Drop(true);
				NonVRPlaceCamera(raycastHit2.point, this.helperObject.transform.rotation);
				flag = true;
			}
		}
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x0002B7D0 File Offset: 0x000299D0
	[PunRPC]
	private void NonVRPlaceCamera(Vector3 point, Quaternion rot)
	{
		base.transform.SetParent(null);
		this.helperObject.SetActive(false);
		base.transform.position = point;
		base.transform.rotation = rot;
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x0002B804 File Offset: 0x00029A04
	[PunRPC]
	private void PlaceCamera(int id)
	{
		Tripod component = PhotonView.Find(id).GetComponent<Tripod>();
		base.GetComponent<Rigidbody>().isKinematic = true;
		base.transform.SetParent(component.snapZone);
		base.transform.localPosition = Vector3.zero;
		base.transform.localRotation = Quaternion.identity;
		this.boxCollider.size = this.startColSize / 2f;
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x0002B878 File Offset: 0x00029A78
	[PunRPC]
	private void NetworkedUse()
	{
		this.IsThisCameraSwitchedOn = !this.IsThisCameraSwitchedOn;
		if (this.IsThisCameraSwitchedOn)
		{
			//CCTVController.instance.AddCamera(this);
			if (!this.isHeadCamera)
			{
				if (!this.isFixedCamera)
				{
					for (int i = 0; i < this.rends.Length; i++)
					{
						this.rends[i].material.SetColor("_EmissionColor", Color.green);
					}
					return;
				}
				for (int j = 0; j < this.rends.Length; j++)
				{
					this.rends[j].material.EnableKeyword("_EMISSION");
				}
				return;
			}
		}
		else
		{
			//CCTVController.instance.RemoveCamera(this);
			if (!this.isHeadCamera)
			{
				if (!this.isFixedCamera)
				{
					for (int k = 0; k < this.rends.Length; k++)
					{
						this.rends[k].material.SetColor("_EmissionColor", Color.red);
					}
					return;
				}
				for (int l = 0; l < this.rends.Length; l++)
				{
					this.rends[l].material.DisableKeyword("_EMISSION");
				}
			}
		}
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x0002B98C File Offset: 0x00029B8C
	private void OnGrabbed()
	{
		if (!this.isHeadCamera)
		{
            if (PhotonNetwork.InRoom)
            {
				this.view.RPC("OnGrabbedSync", RpcTarget.All, Array.Empty<object>());
			} 
			else
            {
				OnGrabbedSync();
            }
			return;
		}
		if (XRDevice.isPresent && GameController.instance.myPlayer.player.playerHeadCamera.headCamera == this)
		{
			GameController.instance.myPlayer.player.playerHeadCamera.VRGrabOrPlaceCamera(this.view.ViewID, false);
		}
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x0002BA05 File Offset: 0x00029C05
	[PunRPC]
	private void OnGrabbedSync()
	{
		this.boxCollider.size = this.startColSize;
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x0002BA18 File Offset: 0x00029C18
	private void Update()
	{
		if (this.isFixedCamera || this.isHeadCamera)
		{
			return;
		}
		if (this.view.IsMine || !PhotonNetwork.InRoom)
		{
			if (this.photonInteract.isGrabbed)
			{
				if (!XRDevice.isPresent)
				{
					Camera playerCam = GameObject.Find("PCPlayerHead").GetComponent<Camera>();
					RaycastHit raycastHit;
					if (Physics.Raycast(playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out raycastHit, this.grabDistance, this.helperMask, QueryTriggerInteraction.Ignore))
					{
						if (!this.helperObject.activeInHierarchy)
						{
							this.helperObject.SetActive(true);
						}
						if (raycastHit.collider.GetComponent<Tripod>())
						{
							this.helperObject.transform.position = raycastHit.collider.GetComponent<Tripod>().snapZone.transform.position;
							this.helperObject.transform.rotation = raycastHit.collider.GetComponent<Tripod>().snapZone.transform.rotation;
						}
						else
						{
							this.helperObject.transform.position = raycastHit.point;
						}
					}
					else if (this.helperObject.activeInHierarchy)
					{
						this.helperObject.SetActive(false);
					}
					if (this.useKeyIsPressed)
					{
						this.helperObject.transform.Rotate(Vector3.up * Time.deltaTime * 100f);
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

	// Token: 0x0600075C RID: 1884 RVA: 0x0002BBAC File Offset: 0x00029DAC
	private void OnDisable()
	{
		if (this.helperObject)
		{
			this.helperObject.SetActive(false);
		}
		if (!this.isFixedCamera && !this.isHeadCamera && !XRDevice.isPresent && GameController.instance != null && GameController.instance.myPlayer != null && GameController.instance.myPlayer.player != null && GameController.instance.myPlayer.player.playerInput != null)
		{
			GameController.instance.myPlayer.player.playerInput.actions["Interact"].started -= delegate(InputAction.CallbackContext _)
			{
				this.UseKeyPressed();
			};
			GameController.instance.myPlayer.player.playerInput.actions["Interact"].canceled -= delegate(InputAction.CallbackContext _)
			{
				this.UseKeyStopped();
			};
		}
		if (CCTVController.instance)
		{
			CCTVController.instance.RemoveCamera(this);
		}
	}

	// Token: 0x0600075D RID: 1885 RVA: 0x0002BCB8 File Offset: 0x00029EB8
	private void OnPlayerSpawned()
	{
		if (GameController.instance != null && !this.isFixedCamera && !this.isHeadCamera && !XRDevice.isPresent)
		{
			GameController.instance.myPlayer.player.playerInput.actions["Interact"].started += delegate(InputAction.CallbackContext _)
			{
				this.UseKeyPressed();
			};
			GameController.instance.myPlayer.player.playerInput.actions["Interact"].canceled += delegate(InputAction.CallbackContext _)
			{
				this.UseKeyStopped();
			};
		}
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x0002BD44 File Offset: 0x00029F44
	public void UseKeyPressed()
	{
		this.useKeyIsPressed = true;
	}

	// Token: 0x0600075F RID: 1887 RVA: 0x0002BD4D File Offset: 0x00029F4D
	public void UseKeyStopped()
	{
		this.useKeyIsPressed = false;
	}
	public RenderTexture rt;

	// Token: 0x04000762 RID: 1890
	public Camera cam;

	public bool isThisCameraActiveOnACCTVScreen;

	// Token: 0x04000764 RID: 1892
	[SerializeField]
	private PhotonObjectInteract photonInteract;

	// Token: 0x04000765 RID: 1893
	[SerializeField]
	private PhotonView view;

	// Token: 0x04000766 RID: 1894
	public Renderer[] rends;

	// Token: 0x04000767 RID: 1895
	[SerializeField]
	private bool IsThisCameraSwitchedOn;

	// Token: 0x04000768 RID: 1896
	public bool isFixedCamera;

	// Token: 0x04000769 RID: 1897
	public bool isHeadCamera;

	// Token: 0x0400076A RID: 1898
	[SerializeField]
	private Nightvision nightVision;

	// Token: 0x0400076B RID: 1899
	[SerializeField]
	private GameObject helperObject;

	// Token: 0x0400076C RID: 1900
	public Light myLight;

	// Token: 0x0400076D RID: 1901
	[SerializeField]
	private BoxCollider boxCollider;

	// Token: 0x0400076E RID: 1902
	[SerializeField]
	private Vector3 startColSize;

	// Token: 0x0400076F RID: 1903
	public Transform headCamParent;

	// Token: 0x04000770 RID: 1904
	public Image mapIcon;

	// Token: 0x04000771 RID: 1905
	[SerializeField]
	private LevelRoom.Type floorType = LevelRoom.Type.firstFloor;

	// Token: 0x04000772 RID: 1906
	[Header("PC")]
	private readonly float grabDistance = 2f;

	// Token: 0x04000773 RID: 1907
	[SerializeField]
	private Ray playerAim;

	// Token: 0x04000774 RID: 1908
	[SerializeField]
	private LayerMask mask;

	// Token: 0x04000775 RID: 1909
	[SerializeField]
	private LayerMask helperMask;

	// Token: 0x04000776 RID: 1910
	[SerializeField]
	private bool useKeyIsPressed;
}

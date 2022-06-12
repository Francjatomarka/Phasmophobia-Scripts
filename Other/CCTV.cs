using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;
using Photon.Pun;

public class CCTV : MonoBehaviour
{
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

	private void SetupCCTV()
	{
		if (MapController.instance)
		{
			MapController.instance.AssignIcon(this.mapIcon.transform, this.floorType);
			return;
		}
		FindObjectOfType<MapController>().AssignIcon(this.mapIcon.transform, this.floorType);
	}

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

	[PunRPC]
	private void NonVRPlaceCamera(Vector3 point, Quaternion rot)
	{
		base.transform.SetParent(null);
		this.helperObject.SetActive(false);
		base.transform.position = point;
		base.transform.rotation = rot;
	}

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

	[PunRPC]
	private void OnGrabbedSync()
	{
		this.boxCollider.size = this.startColSize;
	}

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

	public void UseKeyPressed()
	{
		this.useKeyIsPressed = true;
	}

	public void UseKeyStopped()
	{
		this.useKeyIsPressed = false;
	}
	public RenderTexture rt;

	public Camera cam;

	public bool isThisCameraActiveOnACCTVScreen;

	[SerializeField]
	private PhotonObjectInteract photonInteract;

	[SerializeField]
	private PhotonView view;

	public Renderer[] rends;

	[SerializeField]
	private bool IsThisCameraSwitchedOn;

	public bool isFixedCamera;

	public bool isHeadCamera;

	[SerializeField]
	private Nightvision nightVision;

	[SerializeField]
	private GameObject helperObject;

	public Light myLight;

	[SerializeField]
	private BoxCollider boxCollider;

	[SerializeField]
	private Vector3 startColSize;

	public Transform headCamParent;

	public Image mapIcon;

	[SerializeField]
	private LevelRoom.Type floorType = LevelRoom.Type.firstFloor;

	[Header("PC")]
	private readonly float grabDistance = 2f;

	[SerializeField]
	private Ray playerAim;

	[SerializeField]
	private LayerMask mask;

	[SerializeField]
	private LayerMask helperMask;

	[SerializeField]
	private bool useKeyIsPressed;
}


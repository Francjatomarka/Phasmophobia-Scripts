using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Photon.Pun;

public class GhostWriting : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.rend = base.GetComponent<Renderer>();
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
	}

	private void Start()
	{
		if (!XRDevice.isPresent)
		{
			this.photonInteract.AddPCSecondaryUseEvent(new UnityAction(this.SecondaryUse));
		}
	}

	private void Update()
	{
		if (this.view.IsMine)
		{
			if (this.photonInteract.isGrabbed)
			{
				if (!XRDevice.isPresent)
				{
					Camera playerCam = GameObject.Find("PCPlayerHead").GetComponent<Camera>();
					RaycastHit raycastHit;
					if (Physics.Raycast(playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out raycastHit, this.grabDistance, this.mask, QueryTriggerInteraction.Ignore))
					{
						if (!this.helperObject.activeInHierarchy)
						{
							this.helperObject.SetActive(true);
						}
						this.helperObject.transform.position = raycastHit.point;
						this.helperObject.transform.rotation = Quaternion.LookRotation(raycastHit.normal);
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

	public void SecondaryUse()
	{
		PCPropGrab pcPropGrab = GameObject.Find("PCPlayerHead").GetComponent<PCPropGrab>();
		if (!XRDevice.isPresent && pcPropGrab.inventoryProps[pcPropGrab.inventoryIndex] == this.photonInteract)
		{
			Camera playerCam = GameObject.Find("PCPlayerHead").GetComponent<Camera>();
			this.playerAim = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
			RaycastHit raycastHit;
			if (Physics.Raycast(this.playerAim, out raycastHit, 1.6f, this.mask, QueryTriggerInteraction.Ignore))
			{
				pcPropGrab.Drop(true);
				if (PhotonNetwork.InRoom)
				{
					this.view.RPC("NonVRPlaceGhostBook", RpcTarget.All, new object[]
					{
						raycastHit.point,
						Quaternion.LookRotation(raycastHit.normal)
					});
					return;
				}
				NonVRPlaceGhostBook(raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
			}
		}
	}

	[PunRPC]
	private void NonVRPlaceGhostBook(Vector3 point, Quaternion rot)
	{
		base.transform.SetParent(null);
		this.helperObject.SetActive(false);
		base.transform.position = point;
		base.transform.rotation = rot;
	}

	public void Use()
	{
		if (!this.hasUsed && (LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Spirit || LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Revenant || LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Shade || LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Demon || LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Yurei || LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Oni))
		{
			this.view.RPC("SetTexture", RpcTarget.All, new object[]
			{
				UnityEngine.Random.Range(0, this.textures.Length)
			});
		}
	}

	[PunRPC]
	private void SetTexture(int index)
	{
		this.hasUsed = true;
		this.rend.material.mainTexture = this.textures[index];
	}

	[SerializeField]
	private Texture[] textures;

	private Renderer rend;

	private PhotonView view;

	private bool hasUsed;

	private PhotonObjectInteract photonInteract;

	[Header("PC")]
	private readonly float grabDistance = 3f;

	private Ray playerAim;

	[SerializeField]
	private LayerMask mask;

	[SerializeField]
	private GameObject helperObject;
}


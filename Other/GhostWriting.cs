using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Photon.Pun;

// Token: 0x02000111 RID: 273
public class GhostWriting : MonoBehaviour
{
	// Token: 0x06000798 RID: 1944 RVA: 0x0002D908 File Offset: 0x0002BB08
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.rend = base.GetComponent<Renderer>();
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x0002D92E File Offset: 0x0002BB2E
	private void Start()
	{
		if (!XRDevice.isPresent)
		{
			this.photonInteract.AddPCSecondaryUseEvent(new UnityAction(this.SecondaryUse));
		}
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x0002D950 File Offset: 0x0002BB50
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

	// Token: 0x0600079B RID: 1947 RVA: 0x0002DA50 File Offset: 0x0002BC50
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

	// Token: 0x0600079C RID: 1948 RVA: 0x0002DB55 File Offset: 0x0002BD55
	[PunRPC]
	private void NonVRPlaceGhostBook(Vector3 point, Quaternion rot)
	{
		base.transform.SetParent(null);
		this.helperObject.SetActive(false);
		base.transform.position = point;
		base.transform.rotation = rot;
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x0002DB88 File Offset: 0x0002BD88
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

	// Token: 0x0600079E RID: 1950 RVA: 0x0002DC7C File Offset: 0x0002BE7C
	[PunRPC]
	private void SetTexture(int index)
	{
		this.hasUsed = true;
		this.rend.material.mainTexture = this.textures[index];
	}

	// Token: 0x040007A0 RID: 1952
	[SerializeField]
	private Texture[] textures;

	// Token: 0x040007A1 RID: 1953
	private Renderer rend;

	// Token: 0x040007A2 RID: 1954
	private PhotonView view;

	// Token: 0x040007A3 RID: 1955
	private bool hasUsed;

	// Token: 0x040007A4 RID: 1956
	private PhotonObjectInteract photonInteract;

	// Token: 0x040007A5 RID: 1957
	[Header("PC")]
	private readonly float grabDistance = 3f;

	// Token: 0x040007A6 RID: 1958
	private Ray playerAim;

	// Token: 0x040007A7 RID: 1959
	[SerializeField]
	private LayerMask mask;

	// Token: 0x040007A8 RID: 1960
	[SerializeField]
	private GameObject helperObject;
}

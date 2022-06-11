using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Photon.Pun;

// Token: 0x0200011F RID: 287
[RequireComponent(typeof(PhotonView))]
public class SaltShaker : MonoBehaviour
{
	// Token: 0x06000810 RID: 2064 RVA: 0x00030E56 File Offset: 0x0002F056
	private void Awake()
	{
		this.source = base.GetComponent<AudioSource>();
		this.view = base.GetComponent<PhotonView>();
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
	}

	// Token: 0x06000811 RID: 2065 RVA: 0x00030E7C File Offset: 0x0002F07C
	private void Start()
	{
		if (XRDevice.isPresent)
		{
			this.photonInteract.AddUseEvent(new UnityAction(this.Use));
			return;
		}
		this.photonInteract.AddPCSecondaryUseEvent(new UnityAction(this.Use));
	}

	// Token: 0x06000812 RID: 2066 RVA: 0x00030EB4 File Offset: 0x0002F0B4
	private void Use()
	{
		if (this.usesLeft == 0)
		{
			return;
		}
		Camera playerCamera = GameObject.Find("PCPlayerHead").GetComponent<Camera>();
		RaycastHit raycastHit;
		if (!XRDevice.isPresent)
		{
			if (Physics.Raycast(playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out raycastHit, this.grabDistance, this.mask))
			{
                if (PhotonNetwork.InRoom)
                {
					this.view.RPC("NetworkedUse", RpcTarget.All, Array.Empty<object>());
					this.view.RPC("SpawnSalt", RpcTarget.All, new object[]
					{
					raycastHit.point,
					raycastHit.normal
					});
					return;
				}
				NetworkedUse();
				SpawnSalt(raycastHit.point, raycastHit.normal);
				return;
			}
		}
		else if (Physics.Raycast(base.transform.position, base.transform.TransformDirection(Vector3.forward), out raycastHit, 1.5f, this.mask))
		{
			if (PhotonNetwork.InRoom)
            {
				this.view.RPC("NetworkedUse", RpcTarget.All, Array.Empty<object>());
				this.view.RPC("SpawnSalt", RpcTarget.All, new object[]
				{
				raycastHit.point,
				raycastHit.normal
				});
				return;
			}
			NetworkedUse();
			SpawnSalt(raycastHit.point, raycastHit.normal);
		}
	}

	// Token: 0x06000813 RID: 2067 RVA: 0x00030FEC File Offset: 0x0002F1EC
	private void Update()
	{
		if (this.usesLeft == 0)
		{
			if (this.helperObject.activeInHierarchy)
			{
				this.helperObject.SetActive(false);
			}
			return;
		}
		if (this.view.IsMine)
		{
			if (this.photonInteract.isGrabbed)
			{
				if (!XRDevice.isPresent)
				{
					RaycastHit raycastHit;
					Camera playerCamera = GameObject.Find("PCPlayerHead").GetComponent<Camera>();
					if (Physics.Raycast(playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out raycastHit, this.grabDistance, this.mask))
					{
						if (!this.helperObject.activeInHierarchy)
						{
							this.helperObject.SetActive(true);
						}
						this.helperObject.transform.position = raycastHit.point;
						Quaternion rotation = this.helperObject.transform.rotation;
						rotation.SetLookRotation(raycastHit.normal);
						rotation.eulerAngles = new Vector3(Mathf.Round(rotation.eulerAngles.x / 90f) * 90f + 90f, Mathf.Round(rotation.eulerAngles.y / 90f) * 90f, rotation.eulerAngles.z);
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
					if (Physics.Raycast(base.transform.position, base.transform.TransformDirection(Vector3.forward), out raycastHit, 1.5f, this.mask))
					{
						if (!this.helperObject.activeInHierarchy)
						{
							this.helperObject.SetActive(true);
						}
						this.helperObject.transform.position = raycastHit.point;
						Quaternion rotation2 = this.helperObject.transform.rotation;
						rotation2.SetLookRotation(raycastHit.normal);
						rotation2.eulerAngles = new Vector3(Mathf.Round(rotation2.eulerAngles.x / 90f) * 90f + 90f, Mathf.Round(rotation2.eulerAngles.y / 90f) * 90f, rotation2.eulerAngles.z);
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

	// Token: 0x06000814 RID: 2068 RVA: 0x00031288 File Offset: 0x0002F488
	[PunRPC]
	private void NetworkedUse()
	{
		this.usesLeft--;
		this.source.Play();
		base.StartCoroutine(this.PlayNoiseObject());
		if (this.usesLeft == 0)
		{
			this.helperObject.SetActive(false);
		}
	}

	// Token: 0x06000815 RID: 2069 RVA: 0x000312C4 File Offset: 0x0002F4C4
	[PunRPC]
	private void SpawnSalt(Vector3 hitPos, Vector3 normal)
	{
		Quaternion rotation = this.helperObject.transform.rotation;
		rotation.SetLookRotation(normal);
		rotation.eulerAngles = new Vector3(Mathf.Round(rotation.eulerAngles.x / 90f) * 90f + 90f, Mathf.Round(rotation.eulerAngles.y / 90f) * 90f, rotation.eulerAngles.z);
		this.helperObject.transform.rotation = rotation;
        if (PhotonNetwork.InRoom)
        {
			PhotonNetwork.Instantiate(this.saltPrefab.name, hitPos, Quaternion.identity, 0);
			return;
		}
		Instantiate(this.saltPrefab, hitPos, Quaternion.identity);
	}

	// Token: 0x06000816 RID: 2070 RVA: 0x0003136B File Offset: 0x0002F56B
	private IEnumerator PlayNoiseObject()
	{
		this.noise.gameObject.SetActive(true);
		yield return 0;
		this.noise.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x0400080C RID: 2060
	private PhotonView view;

	// Token: 0x0400080D RID: 2061
	private PhotonObjectInteract photonInteract;

	// Token: 0x0400080E RID: 2062
	private AudioSource source;

	// Token: 0x0400080F RID: 2063
	[SerializeField]
	private LayerMask mask;

	// Token: 0x04000810 RID: 2064
	[SerializeField]
	private GameObject helperObject;

	// Token: 0x04000811 RID: 2065
	[SerializeField]
	private GameObject saltPrefab;

	// Token: 0x04000812 RID: 2066
	[SerializeField]
	private Noise noise;

	// Token: 0x04000813 RID: 2067
	private int usesLeft = 3;

	// Token: 0x04000814 RID: 2068
	[Header("PC")]
	private float grabDistance = 3f;

	// Token: 0x04000815 RID: 2069
	private Ray playerAim;
}

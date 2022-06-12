using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class SaltShaker : MonoBehaviour
{
	private void Awake()
	{
		this.source = base.GetComponent<AudioSource>();
		this.view = base.GetComponent<PhotonView>();
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
	}

	private void Start()
	{
		if (XRDevice.isPresent)
		{
			this.photonInteract.AddUseEvent(new UnityAction(this.Use));
			return;
		}
		this.photonInteract.AddPCSecondaryUseEvent(new UnityAction(this.Use));
	}

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

	private IEnumerator PlayNoiseObject()
	{
		this.noise.gameObject.SetActive(true);
		yield return 0;
		this.noise.gameObject.SetActive(false);
		yield break;
	}

	private PhotonView view;

	private PhotonObjectInteract photonInteract;

	private AudioSource source;

	[SerializeField]
	private LayerMask mask;

	[SerializeField]
	private GameObject helperObject;

	[SerializeField]
	private GameObject saltPrefab;

	[SerializeField]
	private Noise noise;

	private int usesLeft = 3;

	[Header("PC")]
	private float grabDistance = 3f;

	private Ray playerAim;
}


using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Photon.Pun;

public class Lighter : MonoBehaviour
{
	private void Awake()
	{
		this.noise = base.GetComponentInChildren<Noise>();
		this.source = base.GetComponent<AudioSource>();
		this.view = base.GetComponent<PhotonView>();
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.noise.gameObject.SetActive(false);
	}

	private void Start()
	{
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
		this.photonInteract.AddPCSecondaryUseEvent(new UnityAction(this.SecondaryUse));
		this.photonInteract.AddPCUnGrabbedEvent(new UnityAction(this.TurnOff));
		this.photonInteract.AddUnGrabbedEvent(new UnityAction(this.TurnOff));
		this.isOn = false;
		this.flame.SetActive(false);
	}

	private void Use()
	{
        if (PhotonNetwork.InRoom)
        {
			this.view.RPC("NetworkedUse", RpcTarget.All, Array.Empty<object>());
			return;
		}
		NetworkedUse();
	}

	private void SecondaryUse()
	{
		if (this.isOn && !XRDevice.isPresent)
		{
			Camera playerCam = GameObject.Find("PCPlayerHead").GetComponent<Camera>();
			this.playerAim = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
			RaycastHit raycastHit;
			if (Physics.Raycast(this.playerAim, out raycastHit, this.grabDistance, this.mask))
			{
				if (raycastHit.collider.GetComponent<Candle>())
				{
					if (!raycastHit.collider.GetComponent<Candle>().isOn)
					{
						raycastHit.collider.GetComponent<Candle>().Use();
						return;
					}
				}
				else
				{
					if (raycastHit.collider.GetComponent<WhiteSage>())
					{
						raycastHit.collider.GetComponent<WhiteSage>().Use();
						return;
					}
					if (raycastHit.collider.GetComponentInChildren<Candle>() && !raycastHit.collider.GetComponentInChildren<Candle>().isOn)
					{
						raycastHit.collider.GetComponentInChildren<Candle>().Use();
					}
				}
			}
		}
	}

	[PunRPC]
	private void NetworkedUse()
	{
		this.isOn = !this.isOn;
		base.StartCoroutine(this.PlayNoiseObject());
		if (this.source == null)
		{
			this.source = base.GetComponent<AudioSource>();
		}
		this.flame.SetActive(this.isOn);
		this.source.Play();
	}

	private void TurnOff()
	{
        if (PhotonNetwork.InRoom)
        {
			this.view.RPC("NetworkedTurnOff", RpcTarget.All, Array.Empty<object>());
			return;
		}
		NetworkedTurnOff();
	}

	[PunRPC]
	private void NetworkedTurnOff()
	{
		this.isOn = false;
		base.StartCoroutine(this.PlayNoiseObject());
		if (this.source == null)
		{
			this.source = base.GetComponent<AudioSource>();
		}
		this.flame.SetActive(false);
		this.source.Play();
	}

	private IEnumerator PlayNoiseObject()
	{
		this.noise.gameObject.SetActive(true);
		yield return 0;
		this.noise.gameObject.SetActive(false);
		yield break;
	}

	[SerializeField]
	private GameObject flame;

	private AudioSource source;

	private PhotonView view;

	private PhotonObjectInteract photonInteract;

	private Noise noise;

	[HideInInspector]
	public bool isOn;

	[Header("PC")]
	private float grabDistance = 3f;

	private Ray playerAim;

	[SerializeField]
	private LayerMask mask;
}


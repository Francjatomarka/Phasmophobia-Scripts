using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Photon.Pun;

// Token: 0x02000117 RID: 279
public class Lighter : MonoBehaviour
{
	// Token: 0x060007C6 RID: 1990 RVA: 0x0002E8A0 File Offset: 0x0002CAA0
	private void Awake()
	{
		this.noise = base.GetComponentInChildren<Noise>();
		this.source = base.GetComponent<AudioSource>();
		this.view = base.GetComponent<PhotonView>();
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.noise.gameObject.SetActive(false);
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x0002E8F0 File Offset: 0x0002CAF0
	private void Start()
	{
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
		this.photonInteract.AddPCSecondaryUseEvent(new UnityAction(this.SecondaryUse));
		this.photonInteract.AddPCUnGrabbedEvent(new UnityAction(this.TurnOff));
		this.photonInteract.AddUnGrabbedEvent(new UnityAction(this.TurnOff));
		this.isOn = false;
		this.flame.SetActive(false);
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x0002E96C File Offset: 0x0002CB6C
	private void Use()
	{
        if (PhotonNetwork.InRoom)
        {
			this.view.RPC("NetworkedUse", RpcTarget.All, Array.Empty<object>());
			return;
		}
		NetworkedUse();
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x0002E984 File Offset: 0x0002CB84
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

	// Token: 0x060007CA RID: 1994 RVA: 0x0002EA90 File Offset: 0x0002CC90
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

	// Token: 0x060007CB RID: 1995 RVA: 0x0002EAEF File Offset: 0x0002CCEF
	private void TurnOff()
	{
        if (PhotonNetwork.InRoom)
        {
			this.view.RPC("NetworkedTurnOff", RpcTarget.All, Array.Empty<object>());
			return;
		}
		NetworkedTurnOff();
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x0002EB08 File Offset: 0x0002CD08
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

	// Token: 0x060007CD RID: 1997 RVA: 0x0002EB5A File Offset: 0x0002CD5A
	private IEnumerator PlayNoiseObject()
	{
		this.noise.gameObject.SetActive(true);
		yield return 0;
		this.noise.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x040007CF RID: 1999
	[SerializeField]
	private GameObject flame;

	// Token: 0x040007D0 RID: 2000
	private AudioSource source;

	// Token: 0x040007D1 RID: 2001
	private PhotonView view;

	// Token: 0x040007D2 RID: 2002
	private PhotonObjectInteract photonInteract;

	// Token: 0x040007D3 RID: 2003
	private Noise noise;

	// Token: 0x040007D4 RID: 2004
	[HideInInspector]
	public bool isOn;

	// Token: 0x040007D5 RID: 2005
	[Header("PC")]
	private float grabDistance = 3f;

	// Token: 0x040007D6 RID: 2006
	private Ray playerAim;

	// Token: 0x040007D7 RID: 2007
	[SerializeField]
	private LayerMask mask;
}

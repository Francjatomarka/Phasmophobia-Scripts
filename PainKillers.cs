using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Photon.Pun;

// Token: 0x0200015E RID: 350
public class PainKillers : MonoBehaviour
{
	// Token: 0x06000975 RID: 2421 RVA: 0x0003A474 File Offset: 0x00038674
	private void Awake()
	{
		this.noise = base.GetComponentInChildren<Noise>();
		this.source = base.GetComponent<AudioSource>();
		this.view = base.GetComponent<PhotonView>();
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.noise.gameObject.SetActive(false);
	}

	// Token: 0x06000976 RID: 2422 RVA: 0x0003A4C2 File Offset: 0x000386C2
	private void Start()
	{
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x0003A4DC File Offset: 0x000386DC
	private void Use()
	{
		this.hasBeenUsed = true;
		if (!this.hasBeenUsed)
		{
			Player player = GameObject.Find("PCPlayer(Clone)").GetComponent<Player>();
			player.insanity -= 40f;
		}
        if (PhotonNetwork.InRoom)
        {
			this.view.RPC("NetworkedUse", RpcTarget.All, Array.Empty<object>());
			return;
		}
		NetworkedUse();
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x0003A52E File Offset: 0x0003872E
	[PunRPC]
	private void NetworkedUse()
	{
		this.hasBeenUsed = true;
		base.StartCoroutine(this.PlayNoiseObject());
		base.StartCoroutine(this.UsePills());
		if (PhotonNetwork.IsMasterClient || !PhotonNetwork.InRoom)
		{
			if(LevelController.instance != null)
            {
				LevelController.instance.itemSpawner.RemovePainKillers();
			}
		}
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x0003A567 File Offset: 0x00038767
	private IEnumerator UsePills()
	{
		this.source.Play();
		yield return new WaitUntil(() => !this.source.isPlaying);
		if (this.view.IsMine)
		{
			if (XRDevice.isPresent)
			{
				this.photonInteract.ActivateHands();
			}
			else
			{
				PCPropGrab pcPropGrab = GameObject.Find("PCPlayerHead").GetComponent<PCPropGrab>();
				pcPropGrab.Drop(false);
			}
		}
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x0003A576 File Offset: 0x00038776
	private IEnumerator PlayNoiseObject()
	{
		this.noise.gameObject.SetActive(true);
		yield return 0;
		this.noise.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x04000990 RID: 2448
	private AudioSource source;

	// Token: 0x04000991 RID: 2449
	private PhotonView view;

	// Token: 0x04000992 RID: 2450
	private PhotonObjectInteract photonInteract;

	// Token: 0x04000993 RID: 2451
	private Noise noise;

	// Token: 0x04000994 RID: 2452
	private bool hasBeenUsed;
}

using System;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

// Token: 0x02000105 RID: 261
[RequireComponent(typeof(PhotonView))]
public class Radio : MonoBehaviour
{
	// Token: 0x0600072A RID: 1834 RVA: 0x0002AC6F File Offset: 0x00028E6F
	private void Awake()
	{
		this.source = base.GetComponent<AudioSource>();
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.view = base.GetComponent<PhotonView>();
		this.noise.gameObject.SetActive(false);
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x0002ACA8 File Offset: 0x00028EA8
	private void Start()
	{
		this.isOn = false;
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
		this.source.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x0002ACF8 File Offset: 0x00028EF8
	private void Use()
	{
		this.view.RPC("NetworkedUse", RpcTarget.All, Array.Empty<object>());
	}

	// Token: 0x0600072D RID: 1837 RVA: 0x0002AD10 File Offset: 0x00028F10
	[PunRPC]
	private void NetworkedUse()
	{
		this.isOn = !this.isOn;
		this.noise.gameObject.SetActive(this.isOn);
		if (this.isOn)
		{
			this.source.Play();
			return;
		}
		this.source.Stop();
	}

	// Token: 0x0600072E RID: 1838 RVA: 0x0002AD61 File Offset: 0x00028F61
	[PunRPC]
	private void TurnOn()
	{
		this.isOn = true;
		this.source.Play();
		this.noise.gameObject.SetActive(this.isOn);
	}

	// Token: 0x04000740 RID: 1856
	[HideInInspector]
	public PhotonView view;

	// Token: 0x04000741 RID: 1857
	private PhotonObjectInteract photonInteract;

	// Token: 0x04000742 RID: 1858
	[SerializeField]
	private Noise noise;

	// Token: 0x04000743 RID: 1859
	private bool isOn;

	// Token: 0x04000744 RID: 1860
	private AudioSource source;
}

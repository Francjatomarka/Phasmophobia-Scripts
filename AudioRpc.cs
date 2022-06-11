using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000070 RID: 112
public class AudioRpc : MonoBehaviourPunCallbacks
{
	// Token: 0x0600028D RID: 653 RVA: 0x000114DA File Offset: 0x0000F6DA
	private void Awake()
	{
		this.m_Source = base.GetComponent<AudioSource>();
	}

	// Token: 0x0600028E RID: 654 RVA: 0x000114E8 File Offset: 0x0000F6E8
	[PunRPC]
	private void Marco()
	{
		if (!base.enabled)
		{
			return;
		}
		Debug.Log("Marco");
		this.m_Source.clip = this.marco;
		this.m_Source.Play();
	}

	// Token: 0x0600028F RID: 655 RVA: 0x00011519 File Offset: 0x0000F719
	[PunRPC]
	private void Polo()
	{
		if (!base.enabled)
		{
			return;
		}
		Debug.Log("Polo");
		this.m_Source.clip = this.polo;
		this.m_Source.Play();
	}

	// Token: 0x06000290 RID: 656 RVA: 0x0001154A File Offset: 0x0000F74A
	private void OnApplicationFocus(bool focus)
	{
		base.enabled = focus;
	}

	// Token: 0x040002E3 RID: 739
	public AudioClip marco;

	// Token: 0x040002E4 RID: 740
	public AudioClip polo;

	// Token: 0x040002E5 RID: 741
	private AudioSource m_Source;
}

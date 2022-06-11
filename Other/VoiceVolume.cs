using System;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

// Token: 0x02000174 RID: 372
public class VoiceVolume : MonoBehaviour
{
	// Token: 0x06000AA5 RID: 2725 RVA: 0x000421B0 File Offset: 0x000403B0
	private void Awake()
	{
		this.source = base.GetComponent<AudioSource>();
		this.startVolume = this.source.volume;
	}

	// Token: 0x06000AA6 RID: 2726 RVA: 0x000421D0 File Offset: 0x000403D0
	private void Start()
	{
		if (MainManager.instance)
		{
			this.source.outputAudioMixerGroup = SoundController.instance.GetAudioGroupFromSnapshot(SoundController.instance.firstFloorSnapshot);
			this.source.volume = this.startVolume;
			return;
		}
		this.source.volume = this.startVolume * (XRDevice.isPresent ? PauseMenuManager.instance.GetPlayerVolume(int.Parse(this.view.Owner.UserId)) : PauseMenuController.instance.GetPlayerVolume(int.Parse(this.view.Owner.UserId)));
	}

	// Token: 0x06000AA7 RID: 2727 RVA: 0x0004225F File Offset: 0x0004045F
	public void ApplyVoiceVolume(float volume)
	{
		if (MainManager.instance)
		{
			this.source.volume = this.startVolume;
			return;
		}
		this.source.volume = this.startVolume * volume;
	}

	// Token: 0x04000B10 RID: 2832
	private AudioSource source;

	// Token: 0x04000B11 RID: 2833
	[SerializeField]
	private PhotonView view;

	// Token: 0x04000B12 RID: 2834
	private float startVolume;
}

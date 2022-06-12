using System;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

public class VoiceVolume : MonoBehaviour
{
	private void Awake()
	{
		this.source = base.GetComponent<AudioSource>();
		this.startVolume = this.source.volume;
	}

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

	public void ApplyVoiceVolume(float volume)
	{
		if (MainManager.instance)
		{
			this.source.volume = this.startVolume;
			return;
		}
		this.source.volume = this.startVolume * volume;
	}

	private AudioSource source;

	[SerializeField]
	private PhotonView view;

	private float startVolume;
}


using System;
using UnityEngine;

public class VoiceOcclusion : MonoBehaviour
{
	public void SetVoiceMixer()
	{
		this.source.outputAudioMixerGroup = SoundController.instance.GetAudioGroupFromSnapshot(this.player.currentPlayerSnapshot);
	}

	public AudioSource source;

	[SerializeField]
	private Player player;
}


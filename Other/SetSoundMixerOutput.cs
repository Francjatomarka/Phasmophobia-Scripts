using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SetSoundMixerOutput : MonoBehaviour
{
	private void Start()
	{
		if (SoundController.instance)
		{
			base.GetComponent<AudioSource>().outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		}
	}
}


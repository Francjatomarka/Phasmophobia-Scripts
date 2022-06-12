using System;
using UnityEngine;

public class Noise : MonoBehaviour
{
	public void OnEnable()
	{
		if (this.source)
		{
			//this.source.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		}
	}

	public void PlaySound(AudioClip clip, float volume)
	{
		this.source.volume = volume;
		this.source.clip = clip;
		this.source.Play();
	}

	private void Update()
	{
		if (!this.source)
		{
			this.timer -= Time.deltaTime;
			if (this.timer < 0f)
			{
				base.gameObject.SetActive(false);
			}
		}
	}

	public float volume;

	private float timer = 3f;

	public AudioSource source;
}


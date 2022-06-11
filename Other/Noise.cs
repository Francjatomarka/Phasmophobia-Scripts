using System;
using UnityEngine;

// Token: 0x020000ED RID: 237
public class Noise : MonoBehaviour
{
	// Token: 0x06000690 RID: 1680 RVA: 0x000270BD File Offset: 0x000252BD
	public void OnEnable()
	{
		if (this.source)
		{
			//this.source.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		}
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x000270F1 File Offset: 0x000252F1
	public void PlaySound(AudioClip clip, float volume)
	{
		this.source.volume = volume;
		this.source.clip = clip;
		this.source.Play();
	}

	// Token: 0x06000692 RID: 1682 RVA: 0x00027116 File Offset: 0x00025316
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

	// Token: 0x040006AB RID: 1707
	public float volume;

	// Token: 0x040006AC RID: 1708
	private float timer = 3f;

	// Token: 0x040006AD RID: 1709
	public AudioSource source;
}

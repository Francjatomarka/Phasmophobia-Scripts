using System;
using UnityEngine;

// Token: 0x02000139 RID: 313
[RequireComponent(typeof(AudioSource))]
public class SetSoundMixerOutput : MonoBehaviour
{
	// Token: 0x0600083E RID: 2110 RVA: 0x000313F5 File Offset: 0x0002F5F5
	private void Start()
	{
		if (SoundController.instance)
		{
			base.GetComponent<AudioSource>().outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		}
	}
}

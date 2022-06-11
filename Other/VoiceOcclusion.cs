using System;
using UnityEngine;

// Token: 0x02000173 RID: 371
public class VoiceOcclusion : MonoBehaviour
{
	// Token: 0x06000AA3 RID: 2723 RVA: 0x0004218E File Offset: 0x0004038E
	public void SetVoiceMixer()
	{
		this.source.outputAudioMixerGroup = SoundController.instance.GetAudioGroupFromSnapshot(this.player.currentPlayerSnapshot);
	}

	// Token: 0x04000B0E RID: 2830
	public AudioSource source;

	// Token: 0x04000B0F RID: 2831
	[SerializeField]
	private Player player;
}

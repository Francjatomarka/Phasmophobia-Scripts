using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000008 RID: 8
public class SoundsForJoinAndLeave : MonoBehaviourPunCallbacks
{
    // Token: 0x06000020 RID: 32 RVA: 0x00002955 File Offset: 0x00000B55
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
		if (this.JoinClip != null)
		{
			if (this.source == null)
			{
				this.source = UnityEngine.Object.FindObjectOfType<AudioSource>();
			}
			this.source.PlayOneShot(this.JoinClip);
		}
	}

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
		if (this.LeaveClip != null)
		{
			if (this.source == null)
			{
				this.source = UnityEngine.Object.FindObjectOfType<AudioSource>();
			}
			this.source.PlayOneShot(this.LeaveClip);
		}
	}

	// Token: 0x0400002E RID: 46
	public AudioClip JoinClip;

	// Token: 0x0400002F RID: 47
	public AudioClip LeaveClip;

	// Token: 0x04000030 RID: 48
	private AudioSource source;
}

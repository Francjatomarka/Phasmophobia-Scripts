using System;
using Photon.Pun;
using UnityEngine;

public class SoundsForJoinAndLeave : MonoBehaviourPunCallbacks
{
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

	public AudioClip JoinClip;

	public AudioClip LeaveClip;

	private AudioSource source;
}


using System;
using UnityEngine;
using Photon.Pun;

public class GhostAudio : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.ghostAI = base.GetComponent<GhostAI>();
	}

	public void PlaySound(int id, bool loopSource, bool bansheePower)
	{
		if (!bansheePower)
		{
			this.view.RPC("PlaySoundNetworked", RpcTarget.All, new object[]
			{
				id,
				loopSource
			});
			return;
		}
		this.view.RPC("PlaySoundNetworked", this.ghostAI.bansheeTarget.view.Owner, new object[]
		{
			id,
			loopSource
		});
	}

	public void StopSound()
	{
		this.view.RPC("StopSoundNetworked", RpcTarget.All, Array.Empty<object>());
	}

	[PunRPC]
	private void StopSoundNetworked()
	{
		this.soundFXSource.Stop();
	}

	[PunRPC]
	private void PlaySoundNetworked(int id, bool loopSource)
	{
		if (id == 0)
		{
			this.soundFXSource.clip = this.hummingSound;
			this.soundFXSource.volume = 0.6f;
		}
		else if (id == 1)
		{
			this.soundFXSource.clip = this.screamSoundClips[UnityEngine.Random.Range(0, this.screamSoundClips.Length)];
			this.soundFXSource.volume = 0.3f;
		}
		this.soundFXSource.loop = loopSource;
		this.soundFXSource.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(this.ghostAI.transform.position.y);
		this.soundFXSource.Play();
	}

	public void TurnOnOrOffAppearSource(bool on)
	{
		this.view.RPC("TurnOnOrOffAppearSourceSync", RpcTarget.All, new object[]
		{
			on
		});
	}

	[PunRPC]
	private void TurnOnOrOffAppearSourceSync(bool on)
	{
		if (on)
		{
			this.ghostAppearSource.Play();
			return;
		}
		this.ghostAppearSource.Stop();
	}

	public void PlayOrStopAppearSource(bool play)
	{
		this.view.RPC("PlayOrStopAppearSourceSync", RpcTarget.All, new object[]
		{
			play
		});
	}

	[PunRPC]
	private void PlayOrStopAppearSourceSync(bool play)
	{
		this.ghostAppearSource.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		if (!play)
		{
			this.ghostAppearSource.volume = 0f;
			return;
		}
		if (!Physics.Linecast(this.ghostAI.raycastPoint.position, GameController.instance.myPlayer.player.headObject.transform.position, this.ghostAI.mask, QueryTriggerInteraction.Ignore))
		{
			this.ghostAppearSource.volume = 0.1f;
			return;
		}
		this.ghostAppearSource.volume = 0f;
	}

	private PhotonView view;

	private GhostAI ghostAI;

	[SerializeField]
	private AudioSource soundFXSource;

	[SerializeField]
	private AudioClip hummingSound;

	[SerializeField]
	private AudioClip[] screamSoundClips;

	[SerializeField]
	private AudioSource ghostAppearSource;
}


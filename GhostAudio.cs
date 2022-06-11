using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x020000A8 RID: 168
public class GhostAudio : MonoBehaviour
{
	// Token: 0x060004FF RID: 1279 RVA: 0x0001BB97 File Offset: 0x00019D97
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.ghostAI = base.GetComponent<GhostAI>();
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x0001BBB4 File Offset: 0x00019DB4
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

	// Token: 0x06000501 RID: 1281 RVA: 0x0001BC2B File Offset: 0x00019E2B
	public void StopSound()
	{
		this.view.RPC("StopSoundNetworked", RpcTarget.All, Array.Empty<object>());
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x0001BC43 File Offset: 0x00019E43
	[PunRPC]
	private void StopSoundNetworked()
	{
		this.soundFXSource.Stop();
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x0001BC50 File Offset: 0x00019E50
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

	// Token: 0x06000504 RID: 1284 RVA: 0x0001BCF8 File Offset: 0x00019EF8
	public void TurnOnOrOffAppearSource(bool on)
	{
		this.view.RPC("TurnOnOrOffAppearSourceSync", RpcTarget.All, new object[]
		{
			on
		});
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x0001BD1A File Offset: 0x00019F1A
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

	// Token: 0x06000506 RID: 1286 RVA: 0x0001BD36 File Offset: 0x00019F36
	public void PlayOrStopAppearSource(bool play)
	{
		this.view.RPC("PlayOrStopAppearSourceSync", RpcTarget.All, new object[]
		{
			play
		});
	}

	// Token: 0x06000507 RID: 1287 RVA: 0x0001BD58 File Offset: 0x00019F58
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

	// Token: 0x040004C4 RID: 1220
	private PhotonView view;

	// Token: 0x040004C5 RID: 1221
	private GhostAI ghostAI;

	// Token: 0x040004C6 RID: 1222
	[SerializeField]
	private AudioSource soundFXSource;

	// Token: 0x040004C7 RID: 1223
	[SerializeField]
	private AudioClip hummingSound;

	// Token: 0x040004C8 RID: 1224
	[SerializeField]
	private AudioClip[] screamSoundClips;

	// Token: 0x040004C9 RID: 1225
	[SerializeField]
	private AudioSource ghostAppearSource;
}

using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000183 RID: 387
[RequireComponent(typeof(PhotonView))]
public class TruckRadioController : MonoBehaviourPunCallbacks
{
	// Token: 0x06000B03 RID: 2819 RVA: 0x00045B20 File Offset: 0x00043D20
	private void Awake()
	{
		TruckRadioController.instance = this;
		this.view = base.GetComponent<PhotonView>();
		this.source = base.GetComponent<AudioSource>();
	}

	// Token: 0x06000B04 RID: 2820 RVA: 0x00045B40 File Offset: 0x00043D40
	private void Start()
	{
		if (GameController.instance)
		{
			GameController.instance.OnAllPlayersConnected.AddListener(new UnityAction(this.PlayIntroductionAudio));
		}
	}

	// Token: 0x06000B05 RID: 2821 RVA: 0x00045B6C File Offset: 0x00043D6C
	public void PlayIntroductionAudio()
	{
		if (this.source == null)
		{
			return;
		}
		if (!this.view.IsMine)
		{
			return;
		}
		int num = UnityEngine.Random.Range(0, this.introductionClips.Length);
		this.view.RPC("PlayAudioClip", RpcTarget.All, new object[]
		{
			0,
			num
		});
	}

	// Token: 0x06000B06 RID: 2822 RVA: 0x00045BD0 File Offset: 0x00043DD0
	public void PlayKeyWarningAudio()
	{
		if (!this.view.IsMine)
		{
			return;
		}
		if (this.source.isPlaying)
		{
			return;
		}
		int num = UnityEngine.Random.Range(0, this.keyWarningClips.Length);
		this.view.RPC("PlayAudioClip", RpcTarget.All, new object[]
		{
			2,
			num
		});
	}

	// Token: 0x06000B07 RID: 2823 RVA: 0x00045C34 File Offset: 0x00043E34
	public void PlayHintAudio()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		switch (UnityEngine.Random.Range(0, 4))
		{
		case 0:
			this.view.RPC("PlayAudioClip", RpcTarget.All, new object[]
			{
				5,
				UnityEngine.Random.Range(0, this.nonFriendlyHintClips.Length)
			});
			return;
		case 1:
			this.view.RPC("PlayAudioClip", RpcTarget.All, new object[]
			{
				4,
				UnityEngine.Random.Range(0, this.friendlyHintClips.Length)
			});
			return;
		case 2:
			this.view.RPC("PlayAudioClip", RpcTarget.All, new object[]
			{
				3,
				UnityEngine.Random.Range(0, this.aggressiveHintClips.Length)
			});
			return;
		case 3:
			this.view.RPC("PlayAudioClip", RpcTarget.All, new object[]
			{
				1,
				UnityEngine.Random.Range(0, this.noHintClips.Length)
			});
			return;
		default:
			return;
		}
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x00045D43 File Offset: 0x00043F43
	[PunRPC]
	private IEnumerator PlayAudioClip(int id, int rand)
	{
		if (!this.view.IsMine)
		{
			yield return null;
		}
		switch (id)
		{
		case 0:
			yield return new WaitForSeconds(2f);
			this.source.clip = this.introductionClips[rand];
			this.source.Play();
			yield return new WaitForSeconds(this.introductionClips[rand].length + 2f);
			this.PlayHintAudio();
			break;
		case 1:
			this.source.clip = this.noHintClips[rand];
			break;
		case 2:
			if (!this.playedKeyAudio)
			{
				this.source.clip = this.keyWarningClips[rand];
				this.playedKeyAudio = true;
			}
			break;
		case 3:
			this.source.clip = this.aggressiveHintClips[rand];
			break;
		case 4:
			this.source.clip = this.friendlyHintClips[rand];
			break;
		case 5:
			this.source.clip = this.nonFriendlyHintClips[rand];
			break;
		}
		if (id != 0)
		{
			this.source.Play();
		}
		yield break;
	}

	// Token: 0x04000B88 RID: 2952
	public static TruckRadioController instance;

	// Token: 0x04000B89 RID: 2953
	private PhotonView view;

	// Token: 0x04000B8A RID: 2954
	private AudioSource source;

	// Token: 0x04000B8B RID: 2955
	[HideInInspector]
	public bool playedKeyAudio;

	// Token: 0x04000B8C RID: 2956
	[SerializeField]
	private AudioClip[] introductionClips;

	// Token: 0x04000B8D RID: 2957
	[SerializeField]
	private AudioClip[] keyWarningClips;

	// Token: 0x04000B8E RID: 2958
	[SerializeField]
	private AudioClip[] noHintClips;

	// Token: 0x04000B8F RID: 2959
	[SerializeField]
	private AudioClip[] aggressiveHintClips;

	// Token: 0x04000B90 RID: 2960
	[SerializeField]
	private AudioClip[] friendlyHintClips;

	// Token: 0x04000B91 RID: 2961
	[SerializeField]
	private AudioClip[] nonFriendlyHintClips;
}

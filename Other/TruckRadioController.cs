using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PhotonView))]
public class TruckRadioController : MonoBehaviourPunCallbacks
{
	private void Awake()
	{
		TruckRadioController.instance = this;
		this.view = base.GetComponent<PhotonView>();
		this.source = base.GetComponent<AudioSource>();
	}

	private void Start()
	{
		if (GameController.instance)
		{
			GameController.instance.OnAllPlayersConnected.AddListener(new UnityAction(this.PlayIntroductionAudio));
		}
	}

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

	public static TruckRadioController instance;

	private PhotonView view;

	private AudioSource source;

	[HideInInspector]
	public bool playedKeyAudio;

	[SerializeField]
	private AudioClip[] introductionClips;

	[SerializeField]
	private AudioClip[] keyWarningClips;

	[SerializeField]
	private AudioClip[] noHintClips;

	[SerializeField]
	private AudioClip[] aggressiveHintClips;

	[SerializeField]
	private AudioClip[] friendlyHintClips;

	[SerializeField]
	private AudioClip[] nonFriendlyHintClips;
}


using System;
using System.Collections;
using UnityEngine;
using Photon.Pun;

public class MenuAudio : MonoBehaviour
{
	private void Awake()
	{
		MenuAudio.instance = this;
		this.source = base.GetComponent<AudioSource>();
	}

	private void Start()
	{
		base.StartCoroutine(this.PlayIntroAudio());
	}

	private IEnumerator PlayIntroAudio()
	{
		yield return new WaitForSeconds(2f);
		if (PlayerPrefs.GetInt("FirstSession") == 0)
		{
			this.source.clip = this.firstSessionClip;
			this.source.Play();
			PlayerPrefs.SetInt("FirstSession", 1);
		}
		else
		{
			this.source.clip = this.welcomeBackClips[UnityEngine.Random.Range(0, this.welcomeBackClips.Length)];
			this.source.Play();
		}
		yield break;
	}

	public void LobbyScreen(int roomsAmount)
	{
		if (!PhotonNetwork.InLobby)
		{
			return;
		}
		if (roomsAmount > 0)
		{
			this.source.clip = this.lobbyFoundPlayersClips[UnityEngine.Random.Range(0, this.lobbyFoundPlayersClips.Length)];
			this.source.Play();
			return;
		}
		this.source.clip = this.lobbyNoPlayersClips[UnityEngine.Random.Range(0, this.lobbyNoPlayersClips.Length)];
		this.source.Play();
	}

	public static MenuAudio instance;

	private AudioSource source;

	[SerializeField]
	private AudioClip firstSessionClip;

	[SerializeField]
	private AudioClip[] welcomeBackClips;

	[SerializeField]
	private AudioClip[] lobbyNoPlayersClips;

	[SerializeField]
	private AudioClip[] lobbyFoundPlayersClips;
}


using System;
using System.Collections;
using UnityEngine;
using Photon.Pun;

// Token: 0x020000CB RID: 203
public class MenuAudio : MonoBehaviour
{
	// Token: 0x060005A9 RID: 1449 RVA: 0x00020E22 File Offset: 0x0001F022
	private void Awake()
	{
		MenuAudio.instance = this;
		this.source = base.GetComponent<AudioSource>();
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x00020E36 File Offset: 0x0001F036
	private void Start()
	{
		base.StartCoroutine(this.PlayIntroAudio());
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x00020E45 File Offset: 0x0001F045
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

	// Token: 0x060005AC RID: 1452 RVA: 0x00020E54 File Offset: 0x0001F054
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

	// Token: 0x0400054A RID: 1354
	public static MenuAudio instance;

	// Token: 0x0400054B RID: 1355
	private AudioSource source;

	// Token: 0x0400054C RID: 1356
	[SerializeField]
	private AudioClip firstSessionClip;

	// Token: 0x0400054D RID: 1357
	[SerializeField]
	private AudioClip[] welcomeBackClips;

	// Token: 0x0400054E RID: 1358
	[SerializeField]
	private AudioClip[] lobbyNoPlayersClips;

	// Token: 0x0400054F RID: 1359
	[SerializeField]
	private AudioClip[] lobbyFoundPlayersClips;
}

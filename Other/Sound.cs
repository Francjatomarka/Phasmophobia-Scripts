using System;
using System.Collections;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class Sound : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.noise.gameObject.SetActive(false);
	}

	public void Use()
	{
		this.view.RPC("NetworkedUse", RpcTarget.All, new object[]
		{
			UnityEngine.Random.Range(0, this.clips.Length)
		});
	}

	[PunRPC]
	private void NetworkedUse(int id)
	{
		base.StartCoroutine(this.PlayNoiseObject());
		this.source.clip = this.clips[id];
		this.source.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		this.source.Play();
	}

	private IEnumerator PlayNoiseObject()
	{
		this.noise.gameObject.SetActive(true);
		yield return 0;
		this.noise.gameObject.SetActive(false);
		yield break;
	}

	private PhotonView view;

	[SerializeField]
	private Noise noise;

	[SerializeField]
	private AudioClip[] clips;

	[SerializeField]
	private AudioSource source;
}


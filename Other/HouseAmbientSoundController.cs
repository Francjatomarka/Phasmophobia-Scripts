using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class HouseAmbientSoundController : MonoBehaviourPunCallbacks
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.col = base.GetComponent<BoxCollider>();
	}

	private void Update()
	{
		if (!PhotonNetwork.InRoom)
		{
			return;
		}
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		this.timer -= Time.deltaTime;
		if (this.timer < 0f)
		{
			if (!this.src)
			{
				Vector3 vector = new Vector3(UnityEngine.Random.Range(this.col.bounds.min.x, this.col.bounds.max.x), base.transform.position.y, UnityEngine.Random.Range(this.col.bounds.min.z, this.col.bounds.max.z));
				this.view.RPC("PlaySound", RpcTarget.All, new object[]
				{
					vector,
					UnityEngine.Random.Range(0, this.clips.Count)
				});
			}
			this.timer = UnityEngine.Random.Range(5f, 20f);
		}
	}

	[PunRPC]
	private void PlaySound(Vector3 pos, int clipID)
	{
		if (GameController.instance.myPlayer != null && GameController.instance.myPlayer.player.isDead)
		{
			return;
		}
		if (this.roomSpecific)
		{
			if (LevelController.instance.currentPlayerRoom == this.specificRoom)
			{
				ObjectPooler.instance.SpawnFromPool("Noise", pos, Quaternion.identity).GetComponent<Noise>().PlaySound(this.clips[clipID], 0.6f);
				return;
			}
		}
		else
		{
			ObjectPooler.instance.SpawnFromPool("Noise", pos, Quaternion.identity).GetComponent<Noise>().PlaySound(this.clips[clipID], 0.15f);
		}
	}

	private BoxCollider col;

	private float timer;

	public List<AudioClip> clips = new List<AudioClip>();

	private AudioSource src;

	private PhotonView view;

	[SerializeField]
	private LevelRoom specificRoom;

	[SerializeField]
	private bool roomSpecific;
}


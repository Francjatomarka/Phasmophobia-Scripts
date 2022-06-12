using System;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class GhostEventPlayer : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.agent = base.GetComponent<NavMeshAgent>();
	}

	public void SpawnPlayer(Player target, Vector3 pos)
	{
		this.agent.Warp(pos);
		this.targetPlayer = target;
		this.agent.isStopped = false;
		if (PhotonNetwork.PlayerList.Length > 1)
		{
			this.view.RPC("SpawnRandomPlayerModel", RpcTarget.All, new object[]
			{
				GameController.instance.playersData[UnityEngine.Random.Range(0, GameController.instance.playersData.Count)].player.modelID
			});
			return;
		}
		this.view.RPC("SpawnRandomPlayerModel", RpcTarget.All, new object[]
		{
			UnityEngine.Random.Range(0, this.models.Length)
		});
	}

	public void Stop()
	{
		this.targetPlayer = null;
		this.walkTimer = 0.7f;
		this.view.RPC("StopNetworked", RpcTarget.All, Array.Empty<object>());
		this.agent.isStopped = true;
	}

	[PunRPC]
	public void StopNetworked()
	{
		for (int i = 0; i < this.models.Length; i++)
		{
			this.models[i].SetActive(false);
		}
	}

	[PunRPC]
	private void SpawnRandomPlayerModel(int id)
	{
		for (int i = 0; i < this.models.Length; i++)
		{
			this.models[i].SetActive(false);
		}
		this.models[id].SetActive(true);
	}

	private void Update()
	{
		if (this.targetPlayer != null)
		{
			this.agent.SetDestination(this.targetPlayer.headObject.transform.position);
			this.walkTimer -= Time.deltaTime;
			if (this.walkTimer < 0f)
			{
				this.view.RPC("NetworkedPlaySound", RpcTarget.All, Array.Empty<object>());
				this.walkTimer = 0.7f;
			}
		}
	}

	[PunRPC]
	private void NetworkedPlaySound()
	{
		this.source.clip = this.footstepClips[UnityEngine.Random.Range(0, this.footstepClips.Length)];
		this.source.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		this.source.Play();
	}

	private PhotonView view;

	private NavMeshAgent agent;

	private Player targetPlayer;

	[SerializeField]
	private GameObject[] models;

	public AudioSource screamSource;

	[SerializeField]
	private AudioSource source;

	[SerializeField]
	private AudioClip[] footstepClips;

	private float walkTimer = 0.7f;
}


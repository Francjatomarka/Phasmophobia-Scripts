using System;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

// Token: 0x020000A9 RID: 169
public class GhostEventPlayer : MonoBehaviour
{
	// Token: 0x06000509 RID: 1289 RVA: 0x0001BE05 File Offset: 0x0001A005
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.agent = base.GetComponent<NavMeshAgent>();
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x0001BE20 File Offset: 0x0001A020
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

	// Token: 0x0600050B RID: 1291 RVA: 0x0001BED2 File Offset: 0x0001A0D2
	public void Stop()
	{
		this.targetPlayer = null;
		this.walkTimer = 0.7f;
		this.view.RPC("StopNetworked", RpcTarget.All, Array.Empty<object>());
		this.agent.isStopped = true;
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x0001BF08 File Offset: 0x0001A108
	[PunRPC]
	public void StopNetworked()
	{
		for (int i = 0; i < this.models.Length; i++)
		{
			this.models[i].SetActive(false);
		}
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x0001BF38 File Offset: 0x0001A138
	[PunRPC]
	private void SpawnRandomPlayerModel(int id)
	{
		for (int i = 0; i < this.models.Length; i++)
		{
			this.models[i].SetActive(false);
		}
		this.models[id].SetActive(true);
	}

	// Token: 0x0600050E RID: 1294 RVA: 0x0001BF74 File Offset: 0x0001A174
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

	// Token: 0x0600050F RID: 1295 RVA: 0x0001BFF0 File Offset: 0x0001A1F0
	[PunRPC]
	private void NetworkedPlaySound()
	{
		this.source.clip = this.footstepClips[UnityEngine.Random.Range(0, this.footstepClips.Length)];
		this.source.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		this.source.Play();
	}

	// Token: 0x040004CA RID: 1226
	private PhotonView view;

	// Token: 0x040004CB RID: 1227
	private NavMeshAgent agent;

	// Token: 0x040004CC RID: 1228
	private Player targetPlayer;

	// Token: 0x040004CD RID: 1229
	[SerializeField]
	private GameObject[] models;

	// Token: 0x040004CE RID: 1230
	public AudioSource screamSource;

	// Token: 0x040004CF RID: 1231
	[SerializeField]
	private AudioSource source;

	// Token: 0x040004D0 RID: 1232
	[SerializeField]
	private AudioClip[] footstepClips;

	// Token: 0x040004D1 RID: 1233
	private float walkTimer = 0.7f;
}

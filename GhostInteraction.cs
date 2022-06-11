using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// Token: 0x020000AB RID: 171
public class GhostInteraction : MonoBehaviour
{
	// Token: 0x06000515 RID: 1301 RVA: 0x0001C0BF File Offset: 0x0001A2BF
	private void Awake()
	{
		this.listener = base.GetComponent<AudioListener>();
		this.ghostAI = base.GetComponent<GhostAI>();
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x06000516 RID: 1302 RVA: 0x0001C0E8 File Offset: 0x0001A2E8
	private void Update()
	{
		if (this.view.IsMine)
		{
			this.StepTimer -= Time.deltaTime;
			if (this.StepTimer < 0f)
			{
				if (this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Spirit)
				{
					this.StepTimer = ((this.ghostAI.ghostIsAppeared || this.ghostAI.isHunting || this.hasWalkedInSalt) ? UnityEngine.Random.Range(0.3f, 1f) : UnityEngine.Random.Range(2f, 15f));
				}
				else
				{
					this.StepTimer = ((this.ghostAI.ghostIsAppeared || this.ghostAI.isHunting || this.hasWalkedInSalt) ? UnityEngine.Random.Range(0.3f, 1f) : UnityEngine.Random.Range(15f, 40f));
				}
				this.GhostStep();
			}
		}
		if (this.hasWalkedInSalt)
		{
			this.walkedInSaltTimer -= Time.deltaTime;
			if (this.walkedInSaltTimer < 0f)
			{
				this.view.RPC("SyncSaltFalse", RpcTarget.All, Array.Empty<object>());
				this.walkedInSaltTimer = 10f;
			}
		}
	}

	// Token: 0x06000517 RID: 1303 RVA: 0x0001C21F File Offset: 0x0001A41F
	[PunRPC]
	private void SyncSaltFalse()
	{
		this.hasWalkedInSalt = false;
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x0001C228 File Offset: 0x0001A428
	private bool IsEMFEvidence()
	{
		return this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Phantom || this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Banshee || this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Jinn || this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Revenant || this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Shade || this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Oni;
	}

	// Token: 0x06000519 RID: 1305 RVA: 0x0001C2CC File Offset: 0x0001A4CC
	public void CreateInteractionEMF(Vector3 pos)
	{
		if (GameController.instance.myPlayer.player.isDead)
		{
			return;
		}
		if (!this.IsEMFEvidence())
		{
			this.SpawnEMF(pos, EMF.Type.GhostInteraction);
			return;
		}
		if (UnityEngine.Random.Range(0, 3) == 1)
		{
			this.SpawnEMF(pos, EMF.Type.GhostEvidence);
			return;
		}
		this.SpawnEMF(pos, EMF.Type.GhostInteraction);
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x0001C31C File Offset: 0x0001A51C
	public void CreateAppearedEMF(Vector3 pos)
	{
		if (GameController.instance.myPlayer.player.isDead)
		{
			return;
		}
		this.SpawnEMF(pos, EMF.Type.GhostAppeared);
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x0001C33D File Offset: 0x0001A53D
	public void CreateThrowingEMF(Vector3 pos)
	{
		this.SpawnEMF(pos, EMF.Type.GhostThrowing);
		this.view.RPC("PlayThrowingNoise", RpcTarget.All, new object[]
		{
			pos
		});
	}

	// Token: 0x0600051C RID: 1308 RVA: 0x0001C367 File Offset: 0x0001A567
	public void CreateDoorNoise(Vector3 pos)
	{
		if (GameController.instance.myPlayer.player.isDead)
		{
			return;
		}
		this.view.RPC("PlayDoorNoise", RpcTarget.All, new object[]
		{
			pos
		});
		this.CreateInteractionEMF(pos);
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x0001C3A8 File Offset: 0x0001A5A8
	private void GhostStep()
	{
		if (SetupPhaseController.instance && SetupPhaseController.instance.mainDoorHasUnlocked)
		{
			PhotonNetwork.Instantiate("Footstep", this.footstepSpawnPoint.position, this.footstepSpawnPoint.rotation, 0).GetComponent<PhotonView>().RPC("Spawn", RpcTarget.All, new object[]
			{
				UnityEngine.Random.Range(0, 2) == 1
			});
		}
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x0001C41C File Offset: 0x0001A61C
	[PunRPC]
	private void PlayDoorNoise(Vector3 pos)
	{
		Noise component = ObjectPooler.instance.SpawnFromPool("Noise", pos, Quaternion.identity).GetComponent<Noise>();
		component.source.volume = 0.6f;
		component.PlaySound(this.doorNoises[UnityEngine.Random.Range(0, this.doorNoises.Count)], 0.15f);
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x0001C47C File Offset: 0x0001A67C
	[PunRPC]
	private void PlayThrowingNoise(Vector3 pos)
	{
		Noise component = ObjectPooler.instance.SpawnFromPool("Noise", pos, Quaternion.identity).GetComponent<Noise>();
		component.source.volume = 0.6f;
		component.PlaySound(this.throwingNoises[UnityEngine.Random.Range(0, this.throwingNoises.Count)], 0.15f);
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x0001C4D9 File Offset: 0x0001A6D9
	private void SpawnEMF(Vector3 pos, EMF.Type type)
	{
		this.view.RPC("SpawnEMFNetworked", RpcTarget.All, new object[]
		{
			pos,
			(int)type
		});
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x0001C504 File Offset: 0x0001A704
	[PunRPC]
	private void SpawnEMFNetworked(Vector3 pos, int typeID)
	{
		ObjectPooler.instance.SpawnFromPool("EMF", pos, Quaternion.identity).GetComponent<EMF>().SetType((EMF.Type)typeID);
	}

	// Token: 0x040004D6 RID: 1238
	private GhostAI ghostAI;

	// Token: 0x040004D7 RID: 1239
	private AudioListener listener;

	// Token: 0x040004D8 RID: 1240
	private PhotonView view;

	// Token: 0x040004D9 RID: 1241
	[SerializeField]
	public List<AudioClip> throwingNoises = new List<AudioClip>();

	// Token: 0x040004DA RID: 1242
	public List<AudioClip> doorNoises = new List<AudioClip>();

	// Token: 0x040004DB RID: 1243
	[HideInInspector]
	public float StepTimer;

	// Token: 0x040004DC RID: 1244
	[SerializeField]
	private Transform footstepSpawnPoint;

	// Token: 0x040004DD RID: 1245
	[HideInInspector]
	public bool hasWalkedInSalt;

	// Token: 0x040004DE RID: 1246
	private float walkedInSaltTimer = 10f;
}

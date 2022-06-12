using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GhostInteraction : MonoBehaviour
{
	private void Awake()
	{
		this.listener = base.GetComponent<AudioListener>();
		this.ghostAI = base.GetComponent<GhostAI>();
		this.view = base.GetComponent<PhotonView>();
	}

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

	[PunRPC]
	private void SyncSaltFalse()
	{
		this.hasWalkedInSalt = false;
	}

	private bool IsEMFEvidence()
	{
		return this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Phantom || this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Banshee || this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Jinn || this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Revenant || this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Shade || this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Oni;
	}

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

	public void CreateAppearedEMF(Vector3 pos)
	{
		if (GameController.instance.myPlayer.player.isDead)
		{
			return;
		}
		this.SpawnEMF(pos, EMF.Type.GhostAppeared);
	}

	public void CreateThrowingEMF(Vector3 pos)
	{
		this.SpawnEMF(pos, EMF.Type.GhostThrowing);
		this.view.RPC("PlayThrowingNoise", RpcTarget.All, new object[]
		{
			pos
		});
	}

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

	[PunRPC]
	private void PlayDoorNoise(Vector3 pos)
	{
		Noise component = ObjectPooler.instance.SpawnFromPool("Noise", pos, Quaternion.identity).GetComponent<Noise>();
		component.source.volume = 0.6f;
		component.PlaySound(this.doorNoises[UnityEngine.Random.Range(0, this.doorNoises.Count)], 0.15f);
	}

	[PunRPC]
	private void PlayThrowingNoise(Vector3 pos)
	{
		Noise component = ObjectPooler.instance.SpawnFromPool("Noise", pos, Quaternion.identity).GetComponent<Noise>();
		component.source.volume = 0.6f;
		component.PlaySound(this.throwingNoises[UnityEngine.Random.Range(0, this.throwingNoises.Count)], 0.15f);
	}

	private void SpawnEMF(Vector3 pos, EMF.Type type)
	{
		this.view.RPC("SpawnEMFNetworked", RpcTarget.All, new object[]
		{
			pos,
			(int)type
		});
	}

	[PunRPC]
	private void SpawnEMFNetworked(Vector3 pos, int typeID)
	{
		ObjectPooler.instance.SpawnFromPool("EMF", pos, Quaternion.identity).GetComponent<EMF>().SetType((EMF.Type)typeID);
	}

	private GhostAI ghostAI;

	private AudioListener listener;

	private PhotonView view;

	[SerializeField]
	public List<AudioClip> throwingNoises = new List<AudioClip>();

	public List<AudioClip> doorNoises = new List<AudioClip>();

	[HideInInspector]
	public float StepTimer;

	[SerializeField]
	private Transform footstepSpawnPoint;

	[HideInInspector]
	public bool hasWalkedInSalt;

	private float walkedInSaltTimer = 10f;
}


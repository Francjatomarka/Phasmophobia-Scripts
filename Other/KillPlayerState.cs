using System;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class KillPlayerState : IState
{
	public KillPlayerState(GhostAI ghostAI, NavMeshAgent agent, PhotonView view)
	{
		this.ghostAI = ghostAI;
		this.agent = agent;
		this.view = view;
	}

	public void Enter()
	{
		this.ghostAI.ghostAudio.PlayOrStopAppearSource(true);
		if (this.ghostAI.playerToKill.VRIKObj != null)
		{
			DeadZoneController.instance.playerDeathSpot = this.ghostAI.playerToKill.headObject.transform.position;
		}
		else
		{
			DeadZoneController.instance.playerDeathSpot = this.ghostAI.playerToKill.transform.position;
		}
		this.agent.isStopped = true;
		this.ghostAI.ghostAudio.PlaySound(1, false, false);
		this.ghostAI.UnAppear(true);
		this.ghostAI.anim.SetBool("isIdle", true);
		this.ghostAI.anim.SetTrigger("Attack");
		this.ghostAI.playerToKill.StartKillingPlayer();
	}

	public void Execute()
	{
		this.killTimer -= Time.deltaTime;
		if (this.killTimer < 0f)
		{
			this.ghostAI.Appear(true);
			this.agent.isStopped = false;
			DeadZoneController.instance.SpawnDeathRoom();
			this.killTimer = 100f;
		}
	}

	public void Exit()
	{
		this.ghostAI.ghostAudio.PlayOrStopAppearSource(false);
		this.ghostAI.playerToKill = null;
	}

	private GhostAI ghostAI;

	private NavMeshAgent agent;

	private PhotonView view;

	private float killTimer = 4.55f;
}


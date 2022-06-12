using System;
using UnityEngine;
using UnityEngine.AI;

public class BansheePower : IState
{
	public BansheePower(GhostAI ghostAI, GhostInteraction ghostInteraction, GhostAudio ghostAudio, NavMeshAgent agent, LayerMask mask)
	{
		this.ghostInteraction = ghostInteraction;
		this.ghostAudio = ghostAudio;
		this.ghostAI = ghostAI;
		this.agent = agent;
		this.mask = mask;
	}

	public void Enter()
	{
		if (this.ghostAI.bansheeTarget.currentRoom != LevelController.instance.outsideRoom)
		{
			this.agent.destination = this.MoveToTargetPlayerPosition();
			return;
		}
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
	}

	public void Execute()
	{
		this.timer -= Time.deltaTime;
		if (this.timer < 0f)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (!Physics.Linecast(this.ghostAI.transform.position, this.ghostAI.bansheeTarget.transform.position, this.mask))
		{
			this.ghostAI.ChangeState(GameController.instance.isTutorial ? GhostAI.States.idle : GhostAI.States.hunting, null, null);
		}
		if (Vector3.Distance(this.ghostAI.transform.position, this.agent.destination) < 1f)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		}
	}

	public void Exit()
	{
		this.ghostAudio.StopSound();
	}

	private Vector3 MoveToTargetPlayerPosition()
	{
		float num = UnityEngine.Random.Range(1f, 2f);
		NavMeshHit navMeshHit;
		NavMesh.SamplePosition(UnityEngine.Random.insideUnitSphere * num + this.ghostAI.bansheeTarget.transform.position, out navMeshHit, num, 1);
		return navMeshHit.position;
	}

	private GhostInteraction ghostInteraction;

	private GhostAudio ghostAudio;

	private GhostAI ghostAI;

	private NavMeshAgent agent;

	private LayerMask mask;

	private float timer = 20f;
}


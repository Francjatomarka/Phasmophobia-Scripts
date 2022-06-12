using System;
using UnityEngine;
using UnityEngine.AI;

public class PhantomPower : IState
{
	public PhantomPower(GhostAI ghostAI, GhostInteraction ghostInteraction, NavMeshAgent agent)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.agent = agent;
	}

	public void Enter()
	{
		this.ghostAI.anim.SetBool("isIdle", false);
		this.agent.destination = this.MoveToPlayerPosition(GameController.instance.playersData[UnityEngine.Random.Range(0, GameController.instance.playersData.Count)].player);
		this.ghostInteraction.CreateInteractionEMF(this.ghostAI.raycastPoint.position);
	}

	public void Execute()
	{
		if (this.agent.pathStatus == NavMeshPathStatus.PathPartial || this.agent.pathStatus == NavMeshPathStatus.PathInvalid)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (Vector3.Distance(this.ghostAI.transform.position, this.agent.destination) < 1f)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		}
	}

	public void Exit()
	{
	}

	private Vector3 MoveToPlayerPosition(Player player)
	{
		float num = UnityEngine.Random.Range(1f, 5f);
		NavMeshHit navMeshHit;
		NavMesh.SamplePosition(UnityEngine.Random.insideUnitSphere * num + player.transform.position, out navMeshHit, num, 1);
		return navMeshHit.position;
	}

	private GhostInteraction ghostInteraction;

	private GhostAI ghostAI;

	private NavMeshAgent agent;
}


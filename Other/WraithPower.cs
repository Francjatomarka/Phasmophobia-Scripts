using System;
using UnityEngine;
using UnityEngine.AI;

public class WraithPower : IState
{
	public WraithPower(GhostAI ghostAI, GhostInteraction ghostInteraction, NavMeshAgent agent)
	{
		this.ghostInteraction = ghostInteraction;
		this.ghostAI = ghostAI;
		this.agent = agent;
	}

	public void Enter()
	{
		Player player = GameController.instance.playersData[UnityEngine.Random.Range(0, GameController.instance.playersData.Count)].player;
		if (player.currentRoom == LevelController.instance.outsideRoom)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (player.isDead)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		Vector3 vector;
		if (this.GetPositionOnNavMesh(player.transform.position, out vector))
		{
			this.agent.Warp(vector);
			this.ghostInteraction.CreateInteractionEMF(vector);
		}
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
	}

	public void Execute()
	{
	}

	public void Exit()
	{
	}

	private bool GetPositionOnNavMesh(Vector3 pos, out Vector3 result)
	{
		NavMeshHit navMeshHit;
		if (NavMesh.SamplePosition(pos, out navMeshHit, 3f, 1))
		{
			result = navMeshHit.position;
			return true;
		}
		result = Vector3.zero;
		return false;
	}

	private GhostInteraction ghostInteraction;

	private GhostAI ghostAI;

	private NavMeshAgent agent;
}


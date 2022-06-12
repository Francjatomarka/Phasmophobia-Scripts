using System;
using UnityEngine;
using UnityEngine.AI;

public class FavouriteRoomState : IState
{
	public FavouriteRoomState(GhostAI ghostAI, NavMeshAgent agent)
	{
		this.ghostAI = ghostAI;
		this.agent = agent;
	}

	public void Enter()
	{
		this.agent.destination = this.GetRandomPositionInRoom();
	}

	public void Execute()
	{
		this.stuckTimer -= Time.deltaTime;
		if (this.stuckTimer < 0f)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (this.agent.pathStatus == NavMeshPathStatus.PathPartial || this.agent.pathStatus == NavMeshPathStatus.PathInvalid || !this.agent.hasPath)
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

	private Vector3 GetRandomPositionInRoom()
	{
		float maxDistance = UnityEngine.Random.Range(0f, 0.5f);
		BoxCollider boxCollider = this.ghostAI.ghostInfo.favouriteRoom.colliders[UnityEngine.Random.Range(0, this.ghostAI.ghostInfo.favouriteRoom.colliders.Count)];
		NavMeshHit navMeshHit;
		NavMesh.SamplePosition(new Vector3(UnityEngine.Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x), boxCollider.bounds.min.y, UnityEngine.Random.Range(boxCollider.bounds.min.z, boxCollider.bounds.max.z)), out navMeshHit, maxDistance, 1);
		return navMeshHit.position;
	}

	private GhostAI ghostAI;

	private NavMeshAgent agent;

	private float stuckTimer = 30f;
}


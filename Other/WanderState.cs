using System;
using UnityEngine;
using UnityEngine.AI;

public class WanderState : IState
{
	public WanderState(GhostAI ghostAI, NavMeshAgent agent)
	{
		this.ghostAI = ghostAI;
		this.agent = agent;
	}

	public void Enter()
	{
		if (!this.ghostAI.canWander)
		{
			this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
			Debug.Log("GhostAI favourite Room 27");
			return;
		}
		Vector3 vector = this.RandomNavSphere();
		if (!LevelController.instance.currentGhostRoom.isBasementOrAttic && SoundController.instance.GetFloorTypeFromPosition(vector.y) != LevelController.instance.currentGhostRoom.floorType)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		this.agent.destination = vector;
		this.ghostAI.anim.SetBool("isIdle", false);
	}

	public void Execute()
	{
		this.stuckTimer -= Time.deltaTime;
		if (this.stuckTimer < 0f)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
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

	private Vector3 RandomNavSphere()
	{
		NavMeshHit navMeshHit;
		NavMesh.SamplePosition(UnityEngine.Random.insideUnitSphere * 3f + this.ghostAI.transform.position, out navMeshHit, 3f, 1);
		return navMeshHit.position;
	}

	private GhostAI ghostAI;

	private NavMeshAgent agent;

	private float stuckTimer = 30f;
}


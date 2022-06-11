using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020000C6 RID: 198
public class WanderState : IState
{
	// Token: 0x06000595 RID: 1429 RVA: 0x00020B24 File Offset: 0x0001ED24
	public WanderState(GhostAI ghostAI, NavMeshAgent agent)
	{
		this.ghostAI = ghostAI;
		this.agent = agent;
	}

	// Token: 0x06000596 RID: 1430 RVA: 0x00020B48 File Offset: 0x0001ED48
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

	// Token: 0x06000597 RID: 1431 RVA: 0x00020BDC File Offset: 0x0001EDDC
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

	// Token: 0x06000598 RID: 1432 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Exit()
	{
	}

	// Token: 0x06000599 RID: 1433 RVA: 0x00020C78 File Offset: 0x0001EE78
	private Vector3 RandomNavSphere()
	{
		NavMeshHit navMeshHit;
		NavMesh.SamplePosition(UnityEngine.Random.insideUnitSphere * 3f + this.ghostAI.transform.position, out navMeshHit, 3f, 1);
		return navMeshHit.position;
	}

	// Token: 0x04000541 RID: 1345
	private GhostAI ghostAI;

	// Token: 0x04000542 RID: 1346
	private NavMeshAgent agent;

	// Token: 0x04000543 RID: 1347
	private float stuckTimer = 30f;
}

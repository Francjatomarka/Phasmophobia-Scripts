using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020000B8 RID: 184
public class FavouriteRoomState : IState
{
	// Token: 0x06000559 RID: 1369 RVA: 0x0001EC07 File Offset: 0x0001CE07
	public FavouriteRoomState(GhostAI ghostAI, NavMeshAgent agent)
	{
		this.ghostAI = ghostAI;
		this.agent = agent;
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x0001EC28 File Offset: 0x0001CE28
	public void Enter()
	{
		this.agent.destination = this.GetRandomPositionInRoom();
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x0001EC3C File Offset: 0x0001CE3C
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

	// Token: 0x0600055C RID: 1372 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Exit()
	{
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x0001ECE4 File Offset: 0x0001CEE4
	private Vector3 GetRandomPositionInRoom()
	{
		float maxDistance = UnityEngine.Random.Range(0f, 0.5f);
		BoxCollider boxCollider = this.ghostAI.ghostInfo.favouriteRoom.colliders[UnityEngine.Random.Range(0, this.ghostAI.ghostInfo.favouriteRoom.colliders.Count)];
		NavMeshHit navMeshHit;
		NavMesh.SamplePosition(new Vector3(UnityEngine.Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x), boxCollider.bounds.min.y, UnityEngine.Random.Range(boxCollider.bounds.min.z, boxCollider.bounds.max.z)), out navMeshHit, maxDistance, 1);
		return navMeshHit.position;
	}

	// Token: 0x04000510 RID: 1296
	private GhostAI ghostAI;

	// Token: 0x04000511 RID: 1297
	private NavMeshAgent agent;

	// Token: 0x04000512 RID: 1298
	private float stuckTimer = 30f;
}

using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020000A3 RID: 163
public class PhantomPower : IState
{
	// Token: 0x060004C7 RID: 1223 RVA: 0x0001A407 File Offset: 0x00018607
	public PhantomPower(GhostAI ghostAI, GhostInteraction ghostInteraction, NavMeshAgent agent)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.agent = agent;
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x0001A424 File Offset: 0x00018624
	public void Enter()
	{
		this.ghostAI.anim.SetBool("isIdle", false);
		this.agent.destination = this.MoveToPlayerPosition(GameController.instance.playersData[UnityEngine.Random.Range(0, GameController.instance.playersData.Count)].player);
		this.ghostInteraction.CreateInteractionEMF(this.ghostAI.raycastPoint.position);
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x0001A49C File Offset: 0x0001869C
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

	// Token: 0x060004CA RID: 1226 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Exit()
	{
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x0001A50C File Offset: 0x0001870C
	private Vector3 MoveToPlayerPosition(Player player)
	{
		float num = UnityEngine.Random.Range(1f, 5f);
		NavMeshHit navMeshHit;
		NavMesh.SamplePosition(UnityEngine.Random.insideUnitSphere * num + player.transform.position, out navMeshHit, num, 1);
		return navMeshHit.position;
	}

	// Token: 0x0400049F RID: 1183
	private GhostInteraction ghostInteraction;

	// Token: 0x040004A0 RID: 1184
	private GhostAI ghostAI;

	// Token: 0x040004A1 RID: 1185
	private NavMeshAgent agent;
}

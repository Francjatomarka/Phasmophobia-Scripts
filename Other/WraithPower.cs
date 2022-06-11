using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020000A5 RID: 165
public class WraithPower : IState
{
	// Token: 0x060004D0 RID: 1232 RVA: 0x0001A74C File Offset: 0x0001894C
	public WraithPower(GhostAI ghostAI, GhostInteraction ghostInteraction, NavMeshAgent agent)
	{
		this.ghostInteraction = ghostInteraction;
		this.ghostAI = ghostAI;
		this.agent = agent;
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x0001A76C File Offset: 0x0001896C
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

	// Token: 0x060004D2 RID: 1234 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Execute()
	{
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Exit()
	{
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x0001A81C File Offset: 0x00018A1C
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

	// Token: 0x040004A6 RID: 1190
	private GhostInteraction ghostInteraction;

	// Token: 0x040004A7 RID: 1191
	private GhostAI ghostAI;

	// Token: 0x040004A8 RID: 1192
	private NavMeshAgent agent;
}

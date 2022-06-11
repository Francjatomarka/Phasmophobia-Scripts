using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020000A1 RID: 161
public class BansheePower : IState
{
	// Token: 0x060004BE RID: 1214 RVA: 0x0001A1A7 File Offset: 0x000183A7
	public BansheePower(GhostAI ghostAI, GhostInteraction ghostInteraction, GhostAudio ghostAudio, NavMeshAgent agent, LayerMask mask)
	{
		this.ghostInteraction = ghostInteraction;
		this.ghostAudio = ghostAudio;
		this.ghostAI = ghostAI;
		this.agent = agent;
		this.mask = mask;
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x0001A1E0 File Offset: 0x000183E0
	public void Enter()
	{
		if (this.ghostAI.bansheeTarget.currentRoom != LevelController.instance.outsideRoom)
		{
			this.agent.destination = this.MoveToTargetPlayerPosition();
			return;
		}
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x0001A230 File Offset: 0x00018430
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

	// Token: 0x060004C1 RID: 1217 RVA: 0x0001A2F4 File Offset: 0x000184F4
	public void Exit()
	{
		this.ghostAudio.StopSound();
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x0001A304 File Offset: 0x00018504
	private Vector3 MoveToTargetPlayerPosition()
	{
		float num = UnityEngine.Random.Range(1f, 2f);
		NavMeshHit navMeshHit;
		NavMesh.SamplePosition(UnityEngine.Random.insideUnitSphere * num + this.ghostAI.bansheeTarget.transform.position, out navMeshHit, num, 1);
		return navMeshHit.position;
	}

	// Token: 0x04000496 RID: 1174
	private GhostInteraction ghostInteraction;

	// Token: 0x04000497 RID: 1175
	private GhostAudio ghostAudio;

	// Token: 0x04000498 RID: 1176
	private GhostAI ghostAI;

	// Token: 0x04000499 RID: 1177
	private NavMeshAgent agent;

	// Token: 0x0400049A RID: 1178
	private LayerMask mask;

	// Token: 0x0400049B RID: 1179
	private float timer = 20f;
}

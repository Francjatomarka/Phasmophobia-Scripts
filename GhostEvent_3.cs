using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020000B6 RID: 182
public class GhostEvent_3 : IState
{
	// Token: 0x0600054F RID: 1359 RVA: 0x0001DF5B File Offset: 0x0001C15B
	public GhostEvent_3(GhostAI ghost, GhostInteraction ghostInteraction)
	{
		this.ghost = ghost;
		this.ghostInteraction = ghostInteraction;
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x0001DF88 File Offset: 0x0001C188
	public void Enter()
	{
		this.player = GameController.instance.playersData[UnityEngine.Random.Range(0, GameController.instance.playersData.Count)].player;
		float num = Vector3.Distance(this.ghost.raycastPoint.transform.position, this.player.headObject.transform.position);
		for (int i = 0; i < GameController.instance.playersData.Count; i++)
		{
			if (GameController.instance.playersData[i] != null && !GameController.instance.playersData[i].player.isDead && Vector3.Distance(this.ghost.raycastPoint.transform.position, GameController.instance.playersData[i].player.headObject.transform.position) < num)
			{
				num = Vector3.Distance(this.ghost.raycastPoint.transform.position, GameController.instance.playersData[i].player.headObject.transform.position);
				this.player = GameController.instance.playersData[i].player;
			}
		}
		if (this.player == null)
		{
			this.ghost.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (this.player.isDead)
		{
			this.ghost.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		this.room = this.player.currentRoom;
		Collider collider = this.room.colliders[UnityEngine.Random.Range(0, this.room.colliders.Count)];
		Vector3 vector = new Vector3(UnityEngine.Random.Range(collider.bounds.min.x, collider.bounds.max.x), collider.bounds.min.y, UnityEngine.Random.Range(collider.bounds.min.z, collider.bounds.max.z));
		if (this.room == LevelController.instance.outsideRoom)
		{
			this.ghost.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (this.room.doors.Length == 0 && !this.room.isBasementOrAttic)
		{
			this.ghost.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (this.room.floorType != LevelController.instance.currentGhostRoom.floorType)
		{
			this.ghost.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (Vector3.Distance(this.player.headObject.transform.position, this.ghost.raycastPoint.transform.position) > 15f)
		{
			this.ghost.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (Vector3.Distance(this.player.headObject.transform.position, vector) < 2f)
		{
			this.ghost.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (MissionGhostEvent.instance != null && !MissionGhostEvent.instance.completed)
		{
			MissionGhostEvent.instance.CompleteMission();
		}
		for (int j = 0; j < this.room.lightSwitches.Count; j++)
		{
			this.room.lightSwitches[j].TurnOff();
		}
		this.ghost.agent.Warp(this.GetPositionOnNavMesh(vector));
		this.ghost.anim.SetBool("isIdle", true);
		this.ghost.ghostAudio.PlaySound(0, true, false);
		this.ghost.Appear(true);
		this.ghost.ghostAudio.TurnOnOrOffAppearSource(true);
		this.ghost.ghostAudio.PlayOrStopAppearSource(true);
		this.ghost.ghostInteraction.CreateAppearedEMF(this.ghost.transform.position);
	}

	// Token: 0x06000551 RID: 1361 RVA: 0x0001E3A4 File Offset: 0x0001C5A4
	public void Execute()
	{
		this.timer -= Time.deltaTime;
		RaycastHit raycastHit;
		if (Physics.Raycast(this.player.cam.transform.position, this.player.cam.transform.forward, out raycastHit, 2f, this.player.ghostRaycastMask) && raycastHit.collider.CompareTag("Ghost"))
		{
			this.ghost.ghostAudio.PlaySound(1, false, false);
			this.player.ChangeSanity(-20);
			this.ghost.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (Vector3.Distance(this.player.headObject.transform.position, this.ghost.raycastPoint.transform.position) > 5f)
		{
			this.ghost.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (this.player.currentRoom != this.room)
		{
			this.ghost.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (this.timer < 0f)
		{
			this.ghost.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x0001E4D5 File Offset: 0x0001C6D5
	public void Exit()
	{
		this.ghost.ghostAudio.StopSound();
		this.ghost.UnAppear(true);
		this.ghost.ghostAudio.TurnOnOrOffAppearSource(false);
		this.ghost.ghostAudio.PlayOrStopAppearSource(false);
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x0001E514 File Offset: 0x0001C714
	private Vector3 GetPositionOnNavMesh(Vector3 pos)
	{
		NavMeshHit navMeshHit;
		NavMesh.SamplePosition(pos, out navMeshHit, 2f, 1);
		return navMeshHit.position;
	}

	// Token: 0x04000506 RID: 1286
	private GhostAI ghost;

	// Token: 0x04000507 RID: 1287
	private GhostInteraction ghostInteraction;

	// Token: 0x04000508 RID: 1288
	private LevelRoom room;

	// Token: 0x04000509 RID: 1289
	private Player player;

	// Token: 0x0400050A RID: 1290
	private float timer = UnityEngine.Random.Range(5f, 15f);
}

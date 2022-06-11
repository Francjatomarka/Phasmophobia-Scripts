using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020000B7 RID: 183
public class GhostEvent_4 : IState
{
	// Token: 0x06000554 RID: 1364 RVA: 0x0001E537 File Offset: 0x0001C737
	public GhostEvent_4(GhostAI ghost, GhostInteraction ghostInteraction)
	{
		this.ghost = ghost;
		this.ghostInteraction = ghostInteraction;
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x0001E564 File Offset: 0x0001C764
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
		this.ghost.anim.SetBool("isIdle", true);
		GhostController.instance.ghostEventPlayer.SpawnPlayer(this.player, this.GetPositionOnNavMesh(vector));
		this.ghostInteraction.CreateAppearedEMF(this.ghost.transform.position);
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x0001E960 File Offset: 0x0001CB60
	public void Execute()
	{
		this.timer -= Time.deltaTime;
		RaycastHit raycastHit;
		if (Physics.Raycast(this.player.cam.transform.position, this.player.cam.transform.forward, out raycastHit, 2f, this.player.ghostRaycastMask) && raycastHit.collider.CompareTag("Ghost"))
		{
			GhostController.instance.ghostEventPlayer.Stop();
			GhostController.instance.ghostEventPlayer.screamSource.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(GhostController.instance.ghostEventPlayer.transform.position.y);
			GhostController.instance.ghostEventPlayer.screamSource.Play();
			this.player.ChangeSanity(-20);
			this.ghost.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (Vector3.Distance(GhostController.instance.ghostEventPlayer.transform.position, this.player.headObject.transform.position) < 2f)
		{
			GhostController.instance.ghostEventPlayer.Stop();
			GhostController.instance.ghostEventPlayer.screamSource.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(GhostController.instance.ghostEventPlayer.transform.position.y);
			GhostController.instance.ghostEventPlayer.screamSource.Play();
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

	// Token: 0x06000557 RID: 1367 RVA: 0x0001EB88 File Offset: 0x0001CD88
	public void Exit()
	{
		this.ghost.ghostAudio.StopSound();
		GhostController.instance.ghostEventPlayer.Stop();
		this.ghost.UnAppear(true);
		this.ghost.ghostAudio.TurnOnOrOffAppearSource(false);
		this.ghost.ghostAudio.PlayOrStopAppearSource(false);
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x0001EBE4 File Offset: 0x0001CDE4
	private Vector3 GetPositionOnNavMesh(Vector3 pos)
	{
		NavMeshHit navMeshHit;
		NavMesh.SamplePosition(pos, out navMeshHit, 2f, 1);
		return navMeshHit.position;
	}

	// Token: 0x0400050B RID: 1291
	private GhostAI ghost;

	// Token: 0x0400050C RID: 1292
	private GhostInteraction ghostInteraction;

	// Token: 0x0400050D RID: 1293
	private LevelRoom room;

	// Token: 0x0400050E RID: 1294
	private Player player;

	// Token: 0x0400050F RID: 1295
	private float timer = UnityEngine.Random.Range(5f, 15f);
}

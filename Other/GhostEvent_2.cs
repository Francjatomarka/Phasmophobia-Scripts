using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020000B5 RID: 181
public class GhostEvent_2 : IState
{
	// Token: 0x0600054A RID: 1354 RVA: 0x0001D883 File Offset: 0x0001BA83
	public GhostEvent_2(GhostAI ghost, GhostInteraction ghostInteraction)
	{
		this.ghost = ghost;
		this.ghostInteraction = ghostInteraction;
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x0001D8B0 File Offset: 0x0001BAB0
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
		if (Vector3.Distance(this.player.headObject.transform.position, this.ghost.raycastPoint.position) > 15f)
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
		for (int k = 0; k < this.room.doors.Length; k++)
		{
			if (!this.room.doors[k].photonInteract.isGrabbed)
			{
				Rigidbody component = this.room.doors[k].GetComponent<Rigidbody>();
				component.mass = 1f;
				component.isKinematic = false;
				component.AddTorque(new Vector3(0f, (component.GetComponent<HingeJoint>().limits.min == 0f) ? -6f : 6f, 0f), ForceMode.VelocityChange);
				this.ghost.StartCoroutine(this.ghost.ResetRigidbody(component, this.room.doors[k]));
				this.ghostInteraction.CreateDoorNoise(this.room.doors[k].transform.position);
			}
		}
		this.ghost.Appear(true);
		this.ghost.ghostAudio.TurnOnOrOffAppearSource(true);
		this.ghost.ghostAudio.PlayOrStopAppearSource(true);
		this.ghost.ghostInteraction.CreateAppearedEMF(this.ghost.raycastPoint.position);
	}

	// Token: 0x0600054C RID: 1356 RVA: 0x0001DDB8 File Offset: 0x0001BFB8
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
		if (Vector3.Distance(this.player.headObject.transform.position, this.ghost.raycastPoint.position) < 1.5f)
		{
			this.ghost.ghostAudio.PlaySound(1, false, false);
			this.player.ChangeSanity(-20);
			this.ghost.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (this.playerDistance > 5f)
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

	// Token: 0x0600054D RID: 1357 RVA: 0x0001DEF9 File Offset: 0x0001C0F9
	public void Exit()
	{
		this.ghost.ghostAudio.StopSound();
		this.ghost.UnAppear(true);
		this.ghost.ghostAudio.TurnOnOrOffAppearSource(false);
		this.ghost.ghostAudio.PlayOrStopAppearSource(false);
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x0001DF38 File Offset: 0x0001C138
	private Vector3 GetPositionOnNavMesh(Vector3 pos)
	{
		NavMeshHit navMeshHit;
		NavMesh.SamplePosition(pos, out navMeshHit, 2f, 1);
		return navMeshHit.position;
	}

	// Token: 0x04000500 RID: 1280
	private GhostAI ghost;

	// Token: 0x04000501 RID: 1281
	private GhostInteraction ghostInteraction;

	// Token: 0x04000502 RID: 1282
	private Player player;

	// Token: 0x04000503 RID: 1283
	private LevelRoom room;

	// Token: 0x04000504 RID: 1284
	private float playerDistance;

	// Token: 0x04000505 RID: 1285
	private float timer = UnityEngine.Random.Range(5f, 15f);
}

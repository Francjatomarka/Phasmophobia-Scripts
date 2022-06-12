using System;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class HuntingState : IState
{
	public HuntingState(GhostAI ghostAI, NavMeshAgent agent, PhotonView view)
	{
		this.ghostAI = ghostAI;
		this.agent = agent;
		this.view = view;
	}

	public void Enter()
	{
		this.agent.speed = this.ghostAI.defaultSpeed;
		this.ghostAI.anim.SetInteger("WalkType", 1);
		if (GameController.instance.levelDifficulty == Contract.LevelDifficulty.Intermediate)
		{
			this.timer = 35f;
		}
		else if (GameController.instance.levelDifficulty == Contract.LevelDifficulty.Professional)
		{
			this.timer = 50f;
		}
		for (int i = 0; i < LevelController.instance.exitDoors.Length; i++)
		{
			if (!LevelController.instance.exitDoors[i].GetComponent<PhotonView>().IsMine)
			{
				LevelController.instance.exitDoors[i].GetComponent<PhotonView>().RequestOwnership();
			}
		}
		if (this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Shade && LevelController.instance.currentGhostRoom.playersInRoom.Count > 1)
		{
			this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
			return;
		}
		if (this.ghostAI.playerToKill != null)
		{
			this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
			return;
		}
		if (SetupPhaseController.instance.isSetupPhase)
		{
			this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
			return;
		}
		if (!this.ghostAI.canAttack)
		{
			this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
			return;
		}
		bool flag = false;
		for (int j = 0; j < GameController.instance.playersData.Count; j++)
		{
			if (!GameController.instance.playersData[j].player.isDead && GameController.instance.playersData[j].player.currentRoom != LevelController.instance.outsideRoom)
			{
				flag = true;
			}
		}
		if (!flag)
		{
			this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
			return;
		}
		if (!this.ghostAI.canEnterHuntingMode)
		{
			this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
			return;
		}
		for (int k = 0; k < LevelController.instance.crucifix.Count; k++)
		{
			if (LevelController.instance.crucifix[k].gameObject.activeInHierarchy && SoundController.instance.GetFloorTypeFromPosition(LevelController.instance.crucifix[k].transform.position.y) == LevelController.instance.currentGhostRoom.floorType && Vector3.Distance(this.ghostAI.raycastPoint.transform.position, LevelController.instance.crucifix[k].transform.position) < ((this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Banshee) ? 6f : 4f))
			{
				LevelController.instance.crucifix[k].Used();
				if (MissionCrucifix.instance != null && !MissionCrucifix.instance.completed)
				{
					MissionCrucifix.instance.CompleteMission();
				}
				this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
				return;
			}
		}
		if (this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Banshee)
		{
			if (this.ghostAI.bansheeTarget == null)
			{
				this.ghostAI.SetNewBansheeTarget();
				this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
				return;
			}
			if (this.ghostAI.bansheeTarget.currentRoom == LevelController.instance.outsideRoom)
			{
				this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
				return;
			}
		}
		Vector3 zero = Vector3.zero;
		Vector3 destination = Vector3.zero;
		if (this.RandomNavSphere(out zero))
		{
			destination = zero;
			for (int l = 0; l < LevelController.instance.exitDoors.Length; l++)
			{
				LevelController.instance.exitDoors[l].DisableOrEnableDoor(false);
				LevelController.instance.exitDoors[l].DisableOrEnableCollider(false);
			}
			this.ghostAI.ghostInteraction.CreateAppearedEMF(this.ghostAI.transform.position);
			this.agent.SetDestination(destination);
			this.view.RPC("Hunting", RpcTarget.All, new object[]
			{
				true
			});
			this.ghostAI.anim.SetBool("isIdle", false);
			if (this.ghostAI.bansheeTarget != null)
			{
				this.targetPlayer = this.ghostAI.bansheeTarget;
				if (this.targetPlayer.isDead || this.targetPlayer.currentRoom == LevelController.instance.outsideRoom)
				{
					for (int m = 0; m < GameController.instance.playersData.Count; m++)
					{
						if (GameController.instance.playersData[m] != null && GameController.instance.playersData[m].player != null && !GameController.instance.playersData[m].player.isDead && !GameController.instance.playersData[m].player.currentRoom != LevelController.instance.outsideRoom)
						{
							this.targetPlayer = GameController.instance.playersData[m].player;
						}
					}
				}
			}
			else
			{
				for (int n = 0; n < GameController.instance.playersData.Count; n++)
				{
					if (GameController.instance.playersData[n] != null && GameController.instance.playersData[n].player != null && !GameController.instance.playersData[n].player.isDead && !GameController.instance.playersData[n].player.currentRoom != LevelController.instance.outsideRoom)
					{
						this.targetPlayer = GameController.instance.playersData[n].player;
					}
				}
				if (this.targetPlayer == null)
				{
					this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
					return;
				}
			}
			if (this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Revenant)
			{
				this.agent.speed = this.ghostAI.defaultSpeed / 1.5f;
			}
			for (int num = 0; num < LevelController.instance.exitDoors.Length; num++)
			{
				if (!LevelController.instance.exitDoors[num].closed)
				{
					Rigidbody component = LevelController.instance.exitDoors[num].GetComponent<Rigidbody>();
					component.mass = 1f;
					component.isKinematic = false;
					component.useGravity = true;
					component.AddTorque(new Vector3(0f, (component.GetComponent<HingeJoint>().limits.min == 0f) ? -1.25f : 1.25f, 0f), ForceMode.VelocityChange);
					this.ghostAI.StartCoroutine(this.ghostAI.ResetRigidbody(component, LevelController.instance.exitDoors[num]));
					LevelController.instance.exitDoors[num].LockDoor();
				}
			}
			return;
		}
		this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
	}

	public void Execute()
	{
		this.lockDoorDelay -= Time.deltaTime;
		if (this.lockDoorDelay < 0f)
		{
			for (int i = 0; i < LevelController.instance.exitDoors.Length; i++)
			{
				LevelController.instance.exitDoors[i].transform.localRotation = Quaternion.identity;
				Quaternion localRotation = LevelController.instance.exitDoors[i].transform.localRotation;
				Vector3 eulerAngles = localRotation.eulerAngles;
				eulerAngles.y = LevelController.instance.exitDoors[i].closedYRot;
				localRotation.eulerAngles = eulerAngles;
				LevelController.instance.exitDoors[i].transform.localRotation = localRotation;
				LevelController.instance.exitDoors[i].LockDoor();
				LevelController.instance.exitDoors[i].GetComponent<Rigidbody>().isKinematic = true;
				LevelController.instance.exitDoors[i].GetComponent<Rigidbody>().useGravity = false;
				LevelController.instance.exitDoors[i].DisableOrEnableCollider(true);
			}
			this.lockDoorDelay = 100f;
		}
		if (!this.canHunt)
		{
			this.delayTimer -= Time.deltaTime;
			if (this.delayTimer < 0f)
			{
				this.ghostAI.UnAppear(true);
				this.ghostAI.ghostAudio.TurnOnOrOffAppearSource(true);
				this.ghostAI.ghostAudio.PlayOrStopAppearSource(true);
				this.ghostAI.ghostInteraction.StepTimer = 0f;
				this.canHunt = true;
			}
			return;
		}
		this.timer -= Time.deltaTime;
		if (this.timer < 0f)
		{
			this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
			return;
		}
		if (this.ghostAI.bansheeTarget == null && GameController.instance.playersData.Count > 1)
		{
			for (int j = 0; j < GameController.instance.playersData.Count; j++)
			{
				if (GameController.instance.playersData[j] != null && GameController.instance.playersData[j].player != null && GameController.instance.playersData[j].player.currentRoom != LevelController.instance.outsideRoom && GameController.instance.playersData[j].player != this.targetPlayer && !GameController.instance.playersData[j].player.isDead && Vector3.Distance(this.ghostAI.raycastPoint.position, GameController.instance.playersData[j].player.transform.position) < Vector3.Distance(this.ghostAI.raycastPoint.position, this.targetPlayer.transform.position))
				{
					this.targetPlayer = GameController.instance.playersData[j].player;
				}
			}
		}
		if (this.targetPlayer == null)
		{
			this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
			return;
		}
		if (this.ghostAI.smudgeSticksUsed)
		{
			if ((double)Vector3.Distance(this.ghostAI.transform.position, this.agent.destination) < 1.5 || this.agent.pathStatus == NavMeshPathStatus.PathInvalid)
			{
				Vector3 zero = Vector3.zero;
				if (this.RandomNavSphere(out zero))
				{
					this.agent.SetDestination(zero);
					return;
				}
				this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
				return;
			}
		}
		else if (this.player == null)
		{
			if (this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Revenant)
			{
				this.agent.speed = this.ghostAI.defaultSpeed / 1.5f;
			}
			if (!Physics.Linecast(this.ghostAI.raycastPoint.position, this.targetPlayer.headObject.transform.position, this.ghostAI.mask, QueryTriggerInteraction.Ignore))
			{
				if (!Physics.Linecast(this.ghostAI.raycastPoint.position + Vector3.right / 6f, this.targetPlayer.headObject.transform.position, this.ghostAI.mask, QueryTriggerInteraction.Ignore) && !this.targetPlayer.isDead && this.targetPlayer.currentRoom != LevelController.instance.outsideRoom)
				{
					this.player = this.targetPlayer;
					this.player.beingHunted = true;
					this.ghostAI.ChasingPlayer(true);
					return;
				}
			}
			else if (this.ghostAI.bansheeTarget == null && GameController.instance.playersData.Count > 1)
			{
				for (int k = 0; k < GameController.instance.playersData.Count; k++)
				{
					if (GameController.instance.playersData[k] != null && GameController.instance.playersData[k].player != null && !GameController.instance.playersData[k].player.isDead && !GameController.instance.playersData[k].player.currentRoom != LevelController.instance.outsideRoom && !Physics.Linecast(this.ghostAI.raycastPoint.position, GameController.instance.playersData[k].player.headObject.transform.position, this.ghostAI.mask, QueryTriggerInteraction.Ignore) && !Physics.Linecast(this.ghostAI.raycastPoint.position + Vector3.right / 10f, GameController.instance.playersData[k].player.headObject.transform.position, this.ghostAI.mask, QueryTriggerInteraction.Ignore))
					{
						this.player = GameController.instance.playersData[k].player;
						this.player.beingHunted = true;
						this.ghostAI.ChasingPlayer(true);
						return;
					}
				}
			}
			if ((double)Vector3.Distance(this.ghostAI.transform.position, this.agent.destination) < 1.5 || this.agent.pathStatus == NavMeshPathStatus.PathInvalid)
			{
				Vector3 zero2 = Vector3.zero;
				if (this.RandomNavSphere(out zero2))
				{
					this.agent.SetDestination(zero2);
					return;
				}
				this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
				return;
			}
		}
		else if (this.player != null)
		{
			if (this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Revenant)
			{
				this.agent.speed = this.ghostAI.defaultSpeed * 2f;
			}
			if (this.agent.pathStatus == NavMeshPathStatus.PathInvalid || Physics.Linecast(this.ghostAI.raycastPoint.position, this.player.headObject.transform.position, this.ghostAI.mask, QueryTriggerInteraction.Ignore))
			{
				this.player.beingHunted = false;
				this.ghostAI.ChasingPlayer(false);
				this.player = null;
				Vector3 zero3 = Vector3.zero;
				if (this.RandomNavSphere(out zero3))
				{
					this.agent.SetDestination(zero3);
					return;
				}
				this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
				return;
			}
			else if (this.player.currentRoom == LevelController.instance.outsideRoom)
			{
				this.player.beingHunted = false;
				this.ghostAI.ChasingPlayer(false);
				this.player = null;
				Vector3 zero4 = Vector3.zero;
				if (this.RandomNavSphere(out zero4))
				{
					this.agent.SetDestination(zero4);
					return;
				}
				this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
				return;
			}
			else
			{
				this.agent.SetDestination(this.player.aiTargetPoint.position);
				if (Vector3.Distance(this.ghostAI.raycastPoint.transform.position, this.player.headObject.transform.position) < 1f)
				{
					this.agent.SetDestination(this.ghostAI.transform.position);
					this.ghostAI.playerToKill = this.player;
					this.ghostAI.ChangeState(GhostAI.States.killPlayer, null, null);
					this.player.beingHunted = false;
					this.ghostAI.ChasingPlayer(false);
					this.player = null;
					if (this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Banshee && this.player == this.ghostAI.bansheeTarget)
					{
						this.ghostAI.SetNewBansheeTarget();
					}
					return;
				}
				if (this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Jinn)
				{
					if (Vector3.Distance(this.ghostAI.raycastPoint.transform.position, this.player.headObject.transform.position) > 4f)
					{
						if (LevelController.instance.fuseBox.isOn)
						{
							this.agent.speed = 2f;
							return;
						}
					}
					else
					{
						this.agent.speed = this.ghostAI.defaultSpeed;
					}
				}
			}
		}
	}

	public void Exit()
	{
		this.ghostAI.anim.SetInteger("WalkType", 0);
		this.agent.speed = this.ghostAI.defaultSpeed;
		this.ghostAI.ghostAudio.StopSound();
		this.view.RPC("Hunting", RpcTarget.All, new object[]
		{
			false
		});
		for (int i = 0; i < LevelController.instance.exitDoors.Length; i++)
		{
			LevelController.instance.exitDoors[i].DisableOrEnableDoor(true);
		}
	}

	private bool RandomNavSphere(out Vector3 hitPos)
	{
		float num = UnityEngine.Random.Range(2f, 15f);
		NavMeshHit navMeshHit;
		if (NavMesh.SamplePosition(UnityEngine.Random.insideUnitSphere * num + this.ghostAI.transform.position, out navMeshHit, num, 1))
		{
			hitPos = navMeshHit.position;
			return true;
		}
		hitPos = Vector3.zero;
		return false;
	}

	private Vector3 GetPositionOnNavMesh(Vector3 pos)
	{
		NavMeshHit navMeshHit;
		NavMesh.SamplePosition(pos, out navMeshHit, 2f, 1);
		return navMeshHit.position;
	}

	private GhostAI ghostAI;

	private NavMeshAgent agent;

	private PhotonView view;

	private float timer = 25f;

	private Player player;

	private Player targetPlayer;

	private bool canHunt;

	private float delayTimer = 8f;

	private float lockDoorDelay = 1f;
}


using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x020000BB RID: 187
public class IdleState : IState
{
	// Token: 0x06000569 RID: 1385 RVA: 0x0001FFAB File Offset: 0x0001E1AB
	public IdleState(GhostAI ghostAI)
	{
		this.ghostAI = ghostAI;
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x0001FFC4 File Offset: 0x0001E1C4
	public void Enter()
	{
		this.idleTimer = UnityEngine.Random.Range(2f, 6f);
		this.ghostAI.anim.SetInteger("IdleNumber", UnityEngine.Random.Range(0, 2));
		this.ghostAI.anim.SetBool("isIdle", true);
		this.ghostAI.UnAppear(true);
		this.ghostAI.ghostAudio.TurnOnOrOffAppearSource(false);
		this.ghostAI.ghostAudio.PlayOrStopAppearSource(false);
		if (GameController.instance.levelDifficulty == Contract.LevelDifficulty.Amateur)
		{
			this.maxRandomAbilityValue = 100;
		}
		else if (GameController.instance.levelDifficulty == Contract.LevelDifficulty.Intermediate)
		{
			this.maxRandomAbilityValue = 115;
		}
		else if (GameController.instance.levelDifficulty == Contract.LevelDifficulty.Professional)
		{
			this.maxRandomAbilityValue = 130;
		}
		if (this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Oni && LevelController.instance.currentGhostRoom.playersInRoom.Count > 0)
		{
			this.OniMultiplier = 30;
		}
		if (this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Mare)
		{
			if (LevelController.instance.currentGhostRoom.lightSwitches.Count > 0)
			{
				if (LevelController.instance.currentGhostRoom.lightSwitches[0].isOn)
				{
					this.huntingMultiplier -= 10;
				}
				else
				{
					this.huntingMultiplier += 10;
				}
			}
			else
			{
				this.huntingMultiplier += 10;
			}
		}
		if (!SetupPhaseController.instance.mainDoorHasUnlocked)
		{
			this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
			return;
		}
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x0002015C File Offset: 0x0001E35C
	public void Execute()
	{
		this.idleTimer -= Time.deltaTime;
		if (this.idleTimer < 0f)
		{
			this.ghostAI.anim.SetBool("isIdle", false);
			int num = (int)GameController.instance.GetAveragePlayerInsanity();
			this.huntingMultiplier += ((this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Demon) ? 15 : 0);
			if (UnityEngine.Random.Range(0, 2) == 1 && !GameController.instance.isTutorial && this.ghostAI.canEnterHuntingMode && this.ghostAI.canAttack)
			{
				if (num + this.huntingMultiplier >= 50 && num + this.huntingMultiplier < 75)
				{
					if (UnityEngine.Random.Range(0, 5) == 1)
					{
						this.ghostAI.ChangeState(GhostAI.States.hunting, null, null);
						return;
					}
				}
				else if (num + this.huntingMultiplier >= 75 && UnityEngine.Random.Range(0, 3) == 1)
				{
					this.ghostAI.ChangeState(GhostAI.States.hunting, null, null);
					return;
				}
			}
			float num2 = Mathf.Clamp((float)num + this.ghostAI.ghostInfo.activityMultiplier + (float)this.OniMultiplier + (float)((PhotonNetwork.PlayerList.Length == 1) ? 15 : 0), 0f, 100f);
			if ((float)UnityEngine.Random.Range(0, this.maxRandomAbilityValue) <= num2 && UnityEngine.Random.Range(0, 2) == 1)
			{
				int num3 = UnityEngine.Random.Range(0, 11);
				if (num3 == 0 || num3 == 1 || num3 == 2 || num3 == 3 || num3 == 4)
				{
					this.ghostAI.ghostActivity.Interact();
					return;
				}
				if (num3 == 5 || num3 == 6 || num3 == 7 || num3 == 8)
				{
					this.ghostAI.ghostActivity.GhostAbility();
					return;
				}
				if (UnityEngine.Random.Range(0, 3) == 1)
				{
					this.ghostAI.ChangeState(GhostAI.States.wander, null, null);
					return;
				}
				this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
				return;
			}
			else
			{
				if (UnityEngine.Random.Range(0, 5) == 1)
				{
					this.ghostAI.ChangeState(GhostAI.States.wander, null, null);
					return;
				}
				if (UnityEngine.Random.Range(0, 5) == 1)
				{
					this.ghostAI.ghostActivity.Interact();
					return;
				}
				this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
			}
		}
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Exit()
	{
	}

	// Token: 0x0400051F RID: 1311
	private float idleTimer;

	// Token: 0x04000520 RID: 1312
	private GhostAI ghostAI;

	// Token: 0x04000521 RID: 1313
	private int maxRandomAbilityValue = 100;

	// Token: 0x04000522 RID: 1314
	private int OniMultiplier;

	// Token: 0x04000523 RID: 1315
	private int huntingMultiplier;
}

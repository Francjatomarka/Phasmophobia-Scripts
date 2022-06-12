using System;
using UnityEngine;
using UnityEngine.AI;

public class AppearState : IState
{
	public AppearState(GhostAI ghostAI, NavMeshAgent agent)
	{
		this.ghostAI = ghostAI;
		this.agent = agent;
	}

	public void Enter()
	{
		this.ghostAI.Appear(false);
		this.ghostAI.ghostInteraction.CreateAppearedEMF(this.ghostAI.transform.position);
		this.ghostAI.anim.SetBool("isIdle", false);
		if (LevelController.instance.currentGhostRoom == null)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		for (int i = 0; i < LevelController.instance.currentGhostRoom.playersInRoom.Count; i++)
		{
			if (LevelController.instance.currentGhostRoom.playersInRoom[i] == null)
			{
				LevelController.instance.currentGhostRoom.playersInRoom.RemoveAt(i);
				this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
				return;
			}
		}
		if (LevelController.instance.currentGhostRoom.playersInRoom.Count == 0)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		for (int j = 0; j < LevelController.instance.currentGhostRoom.playersInRoom.Count; j++)
		{
			if (LevelController.instance.currentGhostRoom.playersInRoom[j] != null)
			{
				this.target = LevelController.instance.currentGhostRoom.playersInRoom[0].transform.root.GetComponent<Player>().headObject.transform;
				break;
			}
		}
		if (this.target == null)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (Vector3.Distance(this.target.position, this.ghostAI.raycastPoint.position) < 2.5f)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		for (int k = 0; k < LevelController.instance.currentGhostRoom.lightSwitches.Count; k++)
		{
			LevelController.instance.currentGhostRoom.lightSwitches[k].TurnOff();
			if (LevelController.instance.currentGhostRoom.lightSwitches[k].lever != null)
			{
				this.mainLight = LevelController.instance.currentGhostRoom.lightSwitches[k];
			}
		}
	}

	public void Execute()
	{
		this.appearTimer -= Time.deltaTime;
		if (this.appearTimer < 0f)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (this.mainLight == null || LevelController.instance.currentGhostRoom == null || this.target == null)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (this.mainLight.isOn || LevelController.instance.currentGhostRoom.playersInRoom.Count == 0)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		this.agent.SetDestination(this.target.position);
		if (Vector3.Distance(this.ghostAI.transform.position, this.agent.destination) < 1.5f)
		{
			this.target.root.gameObject.GetComponent<Player>().ChangeSanity(-25);
			this.ghostAI.ghostAudio.PlaySound(1, false, false);
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		}
	}

	public void Exit()
	{
		this.ghostAI.UnAppear(false);
	}

	private GhostAI ghostAI;

	private NavMeshAgent agent;

	private LightSwitch mainLight;

	private Transform target;

	private float appearTimer = 2.5f;
}


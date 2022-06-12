using System;
using UnityEngine;

public class LightState : IState
{
	public LightState(GhostAI ghostAI, GhostInteraction ghostInteraction, PhotonObjectInteract obj)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.lightSwitch = obj.GetComponent<LightSwitch>();
	}

	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		if (this.lightSwitch.isOn)
		{
			this.lightSwitch.UseLight();
		}
		else if (UnityEngine.Random.Range(0, 2) == 1)
		{
			this.lightSwitch.UseLight();
		}
		if (EvidenceController.instance.IsFingerPrintEvidence())
		{
			this.lightSwitch.SpawnHandPrintEvidence();
		}
		this.ghostInteraction.CreateInteractionEMF(this.lightSwitch.transform.position);
	}

	public void Execute()
	{
	}

	public void Exit()
	{
	}

	private GhostAI ghostAI;

	private GhostInteraction ghostInteraction;

	private LightSwitch lightSwitch;
}


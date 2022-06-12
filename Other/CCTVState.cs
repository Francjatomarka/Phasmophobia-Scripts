using System;
using Photon.Pun;

public class CCTVState : IState
{
	public CCTVState(GhostAI ghostAI, GhostInteraction ghostInteraction, PhotonObjectInteract obj)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.cam = obj.GetComponent<CCTV>();
	}

	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		if (LevelController.instance.fuseBox.isOn)
		{
			for (int i = 0; i < LevelController.instance.currentGhostRoom.lightSwitches.Count; i++)
			{
				if (LevelController.instance.currentGhostRoom.lightSwitches[i].isOn)
				{
					return;
				}
			}
		}
		if (!this.cam.GetComponent<PhotonView>().IsMine)
		{
			this.cam.GetComponent<PhotonView>().RequestOwnership();
		}
		this.ghostInteraction.CreateInteractionEMF(this.cam.transform.position);
		this.cam.TurnOff();
	}

	public void Execute()
	{
	}

	public void Exit()
	{
	}

	private GhostAI ghostAI;

	private GhostInteraction ghostInteraction;

	private CCTV cam;
}


using System;
using UnityEngine;
using Photon.Pun;

public class PoltergeistPower : IState
{
	public PoltergeistPower(GhostAI ghostAI, GhostInteraction ghostInteraction, LayerMask mask, PhotonObjectInteract[] props)
	{
		this.ghostInteraction = ghostInteraction;
		this.mask = mask;
		this.ghostAI = ghostAI;
		this.props = props;
	}

	public void Enter()
	{
		if (this.props.Length == 0)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		for (int i = 0; i < this.props.Length; i++)
		{
			if (this.props[i] != null)
			{
				if (!this.props[i].GetComponent<PhotonView>().IsMine)
				{
					this.props[i].GetComponent<PhotonView>().RequestOwnership();
				}
				this.props[i].GetComponent<Rigidbody>().AddForce(new Vector3(UnityEngine.Random.Range(-4f, 4f), UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-4f, 4f)), ForceMode.Impulse);
				this.ghostInteraction.CreateThrowingEMF(this.props[i].transform.position);
			}
		}
		Vector3 vector = GameController.instance.myPlayer.player.cam.WorldToViewportPoint(this.ghostInteraction.transform.position);
		if (vector.x > 0f && vector.x < 1f && vector.y > 0f && vector.y < 1f && !Physics.Linecast(this.ghostInteraction.transform.position, GameController.instance.myPlayer.player.cam.transform.position, this.mask))
		{
			GameController.instance.myPlayer.player.insanity += (float)this.props.Length * 2f;
		}
		this.ghostInteraction.CreateInteractionEMF(this.ghostAI.raycastPoint.position);
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
	}

	public void Execute()
	{
	}

	public void Exit()
	{
	}

	private GhostInteraction ghostInteraction;

	private LayerMask mask;

	private GhostAI ghostAI;

	private PhotonObjectInteract[] props;
}


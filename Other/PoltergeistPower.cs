using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x020000E6 RID: 230
public class PoltergeistPower : IState
{
	// Token: 0x0600064B RID: 1611 RVA: 0x00022EAE File Offset: 0x000210AE
	public PoltergeistPower(GhostAI ghostAI, GhostInteraction ghostInteraction, LayerMask mask, PhotonObjectInteract[] props)
	{
		this.ghostInteraction = ghostInteraction;
		this.mask = mask;
		this.ghostAI = ghostAI;
		this.props = props;
	}

	// Token: 0x0600064C RID: 1612 RVA: 0x00022ED4 File Offset: 0x000210D4
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

	// Token: 0x0600064D RID: 1613 RVA: 0x00003F60 File Offset: 0x00002160
	public void Execute()
	{
	}

	// Token: 0x0600064E RID: 1614 RVA: 0x00003F60 File Offset: 0x00002160
	public void Exit()
	{
	}

	// Token: 0x0400062B RID: 1579
	private GhostInteraction ghostInteraction;

	// Token: 0x0400062C RID: 1580
	private LayerMask mask;

	// Token: 0x0400062D RID: 1581
	private GhostAI ghostAI;

	// Token: 0x0400062E RID: 1582
	private PhotonObjectInteract[] props;
}

using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x020000C5 RID: 197
public class ThrowingState : IState
{
	// Token: 0x06000591 RID: 1425 RVA: 0x00020934 File Offset: 0x0001EB34
	public ThrowingState(GhostAI ghostAI, GhostInteraction ghostInteraction, PhotonObjectInteract prop)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.prop = prop;
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x00020954 File Offset: 0x0001EB54
	public void Enter()
	{
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
		if (this.prop == null)
		{
			return;
		}
		if (LevelController.instance.currentGhostRoom == LevelController.instance.outsideRoom)
		{
			return;
		}
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
		if (!this.prop.GetComponent<PhotonView>().IsMine)
		{
			this.prop.GetComponent<PhotonView>().RequestOwnership();
		}
		if (this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Poltergeist)
		{
			this.prop.GetComponent<Rigidbody>().AddForce(new Vector3(UnityEngine.Random.Range(-4f, 4f), UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-2.5f, 2.5f)), ForceMode.Impulse);
		}
		else if (this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Poltergeist)
		{
			this.prop.GetComponent<Rigidbody>().AddForce(new Vector3(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-2.5f, 2.5f), UnityEngine.Random.Range(-3f, 3f)), ForceMode.Impulse);
		}
		else
		{
			this.prop.GetComponent<Rigidbody>().AddForce(new Vector3(UnityEngine.Random.Range(-2.5f, 2.5f), UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-2.5f, 2.5f)), ForceMode.Impulse);
		}
		this.ghostInteraction.CreateThrowingEMF(this.prop.transform.position);
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Execute()
	{
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Exit()
	{
	}

	// Token: 0x0400053E RID: 1342
	private GhostAI ghostAI;

	// Token: 0x0400053F RID: 1343
	private GhostInteraction ghostInteraction;

	// Token: 0x04000540 RID: 1344
	private PhotonObjectInteract prop;
}

using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x020000B3 RID: 179
public class DoorState : IState
{
	// Token: 0x06000541 RID: 1345 RVA: 0x0001CF81 File Offset: 0x0001B181
	public DoorState(GhostAI ghostAI, GhostInteraction ghostInteraction, GhostInfo ghostInfo, PhotonObjectInteract obj)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.ghostInfo = ghostInfo;
		this.door = obj.GetComponent<Door>();
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x0001CFAC File Offset: 0x0001B1AC
	public void Enter()
	{
		if (EvidenceController.instance.IsFingerPrintEvidence())
		{
			this.door.SpawnHandPrintEvidence();
		}
		if (!this.door.GetComponent<PhotonView>().IsMine)
		{
			this.door.GetComponent<PhotonView>().RequestOwnership();
		}
		Rigidbody component = this.door.GetComponent<Rigidbody>();
		component.mass = 1f;
		component.isKinematic = false;
		component.AddTorque(new Vector3(0f, UnityEngine.Random.Range(-1f, 1f), 0f), ForceMode.Impulse);
		this.ghostAI.StartCoroutine(this.ghostAI.ResetRigidbody(component, this.door));
		this.ghostInteraction.CreateDoorNoise(this.door.transform.position);
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Execute()
	{
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Exit()
	{
	}

	// Token: 0x040004F7 RID: 1271
	private GhostAI ghostAI;

	// Token: 0x040004F8 RID: 1272
	private GhostInteraction ghostInteraction;

	// Token: 0x040004F9 RID: 1273
	private GhostInfo ghostInfo;

	// Token: 0x040004FA RID: 1274
	private Door door;
}

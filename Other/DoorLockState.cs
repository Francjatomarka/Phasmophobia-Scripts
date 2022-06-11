using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x020000B2 RID: 178
public class DoorLockState : IState
{
	// Token: 0x0600053D RID: 1341 RVA: 0x0001CE28 File Offset: 0x0001B028
	public DoorLockState(GhostAI ghostAI, GhostInteraction ghostInteraction, PhotonObjectInteract obj)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
		this.door = obj.GetComponent<Door>();
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x0001CE4C File Offset: 0x0001B04C
	public void Enter()
	{
		if (this.door.GetComponent<Door>().type == Key.KeyType.none || this.door.type == Key.KeyType.Car)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (this.door.GetComponent<PhotonObjectInteract>().isGrabbed)
		{
			return;
		}
		if (!this.door.GetComponent<PhotonView>().IsMine)
		{
			this.door.GetComponent<PhotonView>().RequestOwnership();
		}
		if (!this.door.closed)
		{
			Rigidbody component = this.door.GetComponent<Rigidbody>();
			component.mass = 1f;
			component.isKinematic = false;
			component.useGravity = true;
			component.AddTorque(new Vector3(0f, (component.GetComponent<HingeJoint>().limits.min == 0f) ? -1.25f : 1.25f, 0f), ForceMode.VelocityChange);
			this.ghostAI.StartCoroutine(this.ghostAI.ResetRigidbody(component, this.door));
			this.ghostInteraction.CreateDoorNoise(this.door.transform.position);
		}
		this.door.LockDoor();
		this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
	}

	// Token: 0x0600053F RID: 1343 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Execute()
	{
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Exit()
	{
	}

	// Token: 0x040004F4 RID: 1268
	private GhostAI ghostAI;

	// Token: 0x040004F5 RID: 1269
	private GhostInteraction ghostInteraction;

	// Token: 0x040004F6 RID: 1270
	private Door door;
}

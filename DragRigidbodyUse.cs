using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

// Token: 0x02000160 RID: 352
public class DragRigidbodyUse : MonoBehaviour
{
	// Token: 0x060009F5 RID: 2549 RVA: 0x0003C6EC File Offset: 0x0003A8EC
	private void Awake()
	{
		this.isObjectHeld = false;
		this.tryPickupObject = false;
		this.objectHeld = null;
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x0003C703 File Offset: 0x0003A903
	private void FixedUpdate()
	{
		if (!this.interactKeyIsPressed)
		{
			if (this.isObjectHeld)
			{
				this.DropObject();
			}
			return;
		}
		if (!this.isObjectHeld)
		{
			this.TryPickObject();
			this.tryPickupObject = true;
			return;
		}
		this.HoldObject();
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x0003C738 File Offset: 0x0003A938
	private void TryOpenDoor()
	{
		if (this.player.isDead)
		{
			return;
		}
		this.playerAim = this.playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		RaycastHit raycastHit;
		if (Physics.Raycast(this.playerAim, out raycastHit, this.maxDistanceGrab, this.mask) && raycastHit.collider.CompareTag("Door") && raycastHit.collider.GetComponent<Door>() && raycastHit.collider.GetComponent<Door>().locked)
		{
			if (!LevelController.instance.currentGhost.isHunting)
			{
				for (int i = 0; i < this.player.keys.Count; i++)
				{
					if (this.player.keys[i] == raycastHit.collider.GetComponent<Door>().type)
					{
						raycastHit.collider.GetComponent<Door>().UnlockDoor();
					}
				}
			}
			if (raycastHit.collider.GetComponent<Door>().locked)
			{
				raycastHit.collider.GetComponent<Door>().PlayLockedSound();
			}
		}
	}

	// Token: 0x060009F8 RID: 2552 RVA: 0x0003C864 File Offset: 0x0003AA64
	private void TryPickObject()
	{
		if (this.player.isDead)
		{
			return;
		}
		Ray ray = this.player.cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
		RaycastHit hit;
		Physics.Raycast(ray, out hit);
		if (this.tryPickupObject && hit.transform.gameObject.CompareTag("Door"))
		{
			this.objectHeld = hit.collider.gameObject;
			if (this.objectHeld.GetComponent<Door>())
			{
				if (this.objectHeld.GetComponent<PhotonObjectInteract>().isGrabbed)
				{
					return;
				}
				if (this.objectHeld.GetComponent<Door>().locked || !this.objectHeld.GetComponent<Door>().canBeGrabbed)
				{
					return;
				}
				this.objectHeld.GetComponent<Door>().GrabbedDoor();
			}
			else if (this.objectHeld.GetComponent<Drawer>())
			{
				this.objectHeld.GetComponent<Drawer>().Grab();
			}
			if (PhotonNetwork.InRoom)
			{
				this.objectHeld.GetComponent<PhotonView>().RequestOwnership();
			}
			this.isObjectHeld = true;
			this.wasKinematic = this.objectHeld.GetComponent<Rigidbody>().isKinematic;
			this.wasGravity = this.objectHeld.GetComponent<Rigidbody>().useGravity;
			this.objectHeld.GetComponent<Rigidbody>().useGravity = true;
			this.objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
			this.objectHeld.GetComponent<Rigidbody>().isKinematic = false;
		}
	}

	// Token: 0x060009F9 RID: 2553 RVA: 0x0003C9F8 File Offset: 0x0003ABF8
	private void HoldObject()
	{
		this.playerAim = this.playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		Vector3 a = this.playerCam.transform.position + this.playerAim.direction * this.distance;
		Vector3 position = this.objectHeld.transform.position;
		this.objectHeld.GetComponent<Rigidbody>().velocity = (a - position) * 10f;
		if (Vector3.Distance(this.objectHeld.transform.position, this.playerCam.transform.position) > this.maxDistanceGrab)
		{
			this.DropObject();
		}
	}

	// Token: 0x060009FA RID: 2554 RVA: 0x0003CABC File Offset: 0x0003ACBC
	public void DropObject()
	{
		this.isObjectHeld = false;
		this.tryPickupObject = false;
		this.objectHeld.GetComponent<Rigidbody>().useGravity = this.wasGravity;
		this.objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
		this.objectHeld.GetComponent<Rigidbody>().isKinematic = this.wasKinematic;
		if (this.objectHeld.GetComponent<Door>())
		{
			this.objectHeld.GetComponent<Door>().UnGrabbedDoor();
		}
		else if (this.objectHeld.GetComponent<Drawer>())
		{
			this.objectHeld.GetComponent<Drawer>().UnGrab();
		}
		this.objectHeld = null;
	}

	// Token: 0x060009FB RID: 2555 RVA: 0x0003CB64 File Offset: 0x0003AD64
	public void OnInteract(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started)
		{
			/*if (this.player.pcCanvas.isPaused)
			{
				return;
			}
			if (LevelController.instance && LevelController.instance.journalController.isOpen)
			{
				return;
			}*/
			this.interactKeyIsPressed = true;
		}
		if (context.phase == InputActionPhase.Performed)
		{
			/*if (this.player.pcCanvas.isPaused)
			{
				return;
			}
			if (LevelController.instance && LevelController.instance.journalController.isOpen)
			{
				return;
			}*/
			this.TryOpenDoor();
		}
		if (context.phase == InputActionPhase.Canceled)
		{
			this.interactKeyIsPressed = false;
			if (this.isObjectHeld)
			{
				this.DropObject();
			}
			this.tryPickupObject = false;
		}
	}

	// Token: 0x04000A1E RID: 2590
	[SerializeField]
	private Camera playerCam;

	// Token: 0x04000A1F RID: 2591
	private float distance = 1f;

	// Token: 0x04000A20 RID: 2592
	private float maxDistanceGrab = 3f;

	// Token: 0x04000A21 RID: 2593
	private Ray playerAim;

	// Token: 0x04000A22 RID: 2594
	[HideInInspector]
	public GameObject objectHeld;

	// Token: 0x04000A23 RID: 2595
	private bool isObjectHeld;

	// Token: 0x04000A24 RID: 2596
	private bool tryPickupObject;

	// Token: 0x04000A25 RID: 2597
	private bool wasKinematic;

	// Token: 0x04000A26 RID: 2598
	private bool wasGravity;

	// Token: 0x04000A27 RID: 2599
	[SerializeField]
	private LayerMask mask;

	// Token: 0x04000A28 RID: 2600
	[SerializeField]
	private Player player;

	// Token: 0x04000A29 RID: 2601
	private bool interactKeyIsPressed;
}

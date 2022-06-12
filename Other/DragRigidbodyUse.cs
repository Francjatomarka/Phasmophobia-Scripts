using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class DragRigidbodyUse : MonoBehaviour
{
	private void Awake()
	{
		this.isObjectHeld = false;
		this.tryPickupObject = false;
		this.objectHeld = null;
	}

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

	[SerializeField]
	private Camera playerCam;

	private float distance = 1f;

	private float maxDistanceGrab = 3f;

	private Ray playerAim;

	[HideInInspector]
	public GameObject objectHeld;

	private bool isObjectHeld;

	private bool tryPickupObject;

	private bool wasKinematic;

	private bool wasGravity;

	[SerializeField]
	private LayerMask mask;

	[SerializeField]
	private Player player;

	private bool interactKeyIsPressed;
}


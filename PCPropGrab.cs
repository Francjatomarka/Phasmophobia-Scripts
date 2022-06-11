using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PCPropGrab : MonoBehaviour
{
	private float grabDistance = 1.6f;

	private Ray playerAim;

	private PhotonView view;

	[SerializeField]
	private Player player;

	[SerializeField]
	private PCCanvas pcCanvas;

	[SerializeField]
	private PCFlashlight pcFlashlight;

	public List<PhotonObjectInteract> inventoryProps = new List<PhotonObjectInteract>();

	[HideInInspector]
	public int inventoryIndex;

	[SerializeField]
	private Camera playerCam;

	[SerializeField]
	private LayerMask mask;

	[HideInInspector]
	public FixedJoint grabSpotJoint;

	public Transform cameraItemSpot;

	private void Awake()
	{
		view = GetComponent<PhotonView>();
	}

	private void Update()
	{
		playerAim = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		RaycastHit hitInfo;
		if (Physics.Raycast(playerAim, out hitInfo, 3f, mask, QueryTriggerInteraction.Ignore) && (bool)hitInfo.collider.GetComponent<Door>())
		{
			if (hitInfo.collider.GetComponent<Door>().locked)
			{
				pcCanvas.SetState(PCCanvas.State.locked, false);
			}
			else
			{
				pcCanvas.SetState(PCCanvas.State.active, false);
			}
		}
		else if (Physics.Raycast(playerAim, out hitInfo, grabDistance, mask, QueryTriggerInteraction.Ignore))
		{
			if ((bool)hitInfo.collider.GetComponent<PhotonObjectInteract>())
			{
				if (hitInfo.collider.GetComponent<PhotonObjectInteract>().isProp)
				{
					pcCanvas.SetState(PCCanvas.State.active, false);
				}
				else if (!hitInfo.collider.GetComponent<PhotonObjectInteract>().isProp || hitInfo.collider.GetComponent<PhotonObjectInteract>().isGrabbed)
				{
					if ((bool)hitInfo.collider.GetComponent<LightSwitch>())
					{
						pcCanvas.SetState(PCCanvas.State.light, false);
					}
					else
					{
						pcCanvas.SetState(PCCanvas.State.active, false);
					}
				}
				else
				{
					pcCanvas.SetState(PCCanvas.State.none, false);
				}
			}
			else if (hitInfo.collider.CompareTag("MainMenuUI"))
			{
				pcCanvas.SetState(PCCanvas.State.active, false);
			}
			else
			{
				pcCanvas.SetState(PCCanvas.State.none, false);
			}
		}
		else
		{
			pcCanvas.SetState(PCCanvas.State.none, false);
		}
	}

	public void ControlSchemeChanged()
	{
		if (player.playerInput.currentControlScheme != "Keyboard")
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	public void ChangeItemSpotWithFOV(float fov)
	{
		float num = 90f - fov;
		num /= 500f;
		num += 0.124f;
		cameraItemSpot.localPosition = new Vector3(cameraItemSpot.localPosition.x, cameraItemSpot.localPosition.y, num);
	}

	private void AttemptGrab()
	{
		if (player.isDead)
		{
			return;
		}
		bool flag = false;
		int index = 0;
		for (int i = 0; i < inventoryProps.Count; i++)
		{
			if (inventoryProps[i] == null)
			{
				flag = true;
				index = i;
				break;
			}
		}
		if (inventoryProps[inventoryIndex] == null)
		{
			flag = true;
			index = inventoryIndex;
		}
		playerAim = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		RaycastHit hitInfo;
		if (!Physics.Raycast(playerAim, out hitInfo, grabDistance, mask, QueryTriggerInteraction.Ignore) || !hitInfo.collider.GetComponent<PhotonObjectInteract>() || hitInfo.collider.GetComponent<PhotonObjectInteract>().isGrabbed || hitInfo.collider.transform.root.CompareTag("Player"))
		{
			return;
		}
		if (hitInfo.collider.CompareTag("HeadCamera"))
		{
			PlayerHeadCamera playerHeadCamera = GameObject.Find("PCPlayer(Clone)").GetComponent<PlayerHeadCamera>();
			if (!playerHeadCamera.isEquipped)
			{
				playerHeadCamera.GrabCamera(hitInfo.collider.GetComponent<CCTV>());
			}
		}
		else if (flag)
		{
			if ((bool)hitInfo.collider.GetComponent<Tripod>() || (bool)hitInfo.collider.GetComponent<OuijaBoard>())
			{
				if (inventoryProps[inventoryIndex] == null)
				{
					inventoryProps[inventoryIndex] = hitInfo.collider.GetComponent<PhotonObjectInteract>();
					Grab(inventoryProps[inventoryIndex]);
				}
				return;
			}
			if ((bool)hitInfo.collider.GetComponent<DNAEvidence>() || (bool)hitInfo.collider.GetComponent<Key>())
			{
				hitInfo.collider.GetComponent<PhotonObjectInteract>().OnPCGrabbed.Invoke();
				return;
			}
			if ((bool)hitInfo.collider.GetComponent<Torch>() && !hitInfo.collider.GetComponent<Torch>().isBlacklight)
			{
				for (int j = 0; j < inventoryProps.Count; j++)
				{
					if (inventoryProps[j] != null && (bool)inventoryProps[j].GetComponent<Torch>() && !inventoryProps[j].GetComponent<Torch>().isBlacklight)
					{
						return;
					}
				}
				pcFlashlight.GrabbedOrDroppedFlashlight(hitInfo.collider.GetComponent<Torch>(), true);
			}
			inventoryProps[index] = hitInfo.collider.GetComponent<PhotonObjectInteract>();
			Grab(inventoryProps[index]);
			hitInfo.collider.GetComponent<PhotonObjectInteract>().isGrabbed = true;
		}
		else if ((bool)hitInfo.collider.GetComponent<DNAEvidence>() || (bool)hitInfo.collider.GetComponent<Key>())
		{
			hitInfo.collider.GetComponent<PhotonObjectInteract>().OnPCGrabbed.Invoke();
		}
	}

	private void Grab(PhotonObjectInteract grabbedItem)
	{
		if (player.isDead)
		{
			return;
		}
		if (PhotonNetwork.InRoom)
		{
			grabbedItem.view.RequestOwnership();
		}
		grabbedItem.OnPCGrabbed.Invoke();
		if (!(grabbedItem != null))
		{
			return;
		}
		if (inventoryProps[inventoryIndex] == grabbedItem)
		{
			if (grabbedItem.myRightHandModel != null)
			{
				grabbedItem.myRightHandModel.SetActive(true);
			}
		}
		else if (PhotonNetwork.InRoom)
		{
			view.RPC("EnableOrDisableObject", RpcTarget.AllBuffered, grabbedItem.view.ViewID, false);
		}
		else
		{
			EnableOrDisableObject(grabbedItem.view.ViewID, false);
		}
		Collider[] components = grabbedItem.GetComponents<Collider>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].enabled = false;
		}
		player.charAnim.SetTrigger("SwitchHolding");
		player.charAnim.SetBool("isHolding", true);
		if (PhotonNetwork.InRoom)
		{
			view.RPC("NetworkedGrab", RpcTarget.AllBuffered, grabbedItem.view.ViewID);
		}
		Rigidbody component = grabbedItem.GetComponent<Rigidbody>();
		grabSpotJoint.connectedBody = component;
		component.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
		component.isKinematic = true;
		grabbedItem.transform.SetParent(cameraItemSpot);
		if ((bool)grabbedItem.GetComponent<Tripod>())
		{
			grabbedItem.transform.localPosition = new Vector3(0f, -0.6f, 0f);
		}
		else
		{
			grabbedItem.transform.localPosition = grabbedItem.localPlayerPosition;
		}
		Quaternion localRotation = grabbedItem.transform.localRotation;
		localRotation.eulerAngles = grabbedItem.localPlayerRotation;
		grabbedItem.transform.localRotation = localRotation;
	}

	[PunRPC]
	private void NetworkedGrab(int id)
	{
		if (!(PhotonView.Find(id) == null))
		{
			PhotonObjectInteract component = PhotonView.Find(id).GetComponent<PhotonObjectInteract>();
			if ((bool)component.transformView)
			{
				component.transformView.m_SynchronizeRotation = false;
				component.transformView.m_SynchronizePosition = false;
			}
			if (!view.IsMine)
			{
				inventoryProps[inventoryIndex] = component;
			}
			component.isGrabbed = true;
			component.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
			component.GetComponent<Rigidbody>().isKinematic = true;
			component.transform.SetParent(grabSpotJoint.transform);
			if ((bool)component.GetComponent<Tripod>())
			{
				component.transform.localPosition = new Vector3(0f, -0.9f, 0f);
			}
			else
			{
				component.transform.localPosition = Vector3.zero;
			}
			Quaternion localRotation = component.transform.localRotation;
			localRotation.eulerAngles = component.localPlayerRotation;
			component.transform.localRotation = localRotation;
		}
	}

	[PunRPC]
	private void NetworkedUnGrab(int id, string itemName)
	{
		if (!(PhotonView.Find(id) == null))
		{
			PhotonObjectInteract component = PhotonView.Find(id).GetComponent<PhotonObjectInteract>();
			if ((bool)component.transformView)
			{
				component.transformView.m_SynchronizeRotation = true;
				component.transformView.m_SynchronizePosition = true;
			}
			component.isGrabbed = false;
			if (PhotonNetwork.InRoom && !view.IsMine)
			{
				inventoryProps[inventoryIndex] = null;
			}
			if (view.IsMine || !PhotonNetwork.InRoom)
			{
				component.GetComponent<Rigidbody>().isKinematic = component.wasKinematic;
			}
			component.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
			component.transform.SetParent(null);
		}
	}

	public void Drop(bool resetRigid)
	{
		if (inventoryProps[inventoryIndex] == null)
		{
			return;
		}
		if ((bool)inventoryProps[inventoryIndex].GetComponent<Torch>() && !inventoryProps[inventoryIndex].GetComponent<Torch>().isBlacklight)
		{
			pcFlashlight.GrabbedOrDroppedFlashlight(inventoryProps[inventoryIndex].GetComponent<Torch>(), false);
		}
		player.charAnim.SetTrigger("SwitchHolding");
		player.charAnim.SetBool("isHolding", false);
		player.currentHeldObject = null;
		if (PhotonNetwork.InRoom)
		{
			view.RPC("NetworkedUnGrab", RpcTarget.AllBuffered, inventoryProps[inventoryIndex].view.ViewID, inventoryProps[inventoryIndex].gameObject.name);
		}
		else
		{
			NetworkedUnGrab(inventoryProps[inventoryIndex].view.ViewID, inventoryProps[inventoryIndex].gameObject.name);
		}
		if ((bool)inventoryProps[inventoryIndex].myRightHandModel)
		{
			inventoryProps[inventoryIndex].myRightHandModel.SetActive(false);
		}
		inventoryProps[inventoryIndex].transform.SetParent(null);
		grabSpotJoint.connectedBody = null;
		inventoryProps[inventoryIndex].transform.position = player.cam.transform.position + -player.cam.transform.up * 0.05f + player.cam.transform.forward * 0.15f;
		if (resetRigid && !inventoryProps[inventoryIndex].GetComponent<Tripod>())
		{
			inventoryProps[inventoryIndex].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
			inventoryProps[inventoryIndex].GetComponent<Rigidbody>().useGravity = inventoryProps[inventoryIndex].wasGravity;
			inventoryProps[inventoryIndex].GetComponent<Rigidbody>().isKinematic = inventoryProps[inventoryIndex].wasKinematic;
			if (inventoryProps[inventoryIndex].wasGravity)
			{
				inventoryProps[inventoryIndex].GetComponent<Rigidbody>().AddForce(player.cam.transform.forward * 3f, ForceMode.Impulse);
			}
		}
		inventoryProps[inventoryIndex].OnPCUnGrabbed.Invoke();
		Collider[] components = inventoryProps[inventoryIndex].GetComponents<Collider>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].enabled = true;
		}
		inventoryProps[inventoryIndex] = null;
	}

	private void AttemptUse()
	{
		if (!player.isDead)
		{
			playerAim = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
			RaycastHit hitInfo;
			if (Physics.Raycast(playerAim, out hitInfo, grabDistance, mask, QueryTriggerInteraction.Ignore) && hitInfo.collider.GetComponent<PhotonObjectInteract>() != null && (bool)hitInfo.collider.GetComponent<PhotonObjectInteract>())
			{
				hitInfo.collider.GetComponent<PhotonObjectInteract>().OnUse.Invoke();
			}
		}
	}

	public void SwitchHand(int modifier)
	{
		if ((bool)inventoryProps[inventoryIndex])
		{
			if ((bool)inventoryProps[inventoryIndex].GetComponent<CCTV>())
			{
				inventoryProps[inventoryIndex].GetComponent<CCTV>().TurnOff();
			}
			else if ((bool)inventoryProps[inventoryIndex].GetComponent<EVPRecorder>())
			{
				inventoryProps[inventoryIndex].GetComponent<EVPRecorder>().TurnOff();
			}
			else if ((bool)inventoryProps[inventoryIndex].GetComponent<EMFReader>())
			{
				if (inventoryProps[inventoryIndex].GetComponent<EMFReader>().isOn)
				{
					inventoryProps[inventoryIndex].GetComponent<EMFReader>().Use();
				}
			}
			else if ((bool)inventoryProps[inventoryIndex].GetComponent<Tripod>())
			{
				Drop(true);
			}
			else if ((bool)inventoryProps[inventoryIndex].GetComponent<OuijaBoard>())
			{
				Drop(true);
			}
		}
		int index = inventoryIndex;
		inventoryIndex += modifier;
		if (inventoryIndex > inventoryProps.Count - 1)
		{
			inventoryIndex = 0;
		}
		else if (inventoryIndex < 0)
		{
			inventoryIndex = inventoryProps.Count - 1;
		}
		if ((bool)inventoryProps[inventoryIndex])
		{
			PhotonObjectInteract photonObjectInteract = inventoryProps[inventoryIndex];
		}
		if ((bool)inventoryProps[index])
		{
			if (PhotonNetwork.InRoom)
			{
				view.RPC("EnableOrDisableObject", RpcTarget.AllBuffered, inventoryProps[index].view.ViewID, false);
			}
			else
			{
				EnableOrDisableObject(inventoryProps[index].view.ViewID, false);
			}
		}
		if ((bool)inventoryProps[inventoryIndex])
		{
			if (PhotonNetwork.InRoom)
			{
				view.RPC("EnableOrDisableObject", RpcTarget.AllBuffered, inventoryProps[inventoryIndex].view.ViewID, true);
			}
			else
			{
				EnableOrDisableObject(inventoryProps[inventoryIndex].view.ViewID, true);
			}
			Grab(inventoryProps[inventoryIndex]);
			player.currentHeldObject = inventoryProps[inventoryIndex];
			if ((bool)inventoryProps[inventoryIndex].GetComponent<Torch>())
			{
				pcFlashlight.EnableOrDisableLight(false, true);
			}
		}
		else
		{
			player.currentHeldObject = null;
		}
	}

	[PunRPC]
	private void EnableOrDisableObject(int id, bool enable)
	{
		if (PhotonView.Find(id) != null)
		{
			PhotonView.Find(id).gameObject.SetActive(enable);
		}
	}

	private void OnDisable()
	{
		if (view.IsMine)
		{
			DropAllInventoryProps();
		}
	}

	public void DropAllInventoryProps()
	{
		Drop(true);
		player.currentHeldObject = null;
		for (int i = 0; i < inventoryProps.Count; i++)
		{
			if (inventoryProps[i] != null && (bool)inventoryProps[i].GetComponent<Torch>() && !inventoryProps[i].GetComponent<Torch>().isBlacklight)
			{
				pcFlashlight.GrabbedOrDroppedFlashlight(inventoryProps[i].GetComponent<Torch>(), false);
			}
		}
		for (int j = 0; j < inventoryProps.Count; j++)
		{
			if (!(inventoryProps[j] != null))
			{
				continue;
			}
			inventoryProps[j].gameObject.SetActive(true);
			if (PhotonNetwork.InRoom)
			{
				view.RPC("NetworkedUnGrab", RpcTarget.AllBuffered, inventoryProps[j].view.ViewID, inventoryProps[j].gameObject.name);
			}
			else
			{
				NetworkedUnGrab(inventoryProps[j].view.ViewID, inventoryProps[j].gameObject.name);
			}
			if ((bool)inventoryProps[j].myRightHandModel)
			{
				inventoryProps[j].myRightHandModel.SetActive(false);
			}
			inventoryProps[j].transform.SetParent(null);
			if ((bool)grabSpotJoint)
			{
				grabSpotJoint.connectedBody = null;
			}
			if (!inventoryProps[j].GetComponent<Tripod>())
			{
				inventoryProps[j].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
				inventoryProps[j].GetComponent<Rigidbody>().useGravity = inventoryProps[j].wasGravity;
				inventoryProps[j].GetComponent<Rigidbody>().isKinematic = inventoryProps[j].wasKinematic;
				if (inventoryProps[j].wasGravity)
				{
					inventoryProps[j].GetComponent<Rigidbody>().AddForce(player.cam.transform.forward * 3f, ForceMode.Impulse);
				}
			}
			inventoryProps[j].GetComponent<Collider>().enabled = true;
			inventoryProps[j] = null;
		}
	}

	public void OnPickup(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed && !player.isDead)
		{
			playerAim = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
			RaycastHit hitInfo;
			if (Physics.Raycast(playerAim, out hitInfo, grabDistance, mask, QueryTriggerInteraction.Ignore) && (bool)hitInfo.collider.GetComponent<PhotonObjectInteract>())
			{
				AttemptGrab();
			}
		}
	}

	public void OnDrop(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed && inventoryProps[inventoryIndex] != null)
		{
			Drop(true);
		}
	}

	public void OnInteract(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started && !pcCanvas.isPaused && (!LevelController.instance || !LevelController.instance.journalController.isOpen))
		{
			AttemptUse();
		}
	}

	public void OnPrimaryUse(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started && inventoryProps[inventoryIndex] != null)
		{
			inventoryProps[inventoryIndex].OnUse.Invoke();
		}
		if (context.phase == InputActionPhase.Canceled && inventoryProps[inventoryIndex] != null)
		{
			inventoryProps[inventoryIndex].OnPCStopUse.Invoke();
		}
	}

	public void OnSecondaryUse(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started && inventoryProps[inventoryIndex] != null)
		{
			inventoryProps[inventoryIndex].OnPCSecondaryUse.Invoke();
		}
	}

	public void OnInventorySwap(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started && !pcCanvas.isPaused && (!LevelController.instance || !LevelController.instance.journalController.isOpen))
		{
			SwitchHand(1);
		}
	}

	public void OnInventorySwapScroll(InputAction.CallbackContext context)
	{
		if (!pcCanvas.isPaused && (!LevelController.instance || !LevelController.instance.journalController.isOpen))
		{
			Vector2 vector = context.ReadValue<Vector2>();
			if (vector.y > 0f)
			{
				SwitchHand(1);
			}
			else if (vector.y < 0f)
			{
				SwitchHand(-1);
			}
		}
	}
}

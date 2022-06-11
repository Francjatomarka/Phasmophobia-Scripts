using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// Token: 0x020000A7 RID: 167
public class GhostActivity : MonoBehaviour
{
	// Token: 0x060004F2 RID: 1266 RVA: 0x0001B494 File Offset: 0x00019694
	private void Awake()
	{
		this.ghostAI = base.GetComponentInParent<GhostAI>();
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x0001B4A2 File Offset: 0x000196A2
	private void Start()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		if (GameController.instance)
		{
			if (GameController.instance.levelDifficulty != Contract.LevelDifficulty.Amateur)
			{
				this.ghostAbilityRandMax = 14;
				return;
			}
		}
		else if (FindObjectOfType<GameController>().levelDifficulty != Contract.LevelDifficulty.Amateur)
		{
			this.ghostAbilityRandMax = 14;
		}
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x0001B4E4 File Offset: 0x000196E4
	public void Interact()
	{
		if (this.objectsToInteractWith.Count == 0 || !SetupPhaseController.instance.mainDoorHasUnlocked)
		{
			this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
			return;
		}
		if (UnityEngine.Random.Range(0, 3) == 1)
		{
			this.InteractWithARandomDoor();
			return;
		}
		if (UnityEngine.Random.Range(0, 2) == 1)
		{
			this.GhostWriting();
			return;
		}
		this.InteractWithARandomProp();
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x0001B544 File Offset: 0x00019744
	public void InteractWithARandomProp()
	{
		if (this.objectsToInteractWith.Count == 0)
		{
			this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
			return;
		}
		if (this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Shade && LevelController.instance.currentGhostRoom.playersInRoom.Count > 1)
		{
			this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
			return;
		}
		PhotonObjectInteract photonObjectInteract = this.objectsToInteractWith[UnityEngine.Random.Range(0, this.objectsToInteractWith.Count)];
		if (photonObjectInteract.GetComponent<LightSwitch>())
		{
			if (UnityEngine.Random.Range(0, 4) < 3)
			{
				this.ghostAI.ChangeState(GhostAI.States.light, photonObjectInteract, null);
				return;
			}
			this.ghostAI.ChangeState(GhostAI.States.flicker, photonObjectInteract, null);
			return;
		}
		else
		{
			if (photonObjectInteract.GetComponent<Car>())
			{
				this.ghostAI.ChangeState(GhostAI.States.carAlarm, null, null);
				return;
			}
			if (photonObjectInteract.GetComponent<Window>())
			{
				this.ghostAI.ChangeState(GhostAI.States.windowKnock, photonObjectInteract, null);
				return;
			}
			if (photonObjectInteract.GetComponent<Radio>())
			{
				this.ghostAI.ChangeState(GhostAI.States.radio, null, null);
				return;
			}
			if (photonObjectInteract.GetComponent<CCTV>())
			{
				this.ghostAI.ChangeState(GhostAI.States.cctv, photonObjectInteract, null);
				return;
			}
			if (photonObjectInteract.GetComponent<Sink>())
			{
				this.ghostAI.ChangeState(GhostAI.States.sink, photonObjectInteract, null);
				return;
			}
			if (photonObjectInteract.GetComponent<Sound>())
			{
				this.ghostAI.ChangeState(GhostAI.States.sound, photonObjectInteract, null);
				return;
			}
			if (photonObjectInteract.GetComponent<Painting>())
			{
				this.ghostAI.ChangeState(GhostAI.States.painting, photonObjectInteract, null);
				return;
			}
			if (photonObjectInteract.GetComponent<Mannequin>())
			{
				this.ghostAI.ChangeState(GhostAI.States.mannequin, photonObjectInteract, null);
				return;
			}
			if (photonObjectInteract.GetComponent<TeleportableObject>())
			{
				this.ghostAI.ChangeState(GhostAI.States.teleportObject, photonObjectInteract, null);
				return;
			}
			if (photonObjectInteract.GetComponent<AnimationObject>())
			{
				this.ghostAI.ChangeState(GhostAI.States.animationObject, photonObjectInteract, null);
				return;
			}
			this.ghostAI.ChangeState(GhostAI.States.throwing, this.GetPropToThrow(), null);
			return;
		}
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x0001B734 File Offset: 0x00019934
	public void GhostWriting()
	{
		PhotonObjectInteract photonObjectInteract = null;
		for (int i = 0; i < this.objectsToInteractWith.Count; i++)
		{
			if (this.objectsToInteractWith[i].GetComponent<GhostWriting>())
			{
				photonObjectInteract = this.objectsToInteractWith[i];
			}
		}
		if (photonObjectInteract == null)
		{
			this.InteractWithARandomProp();
			return;
		}
		photonObjectInteract.GetComponent<GhostWriting>().Use();
	}

	// Token: 0x060004F7 RID: 1271 RVA: 0x0001B79C File Offset: 0x0001999C
	public void InteractWithARandomDoor()
	{
		if (this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Shade && LevelController.instance.currentGhostRoom.playersInRoom.Count > 1)
		{
			this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
			return;
		}
		PhotonObjectInteract doorToOpen = this.GetDoorToOpen();
		if (doorToOpen == null)
		{
			this.InteractWithARandomProp();
			return;
		}
		if (doorToOpen.GetComponent<Door>())
		{
			if (doorToOpen.GetComponent<Door>().type == Key.KeyType.main)
			{
				if (UnityEngine.Random.Range(0, 5) < 3)
				{
					this.ghostAI.ChangeState(GhostAI.States.door, doorToOpen, null);
					return;
				}
				this.ghostAI.ChangeState(GhostAI.States.doorKnock, null, null);
				return;
			}
			else
			{
				if (UnityEngine.Random.Range(0, 3) < 2)
				{
					this.ghostAI.ChangeState(GhostAI.States.door, doorToOpen, null);
					return;
				}
				this.ghostAI.ChangeState(GhostAI.States.lockDoor, doorToOpen, null);
			}
		}
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x0001B86C File Offset: 0x00019A6C
	public void GhostAbility()
	{
		if (this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Shade && LevelController.instance.currentGhostRoom.playersInRoom.Count > 1)
		{
			this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
			return;
		}
		int num = UnityEngine.Random.Range(0, this.ghostAbilityRandMax);
		if (num == 0 || num == 1)
		{
			this.ghostAI.ChangeState(GhostAI.States.appear, null, null);
			return;
		}
		if (num == 2)
		{
			this.ghostAI.ChangeState(GhostAI.States.fusebox, null, null);
			return;
		}
		if (num == 3 || num == 4)
		{
			this.GetPropsToThrow();
			if (this.propsToThrow.Count > 0)
			{
				this.ghostAI.ChangeState(GhostAI.States.GhostAbility, null, this.propsToThrow.ToArray());
				return;
			}
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		else
		{
			if (GameController.instance.isTutorial)
			{
				this.ghostAI.ChangeState(GhostAI.States.favouriteRoom, null, null);
				return;
			}
			this.ghostAI.ChangeState(GhostAI.States.randomEvent, null, null);
			return;
		}
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x0001B960 File Offset: 0x00019B60
	private void GetPropsToThrow()
	{
		this.propsToThrow.Clear();
		for (int i = 0; i < this.objectsToInteractWith.Count; i++)
		{
			if (this.objectsToInteractWith[i].isProp && !this.objectsToInteractWith[i].GetComponent<Joint>())
			{
				this.propsToThrow.Add(this.objectsToInteractWith[i]);
			}
		}
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x0001B9D0 File Offset: 0x00019BD0
	public PhotonObjectInteract GetPropToThrow()
	{
		PhotonObjectInteract result = null;
		for (int i = 0; i < this.objectsToInteractWith.Count; i++)
		{
			if (this.objectsToInteractWith[i].isProp && !this.objectsToInteractWith[i].GetComponent<Joint>())
			{
				result = this.objectsToInteractWith[i];
			}
		}
		return result;
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x0001BA30 File Offset: 0x00019C30
	public PhotonObjectInteract GetDoorToOpen()
	{
		for (int i = 0; i < this.objectsToInteractWith.Count; i++)
		{
			if (this.objectsToInteractWith[i].GetComponent<Door>() && this.objectsToInteractWith[i].GetComponent<Door>().type != Key.KeyType.main)
			{
				return this.objectsToInteractWith[i];
			}
		}
		return null;
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x0001BA94 File Offset: 0x00019C94
	private void OnTriggerEnter(Collider other)
	{
		if (other.isTrigger)
		{
			return;
		}
		if (other.GetComponent<PhotonObjectInteract>())
		{
			if (!other.CompareTag("Item") && !other.CompareTag("DSLR") && !other.CompareTag("EMFReader"))
			{
				if (!this.objectsToInteractWith.Contains(other.GetComponent<PhotonObjectInteract>()))
				{
					this.objectsToInteractWith.Add(other.GetComponent<PhotonObjectInteract>());
					return;
				}
			}
			else if (other.GetComponent<GhostWriting>() && !this.objectsToInteractWith.Contains(other.GetComponent<PhotonObjectInteract>()))
			{
				this.objectsToInteractWith.Add(other.GetComponent<PhotonObjectInteract>());
			}
		}
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x0001BB34 File Offset: 0x00019D34
	private void OnTriggerExit(Collider other)
	{
		if (other.isTrigger)
		{
			return;
		}
		if (other.GetComponent<PhotonObjectInteract>() && this.objectsToInteractWith.Contains(other.GetComponent<PhotonObjectInteract>()))
		{
			this.objectsToInteractWith.Remove(other.GetComponent<PhotonObjectInteract>());
		}
	}

	// Token: 0x040004C0 RID: 1216
	[SerializeField]
	private List<PhotonObjectInteract> objectsToInteractWith = new List<PhotonObjectInteract>();

	// Token: 0x040004C1 RID: 1217
	private List<PhotonObjectInteract> propsToThrow = new List<PhotonObjectInteract>();

	// Token: 0x040004C2 RID: 1218
	private GhostAI ghostAI;

	// Token: 0x040004C3 RID: 1219
	private int ghostAbilityRandMax = 12;
}

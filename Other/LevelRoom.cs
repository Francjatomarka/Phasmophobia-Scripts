using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class LevelRoom : MonoBehaviour
{
	private void Awake()
	{
		foreach (BoxCollider item in base.GetComponents<BoxCollider>())
		{
			this.colliders.Add(item);
		}
	}

	private void Start()
	{
		if (this.roomName == string.Empty)
		{
			this.roomName = base.gameObject.name;
		}
		if (this == LevelController.instance.outsideRoom)
		{
			this.temperature = (float)UnityEngine.Random.Range(-10, 10);
			this.isOutsideRoom = true;
		}
		else
		{
			this.temperature = UnityEngine.Random.Range(18f, 20f);
			this.startingTemperature = this.temperature;
		}
		GameController.instance.OnGhostSpawned.AddListener(new UnityAction(this.SetGhostType));
		for (int i = 0; i < this.lightSwitches.Count; i++)
		{
			this.lightSwitches[i].myRoom = this;
		}
	}

	private void Update()
	{
		if (!SetupPhaseController.instance.mainDoorHasUnlocked)
		{
			return;
		}
		if (!this.isOutsideRoom)
		{
			if (this.ghostInRoom)
			{
				if (this.isFreezingTemperatureGhost)
				{
					this.temperature -= Time.deltaTime * 0.12f;
				}
				else
				{
					this.temperature -= Time.deltaTime * 0.04f;
				}
			}
			else if (!this.isFreezingTemperatureGhost)
			{
				this.temperature += Time.deltaTime * 0.2f;
			}
			if (this.isFreezingTemperatureGhost)
			{
				this.temperature = Mathf.Clamp(this.temperature, -10f, this.startingTemperature);
			}
			else
			{
				this.temperature = Mathf.Clamp(this.temperature, 5f, this.startingTemperature);
			}
		}
		if (LevelController.instance.currentPlayerRoom == this)
		{
			this.currentPlayerInRoomTimer += Time.deltaTime;
		}
	}

	private void SetGhostType()
	{
		GhostTraits.Type ghostType = LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType;
		if (ghostType == GhostTraits.Type.Phantom || ghostType == GhostTraits.Type.Banshee || ghostType == GhostTraits.Type.Mare || ghostType == GhostTraits.Type.Wraith || ghostType == GhostTraits.Type.Demon || ghostType == GhostTraits.Type.Yurei)
		{
			this.isFreezingTemperatureGhost = true;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.isTrigger)
		{
			return;
		}
		if (other.GetComponent<ThermometerSpot>())
		{
			other.GetComponent<ThermometerSpot>().myThermometer.SetTemperatureValue(this);
		}
		if (other.transform.root.CompareTag("Player"))
		{
			if (other.GetComponent<ThermometerSpot>())
			{
				return;
			}
			if (!this.playersInRoom.Contains(other.transform.root.gameObject) && !other.transform.root.GetComponent<Player>().isDead)
			{
				this.playersInRoom.Add(other.transform.root.gameObject);
			}
			other.transform.root.GetComponent<Player>().currentRoom = this;
			//other.transform.root.GetComponent<Player>().voiceOcclusion.SetVoiceMixer();
			if (other.transform.root.GetComponent<PhotonView>() && other.transform.root.GetComponent<PhotonView>().IsMine)
			{
				LevelController.instance.currentPlayerRoom = this;
			}
		}
		if (other.transform.root.CompareTag("Ghost"))
		{
			LevelController.instance.currentGhostRoom = this;
			this.ghostInRoom = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.isTrigger)
		{
			return;
		}
		if (other.transform.root.CompareTag("Player"))
		{
			if (this.playersInRoom.Contains(other.transform.root.gameObject))
			{
				this.playersInRoom.Remove(other.transform.root.gameObject);
			}
			if (other.transform.root.GetComponent<PhotonView>().IsMine)
			{
				this.currentPlayerInRoomTimer = 0f;
			}
		}
		if (other.transform.root.CompareTag("Ghost"))
		{
			this.ghostInRoom = false;
		}
	}

	[HideInInspector]
	public List<GameObject> playersInRoom = new List<GameObject>();

	public List<LightSwitch> lightSwitches = new List<LightSwitch>();

	public Door[] doors = new Door[0];

	[HideInInspector]
	public List<BoxCollider> colliders = new List<BoxCollider>();

	private bool isFreezingTemperatureGhost;

	private bool ghostInRoom;

	private bool isOutsideRoom;

	public LevelRoom.Type floorType = LevelRoom.Type.firstFloor;

	public string roomName;

	[HideInInspector]
	public float temperature = 15f;

	private float startingTemperature = 15f;

	[HideInInspector]
	public float currentPlayerInRoomTimer;

	public bool isBasementOrAttic;

	public enum Type
	{
		basement,
		firstFloor,
		secondFloor
	}
}


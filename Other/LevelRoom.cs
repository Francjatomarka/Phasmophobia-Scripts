using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

// Token: 0x0200012D RID: 301
[RequireComponent(typeof(Rigidbody))]
public class LevelRoom : MonoBehaviour
{
	// Token: 0x0600085A RID: 2138 RVA: 0x00032C48 File Offset: 0x00030E48
	private void Awake()
	{
		foreach (BoxCollider item in base.GetComponents<BoxCollider>())
		{
			this.colliders.Add(item);
		}
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x00032C7C File Offset: 0x00030E7C
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

	// Token: 0x0600085C RID: 2140 RVA: 0x00032D3C File Offset: 0x00030F3C
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

	// Token: 0x0600085D RID: 2141 RVA: 0x00032E2C File Offset: 0x0003102C
	private void SetGhostType()
	{
		GhostTraits.Type ghostType = LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType;
		if (ghostType == GhostTraits.Type.Phantom || ghostType == GhostTraits.Type.Banshee || ghostType == GhostTraits.Type.Mare || ghostType == GhostTraits.Type.Wraith || ghostType == GhostTraits.Type.Demon || ghostType == GhostTraits.Type.Yurei)
		{
			this.isFreezingTemperatureGhost = true;
		}
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x00032E74 File Offset: 0x00031074
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

	// Token: 0x0600085F RID: 2143 RVA: 0x00032FB4 File Offset: 0x000311B4
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

	// Token: 0x04000866 RID: 2150
	[HideInInspector]
	public List<GameObject> playersInRoom = new List<GameObject>();

	// Token: 0x04000867 RID: 2151
	public List<LightSwitch> lightSwitches = new List<LightSwitch>();

	// Token: 0x04000868 RID: 2152
	public Door[] doors = new Door[0];

	// Token: 0x04000869 RID: 2153
	[HideInInspector]
	public List<BoxCollider> colliders = new List<BoxCollider>();

	// Token: 0x0400086A RID: 2154
	private bool isFreezingTemperatureGhost;

	// Token: 0x0400086B RID: 2155
	private bool ghostInRoom;

	// Token: 0x0400086C RID: 2156
	private bool isOutsideRoom;

	// Token: 0x0400086D RID: 2157
	public LevelRoom.Type floorType = LevelRoom.Type.firstFloor;

	// Token: 0x0400086E RID: 2158
	public string roomName;

	// Token: 0x0400086F RID: 2159
	[HideInInspector]
	public float temperature = 15f;

	// Token: 0x04000870 RID: 2160
	private float startingTemperature = 15f;

	// Token: 0x04000871 RID: 2161
	[HideInInspector]
	public float currentPlayerInRoomTimer;

	// Token: 0x04000872 RID: 2162
	public bool isBasementOrAttic;

	// Token: 0x020004DE RID: 1246
	public enum Type
	{
		// Token: 0x040022ED RID: 8941
		basement,
		// Token: 0x040022EE RID: 8942
		firstFloor,
		// Token: 0x040022EF RID: 8943
		secondFloor
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000D9 RID: 217
public class MapController : MonoBehaviour
{
	// Token: 0x0600061F RID: 1567 RVA: 0x00024490 File Offset: 0x00022690
	private void Awake()
	{
		MapController.instance = this;
		this.players.Clear();
	}

	// Token: 0x06000620 RID: 1568 RVA: 0x000244A3 File Offset: 0x000226A3
	private void Start()
	{
		GameController.instance.OnAllPlayersConnected.AddListener(new UnityAction(this.AllPlayersAreConnected));
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x000244C0 File Offset: 0x000226C0
	private void Update()
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			if (this.players[i] != null)
			{
				this.players[i].mapIcon.position = this.players[i].cam.transform.position;
			}
		}
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x00024528 File Offset: 0x00022728
	public void ChangeFloor()
	{
		this.index++;
		if (this.index > this.maxFloorIndex)
		{
			this.index = this.minFloorIndex;
		}
		if (this.index == 0)
		{
			this.ChangeFloorMonitor(LevelRoom.Type.basement);
			return;
		}
		if (this.index == 1)
		{
			this.ChangeFloorMonitor(LevelRoom.Type.firstFloor);
			return;
		}
		this.ChangeFloorMonitor(LevelRoom.Type.secondFloor);
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x00024588 File Offset: 0x00022788
	private void AllPlayersAreConnected()
	{
		for (int i = 0; i < GameController.instance.playersData.Count; i++)
		{
			this.AssignPlayer(GameController.instance.playersData[i].player);
		}
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x000245CA File Offset: 0x000227CA
	private void AssignPlayer(Player player)
	{
		player.mapIcon = this.playerIcons[this.players.Count].transform;
		player.mapIcon.gameObject.SetActive(true);
		this.players.Add(player);
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x0002460C File Offset: 0x0002280C
	public void RemovePlayer(Player player)
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			if (this.players[i] == null)
			{
				this.players[i].mapIcon.gameObject.SetActive(false);
				this.players.RemoveAt(i);
				return;
			}
			if (this.players[i] == player)
			{
				this.players[i].mapIcon.gameObject.SetActive(false);
				this.players.Remove(player);
				return;
			}
		}
	}

	// Token: 0x06000626 RID: 1574 RVA: 0x000246B0 File Offset: 0x000228B0
	public void AssignSensor(Transform sensor, Transform icon, int floorID, MotionSensor motion)
	{
		icon.position = sensor.position;
		if (floorID == 0)
		{
			icon.SetParent(this.basementFloor);
		}
		else if (floorID == 1)
		{
			icon.SetParent(this.firstFloor);
		}
		else
		{
			icon.SetParent(this.secondFloor);
		}
		icon.transform.localScale = Vector3.one * this.iconScale;
		if (motion)
		{
			if (motion.id == 1)
			{
				MotionSensorData.instance.image1 = motion.sensorIcon;
				return;
			}
			if (motion.id == 2)
			{
				MotionSensorData.instance.image2 = motion.sensorIcon;
				return;
			}
			if (motion.id == 3)
			{
				MotionSensorData.instance.image3 = motion.sensorIcon;
				return;
			}
			if (motion.id == 4)
			{
				MotionSensorData.instance.image4 = motion.sensorIcon;
			}
		}
	}

	// Token: 0x06000627 RID: 1575 RVA: 0x0002478C File Offset: 0x0002298C
	public void AssignIcon(Transform icon, LevelRoom.Type floorType)
	{
		if (floorType == LevelRoom.Type.basement)
		{
			icon.SetParent(this.basementFloor);
		}
		else if (floorType == LevelRoom.Type.firstFloor)
		{
			icon.SetParent(this.firstFloor);
		}
		else
		{
			icon.SetParent(this.secondFloor);
		}
		icon.transform.localScale = Vector3.one * this.iconScale;
	}

	// Token: 0x06000628 RID: 1576 RVA: 0x000247E4 File Offset: 0x000229E4
	public void ChangeFloorMonitor(LevelRoom.Type floorType)
	{
		this.basementFloor.gameObject.SetActive(false);
		this.firstFloor.gameObject.SetActive(false);
		this.secondFloor.gameObject.SetActive(false);
		if (floorType == LevelRoom.Type.basement)
		{
			this.basementFloor.gameObject.SetActive(true);
			return;
		}
		if (floorType == LevelRoom.Type.firstFloor)
		{
			this.firstFloor.gameObject.SetActive(true);
			return;
		}
		this.secondFloor.gameObject.SetActive(true);
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x00024860 File Offset: 0x00022A60
	public void ChangePlayerFloor(Player player, LevelRoom.Type floorType)
	{
		int i = 0;
		while (i < this.players.Count)
		{
			if (this.players[i] == player)
			{
				if (floorType == LevelRoom.Type.basement)
				{
					this.players[i].mapIcon.SetParent(this.basementFloor);
					return;
				}
				if (floorType == LevelRoom.Type.firstFloor)
				{
					this.players[i].mapIcon.SetParent(this.firstFloor);
					return;
				}
				this.players[i].mapIcon.SetParent(this.secondFloor);
				return;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x040005F0 RID: 1520
	public static MapController instance;

	// Token: 0x040005F1 RID: 1521
	[SerializeField]
	private List<Transform> playerIcons = new List<Transform>(4);

	// Token: 0x040005F2 RID: 1522
	private List<Player> players = new List<Player>();

	// Token: 0x040005F3 RID: 1523
	[SerializeField]
	private Transform basementFloor;

	// Token: 0x040005F4 RID: 1524
	[SerializeField]
	private Transform firstFloor;

	// Token: 0x040005F5 RID: 1525
	[SerializeField]
	private Transform secondFloor;

	// Token: 0x040005F6 RID: 1526
	private int index = 1;

	// Token: 0x040005F7 RID: 1527
	[SerializeField]
	private int minFloorIndex;

	// Token: 0x040005F8 RID: 1528
	[SerializeField]
	private int maxFloorIndex = 2;

	// Token: 0x040005F9 RID: 1529
	[SerializeField]
	private float iconScale = 1f;
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapController : MonoBehaviour
{
	private void Awake()
	{
		MapController.instance = this;
		this.players.Clear();
	}

	private void Start()
	{
		GameController.instance.OnAllPlayersConnected.AddListener(new UnityAction(this.AllPlayersAreConnected));
	}

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

	private void AllPlayersAreConnected()
	{
		for (int i = 0; i < GameController.instance.playersData.Count; i++)
		{
			this.AssignPlayer(GameController.instance.playersData[i].player);
		}
	}

	private void AssignPlayer(Player player)
	{
		player.mapIcon = this.playerIcons[this.players.Count].transform;
		player.mapIcon.gameObject.SetActive(true);
		this.players.Add(player);
	}

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

	public static MapController instance;

	[SerializeField]
	private List<Transform> playerIcons = new List<Transform>(4);

	private List<Player> players = new List<Player>();

	[SerializeField]
	private Transform basementFloor;

	[SerializeField]
	private Transform firstFloor;

	[SerializeField]
	private Transform secondFloor;

	private int index = 1;

	[SerializeField]
	private int minFloorIndex;

	[SerializeField]
	private int maxFloorIndex = 2;

	[SerializeField]
	private float iconScale = 1f;
}


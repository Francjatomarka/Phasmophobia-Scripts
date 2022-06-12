using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ServerListItem : MonoBehaviour
{
	private void Awake()
	{
		this.lobbyManager = UnityEngine.Object.FindObjectOfType<LobbyManager>();
	}

	public void SetUI(string name, string population, RoomInfo info)
	{
		int num = name.LastIndexOf("#");
		if (num > 0)
		{
			name = name.Substring(0, num);
		}
		this.serverName.text = "Server: " + name;
		this.serverPopulation.text = population + "/4";
		this.myRoomInfo = info;
	}

	public void Clicked()
	{
		if (!this.myRoomInfo.IsOpen || !this.myRoomInfo.IsVisible)
		{
			this.lobbyManager.RefreshList();
			return;
		}
		this.lobbyManager.JoinServer(this.myRoomInfo);
	}

	public Text serverName;

	public Text serverPopulation;

	private LobbyManager lobbyManager;

	private RoomInfo myRoomInfo;
}


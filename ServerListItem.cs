using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x02000147 RID: 327
public class ServerListItem : MonoBehaviour
{
	// Token: 0x06000930 RID: 2352 RVA: 0x00038B14 File Offset: 0x00036D14
	private void Awake()
	{
		this.lobbyManager = UnityEngine.Object.FindObjectOfType<LobbyManager>();
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x00038B24 File Offset: 0x00036D24
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

	// Token: 0x06000932 RID: 2354 RVA: 0x00038B7E File Offset: 0x00036D7E
	public void Clicked()
	{
		if (!this.myRoomInfo.IsOpen || !this.myRoomInfo.IsVisible)
		{
			this.lobbyManager.RefreshList();
			return;
		}
		this.lobbyManager.JoinServer(this.myRoomInfo);
	}

	// Token: 0x04000960 RID: 2400
	public Text serverName;

	// Token: 0x04000961 RID: 2401
	public Text serverPopulation;

	// Token: 0x04000962 RID: 2402
	private LobbyManager lobbyManager;

	// Token: 0x04000963 RID: 2403
	private RoomInfo myRoomInfo;
}

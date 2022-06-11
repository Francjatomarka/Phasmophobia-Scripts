using System;
using System.Text;
using UnityEngine;
using Photon.Pun;

// Token: 0x0200009F RID: 159
public class SupportLogging : MonoBehaviour
{
	// Token: 0x060004AB RID: 1195 RVA: 0x00019E60 File Offset: 0x00018060
	public void Start()
	{
		if (this.LogTrafficStats)
		{
			base.InvokeRepeating("LogStats", 10f, 10f);
		}
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x00019E80 File Offset: 0x00018080
	protected void OnApplicationPause(bool pause)
	{
		Debug.Log("SupportLogger OnApplicationPause: " + pause.ToString() + " connected: " + PhotonNetwork.ConnectMethod.ToString());
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x00019EB5 File Offset: 0x000180B5
	public void OnApplicationQuit()
	{
		base.CancelInvoke();
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x00019EBD File Offset: 0x000180BD
	public void LogStats()
	{
		if (this.LogTrafficStats)
		{
			Debug.Log("SupportLogger " + PhotonNetwork.NetworkStatisticsToString());
		}
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x00019EDC File Offset: 0x000180DC
	private void LogBasics()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("SupportLogger Info: PUN {0}: ", "1.88");
		stringBuilder.AppendFormat("AppID: {0}*** GameVersion: {1} PeerId: {2} ", PhotonNetwork.NetworkingClient.AppId.Substring(0, 8), PhotonNetwork.NetworkingClient.AppVersion, PhotonNetwork.NetworkingClient.UserId);
		stringBuilder.AppendFormat("Server: {0}. Region: {1} ", PhotonNetwork.ServerAddress, PhotonNetwork.NetworkingClient.CloudRegion);
		stringBuilder.AppendFormat("HostType: {0} ");
		Debug.Log(stringBuilder.ToString());
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x00019F75 File Offset: 0x00018175
	public void OnConnectedToPhoton()
	{
		Debug.Log("SupportLogger OnConnectedToPhoton().");
		this.LogBasics();
		if (this.LogTrafficStats)
		{
			PhotonNetwork.NetworkStatisticsEnabled = true;
		}
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x00019F95 File Offset: 0x00018195
	public void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.Log("SupportLogger OnFailedToConnectToPhoton(" + cause + ").");
		this.LogBasics();
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x00019FB7 File Offset: 0x000181B7
	public void OnJoinedLobby()
	{
		Debug.Log("SupportLogger OnJoinedLobby(" + PhotonNetwork.InLobby + ").");
	}

	// Token: 0x060004B3 RID: 1203 RVA: 0x00019FD4 File Offset: 0x000181D4
	public void OnJoinedRoom()
	{
		Debug.Log(string.Concat(new object[]
		{
			"SupportLogger OnJoinedRoom(",
			PhotonNetwork.CurrentRoom,
			"). ",
			PhotonNetwork.CurrentLobby,
			" GameServer:",
			PhotonNetwork.ServerAddress
		}));
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x0001A024 File Offset: 0x00018224
	public void OnCreatedRoom()
	{
		Debug.Log(string.Concat(new object[]
		{
			"SupportLogger OnCreatedRoom(",
			PhotonNetwork.CurrentRoom,
			"). ",
			PhotonNetwork.CurrentLobby,
			" GameServer:",
			PhotonNetwork.ServerAddress
		}));
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x0001A071 File Offset: 0x00018271
	public void OnLeftRoom()
	{
		Debug.Log("SupportLogger OnLeftRoom().");
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x0001A07D File Offset: 0x0001827D
	public void OnDisconnectedFromPhoton()
	{
		Debug.Log("SupportLogger OnDisconnectedFromPhoton().");
	}

	// Token: 0x04000490 RID: 1168
	public bool LogTrafficStats;
}

using System;
using System.Text;
using UnityEngine;
using Photon.Pun;

public class SupportLogging : MonoBehaviour
{
	public void Start()
	{
		if (this.LogTrafficStats)
		{
			base.InvokeRepeating("LogStats", 10f, 10f);
		}
	}

	protected void OnApplicationPause(bool pause)
	{
		Debug.Log("SupportLogger OnApplicationPause: " + pause.ToString() + " connected: " + PhotonNetwork.ConnectMethod.ToString());
	}

	public void OnApplicationQuit()
	{
		base.CancelInvoke();
	}

	public void LogStats()
	{
		if (this.LogTrafficStats)
		{
			Debug.Log("SupportLogger " + PhotonNetwork.NetworkStatisticsToString());
		}
	}

	private void LogBasics()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("SupportLogger Info: PUN {0}: ", "1.88");
		stringBuilder.AppendFormat("AppID: {0}*** GameVersion: {1} PeerId: {2} ", PhotonNetwork.NetworkingClient.AppId.Substring(0, 8), PhotonNetwork.NetworkingClient.AppVersion, PhotonNetwork.NetworkingClient.UserId);
		stringBuilder.AppendFormat("Server: {0}. Region: {1} ", PhotonNetwork.ServerAddress, PhotonNetwork.NetworkingClient.CloudRegion);
		stringBuilder.AppendFormat("HostType: {0} ");
		Debug.Log(stringBuilder.ToString());
	}

	public void OnConnectedToPhoton()
	{
		Debug.Log("SupportLogger OnConnectedToPhoton().");
		this.LogBasics();
		if (this.LogTrafficStats)
		{
			PhotonNetwork.NetworkStatisticsEnabled = true;
		}
	}

	public void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.Log("SupportLogger OnFailedToConnectToPhoton(" + cause + ").");
		this.LogBasics();
	}

	public void OnJoinedLobby()
	{
		Debug.Log("SupportLogger OnJoinedLobby(" + PhotonNetwork.InLobby + ").");
	}

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

	public void OnLeftRoom()
	{
		Debug.Log("SupportLogger OnLeftRoom().");
	}

	public void OnDisconnectedFromPhoton()
	{
		Debug.Log("SupportLogger OnDisconnectedFromPhoton().");
	}

	public bool LogTrafficStats;
}


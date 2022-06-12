using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorkerInGame : MonoBehaviourPunCallbacks
{
	public void Awake()
	{
		PhotonNetwork.Instantiate(this.playerPrefab.name, base.transform.position, Quaternion.identity, 0);
	}

	public void OnGUI()
	{
		if (GUILayout.Button("Return to Lobby", Array.Empty<GUILayoutOption>()))
		{
			PhotonNetwork.LeaveRoom(true);
		}
	}

	public void OnMasterClientSwitched(Photon.Realtime.Player player)
	{
		Debug.Log("OnMasterClientSwitched: " + player);
		InRoomChat component = base.GetComponent<InRoomChat>();
		if (component != null)
		{
			string newLine;
			if (player.IsLocal)
			{
				newLine = "You are Master Client now.";
			}
			else
			{
				newLine = player.NickName + " is Master Client now.";
			}
			component.AddLine(newLine);
		}
	}

	public void OnLeftRoom()
	{
		Debug.Log("OnLeftRoom (local)");
	}

	public void OnDisconnectedFromPhoton()
	{
		Debug.Log("OnDisconnectedFromPhoton");
	}

	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		Debug.Log("OnPhotonInstantiate " + info.Sender);
	}

	public void OnPhotonPlayerConnected(Photon.Realtime.Player player)
	{
		Debug.Log("OnPhotonPlayerConnected: " + player);
	}

	public void OnPhotonPlayerDisconnected(Photon.Realtime.Player player)
	{
		Debug.Log("OnPlayerDisconneced: " + player);
	}

	public void OnFailedToConnectToPhoton()
	{
		Debug.Log("OnFailedToConnectToPhoton");
	}

	public Transform playerPrefab;
}


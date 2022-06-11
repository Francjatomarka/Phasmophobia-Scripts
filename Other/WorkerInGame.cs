using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200006E RID: 110
public class WorkerInGame : MonoBehaviourPunCallbacks
{
	// Token: 0x06000275 RID: 629 RVA: 0x00010DA0 File Offset: 0x0000EFA0
	public void Awake()
	{
		PhotonNetwork.Instantiate(this.playerPrefab.name, base.transform.position, Quaternion.identity, 0);
	}

	// Token: 0x06000276 RID: 630 RVA: 0x00010DD6 File Offset: 0x0000EFD6
	public void OnGUI()
	{
		if (GUILayout.Button("Return to Lobby", Array.Empty<GUILayoutOption>()))
		{
			PhotonNetwork.LeaveRoom(true);
		}
	}

	// Token: 0x06000277 RID: 631 RVA: 0x00010DF0 File Offset: 0x0000EFF0
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

	// Token: 0x06000278 RID: 632 RVA: 0x00010E45 File Offset: 0x0000F045
	public void OnLeftRoom()
	{
		Debug.Log("OnLeftRoom (local)");
	}

	// Token: 0x06000279 RID: 633 RVA: 0x00010E5B File Offset: 0x0000F05B
	public void OnDisconnectedFromPhoton()
	{
		Debug.Log("OnDisconnectedFromPhoton");
	}

	// Token: 0x0600027A RID: 634 RVA: 0x00010E71 File Offset: 0x0000F071
	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		Debug.Log("OnPhotonInstantiate " + info.Sender);
	}

	// Token: 0x0600027B RID: 635 RVA: 0x00010E88 File Offset: 0x0000F088
	public void OnPhotonPlayerConnected(Photon.Realtime.Player player)
	{
		Debug.Log("OnPhotonPlayerConnected: " + player);
	}

	// Token: 0x0600027C RID: 636 RVA: 0x00010E9A File Offset: 0x0000F09A
	public void OnPhotonPlayerDisconnected(Photon.Realtime.Player player)
	{
		Debug.Log("OnPlayerDisconneced: " + player);
	}

	// Token: 0x0600027D RID: 637 RVA: 0x00010EAC File Offset: 0x0000F0AC
	public void OnFailedToConnectToPhoton()
	{
		Debug.Log("OnFailedToConnectToPhoton");
	}

	// Token: 0x040002D9 RID: 729
	public Transform playerPrefab;
}

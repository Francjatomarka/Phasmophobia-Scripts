using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x02000083 RID: 131
public class ConnectAndJoinRandom : MonoBehaviourPunCallbacks
{

	// Token: 0x06000415 RID: 1045 RVA: 0x000173B8 File Offset: 0x000155B8
	public virtual void Update()
	{
		if (this.ConnectInUpdate && this.AutoConnect && !PhotonNetwork.IsConnected)
		{
			Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");
			this.ConnectInUpdate = false;
			PhotonNetwork.ConnectUsingSettings();
		}
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x00017412 File Offset: 0x00015612
	public virtual void OnConnectedToMaster()
	{
		Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
		PhotonNetwork.JoinRandomRoom();
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x00017424 File Offset: 0x00015624
	public virtual void OnJoinedLobby()
	{
		Debug.Log("OnJoinedLobby(). This client is connected and does get a room-list, which gets stored as PhotonNetwork.GetRoomList(). This script now calls: PhotonNetwork.JoinRandomRoom();");
		PhotonNetwork.JoinRandomRoom();
	}

	// Token: 0x06000418 RID: 1048 RVA: 0x00017436 File Offset: 0x00015636
	public virtual void OnPhotonRandomJoinFailed()
	{
		Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
		PhotonNetwork.CreateRoom(null, new RoomOptions
		{
			MaxPlayers = 4
		}, null);
	}

	// Token: 0x06000419 RID: 1049 RVA: 0x00017456 File Offset: 0x00015656
	public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.LogError("Cause: " + cause);
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x0001746D File Offset: 0x0001566D
	public void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
	}

	// Token: 0x0400042F RID: 1071
	public bool AutoConnect = true;

	// Token: 0x04000430 RID: 1072
	public byte Version = 1;

	// Token: 0x04000431 RID: 1073
	private bool ConnectInUpdate = true;
}

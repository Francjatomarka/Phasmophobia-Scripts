using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000076 RID: 118
public class RandomMatchmaker : MonoBehaviourPunCallbacks
{
	// Token: 0x060002A4 RID: 676 RVA: 0x0001186F File Offset: 0x0000FA6F
	public void Start()
	{
		PhotonNetwork.ConnectUsingSettings();
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x0001187C File Offset: 0x0000FA7C
	public override void OnJoinedLobby()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x0001188E File Offset: 0x0000FA8E
	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x00011896 File Offset: 0x0000FA96
	public void OnPhotonRandomJoinFailed()
	{
		PhotonNetwork.CreateRoom(null);
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x000118A0 File Offset: 0x0000FAA0
	public override void OnJoinedRoom()
	{
		GameObject gameObject = PhotonNetwork.Instantiate("monsterprefab", Vector3.zero, Quaternion.identity, 0);
		gameObject.GetComponent<myThirdPersonController>().isControllable = true;
		this.myPhotonView = gameObject.GetComponent<PhotonView>();
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x000118DC File Offset: 0x0000FADC
	public void OnGUI()
	{
		if (PhotonNetwork.InRoom)
		{
			bool flag = GameLogic.playerWhoIsIt == int.Parse(PhotonNetwork.LocalPlayer.UserId);
			if (flag && GUILayout.Button("Marco!", Array.Empty<GUILayoutOption>()))
			{
				this.myPhotonView.RPC("Marco", RpcTarget.All, Array.Empty<object>());
			}
			if (!flag && GUILayout.Button("Polo!", Array.Empty<GUILayoutOption>()))
			{
				this.myPhotonView.RPC("Polo", RpcTarget.All, Array.Empty<object>());
			}
		}
	}

	// Token: 0x040002EC RID: 748
	private PhotonView myPhotonView;
}

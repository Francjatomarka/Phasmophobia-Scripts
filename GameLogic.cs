using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x02000072 RID: 114
public class GameLogic : MonoBehaviourPunCallbacks
{
	// Token: 0x06000295 RID: 661 RVA: 0x00011615 File Offset: 0x0000F815
	public void Start()
	{
		GameLogic.ScenePhotonView = base.GetComponent<PhotonView>();
	}

	// Token: 0x06000296 RID: 662 RVA: 0x00011622 File Offset: 0x0000F822
	public void OnJoinedRoom()
	{
		if (PhotonNetwork.PlayerList.Length == 1)
		{
			GameLogic.playerWhoIsIt = int.Parse(PhotonNetwork.LocalPlayer.UserId);
		}
		Debug.Log("playerWhoIsIt: " + GameLogic.playerWhoIsIt);
	}

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
		Debug.Log("OnPhotonPlayerConnected: " + newPlayer);
		if (PhotonNetwork.IsMasterClient)
		{
			GameLogic.TagPlayer(GameLogic.playerWhoIsIt);
		}
	}

	// Token: 0x06000298 RID: 664 RVA: 0x00011679 File Offset: 0x0000F879
	public static void TagPlayer(int playerID)
	{
		Debug.Log("TagPlayer: " + playerID);
		GameLogic.ScenePhotonView.RPC("TaggedPlayer", RpcTarget.All, new object[]
		{
			playerID
		});
	}

	// Token: 0x06000299 RID: 665 RVA: 0x000116AF File Offset: 0x0000F8AF
	[PunRPC]
	public void TaggedPlayer(int playerID)
	{
		GameLogic.playerWhoIsIt = playerID;
		Debug.Log("TaggedPlayer: " + playerID);
	}

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
		Debug.Log("OnPhotonPlayerDisconnected: " + otherPlayer);
		if (PhotonNetwork.IsMasterClient && int.Parse(otherPlayer.UserId) == GameLogic.playerWhoIsIt)
		{
			GameLogic.TagPlayer(int.Parse(PhotonNetwork.LocalPlayer.UserId));
		}
	}

	// Token: 0x0600029B RID: 667 RVA: 0x00011701 File Offset: 0x0000F901
	public void OnMasterClientSwitched()
	{
		Debug.Log("OnMasterClientSwitched");
	}

	// Token: 0x040002E6 RID: 742
	public static int playerWhoIsIt;

	// Token: 0x040002E7 RID: 743
	private static PhotonView ScenePhotonView;
}

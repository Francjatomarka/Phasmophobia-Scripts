using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameLogic : MonoBehaviourPunCallbacks
{
	public void Start()
	{
		GameLogic.ScenePhotonView = base.GetComponent<PhotonView>();
	}

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

	public static void TagPlayer(int playerID)
	{
		Debug.Log("TagPlayer: " + playerID);
		GameLogic.ScenePhotonView.RPC("TaggedPlayer", RpcTarget.All, new object[]
		{
			playerID
		});
	}

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

	public void OnMasterClientSwitched()
	{
		Debug.Log("OnMasterClientSwitched");
	}

	public static int playerWhoIsIt;

	private static PhotonView ScenePhotonView;
}


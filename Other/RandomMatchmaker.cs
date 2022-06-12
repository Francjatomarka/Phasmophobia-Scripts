using System;
using Photon.Pun;
using UnityEngine;

public class RandomMatchmaker : MonoBehaviourPunCallbacks
{
	public void Start()
	{
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnJoinedLobby()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	public void OnPhotonRandomJoinFailed()
	{
		PhotonNetwork.CreateRoom(null);
	}

	public override void OnJoinedRoom()
	{
		GameObject gameObject = PhotonNetwork.Instantiate("monsterprefab", Vector3.zero, Quaternion.identity, 0);
		gameObject.GetComponent<myThirdPersonController>().isControllable = true;
		this.myPhotonView = gameObject.GetComponent<PhotonView>();
	}

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

	private PhotonView myPhotonView;
}


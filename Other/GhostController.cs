using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PhotonView))]
public class GhostController : MonoBehaviourPunCallbacks
{
	private void Awake()
	{
		GhostController.instance = this;
		this.view = base.GetComponent<PhotonView>();
		if (SceneManager.GetActiveScene().name == "Menu_New")
		{
			base.gameObject.SetActive(false);
		}
	}

	public override void OnJoinedRoom()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			this.CreateGhost();
		}
	}

	private void Start()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			this.CreateGhost();
		}
	}

	private void OnDisconnectedFromPhoton()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			this.CreateGhost();
		}
	}

	private void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
	{
		if (newMasterClient == PhotonNetwork.LocalPlayer)
		{
			this.CreateGhost();
		}
	}

	private void CreateGhost()
	{
		if (this.createdGhost)
		{
			return;
		}
		this.ghostTraits.ghostType = (GhostTraits.Type)UnityEngine.Random.Range(1, 13);
		Debug.Log("Ghost Type: " + this.ghostTraits.ghostType);
		//this.ghostTraits.victim = LevelController.instance.peopleInHouse[UnityEngine.Random.Range(0, LevelController.instance.peopleInHouse.Count)];
		this.ghostTraits.deathLength = UnityEngine.Random.Range(50, 1000);
		this.ghostTraits.ghostAge = UnityEngine.Random.Range(10, 90);
		this.ghostTraits.isMale = (UnityEngine.Random.Range(0, 2) == 0);
		this.ghostTraits.favouriteRoomID = UnityEngine.Random.Range(0, LevelController.instance.rooms.Length);
		this.ghostTraits.isShy = (UnityEngine.Random.Range(0, 2) == 1);
		string prefabName = Constants.maleGhostNames[0];
		if (this.ghostTraits.isMale)
		{
			this.ghostTraits.ghostName = LevelController.instance.possibleMaleFirstNames[UnityEngine.Random.Range(0, LevelController.instance.possibleMaleFirstNames.Length)] + " " + LevelController.instance.possibleLastNames[UnityEngine.Random.Range(0, LevelController.instance.possibleLastNames.Length)];
			prefabName = Constants.maleGhostNames[UnityEngine.Random.Range(0, Constants.maleGhostNames.Length)];
		}
		else
		{
			this.ghostTraits.ghostName = LevelController.instance.possibleFemaleFirstNames[UnityEngine.Random.Range(0, LevelController.instance.possibleFemaleFirstNames.Length)] + " " + LevelController.instance.possibleLastNames[UnityEngine.Random.Range(0, LevelController.instance.possibleLastNames.Length)];
			prefabName = Constants.femaleGhostNames[UnityEngine.Random.Range(0, Constants.femaleGhostNames.Length)];
		}
		GhostAI component = PhotonNetwork.Instantiate(prefabName, this.spawn.position, this.spawn.rotation, 0).GetComponent<GhostAI>();
		component.ghostInfo.view.RPC("SyncValues", RpcTarget.AllBuffered, new object[]
		{
			Serialisation.SerialiseStruct<GhostTraits>(this.ghostTraits)
		});
		this.view.RPC("GhostHasBeenCreated", RpcTarget.AllBuffered, Array.Empty<object>());
		Collider collider = LevelController.instance.rooms[this.ghostTraits.favouriteRoomID].colliders[UnityEngine.Random.Range(0, LevelController.instance.rooms[this.ghostTraits.favouriteRoomID].colliders.Count)];
		Vector3 pos = new Vector3(UnityEngine.Random.Range(collider.bounds.min.x, collider.bounds.max.x), collider.bounds.min.y, UnityEngine.Random.Range(collider.bounds.min.z, collider.bounds.max.z));
		component.agent.Warp(this.GetPositionOnNavMesh(pos));
	}

	[PunRPC]
	private void GhostHasBeenCreated()
	{
		this.createdGhost = true;
	}

	public void UpdatePlayerSanity()
	{
		this.view.RPC("NetworkedUpdatePlayerSanity", RpcTarget.Others, new object[]
		{
			GameController.instance.myPlayer.player.insanity,
			GameController.instance.myPlayer.actorID
		});
	}

	[PunRPC]
	public void NetworkedUpdatePlayerSanity(float insanity, int actorID)
	{
		for (int i = 0; i < GameController.instance.playersData.Count; i++)
		{
			if (GameController.instance.playersData[i].actorID == actorID)
			{
				GameController.instance.playersData[i].player.insanity = insanity;
			}
		}
	}

	private Vector3 GetPositionOnNavMesh(Vector3 pos)
	{
		NavMeshHit navMeshHit;
		NavMesh.SamplePosition(pos, out navMeshHit, 2f, 1);
		return navMeshHit.position;
	}

	private PhotonView view;

	public static GhostController instance;

	[SerializeField]
	private Transform spawn;

	private GhostTraits ghostTraits;

	private bool createdGhost;

	[SerializeField]
	public GhostEventPlayer ghostEventPlayer;
}


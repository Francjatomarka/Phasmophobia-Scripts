using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

// Token: 0x020000D1 RID: 209
[RequireComponent(typeof(PhotonView))]
public class GhostController : MonoBehaviourPunCallbacks
{
	// Token: 0x060005E3 RID: 1507 RVA: 0x00022190 File Offset: 0x00020390
	private void Awake()
	{
		GhostController.instance = this;
		this.view = base.GetComponent<PhotonView>();
		if (SceneManager.GetActiveScene().name == "Menu_New")
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x060005E4 RID: 1508 RVA: 0x000221D4 File Offset: 0x000203D4
	public override void OnJoinedRoom()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			this.CreateGhost();
		}
	}

	// Token: 0x060005E5 RID: 1509 RVA: 0x000221D4 File Offset: 0x000203D4
	private void Start()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			this.CreateGhost();
		}
	}

	// Token: 0x060005E6 RID: 1510 RVA: 0x000221D4 File Offset: 0x000203D4
	private void OnDisconnectedFromPhoton()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			this.CreateGhost();
		}
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x000221E3 File Offset: 0x000203E3
	private void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
	{
		if (newMasterClient == PhotonNetwork.LocalPlayer)
		{
			this.CreateGhost();
		}
	}

	// Token: 0x060005E8 RID: 1512 RVA: 0x000221F4 File Offset: 0x000203F4
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

	// Token: 0x060005E9 RID: 1513 RVA: 0x000224EC File Offset: 0x000206EC
	[PunRPC]
	private void GhostHasBeenCreated()
	{
		this.createdGhost = true;
	}

	// Token: 0x060005EA RID: 1514 RVA: 0x000224F8 File Offset: 0x000206F8
	public void UpdatePlayerSanity()
	{
		this.view.RPC("NetworkedUpdatePlayerSanity", RpcTarget.Others, new object[]
		{
			GameController.instance.myPlayer.player.insanity,
			GameController.instance.myPlayer.actorID
		});
	}

	// Token: 0x060005EB RID: 1515 RVA: 0x00022550 File Offset: 0x00020750
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

	// Token: 0x060005EC RID: 1516 RVA: 0x000225AC File Offset: 0x000207AC
	private Vector3 GetPositionOnNavMesh(Vector3 pos)
	{
		NavMeshHit navMeshHit;
		NavMesh.SamplePosition(pos, out navMeshHit, 2f, 1);
		return navMeshHit.position;
	}

	// Token: 0x0400058D RID: 1421
	private PhotonView view;

	// Token: 0x0400058E RID: 1422
	public static GhostController instance;

	// Token: 0x0400058F RID: 1423
	[SerializeField]
	private Transform spawn;

	// Token: 0x04000590 RID: 1424
	private GhostTraits ghostTraits;

	// Token: 0x04000591 RID: 1425
	private bool createdGhost;

	// Token: 0x04000592 RID: 1426
	[SerializeField]
	public GhostEventPlayer ghostEventPlayer;
}

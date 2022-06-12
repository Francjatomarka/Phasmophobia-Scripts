using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class EvidenceController : MonoBehaviour
{
	private void Awake()
	{
		EvidenceController.instance = this;
		this.view = base.GetComponent<PhotonView>();
	}

	private void Start()
	{
		GameController.instance.OnAllPlayersConnected.AddListener(new UnityAction(this.StartRecogniserDelay));
	}

	private void StartRecogniserDelay()
	{
		base.Invoke("SpawnOuijaBoard", 10f);
	}

	private void SpawnOuijaBoard()
	{
		if (this.hasRun)
		{
			return;
		}
		if (PhotonNetwork.IsMasterClient)
		{
			int index = UnityEngine.Random.Range(0, this.ouijaBoardSpawnSpots.Count);
			this.ouijaBoard = PhotonNetwork.InstantiateSceneObject("Ouija", this.ouijaBoardSpawnSpots[index].position, Quaternion.identity, 0, null);
			Quaternion rotation = this.ouijaBoard.transform.rotation;
			Vector3 eulerAngles = rotation.eulerAngles;
			eulerAngles = new Vector3(-90f, 0f, 0f);
			rotation.eulerAngles = eulerAngles;
			this.ouijaBoard.transform.rotation = rotation;
		}
	}

	public void SpawnAllGhostTypeEvidence(GhostTraits.Type ghostType)
	{
		if (ghostType != GhostTraits.Type.Spirit && ghostType != GhostTraits.Type.Wraith)
		{
			if (ghostType == GhostTraits.Type.Phantom)
			{
				if (PhotonNetwork.IsMasterClient)
				{
					this.SpawnGhostOrb();
				}
			}
			else if (ghostType == GhostTraits.Type.Poltergeist)
			{
				if (PhotonNetwork.IsMasterClient)
				{
					this.SpawnGhostOrb();
				}
			}
			else if (ghostType != GhostTraits.Type.Banshee)
			{
				if (ghostType == GhostTraits.Type.Jinn)
				{
					if (PhotonNetwork.IsMasterClient)
					{
						this.SpawnGhostOrb();
					}
				}
				else if (ghostType == GhostTraits.Type.Mare)
				{
					if (PhotonNetwork.IsMasterClient)
					{
						this.SpawnGhostOrb();
					}
				}
				else if (ghostType != GhostTraits.Type.Revenant)
				{
					if (ghostType == GhostTraits.Type.Shade)
					{
						if (PhotonNetwork.IsMasterClient)
						{
							this.SpawnGhostOrb();
						}
					}
					else if (ghostType != GhostTraits.Type.Demon)
					{
						if (ghostType == GhostTraits.Type.Yurei)
						{
							if (PhotonNetwork.IsMasterClient)
							{
								this.SpawnGhostOrb();
							}
						}
					}
				}
			}
		}
		if (PhotonNetwork.IsMasterClient)
		{
			this.SpawnBoneDNAEvidence();
		}
	}

	private void SpawnBoneDNAEvidence()
	{
		int num = UnityEngine.Random.Range(0, this.roomsToSpawnDNAEvidenceInside.Length);
		int index = UnityEngine.Random.Range(0, this.roomsToSpawnDNAEvidenceInside[num].colliders.Count);
		Bounds bounds = this.roomsToSpawnDNAEvidenceInside[num].colliders[index].bounds;
		Vector3 position = new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), UnityEngine.Random.Range(bounds.min.y, bounds.max.y), UnityEngine.Random.Range(bounds.min.z, bounds.max.z));
		this.bone = PhotonNetwork.InstantiateSceneObject("Bone", position, Quaternion.identity, 0, null);
	}

	private void SpawnGhostOrb()
	{
		Bounds bounds = LevelController.instance.rooms[LevelController.instance.currentGhost.ghostInfo.ghostTraits.favouriteRoomID].colliders[0].bounds;
		Vector3 position = new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), UnityEngine.Random.Range(bounds.min.y, bounds.max.y), UnityEngine.Random.Range(bounds.min.z, bounds.max.z));
		PhotonNetwork.InstantiateSceneObject("GhostOrb", position, Quaternion.identity, 0, null);
	}

	public bool IsFingerPrintEvidence()
	{
		GhostInfo ghostInfo = LevelController.instance.currentGhost.ghostInfo;
		return ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Wraith || ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Poltergeist || ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Spirit || ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Banshee || ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Revenant;
	}

	public static EvidenceController instance;

	[SerializeField]
	private List<Transform> ouijaBoardSpawnSpots = new List<Transform>(0);

	[HideInInspector]
	public List<Evidence> evidenceInLevel = new List<Evidence>();

	[HideInInspector]
	public PhotonView view;

	[HideInInspector]
	public int totalEvidenceFoundInPhotos;

	[HideInInspector]
	public bool foundGhostDNA;

	public LevelRoom[] roomsToSpawnDNAEvidenceInside;

	[SerializeField]
	private Sink[] sinks;

	public Material[] handPrintMaterials;

	private bool hasRun;

	private GameObject bone;

	private GameObject ouijaBoard;
}


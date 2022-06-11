using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

// Token: 0x02000110 RID: 272
[RequireComponent(typeof(PhotonView))]
public class EvidenceController : MonoBehaviour
{
	// Token: 0x0600074B RID: 1867 RVA: 0x0002A993 File Offset: 0x00028B93
	private void Awake()
	{
		EvidenceController.instance = this;
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x0600074C RID: 1868 RVA: 0x0002A9A7 File Offset: 0x00028BA7
	private void Start()
	{
		GameController.instance.OnAllPlayersConnected.AddListener(new UnityAction(this.StartRecogniserDelay));
	}

	// Token: 0x0600074D RID: 1869 RVA: 0x0002A9C4 File Offset: 0x00028BC4
	private void StartRecogniserDelay()
	{
		base.Invoke("SpawnOuijaBoard", 10f);
	}

	// Token: 0x0600074E RID: 1870 RVA: 0x0002A9D8 File Offset: 0x00028BD8
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

	// Token: 0x0600074F RID: 1871 RVA: 0x0002AA7C File Offset: 0x00028C7C
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

	// Token: 0x06000750 RID: 1872 RVA: 0x0002AB2C File Offset: 0x00028D2C
	private void SpawnBoneDNAEvidence()
	{
		int num = UnityEngine.Random.Range(0, this.roomsToSpawnDNAEvidenceInside.Length);
		int index = UnityEngine.Random.Range(0, this.roomsToSpawnDNAEvidenceInside[num].colliders.Count);
		Bounds bounds = this.roomsToSpawnDNAEvidenceInside[num].colliders[index].bounds;
		Vector3 position = new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), UnityEngine.Random.Range(bounds.min.y, bounds.max.y), UnityEngine.Random.Range(bounds.min.z, bounds.max.z));
		this.bone = PhotonNetwork.InstantiateSceneObject("Bone", position, Quaternion.identity, 0, null);
	}

	// Token: 0x06000751 RID: 1873 RVA: 0x0002ABF0 File Offset: 0x00028DF0
	private void SpawnGhostOrb()
	{
		Bounds bounds = LevelController.instance.rooms[LevelController.instance.currentGhost.ghostInfo.ghostTraits.favouriteRoomID].colliders[0].bounds;
		Vector3 position = new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), UnityEngine.Random.Range(bounds.min.y, bounds.max.y), UnityEngine.Random.Range(bounds.min.z, bounds.max.z));
		PhotonNetwork.InstantiateSceneObject("GhostOrb", position, Quaternion.identity, 0, null);
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x0002ACA4 File Offset: 0x00028EA4
	public bool IsFingerPrintEvidence()
	{
		GhostInfo ghostInfo = LevelController.instance.currentGhost.ghostInfo;
		return ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Wraith || ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Poltergeist || ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Spirit || ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Banshee || ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Revenant;
	}

	// Token: 0x040006F8 RID: 1784
	public static EvidenceController instance;

	// Token: 0x040006F9 RID: 1785
	[SerializeField]
	private List<Transform> ouijaBoardSpawnSpots = new List<Transform>(0);

	// Token: 0x040006FA RID: 1786
	[HideInInspector]
	public List<Evidence> evidenceInLevel = new List<Evidence>();

	// Token: 0x040006FB RID: 1787
	[HideInInspector]
	public PhotonView view;

	// Token: 0x040006FC RID: 1788
	[HideInInspector]
	public int totalEvidenceFoundInPhotos;

	// Token: 0x040006FD RID: 1789
	[HideInInspector]
	public bool foundGhostDNA;

	// Token: 0x040006FE RID: 1790
	public LevelRoom[] roomsToSpawnDNAEvidenceInside;

	// Token: 0x040006FF RID: 1791
	[SerializeField]
	private Sink[] sinks;

	// Token: 0x04000700 RID: 1792
	public Material[] handPrintMaterials;

	// Token: 0x04000701 RID: 1793
	private bool hasRun;

	// Token: 0x04000702 RID: 1794
	private GameObject bone;

	// Token: 0x04000703 RID: 1795
	private GameObject ouijaBoard;
}

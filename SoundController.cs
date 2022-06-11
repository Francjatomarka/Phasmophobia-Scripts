using System;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Audio;

// Token: 0x020000E0 RID: 224
[RequireComponent(typeof(PhotonView))]
public class SoundController : MonoBehaviour
{
	// Token: 0x0600065A RID: 1626 RVA: 0x00025B1E File Offset: 0x00023D1E
	private void Awake()
	{
		SoundController.instance = this;
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x0600065B RID: 1627 RVA: 0x00025B32 File Offset: 0x00023D32
	[PunRPC]
	private void PlayDoorKnockingSound()
	{
		if (!this.doorAudioSource.isPlaying)
		{
			this.doorAudioSource.Play();
		}
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x00025B4C File Offset: 0x00023D4C
	public AudioMixerGroup GetFloorAudioSnapshot(float yPos)
	{
		if (yPos < this.BasementFloorStartYPos)
		{
			return this.basementGroup;
		}
		if (yPos < this.FirstFloorStartYPos)
		{
			return this.firstFloorGroup;
		}
		return this.secondFloorGroup;
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x00025B74 File Offset: 0x00023D74
	public LevelRoom.Type GetFloorTypeFromAudioGroup(AudioMixerGroup group)
	{
		if (group == this.basementGroup)
		{
			return LevelRoom.Type.basement;
		}
		if (group == this.firstFloorGroup)
		{
			return LevelRoom.Type.firstFloor;
		}
		return LevelRoom.Type.secondFloor;
	}

	// Token: 0x0600065E RID: 1630 RVA: 0x00025B97 File Offset: 0x00023D97
	public LevelRoom.Type GetFloorTypeFromPosition(float yPos)
	{
		if (yPos < this.BasementFloorStartYPos)
		{
			return LevelRoom.Type.basement;
		}
		if (yPos < this.FirstFloorStartYPos)
		{
			return LevelRoom.Type.firstFloor;
		}
		return LevelRoom.Type.secondFloor;
	}

	// Token: 0x0600065F RID: 1631 RVA: 0x00025BB0 File Offset: 0x00023DB0
	public AudioMixerGroup GetPlayersAudioGroup(int actorID)
	{
		AudioMixerSnapshot currentPlayerSnapshot = this.firstFloorSnapshot;
		if (MainManager.instance)
		{
			currentPlayerSnapshot = MainManager.instance.localPlayer.currentPlayerSnapshot;
		}
		else
		{
			for (int i = 0; i < GameController.instance.playersData.Count; i++)
			{
				if (GameController.instance.playersData[i].actorID == actorID)
				{
					currentPlayerSnapshot = GameController.instance.playersData[i].player.currentPlayerSnapshot;
				}
			}
		}
		if (currentPlayerSnapshot == this.firstFloorSnapshot)
		{
			return this.firstFloorGroup;
		}
		if (currentPlayerSnapshot == this.secondFloorSnapshot)
		{
			return this.secondFloorGroup;
		}
		if (currentPlayerSnapshot == this.basementFloorSnapshot)
		{
			return this.basementGroup;
		}
		if (currentPlayerSnapshot == this.exteriorSnapshot)
		{
			return this.exteriorGroup;
		}
		return this.TruckGroup;
	}

	// Token: 0x06000660 RID: 1632 RVA: 0x00025C8C File Offset: 0x00023E8C
	public AudioMixerGroup GetAudioGroupFromSnapshot(AudioMixerSnapshot snapshot)
	{
		if (snapshot == this.firstFloorSnapshot)
		{
			return this.firstFloorGroup;
		}
		if (snapshot == this.secondFloorSnapshot)
		{
			return this.secondFloorGroup;
		}
		if (snapshot == this.basementFloorSnapshot)
		{
			return this.basementGroup;
		}
		if (snapshot == this.exteriorSnapshot)
		{
			return this.exteriorGroup;
		}
		return this.TruckGroup;
	}

	// Token: 0x04000624 RID: 1572
	public static SoundController instance;

	// Token: 0x04000625 RID: 1573
	[HideInInspector]
	public PhotonView view;

	// Token: 0x04000626 RID: 1574
	[Header("Door Knocking")]
	public AudioSource doorAudioSource;

	// Token: 0x04000627 RID: 1575
	[Header("Audio Mixer")]
	[SerializeField]
	private float BasementFloorStartYPos;

	// Token: 0x04000628 RID: 1576
	[SerializeField]
	private float FirstFloorStartYPos = 3f;

	// Token: 0x04000629 RID: 1577
	[SerializeField]
	private AudioMixerGroup basementGroup;

	// Token: 0x0400062A RID: 1578
	[SerializeField]
	private AudioMixerGroup firstFloorGroup;

	// Token: 0x0400062B RID: 1579
	[SerializeField]
	private AudioMixerGroup secondFloorGroup;

	// Token: 0x0400062C RID: 1580
	public AudioMixerGroup exteriorGroup;

	// Token: 0x0400062D RID: 1581
	public AudioMixerGroup TruckGroup;

	// Token: 0x0400062E RID: 1582
	public AudioMixerSnapshot exteriorSnapshot;

	// Token: 0x0400062F RID: 1583
	public AudioMixerSnapshot truckSnapshot;

	// Token: 0x04000630 RID: 1584
	public AudioMixerSnapshot firstFloorSnapshot;

	// Token: 0x04000631 RID: 1585
	public AudioMixerSnapshot secondFloorSnapshot;

	// Token: 0x04000632 RID: 1586
	public AudioMixerSnapshot basementFloorSnapshot;

	// Token: 0x04000633 RID: 1587
	public AudioClip[] genericImpactClips;
}

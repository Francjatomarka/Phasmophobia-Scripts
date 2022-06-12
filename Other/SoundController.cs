using System;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Audio;

[RequireComponent(typeof(PhotonView))]
public class SoundController : MonoBehaviour
{
	private void Awake()
	{
		SoundController.instance = this;
		this.view = base.GetComponent<PhotonView>();
	}

	[PunRPC]
	private void PlayDoorKnockingSound()
	{
		if (!this.doorAudioSource.isPlaying)
		{
			this.doorAudioSource.Play();
		}
	}

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

	public static SoundController instance;

	[HideInInspector]
	public PhotonView view;

	[Header("Door Knocking")]
	public AudioSource doorAudioSource;

	[Header("Audio Mixer")]
	[SerializeField]
	private float BasementFloorStartYPos;

	[SerializeField]
	private float FirstFloorStartYPos = 3f;

	[SerializeField]
	private AudioMixerGroup basementGroup;

	[SerializeField]
	private AudioMixerGroup firstFloorGroup;

	[SerializeField]
	private AudioMixerGroup secondFloorGroup;

	public AudioMixerGroup exteriorGroup;

	public AudioMixerGroup TruckGroup;

	public AudioMixerSnapshot exteriorSnapshot;

	public AudioMixerSnapshot truckSnapshot;

	public AudioMixerSnapshot firstFloorSnapshot;

	public AudioMixerSnapshot secondFloorSnapshot;

	public AudioMixerSnapshot basementFloorSnapshot;

	public AudioClip[] genericImpactClips;
}


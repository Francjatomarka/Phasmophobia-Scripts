using System;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Rigidbody))]
public class ChangeMixer : MonoBehaviour
{
	private void Start()
	{
		if (this.isDefaultSnapshot)
		{
			this.snapshot.TransitionTo(0f);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<EVPRecorder>())
		{
			other.GetComponent<EVPRecorder>().loopSource.outputAudioMixerGroup = SoundController.instance.GetAudioGroupFromSnapshot(this.snapshot);
			other.GetComponent<EVPRecorder>().soundSource.outputAudioMixerGroup = SoundController.instance.GetAudioGroupFromSnapshot(this.snapshot);
		}
		else if (other.GetComponent<EMFReader>())
		{
			other.GetComponent<EMFReader>().source.outputAudioMixerGroup = SoundController.instance.GetAudioGroupFromSnapshot(this.snapshot);
		}
		if (other.transform.root.CompareTag("Player"))
		{
			if (other.isTrigger)
			{
				return;
			}
			if (other.GetComponent<PhotonObjectInteract>() && !other.GetComponent<WalkieTalkie>())
			{
				return;
			}
			if (other.GetComponent<ThermometerSpot>())
			{
				return;
			}
			if (GameController.instance == null)
			{
				return;
			}
			if (GameController.instance.myPlayer == null)
			{
				return;
			}
			Player component = other.transform.root.GetComponent<Player>();
			if (component.currentHeldObject != null)
			{
				if (component.currentHeldObject.GetComponent<EVPRecorder>())
				{
					component.currentHeldObject.GetComponent<EVPRecorder>().loopSource.outputAudioMixerGroup = SoundController.instance.GetAudioGroupFromSnapshot(this.snapshot);
					component.currentHeldObject.GetComponent<EVPRecorder>().soundSource.outputAudioMixerGroup = SoundController.instance.GetAudioGroupFromSnapshot(this.snapshot);
				}
				else if (component.currentHeldObject.GetComponent<EMFReader>())
				{
					component.currentHeldObject.GetComponent<EMFReader>().source.outputAudioMixerGroup = SoundController.instance.GetAudioGroupFromSnapshot(this.snapshot);
				}
			}
			if (component.view.IsMine)
			{
				if (this.reverbZone)
				{
					this.reverbZone.gameObject.SetActive(this.activateReverbZone);
				}
				if (component.isDead)
				{
					return;
				}
				this.snapshot.TransitionTo(0.4f);
			}
			component.currentPlayerSnapshot = this.snapshot;
			component.voiceOcclusion.SetVoiceMixer();
			if (MapController.instance && !this.isDeadZoneMixer)
			{
				MapController.instance.ChangePlayerFloor(component, this.floorType);
			}
		}
	}

	[SerializeField]
	private AudioMixerSnapshot snapshot;

	[SerializeField]
	private AudioReverbZone reverbZone;

	[SerializeField]
	private bool activateReverbZone;

	[SerializeField]
	private bool isDefaultSnapshot;

	[SerializeField]
	private LevelRoom.Type floorType = LevelRoom.Type.firstFloor;

	[SerializeField]
	private bool isDeadZoneMixer;
}


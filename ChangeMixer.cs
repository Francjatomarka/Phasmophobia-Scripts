using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x0200012B RID: 299
[RequireComponent(typeof(Rigidbody))]
public class ChangeMixer : MonoBehaviour
{
	// Token: 0x06000853 RID: 2131 RVA: 0x0003278C File Offset: 0x0003098C
	private void Start()
	{
		if (this.isDefaultSnapshot)
		{
			this.snapshot.TransitionTo(0f);
		}
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x000327A8 File Offset: 0x000309A8
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

	// Token: 0x04000860 RID: 2144
	[SerializeField]
	private AudioMixerSnapshot snapshot;

	// Token: 0x04000861 RID: 2145
	[SerializeField]
	private AudioReverbZone reverbZone;

	// Token: 0x04000862 RID: 2146
	[SerializeField]
	private bool activateReverbZone;

	// Token: 0x04000863 RID: 2147
	[SerializeField]
	private bool isDefaultSnapshot;

	// Token: 0x04000864 RID: 2148
	[SerializeField]
	private LevelRoom.Type floorType = LevelRoom.Type.firstFloor;

	// Token: 0x04000865 RID: 2149
	[SerializeField]
	private bool isDeadZoneMixer;
}

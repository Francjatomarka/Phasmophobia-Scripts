using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Photon.Pun;

// Token: 0x02000175 RID: 373
[RequireComponent(typeof(PhotonView))]
public class WalkieTalkie : MonoBehaviour
{
	// Token: 0x06000AA9 RID: 2729 RVA: 0x00042292 File Offset: 0x00040492
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.noise.gameObject.SetActive(false);
	}

	// Token: 0x06000AAA RID: 2730 RVA: 0x000422B4 File Offset: 0x000404B4
	private void Start()
	{
		if (MainManager.instance != null)
		{
			base.enabled = false;
			return;
		}
		if (this.photonInteract && this.view.IsMine)
		{
			this.photonInteract.AddUseEvent(new UnityAction(this.Use));
			this.photonInteract.AddStopEvent(new UnityAction(this.Stop));
			this.photonInteract.AddUnGrabbedEvent(new UnityAction(this.Stop));
		}
	}

	// Token: 0x06000AAB RID: 2731 RVA: 0x00042335 File Offset: 0x00040535
	public void Use()
	{
		this.view.RPC("TurnOn", RpcTarget.All, Array.Empty<object>());
	}

	// Token: 0x06000AAC RID: 2732 RVA: 0x0004234D File Offset: 0x0004054D
	public void Stop()
	{
		this.view.RPC("TurnOff", RpcTarget.All, Array.Empty<object>());
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x00042368 File Offset: 0x00040568
	[PunRPC]
	private void TurnOn()
	{
		if (LevelController.instance == null)
		{
			return;
		}
		this.source.Play();
		if (LevelController.instance.currentGhost && LevelController.instance.currentGhost.isHunting)
		{
			this.staticSource.Play();
			return;
		}
		this.noise.gameObject.SetActive(true);
		this.noise.volume = 0.8f;
		this.photonVoiceSource.outputAudioMixerGroup = SoundController.instance.GetPlayersAudioGroup(GameController.instance.myPlayer.actorID);
		this.photonVoiceSource.volume = 0.04f * (XRDevice.isPresent ? PauseMenuManager.instance.GetPlayerVolume(int.Parse(this.view.Owner.UserId)) : PauseMenuController.instance.GetPlayerVolume(int.Parse(this.view.Owner.UserId)));
		this.photonVoiceSource.spatialBlend = 0f;
		this.photonVoiceSource.GetComponent<AudioDistortionFilter>().distortionLevel = 0.8f;
	}

	// Token: 0x06000AAE RID: 2734 RVA: 0x0004246C File Offset: 0x0004066C
	[PunRPC]
	private void TurnOff()
	{
		this.staticSource.Stop();
		this.isOn = false;
		this.source.Play();
		this.noise.gameObject.SetActive(false);
		this.noise.volume = 0f;
		this.photonVoiceSource.outputAudioMixerGroup = SoundController.instance.GetAudioGroupFromSnapshot(this.player.currentPlayerSnapshot);
		this.photonVoiceSource.volume = 0.8f * (XRDevice.isPresent ? PauseMenuManager.instance.GetPlayerVolume(int.Parse(this.view.Owner.UserId)) : PauseMenuController.instance.GetPlayerVolume(int.Parse(this.view.Owner.UserId)));
		this.photonVoiceSource.spatialBlend = 1f;
		this.photonVoiceSource.GetComponent<AudioDistortionFilter>().distortionLevel = 0f;
	}

	// Token: 0x04000B13 RID: 2835
	[SerializeField]
	private AudioSource source;

	// Token: 0x04000B14 RID: 2836
	[SerializeField]
	private PhotonObjectInteract photonInteract;

	// Token: 0x04000B15 RID: 2837
	[SerializeField]
	private AudioSource photonVoiceSource;

	// Token: 0x04000B16 RID: 2838
	[SerializeField]
	private Noise noise;

	// Token: 0x04000B17 RID: 2839
	[HideInInspector]
	public bool isOn;

	// Token: 0x04000B18 RID: 2840
	[SerializeField]
	private Player player;

	// Token: 0x04000B19 RID: 2841
	[SerializeField]
	private AudioSource staticSource;

	// Token: 0x04000B1A RID: 2842
	private PhotonView view;
}

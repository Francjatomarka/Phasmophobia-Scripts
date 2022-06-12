using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class WalkieTalkie : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.noise.gameObject.SetActive(false);
	}

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

	public void Use()
	{
		this.view.RPC("TurnOn", RpcTarget.All, Array.Empty<object>());
	}

	public void Stop()
	{
		this.view.RPC("TurnOff", RpcTarget.All, Array.Empty<object>());
	}

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

	[SerializeField]
	private AudioSource source;

	[SerializeField]
	private PhotonObjectInteract photonInteract;

	[SerializeField]
	private AudioSource photonVoiceSource;

	[SerializeField]
	private Noise noise;

	[HideInInspector]
	public bool isOn;

	[SerializeField]
	private Player player;

	[SerializeField]
	private AudioSource staticSource;

	private PhotonView view;
}


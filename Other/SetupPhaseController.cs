using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class SetupPhaseController : MonoBehaviour
{
	private void Awake()
	{
		SetupPhaseController.instance = this;
		this.view = base.GetComponent<PhotonView>();
		this.isSetupPhase = true;
		this.clockText.text = "5 : 00";
	}

	private void Start()
	{
		GameController.instance.OnAllPlayersConnected.AddListener(new UnityAction(this.AllPlayersAreLoadedIn));
		if (GameController.instance.levelDifficulty == Contract.LevelDifficulty.Intermediate)
		{
			this.setupPhaseTimerMinutes = 1;
			this.clockText.text = "2 : 00";
			return;
		}
		if (GameController.instance.levelDifficulty == Contract.LevelDifficulty.Professional)
		{
			this.setupPhaseTimerSeconds = 0f;
			this.setupPhaseTimerMinutes = 0;
			this.clockText.text = "0 : 00";
		}
	}

	private void AllPlayersAreLoadedIn()
	{
		this.allPlayersAreLoadedIn = true;
	}

	private void Update()
	{
		if (this.isSetupPhase && this.mainDoorHasUnlocked && this.allPlayersAreLoadedIn)
		{
			this.setupPhaseTimerSeconds -= Time.deltaTime;
			if (this.setupPhaseTimerMinutes < 0)
			{
				this.BeginHuntingPhase();
				return;
			}
			if (!this.clockAudio.isPlaying)
			{
				this.clockAudio.Play();
			}
			if (this.setupPhaseTimerSeconds < 0f)
			{
				this.setupPhaseTimerSeconds = 59f;
				this.setupPhaseTimerMinutes--;
			}
			this.clockText.text = "0" + this.setupPhaseTimerMinutes.ToString() + " : " + this.setupPhaseTimerSeconds.ToString("00");
		}
	}

	public void ForceEnterHuntingPhase()
	{
		if (PhotonNetwork.InRoom)
		{
			this.view.RPC("ForceEnterHuntingPhaseNetworked", RpcTarget.All, Array.Empty<object>());
			return;
		}
		this.ForceEnterHuntingPhaseNetworked();
	}

	[PunRPC]
	private void ForceEnterHuntingPhaseNetworked()
	{
		this.setupPhaseTimerMinutes = -1;
	}

	public void BeginHuntingPhase()
	{
		this.isSetupPhase = false;
		this.clockText.text = "00 : 00";
		this.clockAudio.Stop();
		PlayerPrefs.SetInt("setupPhase", 1);
	}

	public static SetupPhaseController instance;

	private PhotonView view;

	[HideInInspector]
	public bool isSetupPhase = true;

	[SerializeField]
	private Text clockText;

	[SerializeField]
	private AudioSource clockAudio;

	[SerializeField]
	private float setupPhaseTimerSeconds = 59f;

	[SerializeField]
	private int setupPhaseTimerMinutes = 4;

	[HideInInspector]
	public bool mainDoorHasUnlocked;

	[HideInInspector]
	public bool allPlayersAreLoadedIn;
}


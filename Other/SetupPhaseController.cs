using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;

// Token: 0x020000DF RID: 223
[RequireComponent(typeof(PhotonView))]
public class SetupPhaseController : MonoBehaviour
{
	// Token: 0x06000652 RID: 1618 RVA: 0x00025926 File Offset: 0x00023B26
	private void Awake()
	{
		SetupPhaseController.instance = this;
		this.view = base.GetComponent<PhotonView>();
		this.isSetupPhase = true;
		this.clockText.text = "5 : 00";
	}

	// Token: 0x06000653 RID: 1619 RVA: 0x00025954 File Offset: 0x00023B54
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

	// Token: 0x06000654 RID: 1620 RVA: 0x000259D0 File Offset: 0x00023BD0
	private void AllPlayersAreLoadedIn()
	{
		this.allPlayersAreLoadedIn = true;
	}

	// Token: 0x06000655 RID: 1621 RVA: 0x000259DC File Offset: 0x00023BDC
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

	// Token: 0x06000656 RID: 1622 RVA: 0x00025A9F File Offset: 0x00023C9F
	public void ForceEnterHuntingPhase()
	{
		if (PhotonNetwork.InRoom)
		{
			this.view.RPC("ForceEnterHuntingPhaseNetworked", RpcTarget.All, Array.Empty<object>());
			return;
		}
		this.ForceEnterHuntingPhaseNetworked();
	}

	// Token: 0x06000657 RID: 1623 RVA: 0x00025AC5 File Offset: 0x00023CC5
	[PunRPC]
	private void ForceEnterHuntingPhaseNetworked()
	{
		this.setupPhaseTimerMinutes = -1;
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x00025ACE File Offset: 0x00023CCE
	public void BeginHuntingPhase()
	{
		this.isSetupPhase = false;
		this.clockText.text = "00 : 00";
		this.clockAudio.Stop();
		PlayerPrefs.SetInt("setupPhase", 1);
	}

	// Token: 0x0400061B RID: 1563
	public static SetupPhaseController instance;

	// Token: 0x0400061C RID: 1564
	private PhotonView view;

	// Token: 0x0400061D RID: 1565
	[HideInInspector]
	public bool isSetupPhase = true;

	// Token: 0x0400061E RID: 1566
	[SerializeField]
	private Text clockText;

	// Token: 0x0400061F RID: 1567
	[SerializeField]
	private AudioSource clockAudio;

	// Token: 0x04000620 RID: 1568
	[SerializeField]
	private float setupPhaseTimerSeconds = 59f;

	// Token: 0x04000621 RID: 1569
	[SerializeField]
	private int setupPhaseTimerMinutes = 4;

	// Token: 0x04000622 RID: 1570
	[HideInInspector]
	public bool mainDoorHasUnlocked;

	// Token: 0x04000623 RID: 1571
	[HideInInspector]
	public bool allPlayersAreLoadedIn;
}

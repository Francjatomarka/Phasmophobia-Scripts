using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x0200016E RID: 366
public class PauseMenuManager : MonoBehaviourPunCallbacks
{
	// Token: 0x06000A63 RID: 2659 RVA: 0x000400AC File Offset: 0x0003E2AC
	private void Awake()
	{
		if (SceneManager.GetActiveScene().name == "Menu_New")
		{
			base.enabled = false;
		}
		if (this.view.IsMine)
		{
			PauseMenuManager.instance = this;
		}
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x000400F8 File Offset: 0x0003E2F8
	private void Start()
	{
		if (PhotonNetwork.PlayerListOthers.Length != 0)
		{
			this.player2Text.text = PhotonNetwork.PlayerListOthers[0].NickName;
			this.player2Object.SetActive(true);
			this.player2ActorID = int.Parse(PhotonNetwork.PlayerListOthers[0].UserId);
		}
		if (PhotonNetwork.PlayerListOthers.Length > 1)
		{
			this.player3Text.text = PhotonNetwork.PlayerListOthers[1].NickName;
			this.player3Object.SetActive(true);
			this.player3ActorID = int.Parse(PhotonNetwork.PlayerListOthers[1].UserId);
		}
		if (PhotonNetwork.PlayerListOthers.Length > 2)
		{
			this.player4Text.text = PhotonNetwork.PlayerListOthers[2].NickName;
			this.player4Object.SetActive(true);
			this.player4ActorID = int.Parse(PhotonNetwork.PlayerListOthers[2].UserId);
		}
	}

	// Token: 0x06000A65 RID: 2661 RVA: 0x000401C0 File Offset: 0x0003E3C0
	public void Player2SliderValueChanged()
	{
		this.player2ValueText.text = (this.player2Slider.value * 100f).ToString("0") + "%";
		for (int i = 0; i < GameController.instance.playersData.Count; i++)
		{
			if (GameController.instance.playersData[i].actorID == this.player2ActorID)
			{
				GameController.instance.playersData[i].player.voiceVolume.ApplyVoiceVolume(this.player2Slider.value);
			}
		}
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x00040264 File Offset: 0x0003E464
	public void Player3SliderValueChanged()
	{
		this.player3ValueText.text = (this.player3Slider.value * 100f).ToString("0") + "%";
		for (int i = 0; i < GameController.instance.playersData.Count; i++)
		{
			if (GameController.instance.playersData[i].actorID == this.player3ActorID)
			{
				GameController.instance.playersData[i].player.voiceVolume.ApplyVoiceVolume(this.player3Slider.value);
			}
		}
	}

	// Token: 0x06000A67 RID: 2663 RVA: 0x00040308 File Offset: 0x0003E508
	public void Player4SliderValueChanged()
	{
		this.player4ValueText.text = (this.player4Slider.value * 100f).ToString("0") + "%";
		for (int i = 0; i < GameController.instance.playersData.Count; i++)
		{
			if (GameController.instance.playersData[i].actorID == this.player4ActorID)
			{
				GameController.instance.playersData[i].player.voiceVolume.ApplyVoiceVolume(this.player4Slider.value);
			}
		}
	}

	// Token: 0x06000A68 RID: 2664 RVA: 0x000403AC File Offset: 0x0003E5AC
	public float GetPlayerVolume(int actorID)
	{
		if (actorID == this.player2ActorID)
		{
			return this.player2Slider.value;
		}
		if (actorID == this.player3ActorID)
		{
			return this.player3Slider.value;
		}
		if (actorID == this.player4ActorID)
		{
			return this.player4Slider.value;
		}
		return 1f;
	}

	// Token: 0x06000A69 RID: 2665 RVA: 0x00040400 File Offset: 0x0003E600
	private void OnEnable()
	{
		if (this.pauseMenuObject.GetComponent<Canvas>().renderMode == RenderMode.WorldSpace && GameController.instance.myPlayer != null)
		{
			this.pauseMenuObject.GetComponent<Canvas>().worldCamera = GameController.instance.myPlayer.player.cam;
		}
	}

	// Token: 0x06000A6A RID: 2666 RVA: 0x00040450 File Offset: 0x0003E650
	private void Paused(bool isPaused)
	{
		GameController.instance.myPlayer.player.movementSettings.InMenuOrJournal(isPaused);
		this.pauseMenuObject.SetActive(isPaused);
		if (isPaused)
		{
			this.pauseMenuObject.transform.position = this.eye.transform.position + this.eye.transform.forward * 1f;
			this.pauseMenuObject.transform.rotation = this.eye.transform.rotation;
		}
	}
	// Token: 0x06000A6C RID: 2668 RVA: 0x0004053A File Offset: 0x0003E73A
	public void ResumeButton()
	{
		this.pauseMenuObject.SetActive(false);
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void OptionsButton()
	{
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x00040548 File Offset: 0x0003E748
	public void LeaveButton()
	{
		if (GameController.instance.isTutorial)
		{
			PlayerPrefs.SetInt("MissionStatus", 3);
			PlayerPrefs.SetInt("setupPhase", 0);
			PlayerPrefs.SetInt("completedTraining", 1);
			PlayerPrefs.SetInt("StayInServerRoom", 0);
		}
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x0002501F File Offset: 0x0002321F
	public void QuitButton()
	{
		if (GameController.instance.isTutorial)
		{
			PlayerPrefs.SetInt("MissionStatus", 3);
			PlayerPrefs.SetInt("setupPhase", 0);
			PlayerPrefs.SetInt("completedTraining", 1);
			PlayerPrefs.SetInt("StayInServerRoom", 0);
		}
		Application.Quit();
	}

	// Token: 0x04000A8C RID: 2700
	public static PauseMenuManager instance;

	// Token: 0x04000A8D RID: 2701
	[SerializeField]
	private PhotonView view;

	// Token: 0x04000A91 RID: 2705
	[SerializeField]
	private GameObject pauseMenuObject;

	// Token: 0x04000A92 RID: 2706
	[SerializeField]
	private Transform eye;

	// Token: 0x04000A93 RID: 2707
	[SerializeField]
	private GameObject player2Object;

	// Token: 0x04000A94 RID: 2708
	[SerializeField]
	private Text player2Text;

	// Token: 0x04000A95 RID: 2709
	[SerializeField]
	private Text player2ValueText;

	// Token: 0x04000A96 RID: 2710
	[SerializeField]
	private Slider player2Slider;

	// Token: 0x04000A97 RID: 2711
	private int player2ActorID = 999;

	// Token: 0x04000A98 RID: 2712
	[SerializeField]
	private GameObject player3Object;

	// Token: 0x04000A99 RID: 2713
	[SerializeField]
	private Text player3Text;

	// Token: 0x04000A9A RID: 2714
	[SerializeField]
	private Text player3ValueText;

	// Token: 0x04000A9B RID: 2715
	[SerializeField]
	private Slider player3Slider;

	// Token: 0x04000A9C RID: 2716
	private int player3ActorID = 999;

	// Token: 0x04000A9D RID: 2717
	[SerializeField]
	private GameObject player4Object;

	// Token: 0x04000A9E RID: 2718
	[SerializeField]
	private Text player4Text;

	// Token: 0x04000A9F RID: 2719
	[SerializeField]
	private Text player4ValueText;

	// Token: 0x04000AA0 RID: 2720
	[SerializeField]
	private Slider player4Slider;

	// Token: 0x04000AA1 RID: 2721
	private int player4ActorID = 999;
}

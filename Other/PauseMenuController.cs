using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using Photon.Pun;

// Token: 0x020000DC RID: 220
public class PauseMenuController : MonoBehaviour
{
	// Token: 0x06000639 RID: 1593 RVA: 0x00024C6F File Offset: 0x00022E6F
	private void Awake()
	{
		PauseMenuController.instance = this;
		if (XRDevice.isPresent)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x00024C8C File Offset: 0x00022E8C
	private void Start()
	{
		if (PhotonNetwork.PlayerList.Length != 0)
		{
			this.player2Text.text = PhotonNetwork.PlayerList[0].NickName;
			this.player2Object.SetActive(true);
			this.player2ActorID = int.Parse(PhotonNetwork.PlayerList[0].UserId);
		}
		if (PhotonNetwork.PlayerList.Length > 1)
		{
			this.player3Text.text = PhotonNetwork.PlayerList[1].NickName;
			this.player3Object.SetActive(true);
			this.player3ActorID = int.Parse(PhotonNetwork.PlayerList[1].UserId);
		}
		if (PhotonNetwork.PlayerList.Length > 2)
		{
			this.player4Text.text = PhotonNetwork.PlayerList[2].NickName;
			this.player4Object.SetActive(true);
			this.player4ActorID = int.Parse(PhotonNetwork.PlayerList[2].UserId);
		}
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x00024D54 File Offset: 0x00022F54
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

	// Token: 0x0600063C RID: 1596 RVA: 0x00024DF8 File Offset: 0x00022FF8
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

	// Token: 0x0600063D RID: 1597 RVA: 0x00024E9C File Offset: 0x0002309C
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

	// Token: 0x0600063E RID: 1598 RVA: 0x00024F3D File Offset: 0x0002313D
	public void Pause(bool isPaused)
	{
		this.content.SetActive(isPaused);
		this.areYouSure.SetActive(false);
	}

	// Token: 0x0600063F RID: 1599 RVA: 0x00024F58 File Offset: 0x00023158
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

	// Token: 0x06000640 RID: 1600 RVA: 0x00024FA9 File Offset: 0x000231A9
	public void Resume()
	{
		GameController.instance.myPlayer.player.pcCanvas.Pause();
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x00024FD0 File Offset: 0x000231D0
	public void Leave()
	{
		if (GameController.instance.isTutorial)
		{
			PlayerPrefs.SetInt("MissionStatus", 3);
			PlayerPrefs.SetInt("setupPhase", 0);
			PlayerPrefs.SetInt("completedTraining", 1);
			PlayerPrefs.SetInt("StayInServerRoom", 0);
		}
		SceneManager.LoadScene("Menu_New");
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x0002501F File Offset: 0x0002321F
	public void Quit()
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

	// Token: 0x04000600 RID: 1536
	public static PauseMenuController instance;

	// Token: 0x04000601 RID: 1537
	[SerializeField]
	private GameObject content;

	// Token: 0x04000602 RID: 1538
	[SerializeField]
	private GameObject areYouSure;

	// Token: 0x04000603 RID: 1539
	[SerializeField]
	private GameObject player2Object;

	// Token: 0x04000604 RID: 1540
	[SerializeField]
	private Text player2Text;

	// Token: 0x04000605 RID: 1541
	[SerializeField]
	private Text player2ValueText;

	// Token: 0x04000606 RID: 1542
	[SerializeField]
	private Slider player2Slider;

	// Token: 0x04000607 RID: 1543
	private int player2ActorID = 999;

	// Token: 0x04000608 RID: 1544
	[SerializeField]
	private GameObject player3Object;

	// Token: 0x04000609 RID: 1545
	[SerializeField]
	private Text player3Text;

	// Token: 0x0400060A RID: 1546
	[SerializeField]
	private Text player3ValueText;

	// Token: 0x0400060B RID: 1547
	[SerializeField]
	private Slider player3Slider;

	// Token: 0x0400060C RID: 1548
	private int player3ActorID = 999;

	// Token: 0x0400060D RID: 1549
	[SerializeField]
	private GameObject player4Object;

	// Token: 0x0400060E RID: 1550
	[SerializeField]
	private Text player4Text;

	// Token: 0x0400060F RID: 1551
	[SerializeField]
	private Text player4ValueText;

	// Token: 0x04000610 RID: 1552
	[SerializeField]
	private Slider player4Slider;

	// Token: 0x04000611 RID: 1553
	private int player4ActorID = 999;
}

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using Photon.Pun;

public class PauseMenuController : MonoBehaviour
{
	private void Awake()
	{
		PauseMenuController.instance = this;
		if (XRDevice.isPresent)
		{
			base.gameObject.SetActive(false);
		}
	}

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

	public void Pause(bool isPaused)
	{
		this.content.SetActive(isPaused);
		this.areYouSure.SetActive(false);
	}

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

	public void Resume()
	{
		GameController.instance.myPlayer.player.pcCanvas.Pause();
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

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

	public static PauseMenuController instance;

	[SerializeField]
	private GameObject content;

	[SerializeField]
	private GameObject areYouSure;

	[SerializeField]
	private GameObject player2Object;

	[SerializeField]
	private Text player2Text;

	[SerializeField]
	private Text player2ValueText;

	[SerializeField]
	private Slider player2Slider;

	private int player2ActorID = 999;

	[SerializeField]
	private GameObject player3Object;

	[SerializeField]
	private Text player3Text;

	[SerializeField]
	private Text player3ValueText;

	[SerializeField]
	private Slider player3Slider;

	private int player3ActorID = 999;

	[SerializeField]
	private GameObject player4Object;

	[SerializeField]
	private Text player4Text;

	[SerializeField]
	private Text player4ValueText;

	[SerializeField]
	private Slider player4Slider;

	private int player4ActorID = 999;
}


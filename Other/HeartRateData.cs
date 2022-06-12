using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;

public class HeartRateData : MonoBehaviour
{
	private void Start()
	{
		this.SetupUI();
		GameController.instance.OnPlayerSpawned.AddListener(new UnityAction(this.SetupUI));
		GameController.instance.OnPlayerDisconnected.AddListener(new UnityAction(this.SetupUI));
	}

	private void SetupUI()
	{
		this.player1HRObject.SetActive(false);
		this.player2HRObject.SetActive(false);
		this.player3HRObject.SetActive(false);
		this.player4HRObject.SetActive(false);
		if (PhotonNetwork.PlayerList.Length != 0)
		{
			this.player1HRObject.SetActive(true);
		}
		if (PhotonNetwork.PlayerList.Length > 1)
		{
			this.player2HRObject.SetActive(true);
		}
		if (PhotonNetwork.PlayerList.Length > 2)
		{
			this.player3HRObject.SetActive(true);
		}
		if (PhotonNetwork.PlayerList.Length > 3)
		{
			this.player4HRObject.SetActive(true);
		}
	}

	private void UpdateUI()
	{
		if (GameController.instance.playersData.Count > 0)
		{
			this.player1NameText.text = GameController.instance.playersData[0].playerName;
			this.player1HeartRateText.text = (GameController.instance.playersData[0].player.isDead ? "?" : (Mathf.Clamp(100f - GameController.instance.playersData[0].player.insanity + UnityEngine.Random.Range(-2f, 3f), 0f, 100f).ToString("0") + "%"));
		}
		if (GameController.instance.playersData.Count > 1)
		{
			this.player2NameText.text = GameController.instance.playersData[1].playerName;
			this.player2HeartRateText.text = (GameController.instance.playersData[1].player.isDead ? "?" : (Mathf.Clamp(100f - GameController.instance.playersData[1].player.insanity + UnityEngine.Random.Range(-2f, 3f), 0f, 100f).ToString("0") + "%"));
		}
		if (GameController.instance.playersData.Count > 2)
		{
			this.player3NameText.text = GameController.instance.playersData[2].playerName;
			this.player3HeartRateText.text = (GameController.instance.playersData[2].player.isDead ? "?" : (Mathf.Clamp(100f - GameController.instance.playersData[2].player.insanity + UnityEngine.Random.Range(-2f, 3f), 0f, 100f).ToString("0") + "%"));
		}
		if (GameController.instance.playersData.Count > 3)
		{
			this.player4NameText.text = GameController.instance.playersData[3].playerName;
			this.player4HeartRateText.text = (GameController.instance.playersData[3].player.isDead ? "?" : (Mathf.Clamp(100f - GameController.instance.playersData[3].player.insanity + UnityEngine.Random.Range(-2f, 3f), 0f, 100f).ToString("0") + "%"));
		}
	}

	private void Update()
	{
		this.updateTimer -= Time.deltaTime;
		if (this.updateTimer < 0f)
		{
			this.updateTimer = 2f;
			this.UpdateUI();
		}
	}

	[SerializeField]
	private GameObject player1HRObject;

	[SerializeField]
	private GameObject player2HRObject;

	[SerializeField]
	private GameObject player3HRObject;

	[SerializeField]
	private GameObject player4HRObject;

	[SerializeField]
	private Text player1NameText;

	[SerializeField]
	private Text player2NameText;

	[SerializeField]
	private Text player3NameText;

	[SerializeField]
	private Text player4NameText;

	[SerializeField]
	private Text player1HeartRateText;

	[SerializeField]
	private Text player2HeartRateText;

	[SerializeField]
	private Text player3HeartRateText;

	[SerializeField]
	private Text player4HeartRateText;

	private float updateTimer = 1f;
}


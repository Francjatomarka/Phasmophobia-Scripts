using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;

// Token: 0x0200017D RID: 381
public class HeartRateData : MonoBehaviour
{
	// Token: 0x06000AD9 RID: 2777 RVA: 0x000440AE File Offset: 0x000422AE
	private void Start()
	{
		this.SetupUI();
		GameController.instance.OnPlayerSpawned.AddListener(new UnityAction(this.SetupUI));
		GameController.instance.OnPlayerDisconnected.AddListener(new UnityAction(this.SetupUI));
	}

	// Token: 0x06000ADA RID: 2778 RVA: 0x000440EC File Offset: 0x000422EC
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

	// Token: 0x06000ADB RID: 2779 RVA: 0x00044180 File Offset: 0x00042380
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

	// Token: 0x06000ADC RID: 2780 RVA: 0x00044471 File Offset: 0x00042671
	private void Update()
	{
		this.updateTimer -= Time.deltaTime;
		if (this.updateTimer < 0f)
		{
			this.updateTimer = 2f;
			this.UpdateUI();
		}
	}

	// Token: 0x04000B35 RID: 2869
	[SerializeField]
	private GameObject player1HRObject;

	// Token: 0x04000B36 RID: 2870
	[SerializeField]
	private GameObject player2HRObject;

	// Token: 0x04000B37 RID: 2871
	[SerializeField]
	private GameObject player3HRObject;

	// Token: 0x04000B38 RID: 2872
	[SerializeField]
	private GameObject player4HRObject;

	// Token: 0x04000B39 RID: 2873
	[SerializeField]
	private Text player1NameText;

	// Token: 0x04000B3A RID: 2874
	[SerializeField]
	private Text player2NameText;

	// Token: 0x04000B3B RID: 2875
	[SerializeField]
	private Text player3NameText;

	// Token: 0x04000B3C RID: 2876
	[SerializeField]
	private Text player4NameText;

	// Token: 0x04000B3D RID: 2877
	[SerializeField]
	private Text player1HeartRateText;

	// Token: 0x04000B3E RID: 2878
	[SerializeField]
	private Text player2HeartRateText;

	// Token: 0x04000B3F RID: 2879
	[SerializeField]
	private Text player3HeartRateText;

	// Token: 0x04000B40 RID: 2880
	[SerializeField]
	private Text player4HeartRateText;

	// Token: 0x04000B41 RID: 2881
	private float updateTimer = 1f;
}

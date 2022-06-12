using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ErrorManager : MonoBehaviour
{
	private void Start()
	{
		this.ErrorScreenText.text = PlayerPrefs.GetString("ErrorMessage");
		PlayerPrefs.SetString("ErrorMessage", string.Empty);
	}

	public void ResumeButton()
	{
		if (PhotonNetwork.InRoom)
		{
			MainManager.instance.serverManager.EnableMasks(true);
			base.gameObject.SetActive(false);
			return;
		}
		this.mainObject.SetActive(true);
		base.gameObject.SetActive(false);
	}

	[SerializeField]
	private Text ErrorScreenText;

	[SerializeField]
	private GameObject mainObject;
}


using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PasswordManager : MonoBehaviourPunCallbacks
{
	public override void OnEnable()
	{
		this.hasEnteredCode = false;
		this.password = "";
		this.lobbyManager.RefreshList();
	}

	public void NumberButton(int number)
	{
		if (!this.hasEnteredCode)
		{
			this.codeText.text = "";
			this.hasEnteredCode = true;
		}
		if (this.password.Length < 6)
		{
			this.password += number;
		}
		this.codeText.text = this.password;
		this.errorText.text = "";
	}

	public void ErrorJoiningRoom()
    {
		errorText.text = "Fail Joining Room";
    }

	public void EnterButton()
	{
		errorText.text = "";
		this.lobbyManager.JoinServerByName(this.password);
	}

	public void RemoveButton()
	{
		this.errorText.text = "";
		if (this.password.Length > 0 && this.hasEnteredCode)
		{
			this.password = this.password.Remove(this.password.Length - 1, 1);
			this.codeText.text = this.password;
		}
	}

	[SerializeField]
	private Text codeText;

	private bool hasEnteredCode;

	private string password;

	[SerializeField]
	private LobbyManager lobbyManager;

	[SerializeField]
	private Text errorText;

	private RoomInfo[] rooms;
}


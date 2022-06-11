using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000187 RID: 391
public class PasswordManager : MonoBehaviourPunCallbacks
{
	// Token: 0x06000AA9 RID: 2729 RVA: 0x0004274C File Offset: 0x0004094C
	public override void OnEnable()
	{
		this.hasEnteredCode = false;
		this.password = "";
		this.lobbyManager.RefreshList();
	}

	// Token: 0x06000AAA RID: 2730 RVA: 0x0004276C File Offset: 0x0004096C
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

	// Token: 0x06000AAC RID: 2732 RVA: 0x000427F3 File Offset: 0x000409F3
	public void EnterButton()
	{
		errorText.text = "";
		this.lobbyManager.JoinServerByName(this.password);
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x00042808 File Offset: 0x00040A08
	public void RemoveButton()
	{
		this.errorText.text = "";
		if (this.password.Length > 0 && this.hasEnteredCode)
		{
			this.password = this.password.Remove(this.password.Length - 1, 1);
			this.codeText.text = this.password;
		}
	}

	// Token: 0x04000AE3 RID: 2787
	[SerializeField]
	private Text codeText;

	// Token: 0x04000AE4 RID: 2788
	private bool hasEnteredCode;

	// Token: 0x04000AE5 RID: 2789
	private string password;

	// Token: 0x04000AE6 RID: 2790
	[SerializeField]
	private LobbyManager lobbyManager;

	// Token: 0x04000AE7 RID: 2791
	[SerializeField]
	private Text errorText;

	// Token: 0x04000AE8 RID: 2792
	private RoomInfo[] rooms;
}

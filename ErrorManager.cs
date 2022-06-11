using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// Token: 0x02000135 RID: 309
public class ErrorManager : MonoBehaviour
{
	// Token: 0x0600088A RID: 2186 RVA: 0x0003436E File Offset: 0x0003256E
	private void Start()
	{
		this.ErrorScreenText.text = PlayerPrefs.GetString("ErrorMessage");
		PlayerPrefs.SetString("ErrorMessage", string.Empty);
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x00034394 File Offset: 0x00032594
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

	// Token: 0x0400089F RID: 2207
	[SerializeField]
	private Text ErrorScreenText;

	// Token: 0x040008A0 RID: 2208
	[SerializeField]
	private GameObject mainObject;
}

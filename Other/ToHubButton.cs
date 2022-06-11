using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

// Token: 0x02000055 RID: 85
public class ToHubButton : MonoBehaviour
{
	// Token: 0x1700002B RID: 43
	// (get) Token: 0x060001CC RID: 460 RVA: 0x0000C7A5 File Offset: 0x0000A9A5
	public static ToHubButton Instance
	{
		get
		{
			if (ToHubButton.instance == null)
			{
				ToHubButton.instance = (FindObjectOfType(typeof(ToHubButton)) as ToHubButton);
			}
			return ToHubButton.instance;
		}
	}

	// Token: 0x060001CD RID: 461 RVA: 0x0000C7D2 File Offset: 0x0000A9D2
	public void Awake()
	{
		if (ToHubButton.Instance != null && ToHubButton.Instance != this)
		{
			Destroy(base.gameObject);
		}
	}

	// Token: 0x060001CE RID: 462 RVA: 0x0000C7F9 File Offset: 0x0000A9F9
	public void Start()
	{
		if (this.ButtonTexture == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x060001CF RID: 463 RVA: 0x0000C824 File Offset: 0x0000AA24
	public void OnGUI()
	{
		if (SceneManager.GetActiveScene().buildIndex != 0)
		{
			int num = this.ButtonTexture.width + 4;
			int num2 = this.ButtonTexture.height + 4;
			this.ButtonRect = new Rect((float)(Screen.width - num), (float)(Screen.height - num2), (float)num, (float)num2);
			if (GUI.Button(this.ButtonRect, this.ButtonTexture, GUIStyle.none))
			{
				PhotonNetwork.Disconnect();
				SceneManager.LoadScene(0);
			}
		}
	}

	// Token: 0x040001F4 RID: 500
	public Texture2D ButtonTexture;

	// Token: 0x040001F5 RID: 501
	private Rect ButtonRect;

	// Token: 0x040001F6 RID: 502
	private static ToHubButton instance;
}

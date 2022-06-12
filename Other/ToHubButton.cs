using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ToHubButton : MonoBehaviour
{
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

	public void Awake()
	{
		if (ToHubButton.Instance != null && ToHubButton.Instance != this)
		{
			Destroy(base.gameObject);
		}
	}

	public void Start()
	{
		if (this.ButtonTexture == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		DontDestroyOnLoad(base.gameObject);
	}

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

	public Texture2D ButtonTexture;

	private Rect ButtonRect;

	private static ToHubButton instance;
}


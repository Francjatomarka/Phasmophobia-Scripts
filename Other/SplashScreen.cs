using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class SplashScreen : MonoBehaviour
{
	private float timer = 5.1f;

	[SerializeField]
	private GameObject mainScreen;

	[SerializeField]
	private GameObject loadingScreen;

	private int index;

	[SerializeField]
	private Text mainText;

	[SerializeField]
	private GameObject fadeObject;

	[SerializeField]
	private GameObject firstIcon;

	[SerializeField]
	private LoadingAsyncManager loadingManager;

	private void Update()
	{
		if (XRDevice.isPresent)
		{
			return;
		}
		this.timer -= Time.deltaTime;
		if (this.timer < 0f)
		{
			this.timer = 5.1f;
			this.index++;
			if (this.index == 1)
			{
				this.mainText.text = "Testing Phasmophobia by Dani";
				this.firstIcon.SetActive(false);
				base.StartCoroutine(this.ResetFade());
				return;
			}
			if (this.index == 2)
			{
				this.mainScreen.SetActive(false);
				this.loadingScreen.SetActive(true);
				loadingManager.LoadScene("Menu_New");
				//Execute function after Texts shows, probably you should like to load a scene(main menu)
			}
		}
	}

	private IEnumerator ResetFade()
	{
		this.fadeObject.SetActive(false);
		yield return new WaitForEndOfFrame();
		this.fadeObject.SetActive(true);
		yield break;
	}
}


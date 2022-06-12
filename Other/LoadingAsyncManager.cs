using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class LoadingAsyncManager : MonoBehaviour
{
	public void LoadScene(string levelToLoad)
	{
		base.StartCoroutine(this.PCLoadLevel(levelToLoad));
	}

	private IEnumerator PCLoadLevel(string levelToLoad)
	{
		AsyncOperation async = SceneManager.LoadSceneAsync(levelToLoad);
		while (!async.isDone)
		{
			if (this.progressText)
			{
				this.progressText.text = (async.progress * 100f).ToString("0") + "%";
			}
			/*if (async.progress == 0.9f && MainManager.instance && MainManager.instance.localPlayer && XRDevice.isPresent)
			{
				MainManager.instance.localPlayer.pcCanvas.LoadingGame();
			}*/
			yield return null;
		}
		/*if (MainManager.instance && MainManager.instance.localPlayer)
		{
			MainManager.instance.localPlayer.gameObject.SetActive(false);
		}*/
		yield break;
	}

	[SerializeField]
	private Text progressText;
}


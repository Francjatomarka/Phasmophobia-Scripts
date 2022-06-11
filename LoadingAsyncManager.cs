using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

// Token: 0x0200013C RID: 316
public class LoadingAsyncManager : MonoBehaviour
{
	// Token: 0x060008CF RID: 2255 RVA: 0x00036222 File Offset: 0x00034422
	public void LoadScene(string levelToLoad)
	{
		base.StartCoroutine(this.PCLoadLevel(levelToLoad));
	}

	// Token: 0x060008D0 RID: 2256 RVA: 0x00036232 File Offset: 0x00034432
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

	// Token: 0x040008E2 RID: 2274
	[SerializeField]
	private Text progressText;
}

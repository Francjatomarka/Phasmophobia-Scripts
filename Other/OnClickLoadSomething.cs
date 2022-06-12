using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickLoadSomething : MonoBehaviour
{
	public void OnClick()
	{
		OnClickLoadSomething.ResourceTypeOption resourceTypeToLoad = this.ResourceTypeToLoad;
		if (resourceTypeToLoad == OnClickLoadSomething.ResourceTypeOption.Scene)
		{
			SceneManager.LoadScene(this.ResourceToLoad);
			return;
		}
		if (resourceTypeToLoad != OnClickLoadSomething.ResourceTypeOption.Web)
		{
			return;
		}
		Application.OpenURL(this.ResourceToLoad);
	}

	public OnClickLoadSomething.ResourceTypeOption ResourceTypeToLoad;

	public string ResourceToLoad;

	public enum ResourceTypeOption : byte
	{
		Scene,
		Web
	}
}


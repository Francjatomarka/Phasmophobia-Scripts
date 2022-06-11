using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000075 RID: 117
public class OnClickLoadSomething : MonoBehaviour
{
	// Token: 0x060002A2 RID: 674 RVA: 0x0001183C File Offset: 0x0000FA3C
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

	// Token: 0x040002EA RID: 746
	public OnClickLoadSomething.ResourceTypeOption ResourceTypeToLoad;

	// Token: 0x040002EB RID: 747
	public string ResourceToLoad;

	// Token: 0x020004E3 RID: 1251
	public enum ResourceTypeOption : byte
	{
		// Token: 0x040023B5 RID: 9141
		Scene,
		// Token: 0x040023B6 RID: 9142
		Web
	}
}

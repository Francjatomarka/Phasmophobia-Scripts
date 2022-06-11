using System;
using Photon;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000171 RID: 369
public class TrailerVRController : MonoBehaviour
{
	// Token: 0x06000A91 RID: 2705 RVA: 0x000419EC File Offset: 0x0003FBEC
	private void Awake()
	{
		if (SceneManager.GetActiveScene().name == "Menu_New")
		{
			base.enabled = false;
		}
	}
}

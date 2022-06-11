using System;
using UnityEngine;

// Token: 0x0200009B RID: 155
public class QuitOnEscapeOrBack : MonoBehaviour
{
	// Token: 0x060004A0 RID: 1184 RVA: 0x00019B95 File Offset: 0x00017D95
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
}

using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// Token: 0x02000063 RID: 99
public class RpsDebug : MonoBehaviour
{
	// Token: 0x06000235 RID: 565 RVA: 0x0000F1C1 File Offset: 0x0000D3C1
	public void ToggleConnectionDebug()
	{
		this.ShowConnectionDebug = !this.ShowConnectionDebug;
	}

	// Token: 0x06000236 RID: 566 RVA: 0x0000F1D4 File Offset: 0x0000D3D4
	public void Update()
	{
		this.ConnectionDebugButton.GetComponentInChildren<Text>().text = "";
	}

	// Token: 0x04000277 RID: 631
	[SerializeField]
	private Button ConnectionDebugButton;

	// Token: 0x04000278 RID: 632
	public bool ShowConnectionDebug;
}

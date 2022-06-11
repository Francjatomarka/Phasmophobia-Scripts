using System;
using UnityEngine;

// Token: 0x02000011 RID: 17
public class HighlightRecorder : MonoBehaviour
{
	// Token: 0x06000092 RID: 146 RVA: 0x00004C40 File Offset: 0x00002E40
	private void Start()
	{
		this.rendererComp = base.GetComponent<Renderer>();
		if (this.rendererComp == null)
		{
			base.enabled = false;
			return;
		}
	}


	// Token: 0x04000066 RID: 102
	private Renderer rendererComp;
}

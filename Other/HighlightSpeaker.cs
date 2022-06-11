using System;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class HighlightSpeaker : MonoBehaviour
{
	// Token: 0x06000097 RID: 151 RVA: 0x00004E10 File Offset: 0x00003010
	private void Start()
	{
		this.rendererComp = base.GetComponent<Renderer>();
		if (this.rendererComp == null)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0400006B RID: 107
	private Renderer rendererComp;
}

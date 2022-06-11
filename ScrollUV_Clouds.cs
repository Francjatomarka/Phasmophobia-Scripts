using System;
using UnityEngine;

// Token: 0x0200002F RID: 47
public class ScrollUV_Clouds : MonoBehaviour
{
	// Token: 0x06000120 RID: 288 RVA: 0x00008F07 File Offset: 0x00007107
	private void Start()
	{
		this._myRenderer = base.GetComponent<Renderer>();
		if (this._myRenderer == null)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06000121 RID: 289 RVA: 0x00008F2C File Offset: 0x0000712C
	public void FixedUpdate()
	{
		if (this.scroll)
		{
			float y = Time.time * this.verticalScrollSpeed;
			float x = Time.time * this.horizontalScrollSpeed;
			this._myRenderer.material.mainTextureOffset = new Vector2(x, y);
		}
	}

	// Token: 0x06000122 RID: 290 RVA: 0x00008F72 File Offset: 0x00007172
	public void DoActivateTrigger()
	{
		this.scroll = !this.scroll;
	}

	// Token: 0x04000165 RID: 357
	public float horizontalScrollSpeed = 0.25f;

	// Token: 0x04000166 RID: 358
	public float verticalScrollSpeed = 0.25f;

	// Token: 0x04000167 RID: 359
	private Renderer _myRenderer;

	// Token: 0x04000168 RID: 360
	private bool scroll = true;
}

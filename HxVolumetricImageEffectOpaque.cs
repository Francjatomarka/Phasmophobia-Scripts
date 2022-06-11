using System;
using UnityEngine;

// Token: 0x0200000C RID: 12
[ExecuteInEditMode]
public class HxVolumetricImageEffectOpaque : HxVolumetricRenderCallback
{
	// Token: 0x060000B2 RID: 178 RVA: 0x00009485 File Offset: 0x00007685
	private void OnEnable()
	{
		this.RenderOrder = HxVolumetricCamera.hxRenderOrder.ImageEffectOpaque;
		if (this.volumetricCamera == null)
		{
			this.volumetricCamera = base.GetComponent<HxVolumetricCamera>();
		}
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x0000943E File Offset: 0x0000763E
	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		if (this.volumetricCamera == null)
		{
			this.volumetricCamera = base.GetComponent<HxVolumetricCamera>();
		}
		if (this.volumetricCamera == null)
		{
			Graphics.Blit(src, dest);
			return;
		}
		this.volumetricCamera.EventOnRenderImage(src, dest);
	}
}

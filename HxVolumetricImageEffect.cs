using System;
using UnityEngine;

// Token: 0x0200000B RID: 11
[ExecuteInEditMode]
public class HxVolumetricImageEffect : HxVolumetricRenderCallback
{
	// Token: 0x060000AF RID: 175 RVA: 0x0000941B File Offset: 0x0000761B
	private void OnEnable()
	{
		this.RenderOrder = HxVolumetricCamera.hxRenderOrder.ImageEffect;
		if (this.volumetricCamera == null)
		{
			this.volumetricCamera = base.GetComponent<HxVolumetricCamera>();
		}
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x0000943E File Offset: 0x0000763E
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

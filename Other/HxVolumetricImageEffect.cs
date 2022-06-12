using System;
using UnityEngine;

[ExecuteInEditMode]
public class HxVolumetricImageEffect : HxVolumetricRenderCallback
{
	private void OnEnable()
	{
		this.RenderOrder = HxVolumetricCamera.hxRenderOrder.ImageEffect;
		if (this.volumetricCamera == null)
		{
			this.volumetricCamera = base.GetComponent<HxVolumetricCamera>();
		}
	}

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


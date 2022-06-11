using System;
using UnityEngine;

// Token: 0x020000F7 RID: 247
public static class StandardShaderUtils
{
	// Token: 0x060006B9 RID: 1721 RVA: 0x000279AC File Offset: 0x00025BAC
	public static void ChangeRenderMode(Material standardShaderMaterial, StandardShaderUtils.BlendMode blendMode)
	{
		switch (blendMode)
		{
		case StandardShaderUtils.BlendMode.Opaque:
			standardShaderMaterial.SetInt("_SrcBlend", 1);
			standardShaderMaterial.SetInt("_DstBlend", 0);
			standardShaderMaterial.SetInt("_ZWrite", 1);
			standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
			standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
			standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			standardShaderMaterial.renderQueue = -1;
			return;
		case StandardShaderUtils.BlendMode.Cutout:
			standardShaderMaterial.SetInt("_SrcBlend", 1);
			standardShaderMaterial.SetInt("_DstBlend", 0);
			standardShaderMaterial.SetInt("_ZWrite", 1);
			standardShaderMaterial.EnableKeyword("_ALPHATEST_ON");
			standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
			standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			standardShaderMaterial.renderQueue = 2450;
			return;
		case StandardShaderUtils.BlendMode.Fade:
			standardShaderMaterial.SetInt("_SrcBlend", 5);
			standardShaderMaterial.SetInt("_DstBlend", 10);
			standardShaderMaterial.SetInt("_ZWrite", 0);
			standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
			standardShaderMaterial.EnableKeyword("_ALPHABLEND_ON");
			standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			standardShaderMaterial.renderQueue = 3000;
			return;
		case StandardShaderUtils.BlendMode.Transparent:
			standardShaderMaterial.SetInt("_SrcBlend", 1);
			standardShaderMaterial.SetInt("_DstBlend", 10);
			standardShaderMaterial.SetInt("_ZWrite", 0);
			standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
			standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
			standardShaderMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
			standardShaderMaterial.renderQueue = 3000;
			return;
		default:
			return;
		}
	}

	// Token: 0x020004BC RID: 1212
	public enum BlendMode
	{
		// Token: 0x0400227A RID: 8826
		Opaque,
		// Token: 0x0400227B RID: 8827
		Cutout,
		// Token: 0x0400227C RID: 8828
		Fade,
		// Token: 0x0400227D RID: 8829
		Transparent
	}
}

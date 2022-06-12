using System;
using UnityEngine;

public static class StandardShaderUtils
{
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

	public enum BlendMode
	{
		Opaque,
		Cutout,
		Fade,
		Transparent
	}
}


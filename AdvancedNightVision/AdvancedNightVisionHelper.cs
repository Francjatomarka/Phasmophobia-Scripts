using System;
using UnityEngine;

namespace AdvancedNightVision
{
	// Token: 0x0200041A RID: 1050
	public static class AdvancedNightVisionHelper
	{
		// Token: 0x060021A0 RID: 8608 RVA: 0x000A2E09 File Offset: 0x000A1009
		public static int Clamp(int value, int min, int max)
		{
			if (value < min)
			{
				return min;
			}
			if (value <= max)
			{
				return value;
			}
			return max;
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060021A1 RID: 8609 RVA: 0x000A2E18 File Offset: 0x000A1018
		public static bool SupportsDX11
		{
			get
			{
				return SystemInfo.graphicsShaderLevel >= 50 && SystemInfo.supportsComputeShaders;
			}
		}

		// Token: 0x060021A2 RID: 8610 RVA: 0x000A2E2C File Offset: 0x000A102C
		public static bool CheckHardwareRequirements(bool needRT, bool needDepth, bool needHDR, MonoBehaviour effect)
		{
			if (!SystemInfo.supportsImageEffects)
			{
				Debug.LogErrorFormat("Hardware not support Image Effects. '{0}' disabled.", new object[]
				{
					effect.ToString()
				});
				effect.enabled = false;
			}
			else if (needRT && !SystemInfo.supportsRenderTextures)
			{
				Debug.LogErrorFormat("Hardware not support Render Textures. '{0}' disabled.", new object[]
				{
					effect.ToString()
				});
				effect.enabled = false;
			}
			else if (needDepth && !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
			{
				Debug.LogErrorFormat("Hardware not support Depth Buffer. '{0}' disabled.", new object[]
				{
					effect.ToString()
				});
				effect.enabled = false;
			}
			else if (needHDR && !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf))
			{
				Debug.LogErrorFormat("Hardware not support HDR. '{0}' disabled.", new object[]
				{
					effect.ToString()
				});
				effect.enabled = false;
			}
			return true;
		}

		// Token: 0x060021A3 RID: 8611 RVA: 0x000A2EEC File Offset: 0x000A10EC
		public static bool IsSupported(Shader shader, MonoBehaviour effect)
		{
			if (shader == null || !shader.isSupported)
			{
				Debug.LogErrorFormat("Shader not supported. '{0}' disabled.\n{1}Please contact to 'hello@ibuprogames.com'.", new object[]
				{
					effect.ToString(),
					(((AdvancedNightVision)effect).ShaderPass == AdvancedNightVision.ShaderPasses.MultiPass) ? "You can try the 'One Pass' mode.\n" : string.Empty
				});
				effect.enabled = false;
			}
			return true;
		}
	}
}

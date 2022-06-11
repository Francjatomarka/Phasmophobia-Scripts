using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.XR;

// Token: 0x020000EE RID: 238
public class Brightness : MonoBehaviour
{
	// Token: 0x06000694 RID: 1684 RVA: 0x00027163 File Offset: 0x00025363
	private void Start()
	{
		this.ApplyBrightnessSetting();
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x0002716C File Offset: 0x0002536C
	public void ApplyBrightnessSetting()
	{
		if (!XRDevice.isPresent)
		{
			this.pcProfile.TryGetSettings<ColorGrading>(out this.colorGradingLayer);
			this.colorGradingLayer.postExposure.value = PlayerPrefs.GetFloat("brightnessValue");
			return;
		}
		this.vrProfile.TryGetSettings<ColorGrading>(out this.colorGradingLayer);
		this.colorGradingLayer.postExposure.value = ((PlayerPrefs.GetFloat("brightnessValue") == 0f) ? 0.8f : PlayerPrefs.GetFloat("brightnessValue"));
	}

	// Token: 0x040006AE RID: 1710
	[SerializeField]
	private PostProcessProfile pcProfile;

	// Token: 0x040006AF RID: 1711
	[SerializeField]
	private PostProcessProfile vrProfile;

	// Token: 0x040006B0 RID: 1712
	private ColorGrading colorGradingLayer;
}

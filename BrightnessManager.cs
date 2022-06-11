using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

// Token: 0x02000131 RID: 305
public class BrightnessManager : MonoBehaviour
{
	// Token: 0x0600086E RID: 2158 RVA: 0x000335ED File Offset: 0x000317ED
	private void Start()
	{
		this.volume.profile.TryGetSettings<ColorGrading>(out this.colorGradingLayer);
		this.SetScreenResolution();
		this.SetDefaultBrightness();
	}

	// Token: 0x0600086F RID: 2159 RVA: 0x00033614 File Offset: 0x00031814
	private void SetScreenResolution()
	{
		if (!XRDevice.isPresent)
		{
			if (PlayerPrefs.GetInt("resolutionValue") == 0)
			{
				PlayerPrefs.SetInt("resolutionValue", Screen.resolutions.Length - 1);
				Screen.SetResolution(Screen.resolutions[PlayerPrefs.GetInt("resolutionValue")].width, Screen.resolutions[PlayerPrefs.GetInt("resolutionValue")].height, true);
				return;
			}
			Screen.SetResolution(Screen.resolutions[PlayerPrefs.GetInt("resolutionValue")].width, Screen.resolutions[PlayerPrefs.GetInt("resolutionValue")].height, PlayerPrefs.GetInt("fullscreenType") == 1);
		}
	}

	// Token: 0x06000870 RID: 2160 RVA: 0x000336C7 File Offset: 0x000318C7
	private void SetDefaultBrightness()
	{
		if (!XRDevice.isPresent)
		{
			this.colorGradingLayer.postExposure.value = this.slider.value;
		}
	}

	// Token: 0x06000871 RID: 2161 RVA: 0x000336EB File Offset: 0x000318EB
	public void SliderValueChanged()
	{
		this.colorGradingLayer.postExposure.value = this.slider.value;
	}

	// Token: 0x06000872 RID: 2162 RVA: 0x00033708 File Offset: 0x00031908
	public void Confirm()
	{
		PlayerPrefs.SetFloat("brightnessValue", this.slider.value);
		SceneManager.LoadScene("Menu_New");
	}

	// Token: 0x0400087B RID: 2171
	[SerializeField]
	private Slider slider;

	// Token: 0x0400087C RID: 2172
	[SerializeField]
	private PostProcessVolume volume;

	// Token: 0x0400087D RID: 2173
	private ColorGrading colorGradingLayer;
}

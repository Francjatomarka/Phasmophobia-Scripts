using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class BrightnessManager : MonoBehaviour
{
	private void Start()
	{
		this.volume.profile.TryGetSettings<ColorGrading>(out this.colorGradingLayer);
		this.SetScreenResolution();
		this.SetDefaultBrightness();
	}

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

	private void SetDefaultBrightness()
	{
		if (!XRDevice.isPresent)
		{
			this.colorGradingLayer.postExposure.value = this.slider.value;
		}
	}

	public void SliderValueChanged()
	{
		this.colorGradingLayer.postExposure.value = this.slider.value;
	}

	public void Confirm()
	{
		PlayerPrefs.SetFloat("brightnessValue", this.slider.value);
		SceneManager.LoadScene("Menu_New");
	}

	[SerializeField]
	private Slider slider;

	[SerializeField]
	private PostProcessVolume volume;

	private ColorGrading colorGradingLayer;
}


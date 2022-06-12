using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.XR;

public class Brightness : MonoBehaviour
{
	private void Start()
	{
		this.ApplyBrightnessSetting();
	}

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

	[SerializeField]
	private PostProcessProfile pcProfile;

	[SerializeField]
	private PostProcessProfile vrProfile;

	private ColorGrading colorGradingLayer;
}


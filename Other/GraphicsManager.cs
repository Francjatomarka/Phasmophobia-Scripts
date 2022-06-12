using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using UnityEngine.XR;

public class GraphicsManager : MonoBehaviour
{
	private void Awake()
	{
		QualitySettings.SetQualityLevel(0, true);
		if (PlayerPrefs.GetInt("GraphicsSet") == 0)
		{
			PlayerPrefs.SetInt("resolutionValue", Screen.resolutions.Length - 1);
			PlayerPrefs.SetInt("fullscreenType", 1);
			PlayerPrefs.SetInt("taaValue", 1);
			PlayerPrefs.SetInt("shadowType", 1);
			PlayerPrefs.SetInt("shadowRes", 2);
			PlayerPrefs.SetInt("textureRes", 0);
			PlayerPrefs.SetInt("anisotropic", 1);
			PlayerPrefs.SetInt("ambientOcclusion", 1);
			PlayerPrefs.SetInt("GraphicsSet", 1);
		}
	}

	private void Start()
	{
		this.LoadValues();
	}

	private void LoadValues()
	{
		this.resolutionValue = PlayerPrefs.GetInt("resolutionValue");
		this.fullscreenType = PlayerPrefs.GetInt("fullscreenType");
		this.taaValue = PlayerPrefs.GetInt("taaValue");
		this.shadowType = PlayerPrefs.GetInt("shadowType");
		this.shadowRes = PlayerPrefs.GetInt("shadowRes");
		this.textureRes = PlayerPrefs.GetInt("textureRes");
		this.anisotropic = PlayerPrefs.GetInt("anisotropic");
		this.ambientOcclusion = PlayerPrefs.GetInt("ambientOcclusion");
		this.brightnessValue = PlayerPrefs.GetFloat("brightnessValue");
		this.brightnessSlider.value = this.brightnessValue;
		if (this.resolutionValue > Screen.resolutions.Length - 1 || this.resolutionValue < 0)
		{
			this.resolutionValue = Screen.resolutions.Length - 1;
			PlayerPrefs.SetInt("resolutionValue", Screen.resolutions.Length - 1);
		}
		QualitySettings.shadows = this.GetShadowQuality();
		QualitySettings.shadowResolution = this.GetShadowResolution();
		QualitySettings.masterTextureLimit = this.textureRes;
		QualitySettings.anisotropicFiltering = this.GetAnisotropic();
		AmbientOcclusion ambientOcclusion = null;
		this.sceneCamVolume.profile.TryGetSettings<AmbientOcclusion>(out ambientOcclusion);
		ambientOcclusion.enabled.value = (PlayerPrefs.GetInt("ambientOcclusion") == 1);
		if (PlayerPrefs.GetInt("taaValue") == 0)
		{
			this.sceneCamLayer.antialiasingMode = PostProcessLayer.Antialiasing.None;
		}
		else
		{
			this.sceneCamLayer.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
		}
		ColorGrading colorGrading = null;
		this.sceneCamVolume.profile.TryGetSettings<ColorGrading>(out colorGrading);
		colorGrading.postExposure.value = PlayerPrefs.GetFloat("brightnessValue");
		this.UpdateUIValues();
	}

	private void SetValues()
	{
		PlayerPrefs.SetInt("resolutionValue", this.resolutionValue);
		PlayerPrefs.SetInt("fullscreenType", this.fullscreenType);
		PlayerPrefs.SetInt("taaValue", this.taaValue);
		PlayerPrefs.SetInt("shadowType", this.shadowType);
		PlayerPrefs.SetInt("shadowRes", this.shadowRes);
		PlayerPrefs.SetInt("textureRes", this.textureRes);
		PlayerPrefs.SetInt("anisotropic", this.anisotropic);
		PlayerPrefs.SetInt("ambientOcclusion", this.ambientOcclusion);
		QualitySettings.shadows = this.GetShadowQuality();
		QualitySettings.shadowResolution = this.GetShadowResolution();
		QualitySettings.masterTextureLimit = this.textureRes;
		QualitySettings.anisotropicFiltering = this.GetAnisotropic();
		if (this.resolutionValue > Screen.resolutions.Length - 1 || this.resolutionValue < 0)
		{
			this.resolutionValue = Screen.resolutions.Length - 1;
			PlayerPrefs.SetInt("resolutionValue", Screen.resolutions.Length - 1);
		}
		Screen.SetResolution(Screen.resolutions[this.resolutionValue].width, Screen.resolutions[this.resolutionValue].height, this.fullscreenType == 1);
		if (!XRDevice.isPresent)
		{
			AmbientOcclusion ambientOcclusion = null;
			MainManager.instance.localPlayer.postProcessingVolume.profile.TryGetSettings<AmbientOcclusion>(out ambientOcclusion);
			ambientOcclusion.enabled.value = (PlayerPrefs.GetInt("ambientOcclusion") == 1);
			this.sceneCamVolume.profile.TryGetSettings<AmbientOcclusion>(out ambientOcclusion);
			ambientOcclusion.enabled.value = (PlayerPrefs.GetInt("ambientOcclusion") == 1);
			if (PlayerPrefs.GetInt("taaValue") == 0)
			{
				MainManager.instance.localPlayer.postProcessingLayer.antialiasingMode = PostProcessLayer.Antialiasing.None;
				this.sceneCamLayer.antialiasingMode = PostProcessLayer.Antialiasing.None;
			}
			else
			{
				MainManager.instance.localPlayer.postProcessingLayer.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
				this.sceneCamLayer.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
			}
		}
		PlayerPrefs.SetFloat("brightnessValue", this.brightnessValue);
		ColorGrading colorGrading = null;
		MainManager.instance.localPlayer.postProcessingVolume.profile.TryGetSettings<ColorGrading>(out colorGrading);
		colorGrading.postExposure.value = PlayerPrefs.GetFloat("brightnessValue");
		this.sceneCamVolume.profile.TryGetSettings<ColorGrading>(out colorGrading);
		colorGrading.postExposure.value = PlayerPrefs.GetFloat("brightnessValue");
	}

	public void ApplyButton()
	{
		this.SetValues();
	}

	private void UpdateUIValues()
	{
		this.resolutionValueText.text = this.GetResolutionText();
		this.fullscreenTypeValueText.text = this.GetFullscreenText();
		this.taaValueText.text = this.GetTaaText();
		this.shadowTypeValueText.text = this.GetShadowTypeText();
		this.shadowResValueText.text = this.GetShadowResolutionText();
		this.textureResValueText.text = this.GetTextureResolutionText();
		this.anisotropicValueText.text = this.GetAnisotropicText();
		this.ambientOcclusionValueText.text = this.GetAmbientOcclusionText();
		this.brightnessValueText.text = this.brightnessValue.ToString("0.0");
	}

	public void ResolutionChangeValue(int value)
	{
		this.resolutionValue += value;
		if (this.resolutionValue > Screen.resolutions.Length - 1)
		{
			this.resolutionValue = Screen.resolutions.Length - 1;
		}
		else if (this.resolutionValue < 0)
		{
			this.resolutionValue = 0;
		}
		this.UpdateUIValues();
	}

	public void FullscreenTypeChangeValue(int value)
	{
		this.fullscreenType += value;
		if (this.fullscreenType < 0)
		{
			this.fullscreenType = 0;
		}
		else if (this.fullscreenType > 0)
		{
			this.fullscreenType = 1;
		}
		this.UpdateUIValues();
	}

	public void TAAChangeValue(int value)
	{
		this.taaValue += value;
		if (this.taaValue < 0)
		{
			this.taaValue = 0;
		}
		else if (this.taaValue > 1)
		{
			this.taaValue = 1;
		}
		this.UpdateUIValues();
	}

	public void ShadowTypeChangeValue(int value)
	{
		this.shadowType += value;
		if (this.shadowType < 0)
		{
			this.shadowType = 0;
		}
		else if (this.shadowType > 0)
		{
			this.shadowType = 1;
		}
		this.UpdateUIValues();
	}

	public void ShadowResolutionChangeValue(int value)
	{
		this.shadowRes += value;
		if (this.shadowRes < 0)
		{
			this.shadowRes = 0;
		}
		else if (this.shadowRes > 3)
		{
			this.shadowRes = 3;
		}
		this.UpdateUIValues();
	}

	public void TextureResolutionChangeValue(int value)
	{
		this.textureRes += value;
		if (this.textureRes < 0)
		{
			this.textureRes = 0;
		}
		else if (this.textureRes > 3)
		{
			this.textureRes = 3;
		}
		this.UpdateUIValues();
	}

	public void AnisotropicChangeValue(int value)
	{
		this.anisotropic += value;
		if (this.anisotropic < 0)
		{
			this.anisotropic = 0;
		}
		else if (this.anisotropic > 0)
		{
			this.anisotropic = 1;
		}
		this.UpdateUIValues();
	}

	public void AmbientOcclusionChangeValue(int value)
	{
		this.ambientOcclusion += value;
		if (this.ambientOcclusion < 0)
		{
			this.ambientOcclusion = 0;
		}
		else if (this.ambientOcclusion > 0)
		{
			this.ambientOcclusion = 1;
		}
		this.UpdateUIValues();
	}

	public void BrightnessChangeValue()
	{
		this.brightnessValue = this.brightnessSlider.value;
		this.UpdateUIValues();
	}

	private string GetResolutionText()
	{
		if (this.resolutionValue > Screen.resolutions.Length - 1 || this.resolutionValue < 0)
		{
			this.resolutionValue = Screen.resolutions.Length - 1;
			PlayerPrefs.SetInt("resolutionValue", Screen.resolutions.Length - 1);
		}
		return string.Concat(new object[]
		{
			Screen.resolutions[this.resolutionValue].width.ToString(),
			"x",
			Screen.resolutions[this.resolutionValue].height.ToString(),
			"@",
			Screen.resolutions[this.resolutionValue].refreshRate,
			"hz"
		});
	}

	private string GetFullscreenText()
	{
		if (this.fullscreenType != 0)
		{
			return LocalisationSystem.GetLocalisedValue("PC_Fullscreen");
		}
		return LocalisationSystem.GetLocalisedValue("PC_Windowed");
	}

	private string GetTaaText()
	{
		string result = string.Empty;
		if (this.taaValue == 0)
		{
			result = LocalisationSystem.GetLocalisedValue("Graphics_Off");
		}
		else
		{
			result = LocalisationSystem.GetLocalisedValue("Graphics_On");
		}
		return result;
	}

	private string GetShadowTypeText()
	{
		string result = string.Empty;
		if (this.shadowType == 0)
		{
			result = LocalisationSystem.GetLocalisedValue("Graphics_Hard");
		}
		else if (this.shadowType == 1)
		{
			result = LocalisationSystem.GetLocalisedValue("Graphics_Soft");
		}
		return result;
	}

	private string GetShadowResolutionText()
	{
		string result = string.Empty;
		if (this.shadowRes == 0)
		{
			result = LocalisationSystem.GetLocalisedValue("Graphics_Low");
		}
		else if (this.shadowRes == 1)
		{
			result = LocalisationSystem.GetLocalisedValue("Graphics_Medium");
		}
		else if (this.shadowRes == 2)
		{
			result = LocalisationSystem.GetLocalisedValue("Graphics_High");
		}
		else
		{
			result = LocalisationSystem.GetLocalisedValue("Graphics_VeryHigh");
		}
		return result;
	}

	private string GetTextureResolutionText()
	{
		string result = string.Empty;
		if (this.textureRes == 0)
		{
			result = LocalisationSystem.GetLocalisedValue("Graphics_Full");
		}
		else if (this.textureRes == 1)
		{
			result = LocalisationSystem.GetLocalisedValue("Graphics_Half");
		}
		else if (this.textureRes == 2)
		{
			result = LocalisationSystem.GetLocalisedValue("Graphics_Quarter");
		}
		else
		{
			result = LocalisationSystem.GetLocalisedValue("Graphics_Eighth");
		}
		return result;
	}

	private string GetAnisotropicText()
	{
		string result = string.Empty;
		if (this.anisotropic == 0)
		{
			result = LocalisationSystem.GetLocalisedValue("Graphics_Off");
		}
		else
		{
			result = LocalisationSystem.GetLocalisedValue("Graphics_On");
		}
		return result;
	}

	private string GetAmbientOcclusionText()
	{
		string result = string.Empty;
		if (this.ambientOcclusion == 0)
		{
			result = LocalisationSystem.GetLocalisedValue("Graphics_Off");
		}
		else
		{
			result = LocalisationSystem.GetLocalisedValue("Graphics_On");
		}
		return result;
	}

	private ShadowQuality GetShadowQuality()
	{
		if (this.shadowType == 0)
		{
			return ShadowQuality.HardOnly;
		}
		return ShadowQuality.All;
	}

	private ShadowResolution GetShadowResolution()
	{
		if (this.shadowRes == 0)
		{
			return ShadowResolution.Low;
		}
		if (this.shadowRes == 1)
		{
			return ShadowResolution.Medium;
		}
		if (this.shadowRes == 2)
		{
			return ShadowResolution.High;
		}
		return ShadowResolution.VeryHigh;
	}

	private AnisotropicFiltering GetAnisotropic()
	{
		if (this.anisotropic == 0)
		{
			return AnisotropicFiltering.Disable;
		}
		return AnisotropicFiltering.ForceEnable;
	}

	private int resolutionValue;

	private int fullscreenType;

	private int taaValue;

	private int shadowType;

	private int shadowRes;

	private int textureRes;

	private int anisotropic;

	private int ambientOcclusion;

	private float brightnessValue;

	[SerializeField]
	private Text resolutionValueText;

	[SerializeField]
	private Text fullscreenTypeValueText;

	[SerializeField]
	private Text taaValueText;

	[SerializeField]
	private Text shadowTypeValueText;

	[SerializeField]
	private Text shadowResValueText;

	[SerializeField]
	private Text textureResValueText;

	[SerializeField]
	private Text anisotropicValueText;

	[SerializeField]
	private Text ambientOcclusionValueText;

	[SerializeField]
	private Slider brightnessSlider;

	[SerializeField]
	private Text brightnessValueText;

	[SerializeField]
	private PostProcessVolume sceneCamVolume;

	[SerializeField]
	private PostProcessLayer sceneCamLayer;
}


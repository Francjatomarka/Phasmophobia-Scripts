using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using UnityEngine.XR;

// Token: 0x02000137 RID: 311
public class GraphicsManager : MonoBehaviour
{
	// Token: 0x06000891 RID: 2193 RVA: 0x000344DC File Offset: 0x000326DC
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

	// Token: 0x06000892 RID: 2194 RVA: 0x00034567 File Offset: 0x00032767
	private void Start()
	{
		this.LoadValues();
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x00034570 File Offset: 0x00032770
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

	// Token: 0x06000894 RID: 2196 RVA: 0x0003470C File Offset: 0x0003290C
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

	// Token: 0x06000895 RID: 2197 RVA: 0x0003495F File Offset: 0x00032B5F
	public void ApplyButton()
	{
		this.SetValues();
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x00034968 File Offset: 0x00032B68
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

	// Token: 0x06000897 RID: 2199 RVA: 0x00034A18 File Offset: 0x00032C18
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

	// Token: 0x06000898 RID: 2200 RVA: 0x00034A6B File Offset: 0x00032C6B
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

	// Token: 0x06000899 RID: 2201 RVA: 0x00034AA3 File Offset: 0x00032CA3
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

	// Token: 0x0600089A RID: 2202 RVA: 0x00034ADB File Offset: 0x00032CDB
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

	// Token: 0x0600089B RID: 2203 RVA: 0x00034B13 File Offset: 0x00032D13
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

	// Token: 0x0600089C RID: 2204 RVA: 0x00034B4B File Offset: 0x00032D4B
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

	// Token: 0x0600089D RID: 2205 RVA: 0x00034B83 File Offset: 0x00032D83
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

	// Token: 0x0600089E RID: 2206 RVA: 0x00034BBB File Offset: 0x00032DBB
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

	// Token: 0x0600089F RID: 2207 RVA: 0x00034BF3 File Offset: 0x00032DF3
	public void BrightnessChangeValue()
	{
		this.brightnessValue = this.brightnessSlider.value;
		this.UpdateUIValues();
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x00034C0C File Offset: 0x00032E0C
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

	// Token: 0x060008A1 RID: 2209 RVA: 0x00034CD5 File Offset: 0x00032ED5
	private string GetFullscreenText()
	{
		if (this.fullscreenType != 0)
		{
			return LocalisationSystem.GetLocalisedValue("PC_Fullscreen");
		}
		return LocalisationSystem.GetLocalisedValue("PC_Windowed");
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x00034CF4 File Offset: 0x00032EF4
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

	// Token: 0x060008A3 RID: 2211 RVA: 0x00034D28 File Offset: 0x00032F28
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

	// Token: 0x060008A4 RID: 2212 RVA: 0x00034D68 File Offset: 0x00032F68
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

	// Token: 0x060008A5 RID: 2213 RVA: 0x00034DC8 File Offset: 0x00032FC8
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

	// Token: 0x060008A6 RID: 2214 RVA: 0x00034E28 File Offset: 0x00033028
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

	// Token: 0x060008A7 RID: 2215 RVA: 0x00034E5C File Offset: 0x0003305C
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

	// Token: 0x060008A8 RID: 2216 RVA: 0x00034E90 File Offset: 0x00033090
	private ShadowQuality GetShadowQuality()
	{
		if (this.shadowType == 0)
		{
			return ShadowQuality.HardOnly;
		}
		return ShadowQuality.All;
	}

	// Token: 0x060008A9 RID: 2217 RVA: 0x00034E9D File Offset: 0x0003309D
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

	// Token: 0x060008AA RID: 2218 RVA: 0x00034EC0 File Offset: 0x000330C0
	private AnisotropicFiltering GetAnisotropic()
	{
		if (this.anisotropic == 0)
		{
			return AnisotropicFiltering.Disable;
		}
		return AnisotropicFiltering.ForceEnable;
	}

	// Token: 0x040008A5 RID: 2213
	private int resolutionValue;

	// Token: 0x040008A6 RID: 2214
	private int fullscreenType;

	// Token: 0x040008A7 RID: 2215
	private int taaValue;

	// Token: 0x040008A8 RID: 2216
	private int shadowType;

	// Token: 0x040008A9 RID: 2217
	private int shadowRes;

	// Token: 0x040008AA RID: 2218
	private int textureRes;

	// Token: 0x040008AB RID: 2219
	private int anisotropic;

	// Token: 0x040008AC RID: 2220
	private int ambientOcclusion;

	// Token: 0x040008AD RID: 2221
	private float brightnessValue;

	// Token: 0x040008AE RID: 2222
	[SerializeField]
	private Text resolutionValueText;

	// Token: 0x040008AF RID: 2223
	[SerializeField]
	private Text fullscreenTypeValueText;

	// Token: 0x040008B0 RID: 2224
	[SerializeField]
	private Text taaValueText;

	// Token: 0x040008B1 RID: 2225
	[SerializeField]
	private Text shadowTypeValueText;

	// Token: 0x040008B2 RID: 2226
	[SerializeField]
	private Text shadowResValueText;

	// Token: 0x040008B3 RID: 2227
	[SerializeField]
	private Text textureResValueText;

	// Token: 0x040008B4 RID: 2228
	[SerializeField]
	private Text anisotropicValueText;

	// Token: 0x040008B5 RID: 2229
	[SerializeField]
	private Text ambientOcclusionValueText;

	// Token: 0x040008B6 RID: 2230
	[SerializeField]
	private Slider brightnessSlider;

	// Token: 0x040008B7 RID: 2231
	[SerializeField]
	private Text brightnessValueText;

	// Token: 0x040008B8 RID: 2232
	[SerializeField]
	private PostProcessVolume sceneCamVolume;

	// Token: 0x040008B9 RID: 2233
	[SerializeField]
	private PostProcessLayer sceneCamLayer;
}

using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using UnityEngine.XR;

// Token: 0x02000142 RID: 322
public class PCManager : MonoBehaviourPunCallbacks
{
	// Token: 0x0600090B RID: 2315 RVA: 0x00037A54 File Offset: 0x00035C54
	private void Awake()
	{
		if (XRDevice.isPresent)
		{
			this.PCButton.interactable = false;
			this.PCButtonText.color = this.PCButton.colors.disabledColor;
			base.enabled = false;
		}
		else
		{
			if (PlayerPrefs.GetInt("fovValue") == 0)
			{
				PlayerPrefs.SetInt("fovValue", 70);
			}
			if (PlayerPrefs.GetFloat("sensitivityValue") == 0f)
			{
				PlayerPrefs.SetFloat("sensitivityValue", 1f);
			}
			if (PlayerPrefs.GetFloat("brightnessValue") == 0f)
			{
				PlayerPrefs.SetFloat("brightnessValue", 1f);
			}
			if (PlayerPrefs.GetFloat("cursorBrightnessValue") == 0f)
			{
				PlayerPrefs.SetFloat("cursorBrightnessValue", 1f);
			}
		}
		QualitySettings.SetQualityLevel(0, true);
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x00037B1A File Offset: 0x00035D1A
	private void Start()
	{
		if (!XRDevice.isPresent)
		{
			this.LoadValues();
		}
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x00037B2C File Offset: 0x00035D2C
	private void LoadValues()
	{
		this.volumetricLightingValue = PlayerPrefs.GetInt("volumetricLightingValue");
		this.fovValue = PlayerPrefs.GetInt("fovValue");
		this.fovSlider.value = (float)this.fovValue;
		this.vsyncValue = PlayerPrefs.GetInt("vsyncValue");
		this.reflectionValue = PlayerPrefs.GetInt("reflectionValue");
		this.sensitivityValue = PlayerPrefs.GetFloat("sensitivityValue");
		this.sensitivitySlider.value = this.sensitivityValue;
		this.cursorBrightnessValue = PlayerPrefs.GetFloat("cursorBrightnessValue");
		this.cursorBrightnessSlider.value = this.cursorBrightnessValue;
		this.invertedXLookValue = PlayerPrefs.GetInt("invertedXLookValue");
		this.invertedYLookValue = PlayerPrefs.GetInt("invertedYLookValue");
		this.localPushToTalkValue = PlayerPrefs.GetInt("localPushToTalkValue");
		this.UpdateUIValues();
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x00037C04 File Offset: 0x00035E04
	public void SetValues()
	{
		PlayerPrefs.SetInt("volumetricLightingValue", this.volumetricLightingValue);
		PlayerPrefs.SetInt("fovValue", this.fovValue);
		PlayerPrefs.SetInt("vsyncValue", this.vsyncValue);
		PlayerPrefs.SetFloat("sensitivityValue", this.sensitivityValue);
		PlayerPrefs.SetInt("reflectionValue", this.reflectionValue);
		PlayerPrefs.SetFloat("cursorBrightnessValue", this.cursorBrightnessValue);
		PlayerPrefs.SetInt("invertedXLookValue", this.invertedXLookValue);
		PlayerPrefs.SetInt("invertedYLookValue", this.invertedYLookValue);
		PlayerPrefs.SetInt("localPushToTalkValue", this.localPushToTalkValue);
		if (XRDevice.isPresent)
		{
			return;
		}
		QualitySettings.vSyncCount = this.vsyncValue;
		MainManager.instance.localPlayer.cam.fieldOfView = (float)PlayerPrefs.GetInt("fovValue");
		MainManager.instance.localPlayer.pcPropGrab.ChangeItemSpotWithFOV((float)PlayerPrefs.GetInt("fovValue"));
		MainManager.instance.localPlayer.itemSway.SetPosition();
		bool flag = PlayerPrefs.GetInt("invertedXLookValue") == 1;
		bool flag2 = PlayerPrefs.GetInt("invertedYLookValue") == 1;
		MainManager.instance.localPlayer.firstPersonController.m_MouseLook.XSensitivity = PlayerPrefs.GetFloat("sensitivityValue") / 10;
		MainManager.instance.localPlayer.firstPersonController.m_MouseLook.YSensitivity = PlayerPrefs.GetFloat("sensitivityValue") / 10;
		MainManager.instance.localPlayer.pcCanvas.UpdateCursorBrightness();
		if (PlayerPrefs.GetInt("volumetricLightingValue") != 0)
		{
			MainManager.instance.localPlayer.headObject.GetComponent<HxVolumetricCamera>().enabled = true;
			MainManager.instance.localPlayer.headObject.GetComponent<HxVolumetricImageEffect>().enabled = true;
			if (PlayerPrefs.GetInt("volumetricLightingValue") == 1)
			{
				MainManager.instance.localPlayer.headObject.GetComponent<HxVolumetricCamera>().resolution = HxVolumetricCamera.Resolution.quarter;
			}
			else if (PlayerPrefs.GetInt("volumetricLightingValue") == 2)
			{
				MainManager.instance.localPlayer.headObject.GetComponent<HxVolumetricCamera>().resolution = HxVolumetricCamera.Resolution.half;
			}
			else if (PlayerPrefs.GetInt("volumetricLightingValue") == 3)
			{
				MainManager.instance.localPlayer.headObject.GetComponent<HxVolumetricCamera>().resolution = HxVolumetricCamera.Resolution.full;
			}
		}
		else
		{
			MainManager.instance.localPlayer.headObject.GetComponent<HxVolumetricCamera>().enabled = false;
			MainManager.instance.localPlayer.headObject.GetComponent<HxVolumetricImageEffect>().enabled = false;
		}
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x00037EC7 File Offset: 0x000360C7
	public void ApplyButton()
	{
		this.SetValues();
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x00037ED0 File Offset: 0x000360D0
	private void UpdateUIValues()
	{
		this.volumetricLightingValueText.text = this.GetVolumetricLightingText();
		this.fovValueText.text = this.fovValue.ToString();
		this.vsyncValueText.text = this.GetVSyncText();
		this.reflectionValueText.text = this.GetReflectionText();
		this.sensitivityValueText.text = this.sensitivityValue.ToString("0.0");
		this.cursorBrightnessValueText.text = this.cursorBrightnessValue.ToString("0.0");
		this.invertedXLookValueText.text = this.GetInvertedXLookText();
		this.invertedYLookValueText.text = this.GetInvertedYLookText();
		this.localPushToTalkValueText.text = this.GetlocalPushToTalkText();
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x00037F8F File Offset: 0x0003618F
	public void VolumetricLightingUIChangeValue(int value)
	{
		this.volumetricLightingValue += value;
		if (this.volumetricLightingValue < 0)
		{
			this.volumetricLightingValue = 0;
		}
		else if (this.volumetricLightingValue > 3)
		{
			this.volumetricLightingValue = 3;
		}
		this.UpdateUIValues();
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x00037FC7 File Offset: 0x000361C7
	public void FOVChangeValue()
	{
		this.fovValue = (int)this.fovSlider.value;
		this.UpdateUIValues();
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x00037FE1 File Offset: 0x000361E1
	public void VSyncChangeValue(int value)
	{
		this.vsyncValue += value;
		if (this.vsyncValue < 0)
		{
			this.vsyncValue = 0;
		}
		else if (this.vsyncValue > 1)
		{
			this.vsyncValue = 1;
		}
		this.UpdateUIValues();
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x00038019 File Offset: 0x00036219
	public void ReflectionChangeValue(int value)
	{
		this.reflectionValue += value;
		if (this.reflectionValue < 0)
		{
			this.reflectionValue = 0;
		}
		else if (this.reflectionValue > 1)
		{
			this.reflectionValue = 1;
		}
		this.UpdateUIValues();
	}

	// Token: 0x06000915 RID: 2325 RVA: 0x00038051 File Offset: 0x00036251
	public void SensitivityChangeValue()
	{
		this.sensitivityValue = this.sensitivitySlider.value;
		this.UpdateUIValues();
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x0003806A File Offset: 0x0003626A
	public void CursorBrightnessChangeValue()
	{
		this.cursorBrightnessValue = this.cursorBrightnessSlider.value;
		this.UpdateUIValues();
	}

	// Token: 0x06000917 RID: 2327 RVA: 0x00038083 File Offset: 0x00036283
	public void InvertedXLookChangeValue(int value)
	{
		this.invertedXLookValue += value;
		if (this.invertedXLookValue < 0)
		{
			this.invertedXLookValue = 0;
		}
		else if (this.invertedXLookValue > 1)
		{
			this.invertedXLookValue = 1;
		}
		this.UpdateUIValues();
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x000380BB File Offset: 0x000362BB
	public void InvertedYLookChangeValue(int value)
	{
		this.invertedYLookValue += value;
		if (this.invertedYLookValue < 0)
		{
			this.invertedYLookValue = 0;
		}
		else if (this.invertedYLookValue > 1)
		{
			this.invertedYLookValue = 1;
		}
		this.UpdateUIValues();
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x000380F3 File Offset: 0x000362F3
	public void LocalPushToTalkChangeValue(int value)
	{
		this.localPushToTalkValue += value;
		if (this.localPushToTalkValue < 0)
		{
			this.localPushToTalkValue = 0;
		}
		else if (this.localPushToTalkValue > 1)
		{
			this.localPushToTalkValue = 1;
		}
		this.UpdateUIValues();
	}

	// Token: 0x0600091A RID: 2330 RVA: 0x0003812C File Offset: 0x0003632C
	private string GetVolumetricLightingText()
	{
		string result = string.Empty;
		if (this.volumetricLightingValue == 0)
		{
			result = LocalisationSystem.GetLocalisedValue("Graphics_Off");
		}
		else if (this.volumetricLightingValue == 1)
		{
			result = LocalisationSystem.GetLocalisedValue("Graphics_Low");
		}
		else if (this.volumetricLightingValue == 2)
		{
			result = LocalisationSystem.GetLocalisedValue("Graphics_Medium");
		}
		else if (this.volumetricLightingValue == 3)
		{
			result = LocalisationSystem.GetLocalisedValue("Graphics_High");
		}
		return result;
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x00038195 File Offset: 0x00036395
	private string GetVSyncText()
	{
		if (this.vsyncValue != 0)
		{
			return LocalisationSystem.GetLocalisedValue("Graphics_On");
		}
		return LocalisationSystem.GetLocalisedValue("Graphics_Off");
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x000381B4 File Offset: 0x000363B4
	private string GetReflectionText()
	{
		if (this.reflectionValue != 0)
		{
			return LocalisationSystem.GetLocalisedValue("Graphics_On");
		}
		return LocalisationSystem.GetLocalisedValue("Graphics_Off");
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x000381D3 File Offset: 0x000363D3
	private string GetInvertedXLookText()
	{
		if (this.invertedXLookValue != 0)
		{
			return LocalisationSystem.GetLocalisedValue("Graphics_On");
		}
		return LocalisationSystem.GetLocalisedValue("Graphics_Off");
	}

	// Token: 0x0600091E RID: 2334 RVA: 0x000381F2 File Offset: 0x000363F2
	private string GetInvertedYLookText()
	{
		if (this.invertedYLookValue != 0)
		{
			return LocalisationSystem.GetLocalisedValue("Graphics_On");
		}
		return LocalisationSystem.GetLocalisedValue("Graphics_Off");
	}

	// Token: 0x0600091F RID: 2335 RVA: 0x00038211 File Offset: 0x00036411
	private string GetlocalPushToTalkText()
	{
		if (this.localPushToTalkValue != 0)
		{
			return LocalisationSystem.GetLocalisedValue("Graphics_Off");
		}
		return LocalisationSystem.GetLocalisedValue("Graphics_On");
	}

	// Token: 0x04000928 RID: 2344
	private int volumetricLightingValue;

	// Token: 0x04000929 RID: 2345
	private int fovValue;

	// Token: 0x0400092A RID: 2346
	private int vsyncValue;

	// Token: 0x0400092B RID: 2347
	private float sensitivityValue;

	// Token: 0x0400092C RID: 2348
	private int reflectionValue;

	// Token: 0x0400092D RID: 2349
	private float cursorBrightnessValue;

	// Token: 0x0400092E RID: 2350
	private int invertedXLookValue;

	// Token: 0x0400092F RID: 2351
	private int invertedYLookValue;

	// Token: 0x04000930 RID: 2352
	private int localPushToTalkValue;

	// Token: 0x04000931 RID: 2353
	[SerializeField]
	private Button PCButton;

	// Token: 0x04000932 RID: 2354
	[SerializeField]
	private Text PCButtonText;

	// Token: 0x04000933 RID: 2355
	[SerializeField]
	private Text volumetricLightingValueText;

	// Token: 0x04000934 RID: 2356
	[SerializeField]
	private Text vsyncValueText;

	// Token: 0x04000935 RID: 2357
	[SerializeField]
	private Text reflectionValueText;

	// Token: 0x04000936 RID: 2358
	[SerializeField]
	private Text invertedXLookValueText;

	// Token: 0x04000937 RID: 2359
	[SerializeField]
	private Text invertedYLookValueText;

	// Token: 0x04000938 RID: 2360
	[SerializeField]
	private Text localPushToTalkValueText;

	// Token: 0x04000939 RID: 2361
	[SerializeField]
	private Slider fovSlider;

	// Token: 0x0400093A RID: 2362
	[SerializeField]
	private Text fovValueText;

	// Token: 0x0400093B RID: 2363
	[SerializeField]
	private Slider sensitivitySlider;

	// Token: 0x0400093C RID: 2364
	[SerializeField]
	private Text sensitivityValueText;

	// Token: 0x0400093D RID: 2365
	[SerializeField]
	private Slider cursorBrightnessSlider;

	// Token: 0x0400093E RID: 2366
	[SerializeField]
	private Text cursorBrightnessValueText;
}

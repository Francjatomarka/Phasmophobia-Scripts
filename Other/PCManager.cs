using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using UnityEngine.XR;

public class PCManager : MonoBehaviourPunCallbacks
{
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

	private void Start()
	{
		if (!XRDevice.isPresent)
		{
			this.LoadValues();
		}
	}

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

	public void ApplyButton()
	{
		this.SetValues();
	}

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

	public void FOVChangeValue()
	{
		this.fovValue = (int)this.fovSlider.value;
		this.UpdateUIValues();
	}

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

	public void SensitivityChangeValue()
	{
		this.sensitivityValue = this.sensitivitySlider.value;
		this.UpdateUIValues();
	}

	public void CursorBrightnessChangeValue()
	{
		this.cursorBrightnessValue = this.cursorBrightnessSlider.value;
		this.UpdateUIValues();
	}

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

	private string GetVSyncText()
	{
		if (this.vsyncValue != 0)
		{
			return LocalisationSystem.GetLocalisedValue("Graphics_On");
		}
		return LocalisationSystem.GetLocalisedValue("Graphics_Off");
	}

	private string GetReflectionText()
	{
		if (this.reflectionValue != 0)
		{
			return LocalisationSystem.GetLocalisedValue("Graphics_On");
		}
		return LocalisationSystem.GetLocalisedValue("Graphics_Off");
	}

	private string GetInvertedXLookText()
	{
		if (this.invertedXLookValue != 0)
		{
			return LocalisationSystem.GetLocalisedValue("Graphics_On");
		}
		return LocalisationSystem.GetLocalisedValue("Graphics_Off");
	}

	private string GetInvertedYLookText()
	{
		if (this.invertedYLookValue != 0)
		{
			return LocalisationSystem.GetLocalisedValue("Graphics_On");
		}
		return LocalisationSystem.GetLocalisedValue("Graphics_Off");
	}

	private string GetlocalPushToTalkText()
	{
		if (this.localPushToTalkValue != 0)
		{
			return LocalisationSystem.GetLocalisedValue("Graphics_Off");
		}
		return LocalisationSystem.GetLocalisedValue("Graphics_On");
	}

	private int volumetricLightingValue;

	private int fovValue;

	private int vsyncValue;

	private float sensitivityValue;

	private int reflectionValue;

	private float cursorBrightnessValue;

	private int invertedXLookValue;

	private int invertedYLookValue;

	private int localPushToTalkValue;

	[SerializeField]
	private Button PCButton;

	[SerializeField]
	private Text PCButtonText;

	[SerializeField]
	private Text volumetricLightingValueText;

	[SerializeField]
	private Text vsyncValueText;

	[SerializeField]
	private Text reflectionValueText;

	[SerializeField]
	private Text invertedXLookValueText;

	[SerializeField]
	private Text invertedYLookValueText;

	[SerializeField]
	private Text localPushToTalkValueText;

	[SerializeField]
	private Slider fovSlider;

	[SerializeField]
	private Text fovValueText;

	[SerializeField]
	private Slider sensitivitySlider;

	[SerializeField]
	private Text sensitivityValueText;

	[SerializeField]
	private Slider cursorBrightnessSlider;

	[SerializeField]
	private Text cursorBrightnessValueText;
}


using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class VRManager : MonoBehaviourPunCallbacks
{
	private void Awake()
	{
		if (!XRDevice.isPresent)
		{
			this.VRButton.interactable = false;
			this.VRButtonText.color = this.VRButton.colors.disabledColor;
			base.enabled = false;
		}
		QualitySettings.SetQualityLevel(0, true);
	}

	private void Start()
	{
		this.LoadValues();
	}

	private void LoadValues()
	{
		this.locomotionValue = PlayerPrefs.GetInt("locomotionValue");
		this.turningValue = PlayerPrefs.GetInt("turningValue");
		this.turningAngleValue = PlayerPrefs.GetInt("turningAngleValue");
		this.movementDirectionValue = PlayerPrefs.GetInt("movementDirectionValue");
		this.grabTypeValue = PlayerPrefs.GetInt("grabTypeValue");
		this.SmoothCamValue = PlayerPrefs.GetInt("SmoothCamValue");
		this.gripTypeValue = PlayerPrefs.GetInt("gripTypeValue");
		this.teleportGrabValue = PlayerPrefs.GetInt("teleportGrabValue");
		this.turningSpeedValue = ((PlayerPrefs.GetInt("turningSpeedValue") == 0) ? 6 : PlayerPrefs.GetInt("turningSpeedValue"));
		this.controllerRotationXValue = PlayerPrefs.GetInt("controllerRotationXValue");
		this.controllerRotationYValue = PlayerPrefs.GetInt("controllerRotationYValue");
		this.controllerRotationZValue = PlayerPrefs.GetInt("controllerRotationZValue");
		this.UpdateUIValues();
	}

	public void SetValues()
	{
		PlayerPrefs.SetInt("locomotionValue", this.locomotionValue);
		PlayerPrefs.SetInt("turningValue", this.turningValue);
		PlayerPrefs.SetInt("turningAngleValue", this.turningAngleValue);
		PlayerPrefs.SetInt("movementDirectionValue", this.movementDirectionValue);
		PlayerPrefs.SetInt("grabTypeValue", this.grabTypeValue);
		PlayerPrefs.SetInt("SmoothCamValue", this.SmoothCamValue);
		PlayerPrefs.SetInt("gripTypeValue", this.gripTypeValue);
		PlayerPrefs.SetInt("teleportGrabValue", this.teleportGrabValue);
		PlayerPrefs.SetInt("turningSpeedValue", (this.turningSpeedValue == 0) ? 6 : this.turningSpeedValue);
		PlayerPrefs.SetInt("controllerRotationXValue", this.controllerRotationXValue);
		PlayerPrefs.SetInt("controllerRotationYValue", this.controllerRotationYValue);
		PlayerPrefs.SetInt("controllerRotationZValue", this.controllerRotationZValue);
		FindObjectOfType<VRMovementSettings>().ApplySettings();
	}

	public void ApplyButton()
	{
		this.SetValues();
	}

	private void UpdateUIValues()
	{
		this.locomotionValueText.text = this.GetLocomotionText();
		this.turningValueText.text = this.GetTurningText();
		this.turningAngleValueText.text = this.GetTurningAngleValueText();
		this.movementDirectionValueText.text = this.GetMovementDirectionValueText();
		this.grabTypeValueText.text = this.GetGrabTypeValueText();
		this.smoothCamValueText.text = this.GetSmoothCamValueText();
		this.gripTypeValueText.text = this.GetGripTypeValueText();
		this.teleportGrabValueText.text = this.GetTeleportGrabText();
		this.turningSpeedValueText.text = this.GetTurningSpeedValueText();
		this.controllerRotationXValueText.text = this.GetControllerXRotationValueText();
		this.controllerRotationYValueText.text = this.GetControllerYRotationValueText();
		this.controllerRotationZValueText.text = this.GetControllerZRotationValueText();
	}

	public void LocomotionChangeValue(int value)
	{
		this.locomotionValue += value;
		if (this.locomotionValue < 0)
		{
			this.locomotionValue = 0;
		}
		else if (this.locomotionValue > 1)
		{
			this.locomotionValue = 1;
		}
		this.UpdateUIValues();
	}

	public void TurningChangeValue(int value)
	{
		this.turningValue += value;
		if (this.turningValue < 0)
		{
			this.turningValue = 0;
		}
		else if (this.turningValue > 1)
		{
			this.turningValue = 1;
		}
		this.UpdateUIValues();
	}

	public void TurningAngleChangeValue(int value)
	{
		this.turningAngleValue += value;
		if (this.turningAngleValue < 0)
		{
			this.turningAngleValue = 0;
		}
		else if (this.turningAngleValue > 3)
		{
			this.turningAngleValue = 3;
		}
		this.UpdateUIValues();
	}

	public void TurningSpeedChangeValue(int value)
	{
		this.turningSpeedValue += value;
		if (this.turningSpeedValue < 1)
		{
			this.turningSpeedValue = 1;
		}
		else if (this.turningSpeedValue > 15)
		{
			this.turningSpeedValue = 15;
		}
		this.UpdateUIValues();
	}

	public void MovementDirectionChangeValue(int value)
	{
		this.movementDirectionValue += value;
		if (this.movementDirectionValue < 0)
		{
			this.movementDirectionValue = 0;
		}
		else if (this.movementDirectionValue > 1)
		{
			this.movementDirectionValue = 1;
		}
		this.UpdateUIValues();
	}

	public void GrabTypeChangeValue(int value)
	{
		this.grabTypeValue += value;
		if (this.grabTypeValue < 0)
		{
			this.grabTypeValue = 0;
		}
		else if (this.grabTypeValue > 1)
		{
			this.grabTypeValue = 1;
		}
		this.UpdateUIValues();
	}

	public void SmoothCamChangeValue(int value)
	{
		this.SmoothCamValue += value;
		if (this.SmoothCamValue < 0)
		{
			this.SmoothCamValue = 0;
		}
		else if (this.SmoothCamValue > 1)
		{
			this.SmoothCamValue = 1;
		}
		this.UpdateUIValues();
	}

	public void GripTypeChangeValue(int value)
	{
		this.gripTypeValue += value;
		if (this.gripTypeValue < 0)
		{
			this.gripTypeValue = 0;
		}
		else if (this.gripTypeValue > 2)
		{
			this.gripTypeValue = 2;
		}
		this.UpdateUIValues();
	}

	public void TeleportGrabChangeValue(int value)
	{
		this.teleportGrabValue += value;
		if (this.teleportGrabValue < 0)
		{
			this.teleportGrabValue = 0;
		}
		else if (this.teleportGrabValue > 1)
		{
			this.teleportGrabValue = 1;
		}
		this.UpdateUIValues();
	}

	public void ControllerRotationXChangeValue(int value)
	{
		this.controllerRotationXValue += value;
		if (this.controllerRotationXValue < -6)
		{
			this.controllerRotationXValue = -6;
		}
		else if (this.controllerRotationXValue > 6)
		{
			this.controllerRotationXValue = 6;
		}
		this.UpdateUIValues();
	}

	public void ControllerRotationYChangeValue(int value)
	{
		this.controllerRotationYValue += value;
		if (this.controllerRotationYValue < -6)
		{
			this.controllerRotationYValue = -6;
		}
		else if (this.controllerRotationYValue > 6)
		{
			this.controllerRotationYValue = 6;
		}
		this.UpdateUIValues();
	}

	public void ControllerRotationZChangeValue(int value)
	{
		this.controllerRotationZValue += value;
		if (this.controllerRotationZValue < -6)
		{
			this.controllerRotationZValue = -6;
		}
		else if (this.controllerRotationZValue > 6)
		{
			this.controllerRotationZValue = 6;
		}
		this.UpdateUIValues();
	}

	private string GetLocomotionText()
	{
		if (this.locomotionValue != 0)
		{
			return LocalisationSystem.GetLocalisedValue("VR_Teleport");
		}
		return LocalisationSystem.GetLocalisedValue("VR_Smooth");
	}

	private string GetTurningText()
	{
		if (this.turningValue != 0)
		{
			return LocalisationSystem.GetLocalisedValue("VR_Snap");
		}
		return LocalisationSystem.GetLocalisedValue("VR_Smooth");
	}

	private string GetTurningAngleValueText()
	{
		if (this.turningAngleValue == 0)
		{
			return "15";
		}
		if (this.turningAngleValue == 1)
		{
			return "45";
		}
		return "90";
	}

	private string GetTurningSpeedValueText()
	{
		return this.turningSpeedValue.ToString("0");
	}

	private string GetMovementDirectionValueText()
	{
		if (this.movementDirectionValue != 0)
		{
			return LocalisationSystem.GetLocalisedValue("VR_MovementHead");
		}
		return LocalisationSystem.GetLocalisedValue("VR_MovementController");
	}

	private string GetGrabTypeValueText()
	{
		if (this.grabTypeValue != 0)
		{
			return LocalisationSystem.GetLocalisedValue("VR_Hold");
		}
		return LocalisationSystem.GetLocalisedValue("VR_Toggle");
	}

	private string GetSmoothCamValueText()
	{
		if (this.SmoothCamValue != 0)
		{
			return LocalisationSystem.GetLocalisedValue("Menu_On");
		}
		return LocalisationSystem.GetLocalisedValue("Menu_Off");
	}

	private string GetGripTypeValueText()
	{
		if (this.gripTypeValue == 0)
		{
			return LocalisationSystem.GetLocalisedValue("VR_GripPress");
		}
		if (this.gripTypeValue == 1)
		{
			return LocalisationSystem.GetLocalisedValue("VR_GripTouch");
		}
		return LocalisationSystem.GetLocalisedValue("VR_GripThreshold");
	}

	private string GetTeleportGrabText()
	{
		if (this.teleportGrabValue != 0)
		{
			return LocalisationSystem.GetLocalisedValue("Menu_On");
		}
		return LocalisationSystem.GetLocalisedValue("Menu_Off");
	}

	private string GetControllerXRotationValueText()
	{
		if (this.controllerRotationXValue == -6)
		{
			return "-90";
		}
		if (this.controllerRotationXValue == -5)
		{
			return "-75";
		}
		if (this.controllerRotationXValue == -4)
		{
			return "-60";
		}
		if (this.controllerRotationXValue == -3)
		{
			return "-45";
		}
		if (this.controllerRotationXValue == -2)
		{
			return "-30";
		}
		if (this.controllerRotationXValue == -1)
		{
			return "-15";
		}
		if (this.controllerRotationXValue == 0)
		{
			return "0";
		}
		if (this.controllerRotationXValue == 1)
		{
			return "15";
		}
		if (this.controllerRotationXValue == 2)
		{
			return "30";
		}
		if (this.controllerRotationXValue == 3)
		{
			return "45";
		}
		if (this.controllerRotationXValue == 4)
		{
			return "60";
		}
		if (this.controllerRotationXValue == 5)
		{
			return "75";
		}
		return "90";
	}

	private string GetControllerYRotationValueText()
	{
		if (this.controllerRotationYValue == -6)
		{
			return "-90";
		}
		if (this.controllerRotationYValue == -5)
		{
			return "-75";
		}
		if (this.controllerRotationYValue == -4)
		{
			return "-60";
		}
		if (this.controllerRotationYValue == -3)
		{
			return "-45";
		}
		if (this.controllerRotationYValue == -2)
		{
			return "-30";
		}
		if (this.controllerRotationYValue == -1)
		{
			return "-15";
		}
		if (this.controllerRotationYValue == 0)
		{
			return "0";
		}
		if (this.controllerRotationYValue == 1)
		{
			return "15";
		}
		if (this.controllerRotationYValue == 2)
		{
			return "30";
		}
		if (this.controllerRotationYValue == 3)
		{
			return "45";
		}
		if (this.controllerRotationYValue == 4)
		{
			return "60";
		}
		if (this.controllerRotationYValue == 5)
		{
			return "75";
		}
		return "90";
	}

	private string GetControllerZRotationValueText()
	{
		if (this.controllerRotationZValue == -6)
		{
			return "-90";
		}
		if (this.controllerRotationZValue == -5)
		{
			return "-75";
		}
		if (this.controllerRotationZValue == -4)
		{
			return "-60";
		}
		if (this.controllerRotationZValue == -3)
		{
			return "-45";
		}
		if (this.controllerRotationZValue == -2)
		{
			return "-30";
		}
		if (this.controllerRotationZValue == -1)
		{
			return "-15";
		}
		if (this.controllerRotationZValue == 0)
		{
			return "0";
		}
		if (this.controllerRotationZValue == 1)
		{
			return "15";
		}
		if (this.controllerRotationZValue == 2)
		{
			return "30";
		}
		if (this.controllerRotationZValue == 3)
		{
			return "45";
		}
		if (this.controllerRotationZValue == 4)
		{
			return "60";
		}
		if (this.controllerRotationZValue == 5)
		{
			return "75";
		}
		return "90";
	}

	private int locomotionValue;

	private int turningValue;

	private int turningAngleValue;

	private int movementDirectionValue;

	private int grabTypeValue;

	private int SmoothCamValue;

	private int gripTypeValue;

	private int teleportGrabValue;

	private int turningSpeedValue;

	private int controllerRotationXValue;

	private int controllerRotationYValue;

	private int controllerRotationZValue;

	[SerializeField]
	private Button VRButton;

	[SerializeField]
	private Text VRButtonText;

	[SerializeField]
	private Text locomotionValueText;

	[SerializeField]
	private Text turningValueText;

	[SerializeField]
	private Text turningAngleValueText;

	[SerializeField]
	private Text movementDirectionValueText;

	[SerializeField]
	private Text grabTypeValueText;

	[SerializeField]
	private Text smoothCamValueText;

	[SerializeField]
	private Text gripTypeValueText;

	[SerializeField]
	private Text teleportGrabValueText;

	[SerializeField]
	private Text turningSpeedValueText;

	[SerializeField]
	private Text controllerRotationXValueText;

	[SerializeField]
	private Text controllerRotationYValueText;

	[SerializeField]
	private Text controllerRotationZValueText;
}


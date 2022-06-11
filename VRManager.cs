using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

// Token: 0x0200014E RID: 334
public class VRManager : MonoBehaviourPunCallbacks
{
	// Token: 0x06000967 RID: 2407 RVA: 0x0003A0F8 File Offset: 0x000382F8
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

	// Token: 0x06000968 RID: 2408 RVA: 0x0003A144 File Offset: 0x00038344
	private void Start()
	{
		this.LoadValues();
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x0003A14C File Offset: 0x0003834C
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

	// Token: 0x0600096A RID: 2410 RVA: 0x0003A230 File Offset: 0x00038430
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

	// Token: 0x0600096B RID: 2411 RVA: 0x0003A312 File Offset: 0x00038512
	public void ApplyButton()
	{
		this.SetValues();
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x0003A31C File Offset: 0x0003851C
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

	// Token: 0x0600096D RID: 2413 RVA: 0x0003A3F5 File Offset: 0x000385F5
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

	// Token: 0x0600096E RID: 2414 RVA: 0x0003A42D File Offset: 0x0003862D
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

	// Token: 0x0600096F RID: 2415 RVA: 0x0003A465 File Offset: 0x00038665
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

	// Token: 0x06000970 RID: 2416 RVA: 0x0003A49D File Offset: 0x0003869D
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

	// Token: 0x06000971 RID: 2417 RVA: 0x0003A4D7 File Offset: 0x000386D7
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

	// Token: 0x06000972 RID: 2418 RVA: 0x0003A50F File Offset: 0x0003870F
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

	// Token: 0x06000973 RID: 2419 RVA: 0x0003A547 File Offset: 0x00038747
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

	// Token: 0x06000974 RID: 2420 RVA: 0x0003A57F File Offset: 0x0003877F
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

	// Token: 0x06000975 RID: 2421 RVA: 0x0003A5B7 File Offset: 0x000387B7
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

	// Token: 0x06000976 RID: 2422 RVA: 0x0003A5EF File Offset: 0x000387EF
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

	// Token: 0x06000977 RID: 2423 RVA: 0x0003A629 File Offset: 0x00038829
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

	// Token: 0x06000978 RID: 2424 RVA: 0x0003A663 File Offset: 0x00038863
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

	// Token: 0x06000979 RID: 2425 RVA: 0x0003A69D File Offset: 0x0003889D
	private string GetLocomotionText()
	{
		if (this.locomotionValue != 0)
		{
			return LocalisationSystem.GetLocalisedValue("VR_Teleport");
		}
		return LocalisationSystem.GetLocalisedValue("VR_Smooth");
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x0003A6BC File Offset: 0x000388BC
	private string GetTurningText()
	{
		if (this.turningValue != 0)
		{
			return LocalisationSystem.GetLocalisedValue("VR_Snap");
		}
		return LocalisationSystem.GetLocalisedValue("VR_Smooth");
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x0003A6DB File Offset: 0x000388DB
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

	// Token: 0x0600097C RID: 2428 RVA: 0x0003A6FF File Offset: 0x000388FF
	private string GetTurningSpeedValueText()
	{
		return this.turningSpeedValue.ToString("0");
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x0003A711 File Offset: 0x00038911
	private string GetMovementDirectionValueText()
	{
		if (this.movementDirectionValue != 0)
		{
			return LocalisationSystem.GetLocalisedValue("VR_MovementHead");
		}
		return LocalisationSystem.GetLocalisedValue("VR_MovementController");
	}

	// Token: 0x0600097E RID: 2430 RVA: 0x0003A730 File Offset: 0x00038930
	private string GetGrabTypeValueText()
	{
		if (this.grabTypeValue != 0)
		{
			return LocalisationSystem.GetLocalisedValue("VR_Hold");
		}
		return LocalisationSystem.GetLocalisedValue("VR_Toggle");
	}

	// Token: 0x0600097F RID: 2431 RVA: 0x0003A74F File Offset: 0x0003894F
	private string GetSmoothCamValueText()
	{
		if (this.SmoothCamValue != 0)
		{
			return LocalisationSystem.GetLocalisedValue("Menu_On");
		}
		return LocalisationSystem.GetLocalisedValue("Menu_Off");
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x0003A76E File Offset: 0x0003896E
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

	// Token: 0x06000981 RID: 2433 RVA: 0x0003A7A1 File Offset: 0x000389A1
	private string GetTeleportGrabText()
	{
		if (this.teleportGrabValue != 0)
		{
			return LocalisationSystem.GetLocalisedValue("Menu_On");
		}
		return LocalisationSystem.GetLocalisedValue("Menu_Off");
	}

	// Token: 0x06000982 RID: 2434 RVA: 0x0003A7C0 File Offset: 0x000389C0
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

	// Token: 0x06000983 RID: 2435 RVA: 0x0003A88C File Offset: 0x00038A8C
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

	// Token: 0x06000984 RID: 2436 RVA: 0x0003A958 File Offset: 0x00038B58
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

	// Token: 0x040009A5 RID: 2469
	private int locomotionValue;

	// Token: 0x040009A6 RID: 2470
	private int turningValue;

	// Token: 0x040009A7 RID: 2471
	private int turningAngleValue;

	// Token: 0x040009A8 RID: 2472
	private int movementDirectionValue;

	// Token: 0x040009A9 RID: 2473
	private int grabTypeValue;

	// Token: 0x040009AA RID: 2474
	private int SmoothCamValue;

	// Token: 0x040009AB RID: 2475
	private int gripTypeValue;

	// Token: 0x040009AC RID: 2476
	private int teleportGrabValue;

	// Token: 0x040009AD RID: 2477
	private int turningSpeedValue;

	// Token: 0x040009AE RID: 2478
	private int controllerRotationXValue;

	// Token: 0x040009AF RID: 2479
	private int controllerRotationYValue;

	// Token: 0x040009B0 RID: 2480
	private int controllerRotationZValue;

	// Token: 0x040009B1 RID: 2481
	[SerializeField]
	private Button VRButton;

	// Token: 0x040009B2 RID: 2482
	[SerializeField]
	private Text VRButtonText;

	// Token: 0x040009B3 RID: 2483
	[SerializeField]
	private Text locomotionValueText;

	// Token: 0x040009B4 RID: 2484
	[SerializeField]
	private Text turningValueText;

	// Token: 0x040009B5 RID: 2485
	[SerializeField]
	private Text turningAngleValueText;

	// Token: 0x040009B6 RID: 2486
	[SerializeField]
	private Text movementDirectionValueText;

	// Token: 0x040009B7 RID: 2487
	[SerializeField]
	private Text grabTypeValueText;

	// Token: 0x040009B8 RID: 2488
	[SerializeField]
	private Text smoothCamValueText;

	// Token: 0x040009B9 RID: 2489
	[SerializeField]
	private Text gripTypeValueText;

	// Token: 0x040009BA RID: 2490
	[SerializeField]
	private Text teleportGrabValueText;

	// Token: 0x040009BB RID: 2491
	[SerializeField]
	private Text turningSpeedValueText;

	// Token: 0x040009BC RID: 2492
	[SerializeField]
	private Text controllerRotationXValueText;

	// Token: 0x040009BD RID: 2493
	[SerializeField]
	private Text controllerRotationYValueText;

	// Token: 0x040009BE RID: 2494
	[SerializeField]
	private Text controllerRotationZValueText;
}

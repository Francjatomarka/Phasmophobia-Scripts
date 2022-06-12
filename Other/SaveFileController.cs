using System;
using UnityEngine;

public class SaveFileController : MonoBehaviour
{
	public void Awake()
	{
		if (!FileBasedPrefs.HasKey("myTotalExp"))
		{
			if (PlayerPrefs.GetInt("myTotalExp") > 100000)
			{
				PlayerPrefs.SetInt("myTotalExp", 1000);
			}
			FileBasedPrefs.SetInt("myTotalExp", PlayerPrefs.GetInt("myTotalExp"));
		}
		if (!FileBasedPrefs.HasKey("PlayersMoney"))
		{
			FileBasedPrefs.SetInt("PlayersMoney", PlayerPrefs.GetInt("PlayersMoney"));
		}
		if (!FileBasedPrefs.HasKey("EMFReaderInventory"))
		{
			FileBasedPrefs.SetInt("EMFReaderInventory", PlayerPrefs.GetInt("EMFReaderInventory"));
		}
		if (!FileBasedPrefs.HasKey("FlashlightInventory"))
		{
			FileBasedPrefs.SetInt("FlashlightInventory", PlayerPrefs.GetInt("FlashlightInventory"));
		}
		if (!FileBasedPrefs.HasKey("CameraInventory"))
		{
			FileBasedPrefs.SetInt("CameraInventory", PlayerPrefs.GetInt("CameraInventory"));
		}
		if (!FileBasedPrefs.HasKey("LighterInventory"))
		{
			FileBasedPrefs.SetInt("LighterInventory", PlayerPrefs.GetInt("LighterInventory"));
		}
		if (!FileBasedPrefs.HasKey("CandleInventory"))
		{
			FileBasedPrefs.SetInt("CandleInventory", PlayerPrefs.GetInt("CandleInventory"));
		}
		if (!FileBasedPrefs.HasKey("UVFlashlightInventory"))
		{
			FileBasedPrefs.SetInt("UVFlashlightInventory", PlayerPrefs.GetInt("UVFlashlightInventory"));
		}
		if (!FileBasedPrefs.HasKey("CrucifixInventory"))
		{
			FileBasedPrefs.SetInt("CrucifixInventory", PlayerPrefs.GetInt("CrucifixInventory"));
		}
		if (!FileBasedPrefs.HasKey("DSLRCameraInventory"))
		{
			FileBasedPrefs.SetInt("DSLRCameraInventory", PlayerPrefs.GetInt("DSLRCameraInventory"));
		}
		if (!FileBasedPrefs.HasKey("EVPRecorderInventory"))
		{
			FileBasedPrefs.SetInt("EVPRecorderInventory", PlayerPrefs.GetInt("EVPRecorderInventory"));
		}
		if (!FileBasedPrefs.HasKey("SaltInventory"))
		{
			FileBasedPrefs.SetInt("SaltInventory", PlayerPrefs.GetInt("SaltInventory"));
		}
		if (!FileBasedPrefs.HasKey("SageInventory"))
		{
			FileBasedPrefs.SetInt("SageInventory", PlayerPrefs.GetInt("SageInventory"));
		}
		if (!FileBasedPrefs.HasKey("TripodInventory"))
		{
			FileBasedPrefs.SetInt("TripodInventory", PlayerPrefs.GetInt("TripodInventory"));
		}
		if (!FileBasedPrefs.HasKey("StrongFlashlightInventory"))
		{
			FileBasedPrefs.SetInt("StrongFlashlightInventory", PlayerPrefs.GetInt("StrongFlashlightInventory"));
		}
		if (!FileBasedPrefs.HasKey("MotionSensorInventory"))
		{
			FileBasedPrefs.SetInt("MotionSensorInventory", PlayerPrefs.GetInt("MotionSensorInventory"));
		}
		if (!FileBasedPrefs.HasKey("SoundSensorInventory"))
		{
			FileBasedPrefs.SetInt("SoundSensorInventory", PlayerPrefs.GetInt("SoundSensorInventory"));
		}
		if (!FileBasedPrefs.HasKey("SanityPillsInventory"))
		{
			FileBasedPrefs.SetInt("SanityPillsInventory", PlayerPrefs.GetInt("SanityPillsInventory"));
		}
		if (!FileBasedPrefs.HasKey("ThermometerInventory"))
		{
			FileBasedPrefs.SetInt("ThermometerInventory", PlayerPrefs.GetInt("ThermometerInventory"));
		}
		if (!FileBasedPrefs.HasKey("GhostWritingBookInventory"))
		{
			FileBasedPrefs.SetInt("GhostWritingBookInventory", PlayerPrefs.GetInt("GhostWritingBookInventory"));
		}
		if (!FileBasedPrefs.HasKey("IRLightSensorInventory"))
		{
			FileBasedPrefs.SetInt("IRLightSensorInventory", PlayerPrefs.GetInt("IRLightSensorInventory"));
		}
		if (!FileBasedPrefs.HasKey("ParabolicMicrophoneInventory"))
		{
			FileBasedPrefs.SetInt("ParabolicMicrophoneInventory", PlayerPrefs.GetInt("ParabolicMicrophoneInventory"));
		}
		if (!FileBasedPrefs.HasKey("GlowstickInventory"))
		{
			FileBasedPrefs.SetInt("GlowstickInventory", PlayerPrefs.GetInt("GlowstickInventory"));
		}
		if (!FileBasedPrefs.HasKey("HeadMountedCameraInventory"))
		{
			FileBasedPrefs.SetInt("HeadMountedCameraInventory", PlayerPrefs.GetInt("HeadMountedCameraInventory"));
		}
		PlayerPrefs.DeleteKey("myTotalExp");
		PlayerPrefs.DeleteKey("PlayersMoney");
		PlayerPrefs.DeleteKey("EMFReaderInventory");
		PlayerPrefs.DeleteKey("FlashlightInventory");
		PlayerPrefs.DeleteKey("CameraInventory");
		PlayerPrefs.DeleteKey("LighterInventory");
		PlayerPrefs.DeleteKey("CandleInventory");
		PlayerPrefs.DeleteKey("UVFlashlightInventory");
		PlayerPrefs.DeleteKey("CrucifixInventory");
		PlayerPrefs.DeleteKey("DSLRCameraInventory");
		PlayerPrefs.DeleteKey("EVPRecorderInventory");
		PlayerPrefs.DeleteKey("SaltInventory");
		PlayerPrefs.DeleteKey("SageInventory");
		PlayerPrefs.DeleteKey("TripodInventory");
		PlayerPrefs.DeleteKey("StrongFlashlightInventory");
		PlayerPrefs.DeleteKey("MotionSensorInventory");
		PlayerPrefs.DeleteKey("SoundSensorInventory");
		PlayerPrefs.DeleteKey("SanityPillsInventory");
		PlayerPrefs.DeleteKey("ThermometerInventory");
		PlayerPrefs.DeleteKey("GhostWritingBookInventory");
		PlayerPrefs.DeleteKey("IRLightSensorInventory");
		PlayerPrefs.DeleteKey("ParabolicMicrophoneInventory");
		PlayerPrefs.DeleteKey("GlowstickInventory");
		PlayerPrefs.DeleteKey("HeadMountedCameraInventory");
		this.SetDefaultValues();
		this.playerStatsManager.UpdateExperience();
		this.playerStatsManager.UpdateLevel();
		this.playerStatsManager.UpdateMoney();
		this.storeManager.UpdatePlayerMoneyText();
	}

	private void SetDefaultValues()
	{
		if (!FileBasedPrefs.HasKey("myTotalExp"))
		{
			FileBasedPrefs.SetInt("myTotalExp", 0);
		}
		if (!FileBasedPrefs.HasKey("PlayersMoney"))
		{
			FileBasedPrefs.SetInt("PlayersMoney", 0);
		}
		if (!FileBasedPrefs.HasKey("PlayersMoney"))
		{
			FileBasedPrefs.SetInt("PlayersMoney", 0);
		}
		if (!FileBasedPrefs.HasKey("EMFReaderInventory"))
		{
			FileBasedPrefs.SetInt("EMFReaderInventory", 0);
		}
		if (!FileBasedPrefs.HasKey("FlashlightInventory"))
		{
			FileBasedPrefs.SetInt("FlashlightInventory", 0);
		}
		if (!FileBasedPrefs.HasKey("CameraInventory"))
		{
			FileBasedPrefs.SetInt("CameraInventory", 0);
		}
		if (!FileBasedPrefs.HasKey("LighterInventory"))
		{
			FileBasedPrefs.SetInt("LighterInventory", 0);
		}
		if (!FileBasedPrefs.HasKey("CandleInventory"))
		{
			FileBasedPrefs.SetInt("CandleInventory", 0);
		}
		if (!FileBasedPrefs.HasKey("UVFlashlightInventory"))
		{
			FileBasedPrefs.SetInt("UVFlashlightInventory", 0);
		}
		if (!FileBasedPrefs.HasKey("CrucifixInventory"))
		{
			FileBasedPrefs.SetInt("CrucifixInventory", 0);
		}
		if (!FileBasedPrefs.HasKey("DSLRCameraInventory"))
		{
			FileBasedPrefs.SetInt("DSLRCameraInventory", 0);
		}
		if (!FileBasedPrefs.HasKey("EVPRecorderInventory"))
		{
			FileBasedPrefs.SetInt("EVPRecorderInventory", 0);
		}
		if (!FileBasedPrefs.HasKey("SaltInventory"))
		{
			FileBasedPrefs.SetInt("SaltInventory", 0);
		}
		if (!FileBasedPrefs.HasKey("SageInventory"))
		{
			FileBasedPrefs.SetInt("SageInventory", 0);
		}
		if (!FileBasedPrefs.HasKey("TripodInventory"))
		{
			FileBasedPrefs.SetInt("TripodInventory", 0);
		}
		if (!FileBasedPrefs.HasKey("StrongFlashlightInventory"))
		{
			FileBasedPrefs.SetInt("StrongFlashlightInventory", 0);
		}
		if (!FileBasedPrefs.HasKey("MotionSensorInventory"))
		{
			FileBasedPrefs.SetInt("MotionSensorInventory", 0);
		}
		if (!FileBasedPrefs.HasKey("SoundSensorInventory"))
		{
			FileBasedPrefs.SetInt("SoundSensorInventory", 0);
		}
		if (!FileBasedPrefs.HasKey("SanityPillsInventory"))
		{
			FileBasedPrefs.SetInt("SanityPillsInventory", 0);
		}
		if (!FileBasedPrefs.HasKey("ThermometerInventory"))
		{
			FileBasedPrefs.SetInt("ThermometerInventory", 0);
		}
		if (!FileBasedPrefs.HasKey("GhostWritingBookInventory"))
		{
			FileBasedPrefs.SetInt("GhostWritingBookInventory", 0);
		}
		if (!FileBasedPrefs.HasKey("IRLightSensorInventory"))
		{
			FileBasedPrefs.SetInt("IRLightSensorInventory", 0);
		}
		if (!FileBasedPrefs.HasKey("ParabolicMicrophoneInventory"))
		{
			FileBasedPrefs.SetInt("ParabolicMicrophoneInventory", 0);
		}
		if (!FileBasedPrefs.HasKey("GlowstickInventory"))
		{
			FileBasedPrefs.SetInt("GlowstickInventory", 0);
		}
		if (!FileBasedPrefs.HasKey("HeadMountedCameraInventory"))
		{
			FileBasedPrefs.SetInt("HeadMountedCameraInventory", 0);
		}
	}

	[SerializeField]
	private StoreSDKManager storeSDKManager;

	[SerializeField]
	private PlayerStatsManager playerStatsManager;

	[SerializeField]
	private StoreManager storeManager;
}


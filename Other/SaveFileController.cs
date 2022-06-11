using System;
using UnityEngine;

// Token: 0x02000121 RID: 289
public class SaveFileController : MonoBehaviour
{
	// Token: 0x060007D4 RID: 2004 RVA: 0x0002EBA8 File Offset: 0x0002CDA8
	public void SaveData()
    {
		FBPP.Save();
	}
	public void Awake()
	{
		var config = new FBPPConfig()
		{
			AutoSaveData = false,
			EncryptionSecret = "dani"
		};
		FBPP.Start(config);
		if (!FBPP.HasKey("myTotalExp"))
		{
			if (PlayerPrefs.GetInt("myTotalExp") > 100000)
			{
				PlayerPrefs.SetInt("myTotalExp", 1000);
			}
			FBPP.SetInt("myTotalExp", PlayerPrefs.GetInt("myTotalExp"));
		}
		if (!FBPP.HasKey("PlayersMoney"))
		{
			FBPP.SetInt("PlayersMoney", PlayerPrefs.GetInt("PlayersMoney"));
		}
		if (!FBPP.HasKey("EMFReaderInventory"))
		{
			FBPP.SetInt("EMFReaderInventory", PlayerPrefs.GetInt("EMFReaderInventory"));
		}
		if (!FBPP.HasKey("FlashlightInventory"))
		{
			FBPP.SetInt("FlashlightInventory", PlayerPrefs.GetInt("FlashlightInventory"));
		}
		if (!FBPP.HasKey("CameraInventory"))
		{
			FBPP.SetInt("CameraInventory", PlayerPrefs.GetInt("CameraInventory"));
		}
		if (!FBPP.HasKey("LighterInventory"))
		{
			FBPP.SetInt("LighterInventory", PlayerPrefs.GetInt("LighterInventory"));
		}
		if (!FBPP.HasKey("CandleInventory"))
		{
			FBPP.SetInt("CandleInventory", PlayerPrefs.GetInt("CandleInventory"));
		}
		if (!FBPP.HasKey("UVFlashlightInventory"))
		{
			FBPP.SetInt("UVFlashlightInventory", PlayerPrefs.GetInt("UVFlashlightInventory"));
		}
		if (!FBPP.HasKey("CrucifixInventory"))
		{
			FBPP.SetInt("CrucifixInventory", PlayerPrefs.GetInt("CrucifixInventory"));
		}
		if (!FBPP.HasKey("DSLRCameraInventory"))
		{
			FBPP.SetInt("DSLRCameraInventory", PlayerPrefs.GetInt("DSLRCameraInventory"));
		}
		if (!FBPP.HasKey("EVPRecorderInventory"))
		{
			FBPP.SetInt("EVPRecorderInventory", PlayerPrefs.GetInt("EVPRecorderInventory"));
		}
		if (!FBPP.HasKey("SaltInventory"))
		{
			FBPP.SetInt("SaltInventory", PlayerPrefs.GetInt("SaltInventory"));
		}
		if (!FBPP.HasKey("SageInventory"))
		{
			FBPP.SetInt("SageInventory", PlayerPrefs.GetInt("SageInventory"));
		}
		if (!FBPP.HasKey("TripodInventory"))
		{
			FBPP.SetInt("TripodInventory", PlayerPrefs.GetInt("TripodInventory"));
		}
		if (!FBPP.HasKey("StrongFlashlightInventory"))
		{
			FBPP.SetInt("StrongFlashlightInventory", PlayerPrefs.GetInt("StrongFlashlightInventory"));
		}
		if (!FBPP.HasKey("MotionSensorInventory"))
		{
			FBPP.SetInt("MotionSensorInventory", PlayerPrefs.GetInt("MotionSensorInventory"));
		}
		if (!FBPP.HasKey("SoundSensorInventory"))
		{
			FBPP.SetInt("SoundSensorInventory", PlayerPrefs.GetInt("SoundSensorInventory"));
		}
		if (!FBPP.HasKey("SanityPillsInventory"))
		{
			FBPP.SetInt("SanityPillsInventory", PlayerPrefs.GetInt("SanityPillsInventory"));
		}
		if (!FBPP.HasKey("ThermometerInventory"))
		{
			FBPP.SetInt("ThermometerInventory", PlayerPrefs.GetInt("ThermometerInventory"));
		}
		if (!FBPP.HasKey("GhostWritingBookInventory"))
		{
			FBPP.SetInt("GhostWritingBookInventory", PlayerPrefs.GetInt("GhostWritingBookInventory"));
		}
		if (!FBPP.HasKey("IRLightSensorInventory"))
		{
			FBPP.SetInt("IRLightSensorInventory", PlayerPrefs.GetInt("IRLightSensorInventory"));
		}
		if (!FBPP.HasKey("ParabolicMicrophoneInventory"))
		{
			FBPP.SetInt("ParabolicMicrophoneInventory", PlayerPrefs.GetInt("ParabolicMicrophoneInventory"));
		}
		if (!FBPP.HasKey("GlowstickInventory"))
		{
			FBPP.SetInt("GlowstickInventory", PlayerPrefs.GetInt("GlowstickInventory"));
		}
		if (!FBPP.HasKey("HeadMountedCameraInventory"))
		{
			FBPP.SetInt("HeadMountedCameraInventory", PlayerPrefs.GetInt("HeadMountedCameraInventory"));
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

	// Token: 0x060007D5 RID: 2005 RVA: 0x0002EFF8 File Offset: 0x0002D1F8
	private void SetDefaultValues()
	{
		if (!FBPP.HasKey("myTotalExp"))
		{
			FBPP.SetInt("myTotalExp", 0);
		}
		if (!FBPP.HasKey("PlayersMoney"))
		{
			FBPP.SetInt("PlayersMoney", 0);
		}
		if (!FBPP.HasKey("PlayersMoney"))
		{
			FBPP.SetInt("PlayersMoney", 0);
		}
		if (!FBPP.HasKey("EMFReaderInventory"))
		{
			FBPP.SetInt("EMFReaderInventory", 0);
		}
		if (!FBPP.HasKey("FlashlightInventory"))
		{
			FBPP.SetInt("FlashlightInventory", 0);
		}
		if (!FBPP.HasKey("CameraInventory"))
		{
			FBPP.SetInt("CameraInventory", 0);
		}
		if (!FBPP.HasKey("LighterInventory"))
		{
			FBPP.SetInt("LighterInventory", 0);
		}
		if (!FBPP.HasKey("CandleInventory"))
		{
			FBPP.SetInt("CandleInventory", 0);
		}
		if (!FBPP.HasKey("UVFlashlightInventory"))
		{
			FBPP.SetInt("UVFlashlightInventory", 0);
		}
		if (!FBPP.HasKey("CrucifixInventory"))
		{
			FBPP.SetInt("CrucifixInventory", 0);
		}
		if (!FBPP.HasKey("DSLRCameraInventory"))
		{
			FBPP.SetInt("DSLRCameraInventory", 0);
		}
		if (!FBPP.HasKey("EVPRecorderInventory"))
		{
			FBPP.SetInt("EVPRecorderInventory", 0);
		}
		if (!FBPP.HasKey("SaltInventory"))
		{
			FBPP.SetInt("SaltInventory", 0);
		}
		if (!FBPP.HasKey("SageInventory"))
		{
			FBPP.SetInt("SageInventory", 0);
		}
		if (!FBPP.HasKey("TripodInventory"))
		{
			FBPP.SetInt("TripodInventory", 0);
		}
		if (!FBPP.HasKey("StrongFlashlightInventory"))
		{
			FBPP.SetInt("StrongFlashlightInventory", 0);
		}
		if (!FBPP.HasKey("MotionSensorInventory"))
		{
			FBPP.SetInt("MotionSensorInventory", 0);
		}
		if (!FBPP.HasKey("SoundSensorInventory"))
		{
			FBPP.SetInt("SoundSensorInventory", 0);
		}
		if (!FBPP.HasKey("SanityPillsInventory"))
		{
			FBPP.SetInt("SanityPillsInventory", 0);
		}
		if (!FBPP.HasKey("ThermometerInventory"))
		{
			FBPP.SetInt("ThermometerInventory", 0);
		}
		if (!FBPP.HasKey("GhostWritingBookInventory"))
		{
			FBPP.SetInt("GhostWritingBookInventory", 0);
		}
		if (!FBPP.HasKey("IRLightSensorInventory"))
		{
			FBPP.SetInt("IRLightSensorInventory", 0);
		}
		if (!FBPP.HasKey("ParabolicMicrophoneInventory"))
		{
			FBPP.SetInt("ParabolicMicrophoneInventory", 0);
		}
		if (!FBPP.HasKey("GlowstickInventory"))
		{
			FBPP.SetInt("GlowstickInventory", 0);
		}
		if (!FBPP.HasKey("HeadMountedCameraInventory"))
		{
			FBPP.SetInt("HeadMountedCameraInventory", 0);
		}
	}

	// Token: 0x040007A6 RID: 1958
	[SerializeField]
	private StoreSDKManager storeSDKManager;

	// Token: 0x040007A7 RID: 1959
	[SerializeField]
	private PlayerStatsManager playerStatsManager;

	// Token: 0x040007A8 RID: 1960
	[SerializeField]
	private StoreManager storeManager;
}

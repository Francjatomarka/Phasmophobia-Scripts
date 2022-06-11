using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// Token: 0x02000140 RID: 320
public class MyAudioManager : MonoBehaviour
{
	// Token: 0x060008F8 RID: 2296 RVA: 0x00037404 File Offset: 0x00035604
	private void Awake()
	{
		if (!this.isPauseMenu)
		{
			this.AssignAudioDevices();
			if (PlayerPrefs.GetInt("AudioHasBeenSet") == 0)
			{
				PlayerPrefs.SetFloat("MasterVolume", this.masterVolumeSlider.maxValue);
			}
		}
		if (PlayerPrefs.GetFloat("MasterVolume") > this.masterVolumeSlider.maxValue)
		{
			PlayerPrefs.SetFloat("MasterVolume", this.masterVolumeSlider.maxValue);
		}
		if (PlayerPrefs.GetFloat("MasterVolume") < this.masterVolumeSlider.minValue)
		{
			PlayerPrefs.SetFloat("MasterVolume", this.masterVolumeSlider.maxValue);
		}
		if (!this.isPauseMenu)
		{
			this.AssignSavedDevice(PlayerPrefs.GetString("microphoneDevice"));
		}
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x000374B0 File Offset: 0x000356B0
	private void Start()
	{
		this.masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
		this.masterVolumeValueText.text = (this.masterVolumeSlider.value * 100f).ToString("0");
		if (MainManager.instance)
		{
			this.masterAudio.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")) * 20f + 15f);
		}
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x00037534 File Offset: 0x00035734
	private void AssignAudioDevices()
	{
		this.deviceNames.Clear();
		foreach (string item in Microphone.devices)
		{
			this.deviceNames.Add(item);
		}
		if (this.deviceNames.Count > 1)
		{
			this.deviceIndex = 0;
			this.deviceNameText.text = this.deviceNames[0];
			return;
		}
		this.deviceNameText.text = "No Microphone detected";
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x000375B0 File Offset: 0x000357B0
	private void AssignSavedDevice(string savedDeviceName)
	{
		if (this.deviceNames.Count == 0)
		{
			foreach (string item in Microphone.devices)
			{
				this.deviceNames.Add(item);
			}
		}
		for (int j = 0; j < this.deviceNames.Count; j++)
		{
			if (this.deviceNames[j] == savedDeviceName)
			{
				this.deviceIndex = j;
			}
		}
		if (this.deviceNames.Count > 1)
		{
			this.deviceNameText.text = this.deviceNames[this.deviceIndex];
		}
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x00037660 File Offset: 0x00035860
	public void AudioDeviceButton(int value)
	{
		this.deviceIndex += value;
		if (this.deviceIndex < 0)
		{
			this.deviceIndex = this.deviceNames.Count - 1;
		}
		else if (this.deviceIndex == this.deviceNames.Count)
		{
			this.deviceIndex = 0;
		}
		this.deviceNameText.text = this.deviceNames[this.deviceIndex];
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x000376D0 File Offset: 0x000358D0
	public void MasterVolumeValueChange()
	{
		this.masterVolumeValueText.text = (this.masterVolumeSlider.value * 100f).ToString("0");
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x00037706 File Offset: 0x00035906
	public void ApplyButton()
	{
		this.SetValues();
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x00037710 File Offset: 0x00035910
	public void SetValues()
	{
		PlayerPrefs.SetFloat("MasterVolume", this.masterVolumeSlider.value);
		this.masterAudio.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")) * 20f + 15f);
		if (!this.isPauseMenu)
		{
			PlayerPrefs.SetInt("AudioHasBeenSet", 1);
		}
	}

	// Token: 0x0400091B RID: 2331
	[SerializeField]
	private Slider masterVolumeSlider;

	// Token: 0x0400091C RID: 2332
	[SerializeField]
	private Text masterVolumeValueText;

	// Token: 0x0400091D RID: 2333
	[SerializeField]
	private AudioMixer masterAudio;

	// Token: 0x0400091E RID: 2334
	private int deviceIndex;

	// Token: 0x0400091F RID: 2335
	[SerializeField]
	private Text deviceNameText;

	// Token: 0x04000920 RID: 2336
	private List<string> deviceNames = new List<string>();

	// Token: 0x04000921 RID: 2337
	[SerializeField]
	private bool isPauseMenu;
}

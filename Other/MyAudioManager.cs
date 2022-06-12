using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MyAudioManager : MonoBehaviour
{
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

	private void Start()
	{
		this.masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
		this.masterVolumeValueText.text = (this.masterVolumeSlider.value * 100f).ToString("0");
		if (MainManager.instance)
		{
			this.masterAudio.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")) * 20f + 15f);
		}
	}

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

	public void MasterVolumeValueChange()
	{
		this.masterVolumeValueText.text = (this.masterVolumeSlider.value * 100f).ToString("0");
	}

	public void ApplyButton()
	{
		this.SetValues();
	}

	public void SetValues()
	{
		PlayerPrefs.SetFloat("MasterVolume", this.masterVolumeSlider.value);
		this.masterAudio.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")) * 20f + 15f);
		if (!this.isPauseMenu)
		{
			PlayerPrefs.SetInt("AudioHasBeenSet", 1);
		}
	}

	[SerializeField]
	private Slider masterVolumeSlider;

	[SerializeField]
	private Text masterVolumeValueText;

	[SerializeField]
	private AudioMixer masterAudio;

	private int deviceIndex;

	[SerializeField]
	private Text deviceNameText;

	private List<string> deviceNames = new List<string>();

	[SerializeField]
	private bool isPauseMenu;
}


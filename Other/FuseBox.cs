using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class FuseBox : MonoBehaviour
{
	private void Awake()
	{
		if (!this.noise)
		{
			this.noise = base.GetComponentInChildren<Noise>();
		}
		if (!this.source)
		{
			this.source = base.GetComponent<AudioSource>();
		}
		if (!this.view)
		{
			this.view = base.GetComponent<PhotonView>();
		}
		if (!this.photonInteract)
		{
			this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		}
		this.noise.gameObject.SetActive(false);
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
	}

	public void SetupAudioGroup()
	{
		this.source.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		this.floorType = SoundController.instance.GetFloorTypeFromAudioGroup(this.source.outputAudioMixerGroup);
		MapController.instance.AssignIcon(this.mapIcon, this.floorType);
	}

	private void Start()
	{
		if (LevelController.instance.type == LevelController.levelType.small)
		{
			this.maxLights = 10;
			return;
		}
		if (LevelController.instance.type == LevelController.levelType.medium)
		{
			this.maxLights = 9;
			return;
		}
		this.maxLights = 8;
	}

	public void TurnOff()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			if (PhotonNetwork.InRoom)
			{
				this.view.RPC("UseNetworked", RpcTarget.AllBuffered, new object[]
				{
					true
				});
				return;
			}
			this.UseNetworked(true);
		}
	}

	public void Use()
	{
		if (PhotonNetwork.InRoom)
		{
			this.view.RPC("UseNetworked", RpcTarget.AllBuffered, new object[]
			{
				false
			});
			return;
		}
		this.UseNetworked(false);
	}

	[PunRPC]
	private void UseNetworked(bool isGhostUsing)
	{
		if (isGhostUsing)
		{
			this.isOn = false;
		}
		else
		{
			this.isOn = !this.isOn;
		}
		if (this.source == null)
		{
			this.source = base.GetComponent<AudioSource>();
		}
		base.StartCoroutine(this.PlayNoiseObject());
		if (this.isOn)
		{
			this.source.clip = this.onClip;
			foreach (LightSwitch lightSwitch in this.switches)
			{
				if (lightSwitch != null)
				{
					lightSwitch.FuseOn();
				}
			}
			for (int i = 0; i < this.rends.Length; i++)
			{
				if (this.rends[i] != null)
				{
					this.rends[i].materials[1].SetColor("_EmissionColor", Color.green);
				}
			}
			for (int j = 0; j < this.rendsToTurnOff.Length; j++)
			{
				if (this.rendsToTurnOff[j] != null)
				{
					this.rendsToTurnOff[j].materials[0].EnableKeyword("_EMISSION");
				}
			}
			for (int k = 0; k < this.lights.Length; k++)
			{
				if (this.lights[k] != null)
				{
					this.lights[k].enabled = true;
				}
			}
			if (PhotonNetwork.IsMasterClient && CCTVController.instance.activatedCCTVCameras)
			{
				for (int l = 0; l < CCTVController.instance.allFixedCCTVCameras.Count; l++)
				{
					if (CCTVController.instance.allFixedCCTVCameras[l] != null)
					{
						CCTVController.instance.allFixedCCTVCameras[l].TurnOn();
					}
				}
			}
		}
		else
		{
			this.source.clip = this.offClip;
			foreach (LightSwitch lightSwitch2 in this.switches)
			{
				if (lightSwitch2 != null)
				{
					lightSwitch2.FuseOff();
				}
			}
			for (int m = 0; m < this.rends.Length; m++)
			{
				if (this.rends[m] != null)
				{
					this.rends[m].materials[1].SetColor("_EmissionColor", Color.red);
				}
			}
			for (int n = 0; n < this.rendsToTurnOff.Length; n++)
			{
				if (this.rendsToTurnOff[n] != null)
				{
					this.rendsToTurnOff[n].materials[0].DisableKeyword("_EMISSION");
				}
			}
			for (int num = 0; num < this.lights.Length; num++)
			{
				if (this.lights[num] != null)
				{
					this.lights[num].enabled = false;
				}
			}
			if (PhotonNetwork.IsMasterClient)
			{
				for (int num2 = 0; num2 < CCTVController.instance.allFixedCCTVCameras.Count; num2++)
				{
					if (CCTVController.instance.allFixedCCTVCameras[num2] != null)
					{
						CCTVController.instance.allFixedCCTVCameras[num2].TurnOff();
					}
				}
			}
		}
		if (this.source != null)
		{
			this.source.Play();
		}
		foreach (ReflectionProbe reflectionProbe in this.probes)
		{
			if (reflectionProbe != null)
			{
				reflectionProbe.RenderProbe();
			}
		}
	}

	private IEnumerator PlayNoiseObject()
	{
		this.noise.gameObject.SetActive(true);
		yield return 0;
		this.noise.gameObject.SetActive(false);
		yield break;
	}

	public void ChangeOnLights(int value)
	{
		this.currentOnLights += value;
		if (this.currentOnLights == this.maxLights)
		{
			if (PhotonNetwork.IsMasterClient)
			{
				for (int i = 0; i < this.switches.Count; i++)
				{
					if (this.switches[i].isOn)
					{
						this.switches[i].TurnOff();
					}
				}
			}
			this.TurnOff();
			this.currentOnLights = 0;
		}
	}

	[SerializeField]
	private Renderer[] rends;

	[SerializeField]
	private Light[] lights;

	[SerializeField]
	private Renderer[] rendsToTurnOff;

	[SerializeField]
	private AudioSource source;

	[SerializeField]
	private PhotonView view;

	[SerializeField]
	private Noise noise;

	public Transform parentObject;

	[SerializeField]
	private AudioClip onClip;

	[SerializeField]
	private AudioClip offClip;

	[HideInInspector]
	public bool isOn = true;

	public List<LightSwitch> switches = new List<LightSwitch>();

	[SerializeField]
	private PhotonObjectInteract photonInteract;

	[HideInInspector]
	public List<ReflectionProbe> probes = new List<ReflectionProbe>();

	[HideInInspector]
	private int currentOnLights;

	private int maxLights = 10;

	[SerializeField]
	private Transform mapIcon;

	private LevelRoom.Type floorType = LevelRoom.Type.firstFloor;
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

// Token: 0x02000142 RID: 322
public class FuseBox : MonoBehaviour
{
	// Token: 0x0600087B RID: 2171 RVA: 0x00032990 File Offset: 0x00030B90
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

	// Token: 0x0600087C RID: 2172 RVA: 0x00032A2C File Offset: 0x00030C2C
	public void SetupAudioGroup()
	{
		this.source.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		this.floorType = SoundController.instance.GetFloorTypeFromAudioGroup(this.source.outputAudioMixerGroup);
		MapController.instance.AssignIcon(this.mapIcon, this.floorType);
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x00032A8F File Offset: 0x00030C8F
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

	// Token: 0x0600087E RID: 2174 RVA: 0x00032AC3 File Offset: 0x00030CC3
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

	// Token: 0x0600087F RID: 2175 RVA: 0x00032AFB File Offset: 0x00030CFB
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

	// Token: 0x06000880 RID: 2176 RVA: 0x00032B2C File Offset: 0x00030D2C
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

	// Token: 0x06000881 RID: 2177 RVA: 0x00032EE0 File Offset: 0x000310E0
	private IEnumerator PlayNoiseObject()
	{
		this.noise.gameObject.SetActive(true);
		yield return 0;
		this.noise.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x00032EF0 File Offset: 0x000310F0
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

	// Token: 0x040008A3 RID: 2211
	[SerializeField]
	private Renderer[] rends;

	// Token: 0x040008A4 RID: 2212
	[SerializeField]
	private Light[] lights;

	// Token: 0x040008A5 RID: 2213
	[SerializeField]
	private Renderer[] rendsToTurnOff;

	// Token: 0x040008A6 RID: 2214
	[SerializeField]
	private AudioSource source;

	// Token: 0x040008A7 RID: 2215
	[SerializeField]
	private PhotonView view;

	// Token: 0x040008A8 RID: 2216
	[SerializeField]
	private Noise noise;

	// Token: 0x040008A9 RID: 2217
	public Transform parentObject;

	// Token: 0x040008AA RID: 2218
	[SerializeField]
	private AudioClip onClip;

	// Token: 0x040008AB RID: 2219
	[SerializeField]
	private AudioClip offClip;

	// Token: 0x040008AC RID: 2220
	[HideInInspector]
	public bool isOn = true;

	// Token: 0x040008AD RID: 2221
	public List<LightSwitch> switches = new List<LightSwitch>();

	// Token: 0x040008AE RID: 2222
	[SerializeField]
	private PhotonObjectInteract photonInteract;

	// Token: 0x040008AF RID: 2223
	[HideInInspector]
	public List<ReflectionProbe> probes = new List<ReflectionProbe>();

	// Token: 0x040008B0 RID: 2224
	[HideInInspector]
	private int currentOnLights;

	// Token: 0x040008B1 RID: 2225
	private int maxLights = 10;

	// Token: 0x040008B2 RID: 2226
	[SerializeField]
	private Transform mapIcon;

	// Token: 0x040008B3 RID: 2227
	private LevelRoom.Type floorType = LevelRoom.Type.firstFloor;
}

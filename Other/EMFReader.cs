using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

// Token: 0x0200010E RID: 270
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(AudioSource))]
public class EMFReader : MonoBehaviour
{
	// Token: 0x06000770 RID: 1904 RVA: 0x0002BFC4 File Offset: 0x0002A1C4
	private void Awake()
	{
		this.noise.gameObject.SetActive(false);
	}

	// Token: 0x06000771 RID: 1905 RVA: 0x0002BFD7 File Offset: 0x0002A1D7
	private void Start()
	{
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
	}

	// Token: 0x06000772 RID: 1906 RVA: 0x0002BFF0 File Offset: 0x0002A1F0
	public void Update()
	{
		if (this.isOn)
		{
			this.strength = 0;
			this.source.volume = 0f;
			for (int i = 0; i < this.emfZones.Count; i++)
			{
				if (this.emfZones[i].strength > this.strength)
				{
					this.strength = this.emfZones[i].strength;
				}
			}
			if (this.strength > 0)
			{
				this.source.volume = (float)this.strength / 25f;
				if (this.strength >= 1)
				{
					this.rend.materials[5].SetColor("_EmissionColor", this.lightGreen);
				}
				else
				{
					this.rend.materials[5].SetColor("_EmissionColor", Color.black);
				}
				if (this.strength >= 2)
				{
					this.rend.materials[6].SetColor("_EmissionColor", Color.yellow);
				}
				else
				{
					this.rend.materials[6].SetColor("_EmissionColor", Color.black);
				}
				if (this.strength >= 3)
				{
					this.rend.materials[4].SetColor("_EmissionColor", this.orange);
				}
				else
				{
					this.rend.materials[4].SetColor("_EmissionColor", Color.black);
				}
				if (this.strength == 4)
				{
					this.rend.materials[3].SetColor("_EmissionColor", Color.red);
				}
				else
				{
					this.rend.materials[3].SetColor("_EmissionColor", Color.black);
				}
				if (MissionEMFEvidence.instance != null && !MissionEMFEvidence.instance.completed)
				{
					MissionEMFEvidence.instance.CompleteMission();
				}
			}
			else
			{
				this.rend.materials[2].SetColor("_EmissionColor", Color.green);
				this.rend.materials[5].SetColor("_EmissionColor", Color.black);
				this.rend.materials[6].SetColor("_EmissionColor", Color.black);
				this.rend.materials[4].SetColor("_EmissionColor", Color.black);
				this.rend.materials[3].SetColor("_EmissionColor", Color.black);
			}
			this.noise.volume = (float)(this.strength / 4);
		}
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x0002C265 File Offset: 0x0002A465
	public void Use()
	{
        if (PhotonNetwork.InRoom)
        {
			this.view.RPC("NetworkedUse", RpcTarget.All, new object[]
			{
				PhotonNetwork.LocalPlayer.UserId
			});
			return;
		}
		NetworkedUse(710444123);
		
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x0002C290 File Offset: 0x0002A490
	[PunRPC]
	public void NetworkedUse(int actorID)
	{
		if(actorID == 710444123)
        {
			this.isOn = !this.isOn;
			this.onOffAudioSource.Play();
			if (this.isOn)
			{
				this.strength = 0;
				this.source.volume = 0f;
				this.source.enabled = true;
				this.noise.gameObject.SetActive(true);
				this.rend.materials[2].SetColor("_EmissionColor", Color.green);
				this.rend.materials[5].SetColor("_EmissionColor", Color.black);
				this.rend.materials[6].SetColor("_EmissionColor", Color.black);
				this.rend.materials[4].SetColor("_EmissionColor", Color.black);
				this.rend.materials[3].SetColor("_EmissionColor", Color.black);
				return;
			}
			this.strength = 0;
			this.source.volume = 0f;
			this.source.enabled = false;
			this.noise.gameObject.SetActive(false);
			this.rend.materials[2].SetColor("_EmissionColor", Color.black);
			this.rend.materials[5].SetColor("_EmissionColor", Color.black);
			this.rend.materials[6].SetColor("_EmissionColor", Color.black);
			this.rend.materials[4].SetColor("_EmissionColor", Color.black);
			this.rend.materials[3].SetColor("_EmissionColor", Color.black);
			return;
		}
		this.isOn = !this.isOn;
		this.source.outputAudioMixerGroup = SoundController.instance.GetPlayersAudioGroup(actorID);
		this.onOffAudioSource.outputAudioMixerGroup = SoundController.instance.GetPlayersAudioGroup(actorID);
		this.onOffAudioSource.Play();
		if (this.isOn)
		{
			this.strength = 0;
			this.source.volume = 0f;
			this.source.enabled = true;
			this.noise.gameObject.SetActive(true);
			this.rend.materials[2].SetColor("_EmissionColor", Color.green);
			this.rend.materials[5].SetColor("_EmissionColor", Color.black);
			this.rend.materials[6].SetColor("_EmissionColor", Color.black);
			this.rend.materials[4].SetColor("_EmissionColor", Color.black);
			this.rend.materials[3].SetColor("_EmissionColor", Color.black);
			return;
		}
		this.strength = 0;
		this.source.volume = 0f;
		this.source.enabled = false;
		this.noise.gameObject.SetActive(false);
		this.rend.materials[2].SetColor("_EmissionColor", Color.black);
		this.rend.materials[5].SetColor("_EmissionColor", Color.black);
		this.rend.materials[6].SetColor("_EmissionColor", Color.black);
		this.rend.materials[4].SetColor("_EmissionColor", Color.black);
		this.rend.materials[3].SetColor("_EmissionColor", Color.black);
	}

	// Token: 0x06000775 RID: 1909 RVA: 0x0002C470 File Offset: 0x0002A670
	public void RemoveEMFZone(EMF emf)
	{
		if (emf == null)
		{
			return;
		}
		if (this.emfZones.Contains(emf))
		{
			this.emfZones.Remove(emf);
		}
		if (this.emfZones.Count == 0)
		{
			this.strength = 0;
			this.source.volume = 0f;
			this.rend.materials[2].SetColor("_EmissionColor", Color.black);
			this.rend.materials[5].SetColor("_EmissionColor", Color.black);
			this.rend.materials[6].SetColor("_EmissionColor", Color.black);
			this.rend.materials[4].SetColor("_EmissionColor", Color.black);
			this.rend.materials[3].SetColor("_EmissionColor", Color.black);
		}
		for (int i = 0; i < this.emfZones.Count; i++)
		{
			if (this.emfZones[i] == null)
			{
				this.emfZones.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x0002C58C File Offset: 0x0002A78C
	public void AddEMFZone(EMF emf)
	{
		if (!this.emfZones.Contains(emf))
		{
			this.emfZones.Add(emf);
			emf.emfReaders.Add(this);
		}
		for (int i = 0; i < this.emfZones.Count; i++)
		{
			if (this.emfZones[i] == null)
			{
				this.emfZones.RemoveAt(i);
			}
		}
	}

	// Token: 0x0400077F RID: 1919
	public Renderer rend;

	// Token: 0x04000780 RID: 1920
	public AudioSource source;

	// Token: 0x04000781 RID: 1921
	public PhotonView view;

	// Token: 0x04000782 RID: 1922
	public int strength;

	// Token: 0x04000783 RID: 1923
	public List<EMF> emfZones = new List<EMF>();

	// Token: 0x04000784 RID: 1924
	private GhostAI nearestGhost;

	// Token: 0x04000785 RID: 1925
	[HideInInspector]
	public bool isOn;

	// Token: 0x04000786 RID: 1926
	private Color lightGreen = new Color(0.4f, 1f, 0.4f);

	// Token: 0x04000787 RID: 1927
	private Color orange = new Color(1f, 0.5f, 0f);

	// Token: 0x04000788 RID: 1928
	[SerializeField]
	private Noise noise;

	// Token: 0x04000789 RID: 1929
	[SerializeField]
	private PhotonObjectInteract photonInteract;

	// Token: 0x0400078A RID: 1930
	[SerializeField]
	private AudioSource onOffAudioSource;
}

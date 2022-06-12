using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(AudioSource))]
public class EMFReader : MonoBehaviour
{
	private void Awake()
	{
		this.noise.gameObject.SetActive(false);
	}

	private void Start()
	{
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
	}

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

	public Renderer rend;

	public AudioSource source;

	public PhotonView view;

	public int strength;

	public List<EMF> emfZones = new List<EMF>();

	private GhostAI nearestGhost;

	[HideInInspector]
	public bool isOn;

	private Color lightGreen = new Color(0.4f, 1f, 0.4f);

	private Color orange = new Color(1f, 0.5f, 0f);

	[SerializeField]
	private Noise noise;

	[SerializeField]
	private PhotonObjectInteract photonInteract;

	[SerializeField]
	private AudioSource onOffAudioSource;
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class LightSwitch : MonoBehaviour
{
	private void Awake()
	{
		this.isOn = false;
		this.noise = base.GetComponentInChildren<Noise>();
		this.source = base.GetComponent<AudioSource>();
		this.view = base.GetComponent<PhotonView>();
		if (this.photonInteract == null)
		{
			this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		}
		foreach (Light light in this.lights)
		{
			this.lightsMaxIntensity.Add(light.intensity);
		}
		this.blinkTimer = UnityEngine.Random.Range(0f, 0.5f);
		if (this.noise != null)
		{
			this.noise.gameObject.SetActive(false);
		}
	}

	private void Start()
	{
		if (this.handPrintObject != null)
		{
			this.handPrintObject.SetActive(false);
		}
		this.photonInteract.AddUseEvent(new UnityAction(this.UseLight));
		for (int i = 0; i < this.probes.Count; i++)
		{
			if (this.probes[i] != null)
			{
				LevelController.instance.fuseBox.probes.Add(this.probes[i]);
			}
		}
		if (PhotonNetwork.IsMasterClient)
		{
			this.TurnOff();
		}
		this.source.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
	}

	public void UseLight()
	{
		if (this.isBlinking)
		{
			return;
		}
		if (GameController.instance.myPlayer.player.isDead)
		{
			return;
		}
		if (PhotonNetwork.InRoom)
		{
			this.view.RPC("Use", RpcTarget.All, Array.Empty<object>());
			return;
		}
		this.Use();
	}

	[PunRPC]
	private void Use()
	{
		this.isOn = !this.isOn;
		if (this.source == null)
		{
			this.source = base.GetComponent<AudioSource>();
		}
		if (this.view == null)
		{
			this.view = base.GetComponent<PhotonView>();
		}
		if (this.isOn)
		{
			this.source.clip = this.clips[UnityEngine.Random.Range(0, this.clips.Length)];
			this.source.Play();
			if (this.lever != null)
			{
				Quaternion localRotation = this.lever.transform.localRotation;
				localRotation.eulerAngles = new Vector3(15f, 0f, 0f);
				this.lever.transform.localRotation = localRotation;
			}
			if (LevelController.instance.fuseBox.isOn)
			{
				foreach (Light light in this.lights)
				{
					light.enabled = true;
				}
				foreach (Renderer renderer in this.rends)
				{
					for (int i = 0; i < renderer.materials.Length; i++)
					{
						renderer.materials[i].EnableKeyword("_EMISSION");
					}
				}
				Animator[] array = this.animators;
				for (int j = 0; j < array.Length; j++)
				{
					array[j].SetBool("isOn", true);
				}
				AudioSource[] array2 = this.sources;
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j].Play();
				}
				for (int k = 0; k < this.objectsToActivate.Length; k++)
				{
					this.objectsToActivate[k].SetActive(true);
				}
				this.ResetReflectionProbes();
				base.StartCoroutine(this.FlickerAfterTimer());
			}
		}
		else
		{
			this.source.clip = this.clips[UnityEngine.Random.Range(0, this.clips.Length)];
			this.source.Play();
			if (this.lever != null)
			{
				Quaternion localRotation2 = this.lever.transform.localRotation;
				localRotation2.eulerAngles = new Vector3(-15f, 0f, 0f);
				this.lever.transform.localRotation = localRotation2;
			}
			if (LevelController.instance.fuseBox.isOn)
			{
				foreach (Light light2 in this.lights)
				{
					if (light2 != null)
					{
						light2.enabled = false;
					}
				}
				foreach (Renderer renderer2 in this.rends)
				{
					if (renderer2 != null)
					{
						for (int l = 0; l < renderer2.materials.Length; l++)
						{
							renderer2.materials[l].DisableKeyword("_EMISSION");
						}
					}
				}
				Animator[] array = this.animators;
				for (int j = 0; j < array.Length; j++)
				{
					array[j].SetBool("isOn", false);
				}
				AudioSource[] array2 = this.sources;
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j].Stop();
				}
				for (int m = 0; m < this.objectsToActivate.Length; m++)
				{
					this.objectsToActivate[m].SetActive(false);
				}
				this.ResetReflectionProbes();
			}
			base.StopAllCoroutines();
		}
		LevelController.instance.fuseBox.ChangeOnLights(this.isOn ? 1 : -1);
		if (this.noise != null)
		{
			base.StartCoroutine(this.PlayNoiseObject());
		}
	}

	private IEnumerator PlayNoiseObject()
	{
		this.noise.gameObject.SetActive(true);
		yield return 0;
		this.noise.gameObject.SetActive(false);
		yield break;
	}

	public void FuseOff()
	{
		foreach (Light light in this.lights)
		{
			light.enabled = false;
		}
		foreach (Renderer renderer in this.rends)
		{
			for (int i = 0; i < renderer.materials.Length; i++)
			{
				renderer.materials[i].DisableKeyword("_EMISSION");
			}
		}
		Animator[] array = this.animators;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].SetBool("isOn", false);
		}
		AudioSource[] array2 = this.sources;
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j].Stop();
		}
		for (int k = 0; k < this.objectsToActivate.Length; k++)
		{
			this.objectsToActivate[k].SetActive(false);
		}
		base.StopAllCoroutines();
	}

	public void FuseOn()
	{
		if (this.isOn)
		{
			foreach (Light light in this.lights)
			{
				light.enabled = true;
			}
			foreach (Renderer renderer in this.rends)
			{
				for (int i = 0; i < renderer.materials.Length; i++)
				{
					renderer.materials[i].EnableKeyword("_EMISSION");
				}
			}
			Animator[] array = this.animators;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].SetBool("isOn", true);
			}
			AudioSource[] array2 = this.sources;
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j].Play();
			}
			for (int k = 0; k < this.objectsToActivate.Length; k++)
			{
				this.objectsToActivate[k].SetActive(true);
			}
			base.StartCoroutine(this.FlickerAfterTimer());
		}
	}

	public void StartBlinking()
	{
		this.isBlinking = true;
	}

	public void StopBlinking()
	{
		this.isBlinking = false;
		if (PhotonNetwork.IsMasterClient)
		{
			this.view.RPC("ResetLights", RpcTarget.All, Array.Empty<object>());
			this.view.RPC("TurnOffNetworked", RpcTarget.All, new object[]
			{
				false
			});
		}
	}

	[PunRPC]
	private void ResetLights()
	{
		for (int i = 0; i < this.lights.Count; i++)
		{
			this.lights[i].intensity = this.lightsMaxIntensity[i];
		}
	}

	private void Update()
	{
		if (this.isBlinking)
		{
			if (!this.isOn)
			{
				return;
			}
			this.blinkTimer -= Time.deltaTime;
			if (this.blinkTimer < 0f)
			{
				this.Blink();
				this.blinkTimer = UnityEngine.Random.Range(0.1f, 0.5f);
			}
		}
	}

	public void Blink()
	{
		if (LevelController.instance.fuseBox.isOn)
		{
			if (LevelController.instance.currentGhostRoom == this.myRoom)
			{
				for (int i = 0; i < this.lights.Count; i++)
				{
					this.lights[i].intensity = 0f;
				}
				for (int j = 0; j < this.rends.Count; j++)
				{
					for (int k = 0; k < this.rends[j].materials.Length; k++)
					{
						if (this.rends[j].materials[k].IsKeywordEnabled("_EMISSION"))
						{
							this.rends[j].materials[k].DisableKeyword("_EMISSION");
						}
					}
				}
				return;
			}
			for (int l = 0; l < this.lights.Count; l++)
			{
				if (this.lights[l].intensity == 0f)
				{
					this.lights[l].intensity = this.lightsMaxIntensity[l];
				}
				else
				{
					this.lights[l].intensity = 0f;
				}
			}
			for (int m = 0; m < this.rends.Count; m++)
			{
				for (int n = 0; n < this.rends[m].materials.Length; n++)
				{
					if (this.rends[m].materials[n].IsKeywordEnabled("_EMISSION"))
					{
						this.rends[m].materials[n].DisableKeyword("_EMISSION");
					}
					else
					{
						this.rends[m].materials[n].EnableKeyword("_EMISSION");
					}
				}
			}
		}
	}

	public void TurnOff()
	{
		this.view.RPC("TurnOffNetworked", RpcTarget.AllBuffered, new object[]
		{
			true
		});
	}

	[PunRPC]
	public void TurnOffNetworked(bool playSound)
	{
		this.isOn = false;
		if (playSound)
		{
			this.source.clip = this.clips[UnityEngine.Random.Range(0, this.clips.Length)];
			this.source.Play();
			if (this.lever != null)
			{
				Quaternion localRotation = this.lever.transform.localRotation;
				localRotation.eulerAngles = new Vector3(-15f, 0f, 0f);
				this.lever.transform.localRotation = localRotation;
			}
		}
		if (LevelController.instance.fuseBox.isOn)
		{
			foreach (Light light in this.lights)
			{
				if (light != null)
				{
					light.enabled = false;
				}
			}
			foreach (Renderer renderer in this.rends)
			{
				if (renderer != null)
				{
					for (int i = 0; i < renderer.materials.Length; i++)
					{
						renderer.materials[i].DisableKeyword("_EMISSION");
					}
				}
			}
			Animator[] array = this.animators;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].SetBool("isOn", false);
			}
			AudioSource[] array2 = this.sources;
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j].Stop();
			}
			for (int k = 0; k < this.objectsToActivate.Length; k++)
			{
				this.objectsToActivate[k].SetActive(false);
			}
			this.ResetReflectionProbes();
		}
		if (this.noise != null)
		{
			base.StartCoroutine(this.PlayNoiseObject());
		}
	}

	public void TurnOn(bool playSound)
	{
		this.view.RPC("TurnOnNetworked", RpcTarget.All, new object[]
		{
			playSound
		});
	}

	[PunRPC]
	public void TurnOnNetworked(bool playSound)
	{
		this.isOn = true;
		if (playSound)
		{
			this.source.clip = this.clips[UnityEngine.Random.Range(0, this.clips.Length)];
			this.source.Play();
			if (this.lever != null)
			{
				Quaternion localRotation = this.lever.transform.localRotation;
				localRotation.eulerAngles = new Vector3(15f, 0f, 0f);
				this.lever.transform.localRotation = localRotation;
			}
		}
		if (LevelController.instance.fuseBox.isOn)
		{
			foreach (Light light in this.lights)
			{
				light.enabled = true;
			}
			foreach (Renderer renderer in this.rends)
			{
				for (int i = 0; i < renderer.materials.Length; i++)
				{
					renderer.materials[i].EnableKeyword("_EMISSION");
				}
			}
			Animator[] array = this.animators;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].SetBool("isOn", true);
			}
			AudioSource[] array2 = this.sources;
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j].Play();
			}
			for (int k = 0; k < this.objectsToActivate.Length; k++)
			{
				this.objectsToActivate[k].SetActive(true);
			}
			this.ResetReflectionProbes();
			if (playSound)
			{
				base.StartCoroutine(this.FlickerAfterTimer());
			}
		}
	}

	private IEnumerator FlickerAfterTimer()
	{
		yield return new WaitForSeconds(UnityEngine.Random.Range(30f, 160f));
		if (PhotonNetwork.IsMasterClient)
		{
			this.view.RPC("FlickerNetworked", RpcTarget.All, Array.Empty<object>());
		}
		base.StartCoroutine(this.FlickerAfterTimer());
		yield break;
	}

	[PunRPC]
	private void FlickerNetworked()
	{
		base.StartCoroutine(this.Flicker());
	}

	private IEnumerator Flicker()
	{
		if (!this.isOn)
		{
			yield return null;
		}
		foreach (Light light in this.lights)
		{
			light.enabled = false;
		}
		foreach (Renderer renderer in this.rends)
		{
			for (int i = 0; i < renderer.materials.Length; i++)
			{
				renderer.materials[i].DisableKeyword("_EMISSION");
			}
		}
		Animator[] array = this.animators;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].SetBool("isOn", false);
		}
		yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.4f));
		foreach (Light light2 in this.lights)
		{
			light2.enabled = true;
		}
		foreach (Renderer renderer2 in this.rends)
		{
			for (int k = 0; k < renderer2.materials.Length; k++)
			{
				renderer2.materials[k].EnableKeyword("_EMISSION");
			}
		}
		array = this.animators;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].SetBool("isOn", true);
		}
		yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.4f));
		foreach (Light light3 in this.lights)
		{
			light3.enabled = false;
		}
		foreach (Renderer renderer3 in this.rends)
		{
			for (int l = 0; l < renderer3.materials.Length; l++)
			{
				renderer3.materials[l].DisableKeyword("_EMISSION");
			}
		}
		array = this.animators;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].SetBool("isOn", false);
		}
		yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.4f));
		this.TurnOn(false);
		yield break;
	}

	public void ResetReflectionProbes()
	{
		foreach (ReflectionProbe reflectionProbe in this.probes)
		{
			if (reflectionProbe != null)
			{
				reflectionProbe.RenderProbe();
			}
		}
	}

	public void SmashBulb()
	{
		this.view.RPC("SmashBulbNetworked", RpcTarget.All, Array.Empty<object>());
	}

	[PunRPC]
	public void SmashBulbNetworked()
	{
		this.isOn = false;
		this.source.clip = this.bulbSmashClip;
		this.source.Play();
		if (LevelController.instance.fuseBox.isOn)
		{
			foreach (Light light in this.lights)
			{
				if (light != null)
				{
					light.enabled = false;
				}
			}
			foreach (Renderer renderer in this.rends)
			{
				if (renderer != null)
				{
					for (int i = 0; i < renderer.materials.Length; i++)
					{
						renderer.materials[i].DisableKeyword("_EMISSION");
					}
				}
			}
			Animator[] array = this.animators;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].SetBool("isOn", false);
			}
			AudioSource[] array2 = this.sources;
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j].Stop();
			}
			for (int k = 0; k < this.objectsToActivate.Length; k++)
			{
				this.objectsToActivate[k].SetActive(false);
			}
			this.ResetReflectionProbes();
		}
		base.StartCoroutine(this.PlayNoiseObject());
	}

	public void SpawnHandPrintEvidence()
	{
		if (this.handPrintObject == null)
		{
			return;
		}
		if (this.handPrintObject.activeInHierarchy)
		{
			return;
		}
		this.view.RPC("NetworkedSpawnHandPrintEvidence", RpcTarget.All, Array.Empty<object>());
	}

	[PunRPC]
	private void NetworkedSpawnHandPrintEvidence()
	{
		this.handPrintObject.SetActive(true);
	}

	private Light myLight;

	private AudioSource source;

	[HideInInspector]
	public PhotonView view;

	[SerializeField]
	private AudioClip[] clips;

	[SerializeField]
	private AudioClip bulbSmashClip;

	public List<Light> lights = new List<Light>();

	public List<Renderer> rends = new List<Renderer>();

	public List<ReflectionProbe> probes = new List<ReflectionProbe>();

	public Animator[] animators;

	public AudioSource[] sources;

	public GameObject[] objectsToActivate;

	public Transform lever;

	public PhotonObjectInteract photonInteract;

	private List<float> lightsMaxIntensity = new List<float>();

	private float blinkTimer;

	[HideInInspector]
	public bool isBlinking;

	private Noise noise;

	[HideInInspector]
	public bool isOn;

	[HideInInspector]
	public LevelRoom myRoom;

	[SerializeField]
	private GameObject handPrintObject;
}


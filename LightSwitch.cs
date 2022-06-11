using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

// Token: 0x02000101 RID: 257
public class LightSwitch : MonoBehaviour
{
	// Token: 0x06000702 RID: 1794 RVA: 0x000296A4 File Offset: 0x000278A4
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

	// Token: 0x06000703 RID: 1795 RVA: 0x0002977C File Offset: 0x0002797C
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

	// Token: 0x06000704 RID: 1796 RVA: 0x00029838 File Offset: 0x00027A38
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

	// Token: 0x06000705 RID: 1797 RVA: 0x0002988C File Offset: 0x00027A8C
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

	// Token: 0x06000706 RID: 1798 RVA: 0x00029CAC File Offset: 0x00027EAC
	private IEnumerator PlayNoiseObject()
	{
		this.noise.gameObject.SetActive(true);
		yield return 0;
		this.noise.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x00029CBC File Offset: 0x00027EBC
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

	// Token: 0x06000708 RID: 1800 RVA: 0x00029DEC File Offset: 0x00027FEC
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

	// Token: 0x06000709 RID: 1801 RVA: 0x00029F2C File Offset: 0x0002812C
	public void StartBlinking()
	{
		this.isBlinking = true;
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x00029F38 File Offset: 0x00028138
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

	// Token: 0x0600070B RID: 1803 RVA: 0x00029F8C File Offset: 0x0002818C
	[PunRPC]
	private void ResetLights()
	{
		for (int i = 0; i < this.lights.Count; i++)
		{
			this.lights[i].intensity = this.lightsMaxIntensity[i];
		}
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x00029FCC File Offset: 0x000281CC
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

	// Token: 0x0600070D RID: 1805 RVA: 0x0002A024 File Offset: 0x00028224
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

	// Token: 0x0600070E RID: 1806 RVA: 0x0002A203 File Offset: 0x00028403
	public void TurnOff()
	{
		this.view.RPC("TurnOffNetworked", RpcTarget.AllBuffered, new object[]
		{
			true
		});
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x0002A228 File Offset: 0x00028428
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

	// Token: 0x06000710 RID: 1808 RVA: 0x0002A424 File Offset: 0x00028624
	public void TurnOn(bool playSound)
	{
		this.view.RPC("TurnOnNetworked", RpcTarget.All, new object[]
		{
			playSound
		});
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x0002A448 File Offset: 0x00028648
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

	// Token: 0x06000712 RID: 1810 RVA: 0x0002A620 File Offset: 0x00028820
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

	// Token: 0x06000713 RID: 1811 RVA: 0x0002A62F File Offset: 0x0002882F
	[PunRPC]
	private void FlickerNetworked()
	{
		base.StartCoroutine(this.Flicker());
	}

	// Token: 0x06000714 RID: 1812 RVA: 0x0002A63E File Offset: 0x0002883E
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

	// Token: 0x06000715 RID: 1813 RVA: 0x0002A650 File Offset: 0x00028850
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

	// Token: 0x06000716 RID: 1814 RVA: 0x0002A6AC File Offset: 0x000288AC
	public void SmashBulb()
	{
		this.view.RPC("SmashBulbNetworked", RpcTarget.All, Array.Empty<object>());
	}

	// Token: 0x06000717 RID: 1815 RVA: 0x0002A6C4 File Offset: 0x000288C4
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

	// Token: 0x06000718 RID: 1816 RVA: 0x0002A850 File Offset: 0x00028A50
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

	// Token: 0x06000719 RID: 1817 RVA: 0x0002A885 File Offset: 0x00028A85
	[PunRPC]
	private void NetworkedSpawnHandPrintEvidence()
	{
		this.handPrintObject.SetActive(true);
	}

	// Token: 0x04000725 RID: 1829
	private Light myLight;

	// Token: 0x04000726 RID: 1830
	private AudioSource source;

	// Token: 0x04000727 RID: 1831
	[HideInInspector]
	public PhotonView view;

	// Token: 0x04000728 RID: 1832
	[SerializeField]
	private AudioClip[] clips;

	// Token: 0x04000729 RID: 1833
	[SerializeField]
	private AudioClip bulbSmashClip;

	// Token: 0x0400072A RID: 1834
	public List<Light> lights = new List<Light>();

	// Token: 0x0400072B RID: 1835
	public List<Renderer> rends = new List<Renderer>();

	// Token: 0x0400072C RID: 1836
	public List<ReflectionProbe> probes = new List<ReflectionProbe>();

	// Token: 0x0400072D RID: 1837
	public Animator[] animators;

	// Token: 0x0400072E RID: 1838
	public AudioSource[] sources;

	// Token: 0x0400072F RID: 1839
	public GameObject[] objectsToActivate;

	// Token: 0x04000730 RID: 1840
	public Transform lever;

	// Token: 0x04000731 RID: 1841
	public PhotonObjectInteract photonInteract;

	// Token: 0x04000732 RID: 1842
	private List<float> lightsMaxIntensity = new List<float>();

	// Token: 0x04000733 RID: 1843
	private float blinkTimer;

	// Token: 0x04000734 RID: 1844
	[HideInInspector]
	public bool isBlinking;

	// Token: 0x04000735 RID: 1845
	private Noise noise;

	// Token: 0x04000736 RID: 1846
	[HideInInspector]
	public bool isOn;

	// Token: 0x04000737 RID: 1847
	[HideInInspector]
	public LevelRoom myRoom;

	// Token: 0x04000738 RID: 1848
	[SerializeField]
	private GameObject handPrintObject;
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(AudioSource))]
public class Torch : MonoBehaviour
{
	private void Awake()
	{
		this.startIntensity = this.myLight.intensity;
		this.noise.gameObject.SetActive(false);
		this.blinkTimer = UnityEngine.Random.Range(0f, 0.5f);
	}

	private void Start()
	{
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
		this.myLight.enabled = false;
		if (this.isBlacklight)
		{
			this.glass.material.DisableKeyword("_EMISSION");
			this.bulb.material.DisableKeyword("_EMISSION");
		}
		else
		{
			for (int i = 0; i < this.rend.materials.Length; i++)
			{
				this.rend.materials[i].DisableKeyword("_EMISSION");
			}
		}
		if (LevelController.instance)
		{
			LevelController.instance.torches.Add(this);
		}
	}

	public void Use()
	{
		if (PhotonNetwork.InRoom)
		{
			this.view.RPC("NetworkedUse", RpcTarget.AllBuffered, Array.Empty<object>());
			return;
		}
		this.NetworkedUse();
	}

	[PunRPC]
	private void NetworkedUse()
	{
		this.myLight.enabled = !this.myLight.enabled;
		this.source.Play();
		if (this.myLight.enabled)
		{
			if (this.isBlacklight)
			{
				this.glass.material.EnableKeyword("_EMISSION");
				this.bulb.material.EnableKeyword("_EMISSION");
			}
			else
			{
				for (int i = 0; i < this.rend.materials.Length; i++)
				{
					this.rend.materials[i].EnableKeyword("_EMISSION");
				}
			}
		}
		else if (this.isBlacklight)
		{
			this.glass.material.DisableKeyword("_EMISSION");
			this.bulb.material.DisableKeyword("_EMISSION");
		}
		else
		{
			for (int j = 0; j < this.rend.materials.Length; j++)
			{
				this.rend.materials[j].DisableKeyword("_EMISSION");
			}
		}
		base.StartCoroutine(this.PlayNoiseObject());
	}

	public void StartTrailerFlicker()
	{
		base.StartCoroutine(this.TrailerFlicker());
	}

	private IEnumerator TrailerFlicker()
	{
		yield return new WaitForSeconds(1f);
		this.myLight.intensity = 1.7f;
		yield return new WaitForSeconds(0.15f);
		this.myLight.intensity = 0.6f;
		yield return new WaitForSeconds(0.2f);
		this.myLight.intensity = 1.3f;
		yield return new WaitForSeconds(0.1f);
		this.myLight.intensity = 0.9f;
		yield return new WaitForSeconds(0.15f);
		this.myLight.intensity = 1.2f;
		yield return new WaitForSeconds(0.25f);
		this.myLight.intensity = 0f;
		for (int i = 0; i < this.rend.materials.Length; i++)
		{
			this.rend.materials[i].DisableKeyword("_EMISSION");
		}
		yield break;
	}

	public void TurnBlinkOff()
	{
		this.myLight.intensity = this.startIntensity;
		if (this.myLight.enabled)
		{
			if (this.isBlacklight)
			{
				this.glass.material.EnableKeyword("_EMISSION");
				this.bulb.material.EnableKeyword("_EMISSION");
				return;
			}
			for (int i = 0; i < this.rend.materials.Length; i++)
			{
				if (this.rend.materials[i] != null)
				{
					this.rend.materials[i].EnableKeyword("_EMISSION");
				}
			}
			return;
		}
		else
		{
			if (this.isBlacklight)
			{
				this.glass.material.DisableKeyword("_EMISSION");
				this.bulb.material.DisableKeyword("_EMISSION");
				return;
			}
			for (int j = 0; j < this.rend.materials.Length; j++)
			{
				if (this.rend.materials[j] != null)
				{
					this.rend.materials[j].DisableKeyword("_EMISSION");
				}
			}
			return;
		}
	}

	private void Update()
	{
		if (LevelController.instance == null)
		{
			return;
		}
		if (LevelController.instance.currentGhost == null)
		{
			return;
		}
		if (LevelController.instance.currentGhost.isHunting)
		{
			this.blinkTimer -= Time.deltaTime;
			if (this.blinkTimer < 0f)
			{
				this.Blink();
				this.blinkTimer = UnityEngine.Random.Range(0.1f, 0.5f);
			}
		}
	}

	private void Blink()
	{
		if (this.myLight.intensity == 0f)
		{
			this.myLight.intensity = this.startIntensity;
			if (this.isBlacklight)
			{
				this.glass.material.EnableKeyword("_EMISSION");
				this.bulb.material.EnableKeyword("_EMISSION");
				return;
			}
			for (int i = 0; i < this.rend.materials.Length; i++)
			{
				this.rend.materials[i].EnableKeyword("_EMISSION");
			}
			return;
		}
		else
		{
			this.myLight.intensity = 0f;
			if (this.isBlacklight)
			{
				this.glass.material.DisableKeyword("_EMISSION");
				this.bulb.material.DisableKeyword("_EMISSION");
				return;
			}
			for (int j = 0; j < this.rend.materials.Length; j++)
			{
				this.rend.materials[j].DisableKeyword("_EMISSION");
			}
			return;
		}
	}

	private IEnumerator PlayNoiseObject()
	{
		this.noise.gameObject.SetActive(true);
		yield return 0;
		this.noise.gameObject.SetActive(false);
		yield break;
	}

	public Light myLight;

	public AudioSource source;

	[SerializeField]
	private Renderer rend;

	[SerializeField]
	private PhotonView view;

	[SerializeField]
	private PhotonObjectInteract photonInteract;

	public bool isBlacklight;

	[SerializeField]
	private Renderer glass;

	[SerializeField]
	private Renderer bulb;

	[SerializeField]
	private Noise noise;

	private float startIntensity;

	private float blinkTimer;
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LightningController : MonoBehaviour
{
	private void Awake()
	{
		this.myLight = base.GetComponent<Light>();
		this.source = base.GetComponent<AudioSource>();
		this.view = base.GetComponent<PhotonView>();
		LightningController.instance = this;
	}

	private void Start()
	{
		this.defaultExposure = RenderSettings.skybox.GetFloat("_Exposure");
		this.timer = UnityEngine.Random.Range(30f, 100f);
		this.myLight.intensity = 0f;
		this.isUsing = false;
	}

	private void Update()
	{
		if (!this.view.IsMine)
		{
			return;
		}
		if (!this.isUsing)
		{
			this.timer -= Time.deltaTime;
			if (this.timer < 0f)
			{
				this.PlayLightning();
				this.isUsing = true;
			}
		}
	}

	public void PlayLightning()
	{
		this.view.RPC("PlayLightningNetworked", RpcTarget.All, Array.Empty<object>());
	}

	[PunRPC]
	private void PlayLightningNetworked()
	{
		if (!GameController.instance.myPlayer.player.isDead)
		{
			base.StartCoroutine(this.Lightning(UnityEngine.Random.Range(0f, 2f), UnityEngine.Random.Range(0f, 1f)));
		}
	}

	private IEnumerator Lightning(float rand1, float rand2)
	{
		base.Invoke("PlaySound", rand1);
		while ((double)this.myLight.intensity < 0.95)
		{
			this.myLight.intensity = Mathf.Lerp(this.myLight.intensity, 1f, Time.deltaTime * 1f);
			RenderSettings.skybox.SetFloat("_Exposure", Mathf.Lerp(RenderSettings.skybox.GetFloat("_Exposure"), 2f, Time.deltaTime * 4f));
		}
		yield return new WaitForSeconds(rand2);
		this.myLight.intensity = 0f;
		RenderSettings.skybox.SetFloat("_Exposure", this.defaultExposure);
		this.timer = UnityEngine.Random.Range(30f, 100f);
		this.isUsing = false;
		yield break;
	}

	private void PlaySound()
	{
		this.source.volume = UnityEngine.Random.Range(0.005f, 0.01f);
		this.source.clip = this.clips[UnityEngine.Random.Range(0, this.clips.Count)];
		this.source.Play();
	}

	private void OnDisable()
	{
		RenderSettings.skybox.SetFloat("_Exposure", this.defaultExposure);
	}

	private void OnDestroy()
	{
		RenderSettings.skybox.SetFloat("_Exposure", this.defaultExposure);
	}

	private AudioSource source;

	private Light myLight;

	public List<AudioClip> clips = new List<AudioClip>();

	private PhotonView view;

	private float timer = 5f;

	private bool isUsing;

	private float defaultExposure;

	public static LightningController instance;
}


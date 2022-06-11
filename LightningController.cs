using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// Token: 0x020000D7 RID: 215
public class LightningController : MonoBehaviour
{
	// Token: 0x06000612 RID: 1554 RVA: 0x00024273 File Offset: 0x00022473
	private void Awake()
	{
		this.myLight = base.GetComponent<Light>();
		this.source = base.GetComponent<AudioSource>();
		this.view = base.GetComponent<PhotonView>();
		LightningController.instance = this;
	}

	// Token: 0x06000613 RID: 1555 RVA: 0x000242A0 File Offset: 0x000224A0
	private void Start()
	{
		this.defaultExposure = RenderSettings.skybox.GetFloat("_Exposure");
		this.timer = UnityEngine.Random.Range(30f, 100f);
		this.myLight.intensity = 0f;
		this.isUsing = false;
	}

	// Token: 0x06000614 RID: 1556 RVA: 0x000242F0 File Offset: 0x000224F0
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

	// Token: 0x06000615 RID: 1557 RVA: 0x0002433F File Offset: 0x0002253F
	public void PlayLightning()
	{
		this.view.RPC("PlayLightningNetworked", RpcTarget.All, Array.Empty<object>());
	}

	// Token: 0x06000616 RID: 1558 RVA: 0x00024358 File Offset: 0x00022558
	[PunRPC]
	private void PlayLightningNetworked()
	{
		if (!GameController.instance.myPlayer.player.isDead)
		{
			base.StartCoroutine(this.Lightning(UnityEngine.Random.Range(0f, 2f), UnityEngine.Random.Range(0f, 1f)));
		}
	}

	// Token: 0x06000617 RID: 1559 RVA: 0x000243A6 File Offset: 0x000225A6
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

	// Token: 0x06000618 RID: 1560 RVA: 0x000243C4 File Offset: 0x000225C4
	private void PlaySound()
	{
		this.source.volume = UnityEngine.Random.Range(0.005f, 0.01f);
		this.source.clip = this.clips[UnityEngine.Random.Range(0, this.clips.Count)];
		this.source.Play();
	}

	// Token: 0x06000619 RID: 1561 RVA: 0x0002441D File Offset: 0x0002261D
	private void OnDisable()
	{
		RenderSettings.skybox.SetFloat("_Exposure", this.defaultExposure);
	}

	// Token: 0x0600061A RID: 1562 RVA: 0x0002441D File Offset: 0x0002261D
	private void OnDestroy()
	{
		RenderSettings.skybox.SetFloat("_Exposure", this.defaultExposure);
	}

	// Token: 0x040005E6 RID: 1510
	private AudioSource source;

	// Token: 0x040005E7 RID: 1511
	private Light myLight;

	// Token: 0x040005E8 RID: 1512
	public List<AudioClip> clips = new List<AudioClip>();

	// Token: 0x040005E9 RID: 1513
	private PhotonView view;

	// Token: 0x040005EA RID: 1514
	private float timer = 5f;

	// Token: 0x040005EB RID: 1515
	private bool isUsing;

	// Token: 0x040005EC RID: 1516
	private float defaultExposure;

	// Token: 0x040005ED RID: 1517
	public static LightningController instance;
}

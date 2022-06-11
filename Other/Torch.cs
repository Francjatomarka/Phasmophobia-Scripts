using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

// Token: 0x02000125 RID: 293
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(AudioSource))]
public class Torch : MonoBehaviour
{
	// Token: 0x06000835 RID: 2101 RVA: 0x00031E8B File Offset: 0x0003008B
	private void Awake()
	{
		this.startIntensity = this.myLight.intensity;
		this.noise.gameObject.SetActive(false);
		this.blinkTimer = UnityEngine.Random.Range(0f, 0.5f);
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x00031EC4 File Offset: 0x000300C4
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

	// Token: 0x06000837 RID: 2103 RVA: 0x00031F73 File Offset: 0x00030173
	public void Use()
	{
		if (PhotonNetwork.InRoom)
		{
			this.view.RPC("NetworkedUse", RpcTarget.AllBuffered, Array.Empty<object>());
			return;
		}
		this.NetworkedUse();
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x00031F9C File Offset: 0x0003019C
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

	// Token: 0x06000839 RID: 2105 RVA: 0x000320B2 File Offset: 0x000302B2
	public void StartTrailerFlicker()
	{
		base.StartCoroutine(this.TrailerFlicker());
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x000320C1 File Offset: 0x000302C1
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

	// Token: 0x0600083B RID: 2107 RVA: 0x000320D0 File Offset: 0x000302D0
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

	// Token: 0x0600083C RID: 2108 RVA: 0x000321EC File Offset: 0x000303EC
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

	// Token: 0x0600083D RID: 2109 RVA: 0x00032268 File Offset: 0x00030468
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

	// Token: 0x0600083E RID: 2110 RVA: 0x0003236D File Offset: 0x0003056D
	private IEnumerator PlayNoiseObject()
	{
		this.noise.gameObject.SetActive(true);
		yield return 0;
		this.noise.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x0400083C RID: 2108
	public Light myLight;

	// Token: 0x0400083D RID: 2109
	public AudioSource source;

	// Token: 0x0400083E RID: 2110
	[SerializeField]
	private Renderer rend;

	// Token: 0x0400083F RID: 2111
	[SerializeField]
	private PhotonView view;

	// Token: 0x04000840 RID: 2112
	[SerializeField]
	private PhotonObjectInteract photonInteract;

	// Token: 0x04000841 RID: 2113
	public bool isBlacklight;

	// Token: 0x04000842 RID: 2114
	[SerializeField]
	private Renderer glass;

	// Token: 0x04000843 RID: 2115
	[SerializeField]
	private Renderer bulb;

	// Token: 0x04000844 RID: 2116
	[SerializeField]
	private Noise noise;

	// Token: 0x04000845 RID: 2117
	private float startIntensity;

	// Token: 0x04000846 RID: 2118
	private float blinkTimer;
}

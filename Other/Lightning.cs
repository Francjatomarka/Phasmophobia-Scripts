using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200002D RID: 45
public class Lightning : MonoBehaviour
{
	// Token: 0x06000117 RID: 279 RVA: 0x00008DF1 File Offset: 0x00006FF1
	private void Start()
	{
		base.StartCoroutine("Storm");
	}

	// Token: 0x06000118 RID: 280 RVA: 0x00008DFF File Offset: 0x00006FFF
	private IEnumerator Storm()
	{
		for (;;)
		{
			yield return new WaitForSeconds(UnityEngine.Random.Range(this.offMin, this.offMax));
			this.LightningBolt.SetActive(true);
			this.LightningBolt.transform.Rotate(0f, (float)UnityEngine.Random.Range(1, 360), 0f);
			base.StartCoroutine("Soundfx");
			yield return new WaitForSeconds(UnityEngine.Random.Range(this.onMin, this.onMax));
			this.LightningBolt.SetActive(false);
		}
		yield break;
	}

	// Token: 0x06000119 RID: 281 RVA: 0x00008E0E File Offset: 0x0000700E
	private IEnumerator Soundfx()
	{
		this.ThunderRND = UnityEngine.Random.Range(1, 5);
		this.ThunderVol = UnityEngine.Random.Range(0.2f, 1f);
		this.ThunderWait = 9f - this.ThunderVol * 3f * 3f - 2f;
		while (this.ThunderRND == 1)
		{
			yield return new WaitForSeconds(this.ThunderWait);
			this.ThunderAudioA.volume = this.ThunderVol;
			this.ThunderAudioA.Play();
			this.ThunderRND = 0;
		}
		while (this.ThunderRND == 2)
		{
			yield return new WaitForSeconds(this.ThunderWait);
			this.ThunderAudioB.volume = this.ThunderVol;
			this.ThunderAudioB.Play();
			this.ThunderRND = 0;
		}
		while (this.ThunderRND == 3)
		{
			yield return new WaitForSeconds(this.ThunderWait);
			this.ThunderAudioC.volume = this.ThunderVol;
			this.ThunderAudioC.Play();
			this.ThunderRND = 0;
		}
		while (this.ThunderRND == 4)
		{
			yield return new WaitForSeconds(this.ThunderWait);
			this.ThunderAudioD.volume = this.ThunderVol;
			this.ThunderAudioD.Play();
			this.ThunderRND = 0;
		}
		yield break;
	}

	// Token: 0x04000151 RID: 337
	public float offMin = 10f;

	// Token: 0x04000152 RID: 338
	public float offMax = 60f;

	// Token: 0x04000153 RID: 339
	public AudioSource ThunderAudioA;

	// Token: 0x04000154 RID: 340
	public AudioSource ThunderAudioB;

	// Token: 0x04000155 RID: 341
	public AudioSource ThunderAudioC;

	// Token: 0x04000156 RID: 342
	public AudioSource ThunderAudioD;

	// Token: 0x04000157 RID: 343
	public GameObject LightningBolt;

	// Token: 0x04000158 RID: 344
	private float onMin = 0.25f;

	// Token: 0x04000159 RID: 345
	private float onMax = 2f;

	// Token: 0x0400015A RID: 346
	private int ThunderRND = 1;

	// Token: 0x0400015B RID: 347
	private float ThunderVol;

	// Token: 0x0400015C RID: 348
	private float ThunderWait;
}

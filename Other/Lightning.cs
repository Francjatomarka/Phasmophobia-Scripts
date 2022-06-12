using System;
using System.Collections;
using UnityEngine;

public class Lightning : MonoBehaviour
{
	private void Start()
	{
		base.StartCoroutine("Storm");
	}

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

	public float offMin = 10f;

	public float offMax = 60f;

	public AudioSource ThunderAudioA;

	public AudioSource ThunderAudioB;

	public AudioSource ThunderAudioC;

	public AudioSource ThunderAudioD;

	public GameObject LightningBolt;

	private float onMin = 0.25f;

	private float onMax = 2f;

	private int ThunderRND = 1;

	private float ThunderVol;

	private float ThunderWait;
}


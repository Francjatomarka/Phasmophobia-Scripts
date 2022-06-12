using System;
using System.Collections;
using UnityEngine;

public class ProtonPack : MonoBehaviour
{
	private void Start()
	{
		this.ProtonMainFX.SetActive(false);
		this.ProtonExtraFX.SetActive(false);
	}

	private void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			base.StartCoroutine("ProtonPackFire");
		}
		if (Input.GetButtonUp("Fire1"))
		{
			this.ProtonPackStop();
		}
	}

	private IEnumerator ProtonPackFire()
	{
		this.ProtonExtraFX.SetActive(true);
		this.beamStartAudio.Play();
		this.protonBeamFlag = 0;
		yield return new WaitForSeconds(0.5f);
		if (this.protonBeamFlag == 0)
		{
			this.ProtonMainFX.SetActive(true);
			this.lightningBoltParticles.Play();
			this.protonBeamParticles.Play();
			this.beamMainAudio.Play();
		}
		yield break;
	}

	private void ProtonPackStop()
	{
		this.protonBeamFlag = 1;
		this.ProtonMainFX.SetActive(false);
		this.lightningBoltParticles.Stop();
		this.protonBeamParticles.Stop();
		this.beamMainAudio.Stop();
		this.beamStartAudio.Stop();
		this.beamStopAudio.Play();
	}

	public GameObject ProtonMainFX;

	public GameObject ProtonExtraFX;

	public AudioSource beamMainAudio;

	public AudioSource beamStartAudio;

	public AudioSource beamStopAudio;

	public ParticleSystem lightningBoltParticles;

	public ParticleSystem protonBeamParticles;

	private int protonBeamFlag;
}


using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200002E RID: 46
public class ProtonPack : MonoBehaviour
{
	// Token: 0x0600011B RID: 283 RVA: 0x00008E58 File Offset: 0x00007058
	private void Start()
	{
		this.ProtonMainFX.SetActive(false);
		this.ProtonExtraFX.SetActive(false);
	}

	// Token: 0x0600011C RID: 284 RVA: 0x00008E72 File Offset: 0x00007072
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

	// Token: 0x0600011D RID: 285 RVA: 0x00008E9E File Offset: 0x0000709E
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

	// Token: 0x0600011E RID: 286 RVA: 0x00008EB0 File Offset: 0x000070B0
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

	// Token: 0x0400015D RID: 349
	public GameObject ProtonMainFX;

	// Token: 0x0400015E RID: 350
	public GameObject ProtonExtraFX;

	// Token: 0x0400015F RID: 351
	public AudioSource beamMainAudio;

	// Token: 0x04000160 RID: 352
	public AudioSource beamStartAudio;

	// Token: 0x04000161 RID: 353
	public AudioSource beamStopAudio;

	// Token: 0x04000162 RID: 354
	public ParticleSystem lightningBoltParticles;

	// Token: 0x04000163 RID: 355
	public ParticleSystem protonBeamParticles;

	// Token: 0x04000164 RID: 356
	private int protonBeamFlag;
}

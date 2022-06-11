using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000103 RID: 259
public class Painting : MonoBehaviour
{
	// Token: 0x06000721 RID: 1825 RVA: 0x0002AAC9 File Offset: 0x00028CC9
	private void Awake()
	{
		this.rigid = base.GetComponent<Rigidbody>();
		this.noise = base.GetComponentInChildren<Noise>();
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x0002AAE3 File Offset: 0x00028CE3
	private void Start()
	{
		if (this.noise != null)
		{
			this.noise.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000723 RID: 1827 RVA: 0x0002AB04 File Offset: 0x00028D04
	public void KnockOver()
	{
		this.rigid.isKinematic = false;
		this.rigid.useGravity = true;
		base.enabled = false;
		if (this.noise != null)
		{
			this.PlayNoiseObject();
		}
	}

	// Token: 0x06000724 RID: 1828 RVA: 0x0002AB3A File Offset: 0x00028D3A
	private IEnumerator PlayNoiseObject()
	{
		this.noise.gameObject.SetActive(true);
		yield return 0;
		this.noise.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x0400073B RID: 1851
	private Rigidbody rigid;

	// Token: 0x0400073C RID: 1852
	private Noise noise;
}

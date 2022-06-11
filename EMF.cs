using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// Token: 0x020000A0 RID: 160
[RequireComponent(typeof(SphereCollider))]
public class EMF : MonoBehaviour
{
	// Token: 0x060004B8 RID: 1208 RVA: 0x0001A089 File Offset: 0x00018289
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x0001A097 File Offset: 0x00018297
	public void SetType(EMF.Type type)
	{
		if (type == EMF.Type.GhostInteraction)
		{
			this.strength = 1;
			return;
		}
		if (type == EMF.Type.GhostThrowing)
		{
			this.strength = 2;
			return;
		}
		if (type == EMF.Type.GhostAppeared)
		{
			this.strength = 3;
			return;
		}
		if (type == EMF.Type.GhostEvidence)
		{
			this.strength = 4;
		}
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x0001A0C8 File Offset: 0x000182C8
	private void Update()
	{
		this.timerUntilDeath -= Time.deltaTime;
		if (this.timerUntilDeath <= 0f)
		{
			this.timerUntilDeath = 20f;
			for (int i = 0; i < this.emfReaders.Count; i++)
			{
				this.emfReaders[i].RemoveEMFZone(this);
			}
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x0001A133 File Offset: 0x00018333
	private void OnEnable()
	{
		if (EMFData.instance && !EMFData.instance.emfSpots.Contains(this))
		{
			EMFData.instance.emfSpots.Add(this);
		}
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x0001A163 File Offset: 0x00018363
	private void OnDisable()
	{
		if (EMFData.instance && EMFData.instance.emfSpots.Contains(this))
		{
			EMFData.instance.emfSpots.Remove(this);
		}
	}

	// Token: 0x04000491 RID: 1169
	private PhotonView view;

	// Token: 0x04000492 RID: 1170
	public List<EMFReader> emfReaders;

	// Token: 0x04000493 RID: 1171
	public int strength;

	// Token: 0x04000494 RID: 1172
	public EMF.Type type;

	// Token: 0x04000495 RID: 1173
	private float timerUntilDeath = 20f;

	// Token: 0x0200049E RID: 1182
	public enum Type
	{
		// Token: 0x040021E2 RID: 8674
		GhostInteraction,
		// Token: 0x040021E3 RID: 8675
		GhostThrowing,
		// Token: 0x040021E4 RID: 8676
		GhostAppeared,
		// Token: 0x040021E5 RID: 8677
		GhostEvidence
	}
}

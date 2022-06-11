using System;
using UnityEngine;

// Token: 0x0200000E RID: 14
[ExecuteInEditMode]
public class HxVolumetricParticleSystem : MonoBehaviour
{
	// Token: 0x060000ED RID: 237 RVA: 0x0000D6DC File Offset: 0x0000B8DC
	private void OnEnable()
	{
		this.particleRenderer = base.GetComponent<Renderer>();
		this.LastBounds = this.particleRenderer.bounds;
		this.minBounds = this.LastBounds.min;
		this.maxBounds = this.LastBounds.max;
		if (this.octreeNode == null)
		{
			HxVolumetricCamera.AllParticleSystems.Add(this);
			this.octreeNode = HxVolumetricCamera.AddParticleOctree(this, this.minBounds, this.maxBounds);
		}
	}

	// Token: 0x060000EE RID: 238 RVA: 0x0000D754 File Offset: 0x0000B954
	public void UpdatePosition()
	{
		bool hasChanged = base.transform.hasChanged;
		this.LastBounds = this.particleRenderer.bounds;
		this.minBounds = this.LastBounds.min;
		this.maxBounds = this.LastBounds.max;
		HxVolumetricCamera.ParticleOctree.Move(this.octreeNode, this.minBounds, this.maxBounds);
		base.transform.hasChanged = false;
	}

	// Token: 0x060000EF RID: 239 RVA: 0x0000D7C8 File Offset: 0x0000B9C8
	private void OnDisable()
	{
		if (this.octreeNode != null)
		{
			HxVolumetricCamera.AllParticleSystems.Remove(this);
			HxVolumetricCamera.RemoveParticletOctree(this);
			this.octreeNode = null;
		}
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x0000D7C8 File Offset: 0x0000B9C8
	private void OnDestroy()
	{
		if (this.octreeNode != null)
		{
			HxVolumetricCamera.AllParticleSystems.Remove(this);
			HxVolumetricCamera.RemoveParticletOctree(this);
			this.octreeNode = null;
		}
	}

	// Token: 0x04000179 RID: 377
	[Range(0f, 4f)]
	public float DensityStrength = 1f;

	// Token: 0x0400017A RID: 378
	private HxOctreeNode<HxVolumetricParticleSystem>.NodeObject octreeNode;

	// Token: 0x0400017B RID: 379
	[HideInInspector]
	public Renderer particleRenderer;

	// Token: 0x0400017C RID: 380
	public HxVolumetricParticleSystem.ParticleBlendMode BlendMode = HxVolumetricParticleSystem.ParticleBlendMode.Add;

	// Token: 0x0400017D RID: 381
	private Vector3 minBounds;

	// Token: 0x0400017E RID: 382
	private Vector3 maxBounds;

	// Token: 0x0400017F RID: 383
	private Bounds LastBounds;

	// Token: 0x0200031A RID: 794
	public enum ParticleBlendMode
	{
		// Token: 0x04001509 RID: 5385
		Max,
		// Token: 0x0400150A RID: 5386
		Add,
		// Token: 0x0400150B RID: 5387
		Min,
		// Token: 0x0400150C RID: 5388
		Sub
	}
}

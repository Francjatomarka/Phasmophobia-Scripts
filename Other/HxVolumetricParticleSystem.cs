using System;
using UnityEngine;

[ExecuteInEditMode]
public class HxVolumetricParticleSystem : MonoBehaviour
{
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

	public void UpdatePosition()
	{
		bool hasChanged = base.transform.hasChanged;
		this.LastBounds = this.particleRenderer.bounds;
		this.minBounds = this.LastBounds.min;
		this.maxBounds = this.LastBounds.max;
		HxVolumetricCamera.ParticleOctree.Move(this.octreeNode, this.minBounds, this.maxBounds);
		base.transform.hasChanged = false;
	}

	private void OnDisable()
	{
		if (this.octreeNode != null)
		{
			HxVolumetricCamera.AllParticleSystems.Remove(this);
			HxVolumetricCamera.RemoveParticletOctree(this);
			this.octreeNode = null;
		}
	}

	private void OnDestroy()
	{
		if (this.octreeNode != null)
		{
			HxVolumetricCamera.AllParticleSystems.Remove(this);
			HxVolumetricCamera.RemoveParticletOctree(this);
			this.octreeNode = null;
		}
	}

	[Range(0f, 4f)]
	public float DensityStrength = 1f;

	private HxOctreeNode<HxVolumetricParticleSystem>.NodeObject octreeNode;

	[HideInInspector]
	public Renderer particleRenderer;

	public HxVolumetricParticleSystem.ParticleBlendMode BlendMode = HxVolumetricParticleSystem.ParticleBlendMode.Add;

	private Vector3 minBounds;

	private Vector3 maxBounds;

	private Bounds LastBounds;

	public enum ParticleBlendMode
	{
		Max,
		Add,
		Min,
		Sub
	}
}


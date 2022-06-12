using System;
using UnityEngine;

public class HxVolumetricShadersUsed : ScriptableObject
{
	private bool CheckDirty()
	{
		bool result = false;
		if (Resources.Load("HxUsedShaders") == null)
		{
			result = true;
		}
		if (Resources.Load("HxUsedShaderVariantCollection") == null)
		{
			result = true;
		}
		if (this.Full != this.FullLast)
		{
			result = true;
			this.FullLast = this.Full;
		}
		if (this.LowRes != this.LowResLast)
		{
			result = true;
			this.LowResLast = this.LowRes;
		}
		if (this.HeightFog != this.HeightFogLast)
		{
			result = true;
			this.HeightFogLast = this.HeightFog;
		}
		if (this.HeightFogOff != this.HeightFogOffLast)
		{
			result = true;
			this.HeightFogOffLast = this.HeightFogOff;
		}
		if (this.Noise != this.NoiseLast)
		{
			result = true;
			this.NoiseLast = this.Noise;
		}
		if (this.NoiseOff != this.NoiseOffLast)
		{
			result = true;
			this.NoiseOffLast = this.NoiseOff;
		}
		if (this.Transparency != this.TransparencyLast)
		{
			result = true;
			this.TransparencyLast = this.Transparency;
		}
		if (this.TransparencyOff != this.TransparencyOffLast)
		{
			result = true;
			this.TransparencyOffLast = this.TransparencyOff;
		}
		if (this.DensityParticles != this.DensityParticlesLast)
		{
			result = true;
			this.DensityParticlesLast = this.DensityParticles;
		}
		if (this.Point != this.PointLast)
		{
			result = true;
			this.PointLast = this.Point;
		}
		if (this.Spot != this.SpotLast)
		{
			result = true;
			this.SpotLast = this.Spot;
		}
		if (this.Directional != this.DirectionalLast)
		{
			result = true;
			this.DirectionalLast = this.Directional;
		}
		if (this.SinglePassStereo != this.SinglePassStereoLast)
		{
			result = true;
			this.SinglePassStereoLast = this.SinglePassStereo;
		}
		if (this.Projector != this.ProjectorLast)
		{
			result = true;
			this.ProjectorLast = this.Projector;
		}
		return result;
	}

	public HxVolumetricCamera.TransparencyQualities TransperencyQuality = HxVolumetricCamera.TransparencyQualities.Medium;

	public HxVolumetricCamera.DensityParticleQualities DensityParticleQuality = HxVolumetricCamera.DensityParticleQualities.High;

	[HideInInspector]
	public HxVolumetricCamera.DensityParticleQualities LastDensityParticleQuality = HxVolumetricCamera.DensityParticleQualities.High;

	[HideInInspector]
	public HxVolumetricCamera.TransparencyQualities LastTransperencyQuality = HxVolumetricCamera.TransparencyQualities.Medium;

	private static HxVolumetricShadersUsed instance;

	public bool Full;

	public bool LowRes;

	public bool HeightFog;

	public bool HeightFogOff;

	public bool Noise;

	public bool NoiseOff;

	public bool Transparency;

	public bool TransparencyOff;

	public bool DensityParticles;

	public bool Point;

	public bool Spot;

	public bool Directional;

	public bool SinglePassStereo;

	public bool Projector;

	[HideInInspector]
	public bool FullLast;

	[HideInInspector]
	public bool LowResLast;

	[HideInInspector]
	public bool HeightFogLast;

	[HideInInspector]
	public bool HeightFogOffLast;

	[HideInInspector]
	public bool NoiseLast;

	[HideInInspector]
	public bool NoiseOffLast;

	[HideInInspector]
	public bool TransparencyLast;

	[HideInInspector]
	public bool TransparencyOffLast;

	[HideInInspector]
	public bool DensityParticlesLast;

	[HideInInspector]
	public bool PointLast;

	[HideInInspector]
	public bool SpotLast;

	[HideInInspector]
	public bool DirectionalLast;

	[HideInInspector]
	public bool SinglePassStereoLast;

	[HideInInspector]
	public bool ProjectorLast;
}


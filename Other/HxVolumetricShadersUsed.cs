using System;
using UnityEngine;

// Token: 0x02000010 RID: 16
public class HxVolumetricShadersUsed : ScriptableObject
{
	// Token: 0x060000F3 RID: 243 RVA: 0x0000D808 File Offset: 0x0000BA08
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

	// Token: 0x04000182 RID: 386
	public HxVolumetricCamera.TransparencyQualities TransperencyQuality = HxVolumetricCamera.TransparencyQualities.Medium;

	// Token: 0x04000183 RID: 387
	public HxVolumetricCamera.DensityParticleQualities DensityParticleQuality = HxVolumetricCamera.DensityParticleQualities.High;

	// Token: 0x04000184 RID: 388
	[HideInInspector]
	public HxVolumetricCamera.DensityParticleQualities LastDensityParticleQuality = HxVolumetricCamera.DensityParticleQualities.High;

	// Token: 0x04000185 RID: 389
	[HideInInspector]
	public HxVolumetricCamera.TransparencyQualities LastTransperencyQuality = HxVolumetricCamera.TransparencyQualities.Medium;

	// Token: 0x04000186 RID: 390
	private static HxVolumetricShadersUsed instance;

	// Token: 0x04000187 RID: 391
	public bool Full;

	// Token: 0x04000188 RID: 392
	public bool LowRes;

	// Token: 0x04000189 RID: 393
	public bool HeightFog;

	// Token: 0x0400018A RID: 394
	public bool HeightFogOff;

	// Token: 0x0400018B RID: 395
	public bool Noise;

	// Token: 0x0400018C RID: 396
	public bool NoiseOff;

	// Token: 0x0400018D RID: 397
	public bool Transparency;

	// Token: 0x0400018E RID: 398
	public bool TransparencyOff;

	// Token: 0x0400018F RID: 399
	public bool DensityParticles;

	// Token: 0x04000190 RID: 400
	public bool Point;

	// Token: 0x04000191 RID: 401
	public bool Spot;

	// Token: 0x04000192 RID: 402
	public bool Directional;

	// Token: 0x04000193 RID: 403
	public bool SinglePassStereo;

	// Token: 0x04000194 RID: 404
	public bool Projector;

	// Token: 0x04000195 RID: 405
	[HideInInspector]
	public bool FullLast;

	// Token: 0x04000196 RID: 406
	[HideInInspector]
	public bool LowResLast;

	// Token: 0x04000197 RID: 407
	[HideInInspector]
	public bool HeightFogLast;

	// Token: 0x04000198 RID: 408
	[HideInInspector]
	public bool HeightFogOffLast;

	// Token: 0x04000199 RID: 409
	[HideInInspector]
	public bool NoiseLast;

	// Token: 0x0400019A RID: 410
	[HideInInspector]
	public bool NoiseOffLast;

	// Token: 0x0400019B RID: 411
	[HideInInspector]
	public bool TransparencyLast;

	// Token: 0x0400019C RID: 412
	[HideInInspector]
	public bool TransparencyOffLast;

	// Token: 0x0400019D RID: 413
	[HideInInspector]
	public bool DensityParticlesLast;

	// Token: 0x0400019E RID: 414
	[HideInInspector]
	public bool PointLast;

	// Token: 0x0400019F RID: 415
	[HideInInspector]
	public bool SpotLast;

	// Token: 0x040001A0 RID: 416
	[HideInInspector]
	public bool DirectionalLast;

	// Token: 0x040001A1 RID: 417
	[HideInInspector]
	public bool SinglePassStereoLast;

	// Token: 0x040001A2 RID: 418
	[HideInInspector]
	public bool ProjectorLast;
}

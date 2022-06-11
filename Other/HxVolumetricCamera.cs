using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

// Token: 0x0200000A RID: 10
[ExecuteInEditMode]
public class HxVolumetricCamera : MonoBehaviour
{
	// Token: 0x06000066 RID: 102 RVA: 0x0000456C File Offset: 0x0000276C
	private void SetUpRenderOrder()
	{
		if (this.callBackImageEffect != null)
		{
			if (this.TransparencySupport || this.RenderOrder == HxVolumetricCamera.hxRenderOrder.ImageEffectOpaque)
			{
				this.callBackImageEffect.enabled = false;
			}
			else
			{
				this.callBackImageEffect.enabled = true;
			}
		}
		if (this.callBackImageEffectOpaque != null)
		{
			if (this.TransparencySupport || this.RenderOrder == HxVolumetricCamera.hxRenderOrder.ImageEffectOpaque)
			{
				this.callBackImageEffectOpaque.enabled = true;
			}
			else
			{
				this.callBackImageEffectOpaque.enabled = false;
			}
		}
		if (this.callBackImageEffectOpaque == null && (this.TransparencySupport || this.RenderOrder == HxVolumetricCamera.hxRenderOrder.ImageEffectOpaque))
		{
			this.callBackImageEffectOpaque = base.gameObject.GetComponent<HxVolumetricImageEffectOpaque>();
			if (this.callBackImageEffectOpaque == null)
			{
				this.callBackImageEffectOpaque = base.gameObject.AddComponent<HxVolumetricImageEffectOpaque>();
				this.callBackImageEffectOpaque.RenderOrder = HxVolumetricCamera.hxRenderOrder.ImageEffectOpaque;
			}
		}
		if (this.callBackImageEffect == null && !this.TransparencySupport && this.RenderOrder != HxVolumetricCamera.hxRenderOrder.ImageEffectOpaque)
		{
			this.callBackImageEffect = base.gameObject.GetComponent<HxVolumetricImageEffect>();
			if (this.callBackImageEffect == null)
			{
				this.callBackImageEffect = base.gameObject.AddComponent<HxVolumetricImageEffect>();
				this.callBackImageEffect.RenderOrder = HxVolumetricCamera.hxRenderOrder.ImageEffect;
			}
		}
	}

	// Token: 0x06000067 RID: 103 RVA: 0x000046A4 File Offset: 0x000028A4
	public static Material GetDirectionalMaterial(int mid)
	{
		Material material;
		if (!HxVolumetricCamera.DirectionalMaterial.TryGetValue(mid, out material))
		{
			if (HxVolumetricCamera.directionalShader == null)
			{
				HxVolumetricCamera.directionalShader = Shader.Find("Hidden/HxVolumetricDirectionalLight");
			}
			HxVolumetricCamera.CreateShader(HxVolumetricCamera.directionalShader, mid, out material, false);
			HxVolumetricCamera.DirectionalMaterial.Add(mid, material);
		}
		return material;
	}

	// Token: 0x06000068 RID: 104 RVA: 0x000046F8 File Offset: 0x000028F8
	public static Material GetProjectorMaterial(int mid)
	{
		Material material;
		if (!HxVolumetricCamera.ProjectorMaterial.TryGetValue(mid, out material))
		{
			if (HxVolumetricCamera.ProjectorShader == null)
			{
				HxVolumetricCamera.ProjectorShader = Shader.Find("Hidden/HxVolumetricProjector");
			}
			HxVolumetricCamera.CreateShader(HxVolumetricCamera.ProjectorShader, mid, out material, false);
			HxVolumetricCamera.ProjectorMaterial.Add(mid, material);
		}
		return material;
	}

	// Token: 0x06000069 RID: 105 RVA: 0x0000474C File Offset: 0x0000294C
	public static Material GetSpotMaterial(int mid)
	{
		Material material;
		if (!HxVolumetricCamera.SpotMaterial.TryGetValue(mid, out material))
		{
			if (HxVolumetricCamera.spotShader == null)
			{
				HxVolumetricCamera.spotShader = Shader.Find("Hidden/HxVolumetricSpotLight");
			}
			HxVolumetricCamera.CreateShader(HxVolumetricCamera.spotShader, mid, out material, false);
			HxVolumetricCamera.SpotMaterial.Add(mid, material);
		}
		return material;
	}

	// Token: 0x0600006A RID: 106 RVA: 0x000047A0 File Offset: 0x000029A0
	public static Material GetPointMaterial(int mid)
	{
		Material material;
		if (!HxVolumetricCamera.PointMaterial.TryGetValue(mid, out material))
		{
			if (HxVolumetricCamera.pointShader == null)
			{
				HxVolumetricCamera.pointShader = Shader.Find("Hidden/HxVolumetricPointLight");
			}
			HxVolumetricCamera.CreateShader(HxVolumetricCamera.pointShader, mid, out material, true);
			HxVolumetricCamera.PointMaterial.Add(mid, material);
		}
		return material;
	}

	// Token: 0x0600006B RID: 107 RVA: 0x000047F3 File Offset: 0x000029F3
	public HxVolumetricCamera.TransparencyQualities compatibleTBuffer()
	{
		if (HxVolumetricCamera.TransparencyBufferDepth > HxVolumetricCamera.TransparencyQualities.Medium && SystemInfo.graphicsDeviceType != GraphicsDeviceType.Direct3D11 && SystemInfo.graphicsDeviceType != GraphicsDeviceType.Direct3D12 && SystemInfo.graphicsDeviceType != GraphicsDeviceType.PlayStation4)
		{
			return HxVolumetricCamera.TransparencyQualities.High;
		}
		return HxVolumetricCamera.TransparencyBufferDepth;
	}

	// Token: 0x0600006C RID: 108 RVA: 0x0000481E File Offset: 0x00002A1E
	private bool IsRenderBoth()
	{
		return this.Mycamera.stereoTargetEye == StereoTargetEyeMask.Both && Application.isPlaying && XRSettings.enabled && XRDevice.isPresent;
	}

	// Token: 0x0600006D RID: 109 RVA: 0x00004846 File Offset: 0x00002A46
	private HxVolumetricCamera.DensityParticleQualities compatibleDBuffer()
	{
		return HxVolumetricCamera.DensityBufferDepth;
	}

	// Token: 0x0600006E RID: 110 RVA: 0x0000484D File Offset: 0x00002A4D
	private void MyPreCull(Camera cam)
	{
		if (cam != HxVolumetricCamera.ActiveCamera)
		{
			this.ReleaseLightBuffers();
			this.SetUpRenderOrder();
		}
	}

	// Token: 0x0600006F RID: 111 RVA: 0x00004868 File Offset: 0x00002A68
	public bool renderDensityParticleCheck()
	{
		return this.ParticleDensityRenderCount > 0;
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00004874 File Offset: 0x00002A74
	private void WarmUp()
	{
		if (HxVolumetricCamera.CollectionAll == null)
		{
			UnityEngine.Object[] array = Resources.LoadAll("HxUsedShaderVariantCollection");
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] as ShaderVariantCollection != null)
				{
					HxVolumetricCamera.CollectionAll = (array[i] as ShaderVariantCollection);
					break;
				}
			}
			if (HxVolumetricCamera.CollectionAll != null)
			{
				HxVolumetricCamera.CollectionAll.WarmUp();
			}
		}
		if (HxVolumetricCamera.UsedShaderSettings == null)
		{
			HxVolumetricCamera.UsedShaderSettings = (HxVolumetricShadersUsed)Resources.Load("HxUsedShaders");
			if (HxVolumetricCamera.UsedShaderSettings != null)
			{
				HxVolumetricCamera.TransparencyBufferDepth = HxVolumetricCamera.UsedShaderSettings.LastTransperencyQuality;
				HxVolumetricCamera.DensityBufferDepth = HxVolumetricCamera.UsedShaderSettings.LastDensityParticleQuality;
			}
		}
	}

	// Token: 0x06000071 RID: 113 RVA: 0x0000492C File Offset: 0x00002B2C
	private void CreateShaderVariant(Shader source, int i, ref Material[] material, ref ShaderVariantCollection.ShaderVariant[] Variant, bool point = true)
	{
		HxVolumetricCamera.ShaderVariantList.Clear();
		int num = i;
		int num2 = 0;
		if (num >= 64)
		{
			material[i].EnableKeyword("FULL_ON");
			HxVolumetricCamera.ShaderVariantList.Add("FULL_ON");
			num -= 64;
			num2++;
		}
		if (num >= 32)
		{
			material[i].EnableKeyword("VTRANSPARENCY_ON");
			HxVolumetricCamera.ShaderVariantList.Add("VTRANSPARENCY_ON");
			num -= 32;
			num2++;
		}
		if (num >= 16)
		{
			material[i].EnableKeyword("DENSITYPARTICLES_ON");
			HxVolumetricCamera.ShaderVariantList.Add("DENSITYPARTICLES_ON");
			num -= 16;
			num2++;
		}
		if (num >= 8)
		{
			material[i].EnableKeyword("HEIGHTFOG_ON");
			HxVolumetricCamera.ShaderVariantList.Add("HEIGHTFOG_ON");
			num -= 8;
			num2++;
		}
		if (num >= 4)
		{
			material[i].EnableKeyword("NOISE_ON");
			HxVolumetricCamera.ShaderVariantList.Add("NOISE_ON");
			num -= 4;
			num2++;
		}
		if (num >= 2)
		{
			if (point)
			{
				material[i].EnableKeyword("POINT_COOKIE");
				HxVolumetricCamera.ShaderVariantList.Add("POINT_COOKIE");
				num2++;
			}
			num -= 2;
		}
		if (num >= 1)
		{
			num--;
		}
		else
		{
			material[i].EnableKeyword("SHADOWS_OFF");
			HxVolumetricCamera.ShaderVariantList.Add("SHADOWS_OFF");
			num2++;
		}
		if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Shadowmap))
		{
			material[i].EnableKeyword("SHADOWS_NATIVE");
		}
		string[] array = new string[num2];
		HxVolumetricCamera.ShaderVariantList.CopyTo(array);
		Variant[i] = new ShaderVariantCollection.ShaderVariant(source, PassType.Normal, array);
	}

	// Token: 0x06000072 RID: 114 RVA: 0x00004AAC File Offset: 0x00002CAC
	private static void CreateShader(Shader source, int i, out Material outMaterial, bool point = true)
	{
		outMaterial = new Material(source);
		outMaterial.hideFlags = HideFlags.DontSave;
		bool flag = false;
		int num = i;
		int num2 = 0;
		if (num >= 64)
		{
			outMaterial.EnableKeyword("FULL_ON");
			num -= 64;
			num2++;
		}
		if (num >= 32)
		{
			outMaterial.EnableKeyword("VTRANSPARENCY_ON");
			num -= 32;
			num2++;
		}
		if (num >= 16)
		{
			outMaterial.EnableKeyword("DENSITYPARTICLES_ON");
			num -= 16;
			num2++;
		}
		if (num >= 8)
		{
			outMaterial.EnableKeyword("HEIGHTFOG_ON");
			num -= 8;
			num2++;
		}
		if (num >= 4)
		{
			outMaterial.EnableKeyword("NOISE_ON");
			num -= 4;
			num2++;
		}
		if (num >= 2)
		{
			if (point)
			{
				outMaterial.EnableKeyword("POINT_COOKIE");
				num2++;
			}
			num -= 2;
		}
		if (num >= 1)
		{
			num--;
			flag = true;
		}
		else
		{
			outMaterial.EnableKeyword("SHADOWS_OFF");
			num2++;
		}
		if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Shadowmap) && flag)
		{
			outMaterial.EnableKeyword("SHADOWS_NATIVE");
		}
	}

	// Token: 0x06000073 RID: 115 RVA: 0x00004B9C File Offset: 0x00002D9C
	private void CreatePIDs()
	{
		if (this.NoiseTexture3D == null)
		{
			this.Create3DNoiseTexture();
		}
		bool flag = false;
		if (!HxVolumetricCamera.PIDCreated)
		{
			flag = true;
			HxVolumetricCamera.PIDCreated = true;
			HxVolumetricCamera.VolumetricTexturePID = Shader.PropertyToID("VolumetricTexture");
			HxVolumetricCamera.ScaledDepthTexturePID = Shader.PropertyToID("VolumetricDepth");
			HxVolumetricCamera.ShadowMapTexturePID = Shader.PropertyToID("_ShadowMapTexture");
			HxVolumetricCamera.DepthThresholdPID = Shader.PropertyToID("DepthThreshold");
			HxVolumetricCamera.BlurDepthFalloffPID = Shader.PropertyToID("BlurDepthFalloff");
			HxVolumetricCamera.VolumeScalePID = Shader.PropertyToID("VolumeScale");
			HxVolumetricCamera.InverseViewMatrixPID = Shader.PropertyToID("InverseViewMatrix");
			HxVolumetricCamera.InverseProjectionMatrixPID = Shader.PropertyToID("InverseProjectionMatrix");
			HxVolumetricCamera.InverseProjectionMatrix2PID = Shader.PropertyToID("InverseProjectionMatrix2");
			HxVolumetricCamera.NoiseOffsetPID = Shader.PropertyToID("NoiseOffset");
			HxVolumetricCamera.ShadowDistancePID = Shader.PropertyToID("ShadowDistance");
			for (int i = 0; i < this.EnumBufferDepthLength; i++)
			{
				HxVolumetricCamera.VolumetricDensityPID[i] = Shader.PropertyToID("VolumetricDensityTexture" + i);
				HxVolumetricCamera.VolumetricTransparencyPID[i] = Shader.PropertyToID("VolumetricTransparencyTexture" + i);
			}
			HxVolumetricLight.CreatePID();
		}
		if (HxVolumetricCamera.Tile5x5 == null)
		{
			HxVolumetricCamera.CreateTileTexture();
		}
		if (HxVolumetricCamera.DownSampleMaterial == null)
		{
			HxVolumetricCamera.DownSampleMaterial = new Material(Shader.Find("Hidden/HxVolumetricDownscaleDepth"));
			HxVolumetricCamera.DownSampleMaterial.hideFlags = HideFlags.DontSave;
		}
		if (HxVolumetricCamera.TransparencyBlurMaterial == null)
		{
			HxVolumetricCamera.TransparencyBlurMaterial = new Material(Shader.Find("Hidden/HxTransparencyBlur"));
			HxVolumetricCamera.TransparencyBlurMaterial.hideFlags = HideFlags.DontSave;
		}
		if (HxVolumetricCamera.DensityMaterial == null)
		{
			HxVolumetricCamera.DensityMaterial = new Material(Shader.Find("Hidden/HxDensityShader"));
			HxVolumetricCamera.DensityMaterial.hideFlags = HideFlags.DontSave;
		}
		if (HxVolumetricCamera.VolumeBlurMaterial == null)
		{
			HxVolumetricCamera.VolumeBlurMaterial = new Material(Shader.Find("Hidden/HxVolumetricDepthAwareBlur"));
			HxVolumetricCamera.VolumeBlurMaterial.hideFlags = HideFlags.DontSave;
		}
		if (HxVolumetricCamera.ApplyMaterial == null)
		{
			HxVolumetricCamera.ApplyMaterial = new Material(Shader.Find("Hidden/HxVolumetricApply"));
			HxVolumetricCamera.ApplyMaterial.hideFlags = HideFlags.DontSave;
		}
		if (HxVolumetricCamera.ApplyDirectMaterial == null)
		{
			HxVolumetricCamera.ApplyDirectMaterial = new Material(Shader.Find("Hidden/HxVolumetricApplyDirect"));
			HxVolumetricCamera.ApplyDirectMaterial.hideFlags = HideFlags.DontSave;
		}
		if (HxVolumetricCamera.ApplyQueueMaterial == null)
		{
			HxVolumetricCamera.ApplyQueueMaterial = new Material(Shader.Find("Hidden/HxVolumetricApplyRenderQueue"));
			HxVolumetricCamera.ApplyQueueMaterial.hideFlags = HideFlags.DontSave;
		}
		if (HxVolumetricCamera.QuadMesh == null)
		{
			HxVolumetricCamera.QuadMesh = HxVolumetricCamera.CreateQuad();
			HxVolumetricCamera.QuadMesh.hideFlags = HideFlags.DontSave;
		}
		if (HxVolumetricCamera.BoxMesh == null)
		{
			HxVolumetricCamera.BoxMesh = HxVolumetricCamera.CreateBox();
		}
		if (HxVolumetricCamera.SphereMesh == null)
		{
			HxVolumetricCamera.SphereMesh = HxVolumetricCamera.CreateIcoSphere(1, 0.56f);
			HxVolumetricCamera.SphereMesh.hideFlags = HideFlags.DontSave;
		}
		if (HxVolumetricCamera.SpotLightMesh == null)
		{
			HxVolumetricCamera.SpotLightMesh = HxVolumetricCamera.CreateCone(4, false);
			HxVolumetricCamera.SpotLightMesh.hideFlags = HideFlags.DontSave;
		}
		if (HxVolumetricCamera.OrthoProjectorMesh == null)
		{
			HxVolumetricCamera.OrthoProjectorMesh = HxVolumetricCamera.CreateOrtho(4, false);
			HxVolumetricCamera.OrthoProjectorMesh.hideFlags = HideFlags.DontSave;
		}
		if (HxVolumetricCamera.directionalShader == null)
		{
			HxVolumetricCamera.directionalShader = Shader.Find("Hidden/HxVolumetricDirectionalLight");
		}
		if (HxVolumetricCamera.pointShader == null)
		{
			HxVolumetricCamera.pointShader = Shader.Find("Hidden/HxVolumetricPointLight");
		}
		if (HxVolumetricCamera.spotShader == null)
		{
			HxVolumetricCamera.spotShader = Shader.Find("Hidden/HxVolumetricSpotLight");
		}
		if (flag)
		{
			this.WarmUp();
		}
		if (HxVolumetricCamera.ShadowMaterial == null)
		{
			HxVolumetricCamera.ShadowMaterial = new Material(Shader.Find("Hidden/HxShadowCasterFix"));
			HxVolumetricCamera.ShadowMaterial.hideFlags = HideFlags.DontSave;
		}
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00004F48 File Offset: 0x00003148
	public static bool ActiveFull()
	{
		return HxVolumetricCamera.Active.resolution == HxVolumetricCamera.Resolution.full;
	}

	// Token: 0x06000075 RID: 117 RVA: 0x00002E1A File Offset: 0x0000101A
	private void DefineFull()
	{
	}

	// Token: 0x06000076 RID: 118 RVA: 0x00004F57 File Offset: 0x00003157
	private static void UpdateLight(HxOctreeNode<HxVolumetricLight>.NodeObject node, Vector3 boundsMin, Vector3 boundsMax)
	{
		HxVolumetricCamera.LightOctree.Move(node, boundsMin, boundsMax);
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00004F66 File Offset: 0x00003166
	public static HxOctreeNode<HxVolumetricLight>.NodeObject AddLightOctree(HxVolumetricLight light, Vector3 boundsMin, Vector3 boundsMax)
	{
		if (HxVolumetricCamera.LightOctree == null)
		{
			HxVolumetricCamera.LightOctree = new HxOctree<HxVolumetricLight>(Vector3.zero, 100f, 0.1f, 10f);
		}
		return HxVolumetricCamera.LightOctree.Add(light, boundsMin, boundsMax);
	}

	// Token: 0x06000078 RID: 120 RVA: 0x00004F9A File Offset: 0x0000319A
	public static HxOctreeNode<HxVolumetricParticleSystem>.NodeObject AddParticleOctree(HxVolumetricParticleSystem particle, Vector3 boundsMin, Vector3 boundsMax)
	{
		if (HxVolumetricCamera.ParticleOctree == null)
		{
			HxVolumetricCamera.ParticleOctree = new HxOctree<HxVolumetricParticleSystem>(Vector3.zero, 100f, 0.1f, 10f);
		}
		return HxVolumetricCamera.ParticleOctree.Add(particle, boundsMin, boundsMax);
	}

	// Token: 0x06000079 RID: 121 RVA: 0x00004FCE File Offset: 0x000031CE
	public static void RemoveLightOctree(HxVolumetricLight light)
	{
		if (HxVolumetricCamera.LightOctree != null)
		{
			HxVolumetricCamera.LightOctree.Remove(light);
		}
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00004FE3 File Offset: 0x000031E3
	public static void RemoveParticletOctree(HxVolumetricParticleSystem Particle)
	{
		if (HxVolumetricCamera.ParticleOctree != null)
		{
			HxVolumetricCamera.ParticleOctree.Remove(Particle);
		}
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00004FF8 File Offset: 0x000031F8
	private void OnApplicationQuit()
	{
		HxVolumetricCamera.PIDCreated = false;
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00005000 File Offset: 0x00003200
	public Camera GetCamera()
	{
		if (this.Mycamera == null)
		{
			this.Mycamera = base.GetComponent<Camera>();
		}
		return this.Mycamera;
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x0600007D RID: 125 RVA: 0x00005022 File Offset: 0x00003222
	// (set) Token: 0x0600007E RID: 126 RVA: 0x00005061 File Offset: 0x00003261
	[HideInInspector]
	public Texture2D SpotLightCookie
	{
		get
		{
			if (HxVolumetricCamera._SpotLightCookie == null)
			{
				HxVolumetricCamera._SpotLightCookie = (Texture2D)Resources.Load("LightSoftCookie");
				if (HxVolumetricCamera._SpotLightCookie == null)
				{
					Debug.Log("couldnt find default cookie");
				}
			}
			return HxVolumetricCamera._SpotLightCookie;
		}
		set
		{
			HxVolumetricCamera._SpotLightCookie = value;
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x0600007F RID: 127 RVA: 0x00005069 File Offset: 0x00003269
	// (set) Token: 0x06000080 RID: 128 RVA: 0x000050A8 File Offset: 0x000032A8
	[HideInInspector]
	public Texture2D LightFalloff
	{
		get
		{
			if (HxVolumetricCamera._LightFalloff == null)
			{
				HxVolumetricCamera._LightFalloff = (Texture2D)Resources.Load("HxFallOff");
				if (HxVolumetricCamera._LightFalloff == null)
				{
					Debug.Log("couldnt find default Falloff");
				}
			}
			return HxVolumetricCamera._LightFalloff;
		}
		set
		{
			HxVolumetricCamera._LightFalloff = value;
		}
	}

	
	// Token: 0x06000083 RID: 131 RVA: 0x00005278 File Offset: 0x00003478
	private void RenderParticles()
	{
		this.ParticleDensityRenderCount = 0;
		if (this.ParticleDensitySupport)
		{
			
			HxVolumetricCamera.ConstructPlanes(this.Mycamera, 0f, Mathf.Max(this.MaxDirectionalRayDistance, this.MaxLightDistanceUsed));
			this.FindActiveParticleSystems();
			this.ParticleDensityRenderCount += this.RenderSlices();
			if (this.ParticleDensityRenderCount > 0)
			{
				Shader.EnableKeyword("DENSITYPARTICLES_ON");
			
				for (int i = 0; i < (int)(this.compatibleDBuffer() + 1); i++)
				{
					this.BufferRender.SetGlobalTexture(HxVolumetricCamera.VolumetricDensityPID[i], HxVolumetricCamera.VolumetricDensity[(int)this.compatibleDBuffer()][i]);
				}
			}
			else
			{
				Shader.DisableKeyword("DENSITYPARTICLES_ON");
			}
		}
		else
		{
			Shader.DisableKeyword("DENSITYPARTICLES_ON");
		}
		if (this.TransparencySupport)
		{
			Shader.EnableKeyword("VTRANSPARENCY_ON");
			for (int j = 0; j < (int)(this.compatibleTBuffer() + 1); j++)
			{
				this.BufferRender.SetGlobalTexture(HxVolumetricCamera.VolumetricTransparencyPID[j], HxVolumetricCamera.VolumetricTransparencyI[(int)this.compatibleTBuffer()][j]);
			}
			return;
		}
		Shader.DisableKeyword("VTRANSPARENCY_ON");
	}

	// Token: 0x06000084 RID: 132 RVA: 0x0000545B File Offset: 0x0000365B
	private void OnPostRender()
	{
		Shader.DisableKeyword("VTRANSPARENCY_ON");
	}

	// Token: 0x06000085 RID: 133 RVA: 0x00005468 File Offset: 0x00003668
	private int RenderSlices()
	{
		this.BufferRender.SetRenderTarget(HxVolumetricCamera.VolumetricDensity[(int)this.compatibleDBuffer()], HxVolumetricCamera.VolumetricDensity[(int)this.compatibleDBuffer()][0]);
		this.BufferRender.ClearRenderTarget(false, true, new Color(0.5f, 0.5f, 0.5f, 0.5f));
	
		int num = 0;
		for (int i = 0; i < HxVolumetricCamera.ActiveParticleSystems.Count; i++)
		{
			if (HxVolumetricCamera.ActiveParticleSystems[i].BlendMode == HxVolumetricParticleSystem.ParticleBlendMode.Max)
			{
				this.BufferRender.SetGlobalFloat("particleDensity", HxVolumetricCamera.ActiveParticleSystems[i].DensityStrength);
				HxVolumetricCamera.DensityMaterial.CopyPropertiesFromMaterial(HxVolumetricCamera.ActiveParticleSystems[i].particleRenderer.sharedMaterial);
				this.BufferRender.DrawRenderer(HxVolumetricCamera.ActiveParticleSystems[i].particleRenderer, HxVolumetricCamera.DensityMaterial, 0, (int)HxVolumetricCamera.ActiveParticleSystems[i].BlendMode);
				num++;
			}
		}
		for (int j = 0; j < HxVolumetricCamera.ActiveParticleSystems.Count; j++)
		{
			if (HxVolumetricCamera.ActiveParticleSystems[j].BlendMode == HxVolumetricParticleSystem.ParticleBlendMode.Add)
			{
				this.BufferRender.SetGlobalFloat("particleDensity", HxVolumetricCamera.ActiveParticleSystems[j].DensityStrength);
				HxVolumetricCamera.DensityMaterial.CopyPropertiesFromMaterial(HxVolumetricCamera.ActiveParticleSystems[j].particleRenderer.sharedMaterial);
				this.BufferRender.DrawRenderer(HxVolumetricCamera.ActiveParticleSystems[j].particleRenderer, HxVolumetricCamera.DensityMaterial, 0, (int)HxVolumetricCamera.ActiveParticleSystems[j].BlendMode);
				num++;
			}
		}
		for (int k = 0; k < HxVolumetricCamera.ActiveParticleSystems.Count; k++)
		{
			if (HxVolumetricCamera.ActiveParticleSystems[k].BlendMode == HxVolumetricParticleSystem.ParticleBlendMode.Min)
			{
				this.BufferRender.SetGlobalFloat("particleDensity", HxVolumetricCamera.ActiveParticleSystems[k].DensityStrength);
				HxVolumetricCamera.DensityMaterial.CopyPropertiesFromMaterial(HxVolumetricCamera.ActiveParticleSystems[k].particleRenderer.sharedMaterial);
				this.BufferRender.DrawRenderer(HxVolumetricCamera.ActiveParticleSystems[k].particleRenderer, HxVolumetricCamera.DensityMaterial, 0, (int)HxVolumetricCamera.ActiveParticleSystems[k].BlendMode);
				num++;
			}
		}
		for (int l = 0; l < HxVolumetricCamera.ActiveParticleSystems.Count; l++)
		{
			if (HxVolumetricCamera.ActiveParticleSystems[l].BlendMode == HxVolumetricParticleSystem.ParticleBlendMode.Sub)
			{
				this.BufferRender.SetGlobalFloat("particleDensity", HxVolumetricCamera.ActiveParticleSystems[l].DensityStrength);
				HxVolumetricCamera.DensityMaterial.CopyPropertiesFromMaterial(HxVolumetricCamera.ActiveParticleSystems[l].particleRenderer.sharedMaterial);
				this.BufferRender.DrawRenderer(HxVolumetricCamera.ActiveParticleSystems[l].particleRenderer, HxVolumetricCamera.DensityMaterial, 0, (int)HxVolumetricCamera.ActiveParticleSystems[l].BlendMode);
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000086 RID: 134 RVA: 0x0000578E File Offset: 0x0000398E
	private int GetCamPixelHeight()
	{
		if (this.Mycamera.stereoTargetEye != StereoTargetEyeMask.None && Application.isPlaying && XRSettings.enabled && XRDevice.isPresent)
		{
			return XRSettings.eyeTextureHeight;
		}
		return this.Mycamera.pixelHeight;
	}

	// Token: 0x06000087 RID: 135 RVA: 0x000057C4 File Offset: 0x000039C4
	private int GetCamPixelWidth()
	{
		if (this.Mycamera.stereoTargetEye != StereoTargetEyeMask.None && Application.isPlaying && XRSettings.enabled && XRDevice.isPresent)
		{
			return XRSettings.eyeTextureWidth + ((this.Mycamera.stereoTargetEye == StereoTargetEyeMask.Both) ? (XRSettings.eyeTextureWidth + Mathf.CeilToInt(48f * XRSettings.eyeTextureResolutionScale)) : 0);
		}
		return this.Mycamera.pixelWidth;
	}

	// Token: 0x06000088 RID: 136 RVA: 0x0000582C File Offset: 0x00003A2C
	private void CreateTempTextures()
	{
		int width = Mathf.CeilToInt((float)this.GetCamPixelWidth() * HxVolumetricCamera.ResolutionScale[(int)this.resolution]);
		int height = Mathf.CeilToInt((float)this.GetCamPixelHeight() * HxVolumetricCamera.ResolutionScale[(int)this.resolution]);
		if (this.resolution != HxVolumetricCamera.Resolution.full && HxVolumetricCamera.FullBlurRT == null)
		{
			HxVolumetricCamera.FullBlurRT = RenderTexture.GetTemporary(this.GetCamPixelWidth(), this.GetCamPixelHeight(), 16, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
			HxVolumetricCamera.FullBlurRTID = new RenderTargetIdentifier(HxVolumetricCamera.FullBlurRT);
			HxVolumetricCamera.FullBlurRT.filterMode = FilterMode.Bilinear;
			HxVolumetricCamera.FullBlurRT.hideFlags = HideFlags.DontSave;
		}
		if (HxVolumetricCamera.VolumetricTexture == null)
		{
			HxVolumetricCamera.VolumetricTexture = RenderTexture.GetTemporary(width, height, 16, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
			HxVolumetricCamera.VolumetricTexture.filterMode = FilterMode.Bilinear;
			HxVolumetricCamera.VolumetricTexture.hideFlags = HideFlags.DontSave;
			HxVolumetricCamera.VolumetricTextureRTID = new RenderTargetIdentifier(HxVolumetricCamera.VolumetricTexture);
		}
		if (HxVolumetricCamera.ScaledDepthTexture[(int)this.resolution] == null)
		{
			HxVolumetricCamera.ScaledDepthTexture[(int)this.resolution] = RenderTexture.GetTemporary(width, height, 24, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
			HxVolumetricCamera.ScaledDepthTexture[(int)this.resolution].filterMode = FilterMode.Point;
			HxVolumetricCamera.ScaledDepthTextureRTID[(int)this.resolution] = new RenderTargetIdentifier(HxVolumetricCamera.ScaledDepthTexture[(int)this.resolution]);
			HxVolumetricCamera.ScaledDepthTexture[(int)this.resolution].hideFlags = HideFlags.DontSave;
		}
		if (this.TransparencySupport)
		{
			for (int i = 0; i < this.EnumBufferDepthLength; i++)
			{
				HxVolumetricCamera.VolumetricTransparency[i][0] = HxVolumetricCamera.VolumetricTextureRTID;
			}
			for (int j = 0; j < (int)(this.compatibleTBuffer() + 1); j++)
			{
				if (HxVolumetricCamera.VolumetricTransparencyTextures[j] == null)
				{
					HxVolumetricCamera.VolumetricTransparencyTextures[j] = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
					HxVolumetricCamera.VolumetricTransparencyTextures[j].hideFlags = HideFlags.DontSave;
					HxVolumetricCamera.VolumetricTransparencyTextures[j].filterMode = FilterMode.Bilinear;
					RenderTargetIdentifier renderTargetIdentifier = new RenderTargetIdentifier(HxVolumetricCamera.VolumetricTransparencyTextures[j]);
					for (int k = Mathf.Max(j, 0); k < this.EnumBufferDepthLength; k++)
					{
						HxVolumetricCamera.VolumetricTransparency[k][j + 1] = renderTargetIdentifier;
						HxVolumetricCamera.VolumetricTransparencyI[k][j] = renderTargetIdentifier;
					}
				}
			}
		}
		if (HxVolumetricCamera.downScaledBlurRT == null && (this.blurCount > 0 || ((this.BlurTransparency > 0 || this.MapToLDR) && this.TransparencySupport)) && this.resolution != HxVolumetricCamera.Resolution.full)
		{
			HxVolumetricCamera.downScaledBlurRT = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
			HxVolumetricCamera.downScaledBlurRT.filterMode = FilterMode.Bilinear;
			HxVolumetricCamera.downScaledBlurRTID = new RenderTargetIdentifier(HxVolumetricCamera.downScaledBlurRT);
			HxVolumetricCamera.downScaledBlurRT.hideFlags = HideFlags.DontSave;
		}
		if (HxVolumetricCamera.FullBlurRT2 == null && ((this.resolution != HxVolumetricCamera.Resolution.full && this.UpSampledblurCount > 0) || (this.resolution == HxVolumetricCamera.Resolution.full && (this.blurCount > 0 || ((this.BlurTransparency > 0 || this.MapToLDR) && this.TransparencySupport) || this.TemporalSampling)) || this.MapToLDR))
		{
			HxVolumetricCamera.FullBlurRT2 = RenderTexture.GetTemporary(this.GetCamPixelWidth(), this.GetCamPixelHeight(), 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
			HxVolumetricCamera.FullBlurRT2.hideFlags = HideFlags.DontSave;
			HxVolumetricCamera.FullBlurRT2.filterMode = FilterMode.Bilinear;
			HxVolumetricCamera.FullBlurRT2ID = new RenderTargetIdentifier(HxVolumetricCamera.FullBlurRT2);
			if (this.resolution != HxVolumetricCamera.Resolution.full)
			{
				HxVolumetricCamera.VolumetricUpsampledBlurTextures[0] = HxVolumetricCamera.FullBlurRTID;
				HxVolumetricCamera.VolumetricUpsampledBlurTextures[1] = HxVolumetricCamera.FullBlurRT2ID;
			}
		}
		width = Mathf.CeilToInt((float)this.GetCamPixelWidth() * HxVolumetricCamera.ResolutionScale[Mathf.Max((int)this.resolution, (int)this.densityResolution)]);
		height = Mathf.CeilToInt((float)this.GetCamPixelHeight() * HxVolumetricCamera.ResolutionScale[Mathf.Max((int)this.resolution, (int)this.densityResolution)]);
		if (this.ParticleDensitySupport)
		{
			for (int l = 0; l < (int)(this.compatibleDBuffer() + 1); l++)
			{
				if (HxVolumetricCamera.VolumetricDensityTextures[l] == null)
				{
					HxVolumetricCamera.VolumetricDensityTextures[l] = RenderTexture.GetTemporary(width, height, (l == 0) ? 16 : 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
					HxVolumetricCamera.VolumetricDensityTextures[l].hideFlags = HideFlags.DontSave;
					HxVolumetricCamera.VolumetricDensityTextures[l].filterMode = FilterMode.Bilinear;
					RenderTargetIdentifier renderTargetIdentifier2 = new RenderTargetIdentifier(HxVolumetricCamera.VolumetricDensityTextures[l]);
					for (int m = Mathf.Max(l, 0); m < this.EnumBufferDepthLength; m++)
					{
						HxVolumetricCamera.VolumetricDensity[m][l] = renderTargetIdentifier2;
					}
				}
			}
		}
	}

	// Token: 0x06000089 RID: 137 RVA: 0x00005C68 File Offset: 0x00003E68
	public static void ConstructPlanes(Camera cam, float near, float far)
	{
		Vector3 position = cam.transform.position;
		Vector3 forward = cam.transform.forward;
		Vector3 right = cam.transform.right;
		Vector3 up = cam.transform.up;
		Vector3 a = position + forward * far;
		Vector3 a2 = position + forward * near;
		float num = Mathf.Tan(cam.fieldOfView * 0.017453292f / 2f) * far;
		float d = num * cam.aspect;
		float d2 = Mathf.Tan(cam.fieldOfView * 0.017453292f / 2f) * near;
		float d3 = num * cam.aspect;
		Vector3 vector = a + up * num - right * d;
		Vector3 vector2 = a + up * num + right * d;
		Vector3 vector3 = a - up * num - right * d;
		Vector3 vector4 = a - up * num + right * d;
		Vector3 a3 = a2 + up * d2 - right * d3;
		Vector3 b = a2 + up * d2 + right * d3;
		Vector3 c = a2 - up * d2 - right * d3;
		HxVolumetricCamera.CameraPlanes[0] = new Plane(vector3, vector, vector2);
		HxVolumetricCamera.CameraPlanes[1] = new Plane(a3, b, c);
		HxVolumetricCamera.CameraPlanes[2] = new Plane(position, vector, vector3);
		HxVolumetricCamera.CameraPlanes[3] = new Plane(position, vector4, vector2);
		HxVolumetricCamera.CameraPlanes[4] = new Plane(position, vector3, vector4);
		HxVolumetricCamera.CameraPlanes[5] = new Plane(position, vector2, vector);
		HxVolumetricCamera.MinBounds = new Vector3(Mathf.Min(vector.x, Mathf.Min(vector2.x, Mathf.Min(vector3.x, Mathf.Min(vector4.x, position.x)))), Mathf.Min(vector.y, Mathf.Min(vector2.y, Mathf.Min(vector3.y, Mathf.Min(vector4.y, position.y)))), Mathf.Min(vector.z, Mathf.Min(vector2.z, Mathf.Min(vector3.z, Mathf.Min(vector4.z, position.z)))));
		HxVolumetricCamera.MaxBounds = new Vector3(Mathf.Max(vector.x, Mathf.Max(vector2.x, Mathf.Max(vector3.x, Mathf.Max(vector4.x, position.x)))), Mathf.Max(vector.y, Mathf.Max(vector2.y, Mathf.Max(vector3.y, Mathf.Max(vector4.y, position.y)))), Mathf.Max(vector.z, Mathf.Max(vector2.z, Mathf.Max(vector3.z, Mathf.Max(vector4.z, position.z)))));
	}

	// Token: 0x0600008A RID: 138 RVA: 0x00005FB4 File Offset: 0x000041B4
	private void FindActiveLights()
	{
		HxVolumetricCamera.ActiveLights.Clear();
		HxVolumetricCamera.ActiveVolumes.Clear();
		if (HxVolumetricCamera.LightOctree != null)
		{
			HxVolumetricCamera.LightOctree.GetObjectsBoundsPlane(ref HxVolumetricCamera.CameraPlanes, HxVolumetricCamera.MinBounds, HxVolumetricCamera.MaxBounds, HxVolumetricCamera.ActiveLights);
		}
		for (int i = 0; i < HxVolumetricCamera.ActiveDirectionalLights.Count; i++)
		{
			HxVolumetricCamera.ActiveLights.Add(HxVolumetricCamera.ActiveDirectionalLights[i]);
		}
		if (HxDensityVolume.DensityOctree != null)
		{
			HxDensityVolume.DensityOctree.GetObjectsBoundsPlane(ref HxVolumetricCamera.CameraPlanes, HxVolumetricCamera.MinBounds, HxVolumetricCamera.MaxBounds, HxVolumetricCamera.ActiveVolumes);
			HxVolumetricCamera.ActiveVolumes.Sort(delegate(HxDensityVolume a, HxDensityVolume b)
			{
				int blendMode = (int)a.BlendMode;
				return blendMode.CompareTo((int)b.BlendMode);
			});
		}
	}

	// Token: 0x0600008B RID: 139 RVA: 0x00006072 File Offset: 0x00004272
	private void FindActiveParticleSystems()
	{
		HxVolumetricCamera.ActiveParticleSystems.Clear();
		if (HxVolumetricCamera.ParticleOctree != null)
		{
			HxVolumetricCamera.ParticleOctree.GetObjectsBoundsPlane(ref HxVolumetricCamera.CameraPlanes, HxVolumetricCamera.MinBounds, HxVolumetricCamera.MaxBounds, HxVolumetricCamera.ActiveParticleSystems);
		}
	}

	// Token: 0x0600008C RID: 140 RVA: 0x000060A4 File Offset: 0x000042A4
	public void Update()
	{
		this.OffsetUpdated = false;
		if (this.Mycamera == null)
		{
			this.Mycamera = base.GetComponent<Camera>();
		}
		if (this.Mycamera != null)
		{
			if (HxVolumetricCamera.BoxMesh == null)
			{
				HxVolumetricCamera.BoxMesh = HxVolumetricCamera.CreateBox();
			}
			if (HxVolumetricCamera.ShadowMaterial == null)
			{
				HxVolumetricCamera.ShadowMaterial = new Material(Shader.Find("Hidden/HxShadowCasterFix"));
				HxVolumetricCamera.ShadowMaterial.hideFlags = HideFlags.DontSave;
			}
			if (this.ShadowFix)
			{
				Graphics.DrawMesh(HxVolumetricCamera.BoxMesh, Matrix4x4.TRS(base.transform.position, Quaternion.identity, new Vector3(Mathf.Max(this.MaxDirectionalRayDistance, this.MaxLightDistance), Mathf.Max(this.MaxDirectionalRayDistance, this.MaxLightDistance), Mathf.Max(this.MaxDirectionalRayDistance, this.MaxLightDistance)) * 2f), HxVolumetricCamera.ShadowMaterial, 0);
				return;
			}
		}
		else
		{
			base.enabled = false;
		}
	}

	// Token: 0x0600008D RID: 141 RVA: 0x0000619E File Offset: 0x0000439E
	private void Start()
	{
		this.FinalizeBufferDirty = true;
		this.SetupBufferDirty = true;
		if (this.preCullEventAdded)
		{
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(this.MyPreCull));
			this.preCullEventAdded = true;
		}
	}

	// Token: 0x0600008E RID: 142 RVA: 0x0000619E File Offset: 0x0000439E
	private void OnEnable()
	{
		this.FinalizeBufferDirty = true;
		this.SetupBufferDirty = true;
		if (this.preCullEventAdded)
		{
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(this.MyPreCull));
			this.preCullEventAdded = true;
		}
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00002E1A File Offset: 0x0000101A
	private void CreateApplyBuffer()
	{
	}

	// Token: 0x06000090 RID: 144 RVA: 0x000061E0 File Offset: 0x000043E0
	private void CreateSetupBuffer()
	{
		if (this.SetupBufferDirty && this.SetupBufferAdded)
		{
			this.Mycamera.RemoveCommandBuffer(this.lastSetup, this.BufferSetup);
			this.SetupBufferAdded = false;
			this.SetupBufferDirty = false;
		}
		if (!this.SetupBufferAdded)
		{
			if (this.BufferSetup == null)
			{
				this.BufferSetup = new CommandBuffer();
				this.BufferSetup.name = "VolumetricSetup";
			}
			else
			{
				this.BufferSetup.Clear();
			}
			if (this.TransparencySupport)
			{
				this.BufferSetup.SetRenderTarget(HxVolumetricCamera.VolumetricTransparencyI[(int)this.compatibleTBuffer()], HxVolumetricCamera.ScaledDepthTextureRTID[(int)HxVolumetricCamera.Active.resolution]);
				this.BufferSetup.ClearRenderTarget(false, true, new Color32(0, 0, 0, 0));
			}
			this.BufferSetup.SetRenderTarget(HxVolumetricCamera.ScaledDepthTextureRTID[(int)this.resolution]);
			this.BufferSetup.DrawMesh(HxVolumetricCamera.QuadMesh, Matrix4x4.identity, HxVolumetricCamera.DownSampleMaterial, 0, (int)this.resolution);
			this.BufferSetup.SetGlobalTexture(HxVolumetricCamera.ScaledDepthTexturePID, HxVolumetricCamera.ScaledDepthTextureRTID[(int)this.resolution]);
			this.lastSetup = this.SetupEvent;
			this.Mycamera.AddCommandBuffer(this.SetupEvent, this.BufferSetup);
			this.SetupBufferAdded = true;
		}
	}

	// Token: 0x06000091 RID: 145 RVA: 0x00006334 File Offset: 0x00004534
	private bool CheckBufferDirty()
	{
		bool flag = true;
		if (this.TemporalSampling && this.TemporalFirst)
		{
			flag = true;
		}
		if (this.lastDownDepthFalloff != this.DownsampledBlurDepthFalloff)
		{
			flag = true;
			this.lastDownDepthFalloff = this.DownsampledBlurDepthFalloff;
		}
		if (this.lastDepthFalloff != this.BlurDepthFalloff)
		{
			flag = true;
			this.lastDepthFalloff = this.BlurDepthFalloff;
		}
		if (this.lastDensityRes != (int)this.densityResolution)
		{
			flag = true;
			this.lastDensityRes = (int)this.densityResolution;
		}
		if (this.lastTransparency != (this.TransparencySupport ? 1 : 0))
		{
			flag = true;
			this.lastTransparency = (this.TransparencySupport ? 1 : 0);
		}
		if (this.lastDensity != (this.ParticleDensitySupport ? 1 : 0))
		{
			flag = true;
			this.lastDensity = (this.ParticleDensitySupport ? 1 : 0);
		}
		if (this.lastGaussian != (this.GaussianWeights ? 1 : 0))
		{
			flag = true;
			this.lastGaussian = (this.GaussianWeights ? 1 : 0);
		}
		if (this.lastPath != (int)this.Mycamera.actualRenderingPath)
		{
			flag = true;
			this.lastPath = (int)this.Mycamera.actualRenderingPath;
		}
		if (this.lastBanding != (this.RemoveColorBanding ? 1 : 0))
		{
			flag = true;
			this.lastBanding = (this.RemoveColorBanding ? 1 : 0);
		}
		if (this.GetCamPixelHeight() != this.lastH)
		{
			flag = true;
			this.lastH = this.GetCamPixelHeight();
		}
		if (this.GetCamPixelWidth() != this.lastW)
		{
			flag = true;
			this.lastW = this.GetCamPixelWidth();
		}
		if (this.lastLDR != (this.MapToLDR ? 1 : 0))
		{
			flag = true;
			this.lastLDR = (this.MapToLDR ? 1 : 0);
		}
		if (this.lastupSampleBlurCount != this.UpSampledblurCount)
		{
			this.lastupSampleBlurCount = this.UpSampledblurCount;
			flag = true;
		}
		if (this.lastBlurCount != this.blurCount)
		{
			this.lastBlurCount = this.blurCount;
			flag = true;
		}
		if (HxVolumetricCamera.lastRes != (int)this.resolution)
		{
			HxVolumetricCamera.lastRes = (int)this.resolution;
			flag = true;
		}
		if (Application.isPlaying)
		{
			if (!this.LastPlaying)
			{
				flag = true;
			}
			this.LastPlaying = true;
		}
		else
		{
			if (this.LastPlaying)
			{
				flag = true;
			}
			this.LastPlaying = false;
		}
		if (flag)
		{
			this.FinalizeBufferDirty = true;
			this.SetupBufferDirty = true;
			return true;
		}
		return false;
	}

	// Token: 0x06000092 RID: 146 RVA: 0x00006560 File Offset: 0x00004760
	private void CreateFinalizeBuffer()
	{
		if (!this.FinalizeBufferAdded)
		{
			bool flag = true;
			bool flag2 = true;
			if ((this.BlurTransparency > 0 || this.MapToLDR) && this.TransparencySupport)
			{
				int num = (int)this.compatibleTBuffer();
				int num2 = Mathf.Max(this.BlurTransparency, 1);
				for (int i = 0; i < num + 1; i++)
				{
					for (int j = 0; j < num2; j++)
					{
						this.BufferFinalize.SetRenderTarget((this.resolution == HxVolumetricCamera.Resolution.full) ? HxVolumetricCamera.FullBlurRT2ID : HxVolumetricCamera.downScaledBlurRTID);
						this.BufferFinalize.SetGlobalTexture("_MainTex", HxVolumetricCamera.VolumetricTransparencyI[num][i]);
						this.BufferFinalize.DrawMesh(HxVolumetricCamera.QuadMesh, Matrix4x4.identity, HxVolumetricCamera.TransparencyBlurMaterial, 0, 0);
						this.BufferFinalize.SetRenderTarget(HxVolumetricCamera.VolumetricTransparencyI[num][i]);
						this.BufferFinalize.SetGlobalTexture("_MainTex", (this.resolution == HxVolumetricCamera.Resolution.full) ? HxVolumetricCamera.FullBlurRT2ID : HxVolumetricCamera.downScaledBlurRTID);
						this.BufferFinalize.DrawMesh(HxVolumetricCamera.QuadMesh, Matrix4x4.identity, HxVolumetricCamera.TransparencyBlurMaterial, 0, (this.MapToLDR && j == num2 - 1) ? 2 : 1);
					}
				}
			}
			if (this.blurCount > 0 && this.resolution != HxVolumetricCamera.Resolution.full)
			{
				this.BufferFinalize.SetGlobalFloat(HxVolumetricCamera.BlurDepthFalloffPID, this.DownsampledBlurDepthFalloff);
				for (int k = 0; k < this.blurCount; k++)
				{
					if (flag)
					{
						this.BufferFinalize.SetRenderTarget(HxVolumetricCamera.downScaledBlurRTID);
						this.BufferFinalize.SetGlobalTexture("_MainTex", HxVolumetricCamera.VolumetricTextureRTID);
						this.BufferFinalize.DrawMesh(HxVolumetricCamera.QuadMesh, Matrix4x4.identity, HxVolumetricCamera.VolumeBlurMaterial, 0, this.GaussianWeights ? 2 : 0);
					}
					else
					{
						this.BufferFinalize.SetRenderTarget(HxVolumetricCamera.VolumetricTextureRTID);
						this.BufferFinalize.SetGlobalTexture("_MainTex", HxVolumetricCamera.downScaledBlurRTID);
						this.BufferFinalize.DrawMesh(HxVolumetricCamera.QuadMesh, Matrix4x4.identity, HxVolumetricCamera.VolumeBlurMaterial, 0, this.GaussianWeights ? 2 : 0);
					}
					flag = !flag;
				}
			}
			if (this.resolution != HxVolumetricCamera.Resolution.full)
			{
				if (this.TemporalSampling)
				{
					this.BufferFinalize.SetGlobalTexture("hxLastVolumetric", this.TemporalTextureRTID);
					this.BufferFinalize.SetGlobalVector("hxTemporalSettings", new Vector4(this.LuminanceFeedback, this.MaxFeedback, 0f, 0f));
				}
				if (this.UpSampledblurCount == 0)
				{
					this.BufferFinalize.SetRenderTarget(HxVolumetricCamera.FullBlurRT);
					this.BufferFinalize.SetGlobalTexture("_MainTex", flag ? HxVolumetricCamera.VolumetricTextureRTID : HxVolumetricCamera.downScaledBlurRTID);
					this.BufferFinalize.DrawMesh(HxVolumetricCamera.QuadMesh, Matrix4x4.identity, HxVolumetricCamera.ApplyDirectMaterial, 0, (this.TemporalSampling && !this.TemporalFirst) ? 6 : 0);
				}
				else
				{
					this.BufferFinalize.SetGlobalTexture("_MainTex", flag ? HxVolumetricCamera.VolumetricTextureRTID : HxVolumetricCamera.downScaledBlurRTID);
					this.BufferFinalize.SetRenderTarget(HxVolumetricCamera.VolumetricUpsampledBlurTextures, HxVolumetricCamera.FullBlurRT);
					this.BufferFinalize.DrawMesh(HxVolumetricCamera.QuadMesh, Matrix4x4.identity, HxVolumetricCamera.ApplyDirectMaterial, 0, (this.TemporalSampling && !this.TemporalFirst) ? 7 : 5);
				}
				if (this.UpSampledblurCount > 0)
				{
					this.BufferFinalize.SetGlobalFloat(HxVolumetricCamera.BlurDepthFalloffPID, this.BlurDepthFalloff);
					if (this.UpSampledblurCount % 2 != 0)
					{
						flag2 = false;
					}
					for (int l = 0; l < this.UpSampledblurCount; l++)
					{
						if (flag2)
						{
							this.BufferFinalize.SetRenderTarget(HxVolumetricCamera.FullBlurRT2ID);
							this.BufferFinalize.SetGlobalTexture("_MainTex", HxVolumetricCamera.FullBlurRTID);
							this.BufferFinalize.DrawMesh(HxVolumetricCamera.QuadMesh, Matrix4x4.identity, HxVolumetricCamera.VolumeBlurMaterial, 0, 1);
						}
						else
						{
							this.BufferFinalize.SetRenderTarget(HxVolumetricCamera.FullBlurRTID);
							this.BufferFinalize.SetGlobalTexture("_MainTex", HxVolumetricCamera.FullBlurRT2ID);
							this.BufferFinalize.DrawMesh(HxVolumetricCamera.QuadMesh, Matrix4x4.identity, HxVolumetricCamera.VolumeBlurMaterial, 0, 1);
						}
						flag2 = !flag2;
					}
				}
				if (this.MapToLDR)
				{
					this.BufferFinalize.SetGlobalTexture(HxVolumetricCamera.VolumetricTexturePID, HxVolumetricCamera.FullBlurRTID);
					this.BufferFinalize.SetRenderTarget(this.TemporalTexture);
					this.BufferFinalize.DrawMesh(HxVolumetricCamera.QuadMesh, Matrix4x4.identity, HxVolumetricCamera.ApplyDirectMaterial, 0, 8);
					this.BufferFinalize.SetRenderTarget(HxVolumetricCamera.FullBlurRT2);
					this.BufferFinalize.SetGlobalTexture("_MainTex", HxVolumetricCamera.FullBlurRTID);
					this.BufferFinalize.DrawMesh(HxVolumetricCamera.QuadMesh, Matrix4x4.identity, HxVolumetricCamera.TransparencyBlurMaterial, 0, 3);
					this.BufferFinalize.SetGlobalTexture(HxVolumetricCamera.VolumetricTexturePID, HxVolumetricCamera.FullBlurRT2);
				}
				else
				{
					this.BufferFinalize.SetGlobalTexture(HxVolumetricCamera.VolumetricTexturePID, HxVolumetricCamera.FullBlurRT);
				}
			}
			else if (this.blurCount > 0 || this.TemporalSampling)
			{
				if (this.TemporalSampling)
				{
					this.BufferFinalize.SetGlobalTexture("hxLastVolumetric", this.TemporalTextureRTID);
					this.BufferFinalize.SetGlobalVector("hxTemporalSettings", new Vector4(this.LuminanceFeedback, this.MaxFeedback, 0f, 0f));
				}
				this.BufferFinalize.SetGlobalFloat(HxVolumetricCamera.BlurDepthFalloffPID, this.BlurDepthFalloff);
				flag2 = true;
				for (int m = 0; m < this.blurCount; m++)
				{
					if (flag2)
					{
						this.BufferFinalize.SetRenderTarget(HxVolumetricCamera.FullBlurRT2ID);
						this.BufferFinalize.SetGlobalTexture("_MainTex", HxVolumetricCamera.VolumetricTextureRTID);
						this.BufferFinalize.DrawMesh(HxVolumetricCamera.QuadMesh, Matrix4x4.identity, HxVolumetricCamera.VolumeBlurMaterial, 0, this.GaussianWeights ? 5 : 4);
					}
					else
					{
						this.BufferFinalize.SetRenderTarget(HxVolumetricCamera.VolumetricTextureRTID);
						this.BufferFinalize.SetGlobalTexture("_MainTex", HxVolumetricCamera.FullBlurRT2ID);
						this.BufferFinalize.DrawMesh(HxVolumetricCamera.QuadMesh, Matrix4x4.identity, HxVolumetricCamera.VolumeBlurMaterial, 0, this.GaussianWeights ? 5 : 4);
					}
					flag2 = !flag2;
				}
				if (!flag2)
				{
					if (this.MapToLDR)
					{
						if (this.TemporalSampling && !this.TemporalFirst)
						{
							this.BufferFinalize.SetRenderTarget(HxVolumetricCamera.VolumetricTextureRTID);
							this.BufferFinalize.SetGlobalTexture("_MainTex", HxVolumetricCamera.FullBlurRT2);
							this.BufferFinalize.DrawMesh(HxVolumetricCamera.QuadMesh, Matrix4x4.identity, HxVolumetricCamera.TransparencyBlurMaterial, 0, 5);
							this.BufferFinalize.SetGlobalTexture(HxVolumetricCamera.VolumetricTexturePID, HxVolumetricCamera.VolumetricTextureRTID);
							this.TemporalFirst = false;
							this.BufferFinalize.SetRenderTarget(this.TemporalTexture);
							this.BufferFinalize.DrawMesh(HxVolumetricCamera.QuadMesh, Matrix4x4.identity, HxVolumetricCamera.ApplyDirectMaterial, 0, 8);
							this.BufferFinalize.SetRenderTarget(HxVolumetricCamera.FullBlurRT2);
							this.BufferFinalize.SetGlobalTexture("_MainTex", HxVolumetricCamera.VolumetricTextureRTID);
							this.BufferFinalize.DrawMesh(HxVolumetricCamera.QuadMesh, Matrix4x4.identity, HxVolumetricCamera.TransparencyBlurMaterial, 0, (this.TemporalSampling && !this.TemporalFirst) ? 4 : 3);
							this.BufferFinalize.SetGlobalTexture(HxVolumetricCamera.VolumetricTexturePID, HxVolumetricCamera.FullBlurRT2);
						}
						else
						{
							this.BufferFinalize.SetRenderTarget(HxVolumetricCamera.VolumetricTextureRTID);
							this.BufferFinalize.SetGlobalTexture("_MainTex", HxVolumetricCamera.FullBlurRT2ID);
							this.BufferFinalize.DrawMesh(HxVolumetricCamera.QuadMesh, Matrix4x4.identity, HxVolumetricCamera.TransparencyBlurMaterial, 0, 3);
							this.BufferFinalize.SetGlobalTexture(HxVolumetricCamera.VolumetricTexturePID, HxVolumetricCamera.VolumetricTexture);
						}
					}
					else if (this.TemporalSampling && !this.TemporalFirst)
					{
						this.BufferFinalize.SetRenderTarget(HxVolumetricCamera.VolumetricTextureRTID);
						this.BufferFinalize.SetGlobalTexture("_MainTex", HxVolumetricCamera.FullBlurRT2);
						this.BufferFinalize.DrawMesh(HxVolumetricCamera.QuadMesh, Matrix4x4.identity, HxVolumetricCamera.TransparencyBlurMaterial, 0, 5);
						this.BufferFinalize.SetGlobalTexture(HxVolumetricCamera.VolumetricTexturePID, HxVolumetricCamera.VolumetricTextureRTID);
					}
					else
					{
						this.BufferFinalize.SetGlobalTexture(HxVolumetricCamera.VolumetricTexturePID, HxVolumetricCamera.FullBlurRT2);
					}
				}
				else if (this.MapToLDR)
				{
					if (this.TemporalSampling && !this.TemporalFirst)
					{
						this.BufferFinalize.SetRenderTarget(HxVolumetricCamera.FullBlurRT2);
						this.BufferFinalize.SetGlobalTexture("_MainTex", HxVolumetricCamera.VolumetricTextureRTID);
						this.BufferFinalize.DrawMesh(HxVolumetricCamera.QuadMesh, Matrix4x4.identity, HxVolumetricCamera.TransparencyBlurMaterial, 0, 5);
						this.BufferFinalize.SetGlobalTexture(HxVolumetricCamera.VolumetricTexturePID, HxVolumetricCamera.FullBlurRT2);
						this.BufferFinalize.SetRenderTarget(this.TemporalTexture);
						this.BufferFinalize.DrawMesh(HxVolumetricCamera.QuadMesh, Matrix4x4.identity, HxVolumetricCamera.ApplyDirectMaterial, 0, 8);
						this.BufferFinalize.SetRenderTarget(HxVolumetricCamera.VolumetricTextureRTID);
						this.BufferFinalize.SetGlobalTexture("_MainTex", HxVolumetricCamera.FullBlurRT2ID);
						this.BufferFinalize.DrawMesh(HxVolumetricCamera.QuadMesh, Matrix4x4.identity, HxVolumetricCamera.TransparencyBlurMaterial, 0, 3);
						this.BufferFinalize.SetGlobalTexture(HxVolumetricCamera.VolumetricTexturePID, HxVolumetricCamera.VolumetricTexture);
					}
					else
					{
						this.BufferFinalize.SetRenderTarget(HxVolumetricCamera.FullBlurRT2);
						this.BufferFinalize.SetGlobalTexture("_MainTex", HxVolumetricCamera.VolumetricTextureRTID);
						this.BufferFinalize.DrawMesh(HxVolumetricCamera.QuadMesh, Matrix4x4.identity, HxVolumetricCamera.TransparencyBlurMaterial, 0, (this.TemporalSampling && !this.TemporalFirst) ? 4 : 3);
						this.BufferFinalize.SetGlobalTexture(HxVolumetricCamera.VolumetricTexturePID, HxVolumetricCamera.FullBlurRT2);
					}
				}
				else if (this.TemporalSampling && !this.TemporalFirst)
				{
					this.BufferFinalize.SetRenderTarget(HxVolumetricCamera.FullBlurRT2);
					this.BufferFinalize.SetGlobalTexture("_MainTex", HxVolumetricCamera.VolumetricTextureRTID);
					this.BufferFinalize.DrawMesh(HxVolumetricCamera.QuadMesh, Matrix4x4.identity, HxVolumetricCamera.TransparencyBlurMaterial, 0, 5);
					this.BufferFinalize.SetGlobalTexture(HxVolumetricCamera.VolumetricTexturePID, HxVolumetricCamera.FullBlurRT2);
				}
				else
				{
					this.BufferFinalize.SetGlobalTexture(HxVolumetricCamera.VolumetricTexturePID, HxVolumetricCamera.VolumetricTextureRTID);
				}
			}
			else if (this.MapToLDR)
			{
				this.BufferFinalize.SetRenderTarget(HxVolumetricCamera.FullBlurRT2);
				this.BufferFinalize.SetGlobalTexture("_MainTex", HxVolumetricCamera.VolumetricTextureRTID);
				this.BufferFinalize.DrawMesh(HxVolumetricCamera.QuadMesh, Matrix4x4.identity, HxVolumetricCamera.TransparencyBlurMaterial, 0, 3);
				this.BufferFinalize.SetGlobalTexture(HxVolumetricCamera.VolumetricTexturePID, HxVolumetricCamera.FullBlurRT2);
			}
			else
			{
				this.BufferFinalize.SetGlobalTexture(HxVolumetricCamera.VolumetricTexturePID, HxVolumetricCamera.VolumetricTextureRTID);
			}
			if (this.TemporalSampling)
			{
				if (this.MapToLDR)
				{
					this.TemporalFirst = false;
				}
				else
				{
					this.TemporalFirst = false;
					this.BufferFinalize.SetRenderTarget(this.TemporalTexture);
					this.BufferFinalize.DrawMesh(HxVolumetricCamera.QuadMesh, Matrix4x4.identity, HxVolumetricCamera.ApplyDirectMaterial, 0, 8);
				}
			}
			else
			{
				this.TemporalFirst = true;
			}
			this.lastFinalize = this.FinalizeEvent;
			this.lastRender = this.RenderEvent;
			this.lastLightRender = this.LightRenderEvent;
			this.lastFinalize = this.FinalizeEvent;
			this.Mycamera.AddCommandBuffer(this.FinalizeEvent, this.BufferFinalize);
			this.FinalizeBufferAdded = true;
		}
	}

	// Token: 0x06000093 RID: 147 RVA: 0x000070F4 File Offset: 0x000052F4
	private void BuildBuffer()
	{
		if (this.BuffersBuilt)
		{
			if (this.BufferRender != null)
			{
				this.Mycamera.RemoveCommandBuffer(this.lastRender, this.BufferRender);
			}
			this.BuffersBuilt = false;
		}
		this.CreatePIDs();
		this.CalculateEvent();
		this.DefineFull();
		this.CheckTemporalTextures();
		if (this.CheckBufferDirty())
		{
			HxVolumetricCamera.ReleaseTempTextures();
		}
		this.CreateTempTextures();
		HxVolumetricCamera.Active = this;
		HxVolumetricCamera.ActiveCamera = this.Mycamera;
		if (this.FinalizeBufferDirty && this.FinalizeBufferAdded)
		{
			this.Mycamera.RemoveCommandBuffer(this.lastFinalize, this.BufferFinalize);
			this.FinalizeBufferAdded = false;
			this.FinalizeBufferDirty = false;
		}
		if (!this.FinalizeBufferAdded)
		{
			if (this.BufferFinalize == null)
			{
				this.BufferFinalize = new CommandBuffer();
				this.BufferFinalize.name = "VolumetricFinalize";
			}
			else
			{
				this.BufferFinalize.Clear();
			}
		}
		this.CreateSetupBuffer();
		this.CreateApplyBuffer();
		if (this.resolution == HxVolumetricCamera.Resolution.full)
		{
			this.FullUsed = true;
		}
		else
		{
			this.LowResUsed = true;
		}
		this.CurrentTint = new Vector3(((QualitySettings.activeColorSpace == ColorSpace.Gamma) ? this.TintColor : this.TintColor.linear).r, ((QualitySettings.activeColorSpace == ColorSpace.Gamma) ? this.TintColor : this.TintColor.linear).g, ((QualitySettings.activeColorSpace == ColorSpace.Gamma) ? this.TintColor : this.TintColor.linear).b) * this.TintIntensity;
		this.CurrentTintEdge = new Vector3(((QualitySettings.activeColorSpace == ColorSpace.Gamma) ? this.TintColor2 : this.TintColor2.linear).r, ((QualitySettings.activeColorSpace == ColorSpace.Gamma) ? this.TintColor2 : this.TintColor2.linear).g, ((QualitySettings.activeColorSpace == ColorSpace.Gamma) ? this.TintColor2 : this.TintColor2.linear).b) * this.TintIntensity;
		if (this.dirty)
		{
			if (this.BufferRender == null)
			{
				this.BufferRender = new CommandBuffer();
				this.BufferRender.name = "VolumetricRender";
			}
			else
			{
				this.BufferRender.Clear();
			}
			if (this.TemporalSampling)
			{
				Matrix4x4 rhs = this.CurrentView;
				Matrix4x4 gpuprojectionMatrix = GL.GetGPUProjectionMatrix(this.CurrentProj, false);
				this.LastMatrixVP = gpuprojectionMatrix * rhs;
				this.BufferRender.SetGlobalMatrix("hxLastVP", this.LastMatrixVP);
				if (this.IsRenderBoth())
				{
					rhs = this.CurrentView2;
					gpuprojectionMatrix = GL.GetGPUProjectionMatrix(this.CurrentProj2, false);
					this.LastMatrixVP = gpuprojectionMatrix * rhs;
					this.BufferRender.SetGlobalMatrix("hxLastVP2", this.LastMatrixVP);
				}
			}
			if (this.Mycamera.stereoTargetEye != StereoTargetEyeMask.None && Application.isPlaying && XRSettings.enabled && XRDevice.isPresent)
			{
				Camera.StereoscopicEye stereoscopicEye = Camera.StereoscopicEye.Right;
				if (this.IsRenderBoth())
				{
					this.SinglePassStereoUsed = true;
				}
				else if (this.Mycamera.stereoTargetEye == StereoTargetEyeMask.Right)
				{
					stereoscopicEye = Camera.StereoscopicEye.Right;
				}
				else
				{
					stereoscopicEye = Camera.StereoscopicEye.Left;
				}
				this.CurrentProj = this.Mycamera.GetStereoProjectionMatrix(stereoscopicEye);
				this.CurrentView = this.Mycamera.GetStereoViewMatrix(stereoscopicEye);
				this.CurrentInvers = this.CurrentProj.inverse;
				if (this.IsRenderBoth())
				{
					this.CurrentProj2 = this.Mycamera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);
					this.CurrentView2 = this.Mycamera.GetStereoViewMatrix(Camera.StereoscopicEye.Left);
				}
				if (this.IsRenderBoth())
				{
					Matrix4x4 worldToCameraMatrix = this.Mycamera.worldToCameraMatrix;
					Matrix4x4 worldToCameraMatrix2 = this.Mycamera.worldToCameraMatrix;
					ref Matrix4x4 ptr = ref worldToCameraMatrix;
					ptr[12] = ptr[12] + this.Mycamera.stereoSeparation / 2f;
					ptr = ref worldToCameraMatrix2;
					ptr[12] = ptr[12] - this.Mycamera.stereoSeparation / 2f;
					this.BufferRender.SetGlobalMatrix("hxCameraToWorld", worldToCameraMatrix.inverse);
					this.BufferRender.SetGlobalMatrix("hxCameraToWorld2", worldToCameraMatrix2.inverse);
				}
				else
				{
					Matrix4x4 worldToCameraMatrix3 = this.Mycamera.worldToCameraMatrix;
					ref Matrix4x4 ptr = ref worldToCameraMatrix3;
					ptr[12] = ptr[12] + this.Mycamera.stereoSeparation / 2f * (float)((stereoscopicEye == Camera.StereoscopicEye.Left) ? -1 : 1);
					this.BufferRender.SetGlobalMatrix("hxCameraToWorld", worldToCameraMatrix3.inverse);
				}
			}
			else
			{
				this.CurrentView = this.Mycamera.worldToCameraMatrix;
				this.BufferRender.SetGlobalMatrix("hxCameraToWorld", this.Mycamera.cameraToWorldMatrix);
				this.CurrentProj = this.Mycamera.projectionMatrix;
				this.CurrentInvers = this.Mycamera.projectionMatrix.inverse;
			}
			Matrix4x4 gpuprojectionMatrix2 = GL.GetGPUProjectionMatrix(this.CurrentProj, true);
			this.MatrixVP = gpuprojectionMatrix2 * this.CurrentView;
			this.MatrixV = this.CurrentView;
			Matrix4x4 currentView = this.CurrentView;
			Matrix4x4 inverse = (GL.GetGPUProjectionMatrix(this.CurrentProj, false) * currentView).inverse;
			this.BufferRender.SetGlobalMatrix("_InvViewProj", inverse);
			HxVolumetricCamera.BlitScale.z = HxVolumetricCamera.ActiveCamera.nearClipPlane + 1f;
			HxVolumetricCamera.BlitScale.y = (HxVolumetricCamera.ActiveCamera.nearClipPlane + 1f) * Mathf.Tan(0.017453292f * HxVolumetricCamera.ActiveCamera.fieldOfView * 0.51f);
			HxVolumetricCamera.BlitScale.x = HxVolumetricCamera.BlitScale.y * HxVolumetricCamera.ActiveCamera.aspect;
			HxVolumetricCamera.BlitMatrix = Matrix4x4.TRS(HxVolumetricCamera.Active.transform.position, HxVolumetricCamera.Active.transform.rotation, HxVolumetricCamera.BlitScale);
			HxVolumetricCamera.BlitMatrixMVP = HxVolumetricCamera.Active.MatrixVP * HxVolumetricCamera.BlitMatrix;
			HxVolumetricCamera.BlitMatrixMV = HxVolumetricCamera.Active.MatrixV * HxVolumetricCamera.BlitMatrix;
			if (this.TemporalSampling)
			{
				this.currentDitherOffset += this.DitherSpeed;
				if (this.currentDitherOffset > 1f)
				{
					this.currentDitherOffset -= 1f;
				}
				this.BufferRender.SetGlobalFloat("hxRayOffset", this.currentDitherOffset);
			}
			this.BufferRender.SetGlobalMatrix(HxVolumetricLight.VolumetricMVPPID, HxVolumetricCamera.BlitMatrixMVP);
			this.BufferRender.SetGlobalMatrix(HxVolumetricLight.VolumetricMVPID, HxVolumetricCamera.BlitMatrixMV);
			Matrix4x4 gpuprojectionMatrix3 = GL.GetGPUProjectionMatrix(this.Mycamera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left), true);
			Matrix4x4 stereoViewMatrix = this.Mycamera.GetStereoViewMatrix(Camera.StereoscopicEye.Left);
			this.BufferRender.SetGlobalMatrix(HxVolumetricLight.VolumetricMVP2PID, gpuprojectionMatrix3 * stereoViewMatrix * HxVolumetricCamera.BlitMatrix);
			this.BufferRender.SetGlobalMatrix(HxVolumetricCamera.InverseProjectionMatrix2PID, this.Mycamera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left).inverse);
			this.BufferRender.SetGlobalMatrix("InverseProjectionMatrix1", this.Mycamera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right).inverse);
			this.BufferRender.SetGlobalMatrix("hxInverseP1", GL.GetGPUProjectionMatrix(this.Mycamera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left), false).inverse);
			this.BufferRender.SetGlobalMatrix("hxInverseP2", GL.GetGPUProjectionMatrix(this.Mycamera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right), false).inverse);
			this.RenderParticles();
			this.BufferRender.SetRenderTarget(HxVolumetricCamera.VolumetricTextureRTID, HxVolumetricCamera.ScaledDepthTextureRTID[(int)HxVolumetricCamera.Active.resolution]);
			this.BufferRender.ClearRenderTarget(false, true, new Color(0f, 0f, 0f, 0f));
			this.BufferRender.SetGlobalFloat(HxVolumetricCamera.DepthThresholdPID, this.DepthThreshold);
			this.BufferRender.SetGlobalVector("CameraFoward", base.transform.forward);
			this.BufferRender.SetGlobalFloat(HxVolumetricCamera.BlurDepthFalloffPID, this.BlurDepthFalloff);
			this.BufferRender.SetGlobalFloat(HxVolumetricCamera.VolumeScalePID, HxVolumetricCamera.ResolutionScale[(int)this.resolution]);
			this.BufferRender.SetGlobalMatrix(HxVolumetricCamera.InverseViewMatrixPID, this.Mycamera.cameraToWorldMatrix);
			this.BufferRender.SetGlobalMatrix(HxVolumetricCamera.InverseProjectionMatrixPID, this.CurrentInvers);
			if (!this.OffsetUpdated)
			{
				this.OffsetUpdated = true;
				this.Offset += this.NoiseVelocity * Time.deltaTime;
			}
			this.BufferRender.SetGlobalVector(HxVolumetricCamera.NoiseOffsetPID, this.Offset);
			this.BufferRender.SetGlobalFloat(HxVolumetricCamera.ShadowDistancePID, QualitySettings.shadowDistance);
			this.CreateLightbuffers();
			this.CreateFinalizeBuffer();
			this.BuffersBuilt = true;
			this.Mycamera.AddCommandBuffer(this.RenderEvent, this.BufferRender);
			this.Mycamera.AddCommandBuffer(this.LightRenderEvent, this.BufferRenderLights);
		}
	}

	// Token: 0x06000094 RID: 148 RVA: 0x000079CC File Offset: 0x00005BCC
	private void OnDestroy()
	{
		if (this.TemporalTexture != null)
		{
			RenderTexture.ReleaseTemporary(this.TemporalTexture);
			this.TemporalTexture = null;
		}
		if (!this.preCullEventAdded)
		{
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(this.MyPreCull));
			this.preCullEventAdded = false;
		}
		if (HxVolumetricCamera.Active == this)
		{
			HxVolumetricCamera.Active.ReleaseLightBuffers();
			HxVolumetricCamera.ReleaseTempTextures();
		}
		if (this.BuffersBuilt)
		{
			if (this.BufferRenderLights != null && this.LightBufferAdded)
			{
				this.Mycamera.RemoveCommandBuffer(this.lastLightRender, this.BufferRenderLights);
				this.LightBufferAdded = false;
			}
			if (this.BufferSetup != null && this.SetupBufferAdded)
			{
				this.Mycamera.RemoveCommandBuffer(this.lastSetup, this.BufferSetup);
				this.SetupBufferAdded = false;
			}
			if (this.BufferRender != null)
			{
				this.Mycamera.RemoveCommandBuffer(this.lastRender, this.BufferRender);
			}
			if (this.BufferFinalize != null && this.FinalizeBufferAdded)
			{
				this.Mycamera.RemoveCommandBuffer(this.lastFinalize, this.BufferFinalize);
				this.FinalizeBufferAdded = false;
			}
			this.BuffersBuilt = false;
		}
		this.SaveUsedShaderVarience();
		if (this.callBackImageEffect != null)
		{
			this.callBackImageEffect.enabled = false;
		}
		if (this.callBackImageEffectOpaque != null)
		{
			this.callBackImageEffectOpaque.enabled = false;
		}
	}

	// Token: 0x06000095 RID: 149 RVA: 0x00002E1A File Offset: 0x0000101A
	private void SaveUsedShaderVarience()
	{
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00007B3C File Offset: 0x00005D3C
	private void OnDisable()
	{
		if (this.TemporalTexture != null)
		{
			RenderTexture.ReleaseTemporary(this.TemporalTexture);
			this.TemporalTexture = null;
			this.TemporalFirst = true;
		}
		if (!this.preCullEventAdded)
		{
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(this.MyPreCull));
			this.preCullEventAdded = false;
		}
		if (HxVolumetricCamera.Active == this)
		{
			HxVolumetricCamera.Active.ReleaseLightBuffers();
			HxVolumetricCamera.ReleaseTempTextures();
		}
		if (this.BuffersBuilt)
		{
			if (this.BufferRenderLights != null && this.LightBufferAdded)
			{
				this.Mycamera.RemoveCommandBuffer(this.lastLightRender, this.BufferRenderLights);
				this.LightBufferAdded = false;
			}
			if (this.BufferSetup != null && this.SetupBufferAdded)
			{
				this.Mycamera.RemoveCommandBuffer(this.lastSetup, this.BufferSetup);
				this.SetupBufferAdded = false;
			}
			if (this.BufferRender != null)
			{
				this.Mycamera.RemoveCommandBuffer(this.lastRender, this.BufferRender);
			}
			if (this.BufferFinalize != null && this.FinalizeBufferAdded)
			{
				this.Mycamera.RemoveCommandBuffer(this.lastFinalize, this.BufferFinalize);
				this.FinalizeBufferAdded = false;
			}
			this.BuffersBuilt = false;
		}
		if (this.callBackImageEffect != null)
		{
			this.callBackImageEffect.enabled = false;
		}
		if (this.callBackImageEffectOpaque != null)
		{
			this.callBackImageEffectOpaque.enabled = false;
		}
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00007CAC File Offset: 0x00005EAC
	private void CalculateEvent()
	{
		switch (this.Mycamera.actualRenderingPath)
		{
		case RenderingPath.Forward:
			if (this.Mycamera.depthTextureMode == DepthTextureMode.None)
			{
				this.Mycamera.depthTextureMode = DepthTextureMode.Depth;
			}
			if (this.Mycamera.depthTextureMode == DepthTextureMode.Depth || this.Mycamera.depthTextureMode == (DepthTextureMode.Depth | DepthTextureMode.MotionVectors))
			{
				this.RenderEvent = CameraEvent.BeforeDepthTexture;
				this.SetupEvent = CameraEvent.AfterDepthTexture;
			}
			else
			{
				this.RenderEvent = CameraEvent.BeforeDepthNormalsTexture;
				this.SetupEvent = CameraEvent.AfterDepthNormalsTexture;
			}
			this.FinalizeEvent = CameraEvent.AfterForwardOpaque;
			this.LightRenderEvent = CameraEvent.BeforeForwardOpaque;
			return;
		case RenderingPath.DeferredLighting:
			this.SetupEvent = CameraEvent.BeforeLighting;
			this.RenderEvent = CameraEvent.BeforeLighting;
			this.LightRenderEvent = CameraEvent.BeforeLighting;
			this.FinalizeEvent = CameraEvent.AfterLighting;
			return;
		case RenderingPath.DeferredShading:
			this.SetupEvent = CameraEvent.BeforeLighting;
			this.RenderEvent = CameraEvent.BeforeLighting;
			this.LightRenderEvent = CameraEvent.BeforeLighting;
			this.FinalizeEvent = CameraEvent.AfterLighting;
			return;
		default:
			return;
		}
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00007D77 File Offset: 0x00005F77
	public void EventOnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, HxVolumetricCamera.ApplyMaterial, ((QualitySettings.activeColorSpace == ColorSpace.Linear) ? 1 : 2) + (this.RemoveColorBanding ? 0 : 2));
	}

	// Token: 0x06000099 RID: 153 RVA: 0x00007D9E File Offset: 0x00005F9E
	private int ScalePass()
	{
		if (this.resolution == HxVolumetricCamera.Resolution.half)
		{
			return 0;
		}
		if (this.resolution == HxVolumetricCamera.Resolution.quarter)
		{
			return 1;
		}
		return 2;
	}

	// Token: 0x0600009A RID: 154 RVA: 0x00007DB8 File Offset: 0x00005FB8
	private void DownSampledFullBlur(RenderTexture mainColor, RenderBuffer NewColor, RenderBuffer depth, int pass)
	{
		Graphics.SetRenderTarget(NewColor, depth);
		HxVolumetricCamera.VolumeBlurMaterial.SetTexture("_MainTex", mainColor);
		GL.PushMatrix();
		HxVolumetricCamera.VolumeBlurMaterial.SetPass(pass);
		GL.LoadOrtho();
		GL.Begin(7);
		GL.Color(Color.red);
		GL.Vertex3(0f, 0f, 0f);
		GL.Vertex3(1f, 0f, 0f);
		GL.Vertex3(1f, 1f, 0f);
		GL.Vertex3(0f, 1f, 0f);
		GL.End();
		GL.PopMatrix();
	}

	// Token: 0x0600009B RID: 155 RVA: 0x00007E60 File Offset: 0x00006060
	private void CheckTemporalTextures()
	{
		if (this.TemporalSampling)
		{
			if (this.TemporalTexture != null && (this.TemporalTexture.width != this.GetCamPixelWidth() || this.TemporalTexture.height != this.GetCamPixelHeight()))
			{
				RenderTexture.ReleaseTemporary(this.TemporalTexture);
				this.TemporalTexture = null;
				this.TemporalFirst = true;
			}
			if (this.TemporalTexture == null)
			{
				this.TemporalTexture = RenderTexture.GetTemporary(this.GetCamPixelWidth(), this.GetCamPixelHeight(), 16, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
				this.TemporalTextureRTID = new RenderTargetIdentifier(this.TemporalTexture);
				this.TemporalTexture.hideFlags = HideFlags.DontSave;
				return;
			}
		}
		else if (this.TemporalTexture != null)
		{
			RenderTexture.ReleaseTemporary(this.TemporalTexture);
			this.TemporalTexture = null;
			this.TemporalFirst = true;
		}
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00007F34 File Offset: 0x00006134
	public static void ReleaseTempTextures()
	{
		if (HxVolumetricCamera.VolumetricTexture != null)
		{
			RenderTexture.ReleaseTemporary(HxVolumetricCamera.VolumetricTexture);
			HxVolumetricCamera.VolumetricTexture = null;
		}
		if (HxVolumetricCamera.FullBlurRT != null)
		{
			RenderTexture.ReleaseTemporary(HxVolumetricCamera.FullBlurRT);
			HxVolumetricCamera.FullBlurRT = null;
		}
		for (int i = 0; i < HxVolumetricCamera.VolumetricTransparencyTextures.Length; i++)
		{
			if (HxVolumetricCamera.VolumetricTransparencyTextures[i] != null)
			{
				RenderTexture.ReleaseTemporary(HxVolumetricCamera.VolumetricTransparencyTextures[i]);
				HxVolumetricCamera.VolumetricTransparencyTextures[i] = null;
			}
		}
		for (int j = 0; j < HxVolumetricCamera.VolumetricDensityTextures.Length; j++)
		{
			if (HxVolumetricCamera.VolumetricDensityTextures[j] != null)
			{
				RenderTexture.ReleaseTemporary(HxVolumetricCamera.VolumetricDensityTextures[j]);
				HxVolumetricCamera.VolumetricDensityTextures[j] = null;
			}
		}
		if (HxVolumetricCamera.downScaledBlurRT != null)
		{
			RenderTexture.ReleaseTemporary(HxVolumetricCamera.downScaledBlurRT);
			HxVolumetricCamera.downScaledBlurRT = null;
		}
		if (HxVolumetricCamera.FullBlurRT2 != null)
		{
			RenderTexture.ReleaseTemporary(HxVolumetricCamera.FullBlurRT2);
			HxVolumetricCamera.FullBlurRT2 = null;
		}
		if (HxVolumetricCamera.ScaledDepthTexture[0] != null)
		{
			RenderTexture.ReleaseTemporary(HxVolumetricCamera.ScaledDepthTexture[0]);
			HxVolumetricCamera.ScaledDepthTexture[0] = null;
		}
		if (HxVolumetricCamera.ScaledDepthTexture[1] != null)
		{
			RenderTexture.ReleaseTemporary(HxVolumetricCamera.ScaledDepthTexture[1]);
			HxVolumetricCamera.ScaledDepthTexture[1] = null;
		}
		if (HxVolumetricCamera.ScaledDepthTexture[2] != null)
		{
			RenderTexture.ReleaseTemporary(HxVolumetricCamera.ScaledDepthTexture[2]);
			HxVolumetricCamera.ScaledDepthTexture[2] = null;
		}
		if (HxVolumetricCamera.ScaledDepthTexture[3] != null)
		{
			RenderTexture.ReleaseTemporary(HxVolumetricCamera.ScaledDepthTexture[3]);
			HxVolumetricCamera.ScaledDepthTexture[3] = null;
		}
	}

	// Token: 0x0600009D RID: 157 RVA: 0x000080AC File Offset: 0x000062AC
	private void OnPreCull()
	{
		this.SetUpRenderOrder();
		this.ReleaseLightBuffers();
		this.MaxLightDistanceUsed = this.MaxLightDistance;
		HxVolumetricCamera.ConstructPlanes(this.Mycamera, 0f, this.MaxLightDistanceUsed);
		this.UpdateLightPoistions();
		this.UpdateParticlePoistions();
		this.FindActiveLights();
		this.BuildBuffer();
	}

	// Token: 0x0600009E RID: 158 RVA: 0x00008100 File Offset: 0x00006300
	private void UpdateLightPoistions()
	{
		this.MaxLightDistanceUsed = this.MaxLightDistance;
		HashSet<HxVolumetricLight>.Enumerator enumerator = HxVolumetricCamera.AllVolumetricLight.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.CustomMaxLightDistance)
			{
				this.MaxLightDistanceUsed = Mathf.Max(enumerator.Current.MaxLightDistance, this.MaxLightDistanceUsed);
			}
			enumerator.Current.UpdatePosition(false);
		}
		if (HxVolumetricCamera.LightOctree != null)
		{
			HxVolumetricCamera.LightOctree.TryShrink();
		}
		foreach (HxDensityVolume hxDensityVolume in HxVolumetricCamera.AllDensityVolumes)
		{
			hxDensityVolume.UpdateVolume();
		}
		if (HxDensityVolume.DensityOctree != null)
		{
			HxDensityVolume.DensityOctree.TryShrink();
		}
	}

	// Token: 0x0600009F RID: 159 RVA: 0x000081AC File Offset: 0x000063AC
	private void UpdateParticlePoistions()
	{
		if (this.ParticleDensitySupport)
		{
			foreach (HxVolumetricParticleSystem hxVolumetricParticleSystem in HxVolumetricCamera.AllParticleSystems)
			{
				hxVolumetricParticleSystem.UpdatePosition();
			}
			if (HxVolumetricCamera.ParticleOctree != null)
			{
				HxVolumetricCamera.ParticleOctree.TryShrink();
			}
		}
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x000081F4 File Offset: 0x000063F4
	private void Awake()
	{
		if (HxVolumetricCamera._SpotLightCookie == null)
		{
			HxVolumetricCamera._SpotLightCookie = (Texture2D)Resources.Load("LightSoftCookie");
		}
		this.CreatePIDs();
		this.Mycamera = base.GetComponent<Camera>();
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x00008229 File Offset: 0x00006429
	private void start()
	{
		this.Mycamera = base.GetComponent<Camera>();
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x00008238 File Offset: 0x00006438
	public void ReleaseLightBuffers()
	{
		for (int i = 0; i < HxVolumetricCamera.ActiveLights.Count; i++)
		{
			HxVolumetricCamera.ActiveLights[i].ReleaseBuffer();
		}
		HxVolumetricCamera.ActiveLights.Clear();
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00008274 File Offset: 0x00006474
	private void CreateLightbuffers()
	{
		if (this.BufferRenderLights == null)
		{
			this.BufferRenderLights = new CommandBuffer();
			this.BufferRenderLights.name = "renderLights";
		}
		else
		{
			this.BufferRenderLights.Clear();
		}
		if (this.LightBufferAdded)
		{
			HxVolumetricCamera.ActiveCamera.RemoveCommandBuffer(this.lastLightRender, this.BufferRenderLights);
			this.LightBufferAdded = false;
		}
		if (HxVolumetricCamera.Active.TransparencySupport)
		{
			this.BufferRenderLights.SetRenderTarget(HxVolumetricCamera.VolumetricTransparency[(int)HxVolumetricCamera.Active.compatibleTBuffer()], HxVolumetricCamera.ScaledDepthTextureRTID[(int)HxVolumetricCamera.Active.resolution]);
		}
		else
		{
			this.BufferRenderLights.SetRenderTarget(HxVolumetricCamera.VolumetricTextureRTID, HxVolumetricCamera.ScaledDepthTextureRTID[(int)HxVolumetricCamera.Active.resolution]);
		}
		HxVolumetricCamera.FirstDirectional = true;
		for (int i = 0; i < HxVolumetricCamera.ActiveLights.Count; i++)
		{
			HxVolumetricCamera.ActiveLights[i].BuildBuffer(this.BufferRenderLights);
		}
		this.LightBufferAdded = true;
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x00008370 File Offset: 0x00006570
	private static void CreateTileTexture()
	{
		HxVolumetricCamera.Tile5x5 = (Resources.Load("HxOffsetTile") as Texture2D);
		if (HxVolumetricCamera.Tile5x5 == null)
		{
			HxVolumetricCamera.Tile5x5 = new Texture2D(5, 5, TextureFormat.RFloat, false, true);
			HxVolumetricCamera.Tile5x5.hideFlags = HideFlags.DontSave;
			HxVolumetricCamera.Tile5x5.filterMode = FilterMode.Point;
			HxVolumetricCamera.Tile5x5.wrapMode = TextureWrapMode.Repeat;
			Color[] array = new Color[25];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Color((float)HxVolumetricCamera.Tile5x5int[i] * 0.04f, 0f, 0f, 0f);
			}
			HxVolumetricCamera.Tile5x5.SetPixels(array);
			HxVolumetricCamera.Tile5x5.Apply();
			Shader.SetGlobalTexture("Tile5x5", HxVolumetricCamera.Tile5x5);
			Shader.SetGlobalFloat("HxTileSize", 5f);
			return;
		}
		Shader.SetGlobalTexture("Tile5x5", HxVolumetricCamera.Tile5x5);
		Shader.SetGlobalFloat("HxTileSize", (float)HxVolumetricCamera.Tile5x5.width);
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x0000846C File Offset: 0x0000666C
	public static Mesh CreateOrtho(int sides, bool inner = true)
	{
		Vector3[] vertices = new Vector3[]
		{
			new Vector3(-0.5f, -0.5f, 0f),
			new Vector3(0.5f, -0.5f, 0f),
			new Vector3(0.5f, 0.5f, 0f),
			new Vector3(-0.5f, 0.5f, 0f),
			new Vector3(-0.5f, 0.5f, 1f),
			new Vector3(0.5f, 0.5f, 1f),
			new Vector3(0.5f, -0.5f, 1f),
			new Vector3(-0.5f, -0.5f, 1f)
		};
		int[] triangles = new int[]
		{
			0,
			2,
			1,
			0,
			3,
			2,
			2,
			3,
			4,
			2,
			4,
			5,
			1,
			2,
			5,
			1,
			5,
			6,
			0,
			7,
			4,
			0,
			4,
			3,
			5,
			4,
			7,
			5,
			7,
			6,
			0,
			6,
			7,
			0,
			1,
			6
		};
		Mesh mesh = new Mesh();
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		return mesh;
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x00008584 File Offset: 0x00006784
	public static Mesh CreateCone(int sides, bool inner = true)
	{
		Mesh mesh = new Mesh();
		Vector3[] array = new Vector3[sides + 1];
		int[] array2 = new int[sides * 3 + (sides - 2) * 3];
		float num = inner ? Mathf.Cos(3.1415927f / (float)sides) : 1f;
		float num2 = num * Mathf.Tan(3.1415927f / (float)sides);
		Vector3 a = new Vector3(0.5f - (1f - num) / 2f, 0f, 0f);
		Vector3 vector = new Vector3(0f, 0f, num2);
		a += new Vector3(0f, 0f, num2 / 2f);
		Quaternion rotation = Quaternion.Euler(new Vector3(0f, 360f / (float)sides, 0f));
		Quaternion rotation2 = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
		array[0] = new Vector3(0f, 0f, 0f);
		for (int i = 1; i < sides + 1; i++)
		{
			array[i] = rotation2 * (a - Vector3.up);
			a -= vector;
			vector = rotation * vector;
		}
		int num3;
		for (int j = 0; j < sides - 1; j++)
		{
			num3 = j * 3;
			array2[num3] = 0;
			array2[num3 + 1] = j + 1;
			array2[num3 + 2] = j + 2;
		}
		num3 = (sides - 1) * 3;
		array2[num3] = 0;
		array2[num3 + 1] = sides;
		array2[num3 + 2] = 1;
		num3 += 3;
		for (int k = 0; k < sides - 2; k++)
		{
			array2[num3] = 1;
			array2[num3 + 2] = k + 2;
			array2[num3 + 1] = k + 3;
			num3 += 3;
		}
		mesh.vertices = array;
		mesh.triangles = array2;
		mesh.uv = new Vector2[array.Length];
		mesh.colors = new Color[0];
		mesh.bounds = new Bounds(Vector3.zero, Vector3.one);
		mesh.RecalculateNormals();
		return mesh;
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00008790 File Offset: 0x00006990
	public static Mesh CreateQuad()
	{
		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[]
		{
			new Vector3(-1f, -1f, 1f),
			new Vector3(-1f, 1f, 1f),
			new Vector3(1f, -1f, 1f),
			new Vector3(1f, 1f, 1f)
		};
		mesh.triangles = new int[]
		{
			0,
			1,
			2,
			2,
			1,
			3
		};
		mesh.RecalculateBounds();
		return mesh;
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x00008848 File Offset: 0x00006A48
	public static Mesh CreateBox()
	{
		GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
		Mesh sharedMesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
		if (Application.isPlaying)
		{
			Destroy(gameObject);
			return sharedMesh;
		}
		DestroyImmediate(gameObject);
		return sharedMesh;
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x0000887C File Offset: 0x00006A7C
	public static Mesh CreateIcoSphere(int recursionLevel, float radius)
	{
		Mesh mesh = new Mesh();
		mesh.Clear();
		List<Vector3> list = new List<Vector3>();
		Dictionary<long, int> dictionary = new Dictionary<long, int>();
		float num = (1f + Mathf.Sqrt(5f)) / 2f;
		list.Add(new Vector3(-1f, num, 0f).normalized * radius);
		list.Add(new Vector3(1f, num, 0f).normalized * radius);
		list.Add(new Vector3(-1f, -num, 0f).normalized * radius);
		list.Add(new Vector3(1f, -num, 0f).normalized * radius);
		list.Add(new Vector3(0f, -1f, num).normalized * radius);
		list.Add(new Vector3(0f, 1f, num).normalized * radius);
		list.Add(new Vector3(0f, -1f, -num).normalized * radius);
		list.Add(new Vector3(0f, 1f, -num).normalized * radius);
		list.Add(new Vector3(num, 0f, -1f).normalized * radius);
		list.Add(new Vector3(num, 0f, 1f).normalized * radius);
		list.Add(new Vector3(-num, 0f, -1f).normalized * radius);
		list.Add(new Vector3(-num, 0f, 1f).normalized * radius);
		List<HxVolumetricCamera.TriangleIndices> list2 = new List<HxVolumetricCamera.TriangleIndices>();
		list2.Add(new HxVolumetricCamera.TriangleIndices(0, 11, 5));
		list2.Add(new HxVolumetricCamera.TriangleIndices(0, 5, 1));
		list2.Add(new HxVolumetricCamera.TriangleIndices(0, 1, 7));
		list2.Add(new HxVolumetricCamera.TriangleIndices(0, 7, 10));
		list2.Add(new HxVolumetricCamera.TriangleIndices(0, 10, 11));
		list2.Add(new HxVolumetricCamera.TriangleIndices(1, 5, 9));
		list2.Add(new HxVolumetricCamera.TriangleIndices(5, 11, 4));
		list2.Add(new HxVolumetricCamera.TriangleIndices(11, 10, 2));
		list2.Add(new HxVolumetricCamera.TriangleIndices(10, 7, 6));
		list2.Add(new HxVolumetricCamera.TriangleIndices(7, 1, 8));
		list2.Add(new HxVolumetricCamera.TriangleIndices(3, 9, 4));
		list2.Add(new HxVolumetricCamera.TriangleIndices(3, 4, 2));
		list2.Add(new HxVolumetricCamera.TriangleIndices(3, 2, 6));
		list2.Add(new HxVolumetricCamera.TriangleIndices(3, 6, 8));
		list2.Add(new HxVolumetricCamera.TriangleIndices(3, 8, 9));
		list2.Add(new HxVolumetricCamera.TriangleIndices(4, 9, 5));
		list2.Add(new HxVolumetricCamera.TriangleIndices(2, 4, 11));
		list2.Add(new HxVolumetricCamera.TriangleIndices(6, 2, 10));
		list2.Add(new HxVolumetricCamera.TriangleIndices(8, 6, 7));
		list2.Add(new HxVolumetricCamera.TriangleIndices(9, 8, 1));
		for (int i = 0; i < recursionLevel; i++)
		{
			List<HxVolumetricCamera.TriangleIndices> list3 = new List<HxVolumetricCamera.TriangleIndices>();
			foreach (HxVolumetricCamera.TriangleIndices triangleIndices in list2)
			{
				int middlePoint = HxVolumetricCamera.getMiddlePoint(triangleIndices.v1, triangleIndices.v2, ref list, ref dictionary, radius);
				int middlePoint2 = HxVolumetricCamera.getMiddlePoint(triangleIndices.v2, triangleIndices.v3, ref list, ref dictionary, radius);
				int middlePoint3 = HxVolumetricCamera.getMiddlePoint(triangleIndices.v3, triangleIndices.v1, ref list, ref dictionary, radius);
				list3.Add(new HxVolumetricCamera.TriangleIndices(triangleIndices.v1, middlePoint, middlePoint3));
				list3.Add(new HxVolumetricCamera.TriangleIndices(triangleIndices.v2, middlePoint2, middlePoint));
				list3.Add(new HxVolumetricCamera.TriangleIndices(triangleIndices.v3, middlePoint3, middlePoint2));
				list3.Add(new HxVolumetricCamera.TriangleIndices(middlePoint, middlePoint2, middlePoint3));
			}
			list2 = list3;
		}
		mesh.vertices = list.ToArray();
		List<int> list4 = new List<int>();
		for (int j = 0; j < list2.Count; j++)
		{
			list4.Add(list2[j].v1);
			list4.Add(list2[j].v2);
			list4.Add(list2[j].v3);
		}
		mesh.triangles = list4.ToArray();
		mesh.uv = new Vector2[list.Count];
		Vector3[] array = new Vector3[list.Count];
		for (int k = 0; k < array.Length; k++)
		{
			array[k] = list[k].normalized;
		}
		mesh.normals = array;
		mesh.bounds = new Bounds(Vector3.zero, Vector3.one);
		return mesh;
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00008DA4 File Offset: 0x00006FA4
	private static int getMiddlePoint(int p1, int p2, ref List<Vector3> vertices, ref Dictionary<long, int> cache, float radius)
	{
		bool flag = p1 < p2;
		long num = (long)(flag ? p1 : p2);
		long num2 = (long)(flag ? p2 : p1);
		long key = (num << 32) + num2;
		int result;
		if (cache.TryGetValue(key, out result))
		{
			return result;
		}
		Vector3 vector = vertices[p1];
		Vector3 vector2 = vertices[p2];
		Vector3 vector3 = new Vector3((vector.x + vector2.x) / 2f, (vector.y + vector2.y) / 2f, (vector.z + vector2.z) / 2f);
		int count = vertices.Count;
		vertices.Add(vector3.normalized * radius);
		cache.Add(key, count);
		return count;
	}

	// Token: 0x060000AB RID: 171 RVA: 0x00008E5F File Offset: 0x0000705F
	public void Create3DNoiseTexture()
	{
		this.NoiseTexture3D = (Resources.Load("NoiseTexture") as Texture3D);
		Shader.SetGlobalTexture("NoiseTexture3D", this.NoiseTexture3D);
	}

	// Token: 0x060000AC RID: 172 RVA: 0x00008E88 File Offset: 0x00007088
	private int PostoIndex(Vector3 pos)
	{
		if (pos.x >= 32f)
		{
			pos.x = 0f;
		}
		else if (pos.x < 0f)
		{
			pos.x = 31f;
		}
		if (pos.y >= 32f)
		{
			pos.y = 0f;
		}
		else if (pos.y < 0f)
		{
			pos.y = 31f;
		}
		if (pos.z >= 32f)
		{
			pos.z = 0f;
		}
		else if (pos.z < 0f)
		{
			pos.z = 31f;
		}
		return (int)(pos.z * 32f * 32f + pos.y * 32f + pos.x);
	}

	// Token: 0x04000039 RID: 57
	public HxVolumetricCamera.hxRenderOrder RenderOrder;

	// Token: 0x0400003A RID: 58
	public HxVolumetricRenderCallback callBackImageEffect;

	// Token: 0x0400003B RID: 59
	public HxVolumetricRenderCallback callBackImageEffectOpaque;

	// Token: 0x0400003C RID: 60
	public bool ShadowFix = true;

	// Token: 0x0400003D RID: 61
	private bool TemporalFirst = true;

	// Token: 0x0400003E RID: 62
	public bool TemporalSampling = true;

	// Token: 0x0400003F RID: 63
	[Range(0f, 1f)]
	public float DitherSpeed = 0.6256256f;

	// Token: 0x04000040 RID: 64
	[Range(0f, 1f)]
	public float LuminanceFeedback = 0.8f;

	// Token: 0x04000041 RID: 65
	[Range(0f, 1f)]
	public float MaxFeedback = 0.9f;

	// Token: 0x04000042 RID: 66
	[Range(0f, 4f)]
	public float NoiseContrast = 1f;

	// Token: 0x04000043 RID: 67
	private static Shader directionalShader;

	// Token: 0x04000044 RID: 68
	private static Shader pointShader;

	// Token: 0x04000045 RID: 69
	private static Shader spotShader;

	// Token: 0x04000046 RID: 70
	private static Shader ProjectorShader;

	// Token: 0x04000047 RID: 71
	[NonSerialized]
	public bool FullUsed;

	// Token: 0x04000048 RID: 72
	[NonSerialized]
	public bool LowResUsed;

	// Token: 0x04000049 RID: 73
	[NonSerialized]
	public bool HeightFogUsed;

	// Token: 0x0400004A RID: 74
	[NonSerialized]
	public bool HeightFogOffUsed;

	// Token: 0x0400004B RID: 75
	[NonSerialized]
	public bool NoiseUsed;

	// Token: 0x0400004C RID: 76
	[NonSerialized]
	public bool NoiseOffUsed;

	// Token: 0x0400004D RID: 77
	[NonSerialized]
	public bool TransparencyUsed;

	// Token: 0x0400004E RID: 78
	[NonSerialized]
	public bool TransparencyOffUsed;

	// Token: 0x0400004F RID: 79
	[NonSerialized]
	public bool DensityParticlesUsed;

	// Token: 0x04000050 RID: 80
	[NonSerialized]
	public bool PointUsed;

	// Token: 0x04000051 RID: 81
	[NonSerialized]
	public bool SpotUsed;

	// Token: 0x04000052 RID: 82
	[NonSerialized]
	public bool ProjectorUsed;

	// Token: 0x04000053 RID: 83
	[NonSerialized]
	public bool DirectionalUsed;

	// Token: 0x04000054 RID: 84
	[NonSerialized]
	public bool SinglePassStereoUsed;

	// Token: 0x04000055 RID: 85
	public static HxVolumetricCamera.TransparencyQualities TransparencyBufferDepth = HxVolumetricCamera.TransparencyQualities.Medium;

	// Token: 0x04000056 RID: 86
	public static HxVolumetricCamera.DensityParticleQualities DensityBufferDepth = HxVolumetricCamera.DensityParticleQualities.High;

	// Token: 0x04000057 RID: 87
	private int EnumBufferDepthLength = 4;

	// Token: 0x04000058 RID: 88
	private Matrix4x4 CurrentView;

	// Token: 0x04000059 RID: 89
	private Matrix4x4 CurrentProj;

	// Token: 0x0400005A RID: 90
	private Matrix4x4 CurrentInvers;

	// Token: 0x0400005B RID: 91
	private Matrix4x4 CurrentView2;

	// Token: 0x0400005C RID: 92
	private Matrix4x4 CurrentProj2;

	// Token: 0x0400005D RID: 93
	private Matrix4x4 CurrentInvers2;

	// Token: 0x0400005E RID: 94
	private RenderTexture TemporalTexture;

	// Token: 0x0400005F RID: 95
	private RenderTargetIdentifier TemporalTextureRTID;

	// Token: 0x04000060 RID: 96
	private static RenderTexture VolumetricTexture;

	// Token: 0x04000061 RID: 97
	private static RenderTexture FullBlurRT;

	// Token: 0x04000062 RID: 98
	private static RenderTargetIdentifier FullBlurRTID;

	// Token: 0x04000063 RID: 99
	private static RenderTexture downScaledBlurRT;

	// Token: 0x04000064 RID: 100
	private static RenderTargetIdentifier downScaledBlurRTID;

	// Token: 0x04000065 RID: 101
	private static RenderTexture FullBlurRT2;

	// Token: 0x04000066 RID: 102
	private static RenderTargetIdentifier FullBlurRT2ID;

	// Token: 0x04000067 RID: 103
	private static RenderTargetIdentifier[] VolumetricUpsampledBlurTextures = new RenderTargetIdentifier[2];

	// Token: 0x04000068 RID: 104
	private static RenderTexture[] VolumetricDensityTextures = new RenderTexture[8];

	// Token: 0x04000069 RID: 105
	private static int[] VolumetricDensityPID = new int[4];

	// Token: 0x0400006A RID: 106
	private static int[] VolumetricTransparencyPID = new int[4];

	// Token: 0x0400006B RID: 107
	private static RenderTexture[] VolumetricTransparencyTextures = new RenderTexture[8];

	// Token: 0x0400006C RID: 108
	public static RenderTargetIdentifier[][] VolumetricDensity = new RenderTargetIdentifier[][]
	{
		new RenderTargetIdentifier[1],
		new RenderTargetIdentifier[2],
		new RenderTargetIdentifier[3],
		new RenderTargetIdentifier[4],
		new RenderTargetIdentifier[5],
		new RenderTargetIdentifier[6],
		new RenderTargetIdentifier[7],
		new RenderTargetIdentifier[8]
	};

	// Token: 0x0400006D RID: 109
	public static RenderTargetIdentifier[][] VolumetricTransparency = new RenderTargetIdentifier[][]
	{
		new RenderTargetIdentifier[2],
		new RenderTargetIdentifier[3],
		new RenderTargetIdentifier[4],
		new RenderTargetIdentifier[5],
		new RenderTargetIdentifier[6],
		new RenderTargetIdentifier[7],
		new RenderTargetIdentifier[8],
		new RenderTargetIdentifier[9]
	};

	// Token: 0x0400006E RID: 110
	public static RenderTargetIdentifier[][] VolumetricTransparencyI = new RenderTargetIdentifier[][]
	{
		new RenderTargetIdentifier[1],
		new RenderTargetIdentifier[2],
		new RenderTargetIdentifier[3],
		new RenderTargetIdentifier[4],
		new RenderTargetIdentifier[5],
		new RenderTargetIdentifier[6],
		new RenderTargetIdentifier[7],
		new RenderTargetIdentifier[8]
	};

	// Token: 0x0400006F RID: 111
	private static RenderTexture[] ScaledDepthTexture = new RenderTexture[4];

	// Token: 0x04000070 RID: 112
	private static ShaderVariantCollection CollectionAll;

	// Token: 0x04000071 RID: 113
	public static Texture2D Tile5x5;

	// Token: 0x04000072 RID: 114
	private static int VolumetricTexturePID;

	// Token: 0x04000073 RID: 115
	private static int ScaledDepthTexturePID;

	// Token: 0x04000074 RID: 116
	public static int ShadowMapTexturePID;

	// Token: 0x04000075 RID: 117
	public static RenderTargetIdentifier VolumetricTextureRTID;

	// Token: 0x04000076 RID: 118
	public static RenderTargetIdentifier[] ScaledDepthTextureRTID = new RenderTargetIdentifier[4];

	// Token: 0x04000077 RID: 119
	[NonSerialized]
	public static Material DownSampleMaterial;

	// Token: 0x04000078 RID: 120
	[NonSerialized]
	public static Material VolumeBlurMaterial;

	// Token: 0x04000079 RID: 121
	[NonSerialized]
	public static Material TransparencyBlurMaterial;

	// Token: 0x0400007A RID: 122
	[NonSerialized]
	public static Material ApplyMaterial;

	// Token: 0x0400007B RID: 123
	[NonSerialized]
	public static Material ApplyDirectMaterial;

	// Token: 0x0400007C RID: 124
	[NonSerialized]
	public static Material ApplyQueueMaterial;

	// Token: 0x0400007D RID: 125
	public Texture3D NoiseTexture3D;

	// Token: 0x0400007E RID: 126
	public static Matrix4x4 BlitMatrix;

	// Token: 0x0400007F RID: 127
	public static Matrix4x4 BlitMatrixMV;

	// Token: 0x04000080 RID: 128
	public static Matrix4x4 BlitMatrixMVP;

	// Token: 0x04000081 RID: 129
	public static Vector3 BlitScale;

	// Token: 0x04000082 RID: 130
	[Tooltip("Rending resolution, Lower for more speed, higher for better quality")]
	public HxVolumetricCamera.Resolution resolution = HxVolumetricCamera.Resolution.half;

	// Token: 0x04000083 RID: 131
	[Tooltip("How many samples per pixel, Recommended 4-8 for point, 6 - 16 for Directional")]
	[Range(2f, 64f)]
	public int SampleCount = 4;

	// Token: 0x04000084 RID: 132
	[Tooltip("How many samples per pixel, Recommended 4-8 for point, 6 - 16 for Directional")]
	[Range(2f, 64f)]
	public int DirectionalSampleCount = 8;

	// Token: 0x04000085 RID: 133
	[Tooltip("Max distance the directional light gets raymarched.")]
	public float MaxDirectionalRayDistance = 128f;

	// Token: 0x04000086 RID: 134
	[Tooltip("Any point of spot lights passed this point will not render.")]
	public float MaxLightDistance = 128f;

	// Token: 0x04000087 RID: 135
	[Range(0f, 1f)]
	[Tooltip("Density of air")]
	public float Density = 0.05f;

	// Token: 0x04000088 RID: 136
	[Range(0f, 2f)]
	public float AmbientLightingStrength = 0.5f;

	// Token: 0x04000089 RID: 137
	[Tooltip("0 for even scattering, 1 for forward scattering")]
	[Range(0f, 0.995f)]
	public float MieScattering = 0.4f;

	// Token: 0x0400008A RID: 138
	[Range(0f, 1f)]
	[Tooltip("Create a sun using mie Scattering")]
	public float SunSize;

	// Token: 0x0400008B RID: 139
	[Tooltip("Allows the sun to bleed over the edge of objects (recommend using bloom)")]
	public bool SunBleed = true;

	// Token: 0x0400008C RID: 140
	[Range(0f, 0.5f)]
	[Tooltip("dimms results over distance")]
	public float Extinction = 0.05f;

	// Token: 0x0400008D RID: 141
	[Tooltip("Tone down Extinction effect on FinalColor")]
	[Range(0f, 1f)]
	public float ExtinctionEffect;

	// Token: 0x0400008E RID: 142
	public bool FogHeightEnabled;

	// Token: 0x0400008F RID: 143
	public float FogHeight = 5f;

	// Token: 0x04000090 RID: 144
	public float FogTransitionSize = 5f;

	// Token: 0x04000091 RID: 145
	public float AboveFogPercent = 0.1f;

	// Token: 0x04000092 RID: 146
	[Tooltip("Ambient Mode - Use unitys or overide your own")]
	public HxVolumetricCamera.HxAmbientMode Ambient;

	// Token: 0x04000093 RID: 147
	public Color AmbientSky = Color.white;

	// Token: 0x04000094 RID: 148
	public Color AmbientEquator = Color.white;

	// Token: 0x04000095 RID: 149
	public Color AmbientGround = Color.white;

	// Token: 0x04000096 RID: 150
	[Range(0f, 1f)]
	public float AmbientIntensity = 1f;

	// Token: 0x04000097 RID: 151
	public HxVolumetricCamera.HxTintMode TintMode;

	// Token: 0x04000098 RID: 152
	public Color TintColor = Color.red;

	// Token: 0x04000099 RID: 153
	public Color TintColor2 = Color.blue;

	// Token: 0x0400009A RID: 154
	public float TintIntensity = 0.2f;

	// Token: 0x0400009B RID: 155
	[Range(0f, 1f)]
	public float TintGradient = 0.2f;

	// Token: 0x0400009C RID: 156
	public Vector3 CurrentTint;

	// Token: 0x0400009D RID: 157
	public Vector3 CurrentTintEdge;

	// Token: 0x0400009E RID: 158
	[Tooltip("Use 3D noise")]
	public bool NoiseEnabled;

	// Token: 0x0400009F RID: 159
	[Tooltip("The scale of the noise texture")]
	public Vector3 NoiseScale = new Vector3(0.1f, 0.1f, 0.1f);

	// Token: 0x040000A0 RID: 160
	[Tooltip("Used to simulate some wind")]
	public Vector3 NoiseVelocity = new Vector3(1f, 0f, 1f);

	// Token: 0x040000A1 RID: 161
	[Tooltip("Allows particles to modulate the air density")]
	public bool ParticleDensitySupport;

	// Token: 0x040000A2 RID: 162
	[Tooltip("Rending resolution of density, Lower for more speed, higher for more detailed dust")]
	public HxVolumetricCamera.DensityResolution densityResolution = HxVolumetricCamera.DensityResolution.eighth;

	// Token: 0x040000A3 RID: 163
	[Tooltip("Max Distance of density particles")]
	public float densityDistance = 64f;

	// Token: 0x040000A4 RID: 164
	private float densityBias = 1.7f;

	// Token: 0x040000A5 RID: 165
	[Tooltip("Enabling Transparency support has a cost - disable if you dont need it")]
	public bool TransparencySupport;

	// Token: 0x040000A6 RID: 166
	[Tooltip("Max Distance for transparency Support - lower distance will give greater resilts")]
	public float transparencyDistance = 64f;

	// Token: 0x040000A7 RID: 167
	[Tooltip("Cost a little extra but can remove the grainy look on Transparent objects when sample count is low")]
	[Range(0f, 4f)]
	public int BlurTransparency = 1;

	// Token: 0x040000A8 RID: 168
	private float transparencyBias = 1.5f;

	// Token: 0x040000A9 RID: 169
	[Range(0f, 4f)]
	[Tooltip("Blur results of volumetric pass")]
	public int blurCount = 1;

	// Token: 0x040000AA RID: 170
	[Tooltip("Used in final blur pass, Higher number will retain silhouette")]
	public float BlurDepthFalloff = 5f;

	// Token: 0x040000AB RID: 171
	[Tooltip("Used in Downsample blur pass, Higher number will retain silhouette")]
	public float DownsampledBlurDepthFalloff = 5f;

	// Token: 0x040000AC RID: 172
	[Range(0f, 4f)]
	[Tooltip("Blur bad results after upscaling")]
	public int UpSampledblurCount;

	// Token: 0x040000AD RID: 173
	[Tooltip("If depth is with-in this threshold, bilinearly sample result")]
	public float DepthThreshold = 0.06f;

	// Token: 0x040000AE RID: 174
	[Tooltip("Use gaussian weights - makes blur less blurry but can make it more splotchy")]
	public bool GaussianWeights;

	// Token: 0x040000AF RID: 175
	[HideInInspector]
	[Tooltip("Only enable if you arnt using tonemapping and HDR mode")]
	public bool MapToLDR;

	// Token: 0x040000B0 RID: 176
	[Tooltip("A small amount of noise can be added to remove and color banding from the volumetric effect")]
	public bool RemoveColorBanding = true;

	// Token: 0x040000B1 RID: 177
	[NonSerialized]
	public Vector3 Offset = Vector3.zero;

	// Token: 0x040000B2 RID: 178
	private static int DepthThresholdPID;

	// Token: 0x040000B3 RID: 179
	private static int BlurDepthFalloffPID;

	// Token: 0x040000B4 RID: 180
	private static int VolumeScalePID;

	// Token: 0x040000B5 RID: 181
	private static int InverseViewMatrixPID;

	// Token: 0x040000B6 RID: 182
	private static int InverseProjectionMatrixPID;

	// Token: 0x040000B7 RID: 183
	private static int InverseProjectionMatrix2PID;

	// Token: 0x040000B8 RID: 184
	private static int NoiseOffsetPID;

	// Token: 0x040000B9 RID: 185
	private static int ShadowDistancePID;

	// Token: 0x040000BA RID: 186
	private static HxVolumetricShadersUsed UsedShaderSettings;

	// Token: 0x040000BB RID: 187
	private static List<string> ShaderVariantList = new List<string>(10);

	// Token: 0x040000BC RID: 188
	[HideInInspector]
	public static List<HxDensityVolume> ActiveVolumes = new List<HxDensityVolume>();

	// Token: 0x040000BD RID: 189
	public static List<HxVolumetricLight> ActiveLights = new List<HxVolumetricLight>();

	// Token: 0x040000BE RID: 190
	public static List<HxVolumetricParticleSystem> ActiveParticleSystems = new List<HxVolumetricParticleSystem>();

	// Token: 0x040000BF RID: 191
	public static HxOctree<HxVolumetricLight> LightOctree;

	// Token: 0x040000C0 RID: 192
	public static HxOctree<HxVolumetricParticleSystem> ParticleOctree;

	// Token: 0x040000C1 RID: 193
	public static HashSet<HxDensityVolume> AllDensityVolumes = new HashSet<HxDensityVolume>();

	// Token: 0x040000C2 RID: 194
	public static HashSet<HxVolumetricLight> AllVolumetricLight = new HashSet<HxVolumetricLight>();

	// Token: 0x040000C3 RID: 195
	public static HashSet<HxVolumetricParticleSystem> AllParticleSystems = new HashSet<HxVolumetricParticleSystem>();

	// Token: 0x040000C4 RID: 196
	private bool test;

	// Token: 0x040000C5 RID: 197
	public static Mesh QuadMesh;

	// Token: 0x040000C6 RID: 198
	public static Mesh BoxMesh;

	// Token: 0x040000C7 RID: 199
	public static Mesh SphereMesh;

	// Token: 0x040000C8 RID: 200
	public static Mesh SpotLightMesh;

	// Token: 0x040000C9 RID: 201
	public static Mesh OrthoProjectorMesh;

	// Token: 0x040000CA RID: 202
	[HideInInspector]
	private Camera Mycamera;

	// Token: 0x040000CB RID: 203
	private static float[] ResolutionScale = new float[]
	{
		1f,
		0.5f,
		0.25f,
		0.125f
	};

	// Token: 0x040000CC RID: 204
	public static float[] SampleScale = new float[]
	{
		1f,
		4f,
		16f,
		32f
	};

	// Token: 0x040000CD RID: 205
	private CommandBuffer BufferSetup;

	// Token: 0x040000CE RID: 206
	private CommandBuffer BufferRender;

	// Token: 0x040000CF RID: 207
	private CommandBuffer BufferRenderLights;

	// Token: 0x040000D0 RID: 208
	private CommandBuffer BufferFinalize;

	// Token: 0x040000D1 RID: 209
	private bool dirty = true;

	// Token: 0x040000D2 RID: 210
	[NonSerialized]
	public static bool PIDCreated = false;

	// Token: 0x040000D3 RID: 211
	[NonSerialized]
	private static Dictionary<int, Material> DirectionalMaterial = new Dictionary<int, Material>();

	// Token: 0x040000D4 RID: 212
	[NonSerialized]
	private static Dictionary<int, Material> PointMaterial = new Dictionary<int, Material>();

	// Token: 0x040000D5 RID: 213
	[NonSerialized]
	private static Dictionary<int, Material> SpotMaterial = new Dictionary<int, Material>();

	// Token: 0x040000D6 RID: 214
	[NonSerialized]
	private static Dictionary<int, Material> ProjectorMaterial = new Dictionary<int, Material>();

	// Token: 0x040000D7 RID: 215
	public static ShaderVariantCollection.ShaderVariant[] DirectionalVariant = new ShaderVariantCollection.ShaderVariant[128];

	// Token: 0x040000D8 RID: 216
	public static ShaderVariantCollection.ShaderVariant[] PointVariant = new ShaderVariantCollection.ShaderVariant[128];

	// Token: 0x040000D9 RID: 217
	public static ShaderVariantCollection.ShaderVariant[] SpotVariant = new ShaderVariantCollection.ShaderVariant[128];

	// Token: 0x040000DA RID: 218
	public static Material ShadowMaterial;

	// Token: 0x040000DB RID: 219
	public static Material DensityMaterial;

	// Token: 0x040000DC RID: 220
	[HideInInspector]
	public Matrix4x4 MatrixVP;

	// Token: 0x040000DD RID: 221
	public Matrix4x4 LastMatrixVP;

	// Token: 0x040000DE RID: 222
	public Matrix4x4 LastMatrixVPInv;

	// Token: 0x040000DF RID: 223
	public Matrix4x4 LastMatrixVP2;

	// Token: 0x040000E0 RID: 224
	public Matrix4x4 LastMatrixVPInv2;

	// Token: 0x040000E1 RID: 225
	[HideInInspector]
	public Matrix4x4 MatrixV;

	// Token: 0x040000E2 RID: 226
	private bool OffsetUpdated;

	// Token: 0x040000E3 RID: 227
	[HideInInspector]
	private static Texture2D _SpotLightCookie;

	// Token: 0x040000E4 RID: 228
	[HideInInspector]
	private static Texture2D _LightFalloff;

	// Token: 0x040000E5 RID: 229
	private int ParticleDensityRenderCount;

	// Token: 0x040000E6 RID: 230
	private static Matrix4x4 particleMatrix;

	// Token: 0x040000E7 RID: 231
	public static HxVolumetricCamera Active;

	// Token: 0x040000E8 RID: 232
	public static Camera ActiveCamera;

	// Token: 0x040000E9 RID: 233
	private CameraEvent LightRenderEvent = CameraEvent.AfterLighting;

	// Token: 0x040000EA RID: 234
	private CameraEvent SetupEvent = CameraEvent.AfterDepthNormalsTexture;

	// Token: 0x040000EB RID: 235
	private CameraEvent RenderEvent = CameraEvent.BeforeLighting;

	// Token: 0x040000EC RID: 236
	private CameraEvent FinalizeEvent = CameraEvent.AfterLighting;

	// Token: 0x040000ED RID: 237
	public static List<HxVolumetricLight> ActiveDirectionalLights = new List<HxVolumetricLight>();

	// Token: 0x040000EE RID: 238
	private static Vector3 MinBounds;

	// Token: 0x040000EF RID: 239
	private static Vector3 MaxBounds;

	// Token: 0x040000F0 RID: 240
	private static Plane[] CameraPlanes = new Plane[6];

	// Token: 0x040000F1 RID: 241
	private bool preCullEventAdded;

	// Token: 0x040000F2 RID: 242
	private bool BuffersBuilt;

	// Token: 0x040000F3 RID: 243
	private bool LightBufferAdded;

	// Token: 0x040000F4 RID: 244
	private bool SetupBufferAdded;

	// Token: 0x040000F5 RID: 245
	private bool SetupBufferDirty;

	// Token: 0x040000F6 RID: 246
	private bool FinalizeBufferAdded;

	// Token: 0x040000F7 RID: 247
	private bool FinalizeBufferDirty;

	// Token: 0x040000F8 RID: 248
	private CameraEvent lastApply;

	// Token: 0x040000F9 RID: 249
	private CameraEvent lastRender;

	// Token: 0x040000FA RID: 250
	private CameraEvent lastSetup;

	// Token: 0x040000FB RID: 251
	private CameraEvent lastFinalize;

	// Token: 0x040000FC RID: 252
	private CameraEvent lastLightRender;

	// Token: 0x040000FD RID: 253
	private bool LastPlaying;

	// Token: 0x040000FE RID: 254
	[NonSerialized]
	private static int lastRes = -1;

	// Token: 0x040000FF RID: 255
	[NonSerialized]
	private int lastBlurCount = -1;

	// Token: 0x04000100 RID: 256
	[NonSerialized]
	private int lastupSampleBlurCount;

	// Token: 0x04000101 RID: 257
	[NonSerialized]
	private int lastLDR = -1;

	// Token: 0x04000102 RID: 258
	[NonSerialized]
	private int lastBanding = -1;

	// Token: 0x04000103 RID: 259
	[NonSerialized]
	private int lastH = -1;

	// Token: 0x04000104 RID: 260
	[NonSerialized]
	private int lastW = -1;

	// Token: 0x04000105 RID: 261
	[NonSerialized]
	private int lastPath = -1;

	// Token: 0x04000106 RID: 262
	[NonSerialized]
	private int lastGaussian = -1;

	// Token: 0x04000107 RID: 263
	[NonSerialized]
	private int lastTransparency = -1;

	// Token: 0x04000108 RID: 264
	[NonSerialized]
	private int lastDensity = -1;

	// Token: 0x04000109 RID: 265
	[NonSerialized]
	private int lastDensityRes = -1;

	// Token: 0x0400010A RID: 266
	[NonSerialized]
	private float lastDepthFalloff = -1f;

	// Token: 0x0400010B RID: 267
	[NonSerialized]
	private float lastDownDepthFalloff = -1f;

	// Token: 0x0400010C RID: 268
	private float currentDitherOffset;

	// Token: 0x0400010D RID: 269
	private float MaxLightDistanceUsed;

	// Token: 0x0400010E RID: 270
	public static bool FirstDirectional = true;

	// Token: 0x0400010F RID: 271
	private static int[] Tile5x5int = new int[]
	{
		8,
		18,
		22,
		0,
		13,
		4,
		14,
		9,
		19,
		21,
		16,
		23,
		1,
		12,
		6,
		10,
		7,
		15,
		24,
		3,
		20,
		2,
		11,
		5,
		17
	};

	// Token: 0x02000311 RID: 785
	public enum hxRenderOrder
	{
		// Token: 0x040014E5 RID: 5349
		ImageEffect,
		// Token: 0x040014E6 RID: 5350
		ImageEffectOpaque
	}

	// Token: 0x02000312 RID: 786
	public enum TransparencyQualities
	{
		// Token: 0x040014E8 RID: 5352
		Low,
		// Token: 0x040014E9 RID: 5353
		Medium,
		// Token: 0x040014EA RID: 5354
		High,
		// Token: 0x040014EB RID: 5355
		VeryHigh
	}

	// Token: 0x02000313 RID: 787
	public enum DensityParticleQualities
	{
		// Token: 0x040014ED RID: 5357
		Low,
		// Token: 0x040014EE RID: 5358
		Medium,
		// Token: 0x040014EF RID: 5359
		High,
		// Token: 0x040014F0 RID: 5360
		VeryHigh
	}

	// Token: 0x02000314 RID: 788
	public enum HxAmbientMode
	{
		// Token: 0x040014F2 RID: 5362
		UseRenderSettings,
		// Token: 0x040014F3 RID: 5363
		Color,
		// Token: 0x040014F4 RID: 5364
		Gradient
	}

	// Token: 0x02000315 RID: 789
	public enum HxTintMode
	{
		// Token: 0x040014F6 RID: 5366
		Off,
		// Token: 0x040014F7 RID: 5367
		Color,
		// Token: 0x040014F8 RID: 5368
		Edge,
		// Token: 0x040014F9 RID: 5369
		Gradient
	}

	// Token: 0x02000316 RID: 790
	public enum Resolution
	{
		// Token: 0x040014FB RID: 5371
		full,
		// Token: 0x040014FC RID: 5372
		half,
		// Token: 0x040014FD RID: 5373
		quarter
	}

	// Token: 0x02000317 RID: 791
	public enum DensityResolution
	{
		// Token: 0x040014FF RID: 5375
		full,
		// Token: 0x04001500 RID: 5376
		half,
		// Token: 0x04001501 RID: 5377
		quarter,
		// Token: 0x04001502 RID: 5378
		eighth
	}

	// Token: 0x02000318 RID: 792
	private struct TriangleIndices
	{
		// Token: 0x060014A2 RID: 5282 RVA: 0x00057E9F File Offset: 0x0005609F
		public TriangleIndices(int v1, int v2, int v3)
		{
			this.v1 = v1;
			this.v2 = v2;
			this.v3 = v3;
		}

		// Token: 0x04001503 RID: 5379
		public int v1;

		// Token: 0x04001504 RID: 5380
		public int v2;

		// Token: 0x04001505 RID: 5381
		public int v3;
	}
}

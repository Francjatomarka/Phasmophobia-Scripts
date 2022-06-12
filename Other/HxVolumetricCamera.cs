using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

[ExecuteInEditMode]
public class HxVolumetricCamera : MonoBehaviour
{
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

	public HxVolumetricCamera.TransparencyQualities compatibleTBuffer()
	{
		if (HxVolumetricCamera.TransparencyBufferDepth > HxVolumetricCamera.TransparencyQualities.Medium && SystemInfo.graphicsDeviceType != GraphicsDeviceType.Direct3D11 && SystemInfo.graphicsDeviceType != GraphicsDeviceType.Direct3D12 && SystemInfo.graphicsDeviceType != GraphicsDeviceType.PlayStation4)
		{
			return HxVolumetricCamera.TransparencyQualities.High;
		}
		return HxVolumetricCamera.TransparencyBufferDepth;
	}

	private bool IsRenderBoth()
	{
		return this.Mycamera.stereoTargetEye == StereoTargetEyeMask.Both && Application.isPlaying && XRSettings.enabled && XRDevice.isPresent;
	}

	private HxVolumetricCamera.DensityParticleQualities compatibleDBuffer()
	{
		return HxVolumetricCamera.DensityBufferDepth;
	}

	private void MyPreCull(Camera cam)
	{
		if (cam != HxVolumetricCamera.ActiveCamera)
		{
			this.ReleaseLightBuffers();
			this.SetUpRenderOrder();
		}
	}

	public bool renderDensityParticleCheck()
	{
		return this.ParticleDensityRenderCount > 0;
	}

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

	public static bool ActiveFull()
	{
		return HxVolumetricCamera.Active.resolution == HxVolumetricCamera.Resolution.full;
	}

	private void DefineFull()
	{
	}

	private static void UpdateLight(HxOctreeNode<HxVolumetricLight>.NodeObject node, Vector3 boundsMin, Vector3 boundsMax)
	{
		HxVolumetricCamera.LightOctree.Move(node, boundsMin, boundsMax);
	}

	public static HxOctreeNode<HxVolumetricLight>.NodeObject AddLightOctree(HxVolumetricLight light, Vector3 boundsMin, Vector3 boundsMax)
	{
		if (HxVolumetricCamera.LightOctree == null)
		{
			HxVolumetricCamera.LightOctree = new HxOctree<HxVolumetricLight>(Vector3.zero, 100f, 0.1f, 10f);
		}
		return HxVolumetricCamera.LightOctree.Add(light, boundsMin, boundsMax);
	}

	public static HxOctreeNode<HxVolumetricParticleSystem>.NodeObject AddParticleOctree(HxVolumetricParticleSystem particle, Vector3 boundsMin, Vector3 boundsMax)
	{
		if (HxVolumetricCamera.ParticleOctree == null)
		{
			HxVolumetricCamera.ParticleOctree = new HxOctree<HxVolumetricParticleSystem>(Vector3.zero, 100f, 0.1f, 10f);
		}
		return HxVolumetricCamera.ParticleOctree.Add(particle, boundsMin, boundsMax);
	}

	public static void RemoveLightOctree(HxVolumetricLight light)
	{
		if (HxVolumetricCamera.LightOctree != null)
		{
			HxVolumetricCamera.LightOctree.Remove(light);
		}
	}

	public static void RemoveParticletOctree(HxVolumetricParticleSystem Particle)
	{
		if (HxVolumetricCamera.ParticleOctree != null)
		{
			HxVolumetricCamera.ParticleOctree.Remove(Particle);
		}
	}

	private void OnApplicationQuit()
	{
		HxVolumetricCamera.PIDCreated = false;
	}

	public Camera GetCamera()
	{
		if (this.Mycamera == null)
		{
			this.Mycamera = base.GetComponent<Camera>();
		}
		return this.Mycamera;
	}

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

	private void OnPostRender()
	{
		Shader.DisableKeyword("VTRANSPARENCY_ON");
	}

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

	private int GetCamPixelHeight()
	{
		if (this.Mycamera.stereoTargetEye != StereoTargetEyeMask.None && Application.isPlaying && XRSettings.enabled && XRDevice.isPresent)
		{
			return XRSettings.eyeTextureHeight;
		}
		return this.Mycamera.pixelHeight;
	}

	private int GetCamPixelWidth()
	{
		if (this.Mycamera.stereoTargetEye != StereoTargetEyeMask.None && Application.isPlaying && XRSettings.enabled && XRDevice.isPresent)
		{
			return XRSettings.eyeTextureWidth + ((this.Mycamera.stereoTargetEye == StereoTargetEyeMask.Both) ? (XRSettings.eyeTextureWidth + Mathf.CeilToInt(48f * XRSettings.eyeTextureResolutionScale)) : 0);
		}
		return this.Mycamera.pixelWidth;
	}

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

	private void FindActiveParticleSystems()
	{
		HxVolumetricCamera.ActiveParticleSystems.Clear();
		if (HxVolumetricCamera.ParticleOctree != null)
		{
			HxVolumetricCamera.ParticleOctree.GetObjectsBoundsPlane(ref HxVolumetricCamera.CameraPlanes, HxVolumetricCamera.MinBounds, HxVolumetricCamera.MaxBounds, HxVolumetricCamera.ActiveParticleSystems);
		}
	}

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

	private void CreateApplyBuffer()
	{
	}

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

	private void SaveUsedShaderVarience()
	{
	}

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

	public void EventOnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, HxVolumetricCamera.ApplyMaterial, ((QualitySettings.activeColorSpace == ColorSpace.Linear) ? 1 : 2) + (this.RemoveColorBanding ? 0 : 2));
	}

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

	private void Awake()
	{
		if (HxVolumetricCamera._SpotLightCookie == null)
		{
			HxVolumetricCamera._SpotLightCookie = (Texture2D)Resources.Load("LightSoftCookie");
		}
		this.CreatePIDs();
		this.Mycamera = base.GetComponent<Camera>();
	}

	private void start()
	{
		this.Mycamera = base.GetComponent<Camera>();
	}

	public void ReleaseLightBuffers()
	{
		for (int i = 0; i < HxVolumetricCamera.ActiveLights.Count; i++)
		{
			HxVolumetricCamera.ActiveLights[i].ReleaseBuffer();
		}
		HxVolumetricCamera.ActiveLights.Clear();
	}

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

	public void Create3DNoiseTexture()
	{
		this.NoiseTexture3D = (Resources.Load("NoiseTexture") as Texture3D);
		Shader.SetGlobalTexture("NoiseTexture3D", this.NoiseTexture3D);
	}

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

	public HxVolumetricCamera.hxRenderOrder RenderOrder;

	public HxVolumetricRenderCallback callBackImageEffect;

	public HxVolumetricRenderCallback callBackImageEffectOpaque;

	public bool ShadowFix = true;

	private bool TemporalFirst = true;

	public bool TemporalSampling = true;

	[Range(0f, 1f)]
	public float DitherSpeed = 0.6256256f;

	[Range(0f, 1f)]
	public float LuminanceFeedback = 0.8f;

	[Range(0f, 1f)]
	public float MaxFeedback = 0.9f;

	[Range(0f, 4f)]
	public float NoiseContrast = 1f;

	private static Shader directionalShader;

	private static Shader pointShader;

	private static Shader spotShader;

	private static Shader ProjectorShader;

	[NonSerialized]
	public bool FullUsed;

	[NonSerialized]
	public bool LowResUsed;

	[NonSerialized]
	public bool HeightFogUsed;

	[NonSerialized]
	public bool HeightFogOffUsed;

	[NonSerialized]
	public bool NoiseUsed;

	[NonSerialized]
	public bool NoiseOffUsed;

	[NonSerialized]
	public bool TransparencyUsed;

	[NonSerialized]
	public bool TransparencyOffUsed;

	[NonSerialized]
	public bool DensityParticlesUsed;

	[NonSerialized]
	public bool PointUsed;

	[NonSerialized]
	public bool SpotUsed;

	[NonSerialized]
	public bool ProjectorUsed;

	[NonSerialized]
	public bool DirectionalUsed;

	[NonSerialized]
	public bool SinglePassStereoUsed;

	public static HxVolumetricCamera.TransparencyQualities TransparencyBufferDepth = HxVolumetricCamera.TransparencyQualities.Medium;

	public static HxVolumetricCamera.DensityParticleQualities DensityBufferDepth = HxVolumetricCamera.DensityParticleQualities.High;

	private int EnumBufferDepthLength = 4;

	private Matrix4x4 CurrentView;

	private Matrix4x4 CurrentProj;

	private Matrix4x4 CurrentInvers;

	private Matrix4x4 CurrentView2;

	private Matrix4x4 CurrentProj2;

	private Matrix4x4 CurrentInvers2;

	private RenderTexture TemporalTexture;

	private RenderTargetIdentifier TemporalTextureRTID;

	private static RenderTexture VolumetricTexture;

	private static RenderTexture FullBlurRT;

	private static RenderTargetIdentifier FullBlurRTID;

	private static RenderTexture downScaledBlurRT;

	private static RenderTargetIdentifier downScaledBlurRTID;

	private static RenderTexture FullBlurRT2;

	private static RenderTargetIdentifier FullBlurRT2ID;

	private static RenderTargetIdentifier[] VolumetricUpsampledBlurTextures = new RenderTargetIdentifier[2];

	private static RenderTexture[] VolumetricDensityTextures = new RenderTexture[8];

	private static int[] VolumetricDensityPID = new int[4];

	private static int[] VolumetricTransparencyPID = new int[4];

	private static RenderTexture[] VolumetricTransparencyTextures = new RenderTexture[8];

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

	private static RenderTexture[] ScaledDepthTexture = new RenderTexture[4];

	private static ShaderVariantCollection CollectionAll;

	public static Texture2D Tile5x5;

	private static int VolumetricTexturePID;

	private static int ScaledDepthTexturePID;

	public static int ShadowMapTexturePID;

	public static RenderTargetIdentifier VolumetricTextureRTID;

	public static RenderTargetIdentifier[] ScaledDepthTextureRTID = new RenderTargetIdentifier[4];

	[NonSerialized]
	public static Material DownSampleMaterial;

	[NonSerialized]
	public static Material VolumeBlurMaterial;

	[NonSerialized]
	public static Material TransparencyBlurMaterial;

	[NonSerialized]
	public static Material ApplyMaterial;

	[NonSerialized]
	public static Material ApplyDirectMaterial;

	[NonSerialized]
	public static Material ApplyQueueMaterial;

	public Texture3D NoiseTexture3D;

	public static Matrix4x4 BlitMatrix;

	public static Matrix4x4 BlitMatrixMV;

	public static Matrix4x4 BlitMatrixMVP;

	public static Vector3 BlitScale;

	[Tooltip("Rending resolution, Lower for more speed, higher for better quality")]
	public HxVolumetricCamera.Resolution resolution = HxVolumetricCamera.Resolution.half;

	[Tooltip("How many samples per pixel, Recommended 4-8 for point, 6 - 16 for Directional")]
	[Range(2f, 64f)]
	public int SampleCount = 4;

	[Tooltip("How many samples per pixel, Recommended 4-8 for point, 6 - 16 for Directional")]
	[Range(2f, 64f)]
	public int DirectionalSampleCount = 8;

	[Tooltip("Max distance the directional light gets raymarched.")]
	public float MaxDirectionalRayDistance = 128f;

	[Tooltip("Any point of spot lights passed this point will not render.")]
	public float MaxLightDistance = 128f;

	[Range(0f, 1f)]
	[Tooltip("Density of air")]
	public float Density = 0.05f;

	[Range(0f, 2f)]
	public float AmbientLightingStrength = 0.5f;

	[Tooltip("0 for even scattering, 1 for forward scattering")]
	[Range(0f, 0.995f)]
	public float MieScattering = 0.4f;

	[Range(0f, 1f)]
	[Tooltip("Create a sun using mie Scattering")]
	public float SunSize;

	[Tooltip("Allows the sun to bleed over the edge of objects (recommend using bloom)")]
	public bool SunBleed = true;

	[Range(0f, 0.5f)]
	[Tooltip("dimms results over distance")]
	public float Extinction = 0.05f;

	[Tooltip("Tone down Extinction effect on FinalColor")]
	[Range(0f, 1f)]
	public float ExtinctionEffect;

	public bool FogHeightEnabled;

	public float FogHeight = 5f;

	public float FogTransitionSize = 5f;

	public float AboveFogPercent = 0.1f;

	[Tooltip("Ambient Mode - Use unitys or overide your own")]
	public HxVolumetricCamera.HxAmbientMode Ambient;

	public Color AmbientSky = Color.white;

	public Color AmbientEquator = Color.white;

	public Color AmbientGround = Color.white;

	[Range(0f, 1f)]
	public float AmbientIntensity = 1f;

	public HxVolumetricCamera.HxTintMode TintMode;

	public Color TintColor = Color.red;

	public Color TintColor2 = Color.blue;

	public float TintIntensity = 0.2f;

	[Range(0f, 1f)]
	public float TintGradient = 0.2f;

	public Vector3 CurrentTint;

	public Vector3 CurrentTintEdge;

	[Tooltip("Use 3D noise")]
	public bool NoiseEnabled;

	[Tooltip("The scale of the noise texture")]
	public Vector3 NoiseScale = new Vector3(0.1f, 0.1f, 0.1f);

	[Tooltip("Used to simulate some wind")]
	public Vector3 NoiseVelocity = new Vector3(1f, 0f, 1f);

	[Tooltip("Allows particles to modulate the air density")]
	public bool ParticleDensitySupport;

	[Tooltip("Rending resolution of density, Lower for more speed, higher for more detailed dust")]
	public HxVolumetricCamera.DensityResolution densityResolution = HxVolumetricCamera.DensityResolution.eighth;

	[Tooltip("Max Distance of density particles")]
	public float densityDistance = 64f;

	private float densityBias = 1.7f;

	[Tooltip("Enabling Transparency support has a cost - disable if you dont need it")]
	public bool TransparencySupport;

	[Tooltip("Max Distance for transparency Support - lower distance will give greater resilts")]
	public float transparencyDistance = 64f;

	[Tooltip("Cost a little extra but can remove the grainy look on Transparent objects when sample count is low")]
	[Range(0f, 4f)]
	public int BlurTransparency = 1;

	private float transparencyBias = 1.5f;

	[Range(0f, 4f)]
	[Tooltip("Blur results of volumetric pass")]
	public int blurCount = 1;

	[Tooltip("Used in final blur pass, Higher number will retain silhouette")]
	public float BlurDepthFalloff = 5f;

	[Tooltip("Used in Downsample blur pass, Higher number will retain silhouette")]
	public float DownsampledBlurDepthFalloff = 5f;

	[Range(0f, 4f)]
	[Tooltip("Blur bad results after upscaling")]
	public int UpSampledblurCount;

	[Tooltip("If depth is with-in this threshold, bilinearly sample result")]
	public float DepthThreshold = 0.06f;

	[Tooltip("Use gaussian weights - makes blur less blurry but can make it more splotchy")]
	public bool GaussianWeights;

	[HideInInspector]
	[Tooltip("Only enable if you arnt using tonemapping and HDR mode")]
	public bool MapToLDR;

	[Tooltip("A small amount of noise can be added to remove and color banding from the volumetric effect")]
	public bool RemoveColorBanding = true;

	[NonSerialized]
	public Vector3 Offset = Vector3.zero;

	private static int DepthThresholdPID;

	private static int BlurDepthFalloffPID;

	private static int VolumeScalePID;

	private static int InverseViewMatrixPID;

	private static int InverseProjectionMatrixPID;

	private static int InverseProjectionMatrix2PID;

	private static int NoiseOffsetPID;

	private static int ShadowDistancePID;

	private static HxVolumetricShadersUsed UsedShaderSettings;

	private static List<string> ShaderVariantList = new List<string>(10);

	[HideInInspector]
	public static List<HxDensityVolume> ActiveVolumes = new List<HxDensityVolume>();

	public static List<HxVolumetricLight> ActiveLights = new List<HxVolumetricLight>();

	public static List<HxVolumetricParticleSystem> ActiveParticleSystems = new List<HxVolumetricParticleSystem>();

	public static HxOctree<HxVolumetricLight> LightOctree;

	public static HxOctree<HxVolumetricParticleSystem> ParticleOctree;

	public static HashSet<HxDensityVolume> AllDensityVolumes = new HashSet<HxDensityVolume>();

	public static HashSet<HxVolumetricLight> AllVolumetricLight = new HashSet<HxVolumetricLight>();

	public static HashSet<HxVolumetricParticleSystem> AllParticleSystems = new HashSet<HxVolumetricParticleSystem>();

	private bool test;

	public static Mesh QuadMesh;

	public static Mesh BoxMesh;

	public static Mesh SphereMesh;

	public static Mesh SpotLightMesh;

	public static Mesh OrthoProjectorMesh;

	[HideInInspector]
	private Camera Mycamera;

	private static float[] ResolutionScale = new float[]
	{
		1f,
		0.5f,
		0.25f,
		0.125f
	};

	public static float[] SampleScale = new float[]
	{
		1f,
		4f,
		16f,
		32f
	};

	private CommandBuffer BufferSetup;

	private CommandBuffer BufferRender;

	private CommandBuffer BufferRenderLights;

	private CommandBuffer BufferFinalize;

	private bool dirty = true;

	[NonSerialized]
	public static bool PIDCreated = false;

	[NonSerialized]
	private static Dictionary<int, Material> DirectionalMaterial = new Dictionary<int, Material>();

	[NonSerialized]
	private static Dictionary<int, Material> PointMaterial = new Dictionary<int, Material>();

	[NonSerialized]
	private static Dictionary<int, Material> SpotMaterial = new Dictionary<int, Material>();

	[NonSerialized]
	private static Dictionary<int, Material> ProjectorMaterial = new Dictionary<int, Material>();

	public static ShaderVariantCollection.ShaderVariant[] DirectionalVariant = new ShaderVariantCollection.ShaderVariant[128];

	public static ShaderVariantCollection.ShaderVariant[] PointVariant = new ShaderVariantCollection.ShaderVariant[128];

	public static ShaderVariantCollection.ShaderVariant[] SpotVariant = new ShaderVariantCollection.ShaderVariant[128];

	public static Material ShadowMaterial;

	public static Material DensityMaterial;

	[HideInInspector]
	public Matrix4x4 MatrixVP;

	public Matrix4x4 LastMatrixVP;

	public Matrix4x4 LastMatrixVPInv;

	public Matrix4x4 LastMatrixVP2;

	public Matrix4x4 LastMatrixVPInv2;

	[HideInInspector]
	public Matrix4x4 MatrixV;

	private bool OffsetUpdated;

	[HideInInspector]
	private static Texture2D _SpotLightCookie;

	[HideInInspector]
	private static Texture2D _LightFalloff;

	private int ParticleDensityRenderCount;

	private static Matrix4x4 particleMatrix;

	public static HxVolumetricCamera Active;

	public static Camera ActiveCamera;

	private CameraEvent LightRenderEvent = CameraEvent.AfterLighting;

	private CameraEvent SetupEvent = CameraEvent.AfterDepthNormalsTexture;

	private CameraEvent RenderEvent = CameraEvent.BeforeLighting;

	private CameraEvent FinalizeEvent = CameraEvent.AfterLighting;

	public static List<HxVolumetricLight> ActiveDirectionalLights = new List<HxVolumetricLight>();

	private static Vector3 MinBounds;

	private static Vector3 MaxBounds;

	private static Plane[] CameraPlanes = new Plane[6];

	private bool preCullEventAdded;

	private bool BuffersBuilt;

	private bool LightBufferAdded;

	private bool SetupBufferAdded;

	private bool SetupBufferDirty;

	private bool FinalizeBufferAdded;

	private bool FinalizeBufferDirty;

	private CameraEvent lastApply;

	private CameraEvent lastRender;

	private CameraEvent lastSetup;

	private CameraEvent lastFinalize;

	private CameraEvent lastLightRender;

	private bool LastPlaying;

	[NonSerialized]
	private static int lastRes = -1;

	[NonSerialized]
	private int lastBlurCount = -1;

	[NonSerialized]
	private int lastupSampleBlurCount;

	[NonSerialized]
	private int lastLDR = -1;

	[NonSerialized]
	private int lastBanding = -1;

	[NonSerialized]
	private int lastH = -1;

	[NonSerialized]
	private int lastW = -1;

	[NonSerialized]
	private int lastPath = -1;

	[NonSerialized]
	private int lastGaussian = -1;

	[NonSerialized]
	private int lastTransparency = -1;

	[NonSerialized]
	private int lastDensity = -1;

	[NonSerialized]
	private int lastDensityRes = -1;

	[NonSerialized]
	private float lastDepthFalloff = -1f;

	[NonSerialized]
	private float lastDownDepthFalloff = -1f;

	private float currentDitherOffset;

	private float MaxLightDistanceUsed;

	public static bool FirstDirectional = true;

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

	public enum hxRenderOrder
	{
		ImageEffect,
		ImageEffectOpaque
	}

	public enum TransparencyQualities
	{
		Low,
		Medium,
		High,
		VeryHigh
	}

	public enum DensityParticleQualities
	{
		Low,
		Medium,
		High,
		VeryHigh
	}

	public enum HxAmbientMode
	{
		UseRenderSettings,
		Color,
		Gradient
	}

	public enum HxTintMode
	{
		Off,
		Color,
		Edge,
		Gradient
	}

	public enum Resolution
	{
		full,
		half,
		quarter
	}

	public enum DensityResolution
	{
		full,
		half,
		quarter,
		eighth
	}

	private struct TriangleIndices
	{
		public TriangleIndices(int v1, int v2, int v3)
		{
			this.v1 = v1;
			this.v2 = v2;
			this.v3 = v3;
		}

		public int v1;

		public int v2;

		public int v3;
	}
}


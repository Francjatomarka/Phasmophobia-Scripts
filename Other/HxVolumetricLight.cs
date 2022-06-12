using System;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class HxVolumetricLight : MonoBehaviour
{
	public Light LightSafe()
	{
		if (this.myLight == null)
		{
			this.myLight = base.GetComponent<Light>();
		}
		return this.myLight;
	}

	public HxDummyLight DummyLightSafe()
	{
		if (this.myDummyLight == null)
		{
			this.myDummyLight = base.GetComponent<HxDummyLight>();
		}
		return this.myDummyLight;
	}

	private LightType GetLightType()
	{
		if (this.myLight != null)
		{
			return this.myLight.type;
		}
		if (this.myDummyLight != null)
		{
			return this.myDummyLight.type;
		}
		return LightType.Area;
	}

	private LightShadows LightShadow()
	{
		if (this.myLight != null)
		{
			return this.myLight.shadows;
		}
		return LightShadows.None;
	}

	private bool HasLight()
	{
		return this.myLight != null || this.myDummyLight != null;
	}

	private Texture LightCookie()
	{
		if (this.myLight != null)
		{
			return this.myLight.cookie;
		}
		if (this.myDummyLight != null)
		{
			return this.myDummyLight.cookie;
		}
		return null;
	}

	private Texture LightFalloffTexture()
	{
		if (this.LightFalloff != null)
		{
			return this.LightFalloff;
		}
		return HxVolumetricCamera.Active.LightFalloff;
	}

	private float LightShadowBias()
	{
		if (this.myLight != null)
		{
			return this.myLight.shadowBias * 1.05f;
		}
		return 0.1f;
	}

	private Color LightColor()
	{
		if (this.myLight != null)
		{
			if (QualitySettings.activeColorSpace != ColorSpace.Gamma)
			{
				return this.myLight.color.linear;
			}
			return this.myLight.color;
		}
		else if (this.myDummyLight != null)
		{
			if (QualitySettings.activeColorSpace != ColorSpace.Gamma)
			{
				return this.myDummyLight.color.linear;
			}
			return this.myDummyLight.color;
		}
		else
		{
			if (!(this.myProjector != null))
			{
				return Color.white;
			}
			if (QualitySettings.activeColorSpace != ColorSpace.Gamma)
			{
				return this.myProjector.material.GetColor("_Color").linear;
			}
			return this.myProjector.material.GetColor("_Color");
		}
	}

	private float LightSpotAngle()
	{
		if (this.myLight != null)
		{
			return this.myLight.spotAngle;
		}
		if (this.myDummyLight != null)
		{
			return this.myDummyLight.spotAngle;
		}
		if (this.myProjector != null)
		{
			return this.myProjector.fieldOfView;
		}
		return 1f;
	}

	private bool LightEnabled()
	{
		if (this.myLight != null)
		{
			return this.myLight.enabled;
		}
		if (this.myDummyLight != null)
		{
			return this.myDummyLight.enabled;
		}
		if (this.myProjector != null)
		{
			return this.myProjector.enabled;
		}
		this.myLight = base.GetComponent<Light>();
		if (this.myLight != null)
		{
			return this.myLight.enabled;
		}
		this.myDummyLight = base.GetComponent<HxDummyLight>();
		if (this.myDummyLight != null)
		{
			return this.myDummyLight.enabled;
		}
		this.myProjector = base.GetComponent<Projector>();
		return this.myProjector != null && this.myProjector.enabled;
	}

	private float LightRange()
	{
		if (this.myLight != null)
		{
			return this.myLight.range;
		}
		if (this.myDummyLight != null)
		{
			return this.myDummyLight.range;
		}
		if (this.myProjector != null)
		{
			return this.myProjector.farClipPlane;
		}
		return 0f;
	}

	private float LightShadowStrength()
	{
		if (this.myLight != null)
		{
			return this.myLight.shadowStrength;
		}
		return 1f;
	}

	private float LightIntensity()
	{
		if (this.myLight != null)
		{
			return this.myLight.intensity;
		}
		if (this.myDummyLight != null)
		{
			return this.myDummyLight.intensity;
		}
		if (this.myProjector != null)
		{
			return 1f;
		}
		return 0f;
	}

	private void OnEnable()
	{
		this.myLight = base.GetComponent<Light>();
		this.myDummyLight = base.GetComponent<HxDummyLight>();
		this.myProjector = base.GetComponent<Projector>();
		HxVolumetricCamera.AllVolumetricLight.Add(this);
		this.UpdatePosition(true);
		if (this.GetLightType() != LightType.Directional)
		{
			this.octreeNode = HxVolumetricCamera.AddLightOctree(this, this.minBounds, this.maxBounds);
			return;
		}
		HxVolumetricCamera.ActiveDirectionalLights.Add(this);
	}

	private void OnDisable()
	{
		HxVolumetricCamera.AllVolumetricLight.Remove(this);
		if (this.GetLightType() != LightType.Directional)
		{
			HxVolumetricCamera.RemoveLightOctree(this);
			this.octreeNode = null;
			return;
		}
		HxVolumetricCamera.ActiveDirectionalLights.Remove(this);
	}

	private void OnDestroy()
	{
		HxVolumetricCamera.AllVolumetricLight.Remove(this);
		if (this.lastType == LightType.Directional)
		{
			HxVolumetricCamera.ActiveDirectionalLights.Remove(this);
			return;
		}
		HxVolumetricCamera.RemoveLightOctree(this);
		this.octreeNode = null;
	}

	private void Start()
	{
		this.myLight = base.GetComponent<Light>();
		this.myDummyLight = base.GetComponent<HxDummyLight>();
	}

	public void BuildBuffer(CommandBuffer CameraBuffer)
	{
		if (this.LightEnabled() && this.LightIntensity() > 0f)
		{
			switch (this.GetLightType())
			{
			case LightType.Spot:
				this.BuildSpotLightBuffer(CameraBuffer);
				this.LastBufferDirectional = false;
				return;
			case LightType.Directional:
				this.BuildDirectionalBuffer(CameraBuffer);
				this.LastBufferDirectional = true;
				return;
			case LightType.Point:
				this.BuildPointBuffer(CameraBuffer);
				this.LastBufferDirectional = false;
				return;
			case LightType.Area:
				if (this.myProjector != null)
				{
					this.BuildProjectorLightBuffer(CameraBuffer);
					this.LastBufferDirectional = false;
				}
				break;
			default:
				return;
			}
		}
	}

	public void ReleaseBuffer()
	{
		if (this.myLight != null && this.bufferBuilt)
		{
			if (this.LastBufferDirectional)
			{
				this.myLight.RemoveCommandBuffer(LightEvent.AfterShadowMap, this.BufferCopy);
				this.myLight.RemoveCommandBuffer(LightEvent.AfterScreenspaceMask, this.BufferRender);
			}
			else
			{
				this.myLight.RemoveCommandBuffer(LightEvent.AfterShadowMap, this.BufferRender);
			}
			this.bufferBuilt = false;
		}
	}

	public static void CreatePID()
	{
		HxVolumetricLight._hxProjectorTexturePID = Shader.PropertyToID("_ShadowTex");
		HxVolumetricLight._hxProjectorFalloffTexturePID = Shader.PropertyToID("_FalloffTex");
		HxVolumetricLight.hxNearPlanePID = Shader.PropertyToID("hxNearPlane");
		HxVolumetricLight.VolumetricBMVPPID = Shader.PropertyToID("VolumetricBMVP");
		HxVolumetricLight.VolumetricMVPPID = Shader.PropertyToID("VolumetricMVP");
		HxVolumetricLight.VolumetricMVP2PID = Shader.PropertyToID("VolumetricMVP2");
		HxVolumetricLight.LightColourPID = Shader.PropertyToID("LightColour");
		HxVolumetricLight.LightColour2PID = Shader.PropertyToID("LightColour2");
		HxVolumetricLight.VolumetricMVPID = Shader.PropertyToID("VolumetricMV");
		HxVolumetricLight.FogHeightsPID = Shader.PropertyToID("FogHeights");
		HxVolumetricLight.PhasePID = Shader.PropertyToID("Phase");
		HxVolumetricLight._LightParamsPID = Shader.PropertyToID("_LightParams");
		HxVolumetricLight.DensityPID = Shader.PropertyToID("Density");
		HxVolumetricLight.ShadowBiasPID = Shader.PropertyToID("ShadowBias");
		HxVolumetricLight._CustomLightPositionPID = Shader.PropertyToID("_CustomLightPosition");
		HxVolumetricLight.NoiseScalePID = Shader.PropertyToID("NoiseScale");
		HxVolumetricLight.NoiseOffsetPID = Shader.PropertyToID("NoiseOffset");
		HxVolumetricLight._SpotLightParamsPID = Shader.PropertyToID("_SpotLightParams");
		HxVolumetricLight._LightTexture0PID = Shader.PropertyToID("_LightTexture0");
	}

	private float LightNearPlane()
	{
		if (this.myLight != null)
		{
			return this.myLight.shadowNearPlane;
		}
		return 0.1f;
	}

	private int DirectionalPass(CommandBuffer buffer)
	{
		if (HxVolumetricCamera.Active.Ambient == HxVolumetricCamera.HxAmbientMode.UseRenderSettings)
		{
			if (RenderSettings.ambientMode == AmbientMode.Flat)
			{
				buffer.SetGlobalVector("AmbientSkyColor", RenderSettings.ambientSkyColor * RenderSettings.ambientIntensity);
				return 0;
			}
			if (RenderSettings.ambientMode == AmbientMode.Trilight)
			{
				buffer.SetGlobalVector("AmbientSkyColor", RenderSettings.ambientSkyColor * RenderSettings.ambientIntensity);
				buffer.SetGlobalVector("AmbientEquatorColor", RenderSettings.ambientEquatorColor * RenderSettings.ambientIntensity);
				buffer.SetGlobalVector("AmbientGroundColor", RenderSettings.ambientGroundColor * RenderSettings.ambientIntensity);
				return 1;
			}
			return 2;
		}
		else
		{
			if (HxVolumetricCamera.Active.Ambient == HxVolumetricCamera.HxAmbientMode.Color)
			{
				buffer.SetGlobalVector("AmbientSkyColor", HxVolumetricCamera.Active.AmbientSky * HxVolumetricCamera.Active.AmbientIntensity);
				return 0;
			}
			if (HxVolumetricCamera.Active.Ambient == HxVolumetricCamera.HxAmbientMode.Gradient)
			{
				buffer.SetGlobalVector("AmbientSkyColor", HxVolumetricCamera.Active.AmbientSky * HxVolumetricCamera.Active.AmbientIntensity);
				buffer.SetGlobalVector("AmbientEquatorColor", HxVolumetricCamera.Active.AmbientEquator * HxVolumetricCamera.Active.AmbientIntensity);
				buffer.SetGlobalVector("AmbientGroundColor", HxVolumetricCamera.Active.AmbientGround * HxVolumetricCamera.Active.AmbientIntensity);
				return 1;
			}
			return 2;
		}
	}

	private float getContrast()
	{
		if (this.CustomNoiseContrast)
		{
			return this.NoiseContrast;
		}
		return HxVolumetricCamera.Active.NoiseContrast;
	}

	private bool ShaderModel4()
	{
		if (SystemInfo.graphicsDeviceType != GraphicsDeviceType.Direct3D9)
		{
			GraphicsDeviceType graphicsDeviceType = SystemInfo.graphicsDeviceType;
		}
		return false;
	}

	private void BuildDirectionalBuffer(CommandBuffer CameraBuffer)
	{
		bool flag = this.LightShadow() != LightShadows.None && this.Shadows;
		if (this.dirty)
		{
			if (flag)
			{
				if (this.BufferCopy == null)
				{
					this.BufferCopy = new CommandBuffer();
					this.BufferCopy.name = "ShadowCopy";
					this.BufferCopy.SetGlobalTexture(HxVolumetricCamera.ShadowMapTexturePID, BuiltinRenderTextureType.CurrentActive);
				}
				if (this.BufferRender == null)
				{
					this.BufferRender = new CommandBuffer();
					this.BufferRender.name = "VolumetricRender";
				}
				this.bufferBuilt = true;
				CameraBuffer = this.BufferRender;
				this.BufferRender.Clear();
			}
			if (flag && HxVolumetricCamera.Active.ShadowFix)
			{
				Graphics.DrawMesh(HxVolumetricCamera.BoxMesh, HxVolumetricCamera.Active.transform.position, HxVolumetricCamera.Active.transform.rotation, HxVolumetricCamera.ShadowMaterial, 0, HxVolumetricCamera.ActiveCamera, 0, null, true);
			}
			Vector3 forward = base.transform.forward;
			if (this.CustomFogHeightEnabled ? this.FogHeightEnabled : HxVolumetricCamera.Active.FogHeightEnabled)
			{
				CameraBuffer.SetGlobalVector(HxVolumetricLight.FogHeightsPID, new Vector3((this.CustomFogHeight ? this.FogHeight : HxVolumetricCamera.Active.FogHeight) - (this.CustomFogTransitionSize ? this.FogTransitionSize : HxVolumetricCamera.Active.FogTransitionSize), this.CustomFogHeight ? this.FogHeight : HxVolumetricCamera.Active.FogHeight, this.CustomAboveFogPercent ? this.AboveFogPercent : HxVolumetricCamera.Active.AboveFogPercent));
			}
			float fogDensity = this.GetFogDensity();
			CameraBuffer.SetGlobalVector("MaxRayDistance", new Vector2(Mathf.Min(QualitySettings.shadowDistance, this.CustomMaxLightDistance ? this.MaxLightDistance : HxVolumetricCamera.Active.MaxDirectionalRayDistance), this.CustomMaxLightDistance ? this.MaxLightDistance : HxVolumetricCamera.Active.MaxDirectionalRayDistance));
			float num = this.CustomMieScatter ? this.MieScattering : HxVolumetricCamera.Active.MieScattering;
			Vector4 vector = new Vector4(0.07957747f, 1f - num * num, 1f + num * num, 2f * num);
			float num2 = this.CustomSunSize ? this.SunSize : HxVolumetricCamera.Active.SunSize;
			CameraBuffer.SetGlobalVector("SunSize", new Vector2((float)((num2 == 0f) ? 0 : 1), (float)((this.CustomSunBleed ? this.SunBleed : HxVolumetricCamera.Active.SunBleed) ? 1 : 0)));
			num2 = Mathf.Lerp(0.9999f, 0.995f, Mathf.Pow(num2, 4f));
			this.LoadVolumeData();
			this.LoadVolumeDateIntoBuffer(CameraBuffer);
			Vector4 value = new Vector4(0.07957747f, 1f - num2 * num2, 1f + num2 * num2, 2f * num2);
			CameraBuffer.SetGlobalVector("Phase2", value);
			CameraBuffer.SetGlobalVector(HxVolumetricLight.PhasePID, vector);
			this.SetColors(CameraBuffer);
			CameraBuffer.SetGlobalVector(HxVolumetricLight._LightParamsPID, new Vector4(this.CustomStrength ? this.Strength : this.LightShadowStrength(), 0f, 0f, this.CustomIntensity ? this.Intensity : this.LightIntensity()));
			CameraBuffer.SetGlobalVector(HxVolumetricLight.DensityPID, new Vector4(fogDensity, (float)this.GetSampleCount(flag), 0f, this.CustomExtinction ? this.Extinction : HxVolumetricCamera.Active.Extinction));
			CameraBuffer.SetGlobalVector(HxVolumetricLight.ShadowBiasPID, new Vector3(this.LightShadowBias(), this.LightNearPlane(), (1f - (this.CustomStrength ? this.Strength : this.LightShadowStrength())) * vector.x * (vector.y / Mathf.Pow(vector.z - vector.w * -1f, 1.5f))));
			CameraBuffer.SetGlobalVector(HxVolumetricLight._SpotLightParamsPID, new Vector4(forward.x, forward.y, forward.z, 0f));
			Vector3 vector2 = this.CustomNoiseScale ? this.NoiseScale : HxVolumetricCamera.Active.NoiseScale;
			vector2 = new Vector3(1f / vector2.x, 1f / vector2.y, 1f / vector2.z) / 32f;
			CameraBuffer.SetGlobalVector(HxVolumetricLight.NoiseScalePID, vector2);
			if (!this.OffsetUpdated)
			{
				this.OffsetUpdated = true;
				this.Offset += this.NoiseVelocity * Time.deltaTime;
			}
			CameraBuffer.SetGlobalVector(HxVolumetricLight.NoiseOffsetPID, this.CustomNoiseVelocity ? this.Offset : HxVolumetricCamera.Active.Offset);
			CameraBuffer.SetGlobalFloat("FirstLight", (float)(HxVolumetricCamera.FirstDirectional ? 1 : 0));
			CameraBuffer.SetGlobalFloat("AmbientStrength", HxVolumetricCamera.Active.AmbientLightingStrength);
			HxVolumetricCamera.FirstDirectional = false;
			if (flag)
			{
				if (HxVolumetricCamera.Active.TransparencySupport)
				{
					CameraBuffer.SetRenderTarget(HxVolumetricCamera.VolumetricTransparency[(int)HxVolumetricCamera.Active.compatibleTBuffer()], HxVolumetricCamera.ScaledDepthTextureRTID[(int)HxVolumetricCamera.Active.resolution]);
				}
				else
				{
					CameraBuffer.SetRenderTarget(HxVolumetricCamera.VolumetricTextureRTID, HxVolumetricCamera.ScaledDepthTextureRTID[(int)HxVolumetricCamera.Active.resolution]);
				}
			}
			CameraBuffer.SetGlobalMatrix(HxVolumetricLight.VolumetricMVPPID, HxVolumetricCamera.BlitMatrixMVP);
			CameraBuffer.SetGlobalFloat("ExtinctionEffect", HxVolumetricCamera.Active.ExtinctionEffect);
			int mid = this.MID(flag, HxVolumetricCamera.ActiveFull());
			if (this.CustomNoiseEnabled ? this.NoiseEnabled : HxVolumetricCamera.Active.NoiseEnabled)
			{
				if (HxVolumetricLight.propertyBlock == null)
				{
					HxVolumetricLight.propertyBlock = new MaterialPropertyBlock();
				}
				Texture3D noiseTexture = this.GetNoiseTexture();
				if (noiseTexture != null)
				{
					HxVolumetricLight.propertyBlock.SetFloat("hxNoiseContrast", this.getContrast());
					HxVolumetricLight.propertyBlock.SetTexture("NoiseTexture3D", noiseTexture);
				}
			}
			CameraBuffer.DrawMesh(HxVolumetricCamera.QuadMesh, HxVolumetricCamera.BlitMatrix, HxVolumetricCamera.GetDirectionalMaterial(mid), 0, this.DirectionalPass(CameraBuffer), HxVolumetricLight.propertyBlock);
			if (flag)
			{
				this.myLight.AddCommandBuffer(LightEvent.AfterShadowMap, this.BufferCopy);
				this.myLight.AddCommandBuffer(LightEvent.AfterScreenspaceMask, this.BufferRender);
			}
		}
	}

	private void LoadVolumeDateIntoBuffer(CommandBuffer buffer)
	{
		if (this.ShaderModel4())
		{
			buffer.SetGlobalMatrixArray("hxVolumeMatrix", HxVolumetricLight.VolumeMatrixArrays);
			buffer.SetGlobalVectorArray("hxVolumeSettings", HxVolumetricLight.VolumeSettingsArrays);
			return;
		}
		buffer.SetGlobalMatrixArray("hxVolumeMatrixOld", HxVolumetricLight.VolumeMatrixArraysOld);
		buffer.SetGlobalVectorArray("hxVolumeSettingsOld", HxVolumetricLight.VolumeSettingsArraysOld);
	}

	private float CalcLightInstensityDistance(float distance)
	{
		return Mathf.InverseLerp(this.CustomMaxLightDistance ? this.MaxLightDistance : HxVolumetricCamera.Active.MaxLightDistance, (this.CustomMaxLightDistance ? this.MaxLightDistance : HxVolumetricCamera.Active.MaxLightDistance) * 0.8f, distance);
	}

	private void BuildSpotLightBuffer(CommandBuffer cameraBuffer)
	{
		float num = this.ClosestDistanceToCone(HxVolumetricCamera.Active.transform.position);
		float num2 = this.CalcLightInstensityDistance(num);
		if (num2 > 0f)
		{
			bool flag = this.LightShadow() != LightShadows.None && this.Shadows;
			if (flag && num > QualitySettings.shadowDistance - HxVolumetricLight.ShadowDistanceExtra)
			{
				flag = false;
			}
			if (this.dirty)
			{
				if (flag)
				{
					if (this.BufferRender == null)
					{
						this.BufferRender = new CommandBuffer();
						this.BufferRender.name = "VolumetricRender";
					}
					this.bufferBuilt = true;
					cameraBuffer = this.BufferRender;
					this.BufferRender.Clear();
				}
				cameraBuffer.SetGlobalTexture(HxVolumetricCamera.ShadowMapTexturePID, BuiltinRenderTextureType.CurrentActive);
				this.SetColors(cameraBuffer, num2);
				if (flag)
				{
					if (HxVolumetricCamera.Active.TransparencySupport)
					{
						cameraBuffer.SetRenderTarget(HxVolumetricCamera.VolumetricTransparency[(int)HxVolumetricCamera.Active.compatibleTBuffer()], HxVolumetricCamera.ScaledDepthTextureRTID[(int)HxVolumetricCamera.Active.resolution]);
					}
					else
					{
						cameraBuffer.SetRenderTarget(HxVolumetricCamera.VolumetricTextureRTID, HxVolumetricCamera.ScaledDepthTextureRTID[(int)HxVolumetricCamera.Active.resolution]);
					}
				}
				if (this.CustomFogHeightEnabled ? this.FogHeightEnabled : HxVolumetricCamera.Active.FogHeightEnabled)
				{
					cameraBuffer.SetGlobalVector(HxVolumetricLight.FogHeightsPID, new Vector3((this.CustomFogHeight ? this.FogHeight : HxVolumetricCamera.Active.FogHeight) - (this.CustomFogTransitionSize ? this.FogTransitionSize : HxVolumetricCamera.Active.FogTransitionSize), this.CustomFogHeight ? this.FogHeight : HxVolumetricCamera.Active.FogHeight, this.CustomAboveFogPercent ? this.AboveFogPercent : HxVolumetricCamera.Active.AboveFogPercent));
				}
				this.LoadVolumeDataBounds();
				this.LoadVolumeDateIntoBuffer(cameraBuffer);
				float fogDensity = this.GetFogDensity();
				cameraBuffer.SetGlobalMatrix(HxVolumetricLight.VolumetricMVPPID, HxVolumetricCamera.Active.MatrixVP * this.LightMatrix);
				cameraBuffer.SetGlobalMatrix(HxVolumetricLight.VolumetricMVPID, HxVolumetricCamera.Active.MatrixV * this.LightMatrix);
				float num3 = this.CustomMieScatter ? this.MieScattering : HxVolumetricCamera.Active.MieScattering;
				Vector4 vector = new Vector4(0.07957747f, 1f - num3 * num3, 1f + num3 * num3, 2f * num3);
				cameraBuffer.SetGlobalVector(HxVolumetricLight.PhasePID, vector);
				cameraBuffer.SetGlobalVector(HxVolumetricLight._CustomLightPositionPID, base.transform.position);
				cameraBuffer.SetGlobalVector(HxVolumetricLight._LightParamsPID, new Vector4(this.CustomStrength ? this.Strength : this.LightShadowStrength(), 1f / this.LightRange(), this.LightRange(), this.CustomIntensity ? this.Intensity : this.LightIntensity()));
				cameraBuffer.SetGlobalVector(HxVolumetricLight.DensityPID, new Vector4(fogDensity, (float)this.GetSampleCount(flag), 0f, this.CustomExtinction ? this.Extinction : HxVolumetricCamera.Active.Extinction));
				if (flag)
				{
					Graphics.DrawMesh(HxVolumetricCamera.SpotLightMesh, this.LightMatrix, HxVolumetricCamera.ShadowMaterial, 0, HxVolumetricCamera.ActiveCamera, 0, null, ShadowCastingMode.ShadowsOnly);
				}
				float x = (1f - this.LightRange() / this.LightNearPlane()) / this.LightRange();
				float y = this.LightRange() / this.LightNearPlane() / this.LightRange();
				cameraBuffer.SetGlobalVector(HxVolumetricLight.ShadowBiasPID, new Vector4(x, y, (1f - (this.CustomStrength ? this.Strength : this.LightShadowStrength())) * vector.x * (vector.y / Mathf.Pow(vector.z - vector.w * -1f, 1.5f)), this.LightShadowBias()));
				Vector3 vector2 = this.CustomNoiseScale ? this.NoiseScale : HxVolumetricCamera.Active.NoiseScale;
				vector2 = new Vector3(1f / vector2.x, 1f / vector2.y, 1f / vector2.z) / 32f;
				cameraBuffer.SetGlobalFloat(HxVolumetricLight.hxNearPlanePID, this.NearPlane);
				cameraBuffer.SetGlobalVector(HxVolumetricLight.NoiseScalePID, vector2);
				if (!this.OffsetUpdated)
				{
					this.OffsetUpdated = true;
					this.Offset += this.NoiseVelocity * Time.deltaTime;
				}
				cameraBuffer.SetGlobalVector(HxVolumetricLight.NoiseOffsetPID, this.CustomNoiseVelocity ? this.Offset : HxVolumetricCamera.Active.Offset);
				Vector3 forward = base.transform.forward;
				cameraBuffer.SetGlobalVector(HxVolumetricLight._SpotLightParamsPID, new Vector4(forward.x, forward.y, forward.z, (this.LightSpotAngle() + 0.01f) / 2f * 0.017453292f));
				if (HxVolumetricLight.propertyBlock == null)
				{
					HxVolumetricLight.propertyBlock = new MaterialPropertyBlock();
				}
				HxVolumetricLight.propertyBlock.SetTexture(HxVolumetricLight._LightTexture0PID, (this.LightCookie() == null) ? HxVolumetricCamera.Active.SpotLightCookie : this.LightCookie());
				HxVolumetricLight.propertyBlock.SetTexture(HxVolumetricLight._hxProjectorFalloffTexturePID, this.LightFalloffTexture());
				cameraBuffer.SetGlobalVector("TopFrustumNormal", this.TopFrustumNormal);
				cameraBuffer.SetGlobalVector("BottomFrustumNormal", this.BottomFrustumNormal);
				cameraBuffer.SetGlobalVector("LeftFrustumNormal", this.LeftFrustumNormal);
				cameraBuffer.SetGlobalVector("RightFrustumNormal", this.RightFrustumNormal);
				int mid = this.MID(flag, HxVolumetricCamera.ActiveFull());
				if (this.CustomNoiseEnabled ? this.NoiseEnabled : HxVolumetricCamera.Active.NoiseEnabled)
				{
					if (HxVolumetricLight.propertyBlock == null)
					{
						HxVolumetricLight.propertyBlock = new MaterialPropertyBlock();
					}
					Texture3D noiseTexture = this.GetNoiseTexture();
					if (noiseTexture != null)
					{
						HxVolumetricLight.propertyBlock.SetFloat("hxNoiseContrast", this.getContrast());
						HxVolumetricLight.propertyBlock.SetTexture("NoiseTexture3D", noiseTexture);
					}
				}
				cameraBuffer.DrawMesh(HxVolumetricCamera.SpotLightMesh, this.LightMatrix, HxVolumetricCamera.GetSpotMaterial(mid), 0, (this.lastBounds.SqrDistance(HxVolumetricCamera.Active.transform.position) < HxVolumetricCamera.ActiveCamera.nearClipPlane * 2f * (HxVolumetricCamera.ActiveCamera.nearClipPlane * 2f)) ? 0 : 1, HxVolumetricLight.propertyBlock);
				if (flag)
				{
					this.myLight.AddCommandBuffer(LightEvent.AfterShadowMap, this.BufferRender);
				}
			}
		}
	}

	private void BuildProjectorLightBuffer(CommandBuffer cameraBuffer)
	{
		float num = Mathf.Sqrt(this.lastBounds.SqrDistance(HxVolumetricCamera.ActiveCamera.transform.position));
		float num2 = this.CalcLightInstensityDistance(num);
		if (num2 > 0f && this.dirty)
		{
			this.SetColors(cameraBuffer, num2);
			cameraBuffer.SetGlobalTexture(HxVolumetricCamera.ShadowMapTexturePID, BuiltinRenderTextureType.CurrentActive);
			if (this.CustomFogHeightEnabled ? this.FogHeightEnabled : HxVolumetricCamera.Active.FogHeightEnabled)
			{
				cameraBuffer.SetGlobalVector(HxVolumetricLight.FogHeightsPID, new Vector3((this.CustomFogHeight ? this.FogHeight : HxVolumetricCamera.Active.FogHeight) - (this.CustomFogTransitionSize ? this.FogTransitionSize : HxVolumetricCamera.Active.FogTransitionSize), this.CustomFogHeight ? this.FogHeight : HxVolumetricCamera.Active.FogHeight, this.CustomAboveFogPercent ? this.AboveFogPercent : HxVolumetricCamera.Active.AboveFogPercent));
			}
			this.LoadVolumeDataBounds();
			this.LoadVolumeDateIntoBuffer(cameraBuffer);
			float fogDensity = this.GetFogDensity();
			cameraBuffer.SetGlobalMatrix(HxVolumetricLight.VolumetricMVPPID, HxVolumetricCamera.Active.MatrixVP * this.LightMatrix);
			cameraBuffer.SetGlobalMatrix(HxVolumetricLight.VolumetricMVPID, HxVolumetricCamera.Active.MatrixV * this.LightMatrix);
			float num3 = this.CustomMieScatter ? this.MieScattering : HxVolumetricCamera.Active.MieScattering;
			Vector4 value = new Vector4(0.07957747f, 1f - num3 * num3, 1f + num3 * num3, 2f * num3);
			cameraBuffer.SetGlobalVector(HxVolumetricLight.PhasePID, value);
			cameraBuffer.SetGlobalVector(HxVolumetricLight._CustomLightPositionPID, base.transform.position);
			cameraBuffer.SetGlobalVector(HxVolumetricLight._LightParamsPID, new Vector4(this.CustomStrength ? this.Strength : this.LightShadowStrength(), 1f / this.LightRange(), this.LightRange(), this.CustomIntensity ? this.Intensity : this.LightIntensity()));
			cameraBuffer.SetGlobalVector(HxVolumetricLight.DensityPID, new Vector4(fogDensity, (float)this.GetSampleCount(false), 0f, this.CustomExtinction ? this.Extinction : HxVolumetricCamera.Active.Extinction));
			Vector3 vector = this.CustomNoiseScale ? this.NoiseScale : HxVolumetricCamera.Active.NoiseScale;
			vector = new Vector3(1f / vector.x, 1f / vector.y, 1f / vector.z) / 32f;
			cameraBuffer.SetGlobalFloat(HxVolumetricLight.hxNearPlanePID, this.NearPlane);
			cameraBuffer.SetGlobalVector(HxVolumetricLight.NoiseScalePID, vector);
			if (!this.OffsetUpdated)
			{
				this.OffsetUpdated = true;
				this.Offset += this.NoiseVelocity * Time.deltaTime;
			}
			cameraBuffer.SetGlobalVector(HxVolumetricLight.NoiseOffsetPID, this.CustomNoiseVelocity ? this.Offset : HxVolumetricCamera.Active.Offset);
			Vector3 forward = base.transform.forward;
			cameraBuffer.SetGlobalVector(HxVolumetricLight._SpotLightParamsPID, new Vector4(forward.x, forward.y, forward.z, (this.LightSpotAngle() + 0.01f) / 2f * 0.017453292f));
			if (HxVolumetricLight.propertyBlock == null)
			{
				HxVolumetricLight.propertyBlock = new MaterialPropertyBlock();
			}
			HxVolumetricLight.propertyBlock.SetTexture(HxVolumetricLight._hxProjectorTexturePID, this.myProjector.material.GetTexture(HxVolumetricLight._hxProjectorTexturePID));
			HxVolumetricLight.propertyBlock.SetTexture(HxVolumetricLight._hxProjectorFalloffTexturePID, (this.LightFalloff != null) ? this.LightFalloff : this.myProjector.material.GetTexture(HxVolumetricLight._hxProjectorFalloffTexturePID));
			cameraBuffer.SetGlobalVector("TopFrustumNormal", this.TopFrustumNormal);
			cameraBuffer.SetGlobalVector("BottomFrustumNormal", this.BottomFrustumNormal);
			cameraBuffer.SetGlobalVector("LeftFrustumNormal", this.LeftFrustumNormal);
			cameraBuffer.SetGlobalVector("RightFrustumNormal", this.RightFrustumNormal);
			cameraBuffer.SetGlobalFloat("OrthoLight", (float)(this.myProjector.orthographic ? 1 : 0));
			cameraBuffer.SetGlobalVector("UpFrustumOffset", base.transform.up * (this.myProjector.orthographic ? this.myProjector.orthographicSize : 0f));
			cameraBuffer.SetGlobalVector("RightFrustumOffset", base.transform.right * (this.myProjector.orthographic ? (this.myProjector.orthographicSize * this.myProjector.aspectRatio) : 0f));
			int mid = this.MID(false, HxVolumetricCamera.ActiveFull());
			if (this.CustomNoiseEnabled ? this.NoiseEnabled : HxVolumetricCamera.Active.NoiseEnabled)
			{
				if (HxVolumetricLight.propertyBlock == null)
				{
					HxVolumetricLight.propertyBlock = new MaterialPropertyBlock();
				}
				Texture3D noiseTexture = this.GetNoiseTexture();
				if (noiseTexture != null)
				{
					HxVolumetricLight.propertyBlock.SetFloat("hxNoiseContrast", this.getContrast());
					HxVolumetricLight.propertyBlock.SetTexture("NoiseTexture3D", noiseTexture);
				}
			}
			if (this.myProjector.orthographic)
			{
				cameraBuffer.DrawMesh(HxVolumetricCamera.OrthoProjectorMesh, this.LightMatrix, HxVolumetricCamera.GetProjectorMaterial(mid), 0, (num < HxVolumetricCamera.ActiveCamera.nearClipPlane) ? 0 : 1, HxVolumetricLight.propertyBlock);
				return;
			}
			cameraBuffer.DrawMesh(HxVolumetricCamera.SpotLightMesh, this.LightMatrix, HxVolumetricCamera.GetProjectorMaterial(mid), 0, (num < HxVolumetricCamera.ActiveCamera.nearClipPlane) ? 0 : 1, HxVolumetricLight.propertyBlock);
		}
	}

	private void SetColors(CommandBuffer buffer, float distanceLerp)
	{
		Vector4 vector = (this.CustomColor ? this.Color : this.LightColor()) * (this.CustomIntensity ? this.Intensity : this.LightIntensity()) * distanceLerp;
		if (this.CustomTintMode ? (this.TintMode == HxVolumetricCamera.HxTintMode.Off) : (HxVolumetricCamera.Active.TintMode == HxVolumetricCamera.HxTintMode.Off))
		{
			buffer.SetGlobalVector(HxVolumetricLight.LightColourPID, vector);
			buffer.SetGlobalVector(HxVolumetricLight.LightColour2PID, vector);
			return;
		}
		if (this.CustomTintMode ? (this.TintMode == HxVolumetricCamera.HxTintMode.Color) : (HxVolumetricCamera.Active.TintMode == HxVolumetricCamera.HxTintMode.Color))
		{
			buffer.SetGlobalVector(HxVolumetricLight.LightColourPID, this.CalcTintColor(vector));
			buffer.SetGlobalVector(HxVolumetricLight.LightColour2PID, this.CalcTintColor(vector));
			return;
		}
		if (this.CustomTintMode ? (this.TintMode == HxVolumetricCamera.HxTintMode.Edge) : (HxVolumetricCamera.Active.TintMode == HxVolumetricCamera.HxTintMode.Edge))
		{
			buffer.SetGlobalVector(HxVolumetricLight.LightColourPID, vector);
			buffer.SetGlobalVector(HxVolumetricLight.LightColour2PID, this.CalcTintColor(vector));
			buffer.SetGlobalFloat("TintPercent", 1f / (this.CustomTintGradient ? this.TintGradient : HxVolumetricCamera.Active.TintGradient) / 2f);
			return;
		}
		if (this.CustomTintMode ? (this.TintMode == HxVolumetricCamera.HxTintMode.Gradient) : (HxVolumetricCamera.Active.TintMode == HxVolumetricCamera.HxTintMode.Gradient))
		{
			buffer.SetGlobalVector(HxVolumetricLight.LightColourPID, this.CalcTintColor(vector));
			buffer.SetGlobalVector(HxVolumetricLight.LightColour2PID, this.CalcTintColorEdge(vector));
			buffer.SetGlobalFloat("TintPercent", 1f / (this.CustomTintGradient ? this.TintGradient : HxVolumetricCamera.Active.TintGradient) / 2f);
		}
	}

	private void SetColors(CommandBuffer buffer)
	{
		Vector4 vector = (this.CustomColor ? this.Color : this.LightColor()) * (this.CustomIntensity ? this.Intensity : this.LightIntensity());
		if (HxVolumetricCamera.Active.TintMode == HxVolumetricCamera.HxTintMode.Off)
		{
			buffer.SetGlobalVector(HxVolumetricLight.LightColourPID, vector);
			buffer.SetGlobalVector(HxVolumetricLight.LightColour2PID, vector);
			return;
		}
		if (HxVolumetricCamera.Active.TintMode == HxVolumetricCamera.HxTintMode.Color)
		{
			buffer.SetGlobalVector(HxVolumetricLight.LightColourPID, this.CalcTintColor(vector));
			buffer.SetGlobalVector(HxVolumetricLight.LightColour2PID, this.CalcTintColor(vector));
			return;
		}
		if (HxVolumetricCamera.Active.TintMode == HxVolumetricCamera.HxTintMode.Edge)
		{
			buffer.SetGlobalVector(HxVolumetricLight.LightColourPID, vector);
			buffer.SetGlobalVector(HxVolumetricLight.LightColour2PID, this.CalcTintColor(vector));
			buffer.SetGlobalFloat("TintPercent", 1f / HxVolumetricCamera.Active.TintGradient / 2f);
			return;
		}
		if (HxVolumetricCamera.Active.TintMode == HxVolumetricCamera.HxTintMode.Gradient)
		{
			buffer.SetGlobalVector(HxVolumetricLight.LightColourPID, this.CalcTintColor(vector));
			buffer.SetGlobalVector(HxVolumetricLight.LightColour2PID, this.CalcTintColorEdge(vector));
			buffer.SetGlobalFloat("TintPercent", 1f / HxVolumetricCamera.Active.TintGradient / 2f);
		}
	}

	private Vector3 CalcTintColor(Vector4 c)
	{
		Vector3 a = new Vector3(c.x, c.y, c.z);
		float magnitude = a.magnitude;
		if (this.CustomTintColor)
		{
			a += new Vector3(((QualitySettings.activeColorSpace == ColorSpace.Gamma) ? this.TintColor : this.TintColor.linear).r, ((QualitySettings.activeColorSpace == ColorSpace.Gamma) ? this.TintColor : this.TintColor.linear).g, ((QualitySettings.activeColorSpace == ColorSpace.Gamma) ? this.TintColor : this.TintColor.linear).b) * (this.CustomTintIntensity ? this.TintIntensity : HxVolumetricCamera.Active.TintIntensity);
		}
		else
		{
			a += HxVolumetricCamera.Active.CurrentTint;
		}
		return a.normalized * magnitude;
	}

	private Vector3 CalcTintColorEdge(Vector4 c)
	{
		Vector3 a = new Vector3(c.x, c.y, c.z);
		float magnitude = a.magnitude;
		if (this.CustomTintColor2)
		{
			a += new Vector3(((QualitySettings.activeColorSpace == ColorSpace.Gamma) ? this.TintColor2 : this.TintColor2.linear).r, ((QualitySettings.activeColorSpace == ColorSpace.Gamma) ? this.TintColor2 : this.TintColor2.linear).g, ((QualitySettings.activeColorSpace == ColorSpace.Gamma) ? this.TintColor2 : this.TintColor2.linear).b) * (this.CustomTintIntensity ? this.TintIntensity : HxVolumetricCamera.Active.TintIntensity);
		}
		else
		{
			a += HxVolumetricCamera.Active.CurrentTintEdge;
		}
		return a.normalized * magnitude;
	}

	private void BuildPointBuffer(CommandBuffer cameraBuffer)
	{
		float num = Mathf.Max(Vector3.Distance(HxVolumetricCamera.Active.transform.position, base.transform.position) - this.LightRange(), 0f);
		float num2 = this.CalcLightInstensityDistance(num);
		if (num2 > 0f)
		{
			bool flag = this.LightShadow() != LightShadows.None && this.Shadows;
			if (flag && num >= QualitySettings.shadowDistance - HxVolumetricLight.ShadowDistanceExtra)
			{
				flag = false;
			}
			if (this.dirty)
			{
				if (flag)
				{
					if (this.BufferRender == null)
					{
						this.BufferRender = new CommandBuffer();
						this.BufferRender.name = "VolumetricRender";
					}
					this.bufferBuilt = true;
					cameraBuffer = this.BufferRender;
					this.BufferRender.Clear();
				}
				cameraBuffer.SetGlobalTexture(HxVolumetricCamera.ShadowMapTexturePID, BuiltinRenderTextureType.CurrentActive);
				this.SetColors(cameraBuffer, num2);
				if (this.CustomFogHeightEnabled ? this.FogHeightEnabled : HxVolumetricCamera.Active.FogHeightEnabled)
				{
					cameraBuffer.SetGlobalVector(HxVolumetricLight.FogHeightsPID, new Vector3((this.CustomFogHeight ? this.FogHeight : HxVolumetricCamera.Active.FogHeight) - (this.CustomFogTransitionSize ? this.FogTransitionSize : HxVolumetricCamera.Active.FogTransitionSize), this.CustomFogHeight ? this.FogHeight : HxVolumetricCamera.Active.FogHeight, this.CustomAboveFogPercent ? this.AboveFogPercent : HxVolumetricCamera.Active.AboveFogPercent));
				}
				if (flag)
				{
					if (HxVolumetricCamera.Active.TransparencySupport)
					{
						cameraBuffer.SetRenderTarget(HxVolumetricCamera.VolumetricTransparency[(int)HxVolumetricCamera.Active.compatibleTBuffer()], HxVolumetricCamera.ScaledDepthTextureRTID[(int)HxVolumetricCamera.Active.resolution]);
					}
					else
					{
						cameraBuffer.SetRenderTarget(HxVolumetricCamera.VolumetricTextureRTID, HxVolumetricCamera.ScaledDepthTextureRTID[(int)HxVolumetricCamera.Active.resolution]);
					}
				}
				this.LoadVolumeDataBounds();
				this.LoadVolumeDateIntoBuffer(cameraBuffer);
				float fogDensity = this.GetFogDensity();
				cameraBuffer.SetGlobalMatrix(HxVolumetricLight.VolumetricMVPPID, HxVolumetricCamera.Active.MatrixVP * this.LightMatrix);
				cameraBuffer.SetGlobalMatrix(HxVolumetricLight.VolumetricMVPID, HxVolumetricCamera.Active.MatrixV * this.LightMatrix);
				float num3 = this.CustomMieScatter ? this.MieScattering : HxVolumetricCamera.Active.MieScattering;
				Vector4 vector = new Vector4(0.07957747f, 1f - num3 * num3, 1f + num3 * num3, 2f * num3);
				cameraBuffer.SetGlobalVector(HxVolumetricLight.PhasePID, vector);
				cameraBuffer.SetGlobalVector(HxVolumetricLight._CustomLightPositionPID, base.transform.position);
				cameraBuffer.SetGlobalVector(HxVolumetricLight._LightParamsPID, new Vector4(this.CustomStrength ? this.Strength : this.LightShadowStrength(), 1f / this.LightRange(), this.LightRange(), this.CustomIntensity ? this.Intensity : this.LightIntensity()));
				cameraBuffer.SetGlobalVector(HxVolumetricLight.DensityPID, new Vector4(fogDensity, (float)this.GetSampleCount(flag), 0f, this.CustomExtinction ? this.Extinction : HxVolumetricCamera.Active.Extinction));
				cameraBuffer.SetGlobalVector(HxVolumetricLight.ShadowBiasPID, new Vector3(this.LightShadowBias(), this.LightNearPlane(), (1f - (this.CustomStrength ? this.Strength : this.LightShadowStrength())) * vector.x * (vector.y / Mathf.Pow(vector.z - vector.w * -1f, 1.5f))));
				Vector3 vector2 = this.CustomNoiseScale ? this.NoiseScale : HxVolumetricCamera.Active.NoiseScale;
				vector2 = new Vector3(1f / vector2.x, 1f / vector2.y, 1f / vector2.z) / 32f;
				cameraBuffer.SetGlobalVector(HxVolumetricLight.NoiseScalePID, vector2);
				if (!this.OffsetUpdated)
				{
					this.OffsetUpdated = true;
					this.Offset += this.NoiseVelocity * Time.deltaTime;
				}
				cameraBuffer.SetGlobalVector(HxVolumetricLight.NoiseOffsetPID, this.CustomNoiseVelocity ? this.Offset : HxVolumetricCamera.Active.Offset);
				if (flag && HxVolumetricCamera.Active.ShadowFix)
				{
					Graphics.DrawMesh(HxVolumetricCamera.BoxMesh, this.LightMatrix, HxVolumetricCamera.ShadowMaterial, 0, HxVolumetricCamera.ActiveCamera, 0, null, ShadowCastingMode.ShadowsOnly);
				}
				int shaderPass = (num <= this.LightRange() + this.LightRange() * 0.09f + HxVolumetricCamera.ActiveCamera.nearClipPlane * 2f) ? 0 : 1;
				if (HxVolumetricLight.propertyBlock == null)
				{
					HxVolumetricLight.propertyBlock = new MaterialPropertyBlock();
				}
				int mid = this.MID(flag, HxVolumetricCamera.ActiveFull());
				if (this.CustomNoiseEnabled ? this.NoiseEnabled : HxVolumetricCamera.Active.NoiseEnabled)
				{
					Texture3D noiseTexture = this.GetNoiseTexture();
					if (noiseTexture != null)
					{
						HxVolumetricLight.propertyBlock.SetFloat("hxNoiseContrast", this.getContrast());
						HxVolumetricLight.propertyBlock.SetTexture("NoiseTexture3D", noiseTexture);
					}
				}
				HxVolumetricLight.propertyBlock.SetTexture(HxVolumetricLight._hxProjectorFalloffTexturePID, this.LightFalloffTexture());
				if (this.LightCookie() != null)
				{
					HxVolumetricLight.propertyBlock.SetTexture(Shader.PropertyToID("PointCookieTexture"), this.LightCookie());
					cameraBuffer.DrawMesh(HxVolumetricCamera.SphereMesh, this.LightMatrix, HxVolumetricCamera.GetPointMaterial(mid), 0, shaderPass, HxVolumetricLight.propertyBlock);
				}
				else
				{
					cameraBuffer.DrawMesh(HxVolumetricCamera.SphereMesh, this.LightMatrix, HxVolumetricCamera.GetPointMaterial(mid), 0, shaderPass, HxVolumetricLight.propertyBlock);
				}
				if (flag)
				{
					this.myLight.AddCommandBuffer(LightEvent.AfterShadowMap, this.BufferRender);
				}
			}
		}
	}

	public int MID(bool RenderShadows, bool full)
	{
		int num = 0;
		if (RenderShadows)
		{
			num++;
		}
		if (this.LightCookie() != null)
		{
			num += 2;
		}
		if (this.CustomNoiseEnabled ? this.NoiseEnabled : HxVolumetricCamera.Active.NoiseEnabled)
		{
			num += 4;
		}
		if (this.CustomFogHeight ? this.FogHeightEnabled : HxVolumetricCamera.Active.FogHeightEnabled)
		{
			num += 8;
		}
		if (HxVolumetricCamera.Active.renderDensityParticleCheck())
		{
			num += 16;
		}
		if (HxVolumetricCamera.Active.TransparencySupport)
		{
			num += 32;
		}
		if (full)
		{
			num += 64;
		}
		return num;
	}

	private void Update()
	{
		this.OffsetUpdated = false;
	}

	private float GetFogDensity()
	{
		if (this.CustomDensity)
		{
			return this.Density + this.ExtraDensity;
		}
		return HxVolumetricCamera.Active.Density + this.ExtraDensity;
	}

	private Texture3D GetNoiseTexture()
	{
		if (!this.CustomNoiseTexture)
		{
			return HxVolumetricCamera.Active.NoiseTexture3D;
		}
		if (this.NoiseTexture3D == null)
		{
			return HxVolumetricCamera.Active.NoiseTexture3D;
		}
		return this.NoiseTexture3D;
	}

	private int GetSampleCount(bool RenderShadows)
	{
		int b = this.CustomSampleCount ? this.SampleCount : ((this.GetLightType() != LightType.Directional) ? HxVolumetricCamera.Active.SampleCount : HxVolumetricCamera.Active.DirectionalSampleCount);
		return Mathf.Max(2, b);
	}

	public static Vector3 ClosestPointOnLine(Vector3 vA, Vector3 vB, Vector3 vPoint)
	{
		Vector3 rhs = vPoint - vA;
		Vector3 normalized = (vB - vA).normalized;
		float num = Vector3.Distance(vA, vB);
		float num2 = Vector3.Dot(normalized, rhs);
		if (num2 <= 0f)
		{
			return vA;
		}
		if (num2 >= num)
		{
			return vB;
		}
		Vector3 b = normalized * num2;
		return vA + b;
	}

	private float ClosestDistanceToCone(Vector3 Point)
	{
		Vector3 vector = base.transform.forward * this.LightRange();
		Vector3 vector2 = base.transform.position + vector;
		float num = Vector3.Dot(base.transform.forward, Point - vector2);
		if (num == 0f)
		{
			return 0f;
		}
		Vector3 vector3 = Point - num * base.transform.forward;
		float num2 = Mathf.Tan(this.LightSpotAngle() / 2f * 0.017453292f) * this.LightRange();
		Vector3 vector4 = vector3 - vector2;
		if (num > 0f)
		{
			vector3 = vector2 + vector4.normalized * Mathf.Min(vector4.magnitude, num2);
			return Vector3.Distance(Point, vector3);
		}
		float num3 = 0.017453292f * this.LightSpotAngle();
		if (Mathf.Abs(Mathf.Acos(Vector3.Dot(Point - base.transform.position, -vector) / ((Point - base.transform.position).magnitude * this.LightRange())) - num3) >= 1.5707964f)
		{
			return 0f;
		}
		vector3 = vector2 + vector4.normalized * num2;
		vector3 = HxVolumetricLight.ClosestPointOnLine(vector3, base.transform.position, Point);
		return Vector3.Distance(Point, vector3);
	}

	private void UpdateLightMatrix()
	{
		this.LastRange = this.LightRange();
		this.LastSpotAngle = this.LightSpotAngle();
		this.lastType = this.GetLightType();
		if (this.GetLightType() == LightType.Point)
		{
			this.LightMatrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, new Vector3(this.LightRange() * 2f, this.LightRange() * 2f, this.LightRange() * 2f));
			this.matrixReconstruct = false;
		}
		else if (this.GetLightType() == LightType.Spot)
		{
			float num = Mathf.Tan(this.LightSpotAngle() / 2f * 0.017453292f) * this.LightRange();
			this.LightMatrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, new Vector3(num * 2f, num * 2f, this.LightRange()));
		}
		else if (this.GetLightType() == LightType.Area && this.myProjector != null)
		{
			if (this.myProjector.orthographic)
			{
				this.LightMatrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, new Vector3(this.myProjector.orthographicSize * this.myProjector.aspectRatio * 2f, this.myProjector.orthographicSize * 2f, this.LightRange()));
			}
			else
			{
				float num2 = Mathf.Tan(this.LightSpotAngle() / 2f * 0.017453292f) * this.LightRange();
				this.LightMatrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, new Vector3(num2 * 2f, num2 * 2f, this.LightRange()));
			}
		}
		base.transform.hasChanged = false;
		this.matrixReconstruct = false;
	}

	private void CheckLightType()
	{
		if (this.lastType != this.GetLightType())
		{
			if (this.lastType == LightType.Directional)
			{
				this.octreeNode = HxVolumetricCamera.AddLightOctree(this, this.minBounds, this.maxBounds);
				HxVolumetricCamera.ActiveDirectionalLights.Remove(this);
			}
			else if (this.GetLightType() == LightType.Directional && this.lastType != LightType.Directional)
			{
				HxVolumetricCamera.RemoveLightOctree(this);
				this.octreeNode = null;
				HxVolumetricCamera.ActiveDirectionalLights.Add(this);
			}
		}
		this.lastType = this.GetLightType();
	}

	private void LoadVolumeData()
	{
		if (this.ShaderModel4())
		{
			int num = 0;
			for (int i = 0; i < HxVolumetricCamera.ActiveVolumes.Count; i++)
			{
				if (HxVolumetricCamera.ActiveVolumes[i].enabled && (HxVolumetricCamera.ActiveVolumes[i].BlendMode != HxDensityVolume.DensityBlendMode.Add || HxVolumetricCamera.ActiveVolumes[i].BlendMode != HxDensityVolume.DensityBlendMode.Sub) && HxVolumetricCamera.ActiveVolumes[i].Density != 0f)
				{
					HxVolumetricLight.VolumeMatrixArrays[num] = HxVolumetricCamera.ActiveVolumes[i].ToLocalSpace;
					num++;
				}
				if (num >= 50)
				{
					break;
				}
			}
			if (num < 49)
			{
				HxVolumetricLight.VolumeSettingsArrays[num] = new Vector2(0f, -1f);
				return;
			}
		}
		else
		{
			int num2 = 0;
			for (int j = 0; j < HxVolumetricCamera.ActiveVolumes.Count; j++)
			{
				if (HxVolumetricCamera.ActiveVolumes[j].enabled && (HxVolumetricCamera.ActiveVolumes[j].BlendMode != HxDensityVolume.DensityBlendMode.Add || HxVolumetricCamera.ActiveVolumes[j].BlendMode != HxDensityVolume.DensityBlendMode.Sub) && HxVolumetricCamera.ActiveVolumes[j].Density != 0f)
				{
					HxVolumetricLight.VolumeMatrixArraysOld[num2] = HxVolumetricCamera.ActiveVolumes[j].ToLocalSpace;
					num2++;
				}
				if (num2 >= 10)
				{
					break;
				}
			}
			if (num2 < 9)
			{
				HxVolumetricLight.VolumeSettingsArraysOld[num2] = new Vector2(0f, -1f);
			}
		}
	}

	private bool BoundsIntersect(HxDensityVolume vol)
	{
		return this.minBounds.x <= vol.maxBounds.x && this.maxBounds.x >= vol.minBounds.x && this.minBounds.y <= vol.maxBounds.y && this.maxBounds.y >= vol.minBounds.y && this.minBounds.z <= vol.maxBounds.z && this.maxBounds.z >= vol.minBounds.z;
	}

	private void LoadVolumeDataBounds()
	{
		if (this.ShaderModel4())
		{
			int num = 0;
			for (int i = 0; i < HxVolumetricCamera.ActiveVolumes.Count; i++)
			{
				if (HxVolumetricCamera.ActiveVolumes[i].enabled && (HxVolumetricCamera.ActiveVolumes[i].BlendMode != HxDensityVolume.DensityBlendMode.Add || HxVolumetricCamera.ActiveVolumes[i].BlendMode != HxDensityVolume.DensityBlendMode.Sub) && HxVolumetricCamera.ActiveVolumes[i].Density != 0f && this.BoundsIntersect(HxVolumetricCamera.ActiveVolumes[i]))
				{
					HxVolumetricLight.VolumeMatrixArrays[num] = HxVolumetricCamera.ActiveVolumes[i].ToLocalSpace;
					num++;
				}
				if (num >= 50)
				{
					break;
				}
			}
			if (num < 49)
			{
				HxVolumetricLight.VolumeSettingsArrays[num] = new Vector2(0f, -1f);
				return;
			}
		}
		else
		{
			int num2 = 0;
			for (int j = 0; j < HxVolumetricCamera.ActiveVolumes.Count; j++)
			{
				if (HxVolumetricCamera.ActiveVolumes[j].enabled && (HxVolumetricCamera.ActiveVolumes[j].BlendMode != HxDensityVolume.DensityBlendMode.Add || HxVolumetricCamera.ActiveVolumes[j].BlendMode != HxDensityVolume.DensityBlendMode.Sub) && HxVolumetricCamera.ActiveVolumes[j].Density != 0f && this.BoundsIntersect(HxVolumetricCamera.ActiveVolumes[j]))
				{
					HxVolumetricLight.VolumeMatrixArraysOld[num2] = HxVolumetricCamera.ActiveVolumes[j].ToLocalSpace;
					num2++;
				}
				if (num2 >= 10)
				{
					break;
				}
			}
			if (num2 < 9)
			{
				HxVolumetricLight.VolumeSettingsArraysOld[num2] = new Vector2(0f, -1f);
			}
		}
	}

	private Vector4 NormalOfTriangle(Vector3 a, Vector3 b, Vector3 c)
	{
		Vector3 vector = Vector3.Cross(b - a, c - a);
		vector.Normalize();
		return new Vector4(vector.x, vector.y, vector.z, 0f);
	}

	private void DrawIntersect()
	{
		Vector3 forward = base.transform.forward;
		Vector3 forward2 = HxVolumetricCamera.Active.transform.forward;
		Vector3 position = base.transform.position;
		Vector3 position2 = HxVolumetricCamera.Active.transform.position;
		Vector3 vector;
		vector.x = Vector3.Dot(forward, forward2);
		Vector3 vector2;
		vector2.x = Vector3.Dot(position + forward * this.LightRange() - position2, forward);
		vector.y = Vector3.Dot(this.BottomFrustumNormal, forward2);
		vector2.y = Vector3.Dot(position - position2, this.BottomFrustumNormal);
		vector.z = Vector3.Dot(this.LeftFrustumNormal, forward2);
		vector2.z = Vector3.Dot(position - position2, this.LeftFrustumNormal);
		Vector3 vector3;
		vector3.x = Vector3.Dot(-forward, forward2);
		Vector3 vector4;
		vector4.x = Vector3.Dot(position + forward * this.NearPlane - position2, -forward);
		vector3.y = Vector3.Dot(this.TopFrustumNormal, forward2);
		vector4.y = Vector3.Dot(position - position2, this.TopFrustumNormal);
		vector3.z = Vector3.Dot(this.RightFrustumNormal, forward2);
		vector4.z = Vector3.Dot(position - position2, this.RightFrustumNormal);
		float num = 0f;
		float num2 = 100000f;
		if (vector3.x > 0f)
		{
			num2 = Mathf.Min(num2, vector4.x / vector3.x);
		}
		else if (vector3.x < 0f)
		{
			num = Mathf.Max(num, vector4.x / vector3.x);
		}
		if (vector3.y > 0f)
		{
			num2 = Mathf.Min(num2, vector4.y / vector3.y);
		}
		else if (vector3.y < 0f)
		{
			num = Mathf.Max(num, vector4.y / vector3.y);
		}
		if (vector3.z > 0f)
		{
			num2 = Mathf.Min(num2, vector4.z / vector3.z);
		}
		else if (vector3.z < 0f)
		{
			num = Mathf.Max(num, vector4.z / vector3.z);
		}
		if (vector.x > 0f)
		{
			num2 = Mathf.Min(num2, vector2.x / vector.x);
		}
		else if (vector.x < 0f)
		{
			num = Mathf.Max(num, vector2.x / vector.x);
		}
		if (vector.y > 0f)
		{
			num2 = Mathf.Min(num2, vector2.y / vector.y);
		}
		else if (vector.y < 0f)
		{
			num = Mathf.Max(num, vector2.y / vector.y);
		}
		if (vector.z > 0f)
		{
			num2 = Mathf.Min(num2, vector2.z / vector.z);
		}
		else if (vector.z < 0f)
		{
			num = Mathf.Max(num, vector2.z / vector.z);
		}
		Debug.DrawLine(position2 + forward2 * num, position2 + forward2 * num + Vector3.up);
		Debug.DrawLine(position2 + forward2 * num, position2 + forward2 * num2);
	}

	private float GetAspect()
	{
		if (this.myProjector != null)
		{
			return this.myProjector.aspectRatio;
		}
		return 0f;
	}

	private float GetOrthoSize()
	{
		if (this.myProjector != null)
		{
			return this.myProjector.orthographicSize;
		}
		return 0f;
	}

	private bool GetOrtho()
	{
		return this.myProjector != null && this.myProjector.orthographic;
	}

	public void UpdatePosition(bool first = false)
	{
		if (first || base.transform.hasChanged || this.matrixReconstruct || this.LastRange != this.LightRange() || this.LastSpotAngle != this.LightSpotAngle() || this.lastType != this.GetLightType() || (this.GetLightType() == LightType.Area && (this.LastAspect != this.GetAspect() || this.LastOrthoSize == this.GetOrthoSize() || this.LastOrtho == this.GetOrtho())))
		{
			if (this.GetLightType() == LightType.Point)
			{
				Vector3 b = new Vector3(this.LightRange(), this.LightRange(), this.LightRange());
				this.minBounds = base.transform.position - b;
				this.maxBounds = base.transform.position + b;
				this.lastBounds.SetMinMax(this.minBounds, this.maxBounds);
				if (!first)
				{
					this.CheckLightType();
					HxVolumetricCamera.LightOctree.Move(this.octreeNode, this.minBounds, this.maxBounds);
				}
				else
				{
					this.lastType = this.GetLightType();
				}
				this.UpdateLightMatrix();
				return;
			}
			if (this.GetLightType() == LightType.Spot)
			{
				Vector3 position = base.transform.position;
				Vector3 forward = base.transform.forward;
				Vector3 right = base.transform.right;
				Vector3 up = base.transform.up;
				Vector3 a = position + forward * this.LightRange();
				float d = Mathf.Tan(this.LightSpotAngle() * 0.017453292f / 2f) * this.LightRange();
				Vector3 vector = a + up * d - right * d;
				Vector3 vector2 = a + up * d + right * d;
				Vector3 vector3 = a - up * d - right * d;
				Vector3 vector4 = a - up * d + right * d;
				this.TopFrustumNormal = this.NormalOfTriangle(position, vector, vector2);
				this.BottomFrustumNormal = this.NormalOfTriangle(position, vector4, vector3);
				this.LeftFrustumNormal = this.NormalOfTriangle(position, vector3, vector);
				this.RightFrustumNormal = this.NormalOfTriangle(position, vector2, vector4);
				this.minBounds = new Vector3(Mathf.Min(vector.x, Mathf.Min(vector2.x, Mathf.Min(vector3.x, Mathf.Min(vector4.x, position.x)))), Mathf.Min(vector.y, Mathf.Min(vector2.y, Mathf.Min(vector3.y, Mathf.Min(vector4.y, position.y)))), Mathf.Min(vector.z, Mathf.Min(vector2.z, Mathf.Min(vector3.z, Mathf.Min(vector4.z, position.z)))));
				this.maxBounds = new Vector3(Mathf.Max(vector.x, Mathf.Max(vector2.x, Mathf.Max(vector3.x, Mathf.Max(vector4.x, position.x)))), Mathf.Max(vector.y, Mathf.Max(vector2.y, Mathf.Max(vector3.y, Mathf.Max(vector4.y, position.y)))), Mathf.Max(vector.z, Mathf.Max(vector2.z, Mathf.Max(vector3.z, Mathf.Max(vector4.z, position.z)))));
				this.lastBounds.SetMinMax(this.minBounds, this.maxBounds);
				if (!first)
				{
					this.CheckLightType();
					HxVolumetricCamera.LightOctree.Move(this.octreeNode, this.minBounds, this.maxBounds);
				}
				else
				{
					this.lastType = this.GetLightType();
				}
				this.UpdateLightMatrix();
				return;
			}
			if (this.myProjector != null)
			{
				Vector3 position2 = base.transform.position;
				Vector3 forward2 = base.transform.forward;
				Vector3 right2 = base.transform.right;
				Vector3 up2 = base.transform.up;
				Vector3 a2 = position2 + forward2 * this.LightRange();
				Vector3 a3 = position2 + forward2 * this.myProjector.nearClipPlane;
				float num;
				float num2;
				if (this.myProjector.orthographic)
				{
					num = this.myProjector.orthographicSize;
					num2 = this.myProjector.orthographicSize;
				}
				else
				{
					num = Mathf.Tan(this.LightSpotAngle() * 0.017453292f / 2f) * this.LightRange();
					num2 = Mathf.Tan(this.LightSpotAngle() * 0.017453292f / 2f) * this.myProjector.nearClipPlane;
				}
				Vector3 vector5 = a2 + up2 * num - right2 * (num * this.myProjector.aspectRatio);
				Vector3 vector6 = a2 + up2 * num + right2 * (num * this.myProjector.aspectRatio);
				Vector3 vector7 = a2 - up2 * num - right2 * (num * this.myProjector.aspectRatio);
				Vector3 vector8 = a2 - up2 * num + right2 * (num * this.myProjector.aspectRatio);
				Vector3 vector9 = a3 + up2 * num2 - right2 * (num2 * this.myProjector.aspectRatio);
				Vector3 vector10 = a3 + up2 * num2 + right2 * (num2 * this.myProjector.aspectRatio);
				Vector3 vector11 = a3 - up2 * num2 - right2 * (num2 * this.myProjector.aspectRatio);
				Vector3 vector12 = a3 - up2 * num2 + right2 * (num2 * this.myProjector.aspectRatio);
				this.TopFrustumNormal = this.NormalOfTriangle(vector9, vector5, vector6);
				this.BottomFrustumNormal = this.NormalOfTriangle(vector12, vector8, vector7);
				this.LeftFrustumNormal = this.NormalOfTriangle(vector11, vector7, vector5);
				this.RightFrustumNormal = this.NormalOfTriangle(vector10, vector6, vector8);
				this.LastOrtho = this.GetOrtho();
				this.minBounds = Vector3.Min(vector5, Vector3.Min(vector6, Vector3.Min(vector7, Vector3.Min(vector8, Vector3.Min(vector9, Vector3.Min(vector10, Vector3.Min(vector11, vector12)))))));
				this.maxBounds = Vector3.Max(vector5, Vector3.Max(vector6, Vector3.Max(vector7, Vector3.Max(vector8, Vector3.Max(vector9, Vector3.Max(vector10, Vector3.Max(vector11, vector12)))))));
				this.LastOrthoSize = this.GetOrthoSize();
				this.lastBounds.SetMinMax(this.minBounds, this.maxBounds);
				if (!first)
				{
					this.CheckLightType();
					HxVolumetricCamera.LightOctree.Move(this.octreeNode, this.minBounds, this.maxBounds);
				}
				else
				{
					this.lastType = this.GetLightType();
				}
				this.LastAspect = this.GetAspect();
				this.UpdateLightMatrix();
			}
			if (!first)
			{
				this.CheckLightType();
				return;
			}
			this.lastType = this.GetLightType();
		}
	}

	public void DrawBounds()
	{
		if (this.GetLightType() != LightType.Directional)
		{
			Debug.DrawLine(new Vector3(this.minBounds.x, this.minBounds.y, this.minBounds.z), new Vector3(this.maxBounds.x, this.minBounds.y, this.minBounds.z), this.LightColor());
			Debug.DrawLine(new Vector3(this.maxBounds.x, this.minBounds.y, this.minBounds.z), new Vector3(this.maxBounds.x, this.minBounds.y, this.maxBounds.z), this.LightColor());
			Debug.DrawLine(new Vector3(this.maxBounds.x, this.minBounds.y, this.maxBounds.z), new Vector3(this.minBounds.x, this.minBounds.y, this.maxBounds.z), this.LightColor());
			Debug.DrawLine(new Vector3(this.minBounds.x, this.minBounds.y, this.maxBounds.z), new Vector3(this.minBounds.x, this.minBounds.y, this.minBounds.z), this.LightColor());
			Debug.DrawLine(new Vector3(this.minBounds.x, this.maxBounds.y, this.minBounds.z), new Vector3(this.maxBounds.x, this.maxBounds.y, this.minBounds.z), this.LightColor());
			Debug.DrawLine(new Vector3(this.maxBounds.x, this.maxBounds.y, this.minBounds.z), new Vector3(this.maxBounds.x, this.maxBounds.y, this.maxBounds.z), this.LightColor());
			Debug.DrawLine(new Vector3(this.maxBounds.x, this.maxBounds.y, this.maxBounds.z), new Vector3(this.minBounds.x, this.maxBounds.y, this.maxBounds.z), this.LightColor());
			Debug.DrawLine(new Vector3(this.minBounds.x, this.maxBounds.y, this.maxBounds.z), new Vector3(this.minBounds.x, this.maxBounds.y, this.minBounds.z), this.LightColor());
			Debug.DrawLine(new Vector3(this.minBounds.x, this.minBounds.y, this.minBounds.z), new Vector3(this.minBounds.x, this.maxBounds.y, this.minBounds.z), this.LightColor());
			Debug.DrawLine(new Vector3(this.maxBounds.x, this.minBounds.y, this.minBounds.z), new Vector3(this.maxBounds.x, this.maxBounds.y, this.minBounds.z), this.LightColor());
			Debug.DrawLine(new Vector3(this.maxBounds.x, this.minBounds.y, this.maxBounds.z), new Vector3(this.maxBounds.x, this.maxBounds.y, this.maxBounds.z), this.LightColor());
			Debug.DrawLine(new Vector3(this.minBounds.x, this.minBounds.y, this.maxBounds.z), new Vector3(this.minBounds.x, this.maxBounds.y, this.maxBounds.z), this.LightColor());
		}
	}

	private static float ShadowDistanceExtra = 0.75f;

	private Light myLight;

	private HxDummyLight myDummyLight;

	public Texture3D NoiseTexture3D;

	private CommandBuffer BufferRender;

	private CommandBuffer BufferCopy;

	private Projector myProjector;

	public Vector3 NoiseScale = new Vector3(0.5f, 0.5f, 0.5f);

	public Vector3 NoiseVelocity = new Vector3(1f, 1f, 0f);

	private bool dirty = true;

	public Texture LightFalloff;

	public float NearPlane;

	public bool NoiseEnabled;

	public bool CustomMieScatter;

	public bool CustomExtinction;

	public bool CustomExtinctionEffect;

	public bool CustomDensity;

	public bool CustomSampleCount;

	public bool CustomColor;

	public bool CustomNoiseEnabled;

	public bool CustomNoiseTexture;

	public bool CustomNoiseScale;

	public bool CustomNoiseVelocity;

	public bool CustomNoiseContrast;

	public bool CustomFogHeightEnabled;

	public bool CustomFogHeight;

	public bool CustomFogTransitionSize;

	public bool CustomAboveFogPercent;

	public bool CustomSunSize;

	public bool CustomSunBleed;

	public bool ShadowCasting = true;

	public bool CustomStrength;

	public bool CustomIntensity;

	public bool CustomTintMode;

	public bool CustomTintColor;

	public bool CustomTintColor2;

	public bool CustomTintGradient;

	public bool CustomTintIntensity;

	public bool CustomMaxLightDistance;

	[Range(0f, 4f)]
	public float NoiseContrast = 1f;

	public HxVolumetricCamera.HxTintMode TintMode;

	public Color TintColor = Color.red;

	public Color TintColor2 = Color.blue;

	public float TintIntensity = 0.2f;

	[Range(0f, 1f)]
	public float TintGradient = 0.2f;

	[Range(0f, 8f)]
	public float Intensity = 1f;

	[Range(0f, 1f)]
	public float Strength = 1f;

	public Color Color = Color.white;

	[Range(0f, 0.9999f)]
	[Tooltip("0 for even scattering, 1 for forward scattering")]
	public float MieScattering = 0.05f;

	[Range(0f, 1f)]
	[Tooltip("Create a sun using mie scattering")]
	public float SunSize;

	[Tooltip("Allows the sun to bleed over the edge of objects (recommend using bloom)")]
	public bool SunBleed = true;

	[Range(0f, 10f)]
	[Tooltip("dimms results over distance")]
	public float Extinction = 0.01f;

	[Range(0f, 1f)]
	[Tooltip("Density of air")]
	public float Density = 0.2f;

	[Range(0f, 1f)]
	[Tooltip("Useful when you want a light to have slightly more density")]
	public float ExtraDensity;

	[Range(2f, 64f)]
	[Tooltip("How many samples per pixel, Recommended 4-8 for point, 6 - 16 for Directional")]
	public int SampleCount = 4;

	[Tooltip("Ray marching Shadows can be expensive, save some frames by not marching shadows")]
	public bool Shadows = true;

	public bool FogHeightEnabled;

	public float FogHeight = 5f;

	public float FogTransitionSize = 5f;

	public float MaxLightDistance = 128f;

	public float AboveFogPercent = 0.1f;

	private bool OffsetUpdated;

	public Vector3 Offset = Vector3.zero;

	private static MaterialPropertyBlock propertyBlock;

	private bool bufferBuilt;

	public static int VolumetricBMVPPID;

	public static int VolumetricMVPPID;

	public static int VolumetricMVP2PID;

	public static int VolumetricMVPID;

	private static int LightColourPID;

	private static int LightColour2PID;

	private static int FogHeightsPID;

	private static int PhasePID;

	private static int _LightParamsPID;

	private static int DensityPID;

	private static int ShadowBiasPID;

	private static int _CustomLightPositionPID;

	private static int hxNearPlanePID;

	private static int NoiseScalePID;

	private static int NoiseOffsetPID;

	private static int _SpotLightParamsPID;

	private static int _LightTexture0PID;

	private static int _hxProjectorTexturePID;

	private static int _hxProjectorFalloffTexturePID;

	private bool LastBufferDirectional;

	private float LastSpotAngle;

	private float LastRange;

	private float LastAspect;

	private LightType lastType = LightType.Area;

	private Matrix4x4 LightMatrix;

	private Bounds lastBounds;

	private Vector3 minBounds;

	private Vector3 maxBounds;

	private HxOctreeNode<HxVolumetricLight>.NodeObject octreeNode;

	private Vector4 TopFrustumNormal;

	private Vector4 BottomFrustumNormal;

	private Vector4 LeftFrustumNormal;

	private Vector4 RightFrustumNormal;

	private static Matrix4x4[] VolumeMatrixArrays = new Matrix4x4[50];

	private static Vector4[] VolumeSettingsArrays = new Vector4[50];

	private static Matrix4x4[] VolumeMatrixArraysOld = new Matrix4x4[10];

	private static Vector4[] VolumeSettingsArraysOld = new Vector4[10];

	private float LastOrthoSize;

	private bool LastOrtho;

	private bool matrixReconstruct = true;
}


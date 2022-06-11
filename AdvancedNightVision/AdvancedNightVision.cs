using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedNightVision
{
	// Token: 0x02000419 RID: 1049
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Advanced Night Vision")]
	public sealed class AdvancedNightVision : MonoBehaviour
	{
		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06002164 RID: 8548 RVA: 0x000A22C7 File Offset: 0x000A04C7
		// (set) Token: 0x06002165 RID: 8549 RVA: 0x000A22CF File Offset: 0x000A04CF
		public AdvancedNightVision.ShaderPasses ShaderPass
		{
			get
			{
				return this.shaderPasses;
			}
			set
			{
				if (this.shaderPasses != value)
				{
					this.shaderPasses = value;
					this.LoadShader();
				}
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06002166 RID: 8550 RVA: 0x000A22E7 File Offset: 0x000A04E7
		// (set) Token: 0x06002167 RID: 8551 RVA: 0x000A22EF File Offset: 0x000A04EF
		public AdvancedNightVision.RenderTextureResolutions RenderTextureResolution
		{
			get
			{
				return this.renderTextureResolution;
			}
			set
			{
				this.renderTextureResolution = value;
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06002168 RID: 8552 RVA: 0x000A22F8 File Offset: 0x000A04F8
		// (set) Token: 0x06002169 RID: 8553 RVA: 0x000A2300 File Offset: 0x000A0500
		public float BlurPasses
		{
			get
			{
				return this.blurPasses;
			}
			set
			{
				this.blurPasses = (float)((int)Mathf.Clamp(value, 0f, 10f));
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x0600216A RID: 8554 RVA: 0x000A231A File Offset: 0x000A051A
		// (set) Token: 0x0600216B RID: 8555 RVA: 0x000A2322 File Offset: 0x000A0522
		public float Glow
		{
			get
			{
				return this.glow;
			}
			set
			{
				this.glow = (float)((int)Mathf.Clamp(value, 0f, 25f));
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x0600216C RID: 8556 RVA: 0x000A233C File Offset: 0x000A053C
		// (set) Token: 0x0600216D RID: 8557 RVA: 0x000A2344 File Offset: 0x000A0544
		public AdvancedNightVision.ColorControls ColorControl
		{
			get
			{
				return this.colorControlType;
			}
			set
			{
				this.colorControlType = value;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x0600216E RID: 8558 RVA: 0x000A234D File Offset: 0x000A054D
		// (set) Token: 0x0600216F RID: 8559 RVA: 0x000A2355 File Offset: 0x000A0555
		public Color Tint
		{
			get
			{
				return this.tint;
			}
			set
			{
				this.tint = value;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06002170 RID: 8560 RVA: 0x000A235E File Offset: 0x000A055E
		// (set) Token: 0x06002171 RID: 8561 RVA: 0x000A2366 File Offset: 0x000A0566
		public float LuminanceAmount
		{
			get
			{
				return this.luminanceAmount;
			}
			set
			{
				this.luminanceAmount = Mathf.Clamp(value, 0f, 1f);
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06002172 RID: 8562 RVA: 0x000A237E File Offset: 0x000A057E
		// (set) Token: 0x06002173 RID: 8563 RVA: 0x000A2386 File Offset: 0x000A0586
		public float Exposure
		{
			get
			{
				return this.exposure;
			}
			set
			{
				this.exposure = Mathf.Clamp(value, 0f, 10f);
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06002174 RID: 8564 RVA: 0x000A239E File Offset: 0x000A059E
		// (set) Token: 0x06002175 RID: 8565 RVA: 0x000A23A6 File Offset: 0x000A05A6
		public float Brightness
		{
			get
			{
				return this.brightness;
			}
			set
			{
				this.brightness = Mathf.Clamp(value, -1f, 1f);
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06002176 RID: 8566 RVA: 0x000A23BE File Offset: 0x000A05BE
		public float Contrast
		{
			get
			{
				return this.contrast;
			}
			set
			{
				this.contrast = Mathf.Clamp(value, -1f, 1f);
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06002178 RID: 8568 RVA: 0x000A23DE File Offset: 0x000A05DE
		// (set) Token: 0x06002179 RID: 8569 RVA: 0x000A23E6 File Offset: 0x000A05E6
		public float Saturation
		{
			get
			{
				return this.saturation;
			}
			set
			{
				this.saturation = Mathf.Clamp(value, 0f, 1f);
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600217A RID: 8570 RVA: 0x000A23FE File Offset: 0x000A05FE
		// (set) Token: 0x0600217B RID: 8571 RVA: 0x000A2408 File Offset: 0x000A0608
		public Vector3 RGBOffset
		{
			get
			{
				return this.rgbOffset;
			}
			set
			{
				this.rgbOffset = new Vector3(Mathf.Clamp(value.x, -100f, 100f), Mathf.Clamp(value.y, -100f, 100f), Mathf.Clamp(value.z, -100f, 100f));
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600217C RID: 8572 RVA: 0x000A245F File Offset: 0x000A065F
		// (set) Token: 0x0600217D RID: 8573 RVA: 0x000A2467 File Offset: 0x000A0667
		public AdvancedNightVision.VignetteTypes VignetteType
		{
			get
			{
				return this.vignetteType;
			}
			set
			{
				this.vignetteType = value;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600217E RID: 8574 RVA: 0x000A2470 File Offset: 0x000A0670
		// (set) Token: 0x0600217F RID: 8575 RVA: 0x000A2478 File Offset: 0x000A0678
		public float VignetteScale
		{
			get
			{
				return this.vignetteScale;
			}
			set
			{
				this.vignetteScale = Mathf.Clamp(value, 0f, 100f);
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06002180 RID: 8576 RVA: 0x000A2490 File Offset: 0x000A0690
		// (set) Token: 0x06002181 RID: 8577 RVA: 0x000A2498 File Offset: 0x000A0698
		public float VignetteSoftness
		{
			get
			{
				return this.vignetteSoftness;
			}
			set
			{
				this.vignetteSoftness = Mathf.Clamp(value, 0f, 10f);
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06002182 RID: 8578 RVA: 0x000A24B0 File Offset: 0x000A06B0
		// (set) Token: 0x06002183 RID: 8579 RVA: 0x000A24B8 File Offset: 0x000A06B8
		public AdvancedNightVision.ChromaticAberrationTypes ChromaticAberrationType
		{
			get
			{
				return this.chromaticAberrationType;
			}
			set
			{
				this.chromaticAberrationType = value;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06002184 RID: 8580 RVA: 0x000A24C1 File Offset: 0x000A06C1
		// (set) Token: 0x06002185 RID: 8581 RVA: 0x000A24C9 File Offset: 0x000A06C9
		public int DistortionsPasses
		{
			get
			{
				return this.distortionsPasses;
			}
			set
			{
				this.distortionsPasses = AdvancedNightVisionHelper.Clamp(value, 3, 24);
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06002186 RID: 8582 RVA: 0x000A24DA File Offset: 0x000A06DA
		// (set) Token: 0x06002187 RID: 8583 RVA: 0x000A24E2 File Offset: 0x000A06E2
		public float ChromaticAberration
		{
			get
			{
				return this.chromaticAberration;
			}
			set
			{
				this.chromaticAberration = Mathf.Clamp(value, -100f, 100f);
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06002188 RID: 8584 RVA: 0x000A24FA File Offset: 0x000A06FA
		// (set) Token: 0x06002189 RID: 8585 RVA: 0x000A2502 File Offset: 0x000A0702
		public float BarrelDistortion
		{
			get
			{
				return this.barrelDistortion;
			}
			set
			{
				this.barrelDistortion = Mathf.Clamp(value, -100f, 100f);
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x0600218A RID: 8586 RVA: 0x000A251A File Offset: 0x000A071A
		// (set) Token: 0x0600218B RID: 8587 RVA: 0x000A2522 File Offset: 0x000A0722
		public bool AnalogTV
		{
			get
			{
				return this.analogTV;
			}
			set
			{
				this.analogTV = value;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x0600218C RID: 8588 RVA: 0x000A252B File Offset: 0x000A072B
		// (set) Token: 0x0600218D RID: 8589 RVA: 0x000A2533 File Offset: 0x000A0733
		public float Noise
		{
			get
			{
				return this.noise;
			}
			set
			{
				this.noise = Mathf.Clamp(value, 0f, 10f);
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x0600218E RID: 8590 RVA: 0x000A254B File Offset: 0x000A074B
		public float Scanline
		{
			get
			{
				return this.scanline;
			}
			set
			{
				this.scanline = Mathf.Clamp(value, 0f, 2f);
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06002190 RID: 8592 RVA: 0x000A256B File Offset: 0x000A076B
		// (set) Token: 0x06002191 RID: 8593 RVA: 0x000A2573 File Offset: 0x000A0773
		public bool DigitalTV
		{
			get
			{
				return this.digitalTV;
			}
			set
			{
				this.digitalTV = value;
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06002192 RID: 8594 RVA: 0x000A257C File Offset: 0x000A077C
		// (set) Token: 0x06002193 RID: 8595 RVA: 0x000A2584 File Offset: 0x000A0784
		public float DigitalTVNoiseThreshold
		{
			get
			{
				return this.digitalTVNoiseThreshold;
			}
			set
			{
				this.digitalTVNoiseThreshold = Mathf.Clamp01(value);
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06002194 RID: 8596 RVA: 0x000A2592 File Offset: 0x000A0792
		// (set) Token: 0x06002195 RID: 8597 RVA: 0x000A259A File Offset: 0x000A079A
		public float DigitalTVNoiseMaxOffset
		{
			get
			{
				return this.digitalTVNoiseMaxOffset;
			}
			set
			{
				this.digitalTVNoiseMaxOffset = Mathf.Clamp01(value);
			}
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x000A25A8 File Offset: 0x000A07A8
		public void ResetDefaultValues()
		{
			this.blurPasses = 2f;
			this.luminanceAmount = 1f;
			this.exposure = 1f;
			this.rgbOffset = new Vector3(10f, 1f, -10f);
			this.tint = new Color(0.258f, 1f, 0.188f);
			this.brightness = 0f;
			this.contrast = 0f;
			this.saturation = 0.5f;
			this.distortionsPasses = 6;
			this.chromaticAberration = 1f;
			this.barrelDistortion = 10f;
			this.noise = 3f;
			this.scanline = 1f;
			this.vignetteScale = 10f;
			this.vignetteSoftness = 0.1f;
			this.glow = 4f;
			this.digitalTVNoiseThreshold = 0.1f;
			this.digitalTVNoiseMaxOffset = 0.1f;
		}

		// Token: 0x06002197 RID: 8599 RVA: 0x000A2695 File Offset: 0x000A0895
		private void Awake()
		{
			this.LoadShader();
		}

		// Token: 0x06002198 RID: 8600 RVA: 0x000A269D File Offset: 0x000A089D
		private void OnEnable()
		{
			if (AdvancedNightVisionHelper.CheckHardwareRequirements(true, false, false, this) && AdvancedNightVisionHelper.IsSupported(this.shader, this))
			{
				this.material = new Material(this.shader);
			}
		}

		// Token: 0x06002199 RID: 8601 RVA: 0x000A26C9 File Offset: 0x000A08C9
		private void OnDisable()
		{
			if (this.material != null)
			{
				UnityEngine.Object.DestroyImmediate(this.material);
			}
			this.ReleaseAllTemporaryRenderTextures();
		}

		// Token: 0x0600219A RID: 8602 RVA: 0x000A26EC File Offset: 0x000A08EC
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.material != null)
			{
				if (this.shaderPasses == AdvancedNightVision.ShaderPasses.MultiPass)
				{
					if (this.blurPasses > 0f)
					{
						this.material.EnableKeyword("BLUR_ENABLED");
						this.material.SetFloat("_Blur", this.blurPasses);
					}
					else
					{
						this.material.DisableKeyword("BLUR_ENABLED");
					}
					this.material.DisableKeyword("CHROMATIC_NONE");
					this.material.DisableKeyword("CHROMATIC_SIMPLE");
					this.material.DisableKeyword("CHROMATIC_ADVANCED");
					switch (this.chromaticAberrationType)
					{
					case AdvancedNightVision.ChromaticAberrationTypes.None:
						this.material.EnableKeyword("CHROMATIC_NONE");
						break;
					case AdvancedNightVision.ChromaticAberrationTypes.Simple:
						this.material.EnableKeyword("CHROMATIC_SIMPLE");
						this.material.SetFloat("_ChromaticAberration", this.chromaticAberration);
						break;
					case AdvancedNightVision.ChromaticAberrationTypes.Advanced:
						this.material.EnableKeyword("CHROMATIC_ADVANCED");
						this.material.SetInt("_DistortionsPasses", this.distortionsPasses);
						this.material.SetFloat("_ChromaticAberration", this.chromaticAberration);
						this.material.SetFloat("_BarrelDistortion", this.barrelDistortion * 0.05f);
						break;
					}
					if (this.glow > 0f)
					{
						this.material.EnableKeyword("GLOW_ENABLED");
						this.material.SetFloat("_Glow", this.glow);
					}
					else
					{
						this.material.DisableKeyword("GLOW_ENABLED");
					}
				}
				this.material.SetColor("_Tint", this.tint);
				this.material.SetFloat("_LuminanceAmount", this.luminanceAmount);
				this.material.SetFloat("_Exposure", this.exposure);
				if (this.colorControlType == AdvancedNightVision.ColorControls.Advanced)
				{
					this.material.EnableKeyword("COLORCONTROL_ADVANCED");
					this.material.SetVector("_RGBLum", this.rgbOffset);
					this.material.SetFloat("_Brightness", this.brightness);
					this.material.SetFloat("_Contrast", 1f + this.contrast);
					this.material.SetFloat("_Saturation", this.saturation);
				}
				else
				{
					this.material.DisableKeyword("COLORCONTROL_ADVANCED");
				}
				if (this.analogTV)
				{
					this.material.EnableKeyword("ANALOGTV_ENABLED");
					this.material.SetFloat("_Scanline", (this.scanline < 0.04f) ? 0.04f : this.scanline);
					this.material.SetFloat("_Noise", this.noise * 0.1f);
				}
				else
				{
					this.material.DisableKeyword("ANALOGTV_ENABLED");
				}
				if (this.digitalTV)
				{
					this.material.EnableKeyword("DIGITALTV_ENABLED");
					this.material.SetFloat("_DigitalTVNoiseThreshold", this.digitalTVNoiseThreshold);
					this.material.SetFloat("_DigitalTVNoiseMaxOffset", this.digitalTVNoiseMaxOffset);
				}
				else
				{
					this.material.DisableKeyword("DIGITALTV_ENABLED");
				}
				this.material.DisableKeyword("VIGNETTE_NONE");
				this.material.DisableKeyword("VIGNETTE_SCREEN");
				this.material.DisableKeyword("VIGNETTE_MONOCULAR");
				this.material.DisableKeyword("VIGNETTE_BINOCULAR");
				switch (this.vignetteType)
				{
				case AdvancedNightVision.VignetteTypes.None:
					this.material.EnableKeyword("VIGNETTE_NONE");
					break;
				case AdvancedNightVision.VignetteTypes.Screen:
					this.material.EnableKeyword("VIGNETTE_SCREEN");
					break;
				case AdvancedNightVision.VignetteTypes.Monocular:
					this.material.EnableKeyword("VIGNETTE_MONOCULAR");
					break;
				case AdvancedNightVision.VignetteTypes.Binocular:
					this.material.EnableKeyword("VIGNETTE_BINOCULAR");
					break;
				}
				if (this.vignetteType != AdvancedNightVision.VignetteTypes.None)
				{
					this.material.SetInt("_VignetteType", (int)this.vignetteType);
					this.material.SetFloat("_VignetteScale", (this.vignetteType == AdvancedNightVision.VignetteTypes.Screen) ? this.vignetteScale : (this.vignetteScale * 0.15f));
					this.material.SetFloat("_VignetteSoftness", this.vignetteSoftness * 0.075f);
				}
				if (source)
				{
					int width = source.width / (int)this.renderTextureResolution;
					int height = source.height / (int)this.renderTextureResolution;
					if (this.shaderPasses == AdvancedNightVision.ShaderPasses.OnePass)
					{
						if (this.renderTextureResolution == AdvancedNightVision.RenderTextureResolutions.Normal)
						{
							Graphics.Blit(source, destination, this.material);
							return;
						}
						RenderTexture temporaryRenderTexture = this.GetTemporaryRenderTexture(width, height);
						Graphics.Blit(source, temporaryRenderTexture, this.material);
						Graphics.Blit(temporaryRenderTexture, destination, this.material);
						this.ReleaseTemporaryRenderTexture(temporaryRenderTexture);
						return;
					}
					else
					{
						RenderTexture temporaryRenderTexture2 = this.GetTemporaryRenderTexture(width, height);
						Graphics.Blit(source, temporaryRenderTexture2, this.material, 0);
						if (this.glow == 0f)
						{
							Graphics.Blit(temporaryRenderTexture2, destination, this.material, 1);
						}
						else
						{
							RenderTexture temporaryRenderTexture3 = this.GetTemporaryRenderTexture(width, height);
							Graphics.Blit(temporaryRenderTexture2, temporaryRenderTexture3, this.material, 1);
							Graphics.Blit(temporaryRenderTexture3, destination, this.material, 2);
							this.ReleaseTemporaryRenderTexture(temporaryRenderTexture3);
						}
						this.ReleaseTemporaryRenderTexture(temporaryRenderTexture2);
					}
				}
			}
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x000A2C0C File Offset: 0x000A0E0C
		private void LoadShader()
		{
			this.shader = Resources.Load<Shader>(string.Format("Shaders/AdvancedNightVision{0}", this.shaderPasses.ToString()));
			AdvancedNightVisionHelper.IsSupported(this.shader, this);
			this.material = new Material(this.shader);
			this.ReleaseAllTemporaryRenderTextures();
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x000A2C64 File Offset: 0x000A0E64
		private RenderTexture GetTemporaryRenderTexture(int width, int height)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, Application.isMobilePlatform ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR);
			this.renderTextures.Add(temporary);
			return temporary;
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x000A2C93 File Offset: 0x000A0E93
		private void ReleaseTemporaryRenderTexture(RenderTexture renderTexture)
		{
			this.renderTextures.Remove(renderTexture);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x0600219E RID: 8606 RVA: 0x000A2CA8 File Offset: 0x000A0EA8
		private void ReleaseAllTemporaryRenderTextures()
		{
			while (this.renderTextures.Count > 0)
			{
				int index = this.renderTextures.Count - 1;
				RenderTexture temp = this.renderTextures[index];
				this.renderTextures.RemoveAt(index);
				RenderTexture.ReleaseTemporary(temp);
			}
		}

		// Token: 0x04001E98 RID: 7832
		[SerializeField]
		private AdvancedNightVision.ShaderPasses shaderPasses = AdvancedNightVision.ShaderPasses.MultiPass;

		// Token: 0x04001E99 RID: 7833
		[SerializeField]
		private float blurPasses = 2f;

		// Token: 0x04001E9A RID: 7834
		[SerializeField]
		private AdvancedNightVision.RenderTextureResolutions renderTextureResolution = AdvancedNightVision.RenderTextureResolutions.Normal;

		// Token: 0x04001E9B RID: 7835
		[SerializeField]
		private float luminanceAmount = 1f;

		// Token: 0x04001E9C RID: 7836
		[SerializeField]
		private float exposure = 1f;

		// Token: 0x04001E9D RID: 7837
		[SerializeField]
		private Vector3 rgbOffset = new Vector3(10f, 1f, -10f);

		// Token: 0x04001E9E RID: 7838
		[SerializeField]
		private Color tint = new Color(0.258f, 1f, 0.188f);

		// Token: 0x04001E9F RID: 7839
		[SerializeField]
		private float brightness;

		// Token: 0x04001EA0 RID: 7840
		[SerializeField]
		private float contrast;

		// Token: 0x04001EA1 RID: 7841
		[SerializeField]
		private float saturation = 0.5f;

		// Token: 0x04001EA2 RID: 7842
		[SerializeField]
		private int distortionsPasses = 6;

		// Token: 0x04001EA3 RID: 7843
		[SerializeField]
		private float chromaticAberration = 2f;

		// Token: 0x04001EA4 RID: 7844
		[SerializeField]
		private float barrelDistortion = 10f;

		// Token: 0x04001EA5 RID: 7845
		[SerializeField]
		private bool analogTV = true;

		// Token: 0x04001EA6 RID: 7846
		[SerializeField]
		private float noise = 3f;

		// Token: 0x04001EA7 RID: 7847
		[SerializeField]
		private float scanline = 1f;

		// Token: 0x04001EA8 RID: 7848
		[SerializeField]
		private bool digitalTV = true;

		// Token: 0x04001EA9 RID: 7849
		[SerializeField]
		private AdvancedNightVision.ColorControls colorControlType = AdvancedNightVision.ColorControls.Advanced;

		// Token: 0x04001EAA RID: 7850
		[SerializeField]
		private AdvancedNightVision.ChromaticAberrationTypes chromaticAberrationType = AdvancedNightVision.ChromaticAberrationTypes.Advanced;

		// Token: 0x04001EAB RID: 7851
		[SerializeField]
		private AdvancedNightVision.VignetteTypes vignetteType = AdvancedNightVision.VignetteTypes.Screen;

		// Token: 0x04001EAC RID: 7852
		[SerializeField]
		private float vignetteScale = 10f;

		// Token: 0x04001EAD RID: 7853
		[SerializeField]
		private float vignetteSoftness = 0.1f;

		// Token: 0x04001EAE RID: 7854
		[SerializeField]
		private float glow = 4f;

		// Token: 0x04001EAF RID: 7855
		[SerializeField]
		private float digitalTVNoiseThreshold = 0.1f;

		// Token: 0x04001EB0 RID: 7856
		[SerializeField]
		private float digitalTVNoiseMaxOffset = 0.1f;

		// Token: 0x04001EB1 RID: 7857
		private List<RenderTexture> renderTextures = new List<RenderTexture>();

		// Token: 0x04001EB2 RID: 7858
		private Material material;

		// Token: 0x04001EB3 RID: 7859
		private Shader shader;

		// Token: 0x04001EB4 RID: 7860
		private const string variableBlur = "_Blur";

		// Token: 0x04001EB5 RID: 7861
		private const string variableLuminanceAmount = "_LuminanceAmount";

		// Token: 0x04001EB6 RID: 7862
		private const string variableExposure = "_Exposure";

		// Token: 0x04001EB7 RID: 7863
		private const string variableRGBLum = "_RGBLum";

		// Token: 0x04001EB8 RID: 7864
		private const string variableTint = "_Tint";

		// Token: 0x04001EB9 RID: 7865
		private const string variableBrightness = "_Brightness";

		// Token: 0x04001EBA RID: 7866
		private const string variableContrast = "_Contrast";

		// Token: 0x04001EBB RID: 7867
		private const string variableSaturation = "_Saturation";

		// Token: 0x04001EBC RID: 7868
		private const string variableDistortionsPasses = "_DistortionsPasses";

		// Token: 0x04001EBD RID: 7869
		private const string variableChromaticAberration = "_ChromaticAberration";

		// Token: 0x04001EBE RID: 7870
		private const string variableBarrelDistortion = "_BarrelDistortion";

		// Token: 0x04001EBF RID: 7871
		private const string variableNoise = "_Noise";

		// Token: 0x04001EC0 RID: 7872
		private const string variableScanline = "_Scanline";

		// Token: 0x04001EC1 RID: 7873
		private const string variableVignetteType = "_VignetteType";

		// Token: 0x04001EC2 RID: 7874
		private const string variableVignetteScale = "_VignetteScale";

		// Token: 0x04001EC3 RID: 7875
		private const string variableVignetteSoftness = "_VignetteSoftness";

		// Token: 0x04001EC4 RID: 7876
		private const string variableGlow = "_Glow";

		// Token: 0x04001EC5 RID: 7877
		private const string variableDigitalTVNoiseThreshold = "_DigitalTVNoiseThreshold";

		// Token: 0x04001EC6 RID: 7878
		private const string variableDigitalTVNoiseMaxOffset = "_DigitalTVNoiseMaxOffset";

		// Token: 0x04001EC7 RID: 7879
		private const string keywordBlur = "BLUR_ENABLED";

		// Token: 0x04001EC8 RID: 7880
		private const string keywordColorControlAdvanced = "COLORCONTROL_ADVANCED";

		// Token: 0x04001EC9 RID: 7881
		private const string keywordChromaticNone = "CHROMATIC_NONE";

		// Token: 0x04001ECA RID: 7882
		private const string keywordChromaticSimple = "CHROMATIC_SIMPLE";

		// Token: 0x04001ECB RID: 7883
		private const string keywordChromaticAdvanced = "CHROMATIC_ADVANCED";

		// Token: 0x04001ECC RID: 7884
		private const string keywordVignetteNone = "VIGNETTE_NONE";

		// Token: 0x04001ECD RID: 7885
		private const string keywordVignetteScreen = "VIGNETTE_SCREEN";

		// Token: 0x04001ECE RID: 7886
		private const string keywordVignetteMonocular = "VIGNETTE_MONOCULAR";

		// Token: 0x04001ECF RID: 7887
		private const string keywordVignetteBinocular = "VIGNETTE_BINOCULAR";

		// Token: 0x04001ED0 RID: 7888
		private const string keywordAnalogTV = "ANALOGTV_ENABLED";

		// Token: 0x04001ED1 RID: 7889
		private const string keywordDigitalTV = "DIGITALTV_ENABLED";

		// Token: 0x04001ED2 RID: 7890
		private const string keywordGlow = "GLOW_ENABLED";

		// Token: 0x02000725 RID: 1829
		public enum ShaderPasses
		{
			// Token: 0x04002745 RID: 10053
			OnePass = 1,
			// Token: 0x04002746 RID: 10054
			MultiPass = 3
		}

		// Token: 0x02000726 RID: 1830
		public enum RenderTextureResolutions
		{
			// Token: 0x04002748 RID: 10056
			Normal = 1,
			// Token: 0x04002749 RID: 10057
			Half,
			// Token: 0x0400274A RID: 10058
			Quarter = 4
		}

		// Token: 0x02000727 RID: 1831
		public enum ColorControls
		{
			// Token: 0x0400274C RID: 10060
			Simple,
			// Token: 0x0400274D RID: 10061
			Advanced
		}

		// Token: 0x02000728 RID: 1832
		public enum VignetteTypes
		{
			// Token: 0x0400274F RID: 10063
			None,
			// Token: 0x04002750 RID: 10064
			Screen,
			// Token: 0x04002751 RID: 10065
			Monocular,
			// Token: 0x04002752 RID: 10066
			Binocular
		}

		// Token: 0x02000729 RID: 1833
		public enum ChromaticAberrationTypes
		{
			// Token: 0x04002754 RID: 10068
			None,
			// Token: 0x04002755 RID: 10069
			Simple,
			// Token: 0x04002756 RID: 10070
			Advanced
		}
	}
}

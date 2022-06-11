using System;
using UnityEngine;

// Token: 0x02000030 RID: 48
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class Nightvision : MonoBehaviour
{
	// Token: 0x06000124 RID: 292 RVA: 0x00008FA8 File Offset: 0x000071A8
	private void Awake()
	{
		this.nvMaterial = new Material(Shader.Find("Hidden/Nightvision"));
		this.UpdateMaterial();
	}

	// Token: 0x06000125 RID: 293 RVA: 0x00008FC5 File Offset: 0x000071C5
	private void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		this.UpdateMaterial();
		Graphics.Blit(src, dst, this.nvMaterial);
	}

	// Token: 0x06000126 RID: 294 RVA: 0x00008FDC File Offset: 0x000071DC
	public void UpdateMaterial()
	{
		this.nvMaterial.SetFloat("_Power", this.Power);
		this.nvMaterial.SetColor("_Color", this.EffectColor);
		if (this.Vignette.Mode == Nightvision.VignetteSettings.VignetteMode.Off)
		{
			this.nvMaterial.EnableKeyword("Vignette_Off");
		}
		else if (this.Vignette.Mode == Nightvision.VignetteSettings.VignetteMode.Texture)
		{
			this.nvMaterial.SetColor("_VignetteColor", this.Vignette.color);
			this.nvMaterial.SetTexture("_VignetteTex", this.Vignette.Texture.Texture);
			this.nvMaterial.EnableKeyword("Vignette_Texture");
		}
		else if (this.Vignette.Mode == Nightvision.VignetteSettings.VignetteMode.Procedural)
		{
			this.nvMaterial.SetColor("_VignetteColor", this.Vignette.color);
			this.nvMaterial.SetFloat("_VignetteRadius", this.Vignette.Procedural.Radius);
			this.nvMaterial.SetFloat("_VignetteSharpness", this.Vignette.Procedural.Sharpness);
			this.nvMaterial.EnableKeyword("Vignette_Procedural");
		}
		if (this.lastTime < Time.time - this.Noise.OffsetTime)
		{
			this.noiseOffset = new Vector2(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
			this.lastTime = Time.time;
		}
		if (this.Noise.Mode == Nightvision.NoiseSettings.NoiseMode.Off)
		{
			this.nvMaterial.EnableKeyword("Noise_Off");
			return;
		}
		if (this.Noise.Mode == Nightvision.NoiseSettings.NoiseMode.Texture)
		{
			this.nvMaterial.SetTexture("_NoiseTex", this.Noise.Texture.Texture);
			if (this.Noise.Texture.Texture != null)
			{
				Vector2 vector = new Vector2((float)Screen.width / (float)this.Noise.Texture.Texture.width, (float)Screen.height / (float)this.Noise.Texture.Texture.width);
				Vector2 vector2 = Vector2.one;
				if (vector.x > vector.y)
				{
					vector2.x = vector.x / vector.y;
				}
				else
				{
					vector2.y = vector.y / vector.x;
				}
				vector2 /= this.Noise.Texture.Scale;
				this.nvMaterial.SetFloat("_NoiseTileX", vector2.x);
				this.nvMaterial.SetFloat("_NoiseTileY", vector2.y);
			}
			this.nvMaterial.SetFloat("_NoiseOffsetX", this.noiseOffset.x);
			this.nvMaterial.SetFloat("_NoiseOffsetY", this.noiseOffset.y);
			this.nvMaterial.SetFloat("_NoisePower", this.Noise.Power);
			this.nvMaterial.EnableKeyword("Noise_Texture");
			return;
		}
		if (this.Noise.Mode == Nightvision.NoiseSettings.NoiseMode.Procedural)
		{
			this.nvMaterial.SetFloat("_NoiseOffsetX", this.noiseOffset.x);
			this.nvMaterial.SetFloat("_NoiseOffsetY", this.noiseOffset.y);
			this.nvMaterial.SetFloat("_NoisePower", this.Noise.Power);
			this.nvMaterial.EnableKeyword("Noise_Procedural");
		}
	}

	// Token: 0x04000169 RID: 361
	public const float DefaultPower = 20f;

	// Token: 0x0400016A RID: 362
	public float Power = 20f;

	// Token: 0x0400016B RID: 363
	public Nightvision.VignetteSettings Vignette = new Nightvision.VignetteSettings();

	// Token: 0x0400016C RID: 364
	public Nightvision.NoiseSettings Noise = new Nightvision.NoiseSettings();

	// Token: 0x0400016D RID: 365
	public Color EffectColor = Color.green;

	// Token: 0x0400016E RID: 366
	private Material nvMaterial;

	// Token: 0x0400016F RID: 367
	private Vector2 noiseOffset = Vector2.zero;

	// Token: 0x04000170 RID: 368
	private float lastTime;

	// Token: 0x02000483 RID: 1155
	[Serializable]
	public class VignetteSettings
	{
		// Token: 0x04002185 RID: 8581
		public Nightvision.VignetteSettings.VignetteMode Mode;

		// Token: 0x04002186 RID: 8582
		public Color color = Color.black;

		// Token: 0x04002187 RID: 8583
		public Nightvision.VignetteSettings.TextureSettings Texture = new Nightvision.VignetteSettings.TextureSettings();

		// Token: 0x04002188 RID: 8584
		public Nightvision.VignetteSettings.ProceduralSettings Procedural = new Nightvision.VignetteSettings.ProceduralSettings();

		// Token: 0x02000758 RID: 1880
		public enum VignetteMode
		{
			// Token: 0x040027F6 RID: 10230
			Off,
			// Token: 0x040027F7 RID: 10231
			Texture,
			// Token: 0x040027F8 RID: 10232
			Procedural
		}

		// Token: 0x02000759 RID: 1881
		[Serializable]
		public class TextureSettings
		{
			// Token: 0x040027F9 RID: 10233
			public Texture Texture;
		}

		// Token: 0x0200075A RID: 1882
		[Serializable]
		public class ProceduralSettings
		{
			// Token: 0x040027FA RID: 10234
			public const float DefaultRadius = 1f;

			// Token: 0x040027FB RID: 10235
			public const float DefaultSharpness = 50f;

			// Token: 0x040027FC RID: 10236
			public float Radius = 1f;

			// Token: 0x040027FD RID: 10237
			public float Sharpness = 50f;
		}
	}

	// Token: 0x02000484 RID: 1156
	[Serializable]
	public class NoiseSettings
	{
		// Token: 0x04002189 RID: 8585
		public const float DefaultOffsetTime = 0f;

		// Token: 0x0400218A RID: 8586
		public const float DefaultPower = 0.5f;

		// Token: 0x0400218B RID: 8587
		public Nightvision.NoiseSettings.NoiseMode Mode;

		// Token: 0x0400218C RID: 8588
		public Nightvision.NoiseSettings.TextureSettings Texture = new Nightvision.NoiseSettings.TextureSettings();

		// Token: 0x0400218D RID: 8589
		public float OffsetTime;

		// Token: 0x0400218E RID: 8590
		public float Power = 0.5f;

		// Token: 0x0200075B RID: 1883
		public enum NoiseMode
		{
			// Token: 0x040027FF RID: 10239
			Off,
			// Token: 0x04002800 RID: 10240
			Texture,
			// Token: 0x04002801 RID: 10241
			Procedural
		}

		// Token: 0x0200075C RID: 1884
		[Serializable]
		public class TextureSettings
		{
			// Token: 0x04002802 RID: 10242
			public const float DefaultScale = 1f;

			// Token: 0x04002803 RID: 10243
			public Texture Texture;

			// Token: 0x04002804 RID: 10244
			public float Scale = 1f;
		}
	}
}

using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class Nightvision : MonoBehaviour
{
	private void Awake()
	{
		this.nvMaterial = new Material(Shader.Find("Hidden/Nightvision"));
		this.UpdateMaterial();
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		this.UpdateMaterial();
		Graphics.Blit(src, dst, this.nvMaterial);
	}

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

	public const float DefaultPower = 20f;

	public float Power = 20f;

	public Nightvision.VignetteSettings Vignette = new Nightvision.VignetteSettings();

	public Nightvision.NoiseSettings Noise = new Nightvision.NoiseSettings();

	public Color EffectColor = Color.green;

	private Material nvMaterial;

	private Vector2 noiseOffset = Vector2.zero;

	private float lastTime;

	[Serializable]
	public class VignetteSettings
	{
		public Nightvision.VignetteSettings.VignetteMode Mode;

		public Color color = Color.black;

		public Nightvision.VignetteSettings.TextureSettings Texture = new Nightvision.VignetteSettings.TextureSettings();

		public Nightvision.VignetteSettings.ProceduralSettings Procedural = new Nightvision.VignetteSettings.ProceduralSettings();

		public enum VignetteMode
		{
			Off,
			Texture,
			Procedural
		}

		[Serializable]
		public class TextureSettings
		{
			public Texture Texture;
		}

		[Serializable]
		public class ProceduralSettings
		{
			public const float DefaultRadius = 1f;

			public const float DefaultSharpness = 50f;

			public float Radius = 1f;

			public float Sharpness = 50f;
		}
	}

	[Serializable]
	public class NoiseSettings
	{
		public const float DefaultOffsetTime = 0f;

		public const float DefaultPower = 0.5f;

		public Nightvision.NoiseSettings.NoiseMode Mode;

		public Nightvision.NoiseSettings.TextureSettings Texture = new Nightvision.NoiseSettings.TextureSettings();

		public float OffsetTime;

		public float Power = 0.5f;

		public enum NoiseMode
		{
			Off,
			Texture,
			Procedural
		}

		[Serializable]
		public class TextureSettings
		{
			public const float DefaultScale = 1f;

			public Texture Texture;

			public float Scale = 1f;
		}
	}
}


using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Fluorescence : MonoBehaviour
{
	private void Start()
	{
		this.mat = base.GetComponent<Renderer>().material;
	}

	private void Update()
	{
		this.mat.SetInt("_ColorCount", this.FluorescentColors.Length);
		this.mat.SetColorArray("_ColorArray", this.FluorescentColors);
		this.mat.SetColorArray("_ColorReplaceArray", this.ReplacementColors);
		this.mat.SetFloatArray("_PrecisionArray", this.CheckPrecisions);
	}

	public Color[] FluorescentColors;

	public Color[] ReplacementColors;

	public float[] CheckPrecisions;

	private Material mat;
}


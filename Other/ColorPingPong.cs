using System;
using UnityEngine;

public class ColorPingPong : MonoBehaviour
{
	private void Update()
	{
		this.mat.SetColor("_TintColor", Color.Lerp(this.colorA, this.colorB, Mathf.PingPong(Time.time / 30f, 1f)));
	}

	public Material mat;

	public Color colorA;

	public Color colorB;
}


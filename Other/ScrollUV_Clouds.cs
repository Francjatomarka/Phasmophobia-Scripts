using System;
using UnityEngine;

public class ScrollUV_Clouds : MonoBehaviour
{
	private void Start()
	{
		this._myRenderer = base.GetComponent<Renderer>();
		if (this._myRenderer == null)
		{
			base.enabled = false;
		}
	}

	public void FixedUpdate()
	{
		if (this.scroll)
		{
			float y = Time.time * this.verticalScrollSpeed;
			float x = Time.time * this.horizontalScrollSpeed;
			this._myRenderer.material.mainTextureOffset = new Vector2(x, y);
		}
	}

	public void DoActivateTrigger()
	{
		this.scroll = !this.scroll;
	}

	public float horizontalScrollSpeed = 0.25f;

	public float verticalScrollSpeed = 0.25f;

	private Renderer _myRenderer;

	private bool scroll = true;
}


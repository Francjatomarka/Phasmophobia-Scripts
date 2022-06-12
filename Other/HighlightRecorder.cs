using System;
using UnityEngine;

public class HighlightRecorder : MonoBehaviour
{
	private void Start()
	{
		this.rendererComp = base.GetComponent<Renderer>();
		if (this.rendererComp == null)
		{
			base.enabled = false;
			return;
		}
	}


	private Renderer rendererComp;
}


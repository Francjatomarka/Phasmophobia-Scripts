using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

public class OnClickFlashRpc : MonoBehaviourPunCallbacks
{
	private void OnClick()
	{
		base.photonView.RPC("Flash", RpcTarget.All, Array.Empty<object>());
	}

	[PunRPC]
	private IEnumerator Flash()
	{
		if (this.isFlashing)
		{
			yield break;
		}
		this.isFlashing = true;
		this.originalMaterial = base.GetComponent<Renderer>().material;
		if (!this.originalMaterial.HasProperty("_Emission"))
		{
			Debug.LogWarning("Doesnt have emission, can't flash " + base.gameObject);
			yield break;
		}
		this.originalColor = this.originalMaterial.GetColor("_Emission");
		this.originalMaterial.SetColor("_Emission", Color.white);
		for (float f = 0f; f <= 1f; f += 0.08f)
		{
			Color value = Color.Lerp(Color.white, this.originalColor, f);
			this.originalMaterial.SetColor("_Emission", value);
			yield return null;
		}
		this.originalMaterial.SetColor("_Emission", this.originalColor);
		this.isFlashing = false;
		yield break;
	}

	private Material originalMaterial;

	private Color originalColor;

	private bool isFlashing;
}


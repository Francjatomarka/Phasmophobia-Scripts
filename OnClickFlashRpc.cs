using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200003E RID: 62
public class OnClickFlashRpc : MonoBehaviourPunCallbacks
{
	// Token: 0x06000167 RID: 359 RVA: 0x0000A395 File Offset: 0x00008595
	private void OnClick()
	{
		base.photonView.RPC("Flash", RpcTarget.All, Array.Empty<object>());
	}

	// Token: 0x06000168 RID: 360 RVA: 0x0000A3AD File Offset: 0x000085AD
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

	// Token: 0x040001A8 RID: 424
	private Material originalMaterial;

	// Token: 0x040001A9 RID: 425
	private Color originalColor;

	// Token: 0x040001AA RID: 426
	private bool isFlashing;
}

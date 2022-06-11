using System;
using System.Collections;
using UnityEngine;
using Photon.Pun;

// Token: 0x020000FA RID: 250
[RequireComponent(typeof(PhotonView))]
public class AnimationObject : MonoBehaviour
{
	// Token: 0x060006BF RID: 1727 RVA: 0x00027C67 File Offset: 0x00025E67
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.anim.enabled = false;
	}

	// Token: 0x060006C0 RID: 1728 RVA: 0x00027C81 File Offset: 0x00025E81
	public void Use()
	{
		if (!this.anim.isActiveAndEnabled)
		{
			this.view.RPC("NetworkedUse", RpcTarget.All, Array.Empty<object>());
		}
	}

	// Token: 0x060006C1 RID: 1729 RVA: 0x00027CA6 File Offset: 0x00025EA6
	private IEnumerator PlayAnimation()
	{
		if (this.source != null)
		{
			this.source.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
			this.source.Play();
		}
		this.anim.enabled = true;
		yield return new WaitForSeconds(this.animTimer);
		this.anim.enabled = false;
		yield break;
	}

	// Token: 0x060006C2 RID: 1730 RVA: 0x00027CB5 File Offset: 0x00025EB5
	[PunRPC]
	private void NetworkedUse()
	{
		base.StartCoroutine(this.PlayAnimation());
	}

	// Token: 0x040006D4 RID: 1748
	private PhotonView view;

	// Token: 0x040006D5 RID: 1749
	[SerializeField]
	private Animator anim;

	// Token: 0x040006D6 RID: 1750
	[SerializeField]
	private AudioSource source;

	// Token: 0x040006D7 RID: 1751
	[SerializeField]
	private float animTimer;
}

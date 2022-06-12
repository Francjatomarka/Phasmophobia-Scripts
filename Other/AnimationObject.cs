using System;
using System.Collections;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class AnimationObject : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.anim.enabled = false;
	}

	public void Use()
	{
		if (!this.anim.isActiveAndEnabled)
		{
			this.view.RPC("NetworkedUse", RpcTarget.All, Array.Empty<object>());
		}
	}

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

	[PunRPC]
	private void NetworkedUse()
	{
		base.StartCoroutine(this.PlayAnimation());
	}

	private PhotonView view;

	[SerializeField]
	private Animator anim;

	[SerializeField]
	private AudioSource source;

	[SerializeField]
	private float animTimer;
}


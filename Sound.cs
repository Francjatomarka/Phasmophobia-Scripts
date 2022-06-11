using System;
using System.Collections;
using UnityEngine;
using Photon.Pun;

// Token: 0x02000107 RID: 263
[RequireComponent(typeof(PhotonView))]
public class Sound : MonoBehaviour
{
	// Token: 0x06000738 RID: 1848 RVA: 0x0002AF48 File Offset: 0x00029148
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.noise.gameObject.SetActive(false);
	}

	// Token: 0x06000739 RID: 1849 RVA: 0x0002AF67 File Offset: 0x00029167
	public void Use()
	{
		this.view.RPC("NetworkedUse", RpcTarget.All, new object[]
		{
			UnityEngine.Random.Range(0, this.clips.Length)
		});
	}

	// Token: 0x0600073A RID: 1850 RVA: 0x0002AF98 File Offset: 0x00029198
	[PunRPC]
	private void NetworkedUse(int id)
	{
		base.StartCoroutine(this.PlayNoiseObject());
		this.source.clip = this.clips[id];
		this.source.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		this.source.Play();
	}

	// Token: 0x0600073B RID: 1851 RVA: 0x0002AFF5 File Offset: 0x000291F5
	private IEnumerator PlayNoiseObject()
	{
		this.noise.gameObject.SetActive(true);
		yield return 0;
		this.noise.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x04000750 RID: 1872
	private PhotonView view;

	// Token: 0x04000751 RID: 1873
	[SerializeField]
	private Noise noise;

	// Token: 0x04000752 RID: 1874
	[SerializeField]
	private AudioClip[] clips;

	// Token: 0x04000753 RID: 1875
	[SerializeField]
	private AudioSource source;
}

using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x0200014D RID: 333
public class Window : MonoBehaviour
{
	// Token: 0x060008CD RID: 2253 RVA: 0x00034D18 File Offset: 0x00032F18
	private void Awake()
	{
		this.knockingSource = base.GetComponent<AudioSource>();
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x00034D32 File Offset: 0x00032F32
	private void Start()
	{
		if (this.handPrintObject != null)
		{
			this.handPrintObject.SetActive(false);
		}
	}

	// Token: 0x060008CF RID: 2255 RVA: 0x00034D4E File Offset: 0x00032F4E
	public void PlayKnockingSound()
	{
		this.view.RPC("PlayKnockingSoundSynced", RpcTarget.All, Array.Empty<object>());
	}

	// Token: 0x060008D0 RID: 2256 RVA: 0x00034D68 File Offset: 0x00032F68
	[PunRPC]
	private void PlayKnockingSoundSynced()
	{
		this.knockingSource.clip = this.windowAudioClips[UnityEngine.Random.Range(0, this.windowAudioClips.Length)];
		this.knockingSource.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		this.knockingSource.Play();
	}

	// Token: 0x060008D1 RID: 2257 RVA: 0x00034DC5 File Offset: 0x00032FC5
	public void SpawnHandPrintEvidence()
	{
		if (this.handPrintObject == null)
		{
			return;
		}
		if (this.handPrintObject.activeInHierarchy)
		{
			return;
		}
		this.view.RPC("NetworkedSpawnHandPrintEvidence", RpcTarget.All, Array.Empty<object>());
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x00034DFA File Offset: 0x00032FFA
	[PunRPC]
	private void NetworkedSpawnHandPrintEvidence()
	{
		this.handPrintObject.GetComponent<Renderer>().material = EvidenceController.instance.handPrintMaterials[UnityEngine.Random.Range(0, EvidenceController.instance.handPrintMaterials.Length)];
		this.handPrintObject.SetActive(true);
	}

	// Token: 0x040008EC RID: 2284
	[HideInInspector]
	public PhotonView view;

	// Token: 0x040008ED RID: 2285
	private AudioSource knockingSource;

	// Token: 0x040008EE RID: 2286
	[SerializeField]
	private AudioClip[] windowAudioClips;

	// Token: 0x040008EF RID: 2287
	[SerializeField]
	private GameObject handPrintObject;

	// Token: 0x040008F0 RID: 2288
	public Transform windowGhostStart;

	// Token: 0x040008F1 RID: 2289
	public Transform windowGhostEnd;
}

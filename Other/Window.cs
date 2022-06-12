using System;
using UnityEngine;
using Photon.Pun;

public class Window : MonoBehaviour
{
	private void Awake()
	{
		this.knockingSource = base.GetComponent<AudioSource>();
		this.view = base.GetComponent<PhotonView>();
	}

	private void Start()
	{
		if (this.handPrintObject != null)
		{
			this.handPrintObject.SetActive(false);
		}
	}

	public void PlayKnockingSound()
	{
		this.view.RPC("PlayKnockingSoundSynced", RpcTarget.All, Array.Empty<object>());
	}

	[PunRPC]
	private void PlayKnockingSoundSynced()
	{
		this.knockingSource.clip = this.windowAudioClips[UnityEngine.Random.Range(0, this.windowAudioClips.Length)];
		this.knockingSource.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		this.knockingSource.Play();
	}

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

	[PunRPC]
	private void NetworkedSpawnHandPrintEvidence()
	{
		this.handPrintObject.GetComponent<Renderer>().material = EvidenceController.instance.handPrintMaterials[UnityEngine.Random.Range(0, EvidenceController.instance.handPrintMaterials.Length)];
		this.handPrintObject.SetActive(true);
	}

	[HideInInspector]
	public PhotonView view;

	private AudioSource knockingSource;

	[SerializeField]
	private AudioClip[] windowAudioClips;

	[SerializeField]
	private GameObject handPrintObject;

	public Transform windowGhostStart;

	public Transform windowGhostEnd;
}


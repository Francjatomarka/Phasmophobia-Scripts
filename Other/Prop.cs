using System;
using UnityEngine;
using Photon.Pun;

public class Prop : MonoBehaviour
{
	private void Awake()
	{
		if (this.photonInteract == null)
		{
			this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		}
		if (this.body == null)
		{
			this.body = base.GetComponent<Rigidbody>();
		}
		if (this.view == null)
		{
			this.view = base.GetComponent<PhotonView>();
		}
	}

	private void Start()
	{
		if (PhotonNetwork.InRoom && !this.view.IsMine)
		{
			this.body.isKinematic = true;
			this.body.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (this.photonInteract.isGrabbed)
		{
			return;
		}
		if (collision.relativeVelocity.magnitude > 1f)
		{
			if (ObjectPooler.instance == null)
			{
				return;
			}
			if (ObjectPooler.instance.poolDictionary == null)
			{
				return;
			}
			Noise component = ObjectPooler.instance.SpawnFromPool("Noise", base.transform.position, Quaternion.identity).GetComponent<Noise>();
			float volume = Mathf.Clamp(0.15f * (collision.relativeVelocity.magnitude / 6f), 0f, 0.15f);
			if (this.impactClips.Length != 0)
			{
				component.PlaySound(this.impactClips[UnityEngine.Random.Range(0, this.impactClips.Length)], volume);
				return;
			}
			component.PlaySound(SoundController.instance.genericImpactClips[UnityEngine.Random.Range(0, SoundController.instance.genericImpactClips.Length)], volume);
		}
	}

	public PhotonObjectInteract photonInteract;

	[SerializeField]
	private Rigidbody body;

	[SerializeField]
	private PhotonView view;

	[SerializeField]
	private AudioClip[] impactClips;
}


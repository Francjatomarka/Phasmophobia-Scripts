using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x02000147 RID: 327
public class Prop : MonoBehaviour
{
	// Token: 0x060008AC RID: 2220 RVA: 0x000345EC File Offset: 0x000327EC
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

	// Token: 0x060008AD RID: 2221 RVA: 0x00034647 File Offset: 0x00032847
	private void Start()
	{
		if (PhotonNetwork.InRoom && !this.view.IsMine)
		{
			this.body.isKinematic = true;
			this.body.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
		}
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x00034678 File Offset: 0x00032878
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

	// Token: 0x040008CD RID: 2253
	public PhotonObjectInteract photonInteract;

	// Token: 0x040008CE RID: 2254
	[SerializeField]
	private Rigidbody body;

	// Token: 0x040008CF RID: 2255
	[SerializeField]
	private PhotonView view;

	// Token: 0x040008D0 RID: 2256
	[SerializeField]
	private AudioClip[] impactClips;
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

// Token: 0x0200016D RID: 365
public class FootstepController : MonoBehaviour
{
	// Token: 0x06000A5C RID: 2652 RVA: 0x0003FCFC File Offset: 0x0003DEFC
	private void Awake()
	{
		this.noise.gameObject.SetActive(false);
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x0003FD0F File Offset: 0x0003DF0F
	private float GetCurrentSpeed()
	{
		if (this.XAxisSpeed < this.YAxisSpeed)
		{
			return this.YAxisSpeed;
		}
		return this.XAxisSpeed;
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x0003FD2C File Offset: 0x0003DF2C
	private void Update()
	{
		if (PhotonNetwork.InRoom && !this.view.IsMine)
		{
			return;
		}
		if (XRDevice.isPresent)
		{
			return;
		}
		this.isWalking = (this.player.firstPersonController.enabled && this.player.charController.velocity.magnitude > 0.5f);
		if (this.isWalking)
		{
			if (this.player.pcCanvas && this.player.pcCanvas.isPaused)
			{
				return;
			}
			this.walkTimer -= Time.deltaTime;
			if (this.walkTimer < 0f)
			{
				this.PlaySound();
				this.walkTimer = 0.7f;
			}
		}
	}

	// Token: 0x06000A5F RID: 2655 RVA: 0x0003FDF0 File Offset: 0x0003DFF0
	public void PlaySound()
	{
		if (this.source.isPlaying)
		{
			return;
		}
		if (GameController.instance && GameController.instance.isLeavingLevel)
		{
			return;
		}
		RaycastHit raycastHit;
		if (Physics.Linecast(base.transform.position, base.transform.position + Vector3.down * 3f, out raycastHit, this.mask, QueryTriggerInteraction.Ignore))
		{
			string tag = raycastHit.collider.tag;
			if (!(tag == "Carpet"))
			{
				if (!(tag == "Wood"))
				{
					if (!(tag == "Concrete"))
					{
						if (!(tag == "Stairs"))
						{
							if (tag == "Grass")
							{
								this.id = 4;
							}
						}
						else
						{
							this.id = 3;
						}
					}
					else
					{
						this.id = 2;
					}
				}
				else
				{
					this.id = 1;
				}
			}
			else
			{
				this.id = 0;
			}
			if (PhotonNetwork.InRoom)
			{
				this.view.RPC("NetworkedPlaySound", RpcTarget.All, new object[]
				{
					this.id,
					PhotonNetwork.LocalPlayer.UserId
				});
				return;
			}
			this.NetworkedPlaySound(this.id, 0);
		}
	}

	// Token: 0x06000A60 RID: 2656 RVA: 0x0003FF30 File Offset: 0x0003E130
	[PunRPC]
	private void NetworkedPlaySound(int id, int actorID)
	{
		if (id == 0)
		{
			this.source.volume = 0.1f;
			this.source.clip = this.carpetAudioClips[UnityEngine.Random.Range(0, this.carpetAudioClips.Length)];
		}
		else if (id == 1)
		{
			this.source.volume = 0.1f;
			this.source.clip = this.woodAudioClips[UnityEngine.Random.Range(0, this.woodAudioClips.Length)];
		}
		else if (id == 2)
		{
			this.source.volume = 0.4f;
			this.source.clip = this.concreteAudioClips[UnityEngine.Random.Range(0, this.concreteAudioClips.Length)];
		}
		else if (id == 3)
		{
			this.source.volume = 0.6f;
			this.source.clip = this.stairsAudioClips[UnityEngine.Random.Range(0, this.stairsAudioClips.Length)];
		}
		else if (id == 4)
		{
			this.source.volume = 0.6f;
			this.source.clip = this.terrainAudioClips[UnityEngine.Random.Range(0, this.terrainAudioClips.Length)];
		}
		this.source.outputAudioMixerGroup = SoundController.instance.GetPlayersAudioGroup(actorID);
		this.source.Play();
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.PlayNoiseObject());
		}
	}

	// Token: 0x06000A61 RID: 2657 RVA: 0x00040089 File Offset: 0x0003E289
	private IEnumerator PlayNoiseObject()
	{
		this.noise.gameObject.SetActive(true);
		yield return new WaitForEndOfFrame();
		this.noise.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x04000A7D RID: 2685
	[SerializeField]
	private AudioSource source;

	// Token: 0x04000A7E RID: 2686
	[SerializeField]
	private PhotonView view;

	// Token: 0x04000A7F RID: 2687
	[SerializeField]
	private Player player;

	// Token: 0x04000A80 RID: 2688
	[SerializeField]
	private AudioClip[] carpetAudioClips;

	// Token: 0x04000A81 RID: 2689
	[SerializeField]
	private AudioClip[] concreteAudioClips;

	// Token: 0x04000A82 RID: 2690
	[SerializeField]
	private AudioClip[] woodAudioClips;

	// Token: 0x04000A83 RID: 2691
	[SerializeField]
	private AudioClip[] stairsAudioClips;

	// Token: 0x04000A84 RID: 2692
	[SerializeField]
	private AudioClip[] terrainAudioClips;

	// Token: 0x04000A85 RID: 2693
	[SerializeField]
	private Noise noise;

	// Token: 0x04000A86 RID: 2694
	public float XAxisSpeed;

	// Token: 0x04000A87 RID: 2695
	public float YAxisSpeed;

	// Token: 0x04000A88 RID: 2696
	public bool isWalking;

	// Token: 0x04000A89 RID: 2697
	private float walkTimer = 0.7f;

	// Token: 0x04000A8A RID: 2698
	[SerializeField]
	private LayerMask mask;

	// Token: 0x04000A8B RID: 2699
	private int id;
}

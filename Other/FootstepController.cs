using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

public class FootstepController : MonoBehaviour
{
	private void Awake()
	{
		this.noise.gameObject.SetActive(false);
	}

	private float GetCurrentSpeed()
	{
		if (this.XAxisSpeed < this.YAxisSpeed)
		{
			return this.YAxisSpeed;
		}
		return this.XAxisSpeed;
	}

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

	private IEnumerator PlayNoiseObject()
	{
		this.noise.gameObject.SetActive(true);
		yield return new WaitForEndOfFrame();
		this.noise.gameObject.SetActive(false);
		yield break;
	}

	[SerializeField]
	private AudioSource source;

	[SerializeField]
	private PhotonView view;

	[SerializeField]
	private Player player;

	[SerializeField]
	private AudioClip[] carpetAudioClips;

	[SerializeField]
	private AudioClip[] concreteAudioClips;

	[SerializeField]
	private AudioClip[] woodAudioClips;

	[SerializeField]
	private AudioClip[] stairsAudioClips;

	[SerializeField]
	private AudioClip[] terrainAudioClips;

	[SerializeField]
	private Noise noise;

	public float XAxisSpeed;

	public float YAxisSpeed;

	public bool isWalking;

	private float walkTimer = 0.7f;

	[SerializeField]
	private LayerMask mask;

	private int id;
}


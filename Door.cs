using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

// Token: 0x020000FD RID: 253
[RequireComponent(typeof(PhotonView))]
public class Door : MonoBehaviour
{
	// Token: 0x060006D1 RID: 1745 RVA: 0x0002811C File Offset: 0x0002631C
	private void Awake()
	{
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.view = base.GetComponent<PhotonView>();
		this.col = base.GetComponent<BoxCollider>();
		this.body = base.GetComponent<Rigidbody>();
		if (this.noise != null)
		{
			this.noise.gameObject.SetActive(false);
		}
		this.closed = true;
		this.unlockTimer = UnityEngine.Random.Range(10f, 20f);
		if (this.locked)
		{
			this.body.constraints = RigidbodyConstraints.FreezeAll;
		}
	}

	// Token: 0x060006D2 RID: 1746 RVA: 0x000281AC File Offset: 0x000263AC
	private void Start()
	{
		if (this.handPrintObject != null)
		{
			this.handPrintObject.SetActive(false);
		}
		if (this.type != Key.KeyType.main)
		{
			if (this.lockSource)
			{
				this.lockSource.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
			}
			if (this.loopSource)
			{
				this.loopSource.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
			}
		}
	}

	// Token: 0x060006D3 RID: 1747 RVA: 0x00028240 File Offset: 0x00026440
	private void Update()
	{
		if (this.locked)
		{
			if (this.hasBeenUnlocked && PhotonNetwork.IsMasterClient && !LevelController.instance.currentGhost.isHunting)
			{
				this.unlockTimer -= Time.deltaTime;
				if (this.unlockTimer < 0f)
				{
					this.UnlockDoor();
					this.unlockTimer = UnityEngine.Random.Range(10f, 20f);
					return;
				}
			}
		}
		else if (Time.frameCount % 3 == 0)
		{
			if (!this.closed)
			{
				if (!this.loopSource.isPlaying)
				{
					this.loopSource.Play();
				}
				this.loopSource.volume = this.body.velocity.magnitude / this.loopVolumeDivide;
				return;
			}
			if (this.loopSource.isPlaying)
			{
				this.loopSource.Stop();
			}
		}
	}

	// Token: 0x060006D4 RID: 1748 RVA: 0x00028323 File Offset: 0x00026523
	public void DisableOrEnableCollider(bool active)
	{
		this.view.RPC("DisableOrEnableColliderNetworked", RpcTarget.All, new object[]
		{
			active
		});
	}

	// Token: 0x060006D5 RID: 1749 RVA: 0x00028345 File Offset: 0x00026545
	[PunRPC]
	private void DisableOrEnableColliderNetworked(bool active)
	{
		this.col.enabled = active;
	}

	// Token: 0x060006D6 RID: 1750 RVA: 0x00028353 File Offset: 0x00026553
	[PunRPC]
	private void NetworkedGrabbedDoor()
	{
		this.closed = false;
	}

	// Token: 0x060006D7 RID: 1751 RVA: 0x0002835C File Offset: 0x0002655C
	public void AttemptToUnlockDoor()
	{
		bool flag = false;
		if (this.locked)
		{
			if (!LevelController.instance.currentGhost.isHunting)
			{
				for (int i = 0; i < GameController.instance.myPlayer.player.keys.Count; i++)
				{
					if (GameController.instance.myPlayer.player.keys[i] == this.type)
					{
						this.UnlockDoor();
						flag = true;
					}
				}
			}
			if (!flag)
			{
				this.view.RPC("NetworkedPlayLockSound", RpcTarget.All, Array.Empty<object>());
			}
		}
	}

	// Token: 0x060006D8 RID: 1752 RVA: 0x000283EC File Offset: 0x000265EC
	public void GrabbedDoor()
	{
		bool flag = false;
		if (!XRDevice.isPresent)
		{
			this.body.isKinematic = false;
			this.body.useGravity = true;
		}
		if (this.locked)
		{
			if (!LevelController.instance.currentGhost.isHunting)
			{
				for (int i = 0; i < GameController.instance.myPlayer.player.keys.Count; i++)
				{
					if (GameController.instance.myPlayer.player.keys[i] == this.type)
					{
						this.UnlockDoor();
						flag = true;
					}
				}
				if (!flag)
				{
					this.view.RPC("NetworkedPlayLockSound", RpcTarget.All, Array.Empty<object>());
				}
			}
		}
		else
		{
			flag = true;
		}
		if (flag)
		{
			this.view.RPC("NetworkedGrabbedDoor", RpcTarget.All, Array.Empty<object>());
		}
	}

	// Token: 0x060006D9 RID: 1753 RVA: 0x000284B8 File Offset: 0x000266B8
	public void UnGrabbedDoor()
	{
		this.body.mass = 100f;
		this.body.isKinematic = true;
		this.body.useGravity = false;
		Quaternion localRotation = base.transform.localRotation;
		Vector3 eulerAngles = localRotation.eulerAngles;
		if ((eulerAngles.y < this.closedYRot + 7f && !this.isReversed) || (eulerAngles.y > this.closedYRot - 7f && this.isReversed))
		{
			eulerAngles.y = this.closedYRot;
			this.view.RPC("NetworkedPlayClosedSound", RpcTarget.All, Array.Empty<object>());
			localRotation.eulerAngles = eulerAngles;
			base.transform.localRotation = localRotation;
		}
		if (!PhotonNetwork.IsMasterClient)
		{
			this.view.RPC("GiveControlToMasterClient", RpcTarget.MasterClient, Array.Empty<object>());
		}
	}

	// Token: 0x060006DA RID: 1754 RVA: 0x0002858D File Offset: 0x0002678D
	[PunRPC]
	private void GiveControlToMasterClient()
	{
		this.view.RequestOwnership();
	}

	// Token: 0x060006DB RID: 1755 RVA: 0x0002859A File Offset: 0x0002679A
	public void TrailerCloseDoor()
	{
		base.transform.localRotation = new Quaternion(0f, this.closedYRot, 0f, 0f);
		this.view.RPC("NetworkedPlayClosedSound", RpcTarget.All, Array.Empty<object>());
	}

	// Token: 0x060006DC RID: 1756 RVA: 0x000285D8 File Offset: 0x000267D8
	public void UnlockDoor()
	{
		if (!this.locked)
		{
			return;
		}
		this.body.constraints = RigidbodyConstraints.None;
		this.locked = false;
		if (!this.hasBeenUnlocked)
		{
			this.view.RPC("SyncHasBeenLocked", RpcTarget.AllBuffered, Array.Empty<object>());
		}
		this.view.RPC("SyncLockState", RpcTarget.AllBuffered, new object[]
		{
			this.locked
		});
		this.view.RPC("NetworkedPlayUnlockSound", RpcTarget.All, Array.Empty<object>());
	}

	// Token: 0x060006DD RID: 1757 RVA: 0x0002865C File Offset: 0x0002685C
	public void LockDoor()
	{
		if (this.locked)
		{
			return;
		}
		this.body.constraints = RigidbodyConstraints.FreezeAll;
		if ((base.transform.localEulerAngles.y < this.closedYRot + 12f && !this.isReversed) || (base.transform.localEulerAngles.y > this.closedYRot - 12f && this.isReversed))
		{
			base.transform.localEulerAngles = new Vector3(0f, this.closedYRot, 0f);
			this.view.RPC("NetworkedPlayClosedSound", RpcTarget.All, Array.Empty<object>());
		}
		if (!this.closed)
		{
			return;
		}
		this.locked = true;
		this.view.RPC("SyncLockState", RpcTarget.AllBuffered, new object[]
		{
			this.locked
		});
		this.view.RPC("NetworkedPlayLockSound", RpcTarget.All, Array.Empty<object>());
		if (this.type == Key.KeyType.main)
		{
			if (!LevelController.instance.currentGhost.isHunting)
			{
				this.view.RPC("UnlockDoorTimer", RpcTarget.AllBuffered, new object[]
				{
					UnityEngine.Random.Range(5f, 20f)
				});
				return;
			}
		}
		else
		{
			this.view.RPC("UnlockDoorTimer", RpcTarget.AllBuffered, new object[]
			{
				UnityEngine.Random.Range(5f, 20f)
			});
		}
	}

	// Token: 0x060006DE RID: 1758 RVA: 0x000287C4 File Offset: 0x000269C4
	[PunRPC]
	private void SyncLockState(bool isLocked)
	{
		this.locked = isLocked;
		this.body.constraints = (isLocked ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.None);
		this.unlockTimer = UnityEngine.Random.Range(10f, 20f);
		if (this.type == Key.KeyType.main)
		{
			SetupPhaseController.instance.mainDoorHasUnlocked = true;
		}
	}

	// Token: 0x060006DF RID: 1759 RVA: 0x00028813 File Offset: 0x00026A13
	[PunRPC]
	private void SyncHasBeenLocked()
	{
		this.hasBeenUnlocked = true;
	}

	// Token: 0x060006E0 RID: 1760 RVA: 0x0002881C File Offset: 0x00026A1C
	public void PlayLockedSound()
	{
		this.view.RPC("NetworkedPlayLockSound", RpcTarget.All, Array.Empty<object>());
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x00028834 File Offset: 0x00026A34
	[PunRPC]
	private void NetworkedPlayLockSound()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.lockSource.volume = 0.4f;
		this.lockSource.clip = this.doorLockClips[UnityEngine.Random.Range(0, this.doorLockClips.Length)];
		this.lockSource.Play();
		if (this.noise != null)
		{
			base.StartCoroutine(this.PlayNoiseObject(0.12f));
		}
	}

	// Token: 0x060006E2 RID: 1762 RVA: 0x000288AC File Offset: 0x00026AAC
	[PunRPC]
	private void NetworkedPlayUnlockSound()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.lockSource.volume = 0.4f;
		this.lockSource.clip = this.doorUnlockClips[UnityEngine.Random.Range(0, this.doorUnlockClips.Length)];
		this.lockSource.Play();
		if (this.noise != null)
		{
			base.StartCoroutine(this.PlayNoiseObject(0.11f));
		}
	}

	// Token: 0x060006E3 RID: 1763 RVA: 0x00028924 File Offset: 0x00026B24
	[PunRPC]
	private void NetworkedPlayClosedSound()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.closed = true;
		if (!this.lockSource.isPlaying)
		{
			this.lockSource.volume = this.closedVolume;
			this.lockSource.clip = this.doorClosedClips[UnityEngine.Random.Range(0, this.doorClosedClips.Length)];
			this.lockSource.Play();
			if (this.noise != null)
			{
				base.StartCoroutine(this.PlayNoiseObject(0.4f));
			}
		}
	}

	// Token: 0x060006E4 RID: 1764 RVA: 0x000289AF File Offset: 0x00026BAF
	[PunRPC]
	private IEnumerator UnlockDoorTimer(float timer)
	{
		yield return new WaitForSeconds(timer);
		if (this.locked)
		{
			this.UnlockDoor();
		}
		yield break;
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x000289C5 File Offset: 0x00026BC5
	public void SpawnHandPrintEvidence()
	{
		if (this.handPrintObject == null)
		{
			return;
		}
		this.view.RPC("NetworkedSpawnHandPrintEvidence", RpcTarget.All, Array.Empty<object>());
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x000289EC File Offset: 0x00026BEC
	[PunRPC]
	private void NetworkedSpawnHandPrintEvidence()
	{
		this.handPrintObject.GetComponent<Renderer>().material = this.handPrintMaterials[UnityEngine.Random.Range(0, this.handPrintMaterials.Length)];
		this.handPrintObject.SetActive(true);
	}

	// Token: 0x060006E7 RID: 1767 RVA: 0x00028A1F File Offset: 0x00026C1F
	public void DisableOrEnableDoor(bool activate)
	{
		if (activate)
		{
			this.view.RPC("EnableDoorNetworked", RpcTarget.All, Array.Empty<object>());
			return;
		}
		this.view.RPC("ForceDropDoorNetworked", RpcTarget.All, Array.Empty<object>());
	}

	// Token: 0x060006E8 RID: 1768 RVA: 0x00028A54 File Offset: 0x00026C54
	[PunRPC]
	private void ForceDropDoorNetworked()
	{
		this.photonInteract.isGrabbed = false;
		this.canBeGrabbed = false;
		if (!XRDevice.isPresent && GameController.instance.myPlayer.player.dragRigidBodyUse.objectHeld == base.gameObject)
		{
			GameController.instance.myPlayer.player.dragRigidBodyUse.DropObject();
		}
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x00028ABA File Offset: 0x00026CBA
	[PunRPC]
	private void EnableDoorNetworked()
	{
		this.photonInteract.isGrabbed = true;
		this.canBeGrabbed = true;
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x00028ACF File Offset: 0x00026CCF
	private IEnumerator PlayNoiseObject(float volume)
	{
		this.noise.volume = volume;
		this.noise.gameObject.SetActive(true);
		yield return 0;
		this.noise.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x040006ED RID: 1773
	public bool locked;

	// Token: 0x040006EE RID: 1774
	public bool closed;

	// Token: 0x040006EF RID: 1775
	[SerializeField]
	private AudioClip[] doorLockClips;

	// Token: 0x040006F0 RID: 1776
	[SerializeField]
	private AudioClip[] doorUnlockClips;

	// Token: 0x040006F1 RID: 1777
	[SerializeField]
	private AudioClip[] doorClosedClips;

	// Token: 0x040006F2 RID: 1778
	[SerializeField]
	private float closedVolume = 0.1f;

	// Token: 0x040006F3 RID: 1779
	[SerializeField]
	private AudioSource lockSource;

	// Token: 0x040006F4 RID: 1780
	[SerializeField]
	private AudioSource loopSource;

	// Token: 0x040006F5 RID: 1781
	[SerializeField]
	private float loopVolumeDivide = 5f;

	// Token: 0x040006F6 RID: 1782
	[SerializeField]
	private bool hasBeenUnlocked;

	// Token: 0x040006F7 RID: 1783
	private float unlockTimer = 15f;

	public bool canBeGrabbed = true;
	public PhotonView view;

	public Rigidbody body;

	// Token: 0x040006FB RID: 1787
	public PhotonObjectInteract photonInteract;

	// Token: 0x040006FC RID: 1788
	[SerializeField]
	private GameObject handPrintObject;

	// Token: 0x040006FD RID: 1789
	[SerializeField]
	private Material[] handPrintMaterials;

	public BoxCollider col;

	// Token: 0x040006FF RID: 1791
	[SerializeField]
	private bool isReversed;

	// Token: 0x04000700 RID: 1792
	public float closedYRot;

	// Token: 0x04000701 RID: 1793
	[SerializeField]
	private Noise noise;

	// Token: 0x04000702 RID: 1794
	public Renderer rend;

	// Token: 0x04000703 RID: 1795
	public Key.KeyType type;
}

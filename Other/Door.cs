using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class Door : MonoBehaviour
{
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

	public void DisableOrEnableCollider(bool active)
	{
		this.view.RPC("DisableOrEnableColliderNetworked", RpcTarget.All, new object[]
		{
			active
		});
	}

	[PunRPC]
	private void DisableOrEnableColliderNetworked(bool active)
	{
		this.col.enabled = active;
	}

	[PunRPC]
	private void NetworkedGrabbedDoor()
	{
		this.closed = false;
	}

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

	[PunRPC]
	private void GiveControlToMasterClient()
	{
		this.view.RequestOwnership();
	}

	public void TrailerCloseDoor()
	{
		base.transform.localRotation = new Quaternion(0f, this.closedYRot, 0f, 0f);
		this.view.RPC("NetworkedPlayClosedSound", RpcTarget.All, Array.Empty<object>());
	}

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

	[PunRPC]
	private void SyncHasBeenLocked()
	{
		this.hasBeenUnlocked = true;
	}

	public void PlayLockedSound()
	{
		this.view.RPC("NetworkedPlayLockSound", RpcTarget.All, Array.Empty<object>());
	}

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

	public void SpawnHandPrintEvidence()
	{
		if (this.handPrintObject == null)
		{
			return;
		}
		this.view.RPC("NetworkedSpawnHandPrintEvidence", RpcTarget.All, Array.Empty<object>());
	}

	[PunRPC]
	private void NetworkedSpawnHandPrintEvidence()
	{
		this.handPrintObject.GetComponent<Renderer>().material = this.handPrintMaterials[UnityEngine.Random.Range(0, this.handPrintMaterials.Length)];
		this.handPrintObject.SetActive(true);
	}

	public void DisableOrEnableDoor(bool activate)
	{
		if (activate)
		{
			this.view.RPC("EnableDoorNetworked", RpcTarget.All, Array.Empty<object>());
			return;
		}
		this.view.RPC("ForceDropDoorNetworked", RpcTarget.All, Array.Empty<object>());
	}

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

	[PunRPC]
	private void EnableDoorNetworked()
	{
		this.photonInteract.isGrabbed = true;
		this.canBeGrabbed = true;
	}

	private IEnumerator PlayNoiseObject(float volume)
	{
		this.noise.volume = volume;
		this.noise.gameObject.SetActive(true);
		yield return 0;
		this.noise.gameObject.SetActive(false);
		yield break;
	}

	public bool locked;

	public bool closed;

	[SerializeField]
	private AudioClip[] doorLockClips;

	[SerializeField]
	private AudioClip[] doorUnlockClips;

	[SerializeField]
	private AudioClip[] doorClosedClips;

	[SerializeField]
	private float closedVolume = 0.1f;

	[SerializeField]
	private AudioSource lockSource;

	[SerializeField]
	private AudioSource loopSource;

	[SerializeField]
	private float loopVolumeDivide = 5f;

	[SerializeField]
	private bool hasBeenUnlocked;

	private float unlockTimer = 15f;

	public bool canBeGrabbed = true;
	public PhotonView view;

	public Rigidbody body;

	public PhotonObjectInteract photonInteract;

	[SerializeField]
	private GameObject handPrintObject;

	[SerializeField]
	private Material[] handPrintMaterials;

	public BoxCollider col;

	[SerializeField]
	private bool isReversed;

	public float closedYRot;

	[SerializeField]
	private Noise noise;

	public Renderer rend;

	public Key.KeyType type;
}


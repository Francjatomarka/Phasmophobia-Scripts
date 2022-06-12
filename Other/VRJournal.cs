using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class VRJournal : MonoBehaviourPunCallbacks
{
	private void Awake()
	{
		this.rend = base.GetComponent<Renderer>();
		this.view = base.GetComponent<PhotonView>();
		this.boxCollider = base.GetComponent<BoxCollider>();
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.localPosition = new Vector3(0.4f, 0f, 0f);
		this.rend.enabled = false;
	}

	private void Start()
	{
		this.Initialise();
	}

	private void Initialise()
	{
		if (this.view.IsMine || !PhotonNetwork.InRoom)
		{
			if (XRDevice.isPresent)
			{
				if (LevelController.instance)
				{
					LevelController.instance.journalController.gameObject.SetActive(false);
					LevelController.instance.journalController = this.journalController;
				}
				this.boxCollider.center = new Vector3(0f, 0f, 0f);
				this.boxCollider.size = new Vector3(0.4f, 0.4f, 0.4f);
				this.photonInteract.AddGrabbedEvent(new UnityAction(this.OnGrab));
				this.photonInteract.AddUnGrabbedEvent(new UnityAction(this.UnGrabbed));
				if (GameController.instance)
				{
					if (GameController.instance.myPlayer != null)
					{
						base.Invoke("ResetTransform", 5f);
					}
					else
					{
						GameController.instance.OnLocalPlayerSpawned.AddListener(new UnityAction(this.DelayReset));
					}
				}
				else
				{
					base.Invoke("ResetTransform", 5f);
				}
			}
		}
		else
		{
			this.boxCollider.enabled = false;
		}
		this.journalController.gameObject.SetActive(false);
	}

	private void UnGrabbed()
	{
		if (GameController.instance)
		{
			GameController.instance.myPlayer.player.movementSettings.InMenuOrJournal(false);
		}
		else
		{
			MainManager.instance.localPlayer.movementSettings.InMenuOrJournal(false);
		}
		this.boxCollider.center = new Vector3(0f, 0f, 0f);
		this.boxCollider.size = new Vector3(0.4f, 0.4f, 0.4f);
		if (PhotonNetwork.InRoom)
		{
			this.view.RPC("ShowOrHide", RpcTarget.All, new object[]
			{
				false
			});
		}
		else
		{
			this.ShowOrHide(false);
		}
		this.ResetTransform();
	}

	private void DelayReset()
	{
		GameController.instance.OnLocalPlayerSpawned.RemoveListener(new UnityAction(this.DelayReset));
		base.Invoke("ResetTransform", 5f);
	}

	private void ResetTransform()
	{
		if (GameController.instance)
		{
			base.transform.SetParent(GameController.instance.myPlayer.player.cam.transform);
		}
		else
		{
			base.transform.SetParent(MainManager.instance.localPlayer.cam.transform);
		}
		base.transform.localPosition = this.localPosition;
		base.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
	}

	private void OnGrab()
	{
		if (GameController.instance)
		{
			GameController.instance.myPlayer.player.movementSettings.InMenuOrJournal(true);
		}
		else
		{
			MainManager.instance.localPlayer.movementSettings.InMenuOrJournal(true);
		}
		this.boxCollider.center = new Vector3(-0.025f, 0.0175f, 0.15f);
		this.boxCollider.size = new Vector3(0.45f, 0.035f, 0.3f);
		if (PhotonNetwork.InRoom)
		{
			this.view.RPC("ShowOrHide", RpcTarget.All, new object[]
			{
				true
			});
		}
		else
		{
			this.ShowOrHide(true);
		}
		this.openSource.clip = this.openClips[UnityEngine.Random.Range(0, this.openClips.Length)];
		if (!this.openSource.isPlaying)
		{
			this.openSource.Play();
		}
	}

	[PunRPC]
	private void ShowOrHide(bool show)
	{
		this.rend.enabled = show;
		this.journalController.gameObject.SetActive(show);
	}

	private PhotonView view;

	private Renderer rend;

	private PhotonObjectInteract photonInteract;

	private Vector3 localPosition;

	private BoxCollider boxCollider;

	[SerializeField]
	private JournalController journalController;

	[SerializeField]
	private AudioSource openSource;

	[SerializeField]
	private AudioClip[] openClips;
}


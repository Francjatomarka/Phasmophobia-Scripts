using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

// Token: 0x02000185 RID: 389
public class VRJournal : MonoBehaviourPunCallbacks
{
	// Token: 0x06000B0D RID: 2829 RVA: 0x00045EE0 File Offset: 0x000440E0
	private void Awake()
	{
		this.rend = base.GetComponent<Renderer>();
		this.view = base.GetComponent<PhotonView>();
		this.boxCollider = base.GetComponent<BoxCollider>();
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.localPosition = new Vector3(0.4f, 0f, 0f);
		this.rend.enabled = false;
	}

	// Token: 0x06000B0E RID: 2830 RVA: 0x00045F43 File Offset: 0x00044143
	private void Start()
	{
		this.Initialise();
	}

	// Token: 0x06000B0F RID: 2831 RVA: 0x00045F4C File Offset: 0x0004414C
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

	// Token: 0x06000B10 RID: 2832 RVA: 0x00046090 File Offset: 0x00044290
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

	// Token: 0x06000B11 RID: 2833 RVA: 0x0004614E File Offset: 0x0004434E
	private void DelayReset()
	{
		GameController.instance.OnLocalPlayerSpawned.RemoveListener(new UnityAction(this.DelayReset));
		base.Invoke("ResetTransform", 5f);
	}

	// Token: 0x06000B12 RID: 2834 RVA: 0x0004617C File Offset: 0x0004437C
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

	// Token: 0x06000B13 RID: 2835 RVA: 0x00046210 File Offset: 0x00044410
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

	// Token: 0x06000B14 RID: 2836 RVA: 0x00046300 File Offset: 0x00044500
	[PunRPC]
	private void ShowOrHide(bool show)
	{
		this.rend.enabled = show;
		this.journalController.gameObject.SetActive(show);
	}

	// Token: 0x04000B98 RID: 2968
	private PhotonView view;

	// Token: 0x04000B99 RID: 2969
	private Renderer rend;

	// Token: 0x04000B9A RID: 2970
	private PhotonObjectInteract photonInteract;

	// Token: 0x04000B9B RID: 2971
	private Vector3 localPosition;

	// Token: 0x04000B9C RID: 2972
	private BoxCollider boxCollider;

	// Token: 0x04000B9D RID: 2973
	[SerializeField]
	private JournalController journalController;

	// Token: 0x04000B9E RID: 2974
	[SerializeField]
	private AudioSource openSource;

	// Token: 0x04000B9F RID: 2975
	[SerializeField]
	private AudioClip[] openClips;
}

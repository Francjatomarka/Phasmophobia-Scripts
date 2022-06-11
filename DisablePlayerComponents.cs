using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200015D RID: 349
[RequireComponent(typeof(PhotonView))]
public class DisablePlayerComponents : MonoBehaviourPunCallbacks
{
	// Token: 0x060009D0 RID: 2512 RVA: 0x0003B4DB File Offset: 0x000396DB
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.player = base.GetComponent<Player>();
	}

	// Token: 0x060009D1 RID: 2513 RVA: 0x0003B4F5 File Offset: 0x000396F5
	private void Start()
	{
		this.SetupPlayer();
		if (GameController.instance != null)
		{
			GameController.instance.OnExitLevel.AddListener(new UnityAction(this.DestroyObjects));
		}
	}

	// Token: 0x060009D2 RID: 2514 RVA: 0x0003B524 File Offset: 0x00039724
	private void SetupPlayer()
	{
		if (GameController.instance == null)
		{
			this.shadowCamera.gameObject.SetActive(false);
		}
		if (PhotonNetwork.InRoom)
		{
			if (!this.view.IsMine)
			{
				for (int i = 0; i < this.scriptsToDisable.Count; i++)
				{
					UnityEngine.Object.Destroy(this.scriptsToDisable[i]);
				}
				for (int j = 0; j < this.objectsToDisable.Count; j++)
				{
					this.objectsToDisable[j].SetActive(false);
				}
				for (int k = 0; k < this.camerasToDisable.Count; k++)
				{
					this.camerasToDisable[k].enabled = false;
					this.camerasToDisable[k].tag = "Untagged";
				}
				UnityEngine.Object.Destroy(this.player.postProcessingLayer);
				UnityEngine.Object.Destroy(this.player.postProcessingVolume);
				GameObject[] array = this.characterModels;
				for (int l = 0; l < array.Length; l++)
				{
					foreach (object obj in array[l].transform)
					{
						((Transform)obj).gameObject.layer = 0;
					}
				}
				for (int m = 0; m < this.shoulderFlashlights.Length; m++)
				{
					this.shoulderFlashlights[m].layer = 0;
				}
				this.shoulderFlashlightLight.enabled = false;
				this.eye.SetParent(this.head.parent);
				this.head.SetParent(this.eye);
				this.VRRig.SetActive(true);
				this.shadowCamera.gameObject.SetActive(false);
			}
			else
			{
				this.view.RPC("LoadPlayerModel", RpcTarget.AllBuffered, new object[]
				{
					PlayerPrefs.GetInt("CharacterIndex")
				});
				if (MainManager.instance == null)
				{
					this.journalObject = PhotonNetwork.Instantiate(this.vrJournal.name, base.transform.position, Quaternion.identity, 0);
				}
			}
		}
		else
		{
			if (MainManager.instance == null)
			{
				UnityEngine.Object.Instantiate(Resources.Load(this.vrJournal.name), base.transform.position, Quaternion.identity);
			}
			for (int n = 0; n < this.transformViews.Length; n++)
			{
				this.transformViews[n].enabled = false;
			}
		}
		this.VRPlayer.SetActive(true);
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x0003B7EC File Offset: 0x000399EC
	private IEnumerator DisableBeltDelay()
	{
		yield return new WaitForSeconds(0.2f);
		this.vrBelt.SetActive(false);
		yield break;
	}

	// Token: 0x060009D4 RID: 2516 RVA: 0x0003B7FC File Offset: 0x000399FC
	[PunRPC]
	private void LoadPlayerModel(int index)
	{
		this.player.modelID = index;
		this.characterModels[this.player.modelID].SetActive(true);
		this.player.charAnim = this.characterModels[this.player.modelID].GetComponent<Animator>();
		this.player.movementSettings.anim = this.characterModels[this.player.modelID].GetComponent<Animator>();
	}

	// Token: 0x060009D5 RID: 2517 RVA: 0x0003B876 File Offset: 0x00039A76
	private void DestroyObjects()
	{
		if (this.journalObject && this.journalObject.GetComponent<PhotonView>().IsMine)
		{
			PhotonNetwork.Destroy(this.journalObject);
		}
	}

	// Token: 0x040009E6 RID: 2534
	public List<GameObject> objectsToDisable = new List<GameObject>();

	// Token: 0x040009E7 RID: 2535
	public List<UnityEngine.MonoBehaviour> scriptsToDisable = new List<UnityEngine.MonoBehaviour>();

	// Token: 0x040009E8 RID: 2536
	public List<Camera> camerasToDisable = new List<Camera>();

	// Token: 0x040009E9 RID: 2537
	public GameObject VRRig;

	// Token: 0x040009EA RID: 2538
	public GameObject VRPlayer;

	// Token: 0x040009EB RID: 2539
	public Transform eye;

	// Token: 0x040009EC RID: 2540
	public Transform head;

	// Token: 0x040009ED RID: 2541
	[SerializeField]
	private Camera shadowCamera;

	// Token: 0x040009EE RID: 2542
	private Player player;

	// Token: 0x040009EF RID: 2543
	private PhotonView view;

	// Token: 0x040009F0 RID: 2544
	[SerializeField]
	private GameObject[] shoulderFlashlights;

	// Token: 0x040009F1 RID: 2545
	[SerializeField]
	private Light shoulderFlashlightLight;

	// Token: 0x040009F2 RID: 2546
	[SerializeField]
	private GameObject[] characterModels;

	// Token: 0x040009F3 RID: 2547
	[SerializeField]
	private GameObject vrJournal;

	// Token: 0x040009F4 RID: 2548
	[SerializeField]
	private PhotonTransformView[] transformViews;

	// Token: 0x040009F6 RID: 2550
	[SerializeField]
	private GameObject playArea;

	// Token: 0x040009F7 RID: 2551
	[SerializeField]
	private GameObject vrBelt;

	// Token: 0x040009F8 RID: 2552
	private GameObject journalObject;
}

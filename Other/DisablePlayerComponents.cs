using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PhotonView))]
public class DisablePlayerComponents : MonoBehaviourPunCallbacks
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.player = base.GetComponent<Player>();
	}

	private void Start()
	{
		this.SetupPlayer();
		if (GameController.instance != null)
		{
			GameController.instance.OnExitLevel.AddListener(new UnityAction(this.DestroyObjects));
		}
	}

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

	private IEnumerator DisableBeltDelay()
	{
		yield return new WaitForSeconds(0.2f);
		this.vrBelt.SetActive(false);
		yield break;
	}

	[PunRPC]
	private void LoadPlayerModel(int index)
	{
		this.player.modelID = index;
		this.characterModels[this.player.modelID].SetActive(true);
		this.player.charAnim = this.characterModels[this.player.modelID].GetComponent<Animator>();
		this.player.movementSettings.anim = this.characterModels[this.player.modelID].GetComponent<Animator>();
	}

	private void DestroyObjects()
	{
		if (this.journalObject && this.journalObject.GetComponent<PhotonView>().IsMine)
		{
			PhotonNetwork.Destroy(this.journalObject);
		}
	}

	public List<GameObject> objectsToDisable = new List<GameObject>();

	public List<UnityEngine.MonoBehaviour> scriptsToDisable = new List<UnityEngine.MonoBehaviour>();

	public List<Camera> camerasToDisable = new List<Camera>();

	public GameObject VRRig;

	public GameObject VRPlayer;

	public Transform eye;

	public Transform head;

	[SerializeField]
	private Camera shadowCamera;

	private Player player;

	private PhotonView view;

	[SerializeField]
	private GameObject[] shoulderFlashlights;

	[SerializeField]
	private Light shoulderFlashlightLight;

	[SerializeField]
	private GameObject[] characterModels;

	[SerializeField]
	private GameObject vrJournal;

	[SerializeField]
	private PhotonTransformView[] transformViews;

	[SerializeField]
	private GameObject playArea;

	[SerializeField]
	private GameObject vrBelt;

	private GameObject journalObject;
}


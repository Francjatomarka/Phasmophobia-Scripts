using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;
using UnityStandardAssets.Characters.FirstPerson;

[RequireComponent(typeof(PhotonView))]
public class PCDisablePlayerComponents : MonoBehaviourPunCallbacks
{
	private void Start()
	{
		this.ApplySettings();
	}

	private void OnJoinedRoom()
	{
		this.ApplySettings();
	}

	private void ApplySettings()
	{
		if (this.hasApplied)
		{
			return;
		}
		this.hasApplied = true;
		if (PhotonNetwork.InRoom)
		{
			if (!this.view.IsMine)
			{
				base.GetComponent<PCMenu>().enabled = false;
				base.GetComponent<PlayerInput>().enabled = false;
				base.GetComponent<CharacterController>().enabled = false;
				base.GetComponent<Rigidbody>().isKinematic = true;
				base.GetComponent<AudioSource>().enabled = false;
				UnityEngine.Object.Destroy(base.GetComponent<FirstPersonController>());
				this.playerHead.GetComponent<Camera>().enabled = false;
				this.playerHead.GetComponent<AudioListener>().enabled = false;
				this.playerHead.GetComponent<PostProcessLayer>().enabled = false;
				this.playerHead.GetComponent<PostProcessVolume>().enabled = false;
				this.playerHead.GetComponent<DragRigidbodyUse>().enabled = false;
				this.player.pcPropGrab.enabled = false;
				base.GetComponent<PCPushToTalk>().enabled = false;
				this.player.pcCrouch.enabled = false;
				this.player.pcCanvas.enabled = false;
				this.shadowCamera.gameObject.SetActive(false);
				this.player.headObject.GetComponent<HxVolumetricCamera>().enabled = false;
				this.player.headObject.GetComponent<HxVolumetricImageEffect>().enabled = false;
				GameObject[] array = this.characterModels;
				for (int i = 0; i < array.Length; i++)
				{
					foreach (object obj in array[i].transform)
					{
						((Transform)obj).gameObject.layer = 0;
					}
				}
				for (int j = 0; j < this.shoulderFlashlights.Length; j++)
				{
					this.shoulderFlashlights[j].layer = 0;
				}
				this.shoulderFlashlightLight.enabled = false;
				this.pcCanvas.gameObject.SetActive(false);
			}
			else
			{
				this.view.RPC("LoadPlayerModel", RpcTarget.AllBuffered, new object[]
				{
					PlayerPrefs.GetInt("CharacterIndex")
				});
			}
		}
		else
		{
			this.LoadPlayerModel(PlayerPrefs.GetInt("CharacterIndex"));
		}
		if (!PhotonNetwork.InRoom)
		{
			for (int k = 0; k < this.transformViews.Length; k++)
			{
				this.transformViews[k].enabled = false;
			}
		}
		if (PlayerPrefs.GetInt("volumetricLightingValue") != 0)
		{
			this.player.headObject.GetComponent<HxVolumetricCamera>().enabled = true;
			this.player.headObject.GetComponent<HxVolumetricImageEffect>().enabled = true;
			if (PlayerPrefs.GetInt("volumetricLightingValue") == 1)
			{
				this.player.headObject.GetComponent<HxVolumetricCamera>().resolution = HxVolumetricCamera.Resolution.quarter;
				return;
			}
			if (PlayerPrefs.GetInt("volumetricLightingValue") == 2)
			{
				this.player.headObject.GetComponent<HxVolumetricCamera>().resolution = HxVolumetricCamera.Resolution.half;
				return;
			}
			if (PlayerPrefs.GetInt("volumetricLightingValue") == 3)
			{
				this.player.headObject.GetComponent<HxVolumetricCamera>().resolution = HxVolumetricCamera.Resolution.full;
				return;
			}
		}
		else
		{
			this.player.headObject.GetComponent<HxVolumetricCamera>().enabled = false;
			this.player.headObject.GetComponent<HxVolumetricImageEffect>().enabled = false;
		}
	}

	[PunRPC]
	private void LoadPlayerModel(int index)
	{
		this.player.modelID = index;
		this.characterModels[this.player.modelID].SetActive(true);
		this.player.charAnim = this.characterModels[this.player.modelID].GetComponent<Animator>();
		this.player.charAnim.SetFloat("speed", 0f);
		//this.player.pcPropGrab.grabSpotJoint = this.characterModelsHandJoints[this.player.modelID];
	}

	[SerializeField]
	private PhotonView view;

	[SerializeField]
	private GameObject[] characterModels;

	[SerializeField]
	private FixedJoint[] characterModelsHandJoints;

	[SerializeField]
	private GameObject playerHead;

	[SerializeField]
	private Camera shadowCamera;

	[SerializeField]
	private Player player;

	[SerializeField]
	private GameObject[] shoulderFlashlights;

	[SerializeField]
	private Light shoulderFlashlightLight;

	private bool hasApplied;

	[SerializeField]
	private PhotonTransformView[] transformViews;

	[SerializeField]
	private Canvas pcCanvas;
}


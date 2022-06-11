using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;
using UnityStandardAssets.Characters.FirstPerson;

// Token: 0x020001A3 RID: 419
[RequireComponent(typeof(PhotonView))]
public class PCDisablePlayerComponents : MonoBehaviourPunCallbacks
{
	// Token: 0x06000B65 RID: 2917 RVA: 0x000461EF File Offset: 0x000443EF
	private void Start()
	{
		this.ApplySettings();
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x000461EF File Offset: 0x000443EF
	private void OnJoinedRoom()
	{
		this.ApplySettings();
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x000461F8 File Offset: 0x000443F8
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

	// Token: 0x06000B68 RID: 2920 RVA: 0x00046530 File Offset: 0x00044730
	[PunRPC]
	private void LoadPlayerModel(int index)
	{
		this.player.modelID = index;
		this.characterModels[this.player.modelID].SetActive(true);
		this.player.charAnim = this.characterModels[this.player.modelID].GetComponent<Animator>();
		this.player.charAnim.SetFloat("speed", 0f);
		//this.player.pcPropGrab.grabSpotJoint = this.characterModelsHandJoints[this.player.modelID];
	}

	// Token: 0x04000BA8 RID: 2984
	[SerializeField]
	private PhotonView view;

	// Token: 0x04000BA9 RID: 2985
	[SerializeField]
	private GameObject[] characterModels;

	// Token: 0x04000BAA RID: 2986
	[SerializeField]
	private FixedJoint[] characterModelsHandJoints;

	// Token: 0x04000BAB RID: 2987
	[SerializeField]
	private GameObject playerHead;

	// Token: 0x04000BAC RID: 2988
	[SerializeField]
	private Camera shadowCamera;

	// Token: 0x04000BAD RID: 2989
	[SerializeField]
	private Player player;

	// Token: 0x04000BAE RID: 2990
	[SerializeField]
	private GameObject[] shoulderFlashlights;

	// Token: 0x04000BAF RID: 2991
	[SerializeField]
	private Light shoulderFlashlightLight;

	// Token: 0x04000BB0 RID: 2992
	private bool hasApplied;

	// Token: 0x04000BB1 RID: 2993
	[SerializeField]
	private PhotonTransformView[] transformViews;

	// Token: 0x04000BB2 RID: 2994
	[SerializeField]
	private Canvas pcCanvas;
}

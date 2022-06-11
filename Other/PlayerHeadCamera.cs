using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using Photon.Pun;

// Token: 0x02000170 RID: 368
[RequireComponent(typeof(PhotonView))]
public class PlayerHeadCamera : MonoBehaviour
{
	// Token: 0x06000A88 RID: 2696 RVA: 0x0004148D File Offset: 0x0003F68D
	public void GrabCamera(CCTV headCam)
	{
		if (PhotonNetwork.InRoom)
		{
			this.view.RPC("EquippedCamera", RpcTarget.All, new object[]
			{
				headCam.GetComponent<PhotonView>().ViewID
			});
			return;
		}
		EquippedCamera(headCam.GetComponent<PhotonView>().ViewID);
	}

	// Token: 0x06000A89 RID: 2697 RVA: 0x000414BC File Offset: 0x0003F6BC
	private void SecondaryUse()
	{
		
	}

	// Token: 0x06000A8A RID: 2698 RVA: 0x00041587 File Offset: 0x0003F787
	public void DisableCamera()
	{
		if (this.isEquipped)
		{
			this.headCamera.TurnOff();
		}
	}

	// Token: 0x06000A8B RID: 2699 RVA: 0x000415AD File Offset: 0x0003F7AD
	public void VRGrabOrPlaceCamera(int viewID, bool isPlaced)
	{
		this.view.RPC("VRGrabOrPlaceCameraNetworked", RpcTarget.All, new object[]
		{
			viewID,
			isPlaced
		});
	}

	// Token: 0x06000A8C RID: 2700 RVA: 0x000415D8 File Offset: 0x0003F7D8
	[PunRPC]
	private void VRGrabOrPlaceCameraNetworked(int viewID, bool isPlaced)
	{
		if (PhotonView.Find(viewID) == null)
		{
			return;
		}
		CCTV component = PhotonView.Find(viewID).GetComponent<CCTV>();
		if (isPlaced)
		{
			if (this.view.IsMine)
			{
				component.TurnOn();
			}
		}
		else if (this.view.IsMine)
		{
			component.TurnOff();
		}
		for (int i = 0; i < component.rends.Length; i++)
		{
			component.rends[i].enabled = !isPlaced;
		}
		if (!this.view.IsMine)
		{
			for (int j = 0; j < this.headCameraModels.Length; j++)
			{
				this.headCameraModels[j].SetActive(isPlaced);
			}
		}
		this.isEquipped = isPlaced;
		this.headCamera = (isPlaced ? component : null);
	}

	// Token: 0x06000A8D RID: 2701 RVA: 0x00041694 File Offset: 0x0003F894
	[PunRPC]
	private void PlaceCamera(int id)
	{
		this.headCamera.transform.SetParent(PhotonView.Find(id).transform);
		this.headCamera.transform.localPosition = Vector3.zero;
		this.headCamera.transform.localRotation = Quaternion.identity;
		this.headCamera.cam.transform.SetParent(this.headCamera.headCamParent);
		this.headCamera.cam.transform.localPosition = Vector3.zero;
		this.headCamera.cam.transform.localRotation = Quaternion.identity;
		this.headCamera.GetComponent<Collider>().enabled = true;
		this.headCamera.TurnOff();
		this.headCamera.cam.enabled = false;
		this.headCamera.myLight.enabled = false;
		for (int i = 0; i < this.headCamera.rends.Length; i++)
		{
			this.headCamera.rends[i].enabled = true;
		}
		for (int j = 0; j < this.headCameraModels.Length; j++)
		{
			this.headCameraModels[j].SetActive(false);
		}
		this.headCamera = null;
		this.isEquipped = false;
	}

	// Token: 0x06000A8E RID: 2702 RVA: 0x000417D4 File Offset: 0x0003F9D4
	[PunRPC]
	private void EquippedCamera(int viewID)
	{
		CCTV component = PhotonView.Find(viewID).GetComponent<CCTV>();
		this.isEquipped = true;
		this.headCamera = component;
		this.headCamera.transform.localPosition = Vector3.zero;
		this.headCamera.transform.localRotation = Quaternion.identity;
		if(GameController.instance != null)
        {
			for (int i = 0; i < GameController.instance.playersData.Count; i++)
			{
				if (GameController.instance.playersData[i].actorID == int.Parse(this.view.Owner.UserId))
				{
					this.headCamera.cam.transform.SetParent(this.characterHeadCamSpots[GameController.instance.playersData[i].player.modelID]);
					this.headCamera.cam.transform.localPosition = Vector3.zero;
					this.headCamera.cam.transform.localRotation = Quaternion.identity;
					if (this.view.IsMine)
					{
						this.headCamera.cam.transform.SetParent(this.headCameraModels[GameController.instance.playersData[i].player.modelID].transform.parent);
					}
				}
			}
		}
		this.headCamera.GetComponent<Collider>().enabled = false;
		if (this.view.IsMine)
		{
			this.headCamera.TurnOn();
		}
		for (int j = 0; j < this.headCamera.rends.Length; j++)
		{
			this.headCamera.rends[j].enabled = false;
		}
		if (!this.view.IsMine)
		{
			for (int k = 0; k < this.headCameraModels.Length; k++)
			{
				this.headCameraModels[k].SetActive(true);
			}
		}
	}

	// Token: 0x06000A8F RID: 2703 RVA: 0x000419BE File Offset: 0x0003FBBE
	public void OnSecondaryUse(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started && this.isEquipped)
		{
			this.SecondaryUse();
		}
	}

	// Token: 0x04000AE8 RID: 2792
	[HideInInspector]
	public bool isEquipped;

	// Token: 0x04000AE9 RID: 2793
	[HideInInspector]
	public CCTV headCamera;

	// Token: 0x04000AEB RID: 2795
	[SerializeField]
	private GameObject[] headCameraModels;

	// Token: 0x04000AEC RID: 2796
	[SerializeField]
	private Transform[] characterHeadCamSpots;

	// Token: 0x04000AED RID: 2797
	private readonly float grabDistance = 1.6f;

	// Token: 0x04000AEE RID: 2798
	private Ray playerAim;

	// Token: 0x04000AEF RID: 2799
	[SerializeField]
	private PhotonView view;

	// Token: 0x04000AF0 RID: 2800
	[SerializeField]
	private LayerMask mask;
}

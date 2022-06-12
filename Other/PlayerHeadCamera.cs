using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class PlayerHeadCamera : MonoBehaviour
{
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

	private void SecondaryUse()
	{
		
	}

	public void DisableCamera()
	{
		if (this.isEquipped)
		{
			this.headCamera.TurnOff();
		}
	}

	public void VRGrabOrPlaceCamera(int viewID, bool isPlaced)
	{
		this.view.RPC("VRGrabOrPlaceCameraNetworked", RpcTarget.All, new object[]
		{
			viewID,
			isPlaced
		});
	}

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

	public void OnSecondaryUse(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started && this.isEquipped)
		{
			this.SecondaryUse();
		}
	}

	[HideInInspector]
	public bool isEquipped;

	[HideInInspector]
	public CCTV headCamera;

	[SerializeField]
	private GameObject[] headCameraModels;

	[SerializeField]
	private Transform[] characterHeadCamSpots;

	private readonly float grabDistance = 1.6f;

	private Ray playerAim;

	[SerializeField]
	private PhotonView view;

	[SerializeField]
	private LayerMask mask;
}


using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class CCTVController : MonoBehaviour
{
	private void Awake()
	{
		CCTVController.instance = this;
		this.cctvCameras.Clear();
		this.view = base.GetComponent<PhotonView>();
		this.texture = this.screen.material.GetTexture("_EmissionMap");
		this.truckKeyboardPhotonInteract.AddUseEvent(new UnityAction(this.ChangeNightVision));
		this.truckMousePhotonInteract.AddUseEvent(new UnityAction(this.NextCamera));
	}

	private void Start()
	{
		GameController.instance.OnAllPlayersConnected.AddListener(new UnityAction(this.OnAllPlayersConnected));
		for (int i = 0; i < this.allFixedCCTVCameras.Count; i++)
		{
			this.allcctvCameras.Add(this.allFixedCCTVCameras[i]);
		}
	}

	private void OnAllPlayersConnected()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			base.Invoke("ActivateCamerasAfterTimeLimit", 5f);
		}
	}

	public void AddCamera(CCTV c)
	{
		if (!this.cctvCameras.Contains(c))
		{
			this.cctvCameras.Add(c);
		}
		if (this.screen)
		{
			this.screen.material.color = Color.white;
		}
		this.RemoveDeactivatedCameras();
		this.NextCamera();
	}

	public void RemoveCamera(CCTV c)
	{
		if (this.cctvCameras.Contains(c))
		{
			this.cctvCameras.Remove(c);
		}
		if (this.cctvCameras.Count == 0)
		{
			if (this.screen)
			{
				this.screen.material.color = Color.black;
				this.screen.material.SetTexture("_EmissionMap", this.texture);
			}
			if (this.screenText)
			{
				this.screenText.text = "00/00";
			}
		}
		this.RemoveDeactivatedCameras();
		this.NextCamera();
	}

	private void RemoveDeactivatedCameras()
	{
		bool flag = false;
		for (int i = 0; i < this.cctvCameras.Count; i++)
		{
			if (this.cctvCameras[i] == null)
			{
				this.cctvCameras.RemoveAt(i);
				flag = true;
			}
			if (this.cctvCameras.Count == 0)
			{
				this.screen.material.color = Color.black;
				this.screen.material.SetTexture("_EmissionMap", this.texture);
				this.screenText.text = "00/00";
				return;
			}
			if (flag)
			{
				return;
			}
		}
	}

	public void StartRendering()
	{
		this.isRendering = true;
		this.RemoveDeactivatedCameras();
		if (this.cctvCameras.Count > 0 && this.cctvCameras[this.index] != null)
		{
			this.cctvCameras[this.index].cam.enabled = true;
			this.cctvCameras[this.index].myLight.enabled = true;
		}
	}

	public void StopRendering()
	{
		this.isRendering = false;
		this.RemoveDeactivatedCameras();
		for (int i = 0; i < this.allcctvCameras.Count; i++)
		{
			if (this.allcctvCameras[i] != null)
			{
				this.allcctvCameras[i].cam.enabled = false;
				this.allcctvCameras[i].myLight.enabled = false;
			}
		}
	}

	private void ActivateCamerasAfterTimeLimit()
	{
		for (int i = 0; i < this.allFixedCCTVCameras.Count; i++)
		{
			this.allFixedCCTVCameras[i].Use();
		}
		this.view.RPC("ActivatedCameras", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	[PunRPC]
	private void ActivatedCameras()
	{
		this.activatedCCTVCameras = true;
	}

	private void ChangeNightVision()
	{
		if (GameController.instance.allPlayersAreConnected && !GameController.instance.myPlayer.player.isDead)
		{
			this.view.RPC("NetworkedChangeNightVision", RpcTarget.All, new object[]
			{
				!CCTVController.showNightVisionEffect
			});
		}
	}

	[PunRPC]
	private void NetworkedChangeNightVision(bool on)
	{
		CCTVController.showNightVisionEffect = on;
		for (int i = 0; i < this.allcctvCameras.Count; i++)
		{
			if (this.allcctvCameras[i] != null && this.allcctvCameras[i].cam != null && this.allcctvCameras[i].cam.GetComponent<Nightvision>())
			{
				this.allcctvCameras[i].cam.GetComponent<Nightvision>().enabled = on;
			}
		}
	}

	public void NextCamera()
	{
		if (GameController.instance && GameController.instance.myPlayer != null && !GameController.instance.myPlayer.player.isDead)
		{
			this.view.RPC("ChangeCameraNetworked", RpcTarget.MasterClient, Array.Empty<object>());
		}
	}

	[PunRPC]
	private void ChangeCameraNetworked()
	{
		if (this.cctvCameras.Count == 0)
		{
			return;
		}
		this.index++;
		if (this.index >= this.cctvCameras.Count)
		{
			this.index = 0;
		}
		this.view.RPC("SyncCameraNetworked", RpcTarget.All, new object[]
		{
			this.index
		});
	}

	[PunRPC]
	private void SyncCameraNetworked(int newIndex)
	{
		this.RemoveDeactivatedCameras();
		this.index = newIndex;
		if (this.index >= this.cctvCameras.Count)
		{
			this.index = 0;
		}
		for (int i = 0; i < this.cctvCameras.Count; i++)
		{
			if (this.cctvCameras[i] != null)
			{
				this.cctvCameras[i].cam.enabled = false;
				this.cctvCameras[i].myLight.enabled = false;
				this.cctvCameras[i].isThisCameraActiveOnACCTVScreen = false;
				if (this.cctvCameras[i].mapIcon != null)
				{
					this.cctvCameras[i].mapIcon.color = Color.yellow;
				}
			}
		}
		if (this.cctvCameras[this.index] != null)
		{
			if (this.isRendering)
			{
				this.cctvCameras[this.index].cam.enabled = true;
				this.cctvCameras[this.index].myLight.enabled = true;
			}
			this.cctvCameras[this.index].isThisCameraActiveOnACCTVScreen = true;
			if (this.cctvCameras[this.index].isFixedCamera)
			{
				this.cctvCameras[this.index].mapIcon.color = Color.green;
			}
			this.screen.material.mainTexture = this.cctvCameras[this.index].cam.targetTexture;
			this.screen.material.SetTexture("_EmissionMap", this.cctvCameras[this.index].cam.targetTexture);
		}
		this.screenText.text = (this.index + 1).ToString("00") + "/" + this.cctvCameras.Count.ToString("00");
	}

	private void PreviousCamera()
	{
		if (this.cctvCameras.Count == 0)
		{
			return;
		}
		this.index--;
		if (this.index < 0)
		{
			this.index = this.cctvCameras.Count - 1;
		}
		for (int i = 0; i < this.cctvCameras.Count; i++)
		{
			if (this.cctvCameras[i] != null)
			{
				this.cctvCameras[i].isThisCameraActiveOnACCTVScreen = false;
			}
		}
		if (this.cctvCameras[this.index] != null)
		{
			this.cctvCameras[this.index].cam.Render();
			this.cctvCameras[this.index].isThisCameraActiveOnACCTVScreen = true;
		}
	}

	public static CCTVController instance;

	public PhotonView view;

	public Renderer screen;

	public List<CCTV> cctvCameras = new List<CCTV>();

	public int index;

	private float renderTimer = 0.25f;

	private Texture texture;

	[SerializeField]
	private PhotonObjectInteract truckKeyboardPhotonInteract;

	[SerializeField]
	private PhotonObjectInteract truckMousePhotonInteract;

	[SerializeField]
	private Text screenText;

	public List<CCTV> allFixedCCTVCameras = new List<CCTV>();

	public bool activatedCCTVCameras;

	[SerializeField]
	private static bool showNightVisionEffect = true;

	public List<CCTV> allcctvCameras = new List<CCTV>();

	[SerializeField]
	private bool isRendering = true;
}


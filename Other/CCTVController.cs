using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;

// Token: 0x020000CC RID: 204
[RequireComponent(typeof(PhotonView))]
public class CCTVController : MonoBehaviour
{
	// Token: 0x060005AE RID: 1454 RVA: 0x00020EC4 File Offset: 0x0001F0C4
	private void Awake()
	{
		CCTVController.instance = this;
		this.cctvCameras.Clear();
		this.view = base.GetComponent<PhotonView>();
		this.texture = this.screen.material.GetTexture("_EmissionMap");
		this.truckKeyboardPhotonInteract.AddUseEvent(new UnityAction(this.ChangeNightVision));
		this.truckMousePhotonInteract.AddUseEvent(new UnityAction(this.NextCamera));
	}

	// Token: 0x060005AF RID: 1455 RVA: 0x00020F38 File Offset: 0x0001F138
	private void Start()
	{
		GameController.instance.OnAllPlayersConnected.AddListener(new UnityAction(this.OnAllPlayersConnected));
		for (int i = 0; i < this.allFixedCCTVCameras.Count; i++)
		{
			this.allcctvCameras.Add(this.allFixedCCTVCameras[i]);
		}
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x00020F8D File Offset: 0x0001F18D
	private void OnAllPlayersConnected()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			base.Invoke("ActivateCamerasAfterTimeLimit", 5f);
		}
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x00020FA8 File Offset: 0x0001F1A8
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

	// Token: 0x060005B2 RID: 1458 RVA: 0x00021000 File Offset: 0x0001F200
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

	// Token: 0x060005B3 RID: 1459 RVA: 0x0002109C File Offset: 0x0001F29C
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

	// Token: 0x060005B4 RID: 1460 RVA: 0x00021138 File Offset: 0x0001F338
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

	// Token: 0x060005B5 RID: 1461 RVA: 0x000211B4 File Offset: 0x0001F3B4
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

	// Token: 0x060005B6 RID: 1462 RVA: 0x00021228 File Offset: 0x0001F428
	private void ActivateCamerasAfterTimeLimit()
	{
		for (int i = 0; i < this.allFixedCCTVCameras.Count; i++)
		{
			this.allFixedCCTVCameras[i].Use();
		}
		this.view.RPC("ActivatedCameras", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	// Token: 0x060005B7 RID: 1463 RVA: 0x00021272 File Offset: 0x0001F472
	[PunRPC]
	private void ActivatedCameras()
	{
		this.activatedCCTVCameras = true;
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x0002127C File Offset: 0x0001F47C
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

	// Token: 0x060005B9 RID: 1465 RVA: 0x000212D4 File Offset: 0x0001F4D4
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

	// Token: 0x060005BA RID: 1466 RVA: 0x00021364 File Offset: 0x0001F564
	public void NextCamera()
	{
		if (GameController.instance && GameController.instance.myPlayer != null && !GameController.instance.myPlayer.player.isDead)
		{
			this.view.RPC("ChangeCameraNetworked", RpcTarget.MasterClient, Array.Empty<object>());
		}
	}

	// Token: 0x060005BB RID: 1467 RVA: 0x000213B8 File Offset: 0x0001F5B8
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

	// Token: 0x060005BC RID: 1468 RVA: 0x00021420 File Offset: 0x0001F620
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

	// Token: 0x060005BD RID: 1469 RVA: 0x00021640 File Offset: 0x0001F840
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

	// Token: 0x04000550 RID: 1360
	public static CCTVController instance;

	// Token: 0x04000551 RID: 1361
	public PhotonView view;

	// Token: 0x04000552 RID: 1362
	public Renderer screen;

	// Token: 0x04000553 RID: 1363
	public List<CCTV> cctvCameras = new List<CCTV>();

	// Token: 0x04000554 RID: 1364
	public int index;

	// Token: 0x04000555 RID: 1365
	private float renderTimer = 0.25f;

	// Token: 0x04000556 RID: 1366
	private Texture texture;

	// Token: 0x04000557 RID: 1367
	[SerializeField]
	private PhotonObjectInteract truckKeyboardPhotonInteract;

	// Token: 0x04000558 RID: 1368
	[SerializeField]
	private PhotonObjectInteract truckMousePhotonInteract;

	// Token: 0x04000559 RID: 1369
	[SerializeField]
	private Text screenText;

	// Token: 0x0400055A RID: 1370
	public List<CCTV> allFixedCCTVCameras = new List<CCTV>();

	// Token: 0x0400055B RID: 1371
	public bool activatedCCTVCameras;

	// Token: 0x0400055C RID: 1372
	[SerializeField]
	private static bool showNightVisionEffect = true;

	public List<CCTV> allcctvCameras = new List<CCTV>();

	// Token: 0x0400055E RID: 1374
	[SerializeField]
	private bool isRendering = true;
}

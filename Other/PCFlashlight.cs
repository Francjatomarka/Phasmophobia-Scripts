using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using Photon.Pun;

// Token: 0x020001AA RID: 426
public class PCFlashlight : MonoBehaviour
{
	// Token: 0x06000BA3 RID: 2979 RVA: 0x00048258 File Offset: 0x00046458
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.isOn = false;
		this.blinkTimer = UnityEngine.Random.Range(0f, 0.5f);
	}

	// Token: 0x06000BA4 RID: 2980 RVA: 0x00048284 File Offset: 0x00046484
	public void GrabbedOrDroppedFlashlight(Torch torch, bool grabbed)
	{
		this.inventoryLight = (grabbed ? torch.myLight : null);
		this.headLight.cookie = torch.myLight.cookie;
		this.lightIntensity = torch.myLight.intensity;
		this.isOn = false;
		if (PhotonNetwork.InRoom)
		{
			this.view.RPC("GrabbedOrDroppedFlashlightNetworked", RpcTarget.AllBuffered, new object[]
			{
				torch.myLight.intensity,
				torch.myLight.range,
				torch.myLight.spotAngle
			});
			return;
		}
		this.GrabbedOrDroppedFlashlightNetworked(torch.myLight.intensity, torch.myLight.range, torch.myLight.spotAngle);
	}

	// Token: 0x06000BA5 RID: 2981 RVA: 0x00048350 File Offset: 0x00046550
	[PunRPC]
	private void GrabbedOrDroppedFlashlightNetworked(float _intensity, float _range, float _angle)
	{
		this.headLight.intensity = _intensity;
		this.headLight.range = _range;
		this.headLight.spotAngle = _angle;
	}

	// Token: 0x06000BA6 RID: 2982 RVA: 0x00048378 File Offset: 0x00046578
	public void EnableOrDisableLight(bool _active, bool _isSwapping)
	{
		if (_active && this.inventoryLight == null)
		{
			return;
		}
		if (!_isSwapping && this.pcPropGrab.inventoryProps[this.pcPropGrab.inventoryIndex] != null && this.pcPropGrab.inventoryProps[this.pcPropGrab.inventoryIndex].GetComponent<Torch>() && this.pcPropGrab.inventoryProps[this.pcPropGrab.inventoryIndex].GetComponent<Torch>().isBlacklight)
		{
			return;
		}
		if (PhotonNetwork.InRoom)
		{
			this.view.RPC("EnableOrDisableLightNetworked", RpcTarget.AllBuffered, new object[]
			{
				_active,
				_isSwapping
			});
			return;
		}
		this.EnableOrDisableLightNetworked(_active, _isSwapping);
	}

	// Token: 0x06000BA7 RID: 2983 RVA: 0x00048448 File Offset: 0x00046648
	[PunRPC]
	private void EnableOrDisableLightNetworked(bool _active, bool _isSwapping)
	{
		this.headLight.enabled = _active;
		this.isOn = _active;
		if (PhotonNetwork.InRoom)
		{
			this.source.outputAudioMixerGroup = SoundController.instance.GetPlayersAudioGroup(int.Parse(this.view.Owner.UserId));
		}
		if (!_isSwapping)
		{
			if (this.pcPropGrab.inventoryProps[this.pcPropGrab.inventoryIndex] == null)
			{
				this.source.Play();
				return;
			}
			if (this.pcPropGrab.inventoryProps[this.pcPropGrab.inventoryIndex].GetComponent<Torch>())
			{
				if (this.pcPropGrab.inventoryProps[this.pcPropGrab.inventoryIndex].GetComponent<Torch>().isBlacklight)
				{
					this.source.Play();
					return;
				}
			}
			else if (this.inventoryLight)
			{
				this.source.Play();
			}
		}
	}

	// Token: 0x06000BA8 RID: 2984 RVA: 0x0004853B File Offset: 0x0004673B
	public void TurnBlinkOnOrOff(bool active)
	{
		this.isBlinking = active;
		if (!this.isBlinking && this.inventoryLight != null)
		{
			this.headLight.intensity = this.lightIntensity;
		}
	}

	// Token: 0x06000BA9 RID: 2985 RVA: 0x0004856C File Offset: 0x0004676C
	private void Update()
	{
		if (this.isBlinking)
		{
			this.blinkTimer -= Time.deltaTime;
			if (this.blinkTimer < 0f)
			{
				this.Blink();
				this.blinkTimer = UnityEngine.Random.Range(0.1f, 0.5f);
			}
		}
	}

	// Token: 0x06000BAA RID: 2986 RVA: 0x000485BC File Offset: 0x000467BC
	private void Blink()
	{
		if (this.inventoryLight != null)
		{
			if (this.headLight.intensity == 0f)
			{
				this.headLight.intensity = this.lightIntensity;
				return;
			}
			this.headLight.intensity = 0f;
		}
	}

	// Token: 0x06000BAB RID: 2987 RVA: 0x0004860C File Offset: 0x0004680C
	public void OnTorchUse(InputAction.CallbackContext context)
	{
		if (XRDevice.isPresent)
		{
			return;
		}
		if (this.player.isDead)
		{
			return;
		}
		if (PhotonNetwork.InRoom && !this.view.IsMine)
		{
			return;
		}
		if (this.inventoryLight == null)
		{
			return;
		}
		if (context.phase == InputActionPhase.Started)
		{
			this.isOn = !this.isOn;
			if (this.pcPropGrab.inventoryProps[this.pcPropGrab.inventoryIndex] != null && this.pcPropGrab.inventoryProps[this.pcPropGrab.inventoryIndex].GetComponent<Torch>() && !this.pcPropGrab.inventoryProps[this.pcPropGrab.inventoryIndex].GetComponent<Torch>().isBlacklight)
			{
				this.isOn = false;
			}
			if (PhotonNetwork.InRoom)
			{
				this.view.RPC("EnableOrDisableLightNetworked", RpcTarget.AllBuffered, new object[]
				{
					this.isOn,
					false
				});
				return;
			}
			this.EnableOrDisableLightNetworked(this.isOn, false);
		}
	}

	// Token: 0x04000BFB RID: 3067
	[SerializeField]
	private Light headLight;

	// Token: 0x04000BFC RID: 3068
	[SerializeField]
	private PCPropGrab pcPropGrab;

	// Token: 0x04000BFD RID: 3069
	[SerializeField]
	private Player player;

	// Token: 0x04000BFE RID: 3070
	[SerializeField]
	private AudioSource source;

	// Token: 0x04000BFF RID: 3071
	private PhotonView view;

	// Token: 0x04000C00 RID: 3072
	private bool isOn;

	// Token: 0x04000C01 RID: 3073
	private Light inventoryLight;

	// Token: 0x04000C02 RID: 3074
	private bool isBlinking;

	// Token: 0x04000C03 RID: 3075
	private float lightIntensity;

	// Token: 0x04000C04 RID: 3076
	private float blinkTimer;
}

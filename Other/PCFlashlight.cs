using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using Photon.Pun;

public class PCFlashlight : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.isOn = false;
		this.blinkTimer = UnityEngine.Random.Range(0f, 0.5f);
	}

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

	[PunRPC]
	private void GrabbedOrDroppedFlashlightNetworked(float _intensity, float _range, float _angle)
	{
		this.headLight.intensity = _intensity;
		this.headLight.range = _range;
		this.headLight.spotAngle = _angle;
	}

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

	public void TurnBlinkOnOrOff(bool active)
	{
		this.isBlinking = active;
		if (!this.isBlinking && this.inventoryLight != null)
		{
			this.headLight.intensity = this.lightIntensity;
		}
	}

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

	[SerializeField]
	private Light headLight;

	[SerializeField]
	private PCPropGrab pcPropGrab;

	[SerializeField]
	private Player player;

	[SerializeField]
	private AudioSource source;

	private PhotonView view;

	private bool isOn;

	private Light inventoryLight;

	private bool isBlinking;

	private float lightIntensity;

	private float blinkTimer;
}


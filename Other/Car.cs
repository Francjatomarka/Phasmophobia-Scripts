using System;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class Car : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.rend = base.GetComponent<Renderer>();
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
	}

	private void Start()
	{
		this.source.clip = this.alarmClip;
		this.source.loop = true;
		this.alarmOn = false;
		this.rend.material.DisableKeyword("_EMISSION");
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
	}

	private void Update()
	{
		if (this.alarmOn)
		{
			this.timer -= Time.deltaTime;
			if (this.timer < 0f)
			{
				this.SwitchLights();
				this.timer = 0.2f;
			}
		}
	}

	private void Use()
	{
		for (int i = 0; i < GameController.instance.myPlayer.player.keys.Count; i++)
		{
			if (GameController.instance.myPlayer.player.keys[i] == Key.KeyType.Car)
			{
				this.view.RPC("TurnAlarmOff", RpcTarget.All, Array.Empty<object>());
			}
		}
	}

	private void SwitchLights()
	{
		this.isOn = !this.isOn;
		for (int i = 0; i < this.lights.Length; i++)
		{
			this.lights[i].enabled = this.isOn;
		}
		if (this.isOn)
		{
			this.rend.material.EnableKeyword("_EMISSION");
			return;
		}
		this.rend.material.DisableKeyword("_EMISSION");
	}

	[PunRPC]
	private void TurnAlarmOn()
	{
		this.alarmOn = true;
		this.rend.material.EnableKeyword("_EMISSION");
		this.source.loop = true;
		this.source.clip = this.alarmClip;
		this.source.Play();
		if (this.noise)
		{
			this.noise.gameObject.SetActive(true);
		}
	}

	[PunRPC]
	private void TurnAlarmOff()
	{
		this.alarmOn = false;
		this.rend.material.DisableKeyword("_EMISSION");
		this.timer = 0.2f;
		this.source.loop = false;
		this.source.Stop();
		this.source.clip = this.offClip;
		this.source.Play();
		if (this.noise)
		{
			this.noise.gameObject.SetActive(false);
		}
		this.isOn = false;
		for (int i = 0; i < this.lights.Length; i++)
		{
			this.lights[i].enabled = false;
		}
		this.mainRoomLight.ResetReflectionProbes();
	}

	[HideInInspector]
	public PhotonView view;

	private bool alarmOn;

	private bool isOn;

	[SerializeField]
	private Light[] lights;

	private Renderer rend;

	[SerializeField]
	private AudioSource source;

	[SerializeField]
	private AudioClip alarmClip;

	[SerializeField]
	private AudioClip offClip;

	private float timer = 0.2f;

	public Transform raycastSpot;

	[SerializeField]
	private LightSwitch mainRoomLight;

	private PhotonObjectInteract photonInteract;

	[SerializeField]
	private Noise noise;
}


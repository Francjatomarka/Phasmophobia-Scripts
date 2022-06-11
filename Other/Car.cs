using System;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

// Token: 0x0200013E RID: 318
[RequireComponent(typeof(PhotonView))]
public class Car : MonoBehaviour
{
	// Token: 0x0600084B RID: 2123 RVA: 0x00031740 File Offset: 0x0002F940
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.rend = base.GetComponent<Renderer>();
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
	}

	// Token: 0x0600084C RID: 2124 RVA: 0x00031768 File Offset: 0x0002F968
	private void Start()
	{
		this.source.clip = this.alarmClip;
		this.source.loop = true;
		this.alarmOn = false;
		this.rend.material.DisableKeyword("_EMISSION");
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
	}

	// Token: 0x0600084D RID: 2125 RVA: 0x000317C5 File Offset: 0x0002F9C5
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

	// Token: 0x0600084E RID: 2126 RVA: 0x00031800 File Offset: 0x0002FA00
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

	// Token: 0x0600084F RID: 2127 RVA: 0x00031864 File Offset: 0x0002FA64
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

	// Token: 0x06000850 RID: 2128 RVA: 0x000318DC File Offset: 0x0002FADC
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

	// Token: 0x06000851 RID: 2129 RVA: 0x0003194C File Offset: 0x0002FB4C
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

	// Token: 0x04000869 RID: 2153
	[HideInInspector]
	public PhotonView view;

	// Token: 0x0400086A RID: 2154
	private bool alarmOn;

	// Token: 0x0400086B RID: 2155
	private bool isOn;

	// Token: 0x0400086C RID: 2156
	[SerializeField]
	private Light[] lights;

	// Token: 0x0400086D RID: 2157
	private Renderer rend;

	// Token: 0x0400086E RID: 2158
	[SerializeField]
	private AudioSource source;

	// Token: 0x0400086F RID: 2159
	[SerializeField]
	private AudioClip alarmClip;

	// Token: 0x04000870 RID: 2160
	[SerializeField]
	private AudioClip offClip;

	// Token: 0x04000871 RID: 2161
	private float timer = 0.2f;

	// Token: 0x04000872 RID: 2162
	public Transform raycastSpot;

	// Token: 0x04000873 RID: 2163
	[SerializeField]
	private LightSwitch mainRoomLight;

	// Token: 0x04000874 RID: 2164
	private PhotonObjectInteract photonInteract;

	// Token: 0x04000875 RID: 2165
	[SerializeField]
	private Noise noise;
}

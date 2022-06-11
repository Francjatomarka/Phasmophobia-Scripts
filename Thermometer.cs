using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;

// Token: 0x02000123 RID: 291
public class Thermometer : MonoBehaviour
{
	// Token: 0x0600082B RID: 2091 RVA: 0x00031BA4 File Offset: 0x0002FDA4
	private void Awake()
	{
		this.noise = base.GetComponentInChildren<Noise>();
		this.source = base.GetComponent<AudioSource>();
		this.view = base.GetComponent<PhotonView>();
		this.rend = base.GetComponent<MeshRenderer>();
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.noise.gameObject.SetActive(false);
		this.isOn = false;
		this.timer = 0.8f;
	}

	// Token: 0x0600082C RID: 2092 RVA: 0x00031C10 File Offset: 0x0002FE10
	private void Start()
	{
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
		this.temperatureText.text = "";
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x00031C39 File Offset: 0x0002FE39
	private void Update()
	{
		if (this.isOn)
		{
			this.timer -= Time.deltaTime;
			if (this.timer < 0f)
			{
				this.UpdateSpotPosition();
				this.timer = 0.8f;
			}
		}
	}

	// Token: 0x0600082E RID: 2094 RVA: 0x00031C73 File Offset: 0x0002FE73
	private void Use()
	{
        if (PhotonNetwork.InRoom)
        {
			this.view.RPC("NetworkedUse", RpcTarget.All, Array.Empty<object>());
			return;
		}
		NetworkedUse();
	}

	// Token: 0x0600082F RID: 2095 RVA: 0x00031C8C File Offset: 0x0002FE8C
	[PunRPC]
	private void NetworkedUse()
	{
		this.isOn = !this.isOn;
		base.StartCoroutine(this.PlayNoiseObject());
		this.source.Play();
		if (this.isOn)
		{
			this.rend.material.EnableKeyword("_EMISSION");
			this.canvasObj.SetActive(true);
			return;
		}
		this.rend.material.DisableKeyword("_EMISSION");
		this.canvasObj.SetActive(false);
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x00031D0C File Offset: 0x0002FF0C
	private void UpdateSpotPosition()
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position, base.transform.TransformDirection(Vector3.back), out raycastHit, 6f, this.mask))
		{
			this.spot.position = raycastHit.point;
			if(this.room != null)
            {
				this.currentTemp = ((PlayerPrefs.GetInt("degreesValue") == 0) ? this.room.temperature : (this.room.temperature * 1.8f + 32f));
				this.temperatureText.text = (this.currentTemp + UnityEngine.Random.Range(-2f, 2f)).ToString("0.0") + ((PlayerPrefs.GetInt("degreesValue") == 0) ? "C" : "F");
			}
		}
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x00031DE4 File Offset: 0x0002FFE4
	public void SetTemperatureValue(LevelRoom room)
	{
		if(LevelController.instance != null)
        {
			if (room == LevelController.instance.outsideRoom && GameController.instance.myPlayer.player.currentRoom != room)
			{
				return;
			}
		}
		this.room = room;
		if (room.temperature < 10f && MissionTemperature.instance != null && room != LevelController.instance.outsideRoom && !MissionTemperature.instance.completed)
		{
			MissionTemperature.instance.CompleteMission();
		}
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x00031E69 File Offset: 0x00030069
	private IEnumerator PlayNoiseObject()
	{
		this.noise.gameObject.SetActive(true);
		yield return new WaitForEndOfFrame();
		this.noise.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x0400082D RID: 2093
	private AudioSource source;

	// Token: 0x0400082E RID: 2094
	private PhotonView view;

	// Token: 0x0400082F RID: 2095
	private PhotonObjectInteract photonInteract;

	// Token: 0x04000830 RID: 2096
	public LayerMask mask;

	// Token: 0x04000831 RID: 2097
	private Noise noise;

	// Token: 0x04000832 RID: 2098
	private MeshRenderer rend;

	// Token: 0x04000833 RID: 2099
	private bool isOn;

	// Token: 0x04000834 RID: 2100
	private float timer = 0.5f;

	// Token: 0x04000835 RID: 2101
	[SerializeField]
	private Text temperatureText;

	// Token: 0x04000836 RID: 2102
	[SerializeField]
	private Transform raycastSpot;

	// Token: 0x04000837 RID: 2103
	[SerializeField]
	private GameObject canvasObj;

	// Token: 0x04000838 RID: 2104
	[SerializeField]
	private Transform spot;

	// Token: 0x04000839 RID: 2105
	private float currentTemp;

	// Token: 0x0400083A RID: 2106
	private LevelRoom room;
}

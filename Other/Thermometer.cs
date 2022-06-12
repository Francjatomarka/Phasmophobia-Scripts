using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;

public class Thermometer : MonoBehaviour
{
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

	private void Start()
	{
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
		this.temperatureText.text = "";
	}

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

	private void Use()
	{
        if (PhotonNetwork.InRoom)
        {
			this.view.RPC("NetworkedUse", RpcTarget.All, Array.Empty<object>());
			return;
		}
		NetworkedUse();
	}

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

	private IEnumerator PlayNoiseObject()
	{
		this.noise.gameObject.SetActive(true);
		yield return new WaitForEndOfFrame();
		this.noise.gameObject.SetActive(false);
		yield break;
	}

	private AudioSource source;

	private PhotonView view;

	private PhotonObjectInteract photonInteract;

	public LayerMask mask;

	private Noise noise;

	private MeshRenderer rend;

	private bool isOn;

	private float timer = 0.5f;

	[SerializeField]
	private Text temperatureText;

	[SerializeField]
	private Transform raycastSpot;

	[SerializeField]
	private GameObject canvasObj;

	[SerializeField]
	private Transform spot;

	private float currentTemp;

	private LevelRoom room;
}


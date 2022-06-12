using System;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class Sink : MonoBehaviour
{
	private void Awake()
	{
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.view = base.GetComponent<PhotonView>();
		this.noise.gameObject.SetActive(false);
		this.evidence.enabled = false;
	}

	private void Start()
	{
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
	}

	public void Use()
	{
		this.view.RPC("NetworkedUse", RpcTarget.All, Array.Empty<object>());
	}

	public void SpawnDirtyWater()
	{
		this.view.RPC("SpawnDirtyWaterSync", RpcTarget.All, Array.Empty<object>());
	}

	[PunRPC]
	private void SpawnDirtyWaterSync()
	{
		for (int i = 0; i < this.waterRends.Length; i++)
		{
			this.waterRends[i].material.color = new Color32(115, 80, 60, 180);
		}
		this.evidence.enabled = true;
	}

	[PunRPC]
	private void NetworkedUse()
	{
		this.waterIsOn = !this.waterIsOn;
		this.tapWater.SetActive(this.waterIsOn);
		if (this.waterIsOn)
		{
			this.source.Play();
			this.noise.gameObject.SetActive(true);
			return;
		}
		this.source.Stop();
		this.noise.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (this.waterIsOn && this.timer > 0f)
		{
			this.timer -= Time.deltaTime;
			this.water.localPosition = Vector3.MoveTowards(this.water.localPosition, this.target.localPosition, 0.01f * Time.deltaTime);
		}
	}

	private PhotonObjectInteract photonInteract;

	private PhotonView view;

	[SerializeField]
	private Transform water;

	[SerializeField]
	private Transform target;

	private float timer = 20f;

	private bool waterIsOn;

	[SerializeField]
	private AudioSource source;

	[SerializeField]
	private GameObject tapWater;

	[SerializeField]
	private Noise noise;

	[SerializeField]
	private MeshRenderer[] waterRends;

	[SerializeField]
	private Evidence evidence;
}


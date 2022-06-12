using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;

public class ParabolicMicrophone : MonoBehaviour
{
	private void Awake()
	{
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.view = base.GetComponent<PhotonView>();
		this.screenText.gameObject.SetActive(false);
		this.rend.material.DisableKeyword("_EMISSION");
	}

	private void Start()
	{
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
	}

	private void Use()
	{
        if (PhotonNetwork.InRoom)
        {
			this.view.RPC("UseNetworked", RpcTarget.All, Array.Empty<object>());
			return;
		}
		UseNetworked();
	}

	[PunRPC]
	private void UseNetworked()
	{
		this.isOn = !this.isOn;
		this.screenText.text = "00.0";
		this.screenText.gameObject.SetActive(this.isOn);
		if (this.isOn)
		{
			this.rend.material.EnableKeyword("_EMISSION");
			return;
		}
		this.rend.material.DisableKeyword("_EMISSION");
	}

	private void Update()
	{
		if (this.isOn)
		{
			this.checkTimer -= Time.deltaTime;
			if (this.checkTimer < 0f)
			{
				if(this.noises.Count > 0)
                {
					for (int i = 0; i < this.noises.Count; i++)
					{
						this.volume += this.noises[i].volume;
					}
				}
				this.screenText.text = (this.volume * 10f).ToString("00.0");
				this.noises.Clear();
				this.volume = 0f;
				base.StartCoroutine(this.ResetTrigger());
				this.checkTimer = UnityEngine.Random.Range(1f, 2f);
			}
		}
	}

	private IEnumerator ResetTrigger()
	{
		this.col.enabled = false;
		yield return 0;
		this.col.enabled = true;
		yield break;
	}

	[HideInInspector]
	public bool isOn;

	public List<Noise> noises = new List<Noise>();

	private float volume;

	private PhotonObjectInteract photonInteract;

	private PhotonView view;

	private float checkTimer = 2f;

	[SerializeField]
	private BoxCollider col;

	[SerializeField]
	private Text screenText;

	[SerializeField]
	private Renderer rend;
}


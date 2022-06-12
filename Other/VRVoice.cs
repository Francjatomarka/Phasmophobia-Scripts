using System;
using UnityEngine;
using Photon.Pun;

public class VRVoice : MonoBehaviour
{
	private void Awake()
	{
		this.noise.gameObject.SetActive(false);
	}

	private void Start()
	{
		if (PhotonNetwork.InRoom)
		{
			if (GameController.instance != null )
			{
				base.enabled = false;
				base.gameObject.SetActive(false);
			}
		}
	}

	private void Update()
	{
		if (!this.noise.gameObject.activeInHierarchy)
		{
			this.noise.gameObject.SetActive(true);
			if (!this.walkieTalkie.isOn)
			{
				this.noise.volume = 0.4f;
				return;
			}
		}
		if (this.noise.gameObject.activeInHierarchy)
		{
			this.noise.gameObject.SetActive(false);
		}
	}

	[SerializeField]
	private Noise noise;

	[SerializeField]
	private WalkieTalkie walkieTalkie;

}


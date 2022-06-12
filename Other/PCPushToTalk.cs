using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice;

public class PCPushToTalk : MonoBehaviour
{
	private void Awake()
	{
		//this.noise.gameObject.SetActive(false);
	}

	private void Start()
	{
		if (this.view.IsMine && PlayerPrefs.GetInt("localPushToTalkValue") == 1)
		{
			this.isPushToTalk = false;
		}
	}

	private void Update()
	{
		/*if (!this.noise.gameObject.activeInHierarchy)
		{
			this.noise.gameObject.SetActive(true);
			return;
		}
		if (this.noise.gameObject.activeInHierarchy)
		{
			this.noise.gameObject.SetActive(false);
		}*/
	}

	public void OnLocalPushToTalk(InputAction.CallbackContext context)
	{
		
	}

	public void OnGlobalPushToTalk(InputAction.CallbackContext context)
	{
		/*if (context.phase == InputActionPhase.Started && this.walkieTalkie.isActiveAndEnabled && this.player.view.IsMine)
		{
			this.walkieTalkie.Use();
		}
		if (context.phase == InputActionPhase.Canceled && this.walkieTalkie.isActiveAndEnabled && this.player.view.IsMine)
		{
			this.walkieTalkie.Stop();
		}*/
	}

	[SerializeField]
	private Player player;

	[SerializeField]
	private WalkieTalkie walkieTalkie;

	[SerializeField]
	private Noise noise;

	[SerializeField]
	private PhotonView view;

	public bool isPushToTalk = true;
}


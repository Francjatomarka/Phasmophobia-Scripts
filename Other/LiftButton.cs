using System;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

[RequireComponent(typeof(ExitLevel))]
public class LiftButton : MonoBehaviour
{
	private void Start()
	{
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
	}

	private void Update()
	{
		if (this.isAnimating)
		{
			this.timer -= Time.deltaTime;
			if (this.timer < 0f)
			{
				this.isAnimating = false;
				this.timer = 4f;
			}
		}
	}

	private void Use()
	{
		if (GameController.instance)
		{
			this.view.RPC("AttemptUse", RpcTarget.MasterClient, Array.Empty<object>());
		}
	}

	[PunRPC]
	private void AttemptUse()
	{
		if (GameController.instance.playersData.Count != PhotonNetwork.PlayerList.Length)
		{
			return;
		}
		if (this.isAnimating)
		{
			return;
		}
		if (!this.isClosed && this.exitLevel.ThereAreAlivePlayersOutsideTheTruck())
		{
			return;
		}
		this.isAnimating = true;
		this.exitLevel.isExiting = !this.isClosed;
		this.isClosed = !this.isClosed;
		this.view.RPC("NetworkedUse", RpcTarget.AllBufferedViaServer, new object[]
		{
			this.isClosed
		});
	}

	[PunRPC]
	private void NetworkedUse(bool _isClosed)
	{
		this.isClosed = _isClosed;
		this.wallCollider.enabled = _isClosed;
		this.source.Play();
		this.anim.SetTrigger("Switch");
		if (PhotonNetwork.IsMasterClient && this.isClosed)
		{
			this.exitLevel.StartAttemptExitLevel();
		}
	}

	[SerializeField]
	private Animator anim;

	[SerializeField]
	private PhotonView view;

	[SerializeField]
	private PhotonObjectInteract photonInteract;

	[SerializeField]
	private AudioSource source;

	[SerializeField]
	private ExitLevel exitLevel;

	private float timer = 5f;

	private bool isAnimating;

	private bool isClosed = true;

	[SerializeField]
	private Collider wallCollider;
}


using System;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

// Token: 0x020001C4 RID: 452
[RequireComponent(typeof(ExitLevel))]
public class LiftButton : MonoBehaviour
{
	// Token: 0x06000C77 RID: 3191 RVA: 0x0004FCF9 File Offset: 0x0004DEF9
	private void Start()
	{
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
	}

	// Token: 0x06000C78 RID: 3192 RVA: 0x0004FD12 File Offset: 0x0004DF12
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

	// Token: 0x06000C79 RID: 3193 RVA: 0x0004FD4D File Offset: 0x0004DF4D
	private void Use()
	{
		if (GameController.instance)
		{
			this.view.RPC("AttemptUse", RpcTarget.MasterClient, Array.Empty<object>());
		}
	}

	// Token: 0x06000C7A RID: 3194 RVA: 0x0004FD74 File Offset: 0x0004DF74
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

	// Token: 0x06000C7B RID: 3195 RVA: 0x0004FE08 File Offset: 0x0004E008
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

	// Token: 0x04000D18 RID: 3352
	[SerializeField]
	private Animator anim;

	// Token: 0x04000D19 RID: 3353
	[SerializeField]
	private PhotonView view;

	// Token: 0x04000D1A RID: 3354
	[SerializeField]
	private PhotonObjectInteract photonInteract;

	// Token: 0x04000D1B RID: 3355
	[SerializeField]
	private AudioSource source;

	// Token: 0x04000D1C RID: 3356
	[SerializeField]
	private ExitLevel exitLevel;

	// Token: 0x04000D1D RID: 3357
	private float timer = 5f;

	// Token: 0x04000D1E RID: 3358
	private bool isAnimating;

	// Token: 0x04000D1F RID: 3359
	private bool isClosed = true;

	// Token: 0x04000D20 RID: 3360
	[SerializeField]
	private Collider wallCollider;
}

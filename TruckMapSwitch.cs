using System;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

// Token: 0x020001C7 RID: 455
public class TruckMapSwitch : MonoBehaviour
{
	// Token: 0x06000C8B RID: 3211 RVA: 0x00050355 File Offset: 0x0004E555
	private void Awake()
	{
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x06000C8C RID: 3212 RVA: 0x0005036F File Offset: 0x0004E56F
	private void Start()
	{
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
	}

	// Token: 0x06000C8D RID: 3213 RVA: 0x00050388 File Offset: 0x0004E588
	private void Use()
	{
		if (GameController.instance.allPlayersAreConnected)
		{
			this.view.RPC("NetworkedUse", RpcTarget.All, Array.Empty<object>());
		}
	}

	// Token: 0x06000C8E RID: 3214 RVA: 0x000503AC File Offset: 0x0004E5AC
	[PunRPC]
	private void NetworkedUse()
	{
		if (this.source)
		{
			this.source.Play();
		}
		if (MapController.instance)
		{
			MapController.instance.ChangeFloor();
		}
	}

	// Token: 0x04000D3B RID: 3387
	private PhotonView view;

	// Token: 0x04000D3C RID: 3388
	private PhotonObjectInteract photonInteract;

	// Token: 0x04000D3D RID: 3389
	[SerializeField]
	private AudioSource source;
}

using System;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

// Token: 0x02000116 RID: 278
public class Key : MonoBehaviour
{
	// Token: 0x060007C1 RID: 1985 RVA: 0x0002E759 File Offset: 0x0002C959
	private void Awake()
	{
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x0002E774 File Offset: 0x0002C974
	private void Start()
	{
		this.photonInteract.AddGrabbedEvent(new UnityAction(this.Grabbed));
		this.photonInteract.AddPCGrabbedEvent(new UnityAction(this.Grabbed));
		switch (this.type)
		{
		case Key.KeyType.main:
			this.KeyName = "Key_Main";
			return;
		case Key.KeyType.basement:
			this.KeyName = "Key_Basement";
			return;
		case Key.KeyType.garage:
			this.KeyName = "Key_Garage";
			return;
		case Key.KeyType.Car:
			this.KeyName = "Key_Car";
			return;
		default:
			return;
		}
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x0002E7FC File Offset: 0x0002C9FC
	private void Grabbed()
	{
        if (!PhotonNetwork.InRoom)
        {
			GrabbedKey();
		}
		else
        {
			this.view.RPC("GrabbedKey", RpcTarget.AllBuffered, new object[]{});
		}
		if(GameController.instance != null)
        {
			GameController.instance.myPlayer.player.keysAudioSource.Play();
		}
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x0002E838 File Offset: 0x0002CA38
	[PunRPC]
	private void GrabbedKey()
	{
		if (GameController.instance != null && LevelController.instance != null)
		{
			GameController.instance.myPlayer.player.keys.Add(this.type);
			LevelController.instance.journalController.AddKey(LocalisationSystem.GetLocalisedValue(this.KeyName));
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x040007C8 RID: 1992
	private PhotonView view;

	// Token: 0x040007C9 RID: 1993
	public Key.KeyType type;

	// Token: 0x040007CA RID: 1994
	private Door door;

	// Token: 0x040007CB RID: 1995
	private PhotonObjectInteract photonInteract;

	// Token: 0x040007CC RID: 1996
	private Ray playerAim;

	// Token: 0x040007CD RID: 1997
	[SerializeField]
	private LayerMask mask;

	// Token: 0x040007CE RID: 1998
	private string KeyName = "Key";

	// Token: 0x020004CD RID: 1229
	public enum KeyType
	{
		// Token: 0x040022B3 RID: 8883
		main,
		// Token: 0x040022B4 RID: 8884
		basement,
		// Token: 0x040022B5 RID: 8885
		garage,
		// Token: 0x040022B6 RID: 8886
		Car,
		// Token: 0x040022B7 RID: 8887
		none
	}
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice;

// Token: 0x0200016A RID: 362
public class PCPushToTalk : MonoBehaviour
{
	// Token: 0x06000A4A RID: 2634 RVA: 0x0003F8D4 File Offset: 0x0003DAD4
	private void Awake()
	{
		//this.noise.gameObject.SetActive(false);
	}

	// Token: 0x06000A4B RID: 2635 RVA: 0x0003F8E8 File Offset: 0x0003DAE8
	private void Start()
	{
		if (this.view.IsMine && PlayerPrefs.GetInt("localPushToTalkValue") == 1)
		{
			this.isPushToTalk = false;
		}
	}

	// Token: 0x06000A4C RID: 2636 RVA: 0x0003F954 File Offset: 0x0003DB54
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

	// Token: 0x06000A4D RID: 2637 RVA: 0x0003F9BC File Offset: 0x0003DBBC
	public void OnLocalPushToTalk(InputAction.CallbackContext context)
	{
		
	}

	// Token: 0x06000A4E RID: 2638 RVA: 0x0003FA34 File Offset: 0x0003DC34
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

	// Token: 0x04000A70 RID: 2672
	[SerializeField]
	private Player player;

	// Token: 0x04000A71 RID: 2673
	[SerializeField]
	private WalkieTalkie walkieTalkie;

	// Token: 0x04000A72 RID: 2674
	[SerializeField]
	private Noise noise;

	// Token: 0x04000A73 RID: 2675
	[SerializeField]
	private PhotonView view;

	// Token: 0x04000A74 RID: 2676
	public bool isPushToTalk = true;
}

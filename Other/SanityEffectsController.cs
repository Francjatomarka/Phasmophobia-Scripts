using System;
using System.Collections;
using UnityEngine;
using Photon.Pun;

// Token: 0x020000DE RID: 222
[RequireComponent(typeof(PhotonView))]
public class SanityEffectsController : MonoBehaviour
{
	// Token: 0x0600064C RID: 1612 RVA: 0x0002577C File Offset: 0x0002397C
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x0600064D RID: 1613 RVA: 0x0002578C File Offset: 0x0002398C
	private void Update()
	{
		this.timer -= Time.deltaTime;
		if (this.timer < 0f)
		{
			if (PhotonNetwork.IsMasterClient)
			{
				this.AttemptToSpawnWindowGhost();
			}
			this.timer = UnityEngine.Random.Range(10f, (GameController.instance.myPlayer.player.insanity > 50f) ? 30f : 20f);
		}
	}

	// Token: 0x0600064E RID: 1614 RVA: 0x000257FC File Offset: 0x000239FC
	private void AttemptToSpawnWindowGhost()
	{
		if (this.windows.Length == 0 || this.windowGhostObj == null)
		{
			return;
		}
		if (UnityEngine.Random.Range(0, 4) != 1)
		{
			return;
		}
		for (int i = 0; i < GameController.instance.playersData.Count; i++)
		{
			if (!GameController.instance.playersData[i].player.isDead && GameController.instance.playersData[i].player.currentRoom == LevelController.instance.outsideRoom)
			{
				return;
			}
		}
		this.view.RPC("SpawnGhostNetworked", RpcTarget.All, new object[]
		{
			this.windows[UnityEngine.Random.Range(0, this.windows.Length)].view.ViewID
		});
	}

	// Token: 0x0600064F RID: 1615 RVA: 0x000258CC File Offset: 0x00023ACC
	[PunRPC]
	private void SpawnGhostNetworked(int windowViewId)
	{
		if (GameController.instance.myPlayer.player.isDead)
		{
			return;
		}
		base.StartCoroutine(this.SpawnGhostAtWindow(PhotonView.Find(windowViewId).GetComponent<Window>()));
	}

	// Token: 0x06000650 RID: 1616 RVA: 0x000258FD File Offset: 0x00023AFD
	private IEnumerator SpawnGhostAtWindow(Window window)
	{
		if (window.windowGhostStart == null)
		{
			yield return null;
		}
		this.windowGhostObj.transform.position = window.windowGhostStart.position;
		this.windowGhostObj.transform.rotation = window.windowGhostStart.rotation;
		this.windowGhostObj.SetActive(true);
		while (Vector3.Distance(this.windowGhostObj.transform.position, window.windowGhostEnd.position) > 0.2f)
		{
			this.windowGhostObj.transform.Translate(Vector3.forward * Time.deltaTime * 4f);
			yield return null;
		}
		this.windowGhostObj.SetActive(false);
		yield break;
	}

	// Token: 0x04000615 RID: 1557
	private float timer = 30f;

	// Token: 0x04000616 RID: 1558
	private Vector3 pos;

	// Token: 0x04000617 RID: 1559
	private PhotonView view;

	// Token: 0x04000618 RID: 1560
	[SerializeField]
	private Window[] windows;

	// Token: 0x04000619 RID: 1561
	[SerializeField]
	private GameObject windowGhostObj;

	// Token: 0x0400061A RID: 1562
	private GhostTraits.Type ghostType;
}

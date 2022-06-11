using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

// Token: 0x020000CD RID: 205
public class DeadZoneController : MonoBehaviour
{
	// Token: 0x060005C0 RID: 1472 RVA: 0x0002174F File Offset: 0x0001F94F
	private void Awake()
	{
		DeadZoneController.instance = this;
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x060005C1 RID: 1473 RVA: 0x00021763 File Offset: 0x0001F963
	private void Start()
	{
		this.EnableOrDisableDeadZone(false);
	}

	// Token: 0x060005C2 RID: 1474 RVA: 0x0002176C File Offset: 0x0001F96C
	public void SpawnDeathRoom()
	{
		this.playerToAttack = LevelController.instance.currentGhost.playerToKill;
		this.oldGhostPos = LevelController.instance.currentGhost.transform.position;
		this.playerToAttack.SpawnDeadBody(this.playerDeathSpot);
		this.view.RPC("SpawnDeathRoomNetworked", LevelController.instance.currentGhost.playerToKill.view.Owner, Array.Empty<object>());
		base.StartCoroutine(this.TeleportGhostDelay());
		base.StartCoroutine(this.KillPlayerAfterDelay());
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x00021801 File Offset: 0x0001FA01
	private IEnumerator TeleportGhostDelay()
	{
		yield return new WaitForSeconds(3f);
		LevelController.instance.currentGhost.agent.Warp(this.ghostSpawn.position);
		LevelController.instance.currentGhost.anim.SetBool("isIdle", true);
		yield break;
	}

	// Token: 0x060005C4 RID: 1476 RVA: 0x00021810 File Offset: 0x0001FA10
	private IEnumerator KillPlayerAfterDelay()
	{
		yield return new WaitForSeconds(5f);
		this.DespawnDeathRoom();
		this.playerToAttack = null;
		yield break;
	}

	// Token: 0x060005C5 RID: 1477 RVA: 0x00021820 File Offset: 0x0001FA20
	[PunRPC]
	private void SpawnDeathRoomNetworked()
	{
		GameController.instance.myPlayer.player.StopAllMovement();
		GameController.instance.myPlayer.player.ghostDeathHands.SetActive(false);
		this.deadZoneLightObj.SetActive(true);
		if (XRDevice.isPresent)
		{
			this.oldSteamVRPos = GameController.instance.myPlayer.player.steamVRObj.position;
			this.oldVRIKPos = GameController.instance.myPlayer.player.VRIKObj.position;
		}
		else
		{
			this.oldPCPlayerPos = GameController.instance.myPlayer.player.transform.position;
		}
		for (int i = 0; i < this.deathRoomObjets.Length; i++)
		{
			this.deathRoomObjets[i].SetActive(true);
		}
		if (XRDevice.isPresent)
		{
			GameController.instance.myPlayer.player.steamVRObj.position = Vector3.zero;
			GameController.instance.myPlayer.player.VRIKObj.position = this.playerSpawn.position;
			return;
		}
		GameController.instance.myPlayer.player.transform.position = this.playerSpawn.position + Vector3.up;
		GameController.instance.myPlayer.player.charController.velocity.Set(0f, 0f, 0f);
	}

	// Token: 0x060005C6 RID: 1478 RVA: 0x00021998 File Offset: 0x0001FB98
	private void DespawnDeathRoom()
	{
		LevelController.instance.currentGhost.agent.Warp(this.oldGhostPos);
		LevelController.instance.currentGhost.ChangeState(GhostAI.States.favouriteRoom, null, null);
		this.view.RPC("DespawnDeathRoomNetworked", this.playerToAttack.view.Owner, Array.Empty<object>());
	}

	// Token: 0x060005C7 RID: 1479 RVA: 0x000219F7 File Offset: 0x0001FBF7
	[PunRPC]
	private void DespawnDeathRoomNetworked()
	{
		base.StartCoroutine(this.DespawnDeathRoomEvent());
	}

	// Token: 0x060005C8 RID: 1480 RVA: 0x00021A06 File Offset: 0x0001FC06
	private IEnumerator DespawnDeathRoomEvent()
	{
		this.deadZoneLightObj.SetActive(false);
		this.deadZoneLightSmashAudio.Play();
		yield return new WaitForSeconds(2f);
		GameController.instance.myPlayer.player.chokingAudioSource.Play();
		yield return new WaitForSeconds(1.7f);
		for (int i = 0; i < this.deathRoomObjets.Length; i++)
		{
			this.deathRoomObjets[i].SetActive(false);
		}
		GameController.instance.myPlayer.player.KillPlayer();
		if (XRDevice.isPresent)
		{
			GameController.instance.myPlayer.player.steamVRObj.position = this.oldSteamVRPos;
			GameController.instance.myPlayer.player.VRIKObj.position = this.oldVRIKPos;
		}
		else
		{
			GameController.instance.myPlayer.player.transform.position = this.oldPCPlayerPos;
		}
		yield break;
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x00021A15 File Offset: 0x0001FC15
	public void EnableOrDisableDeadZone(bool active)
	{
		this.zoneObjects.SetActive(active);
	}

	// Token: 0x0400055F RID: 1375
	public static DeadZoneController instance;

	// Token: 0x04000560 RID: 1376
	public GameObject zoneObjects;

	// Token: 0x04000561 RID: 1377
	private PhotonView view;

	// Token: 0x04000562 RID: 1378
	[SerializeField]
	private GameObject[] deathRoomObjets;

	// Token: 0x04000563 RID: 1379
	[SerializeField]
	private Transform ghostSpawn;

	// Token: 0x04000564 RID: 1380
	[SerializeField]
	private Transform playerSpawn;

	// Token: 0x04000565 RID: 1381
	private Vector3 oldGhostPos;

	// Token: 0x04000566 RID: 1382
	private Vector3 oldPCPlayerPos;

	// Token: 0x04000567 RID: 1383
	private Vector3 oldSteamVRPos;

	// Token: 0x04000568 RID: 1384
	private Vector3 oldVRIKPos;

	// Token: 0x04000569 RID: 1385
	private Player playerToAttack;

	// Token: 0x0400056A RID: 1386
	[SerializeField]
	private GameObject deadZoneLightObj;

	// Token: 0x0400056B RID: 1387
	[SerializeField]
	private AudioSource deadZoneLightSmashAudio;

	// Token: 0x0400056C RID: 1388
	[HideInInspector]
	public Vector3 playerDeathSpot;
}

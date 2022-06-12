using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

public class DeadZoneController : MonoBehaviour
{
	private void Awake()
	{
		DeadZoneController.instance = this;
		this.view = base.GetComponent<PhotonView>();
	}

	private void Start()
	{
		this.EnableOrDisableDeadZone(false);
	}

	public void SpawnDeathRoom()
	{
		this.playerToAttack = LevelController.instance.currentGhost.playerToKill;
		this.oldGhostPos = LevelController.instance.currentGhost.transform.position;
		this.playerToAttack.SpawnDeadBody(this.playerDeathSpot);
		this.view.RPC("SpawnDeathRoomNetworked", LevelController.instance.currentGhost.playerToKill.view.Owner, Array.Empty<object>());
		base.StartCoroutine(this.TeleportGhostDelay());
		base.StartCoroutine(this.KillPlayerAfterDelay());
	}

	private IEnumerator TeleportGhostDelay()
	{
		yield return new WaitForSeconds(3f);
		LevelController.instance.currentGhost.agent.Warp(this.ghostSpawn.position);
		LevelController.instance.currentGhost.anim.SetBool("isIdle", true);
		yield break;
	}

	private IEnumerator KillPlayerAfterDelay()
	{
		yield return new WaitForSeconds(5f);
		this.DespawnDeathRoom();
		this.playerToAttack = null;
		yield break;
	}

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

	private void DespawnDeathRoom()
	{
		LevelController.instance.currentGhost.agent.Warp(this.oldGhostPos);
		LevelController.instance.currentGhost.ChangeState(GhostAI.States.favouriteRoom, null, null);
		this.view.RPC("DespawnDeathRoomNetworked", this.playerToAttack.view.Owner, Array.Empty<object>());
	}

	[PunRPC]
	private void DespawnDeathRoomNetworked()
	{
		base.StartCoroutine(this.DespawnDeathRoomEvent());
	}

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

	public void EnableOrDisableDeadZone(bool active)
	{
		this.zoneObjects.SetActive(active);
	}

	public static DeadZoneController instance;

	public GameObject zoneObjects;

	private PhotonView view;

	[SerializeField]
	private GameObject[] deathRoomObjets;

	[SerializeField]
	private Transform ghostSpawn;

	[SerializeField]
	private Transform playerSpawn;

	private Vector3 oldGhostPos;

	private Vector3 oldPCPlayerPos;

	private Vector3 oldSteamVRPos;

	private Vector3 oldVRIKPos;

	private Player playerToAttack;

	[SerializeField]
	private GameObject deadZoneLightObj;

	[SerializeField]
	private AudioSource deadZoneLightSmashAudio;

	[HideInInspector]
	public Vector3 playerDeathSpot;
}


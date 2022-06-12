using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine;
using Photon.Pun;

public class WhiteSage : MonoBehaviour
{
	private void Awake()
	{
		this.smoke.SetActive(false);
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.photonInteract.AddPCSecondaryUseEvent(new UnityAction(this.Use));
	}

	private IEnumerator WhiteSageUsed()
	{
		this.smoke.SetActive(true);
		this.isOn = true;
		yield return new WaitForSeconds(9f);
		this.isOn = false;
		this.smoke.SetActive(false);
		for (int i = 0; i < this.rends.Length; i++)
		{
			this.rends[i].material.color = new Color32(99, 99, 99, byte.MaxValue);
		}
		this.Check();
		yield break;
	}

	private void OnDisable()
	{
		if (this.isOn)
		{
			this.isOn = false;
			for (int i = 0; i < this.rends.Length; i++)
			{
				this.rends[i].material.color = new Color32(99, 99, 99, byte.MaxValue);
			}
			this.smoke.SetActive(false);
			this.Check();
		}
	}

	public void Use()
	{
		if (this.hasBeenUsed)
		{
			return;
		}
        if (PhotonNetwork.InRoom)
        {
			this.view.RPC("NetworkedUse", RpcTarget.All, Array.Empty<object>());
			return;
		}
		NetworkedUse();
	}

	[PunRPC]
	private void NetworkedUse()
	{
		this.hasBeenUsed = true;
		base.StartCoroutine(this.WhiteSageUsed());
		this.Check();
	}

	private void Check()
	{
		if (!PhotonNetwork.IsMasterClient && PhotonNetwork.InRoom)
		{
			return;
		}
		if(LevelController.instance != null)
        {
			if (!this.hasMultiplied && LevelController.instance.currentGhostRoom != null && SoundController.instance.GetFloorTypeFromPosition(base.transform.position.y) == LevelController.instance.currentGhostRoom.floorType && Vector3.Distance(base.transform.position, LevelController.instance.currentGhost.transform.position) < 6f)
			{
				LevelController.instance.currentGhost.ghostInfo.activityMultiplier += (float)UnityEngine.Random.Range(20, 30);
				this.hasMultiplied = true;
			}
			LevelController.instance.currentGhost.StartCoroutine(LevelController.instance.currentGhost.TemporarilyStopWander());
			if (LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Yurei)
			{
				LevelController.instance.currentGhost.StartCoroutine(LevelController.instance.currentGhost.TemporarilyStopWander());
			}
			if (MissionBurnSage.instance != null && !MissionBurnSage.instance.completed && LevelController.instance.currentGhostRoom != null && SoundController.instance.GetFloorTypeFromPosition(base.transform.position.y) == LevelController.instance.currentGhostRoom.floorType && Vector3.Distance(base.transform.position, LevelController.instance.currentGhost.transform.position) < 6f)
			{
				MissionBurnSage.instance.CompleteMission();
			}
		}
	}

	[SerializeField]
	private GameObject smoke;

	[SerializeField]
	private PhotonView view;

	[SerializeField]
	private Renderer[] rends;

	private bool hasBeenUsed;

	private bool hasMultiplied;

	private bool isOn;

	private PhotonObjectInteract photonInteract;
}


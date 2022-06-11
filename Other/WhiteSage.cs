using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine;
using Photon.Pun;

// Token: 0x02000128 RID: 296
public class WhiteSage : MonoBehaviour
{
	// Token: 0x06000849 RID: 2121 RVA: 0x000324B3 File Offset: 0x000306B3
	private void Awake()
	{
		this.smoke.SetActive(false);
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.photonInteract.AddPCSecondaryUseEvent(new UnityAction(this.Use));
	}

	// Token: 0x0600084A RID: 2122 RVA: 0x000324C1 File Offset: 0x000306C1
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

	// Token: 0x0600084B RID: 2123 RVA: 0x000324D0 File Offset: 0x000306D0
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

	// Token: 0x0600084C RID: 2124 RVA: 0x00032538 File Offset: 0x00030738
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

	// Token: 0x0600084D RID: 2125 RVA: 0x00032559 File Offset: 0x00030759
	[PunRPC]
	private void NetworkedUse()
	{
		this.hasBeenUsed = true;
		base.StartCoroutine(this.WhiteSageUsed());
		this.Check();
	}

	// Token: 0x0600084E RID: 2126 RVA: 0x00032578 File Offset: 0x00030778
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

	// Token: 0x0400084D RID: 2125
	[SerializeField]
	private GameObject smoke;

	// Token: 0x0400084E RID: 2126
	[SerializeField]
	private PhotonView view;

	// Token: 0x0400084F RID: 2127
	[SerializeField]
	private Renderer[] rends;

	// Token: 0x04000850 RID: 2128
	private bool hasBeenUsed;

	// Token: 0x04000851 RID: 2129
	private bool hasMultiplied;

	// Token: 0x04000852 RID: 2130
	private bool isOn;

	private PhotonObjectInteract photonInteract;
}

using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200006D RID: 109
public class ThirdPersonNetwork : MonoBehaviourPunCallbacks
{
	// Token: 0x06000270 RID: 624 RVA: 0x00010BA4 File Offset: 0x0000EDA4
	private void OnEnable()
	{
		this.firstTake = true;
	}

	// Token: 0x06000271 RID: 625 RVA: 0x00010BB0 File Offset: 0x0000EDB0
	private void Awake()
	{
		this.cameraScript = base.GetComponent<ThirdPersonCamera>();
		this.controllerScript = base.GetComponent<ThirdPersonController>();
		if (base.photonView.IsMine)
		{
			this.cameraScript.enabled = true;
			this.controllerScript.enabled = true;
		}
		else
		{
			this.cameraScript.enabled = false;
			this.controllerScript.enabled = true;
			this.controllerScript.isControllable = false;
		}
		base.gameObject.name = base.gameObject.name + base.photonView.ViewID;
	}

	// Token: 0x06000272 RID: 626 RVA: 0x00010C4C File Offset: 0x0000EE4C
	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext((int)this.controllerScript._characterState);
			stream.SendNext(base.transform.position);
			stream.SendNext(base.transform.rotation);
			return;
		}
		this.controllerScript._characterState = (CharacterState)((int)stream.ReceiveNext());
		this.correctPlayerPos = (Vector3)stream.ReceiveNext();
		this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
		if (this.firstTake)
		{
			this.firstTake = false;
			base.transform.position = this.correctPlayerPos;
			base.transform.rotation = this.correctPlayerRot;
		}
	}

	// Token: 0x06000273 RID: 627 RVA: 0x00010D10 File Offset: 0x0000EF10
	private void Update()
	{
		if (!base.photonView.IsMine)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, this.correctPlayerPos, Time.deltaTime * 5f);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.correctPlayerRot, Time.deltaTime * 5f);
		}
	}

	// Token: 0x040002D4 RID: 724
	private ThirdPersonCamera cameraScript;

	// Token: 0x040002D5 RID: 725
	private ThirdPersonController controllerScript;

	// Token: 0x040002D6 RID: 726
	private bool firstTake;

	// Token: 0x040002D7 RID: 727
	private Vector3 correctPlayerPos = Vector3.zero;

	// Token: 0x040002D8 RID: 728
	private Quaternion correctPlayerRot = Quaternion.identity;
}

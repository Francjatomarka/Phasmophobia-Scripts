using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000074 RID: 116
public class NetworkCharacter : MonoBehaviourPunCallbacks
{
	// Token: 0x0600029F RID: 671 RVA: 0x00011718 File Offset: 0x0000F918
	private void Update()
	{
		if (!base.photonView.IsMine)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, this.correctPlayerPos, Time.deltaTime * 5f);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.correctPlayerRot, Time.deltaTime * 5f);
		}
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x0001178C File Offset: 0x0000F98C
	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(base.transform.position);
			stream.SendNext(base.transform.rotation);
			myThirdPersonController component = base.GetComponent<myThirdPersonController>();
			stream.SendNext((int)component._characterState);
			return;
		}
		this.correctPlayerPos = (Vector3)stream.ReceiveNext();
		this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
		base.GetComponent<myThirdPersonController>()._characterState = (CharacterState)stream.ReceiveNext();
	}

	// Token: 0x040002E8 RID: 744
	private Vector3 correctPlayerPos = Vector3.zero;

	// Token: 0x040002E9 RID: 745
	private Quaternion correctPlayerRot = Quaternion.identity;
}

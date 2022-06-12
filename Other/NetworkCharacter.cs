using System;
using Photon.Pun;
using UnityEngine;

public class NetworkCharacter : MonoBehaviourPunCallbacks
{
	private void Update()
	{
		if (!base.photonView.IsMine)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, this.correctPlayerPos, Time.deltaTime * 5f);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.correctPlayerRot, Time.deltaTime * 5f);
		}
	}

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

	private Vector3 correctPlayerPos = Vector3.zero;

	private Quaternion correctPlayerRot = Quaternion.identity;
}


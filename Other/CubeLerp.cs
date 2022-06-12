using System;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class CubeLerp : MonoBehaviourPunCallbacks, IPunObservable
{
	public void Start()
	{
		this.latestCorrectPos = base.transform.position;
		this.onUpdatePos = base.transform.position;
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			Vector3 localPosition = base.transform.localPosition;
			Quaternion localRotation = base.transform.localRotation;
			stream.Serialize(ref localPosition);
			stream.Serialize(ref localRotation);
			return;
		}
		Vector3 zero = Vector3.zero;
		Quaternion identity = Quaternion.identity;
		stream.Serialize(ref zero);
		stream.Serialize(ref identity);
		this.latestCorrectPos = zero;
		this.onUpdatePos = base.transform.localPosition;
		this.fraction = 0f;
		base.transform.localRotation = identity;
	}

	public void Update()
	{
		if (base.photonView.IsMine)
		{
			return;
		}
		this.fraction += Time.deltaTime * 9f;
		base.transform.localPosition = Vector3.Lerp(this.onUpdatePos, this.latestCorrectPos, this.fraction);
	}

	private Vector3 latestCorrectPos;

	private Vector3 onUpdatePos;

	private float fraction;
}


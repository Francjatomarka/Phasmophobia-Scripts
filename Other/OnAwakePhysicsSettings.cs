using System;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class OnAwakePhysicsSettings : MonoBehaviourPunCallbacks
{
	public void Awake()
	{
		if (!base.photonView.IsMine)
		{
			Rigidbody component = base.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.isKinematic = true;
				return;
			}
			Rigidbody2D component2 = base.GetComponent<Rigidbody2D>();
			if (component2 != null)
			{
				component2.isKinematic = true;
			}
		}
	}
}


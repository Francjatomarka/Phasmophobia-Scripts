using System;
using UnityEngine;
using Photon.Pun;

public class Crucifix : MonoBehaviour
{
	private void Start()
	{
		if (LevelController.instance)
		{
			LevelController.instance.crucifix.Add(this);
		}
	}

	public void Used()
	{
		this.view.RPC("NetworkedUsed", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	[PunRPC]
	public void NetworkedUsed()
	{
		this.usesCount++;
		if (this.usesCount > 1)
		{
			LevelController.instance.crucifix.Remove(this);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	[SerializeField]
	private PhotonView view;

	private int usesCount;
}


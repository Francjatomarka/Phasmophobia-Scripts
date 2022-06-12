using System;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class Mannequin : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
	}

	public void Teleport()
	{
		int num = UnityEngine.Random.Range(0, LevelController.instance.MannequinTeleportSpots.Length);
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < GameController.instance.playersData.Count; i++)
		{
			if (GameController.instance.playersData[i] != null && !Physics.Linecast(base.transform.position + Vector3.up, GameController.instance.playersData[i].player.headObject.transform.position, this.mask, QueryTriggerInteraction.Ignore))
			{
				flag = true;
				break;
			}
		}
		for (int j = 0; j < GameController.instance.playersData.Count; j++)
		{
			if (GameController.instance.playersData[j] != null && !Physics.Linecast(LevelController.instance.MannequinTeleportSpots[num].position + Vector3.up, GameController.instance.playersData[j].player.headObject.transform.position, this.mask, QueryTriggerInteraction.Ignore))
			{
				flag2 = true;
				break;
			}
		}
		if (!flag && !flag2 && LevelController.instance.MannequinTeleportSpots[num].childCount == 0)
		{
			this.view.RPC("TeleportNetworked", RpcTarget.All, new object[]
			{
				num
			});
		}
		this.Rotate();
	}

	public void Rotate()
	{
		this.view.RPC("RotateNetworked", RpcTarget.All, new object[]
		{
			Vector3.up * (float)UnityEngine.Random.Range(0, 360)
		});
	}

	[PunRPC]
	private void TeleportNetworked(int id)
	{
		base.transform.position = LevelController.instance.MannequinTeleportSpots[id].position;
		base.transform.SetParent(LevelController.instance.MannequinTeleportSpots[id]);
	}

	[PunRPC]
	private void RotateNetworked(Vector3 rot)
	{
		base.transform.Rotate(rot);
	}

	private PhotonView view;

	[SerializeField]
	private LayerMask mask;
}


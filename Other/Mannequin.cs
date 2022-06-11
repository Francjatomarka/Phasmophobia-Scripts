using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x02000102 RID: 258
[RequireComponent(typeof(PhotonView))]
public class Mannequin : MonoBehaviour
{
	// Token: 0x0600071B RID: 1819 RVA: 0x0002A8C7 File Offset: 0x00028AC7
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x0600071C RID: 1820 RVA: 0x0002A8D8 File Offset: 0x00028AD8
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

	// Token: 0x0600071D RID: 1821 RVA: 0x0002AA44 File Offset: 0x00028C44
	public void Rotate()
	{
		this.view.RPC("RotateNetworked", RpcTarget.All, new object[]
		{
			Vector3.up * (float)UnityEngine.Random.Range(0, 360)
		});
	}

	// Token: 0x0600071E RID: 1822 RVA: 0x0002AA86 File Offset: 0x00028C86
	[PunRPC]
	private void TeleportNetworked(int id)
	{
		base.transform.position = LevelController.instance.MannequinTeleportSpots[id].position;
		base.transform.SetParent(LevelController.instance.MannequinTeleportSpots[id]);
	}

	// Token: 0x0600071F RID: 1823 RVA: 0x0002AABB File Offset: 0x00028CBB
	[PunRPC]
	private void RotateNetworked(Vector3 rot)
	{
		base.transform.Rotate(rot);
	}

	// Token: 0x04000739 RID: 1849
	private PhotonView view;

	// Token: 0x0400073A RID: 1850
	[SerializeField]
	private LayerMask mask;
}

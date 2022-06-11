using System;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

// Token: 0x02000100 RID: 256
public class KeySpawner : MonoBehaviour
{
	// Token: 0x060006FD RID: 1789 RVA: 0x000294B7 File Offset: 0x000276B7
	private void Start()
	{
		if (this.type != Key.KeyType.main)
		{
			this.SpawnKey(this.type);
		}
		GameController.instance.OnExitLevel.AddListener(new UnityAction(this.DestroyKey));
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x000294E8 File Offset: 0x000276E8
	public void Spawn()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			this.SpawnKey(this.type);
		}
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x00029500 File Offset: 0x00027700
	private void DestroyKey()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		if (this.key)
		{
			this.key.GetComponent<PhotonView>().RequestOwnership();
			if (this.key.GetComponent<PhotonView>().IsMine)
			{
				PhotonNetwork.Destroy(this.key);
			}
		}
	}

	// Token: 0x06000700 RID: 1792 RVA: 0x00029550 File Offset: 0x00027750
	private void SpawnKey(Key.KeyType keyType)
	{
		if (PhotonNetwork.InRoom)
		{
			if (PhotonNetwork.IsMasterClient)
			{
				if (keyType == Key.KeyType.basement)
				{
					this.key = PhotonNetwork.Instantiate("BasementKey", base.transform.position, Quaternion.identity, 0, null);
					return;
				}
				if (keyType == Key.KeyType.Car)
				{
					this.key = PhotonNetwork.Instantiate("CarKey", base.transform.position, Quaternion.identity, 0, null);
					return;
				}
				if (keyType == Key.KeyType.garage)
				{
					this.key = PhotonNetwork.Instantiate("GarageKey", base.transform.position, Quaternion.identity, 0, null);
					return;
				}
				if (keyType == Key.KeyType.main)
				{
					this.key = PhotonNetwork.Instantiate("MainKey", base.transform.position, Quaternion.identity, 0, null);
					return;
				}
			}
		}
		else
		{
			if (keyType == Key.KeyType.basement)
			{
				UnityEngine.Object.Instantiate(Resources.Load("BasementKey"), base.transform.position, Quaternion.identity);
				return;
			}
			if (keyType == Key.KeyType.Car)
			{
				UnityEngine.Object.Instantiate(Resources.Load("CarKey"), base.transform.position, Quaternion.identity);
				return;
			}
			if (keyType == Key.KeyType.garage)
			{
				UnityEngine.Object.Instantiate(Resources.Load("GarageKey"), base.transform.position, Quaternion.identity);
				return;
			}
			if (keyType == Key.KeyType.main)
			{
				UnityEngine.Object.Instantiate(Resources.Load("MainKey"), base.transform.position, Quaternion.identity);
			}
		}
	}

	// Token: 0x04000723 RID: 1827
	public Key.KeyType type;

	// Token: 0x04000724 RID: 1828
	private GameObject key;
}

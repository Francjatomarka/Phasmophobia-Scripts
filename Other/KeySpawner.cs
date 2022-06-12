using System;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class KeySpawner : MonoBehaviour
{
	private void Start()
	{
		if (this.type != Key.KeyType.main)
		{
			this.SpawnKey(this.type);
		}
		GameController.instance.OnExitLevel.AddListener(new UnityAction(this.DestroyKey));
	}

	public void Spawn()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			this.SpawnKey(this.type);
		}
	}

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

	public Key.KeyType type;

	private GameObject key;
}


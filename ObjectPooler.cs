using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

// Token: 0x020000DB RID: 219
public class ObjectPooler : MonoBehaviourPunCallbacks
{
	// Token: 0x06000633 RID: 1587 RVA: 0x00024AF4 File Offset: 0x00022CF4
	private void Awake()
	{
		ObjectPooler.instance = this;
	}

	// Token: 0x06000634 RID: 1588 RVA: 0x00024AFC File Offset: 0x00022CFC
	private void Start()
	{
		if (PhotonNetwork.InRoom || MainManager.instance != null)
		{
			this.SetupPools();
		}
	}

	// Token: 0x06000635 RID: 1589 RVA: 0x00024B18 File Offset: 0x00022D18
	public override void OnJoinedRoom()
	{
		this.SetupPools();
	}

	// Token: 0x06000636 RID: 1590 RVA: 0x00024B20 File Offset: 0x00022D20
	private void SetupPools()
	{
		this.poolDictionary = new Dictionary<string, Queue<GameObject>>();
		foreach (ObjectPooler.Pool pool in this.pools)
		{
			Queue<GameObject> queue = new Queue<GameObject>();
			for (int i = 0; i < pool.size; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(pool.prefab, Vector3.zero, Quaternion.identity);
				gameObject.SetActive(false);
				gameObject.transform.SetParent(base.transform);
				queue.Enqueue(gameObject);
			}
			this.poolDictionary.Add(pool.tag, queue);
		}
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x00024BDC File Offset: 0x00022DDC
	public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
	{
		if (this.poolDictionary == null)
		{
			return null;
		}
		if (!this.poolDictionary.ContainsKey(tag))
		{
			Debug.LogError("Pool with tag: " + tag + " does not exist!");
			return null;
		}
		GameObject gameObject = this.poolDictionary[tag].Dequeue();
		gameObject.SetActive(true);
		gameObject.transform.position = position;
		gameObject.transform.rotation = rotation;
		this.poolDictionary[tag].Enqueue(gameObject);
		return gameObject;
	}

	// Token: 0x040005FD RID: 1533
	public static ObjectPooler instance;

	// Token: 0x040005FE RID: 1534
	public List<ObjectPooler.Pool> pools = new List<ObjectPooler.Pool>();

	// Token: 0x040005FF RID: 1535
	public Dictionary<string, Queue<GameObject>> poolDictionary;

	// Token: 0x020004B5 RID: 1205
	[Serializable]
	public class Pool
	{
		// Token: 0x04002256 RID: 8790
		public string tag;

		// Token: 0x04002257 RID: 8791
		public GameObject prefab;

		// Token: 0x04002258 RID: 8792
		public int size;
	}
}

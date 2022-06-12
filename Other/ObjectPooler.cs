using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ObjectPooler : MonoBehaviourPunCallbacks
{
	private void Awake()
	{
		ObjectPooler.instance = this;
	}

	private void Start()
	{
		if (PhotonNetwork.InRoom || MainManager.instance != null)
		{
			this.SetupPools();
		}
	}

	public override void OnJoinedRoom()
	{
		this.SetupPools();
	}

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

	public static ObjectPooler instance;

	public List<ObjectPooler.Pool> pools = new List<ObjectPooler.Pool>();

	public Dictionary<string, Queue<GameObject>> poolDictionary;

	[Serializable]
	public class Pool
	{
		public string tag;

		public GameObject prefab;

		public int size;
	}
}


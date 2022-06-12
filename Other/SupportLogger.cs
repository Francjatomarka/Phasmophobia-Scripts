using System;
using UnityEngine;

public class SupportLogger : MonoBehaviour
{
	public void Start()
	{
		if (GameObject.Find("PunSupportLogger") == null)
		{
			GameObject gameObject = new GameObject("PunSupportLogger");
			DontDestroyOnLoad(gameObject);
			gameObject.AddComponent<SupportLogging>().LogTrafficStats = this.LogTrafficStats;
		}
	}

	public bool LogTrafficStats = true;
}


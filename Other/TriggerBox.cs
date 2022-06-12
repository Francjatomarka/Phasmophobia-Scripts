using System;
using UnityEngine;

public class TriggerBox : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.gameObject.name);
	}
}


using System;
using UnityEngine;

public class ColliderHit : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		Debug.Log("Collision Enter: " + collision.gameObject.name);
	}

	private void OnCollisionExit(Collision collision)
	{
		Debug.Log("Collision Exit: " + collision.gameObject.name);
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Trigger Stay: " + other.gameObject.name);
	}

	private void OnTriggerExit(Collider other)
	{
		Debug.Log("Trigger Stay: " + other.gameObject.name);
	}
}


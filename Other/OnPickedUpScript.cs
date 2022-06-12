using System;
using UnityEngine;
using Photon.Pun;

public class OnPickedUpScript : MonoBehaviour
{
	public void OnPickedUp(PickupItem item)
	{
		if (item.PickupIsMine)
		{
			Debug.Log("I picked up something. That's a score!");
			return;
		}
		Debug.Log("Someone else picked up something. Lucky!");
	}
}


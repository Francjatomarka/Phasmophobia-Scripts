using System;
using UnityEngine;
using Photon.Pun;

public class OnClickRightDestroy : MonoBehaviour
{
	public void OnPressRight()
	{
		Debug.Log("RightClick Destroy");
		PhotonNetwork.Destroy(base.gameObject);
	}
}


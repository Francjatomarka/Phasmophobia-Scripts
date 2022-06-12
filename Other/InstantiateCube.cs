using System;
using UnityEngine;
using Photon.Pun;

public class InstantiateCube : MonoBehaviour
{
	private void OnClick()
	{
		int instantiateType = this.InstantiateType;
		if (instantiateType == 0)
		{
			PhotonNetwork.Instantiate(this.Prefab.name, base.transform.position + 3f * Vector3.up, Quaternion.identity, 0);
			return;
		}
		if (instantiateType != 1)
		{
			return;
		}
		PhotonNetwork.InstantiateSceneObject(this.Prefab.name, InputToEvent.inputHitPos + new Vector3(0f, 5f, 0f), Quaternion.identity, 0, null);
	}

	public GameObject Prefab;

	public int InstantiateType;

	public bool showGui;
}


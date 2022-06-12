using System;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class ManualPhotonViewAllocator : MonoBehaviour
{
	public void AllocateManualPhotonView()
	{
		PhotonView photonView = base.gameObject.GetPhotonView();
		if (photonView == null)
		{
			Debug.LogError("Can't do manual instantiation without PhotonView component.");
			return;
		}
	}

	[PunRPC]
	public void InstantiateRpc(int viewID)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Prefab, InputToEvent.inputHitPos + new Vector3(0f, 5f, 0f), Quaternion.identity);
		gameObject.GetPhotonView().ViewID = viewID;
		gameObject.GetComponent<OnClickDestroy>().DestroyByRpc = true;
	}

	public GameObject Prefab;
}


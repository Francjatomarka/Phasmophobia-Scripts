using System;
using UnityEngine;
using Photon.Pun;

public class OnClickInstantiate : MonoBehaviour
{
	private void OnClick()
	{
		if (!PhotonNetwork.InRoom)
		{
			return;
		}
		int instantiateType = this.InstantiateType;
		if (instantiateType == 0)
		{
			PhotonNetwork.Instantiate(this.Prefab.name, InputToEvent.inputHitPos + new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
			return;
		}
		if (instantiateType != 1)
		{
			return;
		}
		PhotonNetwork.InstantiateSceneObject(this.Prefab.name, InputToEvent.inputHitPos + new Vector3(0f, 5f, 0f), Quaternion.identity, 0, null);
	}

	private void OnGUI()
	{
		if (this.showGui)
		{
			GUILayout.BeginArea(new Rect((float)(Screen.width - 180), 0f, 180f, 50f));
			this.InstantiateType = GUILayout.Toolbar(this.InstantiateType, this.InstantiateTypeNames, Array.Empty<GUILayoutOption>());
			GUILayout.EndArea();
		}
	}

	public GameObject Prefab;

	public int InstantiateType;

	private string[] InstantiateTypeNames = new string[]
	{
		"Mine",
		"Scene"
	};

	public bool showGui;
}


using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x0200008D RID: 141
public class OnClickInstantiate : MonoBehaviour
{
	// Token: 0x06000455 RID: 1109 RVA: 0x00018A9C File Offset: 0x00016C9C
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

	// Token: 0x06000456 RID: 1110 RVA: 0x00018B2C File Offset: 0x00016D2C
	private void OnGUI()
	{
		if (this.showGui)
		{
			GUILayout.BeginArea(new Rect((float)(Screen.width - 180), 0f, 180f, 50f));
			this.InstantiateType = GUILayout.Toolbar(this.InstantiateType, this.InstantiateTypeNames, Array.Empty<GUILayoutOption>());
			GUILayout.EndArea();
		}
	}

	// Token: 0x0400046B RID: 1131
	public GameObject Prefab;

	// Token: 0x0400046C RID: 1132
	public int InstantiateType;

	// Token: 0x0400046D RID: 1133
	private string[] InstantiateTypeNames = new string[]
	{
		"Mine",
		"Scene"
	};

	// Token: 0x0400046E RID: 1134
	public bool showGui;
}

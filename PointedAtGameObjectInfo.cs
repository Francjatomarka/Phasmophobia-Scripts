using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x02000093 RID: 147
[RequireComponent(typeof(InputToEvent))]
public class PointedAtGameObjectInfo : MonoBehaviour
{
	// Token: 0x06000475 RID: 1141 RVA: 0x00019458 File Offset: 0x00017658
	private void OnGUI()
	{
		if (InputToEvent.goPointedAt != null)
		{
			PhotonView photonView = InputToEvent.goPointedAt.GetPhotonView();
			if (photonView != null)
			{
				GUI.Label(new Rect(Input.mousePosition.x + 5f, (float)Screen.height - Input.mousePosition.y - 15f, 300f, 30f), string.Format("ViewID {0} {1}{2}", photonView.ViewID, photonView.IsSceneView ? "scene " : "", photonView.IsMine ? "mine" : ("owner: " + photonView.Owner.UserId)));
			}
		}
	}
}

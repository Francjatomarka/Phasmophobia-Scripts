using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x0200009C RID: 156
public class ShowStatusWhenConnecting : MonoBehaviour
{
	// Token: 0x060004A2 RID: 1186 RVA: 0x00019BA8 File Offset: 0x00017DA8
	private void OnGUI()
	{
		if (this.Skin != null)
		{
			GUI.skin = this.Skin;
		}
		float num = 400f;
		float num2 = 100f;
		GUILayout.BeginArea(new Rect(((float)Screen.width - num) / 2f, ((float)Screen.height - num2) / 2f, num, num2), GUI.skin.box);
		GUILayout.Label("Connecting" + this.GetConnectingDots(), GUI.skin.customStyles[0], Array.Empty<GUILayoutOption>());
		GUILayout.Label("Status: ", Array.Empty<GUILayoutOption>());
		GUILayout.EndArea();
		if (PhotonNetwork.InRoom)
		{
			base.enabled = false;
		}
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x00019C64 File Offset: 0x00017E64
	private string GetConnectingDots()
	{
		string text = "";
		int num = Mathf.FloorToInt(Time.timeSinceLevelLoad * 3f % 4f);
		for (int i = 0; i < num; i++)
		{
			text += " .";
		}
		return text;
	}

	// Token: 0x0400048B RID: 1163
	public GUISkin Skin;
}

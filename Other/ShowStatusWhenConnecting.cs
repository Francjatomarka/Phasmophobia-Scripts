using System;
using UnityEngine;
using Photon.Pun;

public class ShowStatusWhenConnecting : MonoBehaviour
{
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

	public GUISkin Skin;
}


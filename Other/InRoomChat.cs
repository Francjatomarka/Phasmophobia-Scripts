using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class InRoomChat : MonoBehaviourPunCallbacks
{
	public void Start()
	{
		if (this.AlignBottom)
		{
			this.GuiRect.y = (float)Screen.height - this.GuiRect.height;
		}
	}

	public void OnGUI()
	{
		if (!this.IsVisible || !PhotonNetwork.InRoom)
		{
			return;
		}
		if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
		{
			if (!string.IsNullOrEmpty(this.inputLine))
			{
				base.photonView.RPC("Chat", RpcTarget.All, new object[]
				{
					this.inputLine
				});
				this.inputLine = "";
				GUI.FocusControl("");
				return;
			}
			GUI.FocusControl("ChatInput");
		}
		GUI.SetNextControlName("");
		GUILayout.BeginArea(this.GuiRect);
		this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, Array.Empty<GUILayoutOption>());
		GUILayout.FlexibleSpace();
		for (int i = this.messages.Count - 1; i >= 0; i--)
		{
			GUILayout.Label(this.messages[i], Array.Empty<GUILayoutOption>());
		}
		GUILayout.EndScrollView();
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUI.SetNextControlName("ChatInput");
		this.inputLine = GUILayout.TextField(this.inputLine, Array.Empty<GUILayoutOption>());
		if (GUILayout.Button("Send", new GUILayoutOption[]
		{
			GUILayout.ExpandWidth(false)
		}))
		{
			base.photonView.RPC("Chat", RpcTarget.All, new object[]
			{
				this.inputLine
			});
			this.inputLine = "";
			GUI.FocusControl("");
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	[PunRPC]
	public void Chat(string newLine, PhotonMessageInfo mi)
	{
		string str = "anonymous";
		if (mi.Sender != null)
		{
			if (!string.IsNullOrEmpty(mi.Sender.NickName))
			{
				str = mi.Sender.NickName;
			}
			else
			{
				str = "player " + mi.Sender.UserId;
			}
		}
		this.messages.Add(str + ": " + newLine);
	}

	public void AddLine(string newLine)
	{
		this.messages.Add(newLine);
	}

	public Rect GuiRect = new Rect(0f, 0f, 250f, 300f);

	public bool IsVisible = true;

	public bool AlignBottom;

	public List<string> messages = new List<string>();

	private string inputLine = "";

	private Vector2 scrollPos = Vector2.zero;

	public static readonly string ChatRPC = "Chat";
}


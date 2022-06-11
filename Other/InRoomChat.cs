using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

// Token: 0x020000C9 RID: 201
[RequireComponent(typeof(PhotonView))]
public class InRoomChat : MonoBehaviourPunCallbacks
{
	// Token: 0x060005B4 RID: 1460 RVA: 0x000206CB File Offset: 0x0001E8CB
	public void Start()
	{
		if (this.AlignBottom)
		{
			this.GuiRect.y = (float)Screen.height - this.GuiRect.height;
		}
	}

	// Token: 0x060005B5 RID: 1461 RVA: 0x000206F4 File Offset: 0x0001E8F4
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

	// Token: 0x060005B6 RID: 1462 RVA: 0x00020874 File Offset: 0x0001EA74
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

	// Token: 0x060005B7 RID: 1463 RVA: 0x000208E1 File Offset: 0x0001EAE1
	public void AddLine(string newLine)
	{
		this.messages.Add(newLine);
	}

	// Token: 0x040005D5 RID: 1493
	public Rect GuiRect = new Rect(0f, 0f, 250f, 300f);

	// Token: 0x040005D6 RID: 1494
	public bool IsVisible = true;

	// Token: 0x040005D7 RID: 1495
	public bool AlignBottom;

	// Token: 0x040005D8 RID: 1496
	public List<string> messages = new List<string>();

	// Token: 0x040005D9 RID: 1497
	private string inputLine = "";

	// Token: 0x040005DA RID: 1498
	private Vector2 scrollPos = Vector2.zero;

	// Token: 0x040005DB RID: 1499
	public static readonly string ChatRPC = "Chat";
}

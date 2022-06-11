using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200004C RID: 76
public class FriendItem : MonoBehaviour
{
	// Token: 0x1700002A RID: 42
	// (get) Token: 0x060001A7 RID: 423 RVA: 0x0000B85E File Offset: 0x00009A5E
	// (set) Token: 0x060001A6 RID: 422 RVA: 0x0000B850 File Offset: 0x00009A50
	[HideInInspector]
	public string FriendId
	{
		get
		{
			return this.NameLabel.text;
		}
		set
		{
			this.NameLabel.text = value;
		}
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x0000B86B File Offset: 0x00009A6B
	public void Awake()
	{
		this.Health.text = string.Empty;
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x0000B880 File Offset: 0x00009A80
	public void OnFriendStatusUpdate(int status, bool gotMessage, object message)
	{
		string text;
		switch (status)
		{
		case 1:
			text = "Invisible";
			break;
		case 2:
			text = "Online";
			break;
		case 3:
			text = "Away";
			break;
		case 4:
			text = "Do not disturb";
			break;
		case 5:
			text = "Looking For Game/Group";
			break;
		case 6:
			text = "Playing";
			break;
		default:
			text = "Offline";
			break;
		}
		this.StatusLabel.text = text;
		if (gotMessage)
		{
			string text2 = string.Empty;
			if (message != null)
			{
				string[] array = message as string[];
				if (array != null && array.Length >= 2)
				{
					text2 = array[1] + "%";
				}
			}
			this.Health.text = text2;
		}
	}

	// Token: 0x040001D8 RID: 472
	public Text NameLabel;

	// Token: 0x040001D9 RID: 473
	public Text StatusLabel;

	// Token: 0x040001DA RID: 474
	public Text Health;
}

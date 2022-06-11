using System;
using UnityEngine;

// Token: 0x02000057 RID: 87
public class MessageOverlay : MonoBehaviour
{
	// Token: 0x060001D8 RID: 472 RVA: 0x0000CF79 File Offset: 0x0000B179
	public void Start()
	{
		this.SetActive(true);
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x0000CF82 File Offset: 0x0000B182
	public void OnJoinedRoom()
	{
		this.SetActive(false);
	}

	// Token: 0x060001DA RID: 474 RVA: 0x0000CF79 File Offset: 0x0000B179
	public void OnLeftRoom()
	{
		this.SetActive(true);
	}

	// Token: 0x060001DB RID: 475 RVA: 0x0000CF8C File Offset: 0x0000B18C
	private void SetActive(bool enable)
	{
		GameObject[] objects = this.Objects;
		for (int i = 0; i < objects.Length; i++)
		{
			objects[i].SetActive(enable);
		}
	}

	// Token: 0x040001FD RID: 509
	public GameObject[] Objects;
}

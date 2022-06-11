using System;
using TLGFPowerBooks;
using UnityEngine;

// Token: 0x0200001C RID: 28
public class ExampleUIController : MonoBehaviour
{
	// Token: 0x060000C8 RID: 200 RVA: 0x000065AD File Offset: 0x000047AD
	public void OpenBook()
	{
		this.pBook.OpenBook();
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x000065BA File Offset: 0x000047BA
	public void CloseBook()
	{
		this.pBook.CloseBook();
	}

	// Token: 0x060000CA RID: 202 RVA: 0x000065C7 File Offset: 0x000047C7
	public void NextPage()
	{
		this.pBook.NextPage();
	}

	// Token: 0x060000CB RID: 203 RVA: 0x000065D4 File Offset: 0x000047D4
	public void PrevPage()
	{
		this.pBook.PrevPage();
	}

	// Token: 0x060000CC RID: 204 RVA: 0x000065E1 File Offset: 0x000047E1
	public void GoToLastPage()
	{
		this.pBook.GoToLastPage(50f);
	}

	// Token: 0x060000CD RID: 205 RVA: 0x000065F3 File Offset: 0x000047F3
	public void GoToFirstPage()
	{
		this.pBook.GoToFirstPage(50f);
	}

	// Token: 0x060000CE RID: 206 RVA: 0x00006605 File Offset: 0x00004805
	public void JumpToLastPage()
	{
		this.pBook.JumpToLastPage(true);
	}

	// Token: 0x060000CF RID: 207 RVA: 0x00006613 File Offset: 0x00004813
	public void JumpToFirstPage()
	{
		this.pBook.JumpToFirstPage(true);
	}

	// Token: 0x040000B3 RID: 179
	public PBook pBook;
}

using System;
using TLGFPowerBooks;
using UnityEngine;

// Token: 0x02000018 RID: 24
public class ExampleEventHandler : MonoBehaviour
{
	// Token: 0x060000B1 RID: 177 RVA: 0x00005900 File Offset: 0x00003B00
	private void OnEnable()
	{
		PBook.OnBookOpened += this.OpenBookChangeColor;
		PBook.OnBookWillOpen += this.BookWillOpenChangeColor;
		PBook.OnBookClosed += this.CloseBookChangeColor;
		PBook.OnBookWillClose += this.BookWillCloseChangeColor;
		PBook.OnBookLastPage += this.LastPageChangeColor;
		PBook.OnBookFirstPage += this.FirstPageChangeColor;
		PBook.OnBookTurnToLastPage += this.TurnToLastPageChangeColor;
		PBook.OnBookTurnToFirstPage += this.TurnToFirstPageChangeColor;
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x00005998 File Offset: 0x00003B98
	private void OnDisable()
	{
		PBook.OnBookOpened -= this.OpenBookChangeColor;
		PBook.OnBookWillOpen -= this.BookWillOpenChangeColor;
		PBook.OnBookClosed -= this.CloseBookChangeColor;
		PBook.OnBookWillClose -= this.BookWillCloseChangeColor;
		PBook.OnBookLastPage -= this.LastPageChangeColor;
		PBook.OnBookFirstPage -= this.FirstPageChangeColor;
		PBook.OnBookTurnToLastPage -= this.TurnToLastPageChangeColor;
		PBook.OnBookTurnToFirstPage -= this.TurnToFirstPageChangeColor;
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x00005A30 File Offset: 0x00003C30
	private void OpenBookChangeColor(GameObject sender)
	{
		if (this.pbook != null && sender == this.pbook.gameObject)
		{
			base.transform.GetComponent<Renderer>().material.SetColor("_Color", this.openBookColor);
		}
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x00005A80 File Offset: 0x00003C80
	private void BookWillOpenChangeColor(GameObject sender)
	{
		if (this.pbook != null && sender == this.pbook.gameObject)
		{
			base.transform.GetComponent<Renderer>().material.SetColor("_Color", this.willOpenBookColor);
		}
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x00005AD0 File Offset: 0x00003CD0
	public void CloseBookChangeColor(GameObject sender)
	{
		if (this.pbook != null && sender == this.pbook.gameObject)
		{
			base.transform.GetComponent<Renderer>().material.SetColor("_Color", this.closeBookColor);
		}
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x00005B20 File Offset: 0x00003D20
	private void BookWillCloseChangeColor(GameObject sender)
	{
		if (this.pbook != null && sender == this.pbook.gameObject)
		{
			base.transform.GetComponent<Renderer>().material.SetColor("_Color", this.willCloseBookColor);
		}
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x00005B70 File Offset: 0x00003D70
	private void LastPageChangeColor(GameObject sender)
	{
		if (this.pbook != null && sender == this.pbook.gameObject)
		{
			base.transform.GetComponent<Renderer>().material.SetColor("_Color", this.lastPageColor);
		}
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x00005BC0 File Offset: 0x00003DC0
	private void FirstPageChangeColor(GameObject sender)
	{
		if (this.pbook != null && sender == this.pbook.gameObject)
		{
			base.transform.GetComponent<Renderer>().material.SetColor("_Color", this.firstPageColor);
		}
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x00005C10 File Offset: 0x00003E10
	private void TurnToLastPageChangeColor(GameObject sender)
	{
		if (this.pbook != null && sender == this.pbook.gameObject)
		{
			base.transform.GetComponent<Renderer>().material.SetColor("_Color", this.enterLastPageColor);
		}
	}

	// Token: 0x060000BA RID: 186 RVA: 0x00005C60 File Offset: 0x00003E60
	private void TurnToFirstPageChangeColor(GameObject sender)
	{
		if (this.pbook != null && sender == this.pbook.gameObject)
		{
			base.transform.GetComponent<Renderer>().material.SetColor("_Color", this.enterFirstPageColor);
		}
	}

	// Token: 0x04000087 RID: 135
	public PBook pbook;

	// Token: 0x04000088 RID: 136
	public Color openBookColor;

	// Token: 0x04000089 RID: 137
	public Color willOpenBookColor;

	// Token: 0x0400008A RID: 138
	public Color closeBookColor;

	// Token: 0x0400008B RID: 139
	public Color willCloseBookColor;

	// Token: 0x0400008C RID: 140
	public Color lastPageColor;

	// Token: 0x0400008D RID: 141
	public Color firstPageColor;

	// Token: 0x0400008E RID: 142
	public Color enterLastPageColor;

	// Token: 0x0400008F RID: 143
	public Color enterFirstPageColor;
}

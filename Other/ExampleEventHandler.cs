using System;
using TLGFPowerBooks;
using UnityEngine;

public class ExampleEventHandler : MonoBehaviour
{
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

	private void OpenBookChangeColor(GameObject sender)
	{
		if (this.pbook != null && sender == this.pbook.gameObject)
		{
			base.transform.GetComponent<Renderer>().material.SetColor("_Color", this.openBookColor);
		}
	}

	private void BookWillOpenChangeColor(GameObject sender)
	{
		if (this.pbook != null && sender == this.pbook.gameObject)
		{
			base.transform.GetComponent<Renderer>().material.SetColor("_Color", this.willOpenBookColor);
		}
	}

	public void CloseBookChangeColor(GameObject sender)
	{
		if (this.pbook != null && sender == this.pbook.gameObject)
		{
			base.transform.GetComponent<Renderer>().material.SetColor("_Color", this.closeBookColor);
		}
	}

	private void BookWillCloseChangeColor(GameObject sender)
	{
		if (this.pbook != null && sender == this.pbook.gameObject)
		{
			base.transform.GetComponent<Renderer>().material.SetColor("_Color", this.willCloseBookColor);
		}
	}

	private void LastPageChangeColor(GameObject sender)
	{
		if (this.pbook != null && sender == this.pbook.gameObject)
		{
			base.transform.GetComponent<Renderer>().material.SetColor("_Color", this.lastPageColor);
		}
	}

	private void FirstPageChangeColor(GameObject sender)
	{
		if (this.pbook != null && sender == this.pbook.gameObject)
		{
			base.transform.GetComponent<Renderer>().material.SetColor("_Color", this.firstPageColor);
		}
	}

	private void TurnToLastPageChangeColor(GameObject sender)
	{
		if (this.pbook != null && sender == this.pbook.gameObject)
		{
			base.transform.GetComponent<Renderer>().material.SetColor("_Color", this.enterLastPageColor);
		}
	}

	private void TurnToFirstPageChangeColor(GameObject sender)
	{
		if (this.pbook != null && sender == this.pbook.gameObject)
		{
			base.transform.GetComponent<Renderer>().material.SetColor("_Color", this.enterFirstPageColor);
		}
	}

	public PBook pbook;

	public Color openBookColor;

	public Color willOpenBookColor;

	public Color closeBookColor;

	public Color willCloseBookColor;

	public Color lastPageColor;

	public Color firstPageColor;

	public Color enterLastPageColor;

	public Color enterFirstPageColor;
}


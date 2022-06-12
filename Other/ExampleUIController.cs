using System;
using TLGFPowerBooks;
using UnityEngine;

public class ExampleUIController : MonoBehaviour
{
	public void OpenBook()
	{
		this.pBook.OpenBook();
	}

	public void CloseBook()
	{
		this.pBook.CloseBook();
	}

	public void NextPage()
	{
		this.pBook.NextPage();
	}

	public void PrevPage()
	{
		this.pBook.PrevPage();
	}

	public void GoToLastPage()
	{
		this.pBook.GoToLastPage(50f);
	}

	public void GoToFirstPage()
	{
		this.pBook.GoToFirstPage(50f);
	}

	public void JumpToLastPage()
	{
		this.pBook.JumpToLastPage(true);
	}

	public void JumpToFirstPage()
	{
		this.pBook.JumpToFirstPage(true);
	}

	public PBook pBook;
}


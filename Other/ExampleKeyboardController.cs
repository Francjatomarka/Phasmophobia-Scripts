using System;
using TLGFPowerBooks;
using UnityEngine;

public class ExampleKeyboardController : MonoBehaviour
{
	private void Update()
	{
		if (this.openCloseKey != KeyCode.None && Input.GetKeyDown(this.openCloseKey))
		{
			if (this.pBook.GetBookState() == PBook.BookState.CLOSED)
			{
				this.pBook.OpenBook();
			}
			if (this.pBook.GetBookState() == PBook.BookState.OPEN)
			{
				this.pBook.CloseBook();
			}
		}
		if (this.nextPageKey != KeyCode.None && Input.GetKeyDown(this.nextPageKey))
		{
			this.pBook.NextPage();
		}
		if (this.prevPageKey != KeyCode.None && Input.GetKeyDown(this.prevPageKey))
		{
			this.pBook.PrevPage();
		}
		if (this.gotoLastPageKey != KeyCode.None && Input.GetKeyDown(this.gotoLastPageKey))
		{
			this.pBook.GoToLastPage(this.gotoSpeed);
		}
		if (this.gotoFirstPageKey != KeyCode.None && Input.GetKeyDown(this.gotoFirstPageKey))
		{
			this.pBook.GoToFirstPage(this.gotoSpeed);
		}
		if (this.jumpToLastPageKey != KeyCode.None && Input.GetKeyDown(this.jumpToLastPageKey))
		{
			this.pBook.JumpToLastPage(this.playSoundOnJump);
		}
		if (this.gotoFirstPageKey != KeyCode.None && Input.GetKeyDown(this.jumpToFirstPageKey))
		{
			this.pBook.JumpToFirstPage(this.playSoundOnJump);
		}
	}

	public PBook pBook;

	public KeyCode openCloseKey = KeyCode.Space;

	public KeyCode nextPageKey = KeyCode.D;

	public KeyCode prevPageKey = KeyCode.A;

	public KeyCode gotoLastPageKey = KeyCode.E;

	public KeyCode gotoFirstPageKey = KeyCode.Q;

	public KeyCode jumpToLastPageKey = KeyCode.C;

	public KeyCode jumpToFirstPageKey = KeyCode.Y;

	public float gotoSpeed = 40f;

	public bool playSoundOnJump = true;
}


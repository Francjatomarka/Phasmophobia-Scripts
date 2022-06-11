using System;
using TLGFPowerBooks;
using UnityEngine;

// Token: 0x0200001B RID: 27
public class ExampleKeyboardController : MonoBehaviour
{
	// Token: 0x060000C8 RID: 200 RVA: 0x00006570 File Offset: 0x00004770
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

	// Token: 0x040000AC RID: 172
	public PBook pBook;

	// Token: 0x040000AD RID: 173
	public KeyCode openCloseKey = KeyCode.Space;

	// Token: 0x040000AE RID: 174
	public KeyCode nextPageKey = KeyCode.D;

	// Token: 0x040000AF RID: 175
	public KeyCode prevPageKey = KeyCode.A;

	// Token: 0x040000B0 RID: 176
	public KeyCode gotoLastPageKey = KeyCode.E;

	// Token: 0x040000B1 RID: 177
	public KeyCode gotoFirstPageKey = KeyCode.Q;

	// Token: 0x040000B2 RID: 178
	public KeyCode jumpToLastPageKey = KeyCode.C;

	// Token: 0x040000B3 RID: 179
	public KeyCode jumpToFirstPageKey = KeyCode.Y;

	// Token: 0x040000B4 RID: 180
	public float gotoSpeed = 40f;

	// Token: 0x040000B5 RID: 181
	public bool playSoundOnJump = true;
}

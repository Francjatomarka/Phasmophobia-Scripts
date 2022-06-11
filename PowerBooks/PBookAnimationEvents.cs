using System;
using UnityEngine;

namespace TLGFPowerBooks
{
	// Token: 0x02000016 RID: 22
	public class PBookAnimationEvents : MonoBehaviour
	{
		// Token: 0x0600006F RID: 111 RVA: 0x00004B40 File Offset: 0x00002D40
		private void Start()
		{
			this.bookAnimator = this.bookController.GetBookAnimator();
			this.pageAnimator = this.bookController.GetPageAnimator();
			AnimationEvent animationEvent = new AnimationEvent();
			AnimationEvent animationEvent2 = new AnimationEvent();
			animationEvent.time = 0f;
			animationEvent2.time = 1.983f;
			animationEvent.functionName = "BookClosed";
			animationEvent2.functionName = "BookOpened";
			this.bookAnimator.runtimeAnimatorController.animationClips[1].AddEvent(animationEvent);
			this.bookAnimator.runtimeAnimatorController.animationClips[1].AddEvent(animationEvent2);
			AnimationEvent animationEvent3 = new AnimationEvent();
			AnimationEvent animationEvent4 = new AnimationEvent();
			animationEvent3.time = 1.895f;
			animationEvent4.time = 0.085f;
			animationEvent3.functionName = "NextPageEndAnim";
			animationEvent4.functionName = "PrevPageEndAnim";
			this.pageAnimator.runtimeAnimatorController.animationClips[1].AddEvent(animationEvent3);
			this.pageAnimator.runtimeAnimatorController.animationClips[1].AddEvent(animationEvent4);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00004C3F File Offset: 0x00002E3F
		public void BookOpened()
		{
			if (this.bookController.GetBookState() == PBook.BookState.OPENBOOK || this.bookController.GetBookState() == PBook.BookState.CANCEL_DRAG_OPENCLOSE)
			{
				this.bookController.BookOpenedAnimEvent();
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004C69 File Offset: 0x00002E69
		public void BookClosed()
		{
			if (this.bookController.GetBookState() == PBook.BookState.CLOSEBOOK || this.bookController.GetBookState() == PBook.BookState.CANCEL_DRAG_OPENCLOSE)
			{
				this.bookController.BookClosedAnimEvent();
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00004C93 File Offset: 0x00002E93
		public void NextPageEndAnim()
		{
			if (this.bookController.GetBookState() == PBook.BookState.NEXTPAGE || this.bookController.GetBookState() == PBook.BookState.CANCEL_DRAG_NEXTPREV)
			{
				this.bookController.NextPageEndAnimEvent();
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004CBD File Offset: 0x00002EBD
		public void PrevPageEndAnim()
		{
			if (this.bookController.GetBookState() == PBook.BookState.PREVPAGE || this.bookController.GetBookState() == PBook.BookState.CANCEL_DRAG_NEXTPREV)
			{
				this.bookController.PrevPageEndAnimEvent();
			}
		}

		// Token: 0x0400003F RID: 63
		public PBook bookController;

		// Token: 0x04000040 RID: 64
		private Animator bookAnimator;

		// Token: 0x04000041 RID: 65
		private Animator pageAnimator;
	}
}

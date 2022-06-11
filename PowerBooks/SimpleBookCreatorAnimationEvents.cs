using System;
using UnityEngine;

namespace TLGFPowerBooks
{
	// Token: 0x0200001A RID: 26
	public class SimpleBookCreatorAnimationEvents : MonoBehaviour
	{
		// Token: 0x0600008D RID: 141 RVA: 0x000069E4 File Offset: 0x00004BE4
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

		// Token: 0x0600008E RID: 142 RVA: 0x00006AE3 File Offset: 0x00004CE3
		public void BookOpened()
		{
			if (this.bookController.GetBookState() == SimpleBookCreator.BookState.OPENBOOK)
			{
				this.bookController.BookOpenedEvent();
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00006AFE File Offset: 0x00004CFE
		public void BookClosed()
		{
			if (this.bookController.GetBookState() == SimpleBookCreator.BookState.CLOSEBOOK)
			{
				this.bookController.BookClosedEvent();
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00006B19 File Offset: 0x00004D19
		public void NextPageEndAnim()
		{
			if (this.bookController.GetBookState() == SimpleBookCreator.BookState.NEXTPAGE)
			{
				this.bookController.NextPageEndAnimEvent();
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00006B34 File Offset: 0x00004D34
		public void PrevPageEndAnim()
		{
			if (this.bookController.GetBookState() == SimpleBookCreator.BookState.PREVPAGE)
			{
				this.bookController.PrevPageEndAnimEvent();
			}
		}

		// Token: 0x0400006B RID: 107
		public SimpleBookCreator bookController;

		// Token: 0x0400006C RID: 108
		private Animator bookAnimator;

		// Token: 0x0400006D RID: 109
		private Animator pageAnimator;
	}
}

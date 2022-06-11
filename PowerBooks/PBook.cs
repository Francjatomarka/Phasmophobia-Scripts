using System;
using System.Collections;
using UnityEngine;

namespace TLGFPowerBooks
{
	// Token: 0x0200000C RID: 12
	public class PBook : MonoBehaviour
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600000B RID: 11 RVA: 0x000020A0 File Offset: 0x000002A0
		// (remove) Token: 0x0600000C RID: 12 RVA: 0x000020D4 File Offset: 0x000002D4
		public static event PBook.BooKOpenedAction OnBookOpened;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600000D RID: 13 RVA: 0x00002108 File Offset: 0x00000308
		// (remove) Token: 0x0600000E RID: 14 RVA: 0x0000213C File Offset: 0x0000033C
		public static event PBook.BooKClosedAction OnBookClosed;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600000F RID: 15 RVA: 0x00002170 File Offset: 0x00000370
		// (remove) Token: 0x06000010 RID: 16 RVA: 0x000021A4 File Offset: 0x000003A4
		public static event PBook.BooKWillOpenAction OnBookWillOpen;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000011 RID: 17 RVA: 0x000021D8 File Offset: 0x000003D8
		// (remove) Token: 0x06000012 RID: 18 RVA: 0x0000220C File Offset: 0x0000040C
		public static event PBook.BooKWillCloseAction OnBookWillClose;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000013 RID: 19 RVA: 0x00002240 File Offset: 0x00000440
		// (remove) Token: 0x06000014 RID: 20 RVA: 0x00002274 File Offset: 0x00000474
		public static event PBook.BookFirstPageAction OnBookFirstPage;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000015 RID: 21 RVA: 0x000022A8 File Offset: 0x000004A8
		// (remove) Token: 0x06000016 RID: 22 RVA: 0x000022DC File Offset: 0x000004DC
		public static event PBook.BookLastPageAction OnBookLastPage;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000017 RID: 23 RVA: 0x00002310 File Offset: 0x00000510
		// (remove) Token: 0x06000018 RID: 24 RVA: 0x00002344 File Offset: 0x00000544
		public static event PBook.BookEnterFirstPageAction OnBookTurnToFirstPage;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000019 RID: 25 RVA: 0x00002378 File Offset: 0x00000578
		// (remove) Token: 0x0600001A RID: 26 RVA: 0x000023AC File Offset: 0x000005AC
		public static event PBook.BookEnterLastPageAction OnBookTurnToLastPage;

		// Token: 0x0600001B RID: 27 RVA: 0x000023E0 File Offset: 0x000005E0
		private void Awake()
		{
			this.tr = base.GetComponent<Transform>();
			this.book = this.tr.Find("Book").gameObject;
			this.animatedBook = this.tr.Find("AnimatedBook").gameObject;
			this.animatedBookRenderer = this.tr.Find("AnimatedBook/Book").GetComponent<SkinnedMeshRenderer>();
			Bounds localBounds = this.animatedBookRenderer.localBounds;
			localBounds.extents = new Vector3(this.animatedBookRenderer.localBounds.extents.x, this.animatedBookRenderer.localBounds.extents.x * 2f, this.animatedBookRenderer.localBounds.extents.z);
			this.animatedBookRenderer.localBounds = localBounds;
			this.animatedPage = this.animatedBook.transform.Find("AnimatedPage").gameObject;
			this.bookAnimator = this.animatedBook.GetComponent<Animator>();
			this.pageAnimator = this.animatedPage.GetComponent<Animator>();
			this.audioSource = this.tr.GetComponent<AudioSource>();
			this.pages = new Transform[4];
			this.pages[0] = this.animatedBook.transform.Find("PageCenterLeft/PageLeftB");
			this.pages[1] = this.animatedPage.transform.Find("PageCenter/PageRightP");
			this.pages[2] = this.animatedPage.transform.Find("PageCenter/PageLeftP");
			this.pages[3] = this.animatedBook.transform.Find("PageCenterRight/PageRightB");
			this.contentPages = PBook.GetComponentsInDirectChildren<RectTransform>(this.contentContainer);
			this.pageCount = this.contentPages.Length;
			this.bookAnimator.speed = this.openCloseSpeed;
			this.pageAnimator.speed = this.pageTurnSpeed + 0.6f;
			this.bookState = PBook.BookState.CLOSED;
			if (this.startAtPageNumber > 2 && this.startAtPageNumber <= this.pageCount)
			{
				this.JumpToPage(this.startAtPageNumber, false);
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002604 File Offset: 0x00000804
		private void PlayOpenBookSound()
		{
			if (this.openBookSounds.Length > 0)
			{
				if (this.autoPitchOpenCloseSounds)
				{
					this.audioSource.pitch = this.bookAnimator.speed / 2f;
				}
				else
				{
					this.audioSource.pitch = 1f;
				}
				this.audioSource.volume = this.openCloseSoundVolume;
				this.audioSource.PlayOneShot(this.openBookSounds[UnityEngine.Random.Range(0, this.openBookSounds.Length)]);
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002684 File Offset: 0x00000884
		private void PlayCloseBookSound()
		{
			if (this.closeBookSounds.Length > 0)
			{
				if (this.autoPitchOpenCloseSounds)
				{
					this.audioSource.pitch = this.bookAnimator.speed / 2f;
				}
				else
				{
					this.audioSource.pitch = 1f;
				}
				this.audioSource.volume = this.openCloseSoundVolume;
				this.audioSource.PlayOneShot(this.closeBookSounds[UnityEngine.Random.Range(0, this.closeBookSounds.Length)]);
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002704 File Offset: 0x00000904
		private void PlayPageTurnSound()
		{
			if (this.pageTurnSounds.Length > 0)
			{
				if (this.autoPitchPageTurnSounds)
				{
					this.audioSource.pitch = (this.pageAnimator.speed - 0.6f) / 2f;
				}
				else
				{
					this.audioSource.pitch = 1f;
				}
				this.audioSource.volume = this.pageTurnSoundVolume;
				this.audioSource.PlayOneShot(this.pageTurnSounds[UnityEngine.Random.Range(0, this.pageTurnSounds.Length)]);
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000278C File Offset: 0x0000098C
		public void OpenBook()
		{
			if (this.bookState == PBook.BookState.CLOSED)
			{
				this.bookState = PBook.BookState.OPENBOOK;
				if (this.pageCount > this.currentPageIndex)
				{
					this.contentPages[this.currentPageIndex].SetParent(this.pages[0]);
					this.PrepareContentPage(this.contentPages[this.currentPageIndex]);
				}
				if (this.pageCount > this.currentPageIndex + 1)
				{
					this.contentPages[this.currentPageIndex + 1].SetParent(this.pages[3]);
					this.PrepareContentPage(this.contentPages[this.currentPageIndex + 1]);
				}
				this.book.SetActive(false);
				this.animatedBook.SetActive(true);
				this.bookAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
				this.pageAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
				this.bookAnimator.Play("OpenBook");
				if (this.playOpenBookSound)
				{
					this.PlayOpenBookSound();
				}
				if (PBook.OnBookWillOpen != null)
				{
					PBook.OnBookWillOpen(base.gameObject);
				}
				if (this.onBookWillOpen.GetPersistentEventCount() > 0)
				{
					this.onBookWillOpen.Invoke();
				}
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000028A8 File Offset: 0x00000AA8
		public void BookOpenedAnimEvent()
		{
			if (this.bookState == PBook.BookState.CANCEL_DRAG_OPENCLOSE)
			{
				this.isBlocked = false;
			}
			this.bookState = PBook.BookState.OPEN;
			if (PBook.OnBookOpened != null)
			{
				PBook.OnBookOpened(base.gameObject);
			}
			if (this.onBookOpened.GetPersistentEventCount() > 0)
			{
				this.onBookOpened.Invoke();
			}
			if (this.firstLastPageEventsOnBookOpen)
			{
				if (this.IsFirstPage() && PBook.OnBookFirstPage != null)
				{
					PBook.OnBookFirstPage(base.gameObject);
				}
				if (this.IsLastPage() && PBook.OnBookLastPage != null)
				{
					PBook.OnBookLastPage(base.gameObject);
				}
				if (this.IsFirstPage() && this.onBookFirstPage.GetPersistentEventCount() > 0)
				{
					this.onBookFirstPage.Invoke();
				}
				if (this.IsLastPage() && this.onBookLastPage.GetPersistentEventCount() > 0)
				{
					this.onBookLastPage.Invoke();
				}
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002988 File Offset: 0x00000B88
		public void CloseBook()
		{
			if (this.bookState == PBook.BookState.OPEN)
			{
				this.bookState = PBook.BookState.CLOSEBOOK;
				this.bookAnimator.Play("CloseBook");
				if (this.playCloseBookSound)
				{
					this.PlayCloseBookSound();
				}
				if (PBook.OnBookWillClose != null)
				{
					PBook.OnBookWillClose(base.gameObject);
				}
				if (this.onBookWillClose.GetPersistentEventCount() > 0)
				{
					this.onBookWillClose.Invoke();
				}
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000029F4 File Offset: 0x00000BF4
		public void BookClosedAnimEvent()
		{
			if (this.bookState == PBook.BookState.CANCEL_DRAG_OPENCLOSE)
			{
				this.isBlocked = false;
			}
			this.bookState = PBook.BookState.CLOSED;
			this.book.SetActive(true);
			this.bookAnimator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
			this.pageAnimator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
			this.animatedBook.SetActive(false);
			if (PBook.OnBookClosed != null)
			{
				PBook.OnBookClosed(base.gameObject);
			}
			if (this.onBookClosed.GetPersistentEventCount() > 0)
			{
				this.onBookClosed.Invoke();
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002A7C File Offset: 0x00000C7C
		public void NextPage()
		{
			if (this.bookState == PBook.BookState.OPEN && this.currentPageIndex + 2 < this.pageCount && !this.isBlocked)
			{
				this.bookState = PBook.BookState.NEXTPAGE;
				if (!this.skipPageAnimation)
				{
					base.StartCoroutine(this.NextPageAnim());
					return;
				}
				this.NextPageSkipAnim(true);
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002AD0 File Offset: 0x00000CD0
		private void NextPageSkipAnim(bool playAudio)
		{
			if (this.playPageTurnSound && playAudio)
			{
				this.PlayPageTurnSound();
			}
			int num = this.currentPageIndex;
			if (this.pages[1].childCount > 0)
			{
				this.pages[1].GetChild(0).gameObject.SetActive(false);
				this.pages[1].GetChild(0).SetParent(this.contentContainer);
			}
			if (this.pages[2].childCount > 0)
			{
				this.pages[2].GetChild(0).gameObject.SetActive(false);
				this.pages[2].GetChild(0).SetParent(this.contentContainer);
			}
			for (int i = 1; i < 4; i++)
			{
				if (this.pageCount > num + i)
				{
					if (i > 1)
					{
						this.contentPages[num + i].gameObject.SetActive(false);
					}
					this.contentPages[num + i].SetParent(this.pages[i]);
					this.PrepareContentPage(this.contentPages[num + i]);
				}
			}
			if (this.pages[0].childCount > 0)
			{
				this.pages[0].GetChild(0).gameObject.SetActive(false);
				this.pages[0].GetChild(0).SetParent(this.contentContainer);
			}
			if (this.pageCount >= this.currentPageIndex + 2)
			{
				this.contentPages[this.currentPageIndex + 2].SetParent(this.pages[0]);
				this.PrepareContentPage(this.contentPages[this.currentPageIndex + 2]);
			}
			this.currentPageIndex += 2;
			if (!this.isBlocked)
			{
				this.bookState = PBook.BookState.OPEN;
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002F1C File Offset: 0x0000111C
		private IEnumerator NextPageAnim()
		{
			if (this.currentPageIndex + 4 >= this.pageCount)
			{
				if (PBook.OnBookTurnToLastPage != null)
				{
					PBook.OnBookTurnToLastPage(base.gameObject);
				}
				if (this.onBookTurnToLastPage.GetPersistentEventCount() > 0)
				{
					this.onBookTurnToLastPage.Invoke();
				}
			}
			if (this.playPageTurnSound)
			{
				this.PlayPageTurnSound();
			}
			this.animatedPage.SetActive(true);
			this.pageAnimator.Play("NextPage", 0, 0f);
			int tempPageIndex = this.currentPageIndex;
			if (this.pages[1].childCount > 0)
			{
				this.pages[1].GetChild(0).gameObject.SetActive(false);
				this.pages[1].GetChild(0).SetParent(this.contentContainer);
			}
			if (this.pages[2].childCount > 0)
			{
				this.pages[2].GetChild(0).gameObject.SetActive(false);
				this.pages[2].GetChild(0).SetParent(this.contentContainer);
			}
			for (int i = 1; i < 4; i++)
			{
				if (this.pageCount > tempPageIndex + i)
				{
					yield return null;
					if (i > 1)
					{
						this.contentPages[tempPageIndex + i].gameObject.SetActive(false);
					}
					this.contentPages[tempPageIndex + i].SetParent(this.pages[i]);
					this.PrepareContentPage(this.contentPages[tempPageIndex + i]);
				}
			}
			yield return true;
			yield break;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002F38 File Offset: 0x00001138
		public void NextPageEndAnimEvent()
		{
			if (this.mPrevTime == 0f)
			{
				if (this.pages[0].childCount > 0)
				{
					this.pages[0].GetChild(0).gameObject.SetActive(false);
					this.pages[0].GetChild(0).SetParent(this.contentContainer);
				}
				if (this.pageCount >= this.currentPageIndex + 2)
				{
					this.contentPages[this.currentPageIndex + 2].SetParent(this.pages[0]);
					this.PrepareContentPage(this.contentPages[this.currentPageIndex + 2]);
				}
				this.currentPageIndex += 2;
			}
			if (this.bookState == PBook.BookState.CANCEL_DRAG_NEXTPREV && this.mPrevTime != 0f)
			{
				if (this.pages[0].childCount > 0)
				{
					this.pages[0].GetChild(0).gameObject.SetActive(false);
					this.pages[0].GetChild(0).SetParent(this.contentContainer);
				}
				if (this.currentPageIndex >= 0)
				{
					this.contentPages[this.currentPageIndex].SetParent(this.pages[0]);
					this.PrepareContentPage(this.contentPages[this.currentPageIndex]);
				}
			}
			if (this.bookState == PBook.BookState.CANCEL_DRAG_NEXTPREV)
			{
				this.isBlocked = false;
			}
			this.mPrevTime = 0f;
			this.mNextTime = 0f;
			this.animatedPage.SetActive(false);
			this.bookState = PBook.BookState.OPEN;
			if (this.IsLastPage())
			{
				if (PBook.OnBookLastPage != null)
				{
					PBook.OnBookLastPage(base.gameObject);
				}
				if (this.onBookLastPage.GetPersistentEventCount() > 0)
				{
					this.onBookLastPage.Invoke();
				}
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000030EC File Offset: 0x000012EC
		public void PrevPage()
		{
			if (this.bookState == PBook.BookState.OPEN && this.currentPageIndex - 2 >= 0 && !this.isBlocked)
			{
				this.bookState = PBook.BookState.PREVPAGE;
				if (!this.skipPageAnimation)
				{
					base.StartCoroutine(this.PrevPageAnim());
					return;
				}
				this.PrevPageSkipAnim(true);
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000313C File Offset: 0x0000133C
		private void PrevPageSkipAnim(bool playAudio)
		{
			if (this.playPageTurnSound && playAudio)
			{
				this.PlayPageTurnSound();
			}
			int num = this.currentPageIndex + 1;
			if (this.pages[1].childCount > 0)
			{
				this.pages[1].GetChild(0).gameObject.SetActive(false);
				this.pages[1].GetChild(0).SetParent(this.contentContainer);
			}
			if (this.pages[2].childCount > 0)
			{
				this.pages[2].GetChild(0).gameObject.SetActive(false);
				this.pages[2].GetChild(0).SetParent(this.contentContainer);
			}
			for (int i = 1; i < 4; i++)
			{
				if (num - i >= 0)
				{
					if (i > 1)
					{
						this.contentPages[num - i].gameObject.SetActive(false);
					}
					this.contentPages[num - i].SetParent(this.pages[3 - i]);
					this.PrepareContentPage(this.contentPages[num - i]);
				}
			}
			if (this.pages[3].childCount > 0)
			{
				this.pages[3].GetChild(0).gameObject.SetActive(false);
				this.pages[3].GetChild(0).SetParent(this.contentContainer);
			}
			if (this.currentPageIndex - 1 >= 0)
			{
				this.contentPages[this.currentPageIndex - 1].SetParent(this.pages[3]);
				this.PrepareContentPage(this.contentPages[this.currentPageIndex - 1]);
			}
			this.currentPageIndex -= 2;
			if (!this.isBlocked)
			{
				this.bookState = PBook.BookState.OPEN;
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00003574 File Offset: 0x00001774
		private IEnumerator PrevPageAnim()
		{
			if (this.currentPageIndex - 4 < 0)
			{
				if (PBook.OnBookTurnToFirstPage != null)
				{
					PBook.OnBookTurnToFirstPage(base.gameObject);
				}
				if (this.onBookTurnToFirstPage.GetPersistentEventCount() > 0)
				{
					this.onBookTurnToFirstPage.Invoke();
				}
			}
			if (this.playPageTurnSound)
			{
				this.PlayPageTurnSound();
			}
			this.animatedPage.SetActive(true);
			this.pageAnimator.Play("PrevPage", 0, 0f);
			int tempPageIndex = this.currentPageIndex + 1;
			if (this.pages[1].childCount > 0)
			{
				this.pages[1].GetChild(0).gameObject.SetActive(false);
				this.pages[1].GetChild(0).SetParent(this.contentContainer);
			}
			if (this.pages[2].childCount > 0)
			{
				this.pages[2].GetChild(0).gameObject.SetActive(false);
				this.pages[2].GetChild(0).SetParent(this.contentContainer);
			}
			for (int i = 1; i < 4; i++)
			{
				if (tempPageIndex - i >= 0)
				{
					yield return null;
					if (i > 1)
					{
						this.contentPages[tempPageIndex - i].gameObject.SetActive(false);
					}
					this.contentPages[tempPageIndex - i].SetParent(this.pages[3 - i]);
					this.PrepareContentPage(this.contentPages[tempPageIndex - i]);
				}
			}
			yield return true;
			yield break;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00003590 File Offset: 0x00001790
		public void PrevPageEndAnimEvent()
		{
			if (this.mNextTime == 0f)
			{
				if (this.pages[3].childCount > 0)
				{
					this.pages[3].GetChild(0).gameObject.SetActive(false);
					this.pages[3].GetChild(0).SetParent(this.contentContainer);
				}
				if (this.currentPageIndex - 1 >= 0)
				{
					this.contentPages[this.currentPageIndex - 1].SetParent(this.pages[3]);
					this.PrepareContentPage(this.contentPages[this.currentPageIndex - 1]);
				}
				this.currentPageIndex -= 2;
			}
			if (this.bookState == PBook.BookState.CANCEL_DRAG_NEXTPREV && this.mNextTime != 0f)
			{
				if (this.pages[3].childCount > 0)
				{
					this.pages[3].GetChild(0).gameObject.SetActive(false);
					this.pages[3].GetChild(0).SetParent(this.contentContainer);
				}
				if (this.currentPageIndex >= 0)
				{
					this.contentPages[this.currentPageIndex + 1].SetParent(this.pages[3]);
					this.PrepareContentPage(this.contentPages[this.currentPageIndex + 1]);
				}
			}
			if (this.bookState == PBook.BookState.CANCEL_DRAG_NEXTPREV)
			{
				this.isBlocked = false;
			}
			this.mNextTime = 0f;
			this.mPrevTime = 0f;
			this.animatedPage.SetActive(false);
			this.bookState = PBook.BookState.OPEN;
			if (this.IsFirstPage())
			{
				if (PBook.OnBookFirstPage != null)
				{
					PBook.OnBookFirstPage(base.gameObject);
				}
				if (this.onBookFirstPage.GetPersistentEventCount() > 0)
				{
					this.onBookFirstPage.Invoke();
				}
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003744 File Offset: 0x00001944
		public void DragOpenBook(float animTime)
		{
			if (this.bookState == PBook.BookState.CLOSED && !this.isBlocked)
			{
				this.bookState = PBook.BookState.DRAG_OPENBOOK;
				if (this.pageCount > this.currentPageIndex)
				{
					this.contentPages[this.currentPageIndex].SetParent(this.pages[0]);
					this.PrepareContentPage(this.contentPages[this.currentPageIndex]);
				}
				if (this.pageCount > this.currentPageIndex + 1)
				{
					this.contentPages[this.currentPageIndex + 1].SetParent(this.pages[3]);
					this.PrepareContentPage(this.contentPages[this.currentPageIndex + 1]);
				}
				this.book.SetActive(false);
				this.animatedBook.SetActive(true);
				this.bookAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
				this.pageAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
				this.bookAnimator.speed = 0f;
			}
			if (this.bookState == PBook.BookState.DRAG_OPENBOOK)
			{
				this.mOpenCloseTime = animTime;
				this.bookAnimator.Play("OpenBook", 0, this.mOpenCloseTime);
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003854 File Offset: 0x00001A54
		public void DragCloseBook(float animTime)
		{
			if (this.bookState == PBook.BookState.OPEN && !this.isBlocked)
			{
				this.bookState = PBook.BookState.DRAG_CLOSEBOOK;
				this.bookAnimator.speed = 0f;
			}
			if (this.bookState == PBook.BookState.DRAG_CLOSEBOOK)
			{
				this.mOpenCloseTime = animTime;
				this.bookAnimator.Play("OpenBook", 0, 1f - this.mOpenCloseTime);
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000038B8 File Offset: 0x00001AB8
		public void CancelDragOpenCloseBook()
		{
			if (this.bookState == PBook.BookState.DRAG_OPENBOOK || this.bookState == PBook.BookState.DRAG_CLOSEBOOK)
			{
				this.isBlocked = true;
				this.bookAnimator.speed = this.openCloseSpeed;
				this.mOpenCloseTime = Mathf.Clamp(this.mOpenCloseTime, 0.02f, 0.98f);
				if (this.bookState == PBook.BookState.DRAG_OPENBOOK)
				{
					this.bookState = PBook.BookState.CANCEL_DRAG_OPENCLOSE;
					if (this.mOpenCloseTime <= 0.5f)
					{
						this.bookAnimator.Play("CloseBook", 0, 1f - this.mOpenCloseTime);
					}
					else
					{
						this.bookAnimator.Play("OpenBook", 0, this.mOpenCloseTime);
					}
					this.mOpenCloseTime = 0f;
				}
				if (this.bookState == PBook.BookState.DRAG_CLOSEBOOK)
				{
					this.bookState = PBook.BookState.CANCEL_DRAG_OPENCLOSE;
					if (this.mOpenCloseTime <= 0.5f)
					{
						this.bookAnimator.Play("OpenBook", 0, 1f - this.mOpenCloseTime);
					}
					else
					{
						this.bookAnimator.Play("CloseBook", 0, this.mOpenCloseTime);
					}
					this.mOpenCloseTime = 0f;
				}
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000039CD File Offset: 0x00001BCD
		public void DragNextPage(float animTime)
		{
			if (this.currentPageIndex + 2 < this.pageCount && !this.isBlocked)
			{
				this.mNextTime = animTime;
				this.DragNextPageAnim(this.mNextTime);
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000039FC File Offset: 0x00001BFC
		private void DragNextPageAnim(float animTime)
		{
			if (this.bookState == PBook.BookState.OPEN && this.currentPageIndex + 2 < this.pageCount && !this.isBlocked && !this.animatedPage.activeSelf)
			{
				base.StartCoroutine(this.DragNextPageAnimPrepare());
			}
			if (this.bookState == PBook.BookState.DRAG_NEXTPAGE && !this.isBlocked)
			{
				this.pageAnimator.Play("NextPage", 0, animTime);
				if (animTime < 0.05f)
				{
					if (this.pages[3].childCount > 0)
					{
						this.pages[3].GetChild(0).gameObject.SetActive(false);
					}
				}
				else if (this.pages[3].childCount > 0)
				{
					this.pages[3].GetChild(0).gameObject.SetActive(true);
				}
				if (animTime > 0.92f)
				{
					if (this.pages[0].childCount > 0)
					{
						this.pages[0].GetChild(0).gameObject.SetActive(false);
						return;
					}
				}
				else if (this.pages[0].childCount > 0)
				{
					this.pages[0].GetChild(0).gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003DE4 File Offset: 0x00001FE4
		private IEnumerator DragNextPageAnimPrepare()
		{
			this.pageAnimator.speed = 0.5f;
			this.animatedPage.SetActive(true);
			this.pageAnimator.Play("NextPage", 0, 0f);
			int tempPageIndex = this.currentPageIndex;
			if (this.pages[1].childCount > 0)
			{
				this.pages[1].GetChild(0).gameObject.SetActive(false);
				this.pages[1].GetChild(0).SetParent(this.contentContainer);
			}
			if (this.pages[2].childCount > 0)
			{
				this.pages[2].GetChild(0).gameObject.SetActive(false);
				this.pages[2].GetChild(0).SetParent(this.contentContainer);
			}
			for (int i = 1; i < 4; i++)
			{
				if (this.pageCount > tempPageIndex + i)
				{
					yield return null;
					if (i > 1)
					{
						this.contentPages[tempPageIndex + i].gameObject.SetActive(false);
					}
					this.contentPages[tempPageIndex + i].SetParent(this.pages[i]);
					this.PrepareContentPage(this.contentPages[tempPageIndex + i]);
					if (this.pages[3].childCount > 0)
					{
						this.pages[3].GetChild(0).gameObject.SetActive(false);
					}
				}
			}
			yield return null;
			this.pageAnimator.speed = 0f;
			this.bookState = PBook.BookState.DRAG_NEXTPAGE;
			yield return true;
			yield break;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003E00 File Offset: 0x00002000
		public void DragPrevPage(float animTime)
		{
			if (this.currentPageIndex - 2 >= 0 && !this.isBlocked)
			{
				this.mPrevTime = animTime;
				this.DragPrevPageAnim(this.mPrevTime);
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003E28 File Offset: 0x00002028
		private void DragPrevPageAnim(float animTime)
		{
			if (this.bookState == PBook.BookState.OPEN && this.currentPageIndex - 2 >= 0 && !this.isBlocked && !this.animatedPage.activeSelf)
			{
				base.StartCoroutine(this.DragPrevPageAnimPrepare());
			}
			if (this.bookState == PBook.BookState.DRAG_PREVPAGE && !this.isBlocked)
			{
				this.pageAnimator.Play("PrevPage", 0, animTime);
				if (animTime < 0.05f)
				{
					if (this.pages[0].childCount > 0)
					{
						this.pages[0].GetChild(0).gameObject.SetActive(false);
					}
				}
				else if (this.pages[0].childCount > 0)
				{
					this.pages[0].GetChild(0).gameObject.SetActive(true);
				}
				if (animTime > 0.92f)
				{
					if (this.pages[3].childCount > 0)
					{
						this.pages[3].GetChild(0).gameObject.SetActive(false);
						return;
					}
				}
				else if (this.pages[3].childCount > 0)
				{
					this.pages[3].GetChild(0).gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000420C File Offset: 0x0000240C
		private IEnumerator DragPrevPageAnimPrepare()
		{
			this.pageAnimator.speed = 0.5f;
			this.animatedPage.SetActive(true);
			this.pageAnimator.Play("PrevPage", 0, 0f);
			int tempPageIndex = this.currentPageIndex + 1;
			if (this.pages[1].childCount > 0)
			{
				this.pages[1].GetChild(0).gameObject.SetActive(false);
				this.pages[1].GetChild(0).SetParent(this.contentContainer);
			}
			if (this.pages[2].childCount > 0)
			{
				this.pages[2].GetChild(0).gameObject.SetActive(false);
				this.pages[2].GetChild(0).SetParent(this.contentContainer);
			}
			for (int i = 1; i < 4; i++)
			{
				if (tempPageIndex - i >= 0)
				{
					yield return null;
					if (i > 1)
					{
						this.contentPages[tempPageIndex - i].gameObject.SetActive(false);
					}
					this.contentPages[tempPageIndex - i].SetParent(this.pages[3 - i]);
					this.PrepareContentPage(this.contentPages[tempPageIndex - i]);
					if (this.pages[0].childCount > 0)
					{
						this.pages[0].GetChild(0).gameObject.SetActive(false);
					}
				}
			}
			yield return true;
			this.pageAnimator.speed = 0f;
			this.bookState = PBook.BookState.DRAG_PREVPAGE;
			yield return true;
			yield break;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00004228 File Offset: 0x00002428
		public void CancelDragNextPrevPage()
		{
			if (this.bookState == PBook.BookState.DRAG_NEXTPAGE || this.bookState == PBook.BookState.DRAG_PREVPAGE)
			{
				this.isBlocked = true;
				this.pageAnimator.speed = this.pageTurnSpeed;
				if (this.bookState == PBook.BookState.DRAG_NEXTPAGE)
				{
					this.bookState = PBook.BookState.CANCEL_DRAG_NEXTPREV;
					this.mNextTime = Mathf.Clamp(this.mNextTime, 0.03f, 0.97f);
					if (this.mNextTime <= 0.5f)
					{
						this.pageAnimator.Play("PrevPage", 0, 1f - this.mNextTime);
					}
					else
					{
						this.pageAnimator.Play("NextPage", 0, this.mNextTime);
					}
				}
				if (this.bookState == PBook.BookState.DRAG_PREVPAGE)
				{
					this.bookState = PBook.BookState.CANCEL_DRAG_NEXTPREV;
					this.mPrevTime = Mathf.Clamp(this.mPrevTime, 0.03f, 0.97f);
					if (this.mPrevTime <= 0.5f)
					{
						this.pageAnimator.Play("NextPage", 0, 1f - this.mPrevTime);
						return;
					}
					this.pageAnimator.Play("PrevPage", 0, this.mPrevTime);
				}
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00004340 File Offset: 0x00002540
		public void GoToPage(int pageNumber, float speed)
		{
			if (this.bookState == PBook.BookState.OPEN && pageNumber <= this.pageCount && pageNumber > 0 && !this.isBlocked)
			{
				this.isBlocked = true;
				float speed2 = Mathf.Clamp(speed, 1f, 50f);
				if (pageNumber % 2 == 0)
				{
					pageNumber--;
				}
				int num = pageNumber - (this.currentPageIndex + 1);
				num /= 2;
				base.StartCoroutine(this.GoToPageAnim(num, speed2));
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00004698 File Offset: 0x00002898
		private IEnumerator GoToPageAnim(int times, float speed)
		{
			float savedAnimatorSpeed = this.pageAnimator.speed;
			this.pageAnimator.speed = speed + 0.6f;
			if (times > 0)
			{
				for (int i = 0; i < Mathf.Abs(times); i++)
				{
					this.bookState = PBook.BookState.NEXTPAGE;
					base.StartCoroutine(this.NextPageAnim());
					yield return new WaitUntil(() => this.bookState == PBook.BookState.OPEN);
				}
			}
			else if (times < 0)
			{
				for (int j = 0; j < Mathf.Abs(times); j++)
				{
					this.bookState = PBook.BookState.PREVPAGE;
					base.StartCoroutine(this.PrevPageAnim());
					yield return new WaitUntil(() => this.bookState == PBook.BookState.OPEN);
				}
			}
			this.pageAnimator.speed = savedAnimatorSpeed;
			this.isBlocked = false;
			if (this.pageCount > this.currentPageIndex)
			{
				this.contentPages[this.currentPageIndex].SetParent(this.pages[0]);
				this.PrepareContentPage(this.contentPages[this.currentPageIndex]);
			}
			if (this.pageCount > this.currentPageIndex + 1)
			{
				this.contentPages[this.currentPageIndex + 1].SetParent(this.pages[3]);
				this.PrepareContentPage(this.contentPages[this.currentPageIndex + 1]);
			}
			this.bookState = PBook.BookState.OPEN;
			yield break;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000046C2 File Offset: 0x000028C2
		public void GoToFirstPage(float speed)
		{
			this.GoToPage(1, speed);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000046CC File Offset: 0x000028CC
		public void GoToLastPage(float speed)
		{
			this.GoToPage(this.pageCount, speed);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000046DC File Offset: 0x000028DC
		public void JumpToPage(int pageNumber, bool playSound)
		{
			if ((this.bookState == PBook.BookState.OPEN || this.bookState == PBook.BookState.CLOSED) && pageNumber <= this.pageCount && pageNumber > 0 && !this.isBlocked)
			{
				PBook.BookState bookState = this.bookState;
				this.isBlocked = true;
				if (pageNumber % 2 == 0)
				{
					pageNumber--;
				}
				int num = pageNumber - (this.currentPageIndex + 1);
				num /= 2;
				if (num > 0)
				{
					if (this.playPageTurnSound && playSound)
					{
						this.PlayPageTurnSound();
					}
					for (int i = 0; i < Mathf.Abs(num); i++)
					{
						this.bookState = PBook.BookState.NEXTPAGE;
						this.NextPageSkipAnim(false);
					}
				}
				else if (num < 0)
				{
					if (this.playPageTurnSound && playSound)
					{
						this.PlayPageTurnSound();
					}
					for (int j = 0; j < Mathf.Abs(num); j++)
					{
						this.bookState = PBook.BookState.PREVPAGE;
						this.PrevPageSkipAnim(false);
					}
				}
				this.isBlocked = false;
				if (this.pageCount > this.currentPageIndex)
				{
					this.contentPages[this.currentPageIndex].SetParent(this.pages[0]);
					this.PrepareContentPage(this.contentPages[this.currentPageIndex]);
				}
				if (this.pageCount > this.currentPageIndex + 1)
				{
					this.contentPages[this.currentPageIndex + 1].SetParent(this.pages[3]);
					this.PrepareContentPage(this.contentPages[this.currentPageIndex + 1]);
				}
				this.bookState = bookState;
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00004836 File Offset: 0x00002A36
		public void JumpToFirstPage(bool playSound)
		{
			this.JumpToPage(1, playSound);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00004840 File Offset: 0x00002A40
		public void JumpToLastPage(bool playSound)
		{
			this.JumpToPage(this.pageCount, playSound);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00004850 File Offset: 0x00002A50
		public void SetOpenCloseSpeed(float speed)
		{
			float speed2 = Mathf.Clamp(speed, 1f, 5f);
			this.bookAnimator.speed = speed2;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x0000487C File Offset: 0x00002A7C
		public void SetPageTurnSpeed(float speed)
		{
			float num = Mathf.Clamp(speed, 1f, 5f);
			this.pageAnimator.speed = num + 0.6f;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000048AC File Offset: 0x00002AAC
		public void SetSkipPageAnimation(bool skipAnimation)
		{
			this.skipPageAnimation = skipAnimation;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000048B5 File Offset: 0x00002AB5
		public void SetOpenCloseSoundVolume(float v)
		{
			this.openCloseSoundVolume = v;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000048BE File Offset: 0x00002ABE
		public void SetPageTurnSoundVolume(float v)
		{
			this.pageTurnSoundVolume = v;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000048C7 File Offset: 0x00002AC7
		public int GetPageCount()
		{
			return this.pageCount;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000048CF File Offset: 0x00002ACF
		public int GetCurrentPageIndex()
		{
			return this.currentPageIndex + 1;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000048D9 File Offset: 0x00002AD9
		public bool IsFirstPage()
		{
			return this.currentPageIndex == 0;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000048E6 File Offset: 0x00002AE6
		public bool IsLastPage()
		{
			return this.currentPageIndex + 2 >= this.pageCount;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000048FB File Offset: 0x00002AFB
		public PBook.BookState GetBookState()
		{
			return this.bookState;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00004903 File Offset: 0x00002B03
		public AudioSource GetAudioSource()
		{
			return this.audioSource;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0000490B File Offset: 0x00002B0B
		public Animator GetBookAnimator()
		{
			return this.bookAnimator;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00004913 File Offset: 0x00002B13
		public Animator GetPageAnimator()
		{
			return this.pageAnimator;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000491C File Offset: 0x00002B1C
		public void ChangeContent(Transform container)
		{
			if (this.bookState == PBook.BookState.CLOSED)
			{
				foreach (RectTransform rectTransform in this.contentPages)
				{
					rectTransform.localScale = Vector3.zero;
					rectTransform.gameObject.SetActive(false);
					rectTransform.SetParent(null);
				}
				foreach (RectTransform rectTransform2 in this.contentPages)
				{
					rectTransform2.SetParent(this.contentContainer);
				}
				this.contentContainer = container;
				this.contentPages = PBook.GetComponentsInDirectChildren<RectTransform>(this.contentContainer);
				this.pageCount = this.contentPages.Length;
				this.currentPageIndex = 0;
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000049C8 File Offset: 0x00002BC8
		private void PrepareContentPage(RectTransform cp)
		{
			cp.offsetMin = Vector2.zero;
			cp.offsetMax = Vector2.zero;
			cp.localPosition = Vector3.zero;
			cp.localRotation = Quaternion.identity;
			cp.localScale = Vector3.one;
			cp.gameObject.SetActive(true);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00004A18 File Offset: 0x00002C18
		private static T[] GetComponentsInDirectChildren<T>(Transform t)
		{
			int num = 0;
			foreach (object obj in t)
			{
				Transform transform = (Transform)obj;
				if (transform.GetComponent<T>() != null)
				{
					num++;
				}
			}
			T[] array = new T[num];
			num = 0;
			foreach (object obj2 in t)
			{
				Transform transform2 = (Transform)obj2;
				if (transform2.GetComponent<T>() != null)
				{
					array[num++] = transform2.GetComponent<T>();
				}
			}
			return array;
		}

		// Token: 0x04000001 RID: 1
		private Transform tr;

		// Token: 0x04000002 RID: 2
		private PBook.BookState bookState;

		// Token: 0x04000003 RID: 3
		private GameObject book;

		// Token: 0x04000004 RID: 4
		private GameObject animatedBook;

		// Token: 0x04000005 RID: 5
		private GameObject animatedPage;

		// Token: 0x04000006 RID: 6
		private Animator bookAnimator;

		// Token: 0x04000007 RID: 7
		private Animator pageAnimator;

		// Token: 0x04000008 RID: 8
		private SkinnedMeshRenderer animatedBookRenderer;

		// Token: 0x04000009 RID: 9
		private AudioSource audioSource;

		// Token: 0x0400000A RID: 10
		private int currentPageIndex;

		// Token: 0x0400000B RID: 11
		private int pageCount;

		// Token: 0x0400000C RID: 12
		private Transform[] pages;

		// Token: 0x0400000D RID: 13
		private RectTransform[] contentPages;

		// Token: 0x0400000E RID: 14
		private bool isBlocked;

		// Token: 0x0400000F RID: 15
		private float mNextTime;

		// Token: 0x04000010 RID: 16
		private float mPrevTime;

		// Token: 0x04000011 RID: 17
		private float mOpenCloseTime;

		// Token: 0x04000012 RID: 18
		public Transform contentContainer;

		// Token: 0x04000013 RID: 19
		public int startAtPageNumber = 1;

		// Token: 0x04000014 RID: 20
		[Range(1f, 5f)]
		public float openCloseSpeed;

		// Token: 0x04000015 RID: 21
		[Range(1f, 5f)]
		public float pageTurnSpeed;

		// Token: 0x04000016 RID: 22
		public bool skipPageAnimation;

		// Token: 0x04000017 RID: 23
		public bool playOpenBookSound = true;

		// Token: 0x04000018 RID: 24
		public bool playCloseBookSound = true;

		// Token: 0x04000019 RID: 25
		public bool playPageTurnSound = true;

		// Token: 0x0400001A RID: 26
		public bool autoPitchOpenCloseSounds = true;

		// Token: 0x0400001B RID: 27
		public bool autoPitchPageTurnSounds = true;

		// Token: 0x0400001C RID: 28
		[Range(0f, 1f)]
		public float openCloseSoundVolume = 1f;

		// Token: 0x0400001D RID: 29
		[Range(0f, 1f)]
		public float pageTurnSoundVolume = 1f;

		// Token: 0x0400001E RID: 30
		public AudioClip[] openBookSounds;

		// Token: 0x0400001F RID: 31
		public AudioClip[] closeBookSounds;

		// Token: 0x04000020 RID: 32
		public AudioClip[] pageTurnSounds;

		// Token: 0x04000021 RID: 33
		public bool firstLastPageEventsOnBookOpen;

		// Token: 0x0400002A RID: 42
		[SerializeField]
		public BookOpenedEvent onBookOpened;

		// Token: 0x0400002B RID: 43
		[SerializeField]
		public BookWillOpenEvent onBookWillOpen;

		// Token: 0x0400002C RID: 44
		[SerializeField]
		public BookClosedEvent onBookClosed;

		// Token: 0x0400002D RID: 45
		[SerializeField]
		public BookWillCloseEvent onBookWillClose;

		// Token: 0x0400002E RID: 46
		[SerializeField]
		public BookFirstPageEvent onBookFirstPage;

		// Token: 0x0400002F RID: 47
		[SerializeField]
		public BookTurnToFirstPageEvent onBookTurnToFirstPage;

		// Token: 0x04000030 RID: 48
		[SerializeField]
		public BookLastPageEvent onBookLastPage;

		// Token: 0x04000031 RID: 49
		[SerializeField]
		public BookTurnToLastPageEvent onBookTurnToLastPage;

		// Token: 0x0200000D RID: 13
		[HideInInspector]
		public enum BookState
		{
			// Token: 0x04000033 RID: 51
			CLOSED,
			// Token: 0x04000034 RID: 52
			OPENBOOK,
			// Token: 0x04000035 RID: 53
			OPEN,
			// Token: 0x04000036 RID: 54
			CLOSEBOOK,
			// Token: 0x04000037 RID: 55
			NEXTPAGE,
			// Token: 0x04000038 RID: 56
			PREVPAGE,
			// Token: 0x04000039 RID: 57
			DRAG_NEXTPAGE,
			// Token: 0x0400003A RID: 58
			DRAG_PREVPAGE,
			// Token: 0x0400003B RID: 59
			DRAG_OPENBOOK,
			// Token: 0x0400003C RID: 60
			DRAG_CLOSEBOOK,
			// Token: 0x0400003D RID: 61
			CANCEL_DRAG_NEXTPREV,
			// Token: 0x0400003E RID: 62
			CANCEL_DRAG_OPENCLOSE
		}

		// Token: 0x0200000E RID: 14
		// (Invoke) Token: 0x06000050 RID: 80
		public delegate void BooKOpenedAction(GameObject sender);

		// Token: 0x0200000F RID: 15
		// (Invoke) Token: 0x06000054 RID: 84
		public delegate void BooKClosedAction(GameObject sender);

		// Token: 0x02000010 RID: 16
		// (Invoke) Token: 0x06000058 RID: 88
		public delegate void BooKWillOpenAction(GameObject sender);

		// Token: 0x02000011 RID: 17
		// (Invoke) Token: 0x0600005C RID: 92
		public delegate void BooKWillCloseAction(GameObject sender);

		// Token: 0x02000012 RID: 18
		// (Invoke) Token: 0x06000060 RID: 96
		public delegate void BookFirstPageAction(GameObject sender);

		// Token: 0x02000013 RID: 19
		// (Invoke) Token: 0x06000064 RID: 100
		public delegate void BookLastPageAction(GameObject sender);

		// Token: 0x02000014 RID: 20
		// (Invoke) Token: 0x06000068 RID: 104
		public delegate void BookEnterFirstPageAction(GameObject sender);

		// Token: 0x02000015 RID: 21
		// (Invoke) Token: 0x0600006C RID: 108
		public delegate void BookEnterLastPageAction(GameObject sender);
	}
}

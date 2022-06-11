using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TLGFPowerBooks
{
	// Token: 0x02000017 RID: 23
	public class SimpleBookCreator : MonoBehaviour
	{
		// Token: 0x06000075 RID: 117 RVA: 0x00004CF0 File Offset: 0x00002EF0
		private void Awake()
		{
			this.tr = base.GetComponent<Transform>();
			this.book = this.tr.Find("Book").gameObject;
			this.animatedBook = this.tr.Find("AnimatedBook").gameObject;
			this.animatedPage = this.animatedBook.transform.Find("AnimatedPage").gameObject;
			this.bookAnimator = this.animatedBook.GetComponent<Animator>();
			this.pageAnimator = this.animatedPage.GetComponent<Animator>();
			this.leftPagePrefab = this.tr.Find("SimpleTextBookLeftPage").gameObject;
			this.rightPagePrefab = this.tr.Find("SimpleTextBookRightPage").gameObject;
			this.pages = new Transform[4];
			this.pages[0] = this.animatedBook.transform.Find("PageCenterLeft/PageLeftB");
			this.pages[1] = this.animatedPage.transform.Find("PageCenter/PageRightP");
			this.pages[2] = this.animatedPage.transform.Find("PageCenter/PageLeftP");
			this.pages[3] = this.animatedBook.transform.Find("PageCenterRight/PageRightB");
			this.simpleContentPagesContainer = (RectTransform)this.tr.Find("SimpleTextBookContent");
			this.simpleContentPages = new List<RectTransform>();
			this.simpleContentPagesText = new List<string>();
			this.firstPage = Instantiate<GameObject>(this.leftPagePrefab, this.pages[0]);
			this.simpleBookText = this.firstPage.transform.Find("Text").GetComponentInChildren<Text>();
			this.simpleBookText.rectTransform.offsetMin = new Vector2((float)this.textMargin, (float)this.textMargin);
			this.simpleBookText.rectTransform.offsetMax = new Vector2((float)(Mathf.Max(0, this.textMargin - 100) * -1), (float)(-(float)this.textMargin));
			this.simpleBookText.font = this.textFont;
			this.simpleBookText.fontSize = this.textSize;
			this.simpleBookText.color = this.textColor;
			this.simpleBookText.fontStyle = FontStyle.Normal;
			this.simpleBookText.lineSpacing = 1f;
			this.bookAnimator.speed = 2.6f;
			this.pageAnimator.speed = 2.6f;
			this.bookState = SimpleBookCreator.BookState.PREPARE;
			this.convertState = SimpleBookCreator.ConvertState.NONE;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00004F70 File Offset: 0x00003170
		private void Start()
		{
			if (!this.convertMultipleFiles)
			{
				base.StartCoroutine(this.PrepareBook());
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000057B4 File Offset: 0x000039B4
		private IEnumerator PrepareBook()
		{
			string fileResult = "";
			fileResult = this.textFile.text;
			List<string> splittedStrings = new List<string>();
			while (fileResult.Length > 0)
			{
				string text = fileResult.Substring(0, Mathf.Min(12000, fileResult.Length));
				splittedStrings.Add(text);
				if (text == fileResult)
				{
					break;
				}
				fileResult = fileResult.Substring(text.Length, fileResult.Length - Mathf.Min(text.Length, fileResult.Length));
			}
			yield return new WaitForEndOfFrame();
			int maxCharCount = 0;
			for (int i = 0; i < splittedStrings.Count; i++)
			{
				this.percentComplete = 100 / splittedStrings.Count * (i + 1);
				this.simpleBookText.text = splittedStrings[i];
				TextGenerator t = this.simpleBookText.cachedTextGenerator;
				while (this.simpleBookText.text.Length > 0)
				{
					yield return new WaitForEndOfFrame();
					maxCharCount = t.characterCountVisible;
					string result = this.simpleBookText.text.Substring(0, Mathf.Min(t.characterCountVisible, this.simpleBookText.text.Length));
					if (result == this.simpleBookText.text && result.Length <= maxCharCount && i < splittedStrings.Count - 1)
					{
						splittedStrings[i + 1] = result + splittedStrings[i + 1];
					}
					else
					{
						this.simpleContentPagesText.Add(result);
					}
					if (result == this.simpleBookText.text)
					{
						break;
					}
					this.simpleBookText.text = this.simpleBookText.text.Substring(result.Length, this.simpleBookText.text.Length - Mathf.Min(result.Length, this.simpleBookText.text.Length));
				}
			}
			Destroy(this.firstPage);
			int switchLeftRight = 0;
			foreach (string text2 in this.simpleContentPagesText)
			{
				string text3 = text2;
				if (text3.StartsWith(" ") || text3.StartsWith("\n"))
				{
					text3 = text3.Substring(1);
				}
				Transform transform;
				if (switchLeftRight % 2 == 0)
				{
					transform = Instantiate<GameObject>(this.leftPagePrefab, this.simpleContentPagesContainer).transform;
				}
				else
				{
					transform = Instantiate<GameObject>(this.rightPagePrefab, this.simpleContentPagesContainer).transform;
				}
				Text componentInChildren = transform.Find("Text").GetComponentInChildren<Text>();
				componentInChildren.text = text3;
				componentInChildren.font = this.textFont;
				componentInChildren.fontSize = this.textSize;
				componentInChildren.color = this.textColor;
				componentInChildren.fontStyle = FontStyle.Normal;
				componentInChildren.lineSpacing = 1f;
				Text componentInChildren2 = transform.Find("PageNumber").GetComponentInChildren<Text>();
				if (switchLeftRight % 2 == 0)
				{
					componentInChildren.rectTransform.offsetMin = new Vector2((float)this.textMargin, (float)this.textMargin);
					componentInChildren.rectTransform.offsetMax = new Vector2((float)(Mathf.Max(0, this.textMargin - 100) * -1), (float)(-(float)this.textMargin));
					componentInChildren2.rectTransform.offsetMin = new Vector2((float)this.textMargin, componentInChildren2.rectTransform.offsetMin.y);
					componentInChildren2.rectTransform.offsetMax = new Vector2((float)(Mathf.Max(0, this.textMargin - 100) * -1), componentInChildren2.rectTransform.offsetMax.y);
				}
				else
				{
					componentInChildren.rectTransform.offsetMin = new Vector2((float)Mathf.Max(0, this.textMargin - 100), (float)this.textMargin);
					componentInChildren.rectTransform.offsetMax = new Vector2((float)(-(float)this.textMargin), (float)(-(float)this.textMargin));
					componentInChildren2.rectTransform.offsetMin = new Vector2((float)Mathf.Max(0, this.textMargin - 100), componentInChildren2.rectTransform.offsetMin.y);
					componentInChildren2.rectTransform.offsetMax = new Vector2((float)(-(float)this.textMargin), componentInChildren2.rectTransform.offsetMax.y);
				}
				if (!this.showPageNumbers)
				{
					componentInChildren2.gameObject.SetActive(false);
				}
				else
				{
					componentInChildren2.text = (switchLeftRight + 1).ToString();
					componentInChildren2.font = this.textFont;
					componentInChildren2.fontSize = this.textSize;
					componentInChildren2.color = this.textColor;
				}
				if (this.centerPageNumbers)
				{
					componentInChildren2.alignment = TextAnchor.LowerCenter;
				}
				transform.gameObject.SetActive(false);
				this.simpleContentPages.Add((RectTransform)transform);
				switchLeftRight++;
			}
			Destroy(this.leftPagePrefab);
			Destroy(this.rightPagePrefab);
			this.pageCount = this.simpleContentPages.Count;
			if (this.pageCount > 0)
			{
				this.simpleContentPages[0].SetParent(this.pages[0]);
				this.PrepareContentPage(this.simpleContentPages[0]);
			}
			if (this.pageCount > 1)
			{
				this.simpleContentPages[1].SetParent(this.pages[3]);
				this.PrepareContentPage(this.simpleContentPages[1]);
			}
			this.animatedBook.SetActive(false);
			this.simpleContentPagesContainer.gameObject.SetActive(false);
			this.bookState = SimpleBookCreator.BookState.CLOSED;
			this.OpenBook();
			this.bookCreated = true;
			yield break;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000057D0 File Offset: 0x000039D0
		public Animator GetBookAnimator()
		{
			return this.bookAnimator;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000057D8 File Offset: 0x000039D8
		public Animator GetPageAnimator()
		{
			return this.pageAnimator;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000057E0 File Offset: 0x000039E0
		public SimpleBookCreator.BookState GetBookState()
		{
			return this.bookState;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000057E8 File Offset: 0x000039E8
		public int GetPercentComplete()
		{
			return this.percentComplete;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000057F0 File Offset: 0x000039F0
		public void OpenBook()
		{
			if (this.bookState == SimpleBookCreator.BookState.CLOSED)
			{
				this.bookState = SimpleBookCreator.BookState.OPENBOOK;
				this.book.SetActive(false);
				this.animatedBook.SetActive(true);
				this.bookAnimator.Play("OpenBook");
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0000582A File Offset: 0x00003A2A
		public void BookOpenedEvent()
		{
			this.bookState = SimpleBookCreator.BookState.OPEN;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00005833 File Offset: 0x00003A33
		public void CloseBook()
		{
			if (this.bookState == SimpleBookCreator.BookState.OPEN)
			{
				this.bookState = SimpleBookCreator.BookState.CLOSEBOOK;
				this.bookAnimator.Play("CloseBook");
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00005855 File Offset: 0x00003A55
		public void BookClosedEvent()
		{
			this.bookState = SimpleBookCreator.BookState.CLOSED;
			this.book.SetActive(true);
			this.animatedBook.SetActive(false);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00005878 File Offset: 0x00003A78
		public void NextPage()
		{
			if (this.bookState == SimpleBookCreator.BookState.OPEN && this.currentPageIndex + 2 < this.pageCount)
			{
				this.bookState = SimpleBookCreator.BookState.NEXTPAGE;
				this.animatedPage.SetActive(true);
				this.pageAnimator.Play("NextPage", 0, 0f);
				base.StartCoroutine(this.NextPageAnim());
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00005AD0 File Offset: 0x00003CD0
		private IEnumerator NextPageAnim()
		{
			int tempPageIndex = this.currentPageIndex;
			if (this.pages[1].childCount > 0)
			{
				this.pages[1].GetChild(0).gameObject.SetActive(false);
				this.pages[1].GetChild(0).SetParent(this.simpleContentPagesContainer);
			}
			if (this.pages[2].childCount > 0)
			{
				this.pages[2].GetChild(0).gameObject.SetActive(false);
				this.pages[2].GetChild(0).SetParent(this.simpleContentPagesContainer);
			}
			for (int i = 1; i < 4; i++)
			{
				if (this.pageCount > tempPageIndex + i)
				{
					yield return null;
					if (i > 1)
					{
						this.simpleContentPages[tempPageIndex + i].gameObject.SetActive(false);
					}
					this.simpleContentPages[tempPageIndex + i].SetParent(this.pages[i]);
					this.PrepareContentPage(this.simpleContentPages[tempPageIndex + i]);
				}
			}
			yield break;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00005AEC File Offset: 0x00003CEC
		public void PrevPage()
		{
			if (this.bookState == SimpleBookCreator.BookState.OPEN && this.currentPageIndex - 2 >= 0)
			{
				this.bookState = SimpleBookCreator.BookState.PREVPAGE;
				this.animatedPage.SetActive(true);
				this.pageAnimator.Play("PrevPage", 0, 0f);
				base.StartCoroutine(this.PrevPageAnim());
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00005D3C File Offset: 0x00003F3C
		private IEnumerator PrevPageAnim()
		{
			int tempPageIndex = this.currentPageIndex + 1;
			if (this.pages[1].childCount > 0)
			{
				this.pages[1].GetChild(0).gameObject.SetActive(false);
				this.pages[1].GetChild(0).SetParent(this.simpleContentPagesContainer);
			}
			if (this.pages[2].childCount > 0)
			{
				this.pages[2].GetChild(0).gameObject.SetActive(false);
				this.pages[2].GetChild(0).SetParent(this.simpleContentPagesContainer);
			}
			for (int i = 1; i < 4; i++)
			{
				if (tempPageIndex - i >= 0)
				{
					yield return null;
					if (i > 1)
					{
						this.simpleContentPages[tempPageIndex - i].gameObject.SetActive(false);
					}
					this.simpleContentPages[tempPageIndex - i].SetParent(this.pages[3 - i]);
					this.PrepareContentPage(this.simpleContentPages[tempPageIndex - i]);
				}
			}
			yield break;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00005D58 File Offset: 0x00003F58
		public void NextPageEndAnimEvent()
		{
			if (this.pages[0].childCount > 0)
			{
				this.pages[0].GetChild(0).gameObject.SetActive(false);
				this.pages[0].GetChild(0).SetParent(this.simpleContentPagesContainer);
			}
			if (this.pageCount >= this.currentPageIndex + 2)
			{
				this.simpleContentPages[this.currentPageIndex + 2].SetParent(this.pages[0]);
				this.PrepareContentPage(this.simpleContentPages[this.currentPageIndex + 2]);
			}
			this.animatedPage.SetActive(false);
			this.currentPageIndex += 2;
			this.bookState = SimpleBookCreator.BookState.OPEN;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00005E14 File Offset: 0x00004014
		public void PrevPageEndAnimEvent()
		{
			if (this.pages[3].childCount > 0)
			{
				this.pages[3].GetChild(0).gameObject.SetActive(false);
				this.pages[3].GetChild(0).SetParent(this.simpleContentPagesContainer);
			}
			if (this.currentPageIndex - 1 >= 0)
			{
				this.simpleContentPages[this.currentPageIndex - 1].SetParent(this.pages[3]);
				this.PrepareContentPage(this.simpleContentPages[this.currentPageIndex - 1]);
			}
			this.currentPageIndex -= 2;
			this.animatedPage.SetActive(false);
			this.bookState = SimpleBookCreator.BookState.OPEN;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00005EC8 File Offset: 0x000040C8
		private void PrepareContentPage(RectTransform cp)
		{
			cp.offsetMin = Vector2.zero;
			cp.offsetMax = Vector2.zero;
			cp.localPosition = Vector3.zero;
			cp.localRotation = Quaternion.identity;
			cp.localScale = Vector3.one;
			cp.gameObject.SetActive(true);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00005F18 File Offset: 0x00004118
		public GameObject CreateTextBookContent()
		{
			if (this.bookCreated)
			{
				GameObject gameObject = new GameObject();
				gameObject.transform.SetParent(this.tr);
				gameObject.name = this.prefabName;
				gameObject.AddComponent<RectTransform>();
				foreach (RectTransform rectTransform in this.simpleContentPages)
				{
					rectTransform.gameObject.SetActive(false);
					rectTransform.SetParent(gameObject.transform);
				}
				gameObject.SetActive(false);
				gameObject.transform.SetParent(null);
				gameObject.transform.position = Vector3.zero;
				return gameObject;
			}
			return null;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00005FD8 File Offset: 0x000041D8
		public void PrefabCreatedMessage()
		{
			Debug.Log("Content saved to file: Assets/PowerBooks/BookCreatorScene/SavedBookContent/" + this.prefabName + ".prefab");
			foreach (RectTransform rectTransform in this.simpleContentPages)
			{
				rectTransform.gameObject.SetActive(false);
				rectTransform.SetParent(this.simpleContentPagesContainer);
			}
			if (this.pageCount > this.currentPageIndex)
			{
				this.simpleContentPages[this.currentPageIndex].SetParent(this.pages[0]);
				this.PrepareContentPage(this.simpleContentPages[this.currentPageIndex]);
			}
			if (this.pageCount > this.currentPageIndex + 1)
			{
				this.simpleContentPages[this.currentPageIndex + 1].SetParent(this.pages[3]);
				this.PrepareContentPage(this.simpleContentPages[this.currentPageIndex + 1]);
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000060E4 File Offset: 0x000042E4
		public void PrefabNotCreatedMessage()
		{
			Debug.Log("Content not saved! Please type in a prefab name.");
			foreach (RectTransform rectTransform in this.simpleContentPages)
			{
				rectTransform.gameObject.SetActive(false);
				rectTransform.SetParent(this.simpleContentPagesContainer);
			}
			if (this.pageCount > this.currentPageIndex)
			{
				this.simpleContentPages[this.currentPageIndex].SetParent(this.pages[0]);
				this.PrepareContentPage(this.simpleContentPages[this.currentPageIndex]);
			}
			if (this.pageCount > this.currentPageIndex + 1)
			{
				this.simpleContentPages[this.currentPageIndex + 1].SetParent(this.pages[3]);
				this.PrepareContentPage(this.simpleContentPages[this.currentPageIndex + 1]);
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000061E0 File Offset: 0x000043E0
		public void CreateMultipleTextBooks(string txtFile)
		{
			base.StartCoroutine(this.ConvertMultipleFiles(txtFile));
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000069A4 File Offset: 0x00004BA4
		private IEnumerator ConvertMultipleFiles(string txtFile)
		{
			this.bookCreated = false;
			this.convertState = SimpleBookCreator.ConvertState.WORKING;
			this.simpleContentPagesText.Clear();
			this.simpleContentPages.Clear();
			foreach (object obj in this.simpleContentPagesContainer)
			{
				Transform transform = (Transform)obj;
				Destroy(transform.gameObject);
			}
			string fileResult = "";
			fileResult = txtFile;
			List<string> splittedStrings = new List<string>();
			while (fileResult.Length > 0)
			{
				string text = fileResult.Substring(0, Mathf.Min(12000, fileResult.Length));
				splittedStrings.Add(text);
				if (text == fileResult)
				{
					break;
				}
				fileResult = fileResult.Substring(text.Length, fileResult.Length - Mathf.Min(text.Length, fileResult.Length));
			}
			yield return new WaitForEndOfFrame();
			int maxCharCount = 0;
			for (int i = 0; i < splittedStrings.Count; i++)
			{
				this.percentComplete = 100 / splittedStrings.Count * (i + 1);
				this.simpleBookText.text = splittedStrings[i];
				TextGenerator t = this.simpleBookText.cachedTextGenerator;
				while (this.simpleBookText.text.Length > 0)
				{
					yield return new WaitForEndOfFrame();
					maxCharCount = t.characterCountVisible;
					string result = this.simpleBookText.text.Substring(0, Mathf.Min(t.characterCountVisible, this.simpleBookText.text.Length));
					if (result == this.simpleBookText.text && result.Length <= maxCharCount && i < splittedStrings.Count - 1)
					{
						splittedStrings[i + 1] = result + splittedStrings[i + 1];
					}
					else
					{
						this.simpleContentPagesText.Add(result);
					}
					if (result == this.simpleBookText.text)
					{
						break;
					}
					this.simpleBookText.text = this.simpleBookText.text.Substring(result.Length, this.simpleBookText.text.Length - Mathf.Min(result.Length, this.simpleBookText.text.Length));
				}
			}
			int switchLeftRight = 0;
			using (List<string>.Enumerator enumerator2 = this.simpleContentPagesText.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					string text2 = enumerator2.Current;
					string text3 = text2;
					if (text3.StartsWith(" ") || text3.StartsWith("\n"))
					{
						text3 = text3.Substring(1);
					}
					Transform transform2;
					if (switchLeftRight % 2 == 0)
					{
						transform2 = Instantiate<GameObject>(this.leftPagePrefab, this.simpleContentPagesContainer).transform;
					}
					else
					{
						transform2 = Instantiate<GameObject>(this.rightPagePrefab, this.simpleContentPagesContainer).transform;
					}
					Text componentInChildren = transform2.Find("Text").GetComponentInChildren<Text>();
					componentInChildren.text = text3;
					componentInChildren.font = this.textFont;
					componentInChildren.fontSize = this.textSize;
					componentInChildren.color = this.textColor;
					componentInChildren.fontStyle = FontStyle.Normal;
					componentInChildren.lineSpacing = 1f;
					Text componentInChildren2 = transform2.Find("PageNumber").GetComponentInChildren<Text>();
					if (switchLeftRight % 2 == 0)
					{
						componentInChildren.rectTransform.offsetMin = new Vector2((float)this.textMargin, (float)this.textMargin);
						componentInChildren.rectTransform.offsetMax = new Vector2((float)(Mathf.Max(0, this.textMargin - 100) * -1), (float)(-(float)this.textMargin));
						componentInChildren2.rectTransform.offsetMin = new Vector2((float)this.textMargin, componentInChildren2.rectTransform.offsetMin.y);
						componentInChildren2.rectTransform.offsetMax = new Vector2((float)(Mathf.Max(0, this.textMargin - 100) * -1), componentInChildren2.rectTransform.offsetMax.y);
					}
					else
					{
						componentInChildren.rectTransform.offsetMin = new Vector2((float)Mathf.Max(0, this.textMargin - 100), (float)this.textMargin);
						componentInChildren.rectTransform.offsetMax = new Vector2((float)(-(float)this.textMargin), (float)(-(float)this.textMargin));
						componentInChildren2.rectTransform.offsetMin = new Vector2((float)Mathf.Max(0, this.textMargin - 100), componentInChildren2.rectTransform.offsetMin.y);
						componentInChildren2.rectTransform.offsetMax = new Vector2((float)(-(float)this.textMargin), componentInChildren2.rectTransform.offsetMax.y);
					}
					if (!this.showPageNumbers)
					{
						componentInChildren2.gameObject.SetActive(false);
					}
					else
					{
						componentInChildren2.text = (switchLeftRight + 1).ToString();
						componentInChildren2.font = this.textFont;
						componentInChildren2.fontSize = this.textSize;
						componentInChildren2.color = this.textColor;
					}
					if (this.centerPageNumbers)
					{
						componentInChildren2.alignment = TextAnchor.LowerCenter;
					}
					transform2.gameObject.SetActive(false);
					this.simpleContentPages.Add((RectTransform)transform2);
					switchLeftRight++;
					this.convertState = SimpleBookCreator.ConvertState.READY;
					this.bookCreated = true;
				}
				yield break;
			}
			yield break;
		}

		// Token: 0x04000042 RID: 66
		private Transform tr;

		// Token: 0x04000043 RID: 67
		private SimpleBookCreator.BookState bookState;

		// Token: 0x04000044 RID: 68
		private GameObject book;

		// Token: 0x04000045 RID: 69
		private GameObject animatedBook;

		// Token: 0x04000046 RID: 70
		private GameObject animatedPage;

		// Token: 0x04000047 RID: 71
		private Animator bookAnimator;

		// Token: 0x04000048 RID: 72
		private Animator pageAnimator;

		// Token: 0x04000049 RID: 73
		private int currentPageIndex;

		// Token: 0x0400004A RID: 74
		private int pageCount;

		// Token: 0x0400004B RID: 75
		private GameObject leftPagePrefab;

		// Token: 0x0400004C RID: 76
		private GameObject rightPagePrefab;

		// Token: 0x0400004D RID: 77
		private Transform[] pages;

		// Token: 0x0400004E RID: 78
		private GameObject firstPage;

		// Token: 0x0400004F RID: 79
		private RectTransform simpleContentPagesContainer;

		// Token: 0x04000050 RID: 80
		private List<RectTransform> simpleContentPages;

		// Token: 0x04000051 RID: 81
		private List<string> simpleContentPagesText;

		// Token: 0x04000052 RID: 82
		private Text simpleBookText;

		// Token: 0x04000053 RID: 83
		private int percentComplete;

		// Token: 0x04000054 RID: 84
		private bool bookCreated;

		// Token: 0x04000055 RID: 85
		public TextAsset textFile;

		// Token: 0x04000056 RID: 86
		public Font textFont;

		// Token: 0x04000057 RID: 87
		public int textSize = 32;

		// Token: 0x04000058 RID: 88
		public Color textColor = Color.black;

		// Token: 0x04000059 RID: 89
		[Range(100f, 300f)]
		public int textMargin;

		// Token: 0x0400005A RID: 90
		public bool showPageNumbers;

		// Token: 0x0400005B RID: 91
		public bool centerPageNumbers;

		// Token: 0x0400005C RID: 92
		public string prefabName;

		// Token: 0x0400005D RID: 93
		public bool convertMultipleFiles;

		// Token: 0x0400005E RID: 94
		[HideInInspector]
		public SimpleBookCreator.ConvertState convertState;

		// Token: 0x02000018 RID: 24
		[HideInInspector]
		public enum BookState
		{
			// Token: 0x04000060 RID: 96
			PREPARE,
			// Token: 0x04000061 RID: 97
			CLOSED,
			// Token: 0x04000062 RID: 98
			OPENBOOK,
			// Token: 0x04000063 RID: 99
			OPEN,
			// Token: 0x04000064 RID: 100
			CLOSEBOOK,
			// Token: 0x04000065 RID: 101
			NEXTPAGE,
			// Token: 0x04000066 RID: 102
			PREVPAGE
		}

		// Token: 0x02000019 RID: 25
		public enum ConvertState
		{
			// Token: 0x04000068 RID: 104
			NONE,
			// Token: 0x04000069 RID: 105
			WORKING,
			// Token: 0x0400006A RID: 106
			READY
		}
	}
}

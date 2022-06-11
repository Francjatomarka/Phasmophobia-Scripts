using System;
using System.Collections;
using TLGFPowerBooks;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000016 RID: 22
public class SimpleBookCreatorUI : MonoBehaviour
{
	// Token: 0x060000A4 RID: 164 RVA: 0x0000504B File Offset: 0x0000324B
	private void Start()
	{
		if (this.sbc.convertMultipleFiles)
		{
			this.CreateMultipleBookPrefabs();
		}
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x00005060 File Offset: 0x00003260
	private void Update()
	{
		this.loadingBarPercent.fillAmount = (float)this.sbc.GetPercentComplete() / 100f;
		if (this.sbc.GetBookState() == SimpleBookCreator.BookState.OPEN && !this.loadingComplete)
		{
			this.loadingComplete = true;
			this.ShowControlUI();
		}
		if (Input.GetAxis("Horizontal") > 0f)
		{
			this.sbc.NextPage();
		}
		if (Input.GetAxis("Horizontal") < 0f)
		{
			this.sbc.PrevPage();
		}
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x000050E5 File Offset: 0x000032E5
	private void ShowControlUI()
	{
		this.loadingBar.SetActive(false);
		this.controlPanel.SetActive(true);
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void CreateBookPrefab()
	{
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x000050FF File Offset: 0x000032FF
	public void CreateMultipleBookPrefabs()
	{
		base.StartCoroutine(this.CreateMultipleBooks());
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x0000510E File Offset: 0x0000330E
	private IEnumerator CreateMultipleBooks()
	{
		yield return null;
		yield break;
	}

	// Token: 0x04000076 RID: 118
	public SimpleBookCreator sbc;

	// Token: 0x04000077 RID: 119
	public GameObject loadingBar;

	// Token: 0x04000078 RID: 120
	public Image loadingBarPercent;

	// Token: 0x04000079 RID: 121
	public GameObject controlPanel;

	// Token: 0x0400007A RID: 122
	private bool loadingComplete;
}

using System;
using System.Collections;
using TLGFPowerBooks;
using UnityEngine;
using UnityEngine.UI;

public class SimpleBookCreatorUI : MonoBehaviour
{
	private void Start()
	{
		if (this.sbc.convertMultipleFiles)
		{
			this.CreateMultipleBookPrefabs();
		}
	}

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

	private void ShowControlUI()
	{
		this.loadingBar.SetActive(false);
		this.controlPanel.SetActive(true);
	}

	public void CreateBookPrefab()
	{
	}

	public void CreateMultipleBookPrefabs()
	{
		base.StartCoroutine(this.CreateMultipleBooks());
	}

	private IEnumerator CreateMultipleBooks()
	{
		yield return null;
		yield break;
	}

	public SimpleBookCreator sbc;

	public GameObject loadingBar;

	public Image loadingBarPercent;

	public GameObject controlPanel;

	private bool loadingComplete;
}


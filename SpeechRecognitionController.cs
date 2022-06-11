using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Windows.Speech;

// Token: 0x020000E1 RID: 225
public class SpeechRecognitionController : MonoBehaviour
{
	// Token: 0x06000662 RID: 1634 RVA: 0x00025D06 File Offset: 0x00023F06
	private void Awake()
	{
		SpeechRecognitionController.instance = this;
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x06000663 RID: 1635 RVA: 0x00025D1A File Offset: 0x00023F1A
	public void AddOuijaBoard(OuijaBoard b)
	{
		this.board = b;
		this.StartPhraseRecogniser();
	}

	// Token: 0x06000664 RID: 1636 RVA: 0x00025D29 File Offset: 0x00023F29
	public void AddEVPRecorder(EVPRecorder r)
	{
		r.SetupKeywords();
		this.recorders.Add(r);
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x00025D40 File Offset: 0x00023F40
	public void StartPhraseRecogniser()
	{
		this.hasStartedRecogniser = true;
		this.Recognizer = new KeywordRecognizer(this.Keywords.ToArray(), ConfidenceLevel.Low);
		this.Recognizer.OnPhraseRecognized += this.OnPhraseRecognized;
		this.Recognizer.Start();
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x00025D90 File Offset: 0x00023F90
	private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
	{
		this.board.OnPhraseRecognized(args.text);
		this.listener.OnPhraseRecognized(args.text);
		for (int i = 0; i < this.recorders.Count; i++)
		{
			this.recorders[i].OnPhraseRecognized(args.text);
		}
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x00025DEC File Offset: 0x00023FEC
	public void AddKeyword(string key)
	{
		if (!this.Keywords.Contains(key) && !string.IsNullOrEmpty(key))
		{
			this.Keywords.Add(key);
		}
	}

	// Token: 0x06000668 RID: 1640 RVA: 0x00025E10 File Offset: 0x00024010
	private void OnDisable()
	{
		if (this.hasStartedRecogniser)
		{
			this.Recognizer.OnPhraseRecognized -= this.OnPhraseRecognized;
		}
	}

	// Token: 0x04000634 RID: 1588
	public static SpeechRecognitionController instance;

	// Token: 0x04000635 RID: 1589
	private KeywordRecognizer Recognizer;

	// Token: 0x04000636 RID: 1590
	private List<string> Keywords = new List<string>();

	// Token: 0x04000637 RID: 1591
	[SerializeField]
	private PhraseListenerController listener;

	// Token: 0x04000638 RID: 1592
	private List<EVPRecorder> recorders = new List<EVPRecorder>();

	// Token: 0x04000639 RID: 1593
	private OuijaBoard board;

	// Token: 0x0400063A RID: 1594
	[HideInInspector]
	public bool hasStartedRecogniser;

	// Token: 0x0400063B RID: 1595
	[HideInInspector]
	public bool hasSaidGhostsName;

	// Token: 0x0400063C RID: 1596
	private PhotonView view;
}

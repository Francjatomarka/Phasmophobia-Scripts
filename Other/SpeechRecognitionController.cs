using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Windows.Speech;

public class SpeechRecognitionController : MonoBehaviour
{
	private void Awake()
	{
		SpeechRecognitionController.instance = this;
		this.view = base.GetComponent<PhotonView>();
	}

	public void AddOuijaBoard(OuijaBoard b)
	{
		this.board = b;
		this.StartPhraseRecogniser();
	}

	public void AddEVPRecorder(EVPRecorder r)
	{
		r.SetupKeywords();
		this.recorders.Add(r);
	}

	public void StartPhraseRecogniser()
	{
		this.hasStartedRecogniser = true;
		this.Recognizer = new KeywordRecognizer(this.Keywords.ToArray(), ConfidenceLevel.Low);
		this.Recognizer.OnPhraseRecognized += this.OnPhraseRecognized;
		this.Recognizer.Start();
	}

	private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
	{
		this.board.OnPhraseRecognized(args.text);
		this.listener.OnPhraseRecognized(args.text);
		for (int i = 0; i < this.recorders.Count; i++)
		{
			this.recorders[i].OnPhraseRecognized(args.text);
		}
	}

	public void AddKeyword(string key)
	{
		if (!this.Keywords.Contains(key) && !string.IsNullOrEmpty(key))
		{
			this.Keywords.Add(key);
		}
	}

	private void OnDisable()
	{
		if (this.hasStartedRecogniser)
		{
			this.Recognizer.OnPhraseRecognized -= this.OnPhraseRecognized;
		}
	}

	public static SpeechRecognitionController instance;

	private KeywordRecognizer Recognizer;

	private List<string> Keywords = new List<string>();

	[SerializeField]
	private PhraseListenerController listener;

	private List<EVPRecorder> recorders = new List<EVPRecorder>();

	private OuijaBoard board;

	[HideInInspector]
	public bool hasStartedRecogniser;

	[HideInInspector]
	public bool hasSaidGhostsName;

	private PhotonView view;
}


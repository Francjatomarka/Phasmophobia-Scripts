using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class VoiceRecognitionManager : MonoBehaviour
{
	private void OnEnable()
	{
		this.phraseHasBeenRecognised = false;
		this.phraseText.text = LocalisationSystem.GetLocalisedValue("Options_Say") + " \"" + LocalisationSystem.GetLocalisedVoiceValue("Q_Give us a sign") + "\"";
		this.listeningString = LocalisationSystem.GetLocalisedValue("Options_Listening");
		this.answerText.text = this.listeningString;
		if (!this.keywords.Contains(LocalisationSystem.GetLocalisedVoiceValue("Q_Give us a sign")))
		{
			this.keywords.Add(LocalisationSystem.GetLocalisedVoiceValue("Q_Give us a sign"));
		}
		if (this.recognizer == null)
		{
			this.recognizer = new KeywordRecognizer(this.keywords.ToArray(), ConfidenceLevel.Low);
			this.recognizer.OnPhraseRecognized += this.OnPhraseRecognized;
			this.recognizer.Start();
			this.statusText.text = "Voice recognition is setup correctly.";
			return;
		}
		this.statusText.text = "Error: Voice recognition is not setup on your PC correctly.";
	}

	private void OnDisable()
	{
		if (this.recognizer != null)
		{
			this.recognizer.OnPhraseRecognized -= this.OnPhraseRecognized;
			this.recognizer.Stop();
			this.recognizer = null;
		}
	}

	private void Update()
	{
		if (this.phraseHasBeenRecognised)
		{
			return;
		}
		this.timer -= Time.deltaTime;
		if (this.timer < 0f)
		{
			this.count++;
			if (this.count > 3)
			{
				this.count = 0;
			}
			if (this.count == 0)
			{
				this.answerText.text = this.listeningString;
			}
			else if (this.count == 1)
			{
				this.answerText.text = this.listeningString + ".";
			}
			else if (this.count == 2)
			{
				this.answerText.text = this.listeningString + "..";
			}
			else
			{
				this.answerText.text = this.listeningString + "...";
			}
			this.timer = 0.5f;
		}
	}

	private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
	{
		this.phraseHasBeenRecognised = true;
		this.answerText.text = LocalisationSystem.GetLocalisedValue("Options_Heard");
	}

	private KeywordRecognizer recognizer;

	private List<string> keywords = new List<string>();

	[SerializeField]
	private Text phraseText;

	[SerializeField]
	private Text answerText;

	[SerializeField]
	private Text statusText;

	private bool phraseHasBeenRecognised;

	private string listeningString = "Listening...";

	private float timer = 0.5f;

	private int count;

	private const string quote = "\"";
}


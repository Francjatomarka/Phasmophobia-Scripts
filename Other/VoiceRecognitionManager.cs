using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

// Token: 0x02000194 RID: 404
public class VoiceRecognitionManager : MonoBehaviour
{
	// Token: 0x06000B13 RID: 2835 RVA: 0x00045294 File Offset: 0x00043494
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

	// Token: 0x06000B14 RID: 2836 RVA: 0x00045385 File Offset: 0x00043585
	private void OnDisable()
	{
		if (this.recognizer != null)
		{
			this.recognizer.OnPhraseRecognized -= this.OnPhraseRecognized;
			this.recognizer.Stop();
			this.recognizer = null;
		}
	}

	// Token: 0x06000B15 RID: 2837 RVA: 0x000453B8 File Offset: 0x000435B8
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

	// Token: 0x06000B16 RID: 2838 RVA: 0x0004549B File Offset: 0x0004369B
	private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
	{
		this.phraseHasBeenRecognised = true;
		this.answerText.text = LocalisationSystem.GetLocalisedValue("Options_Heard");
	}

	// Token: 0x04000B69 RID: 2921
	private KeywordRecognizer recognizer;

	// Token: 0x04000B6A RID: 2922
	private List<string> keywords = new List<string>();

	// Token: 0x04000B6B RID: 2923
	[SerializeField]
	private Text phraseText;

	// Token: 0x04000B6C RID: 2924
	[SerializeField]
	private Text answerText;

	// Token: 0x04000B6D RID: 2925
	[SerializeField]
	private Text statusText;

	// Token: 0x04000B6E RID: 2926
	private bool phraseHasBeenRecognised;

	// Token: 0x04000B6F RID: 2927
	private string listeningString = "Listening...";

	// Token: 0x04000B70 RID: 2928
	private float timer = 0.5f;

	// Token: 0x04000B71 RID: 2929
	private int count;

	// Token: 0x04000B72 RID: 2930
	private const string quote = "\"";
}

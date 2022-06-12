using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenHint : MonoBehaviour
{
	private void Awake()
	{
		this.myText = base.GetComponent<Text>();
		this.hints.Add(LocalisationSystem.GetLocalisedValue("Loading_Hint1"));
		this.hints.Add(LocalisationSystem.GetLocalisedValue("Loading_Hint2"));
		this.hints.Add(LocalisationSystem.GetLocalisedValue("Loading_Hint3"));
		this.hints.Add(LocalisationSystem.GetLocalisedValue("Loading_Hint4"));
		this.hints.Add(LocalisationSystem.GetLocalisedValue("Loading_Hint5"));
		this.hints.Add(LocalisationSystem.GetLocalisedValue("Loading_Hint6"));
		this.hints.Add(LocalisationSystem.GetLocalisedValue("Loading_Hint7"));
		this.hints.Add(LocalisationSystem.GetLocalisedValue("Loading_Hint8"));
		this.hints.Add(LocalisationSystem.GetLocalisedValue("Loading_Hint9"));
		this.hints.Add(LocalisationSystem.GetLocalisedValue("Loading_Hint10"));
		this.hints.Add(LocalisationSystem.GetLocalisedValue("Loading_Hint11"));
		this.myText.text = LocalisationSystem.GetLocalisedValue("Loading_Hint") + ": " + this.hints[UnityEngine.Random.Range(0, this.hints.Count)];
	}

	private List<string> hints = new List<string>();

	private Text myText;
}


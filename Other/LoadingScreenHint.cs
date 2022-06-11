using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000181 RID: 385
public class LoadingScreenHint : MonoBehaviour
{
	// Token: 0x06000A55 RID: 2645 RVA: 0x00040440 File Offset: 0x0003E640
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

	// Token: 0x04000A81 RID: 2689
	private List<string> hints = new List<string>();

	// Token: 0x04000A82 RID: 2690
	private Text myText;
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200012F RID: 303
[RequireComponent(typeof(Dropdown))]
public class SetDropdownValues : MonoBehaviour
{
	// Token: 0x06000866 RID: 2150 RVA: 0x000334DF File Offset: 0x000316DF
	private void Awake()
	{
		this.dropdown = base.GetComponent<Dropdown>();
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x000334F0 File Offset: 0x000316F0
	private void Start()
	{
		for (int i = 0; i < this.dropdown.options.Count; i++)
		{
			this.values.Add(LocalisationSystem.GetLocalisedValue(this.dropdown.options[i].text));
		}
		this.dropdown.ClearOptions();
		this.dropdown.AddOptions(this.values);
	}

	// Token: 0x04000877 RID: 2167
	private Dropdown dropdown;

	// Token: 0x04000878 RID: 2168
	private List<string> values = new List<string>();
}

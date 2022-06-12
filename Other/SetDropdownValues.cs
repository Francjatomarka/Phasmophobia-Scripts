using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class SetDropdownValues : MonoBehaviour
{
	private void Awake()
	{
		this.dropdown = base.GetComponent<Dropdown>();
	}

	private void Start()
	{
		for (int i = 0; i < this.dropdown.options.Count; i++)
		{
			this.values.Add(LocalisationSystem.GetLocalisedValue(this.dropdown.options[i].text));
		}
		this.dropdown.ClearOptions();
		this.dropdown.AddOptions(this.values);
	}

	private Dropdown dropdown;

	private List<string> values = new List<string>();
}


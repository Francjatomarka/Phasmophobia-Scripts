using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextLocaliserUI : MonoBehaviour
{
	private void Awake()
	{
		this.textField = base.GetComponent<Text>();
	}

	private void Start()
	{
		this.LoadText();
	}

	private void OnEnable()
	{
		if (SceneManager.GetActiveScene().name == "Menu_New")
		{
			this.LoadText();
		}
	}

	public void LoadText()
	{
		if (this.textField != null && this.textField.text != string.Empty)
		{
			this.textField.text = LocalisationSystem.GetLocalisedValue(this.key);
		}
	}

	public string key;

	private Text textField;
}


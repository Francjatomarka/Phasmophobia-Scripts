using System;
using RedScarf.EasyCSV;
using UnityEngine;

public class LocalisationController : MonoBehaviour
{
	private void Awake()
	{
		this.text = Resources.Load<TextAsset>("localisation");
	}

	private void Start()
	{
		CsvHelper.Init(',');
		this.table = CsvHelper.Create(this.text.name, this.text.text, true, true);
	}

	private TextAsset text;

	private CsvTable table;
}


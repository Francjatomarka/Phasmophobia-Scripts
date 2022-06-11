using System;
using RedScarf.EasyCSV;
using UnityEngine;

// Token: 0x0200011A RID: 282
public class LocalisationController : MonoBehaviour
{
	// Token: 0x0600079C RID: 1948 RVA: 0x0002D41E File Offset: 0x0002B61E
	private void Awake()
	{
		this.text = Resources.Load<TextAsset>("localisation");
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x0002D430 File Offset: 0x0002B630
	private void Start()
	{
		CsvHelper.Init(',');
		this.table = CsvHelper.Create(this.text.name, this.text.text, true, true);
	}

	// Token: 0x04000778 RID: 1912
	private TextAsset text;

	// Token: 0x04000779 RID: 1913
	private CsvTable table;
}

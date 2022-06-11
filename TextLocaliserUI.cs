using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000130 RID: 304
public class TextLocaliserUI : MonoBehaviour
{
	// Token: 0x06000869 RID: 2153 RVA: 0x0003356D File Offset: 0x0003176D
	private void Awake()
	{
		this.textField = base.GetComponent<Text>();
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x0003357B File Offset: 0x0003177B
	private void Start()
	{
		this.LoadText();
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x00033584 File Offset: 0x00031784
	private void OnEnable()
	{
		if (SceneManager.GetActiveScene().name == "Menu_New")
		{
			this.LoadText();
		}
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x000335B0 File Offset: 0x000317B0
	public void LoadText()
	{
		if (this.textField != null && this.textField.text != string.Empty)
		{
			this.textField.text = LocalisationSystem.GetLocalisedValue(this.key);
		}
	}

	// Token: 0x04000879 RID: 2169
	public string key;

	// Token: 0x0400087A RID: 2170
	private Text textField;
}

using System;
using UnityEngine;
using UnityEngine.UI;

public class DeveloperGhostSelect : MonoBehaviour
{
	private void Start()
	{
		this.GhostTypeText.gameObject.SetActive(false);
		this.GhostTypeText.text = this.ghostType.ToString();
	}

	public void ChangeButton(int amount)
	{
		this.id += amount;
		this.id = Mathf.Clamp(this.id, 0, 12);
		this.ghostType = (GhostTraits.Type)this.id;
		this.GhostTypeText.text = this.ghostType.ToString();
		PlayerPrefs.SetInt("Developer_GhostType", this.id);
	}

	[SerializeField]
	private StoreSDKManager storeSDKManager;

	[SerializeField]
	private Text GhostTypeText;

	private int id;

	public GhostTraits.Type ghostType;
}


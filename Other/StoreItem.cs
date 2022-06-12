using System;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
	private void Start()
	{
		this.costText.text = "$" + this.cost.ToString();
	}

	public int cost;

	public GameObject description;

	public Text costText;

	public Text amountOwnedText;

	public string itemName;

	public int requiredLevel;

	public Button buyButton;

	public Text buyButtonText;
}


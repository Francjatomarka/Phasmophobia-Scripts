using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200014B RID: 331
public class StoreItem : MonoBehaviour
{
	// Token: 0x06000957 RID: 2391 RVA: 0x00039CD3 File Offset: 0x00037ED3
	private void Start()
	{
		this.costText.text = "$" + this.cost.ToString();
	}

	// Token: 0x04000990 RID: 2448
	public int cost;

	// Token: 0x04000991 RID: 2449
	public GameObject description;

	// Token: 0x04000992 RID: 2450
	public Text costText;

	// Token: 0x04000993 RID: 2451
	public Text amountOwnedText;

	// Token: 0x04000994 RID: 2452
	public string itemName;

	// Token: 0x04000995 RID: 2453
	public int requiredLevel;

	// Token: 0x04000996 RID: 2454
	public Button buyButton;

	// Token: 0x04000997 RID: 2455
	public Text buyButtonText;
}

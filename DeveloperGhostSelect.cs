using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000178 RID: 376
public class DeveloperGhostSelect : MonoBehaviour
{
	// Token: 0x06000A06 RID: 2566 RVA: 0x0003E260 File Offset: 0x0003C460
	private void Start()
	{
		this.GhostTypeText.gameObject.SetActive(false);
		this.GhostTypeText.text = this.ghostType.ToString();
	}

	// Token: 0x06000A07 RID: 2567 RVA: 0x0003E2BC File Offset: 0x0003C4BC
	public void ChangeButton(int amount)
	{
		this.id += amount;
		this.id = Mathf.Clamp(this.id, 0, 12);
		this.ghostType = (GhostTraits.Type)this.id;
		this.GhostTypeText.text = this.ghostType.ToString();
		PlayerPrefs.SetInt("Developer_GhostType", this.id);
	}

	// Token: 0x04000A36 RID: 2614
	[SerializeField]
	private StoreSDKManager storeSDKManager;

	// Token: 0x04000A37 RID: 2615
	[SerializeField]
	private Text GhostTypeText;

	// Token: 0x04000A38 RID: 2616
	private int id;

	// Token: 0x04000A39 RID: 2617
	public GhostTraits.Type ghostType;
}

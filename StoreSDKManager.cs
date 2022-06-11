using System;
using UnityEngine;

// Token: 0x02000177 RID: 375
public class StoreSDKManager : MonoBehaviour
{
	// Token: 0x06000AB8 RID: 2744 RVA: 0x000426D0 File Offset: 0x000408D0
	private void Awake()
	{
		this.storeBranchType = StoreSDKManager.StoreBranchType.normal;
	}

	// Token: 0x06000AB9 RID: 2745 RVA: 0x00042776 File Offset: 0x00040976
	private void Start()
	{
		PlayerPrefs.SetInt("isYoutuberVersion", (this.storeBranchType == StoreSDKManager.StoreBranchType.youtube) ? 1 : 0);
	}

	// Token: 0x06000ABA RID: 2746 RVA: 0x0004278F File Offset: 0x0004098F
	public void QueryArcadeLicense()
	{
		
	}

	// Token: 0x06000ABB RID: 2747 RVA: 0x000427AB File Offset: 0x000409AB
	private void QueryRunTimeHandler(int nResult, int nMode)
	{
		if (nResult == 0 && nMode == 2)
		{
			this.storeBranchType = StoreSDKManager.StoreBranchType.youtube;
		}
	}

	// Token: 0x04000B1F RID: 2847
	public StoreSDKManager.StoreSDKType storeSDKType;

	// Token: 0x04000B20 RID: 2848
	[HideInInspector]
	public StoreSDKManager.StoreBranchType storeBranchType;

	// Token: 0x04000B21 RID: 2849
	public string serverVersion;

	// Token: 0x04000B24 RID: 2852
	[SerializeField]
	private MainManager mainManager;

	// Token: 0x020004F5 RID: 1269
	public enum StoreSDKType
	{
		// Token: 0x04002358 RID: 9048
		steam,
		// Token: 0x04002359 RID: 9049
		viveport
	}

	// Token: 0x020004F6 RID: 1270
	public enum StoreBranchType
	{
		// Token: 0x0400235B RID: 9051
		normal,
		// Token: 0x0400235C RID: 9052
		beta,
		// Token: 0x0400235D RID: 9053
		youtube
	}
}

using System;
using UnityEngine;

public class StoreSDKManager : MonoBehaviour
{
	private void Awake()
	{
		this.storeBranchType = StoreSDKManager.StoreBranchType.normal;
	}

	private void Start()
	{
		PlayerPrefs.SetInt("isYoutuberVersion", (this.storeBranchType == StoreSDKManager.StoreBranchType.youtube) ? 1 : 0);
	}

	public void QueryArcadeLicense()
	{
		
	}

	private void QueryRunTimeHandler(int nResult, int nMode)
	{
		if (nResult == 0 && nMode == 2)
		{
			this.storeBranchType = StoreSDKManager.StoreBranchType.youtube;
		}
	}

	public StoreSDKManager.StoreSDKType storeSDKType;

	[HideInInspector]
	public StoreSDKManager.StoreBranchType storeBranchType;

	public string serverVersion;

	[SerializeField]
	private MainManager mainManager;

	public enum StoreSDKType
	{
		steam,
		viveport
	}

	public enum StoreBranchType
	{
		normal,
		beta,
		youtube
	}
}


using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x020000EB RID: 235
[RequireComponent(typeof(PhotonView))]
public class Evidence : MonoBehaviourPunCallbacks
{
	// Token: 0x06000683 RID: 1667 RVA: 0x0002697E File Offset: 0x00024B7E
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x06000684 RID: 1668 RVA: 0x0002698C File Offset: 0x00024B8C
	private void OnEnable()
	{
		if (base.GetComponent<EMF>())
		{
			this.SetValuesLocal();
			return;
		}
		this.SetValuesNetworked();
	}

	// Token: 0x06000685 RID: 1669 RVA: 0x0002698C File Offset: 0x00024B8C
	private void OnJoinedRoom()
	{
		if (base.GetComponent<EMF>())
		{
			this.SetValuesLocal();
			return;
		}
		this.SetValuesNetworked();
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x000269A8 File Offset: 0x00024BA8
	private void SetValuesNetworked()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		switch (this.EvidenceType)
		{
		case Evidence.Type.emfSpot:
			this.view.RPC("SyncEvidenceAmount", RpcTarget.All, new object[]
			{
				UnityEngine.Random.Range(30, 50),
				"Evidence_Interaction"
			});
			return;
		case Evidence.Type.ouijaBoard:
			this.view.RPC("SyncEvidenceAmount", RpcTarget.All, new object[]
			{
				UnityEngine.Random.Range(50, 100),
				"Evidence_OuijaBoard"
			});
			return;
		case Evidence.Type.fingerprint:
			this.view.RPC("SyncEvidenceAmount", RpcTarget.All, new object[]
			{
				UnityEngine.Random.Range(15, 60),
				"Evidence_Fingerprints"
			});
			return;
		case Evidence.Type.footstep:
			this.view.RPC("SyncEvidenceAmount", RpcTarget.All, new object[]
			{
				UnityEngine.Random.Range(20, 40),
				"Evidence_Footstep"
			});
			return;
		case Evidence.Type.DNA:
			this.view.RPC("SyncEvidenceAmount", RpcTarget.All, new object[]
			{
				UnityEngine.Random.Range(40, 80),
				"Evidence_DNA"
			});
			return;
		case Evidence.Type.ghost:
			this.view.RPC("SyncEvidenceAmount", RpcTarget.All, new object[]
			{
				100,
				"Evidence_Ghost"
			});
			return;
		case Evidence.Type.deadBody:
			this.view.RPC("SyncEvidenceAmount", RpcTarget.All, new object[]
			{
				UnityEngine.Random.Range(10, 30),
				"Evidence_Dead Body"
			});
			return;
		case Evidence.Type.dirtyWater:
			this.view.RPC("SyncEvidenceAmount", RpcTarget.All, new object[]
			{
				UnityEngine.Random.Range(15, 30),
				"Evidence_DirtyWater"
			});
			return;
		default:
			Debug.LogError("Evidence: " + base.gameObject.name + " has no evidence type.");
			return;
		}
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x00026B90 File Offset: 0x00024D90
	private void SetValuesLocal()
	{
		switch (this.EvidenceType)
		{
		case Evidence.Type.emfSpot:
			this.SyncEvidenceAmount(UnityEngine.Random.Range(30, 50), "Evidence_Interaction");
			return;
		case Evidence.Type.ouijaBoard:
			this.SyncEvidenceAmount(UnityEngine.Random.Range(50, 100), "Evidence_OuijaBoard");
			return;
		case Evidence.Type.fingerprint:
			this.SyncEvidenceAmount(UnityEngine.Random.Range(15, 60), "Evidence_Fingerprints");
			return;
		case Evidence.Type.footstep:
			this.SyncEvidenceAmount(UnityEngine.Random.Range(20, 40), "Evidence_Footstep");
			return;
		case Evidence.Type.DNA:
			this.SyncEvidenceAmount(UnityEngine.Random.Range(40, 80), "Evidence_DNA");
			return;
		case Evidence.Type.ghost:
			this.SyncEvidenceAmount(100, "Evidence_Ghost");
			return;
		case Evidence.Type.deadBody:
			this.SyncEvidenceAmount(UnityEngine.Random.Range(10, 30), "Evidence_Dead Body");
			return;
		case Evidence.Type.dirtyWater:
			this.SyncEvidenceAmount(UnityEngine.Random.Range(15, 30), "Evidence_DirtyWater");
			return;
		default:
			Debug.LogError("Evidence: " + base.gameObject.name + " has no evidence type.");
			return;
		}
	}

	// Token: 0x06000688 RID: 1672 RVA: 0x00026C90 File Offset: 0x00024E90
	[PunRPC]
	private void SyncEvidenceAmount(int amount, string name)
	{
		this.paranormalEvidenceAmount = amount;
		this.evidenceName = LocalisationSystem.GetLocalisedValue(name);
		if (EvidenceController.instance && !EvidenceController.instance.evidenceInLevel.Contains(this))
		{
			EvidenceController.instance.evidenceInLevel.Add(this);
		}
	}

	// Token: 0x06000689 RID: 1673 RVA: 0x00026CDE File Offset: 0x00024EDE
	private void OnDisable()
	{
		if (EvidenceController.instance && EvidenceController.instance.evidenceInLevel.Contains(this))
		{
			EvidenceController.instance.evidenceInLevel.Remove(this);
		}
	}

	// Token: 0x0600068A RID: 1674 RVA: 0x00026D0F File Offset: 0x00024F0F
	public int GetEvidenceAmount()
	{
		this.hasAlreadyTakenPhoto = true;
		return this.paranormalEvidenceAmount;
	}

	// Token: 0x0400069D RID: 1693
	private PhotonView view;

	// Token: 0x0400069E RID: 1694
	public bool showsGhostVictim;

	// Token: 0x0400069F RID: 1695
	[HideInInspector]
	public bool hasAlreadyTakenPhoto;

	// Token: 0x040006A0 RID: 1696
	[HideInInspector]
	public int paranormalEvidenceAmount;

	// Token: 0x040006A1 RID: 1697
	public Evidence.Type EvidenceType;

	// Token: 0x040006A2 RID: 1698
	public string evidenceName;

	// Token: 0x020004BB RID: 1211
	public enum Type
	{
		// Token: 0x04002271 RID: 8817
		emfSpot,
		// Token: 0x04002272 RID: 8818
		ouijaBoard,
		// Token: 0x04002273 RID: 8819
		fingerprint,
		// Token: 0x04002274 RID: 8820
		footstep,
		// Token: 0x04002275 RID: 8821
		DNA,
		// Token: 0x04002276 RID: 8822
		ghost,
		// Token: 0x04002277 RID: 8823
		deadBody,
		// Token: 0x04002278 RID: 8824
		dirtyWater
	}
}

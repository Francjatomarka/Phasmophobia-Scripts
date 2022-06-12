using System;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class Evidence : MonoBehaviourPunCallbacks
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
	}

	private void OnEnable()
	{
		if (base.GetComponent<EMF>())
		{
			this.SetValuesLocal();
			return;
		}
		this.SetValuesNetworked();
	}

	private void OnJoinedRoom()
	{
		if (base.GetComponent<EMF>())
		{
			this.SetValuesLocal();
			return;
		}
		this.SetValuesNetworked();
	}

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

	private void OnDisable()
	{
		if (EvidenceController.instance && EvidenceController.instance.evidenceInLevel.Contains(this))
		{
			EvidenceController.instance.evidenceInLevel.Remove(this);
		}
	}

	public int GetEvidenceAmount()
	{
		this.hasAlreadyTakenPhoto = true;
		return this.paranormalEvidenceAmount;
	}

	private PhotonView view;

	public bool showsGhostVictim;

	[HideInInspector]
	public bool hasAlreadyTakenPhoto;

	[HideInInspector]
	public int paranormalEvidenceAmount;

	public Evidence.Type EvidenceType;

	public string evidenceName;

	public enum Type
	{
		emfSpot,
		ouijaBoard,
		fingerprint,
		footstep,
		DNA,
		ghost,
		deadBody,
		dirtyWater
	}
}


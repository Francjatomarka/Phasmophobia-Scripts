using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000092 RID: 146
[RequireComponent(typeof(PhotonView))]
public class PickupItemSyncer : MonoBehaviourPunCallbacks
{
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
		if (PhotonNetwork.IsMasterClient)
		{
			this.SendPickedUpItems(otherPlayer);
		}
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x000190A8 File Offset: 0x000172A8
	public void OnJoinedRoom()
	{
		Debug.Log(string.Concat(new object[]
		{
			"Joined Room. isMasterClient: ",
			PhotonNetwork.IsMasterClient.ToString(),
			" id: ",
			PhotonNetwork.LocalPlayer.UserId
		}));
		this.IsWaitingForPickupInit = !PhotonNetwork.IsMasterClient;
		if (PhotonNetwork.PlayerList.Length >= 2)
		{
			base.Invoke("AskForPickupItemSpawnTimes", 2f);
		}
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x00019120 File Offset: 0x00017320
	public void AskForPickupItemSpawnTimes()
	{
		if (this.IsWaitingForPickupInit)
		{
			if (PhotonNetwork.PlayerList.Length < 2)
			{
				Debug.Log("Cant ask anyone else for PickupItem spawn times.");
				this.IsWaitingForPickupInit = false;
				return;
			}
			Photon.Realtime.Player next = PhotonNetwork.MasterClient.GetNext();
			if (next == null || next.Equals(PhotonNetwork.LocalPlayer))
			{
				next = PhotonNetwork.LocalPlayer.GetNext();
			}
			if (next != null && !next.Equals(PhotonNetwork.LocalPlayer))
			{
				base.photonView.RPC("RequestForPickupItems", next, Array.Empty<object>());
				return;
			}
			Debug.Log("No player left to ask");
			this.IsWaitingForPickupInit = false;
		}
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x000191AF File Offset: 0x000173AF
	[PunRPC]
	[Obsolete("Use RequestForPickupItems(PhotonMessageInfo msgInfo) with corrected typing instead.")]
	public void RequestForPickupTimes(PhotonMessageInfo msgInfo)
	{
		this.RequestForPickupItems(msgInfo);
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x000191B8 File Offset: 0x000173B8
	[PunRPC]
	public void RequestForPickupItems(PhotonMessageInfo msgInfo)
	{
		if (msgInfo.Sender == null)
		{
			Debug.LogError("Unknown player asked for PickupItems");
			return;
		}
		this.SendPickedUpItems(msgInfo.Sender);
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x000191DC File Offset: 0x000173DC
	private void SendPickedUpItems(Photon.Realtime.Player targetPlayer)
	{
		if (targetPlayer == null)
		{
			Debug.LogWarning("Cant send PickupItem spawn times to unknown targetPlayer.");
			return;
		}
		double time = PhotonNetwork.Time;
		double num = time + 0.20000000298023224;
		PickupItem[] array = new PickupItem[PickupItem.DisabledPickupItems.Count];
		PickupItem.DisabledPickupItems.CopyTo(array);
		List<float> list = new List<float>(array.Length * 2);
		foreach (PickupItem pickupItem in array)
		{
			if (pickupItem.SecondsBeforeRespawn <= 0f)
			{
				list.Add((float)pickupItem.ViewID);
				list.Add(0f);
			}
			else
			{
				double num2 = pickupItem.TimeOfRespawn - PhotonNetwork.Time;
				if (pickupItem.TimeOfRespawn > num)
				{
					Debug.Log(string.Concat(new object[]
					{
						pickupItem.ViewID,
						" respawn: ",
						pickupItem.TimeOfRespawn,
						" timeUntilRespawn: ",
						num2,
						" (now: ",
						PhotonNetwork.Time,
						")"
					}));
					list.Add((float)pickupItem.ViewID);
					list.Add((float)num2);
				}
			}
		}
		Debug.Log(string.Concat(new object[]
		{
			"Sent count: ",
			list.Count,
			" now: ",
			time
		}));
		base.photonView.RPC("PickupItemInit", targetPlayer, new object[]
		{
			PhotonNetwork.Time,
			list.ToArray()
		});
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x00019374 File Offset: 0x00017574
	[PunRPC]
	public void PickupItemInit(double timeBase, float[] inactivePickupsAndTimes)
	{
		this.IsWaitingForPickupInit = false;
		for (int i = 0; i < inactivePickupsAndTimes.Length / 2; i++)
		{
			int num = i * 2;
			int viewID = (int)inactivePickupsAndTimes[num];
			float num2 = inactivePickupsAndTimes[num + 1];
			PhotonView photonView = PhotonView.Find(viewID);
			PickupItem component = photonView.GetComponent<PickupItem>();
			if (num2 <= 0f)
			{
				component.PickedUp(0f);
			}
			else
			{
				double num3 = (double)num2 + timeBase;
				Debug.Log(string.Concat(new object[]
				{
					photonView.ViewID,
					" respawn: ",
					num3,
					" timeUntilRespawnBasedOnTimeBase:",
					num2,
					" SecondsBeforeRespawn: ",
					component.SecondsBeforeRespawn
				}));
				double num4 = num3 - PhotonNetwork.Time;
				if (num2 <= 0f)
				{
					num4 = 0.0;
				}
				component.PickedUp((float)num4);
			}
		}
	}

	// Token: 0x0400047C RID: 1148
	public bool IsWaitingForPickupInit;

	// Token: 0x0400047D RID: 1149
	private const float TimeDeltaToIgnore = 0.2f;
}

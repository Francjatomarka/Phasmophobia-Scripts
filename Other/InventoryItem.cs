using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// Token: 0x02000139 RID: 313
public class InventoryItem : MonoBehaviour
{
	// Token: 0x060008AF RID: 2223 RVA: 0x00034F70 File Offset: 0x00033170
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		PlayerPrefs.SetInt("current" + this.itemName + "Amount", 0);
		PlayerPrefs.SetInt("total" + this.itemName + "Amount", 0);
		for (int i = 0; i < 4; i++)
		{
			this.players.Add(new InventoryItem.PlayerInventoryItem());
		}
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x00034FDC File Offset: 0x000331DC
	private void Start()
	{
		if (this.defaultAmount > 0)
		{
			this.ChangeTotalAmountNetworked(999, this.defaultAmount);
		}
		this.view.RPC("AddPlayer", RpcTarget.AllBuffered, new object[]
		{
			1
		});
		this.UpdateTotalText();
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x00035034 File Offset: 0x00033234
	[PunRPC]
	private void AddPlayer(int id)
	{
		int i = 0;
		while (i < this.players.Count)
		{
			if (!this.players[i].isAssigned)
			{
				this.players[i].isAssigned = true;
				this.players[i].actorID = id;
				this.players[i].currentAmount = 0;
				this.players[i].isLocalPlayer = true;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x000350C0 File Offset: 0x000332C0
	[PunRPC]
	private void RemovePlayer(int id)
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			if (1 == id)
			{
				this.ChangeTotalAmount(id, -this.players[i].currentAmount);
				this.players[i].isAssigned = false;
				this.players[i].actorID = 0;
				this.players[i].currentAmount = 0;
				return;
			}
		}
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x00035148 File Offset: 0x00033348
	public void UpdateTotalText()
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			if (this.players[i].isLocalPlayer)
			{
				this.totalText.text = (PlayerPrefs.GetInt(this.itemName + "Inventory") - this.players[i].currentAmount).ToString();
				return;
			}
		}
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x000351B9 File Offset: 0x000333B9
	public void ChangeTotalAmount(int actorID, int amount)
	{
		this.view.RPC("ChangeTotalAmountNetworked", RpcTarget.AllBufferedViaServer, new object[]
		{
			1,
			amount
		});
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x000351E4 File Offset: 0x000333E4
	[PunRPC]
	private void ChangeTotalAmountNetworked(int actorID, int amount)
	{
		if (actorID != 999)
		{
			if (amount == 1 && this.totalAmount >= this.maxAmount)
			{
				this.totalAmount = this.maxAmount;
				this.canChangeAmount = true;
				this.inventoryManager.UpdateText();
				return;
			}
			for (int i = 0; i < this.players.Count; i++)
			{
				if (1 == actorID)
				{
					this.players[i].currentAmount += amount;
				}
			}
		}
		this.totalAmount += amount;
		this.totalAmount = Mathf.Clamp(this.totalAmount, 0, this.maxAmount);
		this.UpdateTotalText();
		this.canChangeAmount = true;
		this.inventoryManager.UpdateText();
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x000352AC File Offset: 0x000334AC
	public void SaveItem()
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			if (this.players[i].isLocalPlayer)
			{
				PlayerPrefs.SetInt("current" + this.itemName + "Amount", this.players[i].currentAmount);
				break;
			}
		}
		PlayerPrefs.SetInt("total" + this.itemName + "Amount", this.totalAmount);
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x00035330 File Offset: 0x00033530
	public void LeftRoom(int actorID)
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			if (this.players[i].actorID == actorID)
			{
				this.view.RPC("RemovePlayer", RpcTarget.AllBuffered, new object[]
				{
					1
				});
			}
		}
	}

	// Token: 0x040008BC RID: 2236
	public string itemName;

	// Token: 0x040008BD RID: 2237
	public string localisedItemName;

	// Token: 0x040008BE RID: 2238
	[SerializeField]
	private int defaultAmount;

	// Token: 0x040008BF RID: 2239
	[HideInInspector]
	public int totalAmount;

	// Token: 0x040008C0 RID: 2240
	public int maxAmount;

	// Token: 0x040008C1 RID: 2241
	[SerializeField]
	private Text totalText;

	// Token: 0x040008C2 RID: 2242
	[HideInInspector]
	public PhotonView view;

	// Token: 0x040008C3 RID: 2243
	[HideInInspector]
	public bool canChangeAmount = true;

	// Token: 0x040008C4 RID: 2244
	[SerializeField]
	private InventoryManager inventoryManager;

	// Token: 0x040008C5 RID: 2245
	public List<InventoryItem.PlayerInventoryItem> players = new List<InventoryItem.PlayerInventoryItem>();

	// Token: 0x020004E3 RID: 1251
	public class PlayerInventoryItem
	{
		// Token: 0x0400231C RID: 8988
		public bool isAssigned;

		// Token: 0x0400231D RID: 8989
		public int actorID;

		// Token: 0x0400231E RID: 8990
		public int currentAmount;

		// Token: 0x0400231F RID: 8991
		public bool isLocalPlayer;
	}
}

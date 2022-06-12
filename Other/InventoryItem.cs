using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class InventoryItem : MonoBehaviour
{
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

	public void ChangeTotalAmount(int actorID, int amount)
	{
		this.view.RPC("ChangeTotalAmountNetworked", RpcTarget.AllBufferedViaServer, new object[]
		{
			1,
			amount
		});
	}

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

	public string itemName;

	public string localisedItemName;

	[SerializeField]
	private int defaultAmount;

	[HideInInspector]
	public int totalAmount;

	public int maxAmount;

	[SerializeField]
	private Text totalText;

	[HideInInspector]
	public PhotonView view;

	[HideInInspector]
	public bool canChangeAmount = true;

	[SerializeField]
	private InventoryManager inventoryManager;

	public List<InventoryItem.PlayerInventoryItem> players = new List<InventoryItem.PlayerInventoryItem>();

	public class PlayerInventoryItem
	{
		public bool isAssigned;

		public int actorID;

		public int currentAmount;

		public bool isLocalPlayer;
	}
}


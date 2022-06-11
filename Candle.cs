using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

// Token: 0x0200010C RID: 268
public class Candle : MonoBehaviour
{
	// Token: 0x06000765 RID: 1893 RVA: 0x0002BD80 File Offset: 0x00029F80
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x0002BD90 File Offset: 0x00029F90
	private void Start()
	{
		this.isOn = false;
		this.flame.SetActive(false);
		if (this.photonInteract == null)
		{
			base.GetComponent<PhotonObjectInteract>().AddPCSecondaryUseEvent(new UnityAction(this.SecondaryUse));
			return;
		}
		this.photonInteract.AddPCSecondaryUseEvent(new UnityAction(this.SecondaryUse));
	}

	// Token: 0x06000767 RID: 1895 RVA: 0x0002BDED File Offset: 0x00029FED
	public void Use()
	{
		this.isOn = !this.isOn;
        if (!PhotonNetwork.InRoom)
        {
			this.NetworkedUse(this.isOn);
        } else
        {
			this.view.RPC("NetworkedUse", RpcTarget.All, new object[]
		{
			this.isOn
		});
		}
	}

	// Token: 0x06000768 RID: 1896 RVA: 0x0002BE23 File Offset: 0x0002A023
	[PunRPC]
	private void NetworkedUse(bool _isOn)
	{
		base.StopCoroutine(this.CandleOffTimer());
		this.isOn = _isOn;
		this.flame.SetActive(this.isOn);
		if (this.view.IsMine)
		{
			base.StartCoroutine(this.CandleOffTimer());
		}
	}

	// Token: 0x06000769 RID: 1897 RVA: 0x0002BE63 File Offset: 0x0002A063
	private IEnumerator CandleOffTimer()
	{
		yield return new WaitForSeconds(UnityEngine.Random.Range(150f, 300f));
		if (!this.stayOn && this.isOn)
		{
			this.Use();
		}
		yield break;
	}

	// Token: 0x0600076A RID: 1898 RVA: 0x0002BE74 File Offset: 0x0002A074
	private void SecondaryUse()
	{
		if (this.isOn)
		{
			this.playerAim = GameController.instance.myPlayer.player.cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
			RaycastHit raycastHit;
			if (Physics.Raycast(this.playerAim, out raycastHit, this.grabDistance, this.mask))
			{
				if (raycastHit.collider.GetComponent<Candle>())
				{
					if (!raycastHit.collider.GetComponent<Candle>().isOn)
					{
						raycastHit.collider.GetComponent<Candle>().Use();
						return;
					}
				}
				else if (raycastHit.collider.GetComponent<WhiteSage>())
				{
					raycastHit.collider.GetComponent<WhiteSage>().Use();
				}
			}
		}
	}

	// Token: 0x0600076B RID: 1899 RVA: 0x0002BF3C File Offset: 0x0002A13C
	private void OnTriggerEnter(Collider other)
	{
		if (this.isOn)
		{
			return;
		}
		if (other.GetComponent<Lighter>())
		{
			if (other.GetComponent<Lighter>().isOn)
			{
				this.Use();
				return;
			}
		}
		else if (other.GetComponent<Candle>() && other.GetComponent<Candle>().isOn)
		{
			this.Use();
		}
	}

	// Token: 0x04000777 RID: 1911
	[SerializeField]
	private GameObject flame;

	// Token: 0x04000778 RID: 1912
	[HideInInspector]
	public PhotonView view;

	// Token: 0x04000779 RID: 1913
	[HideInInspector]
	public bool isOn;

	// Token: 0x0400077A RID: 1914
	public bool stayOn;

	// Token: 0x0400077B RID: 1915
	[SerializeField]
	private PhotonObjectInteract photonInteract;

	// Token: 0x0400077C RID: 1916
	[Header("PC")]
	private float grabDistance = 3f;

	// Token: 0x0400077D RID: 1917
	private Ray playerAim;

	// Token: 0x0400077E RID: 1918
	[SerializeField]
	private LayerMask mask;
}

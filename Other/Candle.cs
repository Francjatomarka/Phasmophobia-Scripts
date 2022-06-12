using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class Candle : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
	}

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

	private IEnumerator CandleOffTimer()
	{
		yield return new WaitForSeconds(UnityEngine.Random.Range(150f, 300f));
		if (!this.stayOn && this.isOn)
		{
			this.Use();
		}
		yield break;
	}

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

	[SerializeField]
	private GameObject flame;

	[HideInInspector]
	public PhotonView view;

	[HideInInspector]
	public bool isOn;

	public bool stayOn;

	[SerializeField]
	private PhotonObjectInteract photonInteract;

	[Header("PC")]
	private float grabDistance = 3f;

	private Ray playerAim;

	[SerializeField]
	private LayerMask mask;
}


using System;
using UnityEngine;
using Photon.Pun;

public class PlayerDiamond : MonoBehaviour
{
	// (get) Token: 0x060002AE RID: 686 RVA: 0x00011B72 File Offset: 0x0000FD72
	private PhotonView PhotonView
	{
		get
		{
			if (this.m_PhotonView == null)
			{
				this.m_PhotonView = base.transform.parent.GetComponent<PhotonView>();
			}
			return this.m_PhotonView;
		}
	}

	// (get) Token: 0x060002AF RID: 687 RVA: 0x00011B9E File Offset: 0x0000FD9E
	private Renderer DiamondRenderer
	{
		get
		{
			if (this.m_DiamondRenderer == null)
			{
				this.m_DiamondRenderer = base.GetComponentInChildren<Renderer>();
			}
			return this.m_DiamondRenderer;
		}
	}

	private void Start()
	{
		this.m_Height = this.HeightOffset;
		if (this.HeadTransform != null)
		{
			this.m_Height += this.HeadTransform.position.y;
		}
	}

	private void Update()
	{
		this.UpdateDiamondPosition();
		this.UpdateDiamondRotation();
		this.UpdateDiamondVisibility();
	}

	private void UpdateDiamondPosition()
	{
		Vector3 vector = Vector3.zero;
		if (this.HeadTransform != null)
		{
			vector = this.HeadTransform.position;
		}
		vector.y = this.m_Height;
		if (!float.IsNaN(vector.x) && !float.IsNaN(vector.z))
		{
			base.transform.position = Vector3.Lerp(base.transform.position, vector, Time.deltaTime * 10f);
		}
	}

	private void UpdateDiamondRotation()
	{
		this.m_Rotation += Time.deltaTime * 180f;
		this.m_Rotation %= 360f;
		base.transform.rotation = Quaternion.Euler(0f, this.m_Rotation, 0f);
	}

	private void UpdateDiamondVisibility()
	{
		this.DiamondRenderer.enabled = true;
		if (this.PhotonView == null || !this.PhotonView.IsMine)
		{
			this.DiamondRenderer.enabled = false;
		}
	}

	public Transform HeadTransform;

	public float HeightOffset = 0.5f;

	private PhotonView m_PhotonView;

	private Renderer m_DiamondRenderer;

	private float m_Rotation;

	private float m_Height;
}


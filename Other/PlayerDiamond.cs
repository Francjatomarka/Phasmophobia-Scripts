using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x02000078 RID: 120
public class PlayerDiamond : MonoBehaviour
{
	// Token: 0x1700002D RID: 45
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

	// Token: 0x1700002E RID: 46
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

	// Token: 0x060002B0 RID: 688 RVA: 0x00011BC0 File Offset: 0x0000FDC0
	private void Start()
	{
		this.m_Height = this.HeightOffset;
		if (this.HeadTransform != null)
		{
			this.m_Height += this.HeadTransform.position.y;
		}
	}

	// Token: 0x060002B1 RID: 689 RVA: 0x00011BF9 File Offset: 0x0000FDF9
	private void Update()
	{
		this.UpdateDiamondPosition();
		this.UpdateDiamondRotation();
		this.UpdateDiamondVisibility();
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x00011C10 File Offset: 0x0000FE10
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

	// Token: 0x060002B3 RID: 691 RVA: 0x00011C8C File Offset: 0x0000FE8C
	private void UpdateDiamondRotation()
	{
		this.m_Rotation += Time.deltaTime * 180f;
		this.m_Rotation %= 360f;
		base.transform.rotation = Quaternion.Euler(0f, this.m_Rotation, 0f);
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x00011CE3 File Offset: 0x0000FEE3
	private void UpdateDiamondVisibility()
	{
		this.DiamondRenderer.enabled = true;
		if (this.PhotonView == null || !this.PhotonView.IsMine)
		{
			this.DiamondRenderer.enabled = false;
		}
	}

	// Token: 0x040002F7 RID: 759
	public Transform HeadTransform;

	// Token: 0x040002F8 RID: 760
	public float HeightOffset = 0.5f;

	// Token: 0x040002F9 RID: 761
	private PhotonView m_PhotonView;

	// Token: 0x040002FA RID: 762
	private Renderer m_DiamondRenderer;

	// Token: 0x040002FB RID: 763
	private float m_Rotation;

	// Token: 0x040002FC RID: 764
	private float m_Height;
}

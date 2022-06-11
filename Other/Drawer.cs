using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x020000FE RID: 254
public class Drawer : MonoBehaviour
{
	// Token: 0x060006EC RID: 1772 RVA: 0x00028B18 File Offset: 0x00026D18
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.body = base.GetComponent<Rigidbody>();
		this.startPos = base.transform.localPosition;
		this.startWorldPos = base.transform.position;
		this.closed = true;
	}


	// Token: 0x060006ED RID: 1773 RVA: 0x00028B68 File Offset: 0x00026D68
	private void Start()
	{
		if (this.loopSource)
		{
			this.loopSource.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		}
		if (this.closedSource)
		{
			this.closedSource.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		}
	}

	// Token: 0x060006EE RID: 1774 RVA: 0x00028BDC File Offset: 0x00026DDC
	private void Update()
	{
		if (this.isZ)
		{
			if (base.transform.localPosition.z < this.startPos.z)
			{
				this.pos = base.transform.localPosition;
				this.pos.z = this.startPos.z;
				base.transform.localPosition = this.pos;
			}
		}
		else if (this.isX)
		{
			if (base.transform.localPosition.x < this.startPos.x)
			{
				this.pos = base.transform.localPosition;
				this.pos.x = this.startPos.x;
				base.transform.localPosition = this.pos;
			}
		}
		else if (this.isY && base.transform.localPosition.y < this.startPos.y)
		{
			this.pos = base.transform.localPosition;
			this.pos.y = this.startPos.y;
			base.transform.localPosition = this.pos;
		}
		if (Time.frameCount % 3 == 0)
		{
			if (!this.closed)
			{
				if (!this.loopSource.isPlaying)
				{
					this.loopSource.Play();
				}
				this.velocity = (base.transform.position - this.oldPos).magnitude / Time.deltaTime;
				this.oldPos = base.transform.position;
				this.loopSource.volume = this.velocity / 6f;
				return;
			}
			if (this.loopSource.isPlaying)
			{
				this.loopSource.Stop();
			}
		}
	}

	// Token: 0x060006EF RID: 1775 RVA: 0x00028DA4 File Offset: 0x00026FA4
	public void UnGrab()
	{
		Vector3 localPosition = base.transform.localPosition;
		if (Vector3.Distance(base.transform.position, this.startWorldPos) <= 0.03f)
		{
			if (this.isZ)
			{
				localPosition.z = 0f;
			}
			else if (this.isY)
			{
				localPosition.y = 0f;
			}
			else if (this.isX)
			{
				localPosition.z = 0f;
			}
			this.view.RPC("NetworkedPlayClosedSound", RpcTarget.All, Array.Empty<object>());
			base.transform.localPosition = localPosition;
		}
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x00028E3C File Offset: 0x0002703C
	public void Grab()
	{
		this.view.RPC("NetworkedGrab", RpcTarget.All, Array.Empty<object>());
	}

	// Token: 0x060006F1 RID: 1777 RVA: 0x00028E54 File Offset: 0x00027054
	[PunRPC]
	private void NetworkedGrab()
	{
		this.closed = false;
	}


	// Token: 0x060006F2 RID: 1778 RVA: 0x00028E60 File Offset: 0x00027060
	[PunRPC]
	private void NetworkedPlayClosedSound()
	{
		this.closed = true;
		if (this.closedSource.isPlaying)
		{
			return;
		}
		if (this.doorClosedClips.Length != 0)
		{
			this.closedSource.clip = this.doorClosedClips[UnityEngine.Random.Range(0, this.doorClosedClips.Length)];
			this.closedSource.Play();
			return;
		}
		Debug.LogError(base.gameObject.name + " needs a drawer closing audio clip");
	}

	// Token: 0x04000704 RID: 1796
	private Vector3 startPos;

	// Token: 0x04000705 RID: 1797
	public bool isX;

	// Token: 0x04000706 RID: 1798
	public bool isY;

	// Token: 0x04000707 RID: 1799
	public bool isZ = true;

	// Token: 0x04000708 RID: 1800
	private Vector3 pos;

	// Token: 0x04000709 RID: 1801
	private Rigidbody body;

	// Token: 0x0400070A RID: 1802
	private PhotonView view;

	// Token: 0x0400070B RID: 1803
	[HideInInspector]
	public bool closed;

	// Token: 0x0400070C RID: 1804
	[SerializeField]
	private AudioClip[] doorClosedClips;

	// Token: 0x0400070D RID: 1805
	[SerializeField]
	private AudioSource loopSource;

	// Token: 0x0400070E RID: 1806
	[SerializeField]
	private AudioSource closedSource;

	// Token: 0x0400070F RID: 1807
	private Vector3 startWorldPos;

	// Token: 0x04000710 RID: 1808
	private Vector3 oldPos;

	// Token: 0x04000711 RID: 1809
	private float velocity;
}

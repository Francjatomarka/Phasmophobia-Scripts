using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000065 RID: 101
[RequireComponent(typeof(PhotonView))]
public class CubeExtra : MonoBehaviourPunCallbacks, IPunObservable
{
	// Token: 0x06000242 RID: 578 RVA: 0x0000F47C File Offset: 0x0000D67C
	public void Awake()
	{
		this.latestCorrectPos = base.transform.position;
	}

	// Token: 0x06000243 RID: 579 RVA: 0x0000F490 File Offset: 0x0000D690
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			Vector3 localPosition = base.transform.localPosition;
			stream.Serialize(ref localPosition);
			return;
		}
		Vector3 zero = Vector3.zero;
		stream.Serialize(ref zero);
		double num = info.timestamp - this.lastTime;
		this.lastTime = info.timestamp;
		this.movementVector = (zero - this.latestCorrectPos) / (float)num;
		this.errorVector = (zero - base.transform.localPosition) / (float)num;
		this.latestCorrectPos = zero;
	}

	// Token: 0x06000244 RID: 580 RVA: 0x0000F524 File Offset: 0x0000D724
	public void Update()
	{
		if (base.photonView.IsMine)
		{
			return;
		}
		base.transform.localPosition += (this.movementVector + this.errorVector) * this.Factor * Time.deltaTime;
	}

	// Token: 0x0400027F RID: 639
	[Range(0.9f, 1.1f)]
	public float Factor = 0.98f;

	// Token: 0x04000280 RID: 640
	private Vector3 latestCorrectPos = Vector3.zero;

	// Token: 0x04000281 RID: 641
	private Vector3 movementVector = Vector3.zero;

	// Token: 0x04000282 RID: 642
	private Vector3 errorVector = Vector3.zero;

	// Token: 0x04000283 RID: 643
	private double lastTime;
}

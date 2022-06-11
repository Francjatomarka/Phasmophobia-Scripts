using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000067 RID: 103
[RequireComponent(typeof(PhotonView))]
public class CubeLerp : MonoBehaviourPunCallbacks, IPunObservable
{
	// Token: 0x06000249 RID: 585 RVA: 0x0000F861 File Offset: 0x0000DA61
	public void Start()
	{
		this.latestCorrectPos = base.transform.position;
		this.onUpdatePos = base.transform.position;
	}

	// Token: 0x0600024A RID: 586 RVA: 0x0000F888 File Offset: 0x0000DA88
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			Vector3 localPosition = base.transform.localPosition;
			Quaternion localRotation = base.transform.localRotation;
			stream.Serialize(ref localPosition);
			stream.Serialize(ref localRotation);
			return;
		}
		Vector3 zero = Vector3.zero;
		Quaternion identity = Quaternion.identity;
		stream.Serialize(ref zero);
		stream.Serialize(ref identity);
		this.latestCorrectPos = zero;
		this.onUpdatePos = base.transform.localPosition;
		this.fraction = 0f;
		base.transform.localRotation = identity;
	}

	// Token: 0x0600024B RID: 587 RVA: 0x0000F914 File Offset: 0x0000DB14
	public void Update()
	{
		if (base.photonView.IsMine)
		{
			return;
		}
		this.fraction += Time.deltaTime * 9f;
		base.transform.localPosition = Vector3.Lerp(this.onUpdatePos, this.latestCorrectPos, this.fraction);
	}

	// Token: 0x04000287 RID: 647
	private Vector3 latestCorrectPos;

	// Token: 0x04000288 RID: 648
	private Vector3 onUpdatePos;

	// Token: 0x04000289 RID: 649
	private float fraction;
}

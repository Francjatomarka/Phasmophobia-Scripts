using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000066 RID: 102
[RequireComponent(typeof(PhotonView))]
public class CubeInter : MonoBehaviourPunCallbacks, IPunObservable
{
	// Token: 0x06000246 RID: 582 RVA: 0x0000F5B0 File Offset: 0x0000D7B0
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			Vector3 localPosition = base.transform.localPosition;
			Quaternion localRotation = base.transform.localRotation;
			stream.Serialize(ref localPosition);
			stream.Serialize(ref localRotation);
			stream.SendNext(Environment.TickCount);
			return;
		}
		Vector3 zero = Vector3.zero;
		Quaternion identity = Quaternion.identity;
		stream.Serialize(ref zero);
		stream.Serialize(ref identity);
		for (int i = this.m_BufferedState.Length - 1; i >= 1; i--)
		{
			this.m_BufferedState[i] = this.m_BufferedState[i - 1];
		}
		CubeInter.State state;
		state.timestamp = info.timestamp;
		state.pos = zero;
		state.rot = identity;
		this.m_BufferedState[0] = state;
		this.m_TimestampCount = Mathf.Min(this.m_TimestampCount + 1, this.m_BufferedState.Length);
		for (int j = 0; j < this.m_TimestampCount - 1; j++)
		{
			if (this.m_BufferedState[j].timestamp < this.m_BufferedState[j + 1].timestamp)
			{
				Debug.Log("State inconsistent");
			}
		}
	}

	// Token: 0x06000247 RID: 583 RVA: 0x0000F6E0 File Offset: 0x0000D8E0
	public void Update()
	{
		if (base.photonView.IsMine || !PhotonNetwork.InRoom)
		{
			return;
		}
		double num = PhotonNetwork.Time - this.InterpolationDelay;
		if (this.m_BufferedState[0].timestamp > num)
		{
			for (int i = 0; i < this.m_TimestampCount; i++)
			{
				if (this.m_BufferedState[i].timestamp <= num || i == this.m_TimestampCount - 1)
				{
					CubeInter.State state = this.m_BufferedState[Mathf.Max(i - 1, 0)];
					CubeInter.State state2 = this.m_BufferedState[i];
					double num2 = state.timestamp - state2.timestamp;
					float t = 0f;
					if (num2 > 0.0001)
					{
						t = (float)((num - state2.timestamp) / num2);
					}
					base.transform.localPosition = Vector3.Lerp(state2.pos, state.pos, t);
					base.transform.localRotation = Quaternion.Slerp(state2.rot, state.rot, t);
					return;
				}
			}
			return;
		}
		CubeInter.State state3 = this.m_BufferedState[0];
		base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, state3.pos, Time.deltaTime * 20f);
		base.transform.localRotation = state3.rot;
	}

	// Token: 0x04000284 RID: 644
	private CubeInter.State[] m_BufferedState = new CubeInter.State[20];

	// Token: 0x04000285 RID: 645
	private int m_TimestampCount;

	// Token: 0x04000286 RID: 646
	public double InterpolationDelay = 0.15;

	// Token: 0x020004E1 RID: 1249
	internal struct State
	{
		// Token: 0x040023AE RID: 9134
		internal double timestamp;

		// Token: 0x040023AF RID: 9135
		internal Vector3 pos;

		// Token: 0x040023B0 RID: 9136
		internal Quaternion rot;
	}
}

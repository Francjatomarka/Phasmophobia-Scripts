using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x0200012A RID: 298
public class WiccanAltar : MonoBehaviour
{
	// Token: 0x04000854 RID: 2132
	[SerializeField]
	private Transform[] markers;

	// Token: 0x04000855 RID: 2133
	[SerializeField]
	private Candle candle_1;

	// Token: 0x04000856 RID: 2134
	[SerializeField]
	private Candle candle_2;

	// Token: 0x04000857 RID: 2135
	[SerializeField]
	private Light myLight;

	// Token: 0x04000858 RID: 2136
	[SerializeField]
	private AudioSource source;

	// Token: 0x04000859 RID: 2137
	[SerializeField]
	private ParticleSystem particles;

	// Token: 0x0400085B RID: 2139
	private PhotonObjectInteract photonInteract;

	// Token: 0x0400085C RID: 2140
	private Renderer rend;

	// Token: 0x0400085D RID: 2141
	[HideInInspector]
	public PhotonView view;

	// Token: 0x0400085E RID: 2142
	public static WiccanAltar instance;

	// Token: 0x0400085F RID: 2143
	private bool inUse;
}

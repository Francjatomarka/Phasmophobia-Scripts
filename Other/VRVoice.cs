using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x02000187 RID: 391
public class VRVoice : MonoBehaviour
{
	// Token: 0x06000B1C RID: 2844 RVA: 0x00046489 File Offset: 0x00044689
	private void Awake()
	{
		this.noise.gameObject.SetActive(false);
	}

	// Token: 0x06000B1D RID: 2845 RVA: 0x0004649C File Offset: 0x0004469C
	private void Start()
	{
		if (PhotonNetwork.InRoom)
		{
			if (GameController.instance != null )
			{
				base.enabled = false;
				base.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000B1E RID: 2846 RVA: 0x00046520 File Offset: 0x00044720
	private void Update()
	{
		if (!this.noise.gameObject.activeInHierarchy)
		{
			this.noise.gameObject.SetActive(true);
			if (!this.walkieTalkie.isOn)
			{
				this.noise.volume = 0.4f;
				return;
			}
		}
		if (this.noise.gameObject.activeInHierarchy)
		{
			this.noise.gameObject.SetActive(false);
		}
	}

	// Token: 0x04000BA4 RID: 2980
	[SerializeField]
	private Noise noise;

	// Token: 0x04000BA5 RID: 2981
	[SerializeField]
	private WalkieTalkie walkieTalkie;

}

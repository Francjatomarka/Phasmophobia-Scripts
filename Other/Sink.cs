using System;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

// Token: 0x02000106 RID: 262
public class Sink : MonoBehaviour
{
	// Token: 0x06000730 RID: 1840 RVA: 0x0002AD8B File Offset: 0x00028F8B
	private void Awake()
	{
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.view = base.GetComponent<PhotonView>();
		this.noise.gameObject.SetActive(false);
		this.evidence.enabled = false;
	}

	// Token: 0x06000731 RID: 1841 RVA: 0x0002ADC2 File Offset: 0x00028FC2
	private void Start()
	{
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
	}

	// Token: 0x06000732 RID: 1842 RVA: 0x0002ADDB File Offset: 0x00028FDB
	public void Use()
	{
		this.view.RPC("NetworkedUse", RpcTarget.All, Array.Empty<object>());
	}

	// Token: 0x06000733 RID: 1843 RVA: 0x0002ADF3 File Offset: 0x00028FF3
	public void SpawnDirtyWater()
	{
		this.view.RPC("SpawnDirtyWaterSync", RpcTarget.All, Array.Empty<object>());
	}

	// Token: 0x06000734 RID: 1844 RVA: 0x0002AE0C File Offset: 0x0002900C
	[PunRPC]
	private void SpawnDirtyWaterSync()
	{
		for (int i = 0; i < this.waterRends.Length; i++)
		{
			this.waterRends[i].material.color = new Color32(115, 80, 60, 180);
		}
		this.evidence.enabled = true;
	}

	// Token: 0x06000735 RID: 1845 RVA: 0x0002AE60 File Offset: 0x00029060
	[PunRPC]
	private void NetworkedUse()
	{
		this.waterIsOn = !this.waterIsOn;
		this.tapWater.SetActive(this.waterIsOn);
		if (this.waterIsOn)
		{
			this.source.Play();
			this.noise.gameObject.SetActive(true);
			return;
		}
		this.source.Stop();
		this.noise.gameObject.SetActive(false);
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x0002AED0 File Offset: 0x000290D0
	private void Update()
	{
		if (this.waterIsOn && this.timer > 0f)
		{
			this.timer -= Time.deltaTime;
			this.water.localPosition = Vector3.MoveTowards(this.water.localPosition, this.target.localPosition, 0.01f * Time.deltaTime);
		}
	}

	// Token: 0x04000745 RID: 1861
	private PhotonObjectInteract photonInteract;

	// Token: 0x04000746 RID: 1862
	private PhotonView view;

	// Token: 0x04000747 RID: 1863
	[SerializeField]
	private Transform water;

	// Token: 0x04000748 RID: 1864
	[SerializeField]
	private Transform target;

	// Token: 0x04000749 RID: 1865
	private float timer = 20f;

	// Token: 0x0400074A RID: 1866
	private bool waterIsOn;

	// Token: 0x0400074B RID: 1867
	[SerializeField]
	private AudioSource source;

	// Token: 0x0400074C RID: 1868
	[SerializeField]
	private GameObject tapWater;

	// Token: 0x0400074D RID: 1869
	[SerializeField]
	private Noise noise;

	// Token: 0x0400074E RID: 1870
	[SerializeField]
	private MeshRenderer[] waterRends;

	// Token: 0x0400074F RID: 1871
	[SerializeField]
	private Evidence evidence;
}

using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000115 RID: 277
[RequireComponent(typeof(PhotonView))]
public class HouseAmbientSoundController : MonoBehaviourPunCallbacks
{
	// Token: 0x0600076E RID: 1902 RVA: 0x0002B570 File Offset: 0x00029770
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.col = base.GetComponent<BoxCollider>();
	}

	// Token: 0x0600076F RID: 1903 RVA: 0x0002B58C File Offset: 0x0002978C
	private void Update()
	{
		if (!PhotonNetwork.InRoom)
		{
			return;
		}
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		this.timer -= Time.deltaTime;
		if (this.timer < 0f)
		{
			if (!this.src)
			{
				Vector3 vector = new Vector3(UnityEngine.Random.Range(this.col.bounds.min.x, this.col.bounds.max.x), base.transform.position.y, UnityEngine.Random.Range(this.col.bounds.min.z, this.col.bounds.max.z));
				this.view.RPC("PlaySound", RpcTarget.All, new object[]
				{
					vector,
					UnityEngine.Random.Range(0, this.clips.Count)
				});
			}
			this.timer = UnityEngine.Random.Range(5f, 20f);
		}
	}

	// Token: 0x06000770 RID: 1904 RVA: 0x0002B6AC File Offset: 0x000298AC
	[PunRPC]
	private void PlaySound(Vector3 pos, int clipID)
	{
		if (GameController.instance.myPlayer != null && GameController.instance.myPlayer.player.isDead)
		{
			return;
		}
		if (this.roomSpecific)
		{
			if (LevelController.instance.currentPlayerRoom == this.specificRoom)
			{
				ObjectPooler.instance.SpawnFromPool("Noise", pos, Quaternion.identity).GetComponent<Noise>().PlaySound(this.clips[clipID], 0.6f);
				return;
			}
		}
		else
		{
			ObjectPooler.instance.SpawnFromPool("Noise", pos, Quaternion.identity).GetComponent<Noise>().PlaySound(this.clips[clipID], 0.15f);
		}
	}

	// Token: 0x04000725 RID: 1829
	private BoxCollider col;

	// Token: 0x04000726 RID: 1830
	private float timer;

	// Token: 0x04000727 RID: 1831
	public List<AudioClip> clips = new List<AudioClip>();

	// Token: 0x04000728 RID: 1832
	private AudioSource src;

	// Token: 0x04000729 RID: 1833
	private PhotonView view;

	// Token: 0x0400072A RID: 1834
	[SerializeField]
	private LevelRoom specificRoom;

	// Token: 0x0400072B RID: 1835
	[SerializeField]
	private bool roomSpecific;
}

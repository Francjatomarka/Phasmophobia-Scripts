using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x020000AA RID: 170
public class GhostInfo : MonoBehaviourPunCallbacks
{
	// Token: 0x06000511 RID: 1297 RVA: 0x0001C060 File Offset: 0x0001A260
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x0001C06E File Offset: 0x0001A26E
	private void Update()
	{
		if (this.activityMultiplier > 0f)
		{
			this.activityMultiplier -= Time.deltaTime / 2f;
		}
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x0001C095 File Offset: 0x0001A295
	[PunRPC]
	public void SyncValues(byte[] traits)
	{
		this.ghostTraits = Serialisation.DeserialiseStruct<GhostTraits>(traits);
		this.favouriteRoom = LevelController.instance.rooms[this.ghostTraits.favouriteRoomID];
	}

	// Token: 0x040004D2 RID: 1234
	[HideInInspector]
	public PhotonView view;

	// Token: 0x040004D3 RID: 1235
	[HideInInspector]
	public GhostTraits ghostTraits;

	// Token: 0x040004D4 RID: 1236
	[HideInInspector]
	public LevelRoom favouriteRoom;

	// Token: 0x040004D5 RID: 1237
	[HideInInspector]
	public float activityMultiplier;
}

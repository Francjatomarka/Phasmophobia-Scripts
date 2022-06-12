using System;
using Photon.Pun;
using UnityEngine;

public class GhostInfo : MonoBehaviourPunCallbacks
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
	}

	private void Update()
	{
		if (this.activityMultiplier > 0f)
		{
			this.activityMultiplier -= Time.deltaTime / 2f;
		}
	}

	[PunRPC]
	public void SyncValues(byte[] traits)
	{
		this.ghostTraits = Serialisation.DeserialiseStruct<GhostTraits>(traits);
		this.favouriteRoom = LevelController.instance.rooms[this.ghostTraits.favouriteRoomID];
	}

	[HideInInspector]
	public PhotonView view;

	[HideInInspector]
	public GhostTraits ghostTraits;

	[HideInInspector]
	public LevelRoom favouriteRoom;

	[HideInInspector]
	public float activityMultiplier;
}


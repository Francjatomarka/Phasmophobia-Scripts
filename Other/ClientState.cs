using System;

// Token: 0x0200005A RID: 90
public enum ClientState
{
	// Token: 0x040002D4 RID: 724
	Uninitialized,
	// Token: 0x040002D5 RID: 725
	PeerCreated,
	// Token: 0x040002D6 RID: 726
	Queued,
	// Token: 0x040002D7 RID: 727
	Authenticated,
	// Token: 0x040002D8 RID: 728
	JoinedLobby,
	// Token: 0x040002D9 RID: 729
	DisconnectingFromMasterserver,
	// Token: 0x040002DA RID: 730
	ConnectingToGameserver,
	// Token: 0x040002DB RID: 731
	ConnectedToGameserver,
	// Token: 0x040002DC RID: 732
	Joining,
	// Token: 0x040002DD RID: 733
	Joined,
	// Token: 0x040002DE RID: 734
	Leaving,
	// Token: 0x040002DF RID: 735
	DisconnectingFromGameserver,
	// Token: 0x040002E0 RID: 736
	ConnectingToMasterserver,
	// Token: 0x040002E1 RID: 737
	QueuedComingFromGameserver,
	// Token: 0x040002E2 RID: 738
	Disconnecting,
	// Token: 0x040002E3 RID: 739
	Disconnected,
	// Token: 0x040002E4 RID: 740
	ConnectedToMaster,
	// Token: 0x040002E5 RID: 741
	ConnectingToNameServer,
	// Token: 0x040002E6 RID: 742
	ConnectedToNameServer,
	// Token: 0x040002E7 RID: 743
	DisconnectingFromNameServer,
	// Token: 0x040002E8 RID: 744
	Authenticating
}

using System;

// Token: 0x0200004C RID: 76
public class OperationCode
{
	// Token: 0x04000272 RID: 626
	[Obsolete("Exchanging encrpytion keys is done internally in the lib now. Don't expect this operation-result.")]
	public const byte ExchangeKeysForEncryption = 250;

	// Token: 0x04000273 RID: 627
	[Obsolete]
	public const byte Join = 255;

	// Token: 0x04000274 RID: 628
	public const byte AuthenticateOnce = 231;

	// Token: 0x04000275 RID: 629
	public const byte Authenticate = 230;

	// Token: 0x04000276 RID: 630
	public const byte JoinLobby = 229;

	// Token: 0x04000277 RID: 631
	public const byte LeaveLobby = 228;

	// Token: 0x04000278 RID: 632
	public const byte CreateGame = 227;

	// Token: 0x04000279 RID: 633
	public const byte JoinGame = 226;

	// Token: 0x0400027A RID: 634
	public const byte JoinRandomGame = 225;

	// Token: 0x0400027B RID: 635
	public const byte Leave = 254;

	// Token: 0x0400027C RID: 636
	public const byte RaiseEvent = 253;

	// Token: 0x0400027D RID: 637
	public const byte SetProperties = 252;

	// Token: 0x0400027E RID: 638
	public const byte GetProperties = 251;

	// Token: 0x0400027F RID: 639
	public const byte ChangeGroups = 248;

	// Token: 0x04000280 RID: 640
	public const byte FindFriends = 222;

	// Token: 0x04000281 RID: 641
	public const byte GetLobbyStats = 221;

	// Token: 0x04000282 RID: 642
	public const byte GetRegions = 220;

	// Token: 0x04000283 RID: 643
	public const byte WebRpc = 219;

	// Token: 0x04000284 RID: 644
	public const byte ServerSettings = 218;

	// Token: 0x04000285 RID: 645
	public const byte GetGameList = 217;
}

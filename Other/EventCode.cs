using System;

// Token: 0x0200004A RID: 74
public class EventCode
{
	// Token: 0x04000221 RID: 545
	public const byte GameList = 230;

	// Token: 0x04000222 RID: 546
	public const byte GameListUpdate = 229;

	// Token: 0x04000223 RID: 547
	public const byte QueueState = 228;

	// Token: 0x04000224 RID: 548
	public const byte Match = 227;

	// Token: 0x04000225 RID: 549
	public const byte AppStats = 226;

	// Token: 0x04000226 RID: 550
	public const byte LobbyStats = 224;

	// Token: 0x04000227 RID: 551
	[Obsolete("TCP routing was removed after becoming obsolete.")]
	public const byte AzureNodeInfo = 210;

	// Token: 0x04000228 RID: 552
	public const byte Join = 255;

	// Token: 0x04000229 RID: 553
	public const byte Leave = 254;

	// Token: 0x0400022A RID: 554
	public const byte PropertiesChanged = 253;

	// Token: 0x0400022B RID: 555
	[Obsolete("Use PropertiesChanged now.")]
	public const byte SetProperties = 253;

	// Token: 0x0400022C RID: 556
	public const byte ErrorInfo = 251;

	// Token: 0x0400022D RID: 557
	public const byte CacheSliceChanged = 250;

	// Token: 0x0400022E RID: 558
	public const byte AuthEvent = 223;
}

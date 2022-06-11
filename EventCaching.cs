using System;

// Token: 0x02000050 RID: 80
public enum EventCaching : byte
{
	// Token: 0x04000294 RID: 660
	DoNotCache,
	// Token: 0x04000295 RID: 661
	[Obsolete]
	MergeCache,
	// Token: 0x04000296 RID: 662
	[Obsolete]
	ReplaceCache,
	// Token: 0x04000297 RID: 663
	[Obsolete]
	RemoveCache,
	// Token: 0x04000298 RID: 664
	AddToRoomCache,
	// Token: 0x04000299 RID: 665
	AddToRoomCacheGlobal,
	// Token: 0x0400029A RID: 666
	RemoveFromRoomCache,
	// Token: 0x0400029B RID: 667
	RemoveFromRoomCacheForActorsLeft,
	// Token: 0x0400029C RID: 668
	SliceIncreaseIndex = 10,
	// Token: 0x0400029D RID: 669
	SliceSetIndex,
	// Token: 0x0400029E RID: 670
	SlicePurgeIndex,
	// Token: 0x0400029F RID: 671
	SlicePurgeUpToIndex
}

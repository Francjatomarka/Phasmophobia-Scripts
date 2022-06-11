using System;
using UnityEngine;

// Token: 0x020000D2 RID: 210
[SerializeField]
public struct GhostTraits
{
	// Token: 0x04000593 RID: 1427
	public GhostTraits.Type ghostType;

	// Token: 0x04000594 RID: 1428
	public LevelController.Person victim;

	// Token: 0x04000595 RID: 1429
	public int ghostAge;

	// Token: 0x04000596 RID: 1430
	public bool isMale;

	// Token: 0x04000597 RID: 1431
	public string ghostName;

	// Token: 0x04000598 RID: 1432
	public bool isShy;

	// Token: 0x04000599 RID: 1433
	public int deathLength;

	// Token: 0x0400059A RID: 1434
	public int favouriteRoomID;

	// Token: 0x020004AE RID: 1198
	public enum Type
	{
		// Token: 0x0400222C RID: 8748
		none,
		// Token: 0x0400222D RID: 8749
		Spirit,
		// Token: 0x0400222E RID: 8750
		Wraith,
		// Token: 0x0400222F RID: 8751
		Phantom,
		// Token: 0x04002230 RID: 8752
		Poltergeist,
		// Token: 0x04002231 RID: 8753
		Banshee,
		// Token: 0x04002232 RID: 8754
		Jinn,
		// Token: 0x04002233 RID: 8755
		Mare,
		// Token: 0x04002234 RID: 8756
		Revenant,
		// Token: 0x04002235 RID: 8757
		Shade,
		// Token: 0x04002236 RID: 8758
		Demon,
		// Token: 0x04002237 RID: 8759
		Yurei,
		// Token: 0x04002238 RID: 8760
		Oni
	}
}

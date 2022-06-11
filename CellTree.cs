using System;

// Token: 0x02000085 RID: 133
public class CellTree
{
	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x0600042A RID: 1066 RVA: 0x00017BC9 File Offset: 0x00015DC9
	// (set) Token: 0x0600042B RID: 1067 RVA: 0x00017BD1 File Offset: 0x00015DD1
	public CellTreeNode RootNode { get; private set; }

	// Token: 0x0600042C RID: 1068 RVA: 0x000086EE File Offset: 0x000068EE
	public CellTree()
	{
	}

	// Token: 0x0600042D RID: 1069 RVA: 0x00017BDA File Offset: 0x00015DDA
	public CellTree(CellTreeNode root)
	{
		this.RootNode = root;
	}
}

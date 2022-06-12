using System;

public class CellTree
{
	// (get) Token: 0x0600042A RID: 1066 RVA: 0x00017BC9 File Offset: 0x00015DC9
	// (set) Token: 0x0600042B RID: 1067 RVA: 0x00017BD1 File Offset: 0x00015DD1
	public CellTreeNode RootNode { get; private set; }

	public CellTree()
	{
	}

	public CellTree(CellTreeNode root)
	{
		this.RootNode = root;
	}
}


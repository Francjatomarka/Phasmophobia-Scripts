using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000086 RID: 134
public class CellTreeNode
{
	// Token: 0x0600042E RID: 1070 RVA: 0x000086EE File Offset: 0x000068EE
	public CellTreeNode()
	{
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x00017BE9 File Offset: 0x00015DE9
	public CellTreeNode(byte id, CellTreeNode.ENodeType nodeType, CellTreeNode parent)
	{
		this.Id = id;
		this.NodeType = nodeType;
		this.Parent = parent;
	}

	// Token: 0x06000430 RID: 1072 RVA: 0x00017C06 File Offset: 0x00015E06
	public void AddChild(CellTreeNode child)
	{
		if (this.Childs == null)
		{
			this.Childs = new List<CellTreeNode>(1);
		}
		this.Childs.Add(child);
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x00003D4C File Offset: 0x00001F4C
	public void Draw()
	{
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x00017C28 File Offset: 0x00015E28
	public void GetActiveCells(List<byte> activeCells, bool yIsUpAxis, Vector3 position)
	{
		if (this.NodeType != CellTreeNode.ENodeType.Leaf)
		{
			using (List<CellTreeNode>.Enumerator enumerator = this.Childs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CellTreeNode cellTreeNode = enumerator.Current;
					cellTreeNode.GetActiveCells(activeCells, yIsUpAxis, position);
				}
				return;
			}
		}
		if (this.IsPointNearCell(yIsUpAxis, position))
		{
			if (this.IsPointInsideCell(yIsUpAxis, position))
			{
				activeCells.Insert(0, this.Id);
				for (CellTreeNode parent = this.Parent; parent != null; parent = parent.Parent)
				{
					activeCells.Insert(0, parent.Id);
				}
				return;
			}
			activeCells.Add(this.Id);
		}
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x00017CD4 File Offset: 0x00015ED4
	public bool IsPointInsideCell(bool yIsUpAxis, Vector3 point)
	{
		if (point.x < this.TopLeft.x || point.x > this.BottomRight.x)
		{
			return false;
		}
		if (yIsUpAxis)
		{
			if (point.y >= this.TopLeft.y && point.y <= this.BottomRight.y)
			{
				return true;
			}
		}
		else if (point.z >= this.TopLeft.z && point.z <= this.BottomRight.z)
		{
			return true;
		}
		return false;
	}

	// Token: 0x06000434 RID: 1076 RVA: 0x00017D60 File Offset: 0x00015F60
	public bool IsPointNearCell(bool yIsUpAxis, Vector3 point)
	{
		if (this.maxDistance == 0f)
		{
			this.maxDistance = (this.Size.x + this.Size.y + this.Size.z) / 2f;
		}
		return (point - this.Center).sqrMagnitude <= this.maxDistance * this.maxDistance;
	}

	// Token: 0x04000443 RID: 1091
	public byte Id;

	// Token: 0x04000444 RID: 1092
	public Vector3 Center;

	// Token: 0x04000445 RID: 1093
	public Vector3 Size;

	// Token: 0x04000446 RID: 1094
	public Vector3 TopLeft;

	// Token: 0x04000447 RID: 1095
	public Vector3 BottomRight;

	// Token: 0x04000448 RID: 1096
	public CellTreeNode.ENodeType NodeType;

	// Token: 0x04000449 RID: 1097
	public CellTreeNode Parent;

	// Token: 0x0400044A RID: 1098
	public List<CellTreeNode> Childs;

	// Token: 0x0400044B RID: 1099
	private float maxDistance;

	// Token: 0x0200049B RID: 1179
	public enum ENodeType
	{
		// Token: 0x040021D7 RID: 8663
		Root,
		// Token: 0x040021D8 RID: 8664
		Node,
		// Token: 0x040021D9 RID: 8665
		Leaf
	}
}

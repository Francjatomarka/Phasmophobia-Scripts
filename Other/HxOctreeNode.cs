using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000009 RID: 9
[Serializable]
public class HxOctreeNode<T>
{
	// Token: 0x17000002 RID: 2
	// (get) Token: 0x0600004C RID: 76 RVA: 0x00003338 File Offset: 0x00001538
	// (set) Token: 0x0600004D RID: 77 RVA: 0x00003340 File Offset: 0x00001540
	public Vector3 Origin { get; private set; }

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x0600004E RID: 78 RVA: 0x00003349 File Offset: 0x00001549
	// (set) Token: 0x0600004F RID: 79 RVA: 0x00003351 File Offset: 0x00001551
	public float Size { get; private set; }

	// Token: 0x06000050 RID: 80 RVA: 0x0000335A File Offset: 0x0000155A
	public HxOctreeNode(float size, float overlap, float minSize, Vector3 origin, HxOctreeNode<T> parent)
	{
		int idCtr = HxOctreeNode<T>._idCtr;
		HxOctreeNode<T>._idCtr = idCtr + 1;
		this.ID = idCtr;
		this.Init(size, overlap, minSize, origin, parent);
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00003390 File Offset: 0x00001590
	private void Init(float size, float overlap, float minSize, Vector3 origin, HxOctreeNode<T> parent)
	{
		this.Parent = parent;
		this.Size = size;
		this.MinSize = minSize;
		this.Overlap = overlap;
		this.Origin = origin;
		this.SizeWithOverlap = (1f + this.Overlap) * this.Size;
		Vector3 b = new Vector3(this.SizeWithOverlap, this.SizeWithOverlap, this.SizeWithOverlap) / 2f;
		this.BoundsMin = this.Origin - b;
		this.BoundsMax = this.Origin + b;
		Vector3 b2 = Vector3.one * (this.Size / 2f) * (1f + this.Overlap) / 2f;
		float d = this.Size / 4f;
		this.ChildrenBoundsMin = new Vector3[8];
		this.ChildrenBoundsMax = new Vector3[8];
		Vector3 a = this.Origin + new Vector3(-1f, 1f, -1f) * d;
		this.ChildrenBoundsMin[0] = a - b2;
		this.ChildrenBoundsMax[0] = a + b2;
		a = this.Origin + new Vector3(1f, 1f, -1f) * d;
		this.ChildrenBoundsMin[1] = a - b2;
		this.ChildrenBoundsMax[1] = a + b2;
		a = this.Origin + new Vector3(-1f, 1f, 1f) * d;
		this.ChildrenBoundsMin[2] = a - b2;
		this.ChildrenBoundsMax[2] = a + b2;
		a = this.Origin + new Vector3(1f, 1f, 1f) * d;
		this.ChildrenBoundsMin[3] = a - b2;
		this.ChildrenBoundsMax[3] = a + b2;
		a = this.Origin + new Vector3(-1f, -1f, -1f) * d;
		this.ChildrenBoundsMin[4] = a - b2;
		this.ChildrenBoundsMax[4] = a + b2;
		a = this.Origin + new Vector3(1f, -1f, -1f) * d;
		this.ChildrenBoundsMin[5] = a - b2;
		this.ChildrenBoundsMax[5] = a + b2;
		a = this.Origin + new Vector3(-1f, -1f, 1f) * d;
		this.ChildrenBoundsMin[6] = a - b2;
		this.ChildrenBoundsMax[6] = a + b2;
		a = this.Origin + new Vector3(1f, -1f, 1f) * d;
		this.ChildrenBoundsMin[7] = a - b2;
		this.ChildrenBoundsMax[7] = a + b2;
	}

	// Token: 0x06000052 RID: 82 RVA: 0x000036D8 File Offset: 0x000018D8
	public void Add(HxOctreeNode<T>.NodeObject node)
	{
		if (this.Objects.Count < 8 || this.Size < this.MinSize * 2f)
		{
			node.Node = this;
			this.Objects.Add(node);
			return;
		}
		int num;
		if (this.Children == null)
		{
			float size = this.Size / 2f;
			float d = this.Size / 4f;
			this.Children = new HxOctreeNode<T>[8];
			this.Children[0] = new HxOctreeNode<T>(size, this.Overlap, this.MinSize, this.Origin + new Vector3(-1f, 1f, -1f) * d, this);
			this.Children[1] = new HxOctreeNode<T>(size, this.Overlap, this.MinSize, this.Origin + new Vector3(1f, 1f, -1f) * d, this);
			this.Children[2] = new HxOctreeNode<T>(size, this.Overlap, this.MinSize, this.Origin + new Vector3(-1f, 1f, 1f) * d, this);
			this.Children[3] = new HxOctreeNode<T>(size, this.Overlap, this.MinSize, this.Origin + new Vector3(1f, 1f, 1f) * d, this);
			this.Children[4] = new HxOctreeNode<T>(size, this.Overlap, this.MinSize, this.Origin + new Vector3(-1f, -1f, -1f) * d, this);
			this.Children[5] = new HxOctreeNode<T>(size, this.Overlap, this.MinSize, this.Origin + new Vector3(1f, -1f, -1f) * d, this);
			this.Children[6] = new HxOctreeNode<T>(size, this.Overlap, this.MinSize, this.Origin + new Vector3(-1f, -1f, 1f) * d, this);
			this.Children[7] = new HxOctreeNode<T>(size, this.Overlap, this.MinSize, this.Origin + new Vector3(1f, -1f, 1f) * d, this);
			for (int i = this.Objects.Count - 1; i >= 0; i--)
			{
				HxOctreeNode<T>.NodeObject nodeObject = this.Objects[i];
				num = this.OctantIndex(nodeObject.Center);
				if (HxOctreeNode<T>.BoundsContains(this.Children[num].BoundsMin, this.Children[num].BoundsMax, nodeObject.BoundsMin, nodeObject.BoundsMax))
				{
					this.Children[num].Add(nodeObject);
					this.Objects.Remove(nodeObject);
				}
			}
		}
		num = this.OctantIndex(node.Center);
		if (HxOctreeNode<T>.BoundsContains(this.Children[num].BoundsMin, this.Children[num].BoundsMax, node.BoundsMin, node.BoundsMax))
		{
			this.Children[num].Add(node);
			return;
		}
		node.Node = this;
		this.Objects.Add(node);
	}

	// Token: 0x06000053 RID: 83 RVA: 0x00003A28 File Offset: 0x00001C28
	public bool Remove(T value)
	{
		bool flag = false;
		for (int i = 0; i < this.Objects.Count; i++)
		{
			if (this.Objects[i].Value.Equals(value))
			{
				flag = this.Objects.Remove(this.Objects[i]);
				break;
			}
		}
		if (!flag && this.Children != null)
		{
			for (int j = 0; j < 8; j++)
			{
				if (this.Children[j].Remove(value))
				{
					flag = true;
					break;
				}
			}
		}
		if (flag && this.Children != null)
		{
			int num = this.Objects.Count;
			if (this.Children != null)
			{
				for (int k = 0; k < 8; k++)
				{
					HxOctreeNode<T> hxOctreeNode = this.Children[k];
					if (hxOctreeNode.Children != null)
					{
						return flag;
					}
					num += hxOctreeNode.Objects.Count;
				}
			}
			if (num <= 8)
			{
				for (int l = 0; l < 8; l++)
				{
					List<HxOctreeNode<T>.NodeObject> objects = this.Children[l].Objects;
					for (int m = 0; m < objects.Count; m++)
					{
						objects[m].Node = this;
						this.Objects.Add(objects[m]);
					}
				}
				this.Children = null;
			}
		}
		return flag;
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00003B78 File Offset: 0x00001D78
	public void GetObjects(Vector3 boundsMin, Vector3 boundsMax, List<T> items)
	{
		if (!HxOctreeNode<T>.BoundsIntersects(boundsMin, boundsMax, this.BoundsMin, this.BoundsMax))
		{
			return;
		}
		for (int i = 0; i < this.Objects.Count; i++)
		{
			if (HxOctreeNode<T>.BoundsIntersects(boundsMin, boundsMax, this.Objects[i].BoundsMin, this.Objects[i].BoundsMax))
			{
				items.Add(this.Objects[i].Value);
			}
		}
		if (this.Children != null)
		{
			for (int j = 0; j < 8; j++)
			{
				this.Children[j].GetObjects(boundsMin, boundsMax, items);
			}
		}
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00003C18 File Offset: 0x00001E18
	public void GetObjects2(Vector3 boundsMin, Vector3 boundsMax, List<T> items)
	{
		if (!HxOctreeNode<T>.BoundsIntersects(boundsMin, boundsMax, this.BoundsMin, this.BoundsMax))
		{
			return;
		}
		if (HxOctreeNode<T>.BoundsContains(boundsMin, boundsMax, this.BoundsMin, this.BoundsMax))
		{
			this.addAllObjectsToList(items);
			return;
		}
		for (int i = 0; i < this.Objects.Count; i++)
		{
			if (HxOctreeNode<T>.BoundsIntersects(this.Objects[i].BoundsMin, this.Objects[i].BoundsMax, boundsMin, boundsMax))
			{
				items.Add(this.Objects[i].Value);
			}
		}
		if (this.Children != null)
		{
			for (int j = 0; j < 8; j++)
			{
				this.Children[j].GetObjects2(boundsMin, boundsMax, items);
			}
		}
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00003CD4 File Offset: 0x00001ED4
	public void GetObjects2BoundsPlane(ref Plane[] planes, Vector3 boundsMin, Vector3 boundsMax, List<T> items)
	{
		if (!HxOctreeNode<T>.BoundsIntersects(boundsMin, boundsMax, this.BoundsMin, this.BoundsMax))
		{
			return;
		}
		if (HxOctreeNode<T>.BoundsContains(boundsMin, boundsMax, this.BoundsMin, this.BoundsMax) && this.BoundsInPlanes(boundsMin, boundsMax, ref planes) == 2)
		{
			this.addAllObjectsToList(items);
			return;
		}
		for (int i = 0; i < this.Objects.Count; i++)
		{
			if (HxOctreeNode<T>.BoundsIntersects(this.Objects[i].BoundsMin, this.Objects[i].BoundsMax, boundsMin, boundsMax) && this.BoundsInPlanes(this.Objects[i].BoundsMin, this.Objects[i].BoundsMax, ref planes) >= 1)
			{
				items.Add(this.Objects[i].Value);
			}
		}
		if (this.Children != null)
		{
			for (int j = 0; j < 8; j++)
			{
				this.Children[j].GetObjects2BoundsPlane(ref planes, boundsMin, boundsMax, items);
			}
		}
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00003DD0 File Offset: 0x00001FD0
	private void DrawBounds(Vector3 min, Vector3 max)
	{
		Debug.DrawLine(min, new Vector3(min.x, min.y, max.z), Color.red);
		Debug.DrawLine(min, new Vector3(min.x, max.y, min.z), Color.red);
		Debug.DrawLine(min, new Vector3(max.x, min.y, min.z), Color.red);
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00003E44 File Offset: 0x00002044
	private int BoundsInPlanes(Vector3 min, Vector3 max, ref Plane[] planes)
	{
		int result = 2;
		for (int i = 0; i < planes.Length; i++)
		{
			if (planes[i].GetDistanceToPoint(this.GetVertexP(min, max, planes[i].normal)) < 0f)
			{
				return 0;
			}
			if (planes[i].GetDistanceToPoint(this.GetVertexN(min, max, planes[i].normal)) < 0f)
			{
				result = 1;
			}
		}
		return result;
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00003EBC File Offset: 0x000020BC
	private bool ObjectInPlanes(Vector3 min, Vector3 max, ref Plane[] planes)
	{
		for (int i = 0; i < planes.Length; i++)
		{
			if (!planes[i].GetSide(this.GetVertexP(min, max, planes[i].normal)))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00003F00 File Offset: 0x00002100
	private Vector3 GetVertexP(Vector3 min, Vector3 max, Vector3 normal)
	{
		if (normal.x > 0f)
		{
			min.x = max.x;
		}
		if (normal.y > 0f)
		{
			min.y = max.y;
		}
		if (normal.z > 0f)
		{
			min.z = max.z;
		}
		return min;
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00003F5C File Offset: 0x0000215C
	private Vector3 GetVertexN(Vector3 min, Vector3 max, Vector3 normal)
	{
		if (normal.x > 0f)
		{
			max.x = min.x;
		}
		if (normal.y > 0f)
		{
			max.y = min.y;
		}
		if (normal.z > 0f)
		{
			max.z = min.z;
		}
		return max;
	}

	// Token: 0x0600005C RID: 92 RVA: 0x00003FB8 File Offset: 0x000021B8
	private void addAllObjectsToList(List<T> items)
	{
		for (int i = 0; i < this.Objects.Count; i++)
		{
			items.Add(this.Objects[i].Value);
		}
		if (this.Children != null)
		{
			for (int j = 0; j < 8; j++)
			{
				this.Children[j].addAllObjectsToList(items);
			}
		}
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00004014 File Offset: 0x00002214
	private void addAllObjectsToList(List<T> items, ref Vector3 min, ref Vector3 max)
	{
		for (int i = 0; i < this.Objects.Count; i++)
		{
			items.Add(this.Objects[i].Value);
			min = new Vector3(Mathf.Min(min.x, this.Objects[i].BoundsMin.x), Mathf.Min(min.y, this.Objects[i].BoundsMin.y), Mathf.Min(min.z, this.Objects[i].BoundsMin.z));
			max = new Vector3(Mathf.Max(max.x, this.Objects[i].BoundsMax.x), Mathf.Max(max.y, this.Objects[i].BoundsMax.y), Mathf.Max(max.z, this.Objects[i].BoundsMax.z));
		}
		if (this.Children != null)
		{
			for (int j = 0; j < 8; j++)
			{
				this.Children[j].addAllObjectsToList(items, ref min, ref max);
			}
		}
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00004154 File Offset: 0x00002354
	public HxOctreeNode<T> TryShrink(float minSize)
	{
		if (this.Size < 2f * minSize)
		{
			return this;
		}
		if (this.Objects.Count == 0 && (this.Children == null || this.Children.Length == 0))
		{
			return this;
		}
		int num = -1;
		for (int i = 0; i < this.Objects.Count; i++)
		{
			HxOctreeNode<T>.NodeObject nodeObject = this.Objects[i];
			int num2 = this.OctantIndex(nodeObject.Center);
			if (i != 0 && num2 != num)
			{
				return this;
			}
			if (!HxOctreeNode<T>.BoundsContains(this.ChildrenBoundsMin[num2], this.ChildrenBoundsMax[num2], nodeObject.BoundsMin, nodeObject.BoundsMax))
			{
				return this;
			}
			if (num < 0)
			{
				num = num2;
			}
		}
		if (this.Children != null)
		{
			bool flag = false;
			for (int j = 0; j < this.Children.Length; j++)
			{
				if (this.Children[j].HasObjects())
				{
					if (flag)
					{
						return this;
					}
					if (num >= 0 && num != j)
					{
						return this;
					}
					flag = true;
					num = j;
				}
			}
		}
		if (this.Children == null)
		{
			this.Init(this.Size / 2f, this.Overlap, this.MinSize, (this.ChildrenBoundsMin[num] + this.ChildrenBoundsMax[num]) / 2f, this.Parent);
			return this;
		}
		for (int k = 0; k < this.Objects.Count; k++)
		{
			HxOctreeNode<T>.NodeObject node = this.Objects[k];
			this.Children[num].Add(node);
		}
		if (num < 0)
		{
			return this;
		}
		this.Children[num].Parent = this.Parent;
		return this.Children[num];
	}

	// Token: 0x0600005F RID: 95 RVA: 0x000042F9 File Offset: 0x000024F9
	private Vector3 GetVertexP(Vector3 normal)
	{
		return Vector3.zero;
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00004300 File Offset: 0x00002500
	private bool HasObjects()
	{
		if (this.Objects.Count > 0)
		{
			return true;
		}
		if (this.Children != null)
		{
			for (int i = 0; i < 8; i++)
			{
				if (this.Children[i].HasObjects())
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00004344 File Offset: 0x00002544
	public static bool BoundsIntersects(Vector3 aMin, Vector3 aMax, Vector3 bMin, Vector3 bMax)
	{
		return aMax.x >= bMin.x && aMax.y >= bMin.y && aMax.z >= bMin.z && bMax.x >= aMin.x && bMax.y >= aMin.y && bMax.z >= aMin.z;
	}

	// Token: 0x06000062 RID: 98 RVA: 0x000043AC File Offset: 0x000025AC
	public static bool BoundsContains(Vector3 outerMin, Vector3 outerMax, Vector3 innerMin, Vector3 innerMax)
	{
		return outerMin.x <= innerMin.x && outerMin.y <= innerMin.y && outerMin.z <= innerMin.z && (outerMax.x >= innerMax.x && outerMax.y >= innerMax.y) && outerMax.z >= innerMax.z;
	}

	// Token: 0x06000063 RID: 99 RVA: 0x00004414 File Offset: 0x00002614
	private int OctantIndex(Vector3 point)
	{
		return ((point.x <= this.Origin.x) ? 0 : 1) + ((point.z <= this.Origin.z) ? 0 : 2) + ((point.y >= this.Origin.y) ? 0 : 4);
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00004468 File Offset: 0x00002668
	public void Draw(int counter = 0)
	{
		Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.25f);
		for (int i = 0; i < this.Objects.Count; i++)
		{
			Vector3 center = (this.Objects[i].BoundsMax + this.Objects[i].BoundsMin) / 2f;
			Vector3 size = this.Objects[i].BoundsMax - this.Objects[i].BoundsMin;
			Gizmos.DrawCube(center, size);
		}
		Gizmos.color = new Color((float)counter / 5f, 1f, (float)counter / 5f);
		Gizmos.DrawWireCube(this.Origin, Vector3.one * this.SizeWithOverlap);
		if (this.Children != null)
		{
			counter++;
			for (int j = 0; j < 8; j++)
			{
				this.Children[j].Draw(counter);
			}
		}
	}

	// Token: 0x0400002C RID: 44
	public HxOctreeNode<T> Parent;

	// Token: 0x0400002D RID: 45
	private float MinSize;

	// Token: 0x0400002E RID: 46
	private float Overlap;

	// Token: 0x0400002F RID: 47
	private float SizeWithOverlap;

	// Token: 0x04000030 RID: 48
	public Vector3 BoundsMin;

	// Token: 0x04000031 RID: 49
	public Vector3 BoundsMax;

	// Token: 0x04000032 RID: 50
	private readonly List<HxOctreeNode<T>.NodeObject> Objects = new List<HxOctreeNode<T>.NodeObject>();

	// Token: 0x04000033 RID: 51
	private const int MaxObjectCount = 8;

	// Token: 0x04000034 RID: 52
	public HxOctreeNode<T>[] Children;

	// Token: 0x04000035 RID: 53
	private Vector3[] ChildrenBoundsMin;

	// Token: 0x04000036 RID: 54
	private Vector3[] ChildrenBoundsMax;

	// Token: 0x04000037 RID: 55
	public int ID;

	// Token: 0x04000038 RID: 56
	private static int _idCtr;

	// Token: 0x02000310 RID: 784
	[Serializable]
	public class NodeObject
	{
		// Token: 0x060014A1 RID: 5281 RVA: 0x00057E61 File Offset: 0x00056061
		public NodeObject(T value, Vector3 boundsMin, Vector3 boundsMax)
		{
			this.Value = value;
			this.BoundsMin = boundsMin;
			this.BoundsMax = boundsMax;
			this.Center = (this.BoundsMax + this.BoundsMin) / 2f;
		}

		// Token: 0x040014DF RID: 5343
		public HxOctreeNode<T> Node;

		// Token: 0x040014E0 RID: 5344
		public T Value;

		// Token: 0x040014E1 RID: 5345
		public Vector3 BoundsMin;

		// Token: 0x040014E2 RID: 5346
		public Vector3 BoundsMax;

		// Token: 0x040014E3 RID: 5347
		public Vector3 Center;
	}
}

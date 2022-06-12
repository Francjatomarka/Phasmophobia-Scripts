using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HxOctreeNode<T>
{
	// (get) Token: 0x0600004C RID: 76 RVA: 0x00003338 File Offset: 0x00001538
	// (set) Token: 0x0600004D RID: 77 RVA: 0x00003340 File Offset: 0x00001540
	public Vector3 Origin { get; private set; }

	// (get) Token: 0x0600004E RID: 78 RVA: 0x00003349 File Offset: 0x00001549
	// (set) Token: 0x0600004F RID: 79 RVA: 0x00003351 File Offset: 0x00001551
	public float Size { get; private set; }

	public HxOctreeNode(float size, float overlap, float minSize, Vector3 origin, HxOctreeNode<T> parent)
	{
		int idCtr = HxOctreeNode<T>._idCtr;
		HxOctreeNode<T>._idCtr = idCtr + 1;
		this.ID = idCtr;
		this.Init(size, overlap, minSize, origin, parent);
	}

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

	private void DrawBounds(Vector3 min, Vector3 max)
	{
		Debug.DrawLine(min, new Vector3(min.x, min.y, max.z), Color.red);
		Debug.DrawLine(min, new Vector3(min.x, max.y, min.z), Color.red);
		Debug.DrawLine(min, new Vector3(max.x, min.y, min.z), Color.red);
	}

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

	private Vector3 GetVertexP(Vector3 normal)
	{
		return Vector3.zero;
	}

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

	public static bool BoundsIntersects(Vector3 aMin, Vector3 aMax, Vector3 bMin, Vector3 bMax)
	{
		return aMax.x >= bMin.x && aMax.y >= bMin.y && aMax.z >= bMin.z && bMax.x >= aMin.x && bMax.y >= aMin.y && bMax.z >= aMin.z;
	}

	public static bool BoundsContains(Vector3 outerMin, Vector3 outerMax, Vector3 innerMin, Vector3 innerMax)
	{
		return outerMin.x <= innerMin.x && outerMin.y <= innerMin.y && outerMin.z <= innerMin.z && (outerMax.x >= innerMax.x && outerMax.y >= innerMax.y) && outerMax.z >= innerMax.z;
	}

	private int OctantIndex(Vector3 point)
	{
		return ((point.x <= this.Origin.x) ? 0 : 1) + ((point.z <= this.Origin.z) ? 0 : 2) + ((point.y >= this.Origin.y) ? 0 : 4);
	}

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

	public HxOctreeNode<T> Parent;

	private float MinSize;

	private float Overlap;

	private float SizeWithOverlap;

	public Vector3 BoundsMin;

	public Vector3 BoundsMax;

	private readonly List<HxOctreeNode<T>.NodeObject> Objects = new List<HxOctreeNode<T>.NodeObject>();

	private const int MaxObjectCount = 8;

	public HxOctreeNode<T>[] Children;

	private Vector3[] ChildrenBoundsMin;

	private Vector3[] ChildrenBoundsMax;

	public int ID;

	private static int _idCtr;

	[Serializable]
	public class NodeObject
	{
		public NodeObject(T value, Vector3 boundsMin, Vector3 boundsMax)
		{
			this.Value = value;
			this.BoundsMin = boundsMin;
			this.BoundsMax = boundsMax;
			this.Center = (this.BoundsMax + this.BoundsMin) / 2f;
		}

		public HxOctreeNode<T> Node;

		public T Value;

		public Vector3 BoundsMin;

		public Vector3 BoundsMax;

		public Vector3 Center;
	}
}


using System;
using System.Collections.Generic;
using UnityEngine;

public class HxOctree<T>
{
	// (get) Token: 0x0600003F RID: 63 RVA: 0x00002E57 File Offset: 0x00001057
	// (set) Token: 0x06000040 RID: 64 RVA: 0x00002E5F File Offset: 0x0000105F
	public int Count { get; private set; }

	public HxOctree(Vector3 origin = default(Vector3), float initialSize = 10f, float overlap = 0f, float minNodeSize = 1f)
	{
		this.Count = 0;
		this.InitialSize = Mathf.Max(minNodeSize, initialSize);
		this.MinNodeSize = Mathf.Min(minNodeSize, initialSize);
		this.Overlap = Mathf.Clamp(overlap, 0f, 1f);
		this.Root = new HxOctreeNode<T>(this.InitialSize, overlap, this.MinNodeSize, origin, null);
		this.NodeMap = new Dictionary<T, HxOctreeNode<T>.NodeObject>();
	}

	public HxOctreeNode<T>.NodeObject Add(T value, Vector3 boundsMin, Vector3 boundsMax)
	{
		int num = 0;
		while (!HxOctreeNode<T>.BoundsContains(this.Root.BoundsMin, this.Root.BoundsMax, boundsMin, boundsMax))
		{
			this.ExpandRoot((boundsMin + boundsMax) / 2f);
			if (++num > 16)
			{
				Debug.LogError("The octree could not contain the bounds.");
				return null;
			}
		}
		HxOctreeNode<T>.NodeObject nodeObject = new HxOctreeNode<T>.NodeObject(value, boundsMin, boundsMax);
		this.NodeMap[value] = nodeObject;
		this.Root.Add(nodeObject);
		int count = this.Count;
		this.Count = count + 1;
		return nodeObject;
	}

	public void Print()
	{
		Debug.Log("=============================");
		foreach (KeyValuePair<T, HxOctreeNode<T>.NodeObject> keyValuePair in this.NodeMap)
		{
			string text;
			if (keyValuePair.Value.Node.Children == null)
			{
				text = "leaf";
			}
			else
			{
				text = "branch";
			}
			Debug.Log(string.Concat(new object[]
			{
				keyValuePair.Key,
				" is in ",
				keyValuePair.Value.Node.ID,
				", a ",
				text,
				"."
			}));
		}
	}

	public void Move(HxOctreeNode<T>.NodeObject value, Vector3 boundsMin, Vector3 boundsMax)
	{
		if (value == null)
		{
			Debug.Log("null");
		}
		value.BoundsMin = boundsMin;
		value.BoundsMax = boundsMax;
		HxOctreeNode<T> hxOctreeNode = value.Node;
		if (!HxOctreeNode<T>.BoundsContains(hxOctreeNode.BoundsMin, hxOctreeNode.BoundsMax, boundsMin, boundsMax))
		{
			hxOctreeNode.Remove(value.Value);
			int num = 0;
			while (!HxOctreeNode<T>.BoundsContains(hxOctreeNode.BoundsMin, hxOctreeNode.BoundsMax, boundsMin, boundsMax))
			{
				if (hxOctreeNode.Parent != null)
				{
					hxOctreeNode = hxOctreeNode.Parent;
				}
				else
				{
					num++;
					this.ExpandRoot((boundsMin + boundsMax) / 2f);
					hxOctreeNode = this.Root;
					if (num > 16)
					{
						Debug.LogError("The octree could not contain the bounds.");
						return;
					}
				}
			}
			hxOctreeNode.Add(value);
		}
	}

	public void Move(T value, Vector3 boundsMin, Vector3 boundsMax)
	{
		HxOctreeNode<T>.NodeObject value2;
		if (this.NodeMap.TryGetValue(value, out value2))
		{
			this.Move(value2, boundsMin, boundsMax);
		}
	}

	public void TryShrink()
	{
		this.Root = this.Root.TryShrink(this.InitialSize);
	}

	public bool Remove(T value)
	{
		if (this.Root.Remove(value))
		{
			this.NodeMap.Remove(value);
			int count = this.Count;
			this.Count = count - 1;
			this.Root = this.Root.TryShrink(this.InitialSize);
			return true;
		}
		return false;
	}

	private void ExpandRoot(Vector3 center)
	{
		Vector3 vector = this.Root.Origin - center;
		int num = (vector.x < 0f) ? -1 : 1;
		int num2 = (vector.y < 0f) ? -1 : 1;
		int num3 = (vector.z < 0f) ? -1 : 1;
		HxOctreeNode<T> root = this.Root;
		float d = this.Root.Size / 2f;
		Vector3 vector2 = this.Root.Origin - new Vector3((float)num, (float)num2, (float)num3) * d;
		this.Root = new HxOctreeNode<T>(this.Root.Size * 2f, this.Overlap, this.MinNodeSize, vector2, null);
		root.Parent = this.Root;
		int num4 = 0;
		if (num > 0)
		{
			num4++;
		}
		if (num3 > 0)
		{
			num4 += 2;
		}
		if (num2 < 0)
		{
			num4 += 4;
		}
		HxOctreeNode<T>[] array = new HxOctreeNode<T>[8];
		for (int i = 0; i < 8; i++)
		{
			if (i == num4)
			{
				array[i] = root;
			}
			else
			{
				num = ((i % 2 == 0) ? -1 : 1);
				num2 = ((i > 3) ? -1 : 1);
				num3 = ((i < 2 || (i > 3 && i < 6)) ? -1 : 1);
				array[i] = new HxOctreeNode<T>(root.Size, this.Overlap, this.MinNodeSize, vector2 + new Vector3((float)num, (float)num2, (float)num3) * d, this.Root);
			}
		}
		this.Root.Children = array;
	}

	public void GetObjects(Vector3 boundsMin, Vector3 boundsMax, List<T> items)
	{
		this.Root.GetObjects2(boundsMin, boundsMax, items);
	}

	public void GetObjectsBoundsPlane(ref Plane[] planes, Vector3 min, Vector3 max, List<T> items)
	{
		this.Root.GetObjects2BoundsPlane(ref planes, min, max, items);
	}

	public void Draw()
	{
		this.Root.Draw(0);
	}

	private HxOctreeNode<T> Root;

	private float Overlap;

	private float InitialSize;

	private float MinNodeSize;

	private Dictionary<T, HxOctreeNode<T>.NodeObject> NodeMap;
}


using System;
using UnityEngine;

// Token: 0x02000006 RID: 6
[ExecuteInEditMode]
public class HxDensityVolume : MonoBehaviour
{
	// Token: 0x06000034 RID: 52 RVA: 0x00002AF0 File Offset: 0x00000CF0
	private void OnEnable()
	{
		this.CalculateBounds();
		if (HxDensityVolume.DensityOctree == null)
		{
			HxDensityVolume.DensityOctree = new HxOctree<HxDensityVolume>(default(Vector3), 10f, 0f, 1f);
		}
		HxVolumetricCamera.AllDensityVolumes.Add(this);
		this.octreeNode = HxDensityVolume.DensityOctree.Add(this, this.minBounds, this.maxBounds);
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00002B55 File Offset: 0x00000D55
	private void OnDrawGizmos()
	{
		Gizmos.DrawIcon(base.transform.position, "AreaLight Gizmo", true);
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00002B6D File Offset: 0x00000D6D
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = HxDensityVolume.gizmoColor;
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00002B98 File Offset: 0x00000D98
	private void OnDisable()
	{
		HxVolumetricCamera.AllDensityVolumes.Remove(this);
		if (HxDensityVolume.DensityOctree != null)
		{
			HxDensityVolume.DensityOctree.Remove(this);
			HxDensityVolume.DensityOctree = null;
		}
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00002B98 File Offset: 0x00000D98
	private void OnDestroy()
	{
		HxVolumetricCamera.AllDensityVolumes.Remove(this);
		if (HxDensityVolume.DensityOctree != null)
		{
			HxDensityVolume.DensityOctree.Remove(this);
			HxDensityVolume.DensityOctree = null;
		}
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00002BBF File Offset: 0x00000DBF
	public void UpdateVolume()
	{
		if (base.transform.hasChanged)
		{
			this.CalculateBounds();
			HxDensityVolume.DensityOctree.Move(this.octreeNode, this.minBounds, this.maxBounds);
			base.transform.hasChanged = false;
		}
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00002BFC File Offset: 0x00000DFC
	private void CalculateBounds()
	{
		Vector3 lhs = base.transform.TransformPoint(HxDensityVolume.c1);
		Vector3 lhs2 = base.transform.TransformPoint(HxDensityVolume.c2);
		Vector3 lhs3 = base.transform.TransformPoint(HxDensityVolume.c3);
		Vector3 lhs4 = base.transform.TransformPoint(HxDensityVolume.c4);
		Vector3 lhs5 = base.transform.TransformPoint(HxDensityVolume.c5);
		Vector3 lhs6 = base.transform.TransformPoint(HxDensityVolume.c6);
		Vector3 lhs7 = base.transform.TransformPoint(HxDensityVolume.c7);
		Vector3 rhs = base.transform.TransformPoint(HxDensityVolume.c8);
		this.minBounds = Vector3.Min(lhs, Vector3.Min(lhs2, Vector3.Min(lhs3, Vector3.Min(lhs4, Vector3.Min(lhs5, Vector3.Min(lhs6, Vector3.Min(lhs7, rhs)))))));
		this.maxBounds = Vector3.Max(lhs, Vector3.Max(lhs2, Vector3.Max(lhs3, Vector3.Max(lhs4, Vector3.Max(lhs5, Vector3.Max(lhs6, Vector3.Max(lhs7, rhs)))))));
		this.ToLocalSpace = base.transform.worldToLocalMatrix;
	}

	// Token: 0x0400000D RID: 13
	public static HxOctree<HxDensityVolume> DensityOctree;

	// Token: 0x0400000E RID: 14
	private HxOctreeNode<HxDensityVolume>.NodeObject octreeNode;

	// Token: 0x0400000F RID: 15
	public HxDensityVolume.DensityShape Shape;

	// Token: 0x04000010 RID: 16
	public HxDensityVolume.DensityBlendMode BlendMode = HxDensityVolume.DensityBlendMode.Add;

	// Token: 0x04000011 RID: 17
	[HideInInspector]
	public Vector3 minBounds;

	// Token: 0x04000012 RID: 18
	[HideInInspector]
	public Vector3 maxBounds;

	// Token: 0x04000013 RID: 19
	[HideInInspector]
	public Matrix4x4 ToLocalSpace;

	// Token: 0x04000014 RID: 20
	public float Density = 0.1f;

	// Token: 0x04000015 RID: 21
	private static Color gizmoColor = new Color(0.992f, 0.749f, 0.592f);

	// Token: 0x04000016 RID: 22
	private static Vector3 c1 = new Vector3(0.5f, 0.5f, 0.5f);

	// Token: 0x04000017 RID: 23
	private static Vector3 c2 = new Vector3(-0.5f, 0.5f, 0.5f);

	// Token: 0x04000018 RID: 24
	private static Vector3 c3 = new Vector3(0.5f, 0.5f, -0.5f);

	// Token: 0x04000019 RID: 25
	private static Vector3 c4 = new Vector3(-0.5f, 0.5f, -0.5f);

	// Token: 0x0400001A RID: 26
	private static Vector3 c5 = new Vector3(0.5f, -0.5f, 0.5f);

	// Token: 0x0400001B RID: 27
	private static Vector3 c6 = new Vector3(-0.5f, -0.5f, 0.5f);

	// Token: 0x0400001C RID: 28
	private static Vector3 c7 = new Vector3(0.5f, -0.5f, -0.5f);

	// Token: 0x0400001D RID: 29
	private static Vector3 c8 = new Vector3(-0.5f, -0.5f, -0.5f);

	// Token: 0x0200030E RID: 782
	public enum DensityShape
	{
		// Token: 0x040014D7 RID: 5335
		Square,
		// Token: 0x040014D8 RID: 5336
		Sphere,
		// Token: 0x040014D9 RID: 5337
		Cylinder
	}

	// Token: 0x0200030F RID: 783
	public enum DensityBlendMode
	{
		// Token: 0x040014DB RID: 5339
		Max,
		// Token: 0x040014DC RID: 5340
		Add,
		// Token: 0x040014DD RID: 5341
		Min,
		// Token: 0x040014DE RID: 5342
		Sub
	}
}

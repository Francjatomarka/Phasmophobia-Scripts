using System;
using UnityEngine;

[ExecuteInEditMode]
public class HxDensityVolume : MonoBehaviour
{
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

	private void OnDrawGizmos()
	{
		Gizmos.DrawIcon(base.transform.position, "AreaLight Gizmo", true);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = HxDensityVolume.gizmoColor;
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
	}

	private void OnDisable()
	{
		HxVolumetricCamera.AllDensityVolumes.Remove(this);
		if (HxDensityVolume.DensityOctree != null)
		{
			HxDensityVolume.DensityOctree.Remove(this);
			HxDensityVolume.DensityOctree = null;
		}
	}

	private void OnDestroy()
	{
		HxVolumetricCamera.AllDensityVolumes.Remove(this);
		if (HxDensityVolume.DensityOctree != null)
		{
			HxDensityVolume.DensityOctree.Remove(this);
			HxDensityVolume.DensityOctree = null;
		}
	}

	public void UpdateVolume()
	{
		if (base.transform.hasChanged)
		{
			this.CalculateBounds();
			HxDensityVolume.DensityOctree.Move(this.octreeNode, this.minBounds, this.maxBounds);
			base.transform.hasChanged = false;
		}
	}

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

	public static HxOctree<HxDensityVolume> DensityOctree;

	private HxOctreeNode<HxDensityVolume>.NodeObject octreeNode;

	public HxDensityVolume.DensityShape Shape;

	public HxDensityVolume.DensityBlendMode BlendMode = HxDensityVolume.DensityBlendMode.Add;

	[HideInInspector]
	public Vector3 minBounds;

	[HideInInspector]
	public Vector3 maxBounds;

	[HideInInspector]
	public Matrix4x4 ToLocalSpace;

	public float Density = 0.1f;

	private static Color gizmoColor = new Color(0.992f, 0.749f, 0.592f);

	private static Vector3 c1 = new Vector3(0.5f, 0.5f, 0.5f);

	private static Vector3 c2 = new Vector3(-0.5f, 0.5f, 0.5f);

	private static Vector3 c3 = new Vector3(0.5f, 0.5f, -0.5f);

	private static Vector3 c4 = new Vector3(-0.5f, 0.5f, -0.5f);

	private static Vector3 c5 = new Vector3(0.5f, -0.5f, 0.5f);

	private static Vector3 c6 = new Vector3(-0.5f, -0.5f, 0.5f);

	private static Vector3 c7 = new Vector3(0.5f, -0.5f, -0.5f);

	private static Vector3 c8 = new Vector3(-0.5f, -0.5f, -0.5f);

	public enum DensityShape
	{
		Square,
		Sphere,
		Cylinder
	}

	public enum DensityBlendMode
	{
		Max,
		Add,
		Min,
		Sub
	}
}


using System;
using UnityEngine;

[ExecuteInEditMode]
public class ListMeshVertCount : MonoBehaviour
{
	private void Update()
	{
		if (this.listVertCount)
		{
			this.listVertCount = false;
			this.ListVertCount();
		}
	}

	private void ListVertCount()
	{
		MeshFilter[] componentsInChildren = base.GetComponentsInChildren<MeshFilter>(this.includeInActive);
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Mesh sharedMesh = componentsInChildren[i].sharedMesh;
			if (!(sharedMesh == null))
			{
				num += sharedMesh.vertexCount;
				num2 += sharedMesh.triangles.Length;
			}
		}
		Debug.Log(string.Concat(new object[]
		{
			base.gameObject.name,
			" Vertices ",
			num,
			"  Triangles ",
			num2
		}));
	}

	public bool includeInActive;

	public bool listVertCount;
}


using System;
using UnityEngine;

// Token: 0x02000003 RID: 3
[ExecuteInEditMode]
public class ListMeshVertCount : MonoBehaviour
{
	// Token: 0x06000011 RID: 17 RVA: 0x00002349 File Offset: 0x00000549
	private void Update()
	{
		if (this.listVertCount)
		{
			this.listVertCount = false;
			this.ListVertCount();
		}
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00002360 File Offset: 0x00000560
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

	// Token: 0x04000017 RID: 23
	public bool includeInActive;

	// Token: 0x04000018 RID: 24
	public bool listVertCount;
}

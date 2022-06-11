using System;
using UnityEngine;

// Token: 0x02000004 RID: 4
public class VisualizeMesh : MonoBehaviour
{
	// Token: 0x06000014 RID: 20 RVA: 0x000023EC File Offset: 0x000005EC
	private void OnDrawGizmosSelected()
	{
		if (!this.mf)
		{
			this.mf = base.GetComponent<MeshFilter>();
		}
		if (!this.mf)
		{
			return;
		}
		if (!this.m)
		{
			this.m = this.mf.sharedMesh;
		}
		if (!this.m)
		{
			return;
		}
		Vector3[] vertices = this.m.vertices;
		Vector3[] normals = this.m.normals;
		Vector4[] tangents = this.m.tangents;
		Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
		Matrix4x4 transpose = localToWorldMatrix.inverse.transpose;
		for (int i = 0; i < vertices.Length; i++)
		{
			Gizmos.color = Color.green;
			Vector3 vector = localToWorldMatrix.MultiplyPoint3x4(vertices[i]);
			Gizmos.DrawSphere(vector, this.sphereRadius);
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(vector, vector + transpose.MultiplyVector(normals[i]) * 0.5f);
			Gizmos.color = Color.red;
			Gizmos.DrawLine(vector, vector + localToWorldMatrix.MultiplyVector(new Vector3(tangents[i].x, tangents[i].y, tangents[i].z)) * 0.5f);
		}
	}

	// Token: 0x04000018 RID: 24
	public float sphereRadius = 0.05f;

	// Token: 0x04000019 RID: 25
	private MeshFilter mf;

	// Token: 0x0400001A RID: 26
	private Mesh m;
}

using System;
using UnityEngine;

public class VisualizeMesh : MonoBehaviour
{
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

	public float sphereRadius = 0.05f;

	private MeshFilter mf;

	private Mesh m;
}


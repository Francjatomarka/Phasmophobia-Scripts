using System;
using UnityEngine;

[ExecuteInEditMode]
public class EnableChildrenMeshRenderers : MonoBehaviour
{
	private void Update()
	{
		if (this.execute)
		{
			this.execute = false;
			this.Execute();
		}
	}

	private void Execute()
	{
		MeshRenderer[] componentsInChildren = base.GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = true;
		}
	}

	public bool execute;
}


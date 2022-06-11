using System;
using UnityEngine;

// Token: 0x02000005 RID: 5
[ExecuteInEditMode]
public class EnableChildrenMeshRenderers : MonoBehaviour
{
	// Token: 0x06000016 RID: 22 RVA: 0x0000255F File Offset: 0x0000075F
	private void Update()
	{
		if (this.execute)
		{
			this.execute = false;
			this.Execute();
		}
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002578 File Offset: 0x00000778
	private void Execute()
	{
		MeshRenderer[] componentsInChildren = base.GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = true;
		}
	}

	// Token: 0x0400001B RID: 27
	public bool execute;
}

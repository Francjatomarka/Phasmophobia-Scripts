using System;
using UnityEngine;

// Token: 0x02000054 RID: 84
[RequireComponent(typeof(Camera))]
public class MoveCam : MonoBehaviour
{
	// Token: 0x060001C9 RID: 457 RVA: 0x0000C6B4 File Offset: 0x0000A8B4
	private void Start()
	{
		this.camTransform = base.GetComponent<Camera>().transform;
		this.originalPos = this.camTransform.position;
		this.randomPos = this.originalPos + new Vector3((float)UnityEngine.Random.Range(-2, 2), (float)UnityEngine.Random.Range(-2, 2), (float)UnityEngine.Random.Range(-1, 1));
	}

	// Token: 0x060001CA RID: 458 RVA: 0x0000C714 File Offset: 0x0000A914
	private void Update()
	{
		this.camTransform.position = Vector3.Slerp(this.camTransform.position, this.randomPos, Time.deltaTime);
		this.camTransform.LookAt(this.lookAt);
		if (Vector3.Distance(this.camTransform.position, this.randomPos) < 0.5f)
		{
			this.randomPos = this.originalPos + new Vector3((float)UnityEngine.Random.Range(-2, 2), (float)UnityEngine.Random.Range(-2, 2), (float)UnityEngine.Random.Range(-1, 1));
		}
	}

	// Token: 0x040001F0 RID: 496
	private Vector3 originalPos;

	// Token: 0x040001F1 RID: 497
	private Vector3 randomPos;

	// Token: 0x040001F2 RID: 498
	private Transform camTransform;

	// Token: 0x040001F3 RID: 499
	public Transform lookAt;
}

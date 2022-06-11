using System;
using UnityEngine;

// Token: 0x02000031 RID: 49
public class ArcRotation : MonoBehaviour
{
	// Token: 0x0600012A RID: 298 RVA: 0x000094E0 File Offset: 0x000076E0
	private void Start()
	{
		this.initialRotation = base.transform.rotation.eulerAngles;
		this.RotTarget = base.transform.rotation.eulerAngles;
	}

	// Token: 0x0600012B RID: 299 RVA: 0x00009520 File Offset: 0x00007720
	private void Update()
	{
		this.dist = Vector3.Distance(base.transform.rotation.eulerAngles, this.RotTarget);
		if (this.dist - (float)((int)(this.dist / 360f) * 360) > this.Speed * Time.deltaTime * 5f)
		{
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.Euler(this.RotTarget), this.Speed * Time.deltaTime);
			return;
		}
		if (this.add)
		{
			this.add = false;
			this.RotTarget = this.initialRotation + this.Axis * this.MinAngle;
			return;
		}
		this.add = true;
		this.RotTarget = this.initialRotation + this.Axis * this.MaxAngle;
	}

	// Token: 0x04000174 RID: 372
	public Vector3 Axis = Vector3.up;

	// Token: 0x04000175 RID: 373
	private Vector3 initialRotation;

	// Token: 0x04000176 RID: 374
	private Vector3 RotTarget;

	// Token: 0x04000177 RID: 375
	public float MinAngle = -45f;

	// Token: 0x04000178 RID: 376
	public float MaxAngle = 45f;

	// Token: 0x04000179 RID: 377
	public float Speed = 5f;

	// Token: 0x0400017A RID: 378
	private float dist;

	// Token: 0x0400017B RID: 379
	public bool add = true;
}

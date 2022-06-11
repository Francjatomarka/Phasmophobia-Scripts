using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200002A RID: 42
public class BugTarget : MonoBehaviour
{
	// Token: 0x06000109 RID: 265 RVA: 0x00008918 File Offset: 0x00006B18
	private void Awake()
	{
		this.TargetInterval = UnityEngine.Random.Range(this.TargetIntervalRange.x, this.TargetIntervalRange.y);
	}

	// Token: 0x0600010A RID: 266 RVA: 0x0000893B File Offset: 0x00006B3B
	private void Start()
	{
		base.StartCoroutine(this.RandomTargetLocation());
	}

	// Token: 0x0600010B RID: 267 RVA: 0x0000894A File Offset: 0x00006B4A
	private void Update()
	{
		this.placenewtarget();
	}

	// Token: 0x0600010C RID: 268 RVA: 0x00008952 File Offset: 0x00006B52
	private void placenewtarget()
	{
		base.transform.localPosition = new Vector3(this.x, 0f, this.z);
	}

	// Token: 0x0600010D RID: 269 RVA: 0x00008975 File Offset: 0x00006B75
	private IEnumerator RandomTargetLocation()
	{
		for (;;)
		{
			this.x = UnityEngine.Random.Range(this.xRegionSize.x, this.xRegionSize.y);
			this.z = UnityEngine.Random.Range(this.yRegionSize.x, this.yRegionSize.y);
			yield return new WaitForSeconds(this.TargetInterval);
		}
		yield break;
	}

	// Token: 0x04000138 RID: 312
	public Vector2 TargetIntervalRange = new Vector2(0.1f, 0.2f);

	// Token: 0x04000139 RID: 313
	public float smoothing = 1f;

	// Token: 0x0400013A RID: 314
	public float speed;

	// Token: 0x0400013B RID: 315
	private Vector3 targetpos;

	// Token: 0x0400013C RID: 316
	private float x;

	// Token: 0x0400013D RID: 317
	private float z;

	// Token: 0x0400013E RID: 318
	private float TargetInterval;

	// Token: 0x0400013F RID: 319
	public Vector2 xRegionSize = new Vector2(-1f, 1f);

	// Token: 0x04000140 RID: 320
	public Vector2 yRegionSize = new Vector2(-1f, 1f);
}

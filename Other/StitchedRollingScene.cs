using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000035 RID: 53
public class StitchedRollingScene : MonoBehaviour
{
	// Token: 0x06000133 RID: 307 RVA: 0x00009674 File Offset: 0x00007874
	private void Awake()
	{
		for (int i = 0; i < this.NumberOfSegments; i++)
		{
			Stitchable component = UnityEngine.Object.Instantiate<GameObject>(this.SegmentPrefab, base.transform.position, Quaternion.identity).GetComponent<Stitchable>();
			this.Segments.Add(component);
			if (i > 0)
			{
				component.transform.position = this.Segments[i - 1].StitchPoint.position;
			}
		}
	}

	// Token: 0x06000134 RID: 308 RVA: 0x000096E8 File Offset: 0x000078E8
	private void FixedUpdate()
	{
		for (int i = 0; i < this.Segments.Count; i++)
		{
			this.Segments[i].transform.position += Vector3.forward * this.speed * Time.deltaTime;
			if (this.Segments[i].StitchPoint.position.z >= base.transform.position.z)
			{
				int num = i + (this.Segments.Count - 1);
				if (num >= this.Segments.Count)
				{
					num -= this.Segments.Count;
				}
				this.Segments[i].transform.position = this.Segments[num].StitchPoint.position;
			}
		}
	}

	// Token: 0x04000185 RID: 389
	public GameObject SegmentPrefab;

	// Token: 0x04000186 RID: 390
	public int NumberOfSegments = 4;

	// Token: 0x04000187 RID: 391
	public List<Stitchable> Segments;

	// Token: 0x04000188 RID: 392
	public float speed = 2f;
}

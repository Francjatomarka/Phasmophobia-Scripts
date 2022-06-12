using System;
using System.Collections.Generic;
using UnityEngine;

public class StitchedRollingScene : MonoBehaviour
{
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

	public GameObject SegmentPrefab;

	public int NumberOfSegments = 4;

	public List<Stitchable> Segments;

	public float speed = 2f;
}


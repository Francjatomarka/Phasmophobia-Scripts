using System;
using System.Collections;
using UnityEngine;

public class BugTarget : MonoBehaviour
{
	private void Awake()
	{
		this.TargetInterval = UnityEngine.Random.Range(this.TargetIntervalRange.x, this.TargetIntervalRange.y);
	}

	private void Start()
	{
		base.StartCoroutine(this.RandomTargetLocation());
	}

	private void Update()
	{
		this.placenewtarget();
	}

	private void placenewtarget()
	{
		base.transform.localPosition = new Vector3(this.x, 0f, this.z);
	}

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

	public Vector2 TargetIntervalRange = new Vector2(0.1f, 0.2f);

	public float smoothing = 1f;

	public float speed;

	private Vector3 targetpos;

	private float x;

	private float z;

	private float TargetInterval;

	public Vector2 xRegionSize = new Vector2(-1f, 1f);

	public Vector2 yRegionSize = new Vector2(-1f, 1f);
}


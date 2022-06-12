using System;
using UnityEngine;

public class ArcRotation : MonoBehaviour
{
	private void Start()
	{
		this.initialRotation = base.transform.rotation.eulerAngles;
		this.RotTarget = base.transform.rotation.eulerAngles;
	}

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

	public Vector3 Axis = Vector3.up;

	private Vector3 initialRotation;

	private Vector3 RotTarget;

	public float MinAngle = -45f;

	public float MaxAngle = 45f;

	public float Speed = 5f;

	private float dist;

	public bool add = true;
}


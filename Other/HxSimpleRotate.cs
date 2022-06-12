using System;
using UnityEngine;

public class HxSimpleRotate : MonoBehaviour
{
	private void Update()
	{
		base.transform.Rotate(this.RotateSpeed * Time.deltaTime, Space.Self);
	}

	public Vector3 RotateSpeed;
}


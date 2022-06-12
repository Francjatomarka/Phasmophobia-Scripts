using System;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
	private void Start()
	{
		this.tr = base.GetComponent<Transform>();
	}

	private void Update()
	{
		this.tr.Rotate(this.rotation * Time.deltaTime);
	}

	private Transform tr;

	public Vector3 rotation;
}


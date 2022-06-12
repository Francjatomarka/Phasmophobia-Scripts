using System;
using UnityEngine;

public class PCLook : MonoBehaviour
{
	private void LateUpdate()
	{
		this.spineBoneTransform.rotation = this.cam.transform.rotation;
	}

	[SerializeField]
	private Transform spineBoneTransform;

	[SerializeField]
	private Camera cam;
}


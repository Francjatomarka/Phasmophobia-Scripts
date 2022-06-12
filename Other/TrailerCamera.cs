using System;
using UnityEngine;

public class TrailerCamera : MonoBehaviour
{
	private void Awake()
	{
		base.transform.SetParent(null);
		if (this.cam == null)
		{
			base.enabled = false;
		}
	}

	private void OnEnable()
	{
		base.transform.position = this.cam.transform.position;
		base.transform.rotation = this.cam.transform.rotation;
	}

	private void Update()
	{
		if (this.cam != null)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, this.cam.transform.position, Time.deltaTime * this.posSpeed);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.cam.transform.rotation, Time.deltaTime * this.rotSpeed);
		}
	}

	[SerializeField]
	private Camera cam;

	[SerializeField]
	private float posSpeed = 5f;

	[SerializeField]
	private float rotSpeed = 5f;
}


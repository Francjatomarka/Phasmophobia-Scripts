using System;
using UnityEngine;

public class ViewDrag : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			this.hit_position = Input.mousePosition;
			this.camera_position = base.transform.position;
		}
		if (Input.GetMouseButton(0))
		{
			this.current_position = Input.mousePosition;
			this.LeftMouseDrag();
		}
	}

	private void LeftMouseDrag()
	{
		this.current_position.z = (this.hit_position.z = this.camera_position.y);
		Vector3 vector = Camera.main.ScreenToWorldPoint(this.current_position) - Camera.main.ScreenToWorldPoint(this.hit_position);
		if (this.invert)
		{
			vector *= -1f;
		}
		if (this.yBecomesZ)
		{
			vector.z = vector.y;
			vector.y = 0f;
		}
		Vector3 position = this.camera_position + vector * this.speed;
		base.transform.position = position;
	}

	private Vector3 hit_position = Vector3.zero;

	private Vector3 current_position = Vector3.zero;

	private Vector3 camera_position = Vector3.zero;

	public bool invert;

	public bool yBecomesZ = true;

	public float speed = 2f;
}


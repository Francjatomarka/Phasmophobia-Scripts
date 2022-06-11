using System;
using UnityEngine;

// Token: 0x02000014 RID: 20
public class ViewDrag : MonoBehaviour
{
	// Token: 0x0600009D RID: 157 RVA: 0x00003D4C File Offset: 0x00001F4C
	private void Start()
	{
	}

	// Token: 0x0600009E RID: 158 RVA: 0x00004E48 File Offset: 0x00003048
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

	// Token: 0x0600009F RID: 159 RVA: 0x00004E88 File Offset: 0x00003088
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

	// Token: 0x0400006B RID: 107
	private Vector3 hit_position = Vector3.zero;

	// Token: 0x0400006C RID: 108
	private Vector3 current_position = Vector3.zero;

	// Token: 0x0400006D RID: 109
	private Vector3 camera_position = Vector3.zero;

	// Token: 0x0400006E RID: 110
	public bool invert;

	// Token: 0x0400006F RID: 111
	public bool yBecomesZ = true;

	// Token: 0x04000070 RID: 112
	public float speed = 2f;
}
